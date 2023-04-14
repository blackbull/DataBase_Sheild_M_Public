using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using Report_Sheild_M_Interfaces;
using System.Xml.Serialization;

namespace ResultTypesClassLibrary
{

    /// <summary>
    /// Диаграмма суммы, разности, Усреднённая
    /// </summary>
    [Serializable]
    public class ResultTypeClassUnion : ResultType_MAINClass, Report_Sheild_M_Interfaces.IUnionDiagramDirection, IResultType_Union
    {
        public ResultTypeClassUnion() { }

        public ResultTypeClassUnion(bool FillMeasurementData)
            :base(FillMeasurementData)
        {
        }

         [XmlIgnore]
        protected IAmplitudeDiagramDirection Result_for_Report_AmplitudeDiagram = null;

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IAmplitudeDiagramDirection GetSelectedResult_for_Report_AmplitudeDiagram()
        {
            if (Result_for_Report_AmplitudeDiagram == null)
            {
                Result_for_Report_AmplitudeDiagram = new ResultTypeClassДН_for_Report_AmplitudeDiagram(this, 0, this.SelectedPolarization.SelectedFrequencyIndex);
            }

            return Result_for_Report_AmplitudeDiagram;
        }

        /// <summary>
        /// массив исходных результатов измерения
        /// </summary>
        [XmlIgnore] //если включать исходные рузультаты в XML то не работает Экспорт
        public List<ResultType_MAINClass> InitialResults = new List<ResultType_MAINClass>();

        /// <summary>
        /// массив исходных результатов измерения
        /// </summary>
        //public IList<ResultType_MAINClass> InitialResults
        //{
        //    get
        //    {
        //        return _InitialResults.ToList();
        //    }
        //}

        public void AddToInitialResults(ResultType_MAINClass newResult)
        {
            if (newResult is ResultTypeClassДН || newResult is ResultTypeClassСДНМ || newResult is ResultTypeClassUnion)
            {
                //устанавливаем тип измерения
                if (InitialResults.Count == 0)
                {
                    if (newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                    {
                        this.MainOptions.MeasurementResultType = MeasurementTypeEnum.ДН_Азимут;
                    }
                    else
                    {
                        if (newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                        {
                            this.MainOptions.MeasurementResultType = MeasurementTypeEnum.ДН_Меридиан;
                        }
                    }
                }

                else
                {
                    bool needError = false;

                    //проверяем на соответствие типов измерений
                    if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || this.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                    {
                        if (newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                        {
                            needError = true;
                        }
                    }
                    else
                    {
                        if (newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || newResult.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                        {
                            needError = true;
                        }
                    }


                    if (needError)
                    {
                        throw new FormatException(string.Format("Нельзя рассчитывать измерения в разных поляризациях: {0} и {1}", this.MainOptions.MeasurementResultType.ToString(), newResult.MainOptions.MeasurementResultType.ToString()));
                    }
                }

                
                //проверки прошли успешно - добавляем результат к рассчёту
                InitialResults.Add(newResult);
            }
            else
            {
                throw new FormatException("Невозможно производить рассчёт с ResultTypeClassКУ, ResultTypeПХ и другими не зарегистрированными типами измерений");
            }
        }

        /// <summary>
        /// был ли произведён рассчёт данных
        /// false - рассчёт произведён
        /// </summary>
        protected bool IsNotCalculation = true;

        /// <summary>
        /// блокировка автоматического перерассчёта данных
        /// true - заблокировано
        /// </summary>
        public bool UserSetCalculationData = false;

        protected virtual List<PolarizationElementClass> CalculateMeasurement(CalculationResultTypeEnum whatDoing)
        {
            if (this.InitialResults.Count < 2)
            {
                this.PolarizationElements[0] = new PolarizationElementClass();
            }
            else
            {
                List<FrequencyElementClass> tempFreq = this.GetListFrequencyElements(this.InitialResults);


                #region Проверяем полученные массивы на соответствие и делаем интерполяцию

                #region ищем минимальный диапазон координат для последующего рассчёта

                decimal Start = decimal.MinValue;
                decimal Stop = decimal.MaxValue;
                decimal Step = decimal.MinValue;

                decimal tempStart=0;
                decimal tempStop=0;
                decimal tempStep=0;

                for (int i = 0; i < InitialResults.Count; i++)
                {
                    tempStep = InitialResults[i].MainOptions.Parameters.StepMeasurement;

                    if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
                    {
                        tempStart = InitialResults[i].MainOptions.Parameters.StartOPU_W;
                        tempStop = InitialResults[i].MainOptions.Parameters.StopOPU_W;
                    }
                    if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан)
                    {
                        tempStart = InitialResults[i].MainOptions.Parameters.StartOPU_Y;
                        tempStop = InitialResults[i].MainOptions.Parameters.StopOPU_Y;
                    }

                    //ищем самый большлй шаг
                    if (Step < tempStep)
                    {
                        if (tempStep == 0)
                        {
                            //шаг измерения не должен быть нулём
                            tempStep = 0.1m;
                        }

                        Step = tempStep;
                    }


                    //если старт меньше стоп то сравниваем со стартовой
                    if (tempStart < tempStop)
                    {
                        if (Start < tempStart)
                        {
                            Start = tempStart;
                        }

                        if (Stop > tempStop)
                        {
                            Stop = tempStop;
                        }
                    }
                    else
                    {
                        //если нет то сравниваем со стоп
                        if (Start < tempStop)
                        {
                            Start = tempStop;
                        }

                        if (Stop > tempStart)
                        {
                            Stop = tempStart;
                        }
                    }
                }

                this.MainOptions.Parameters.StepMeasurement = Step;
                if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
                {
                    this.MainOptions.Parameters.StartOPU_W = Start;
                    this.MainOptions.Parameters.StopOPU_W = Stop;
                }
                if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан)
                {
                    this.MainOptions.Parameters.StartOPU_Y = Start;
                    this.MainOptions.Parameters.StopOPU_Y = Stop;
                }

                #endregion

                List<FrequencyElementClass> tempInterpolFreq = new List<FrequencyElementClass>();

                foreach (FrequencyElementClass freq in tempFreq)
                {
                    tempInterpolFreq.Add(GetInterpolationFreq(freq, this.MainOptions));
                }

                #endregion

                FrequencyElementClass retfreq = new FrequencyElementClass();

                switch (whatDoing)
                {
                    case CalculationResultTypeEnum.Difference:
                        {
                            retfreq = FrequencyElementClass.RaznFrequency(tempInterpolFreq);
                            break;
                        }
                    case CalculationResultTypeEnum.Averaged:
                        {
                            retfreq = FrequencyElementClass.SrednFrequency(tempInterpolFreq);
                            break;
                        }
                    case CalculationResultTypeEnum.Sum_by_RAZ:
                        {
                            retfreq = FrequencyElementClass.SumFrequency(tempInterpolFreq);
                            break;
                        }
                    case CalculationResultTypeEnum.Sum_by_DB:
                        {
                            retfreq = FrequencyElementClass.CalculateFrequency(FrequencyElementClass.SumRaznFreqEnum.Sum_in_DB, tempInterpolFreq);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Не определён тип рассчёта, " + this.GetType().ToString());
                        }
                }

                #region Устанавливаем тип измерения и другие опции рассчёта

                //this.MainOptions.Parameters.MeasurementResultType = _InitialResults[0].MainOptions.Parameters.MeasurementResultType;

                //this.MainOptions.Parameters.StartOPU_W = _InitialResults[0].MainOptions.Parameters.StartOPU_W;
                //this.MainOptions.Parameters.StartOPU_Y = _InitialResults[0].MainOptions.Parameters.StartOPU_Y;
                //this.MainOptions.Parameters.StartTower_W = _InitialResults[0].MainOptions.Parameters.StartTower_W;
                //this.MainOptions.Parameters.StartTower_Y = _InitialResults[0].MainOptions.Parameters.StartTower_Y;

                //this.MainOptions.Parameters.StopOPU_W = _InitialResults[0].MainOptions.Parameters.StopOPU_W;
                //this.MainOptions.Parameters.StopOPU_Y = _InitialResults[0].MainOptions.Parameters.StopOPU_Y;
                //this.MainOptions.Parameters.StopTower_W = _InitialResults[0].MainOptions.Parameters.StopTower_W;
                //this.MainOptions.Parameters.StopTower_Y = _InitialResults[0].MainOptions.Parameters.StopTower_Y;

                //this.MainOptions.Parameters.StepMeasurement = _InitialResults[0].MainOptions.Parameters.StepMeasurement;

                #endregion

                this.PolarizationElements[0] = new PolarizationElementClass();
                this.PolarizationElements[0].SelectedFrequency = retfreq;

                CalculationResultsClass.CalculateAll_NO_Constant(this.PolarizationElements[0].SelectedFrequency);

                this.IsNotCalculation = false;
            }

            return this.PolarizationElements;
        }

        /// <summary>
        /// Получить массив частот, проверить и интерполировать их
        /// </summary>
        /// <param name="MyResults"></param>
        /// <returns></returns>
        protected List<FrequencyElementClass> GetListFrequencyElements(List<ResultType_MAINClass> MyResults)
        {
            List<FrequencyElementClass> ret = new List<FrequencyElementClass>();

            //проверяем все элементы на наличие значений для рассчёта
            if (this.InitialResults.Count != 0)
            {
                
                

                foreach (ResultType_MAINClass result in this.InitialResults)
                {
                    if (result.PolarizationElements.Count == 0)
                    {
                        throw new Exception("В массиве исходных результатов для рассчёта ДН не хватает значений поляризаций \n Name: " + result.MainOptions.Name);
                    }
                    if (result.SelectedPolarization.SelectedFrequency==null)
                    {
                        throw new Exception("В массиве исходных результатов для рассчёта ДН не хватает значений частот \n Name: " + result.MainOptions.Name);
                    }


                    //if (this._InitialResults[0].SelectedPolarization.SelectedFrequency.Frequency == result.SelectedPolarization.SelectedFrequency.Frequency)
                    //{

                    //}
                    //else
                    //{
                    //    //сомнительный запрет
                    //    throw new Exception("Запрещено рассчитывать разные частоты");
                    //}

                    ret.Add(result.SelectedPolarization.SelectedFrequency);
                }
            }
            else
            {
                throw new Exception("В массиве исходных результатов для рассчёта ДН пусто");
            }
            return ret;
        }

        /// <summary>
        /// Тип рассчитанного результата сумма, разность, усреднённый
        /// </summary>
        [XmlIgnore]
        public CalculationResultTypeEnum WhatIs
        {
            get
            {
                return this.MainOptions.CalculationResultType;
            }
            set
            {
                this.MainOptions.CalculationResultType = value;
            }
        }

        #region override

        public override void ReCalculateData()
        {
            this.IsNotCalculation = true;
            InitialResultsForReport = null;
            base.ReCalculateData();
        }

        public override void CalculateSpesialPolarizations()
        {
            if (!UserSetCalculationData)
            {
                if (IsNotCalculation)
                {
                    this.CalculateMeasurement(WhatIs);
                }
            }

            base.CalculateSpesialPolarizations();
        }

        #endregion

        #region IUnionDiagramDirection
         [XmlIgnore]
        IAmplitudeDiagramDirection IUnionDiagramDirection.MainDiagramDirection
        {
            get
            {
                return this.GetSelectedResult_for_Report_AmplitudeDiagram();
            }
        }
         [XmlIgnore]
        protected List<IAmplitudeDiagramDirection> InitialResultsForReport = null;
         [XmlIgnore]
        List<IAmplitudeDiagramDirection> IUnionDiagramDirection.DiagramDirections
        {
            get
            {
                if (InitialResultsForReport == null)
                {
                    #region создание листа

                    InitialResultsForReport = new List<IAmplitudeDiagramDirection>();

                    foreach (ResultType_MAINClass res in this.InitialResults)
                    {
                        if (res is ResultTypeClassДН)
                        {
                            ResultTypeClassДН res1 = res as ResultTypeClassДН;

                            InitialResultsForReport.Add(res1.GetSelectedResult_for_Report_AmplitudeDiagram());
                        }
                        else
                        {
                            if (res is ResultTypeClassСДНМ)
                            {
                                ResultTypeClassСДНМ res1 = res as ResultTypeClassСДНМ;

                                InitialResultsForReport.Add(res1.GetSelectedResult_for_Report_AmplitudeDiagram());
                            }
                            else
                            {
                                throw new Exception("Неизвесный тип измерения попал в исходные результаты");
                            }
                        }
                    }

                    #endregion
                }


                return InitialResultsForReport;
            }
        }
         [XmlIgnore]
        UnionDiagramDirectionType IUnionDiagramDirection.TypeDiagram
        {
            get
            {
                UnionDiagramDirectionType ret = UnionDiagramDirectionType.SumDiagram_in_RAZ;

                switch (WhatIs)
                {
                    case CalculationResultTypeEnum.Averaged:
                        {
                            ret = UnionDiagramDirectionType.AveragedDiagram;
                            break;
                        }
                    case CalculationResultTypeEnum.Sum_by_RAZ:
                        {
                            ret = UnionDiagramDirectionType.SumDiagram_in_RAZ;
                            break;
                        }
                    case CalculationResultTypeEnum.Sum_by_DB:
                        {
                            ret = UnionDiagramDirectionType.SumDiagram_in_DB;
                            break;
                        }
                    case CalculationResultTypeEnum.Difference:
                        {
                            ret = UnionDiagramDirectionType.DifferenceDiagram;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Не определён тип рассчёта, " + this.GetType().ToString());
                        }
                }

                return ret;
            }
        }

        protected bool _NeedOnlySUM = true;
         [XmlIgnore]
        bool Report_Sheild_M_Interfaces.IUnionDiagramDirection.NeedOnlySUM
        {
            get
            {
                return _NeedOnlySUM;
            }
            set
            {
                _NeedOnlySUM = value;
            }
        }

        #endregion
    }
}
