using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using System.Xml.Serialization;
using Report_Sheild_M_Interfaces;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Результат измерения СДНМ
    /// </summary>
    [Serializable]
    public class ResultTypeClassСДНМ : ResultType_MAINClass, IResultType_СДНМ
    {
        public ResultTypeClassСДНМ() { }

        public ResultTypeClassСДНМ(bool FillMeasurementData)
            : base(FillMeasurementData) { }

        [XmlIgnore]
        public PolarizationElementClass SUM_Polarization
        {
            get
            {
                this.CalculateSpesialPolarizations();

                return PolarizationElements[2];
            }
            protected set
            {
                this.PolarizationElements[2] = value;
            }
        }

        [XmlIgnore]
        public PolarizationElementClass Main_Polarization
        {
            get
            {
                return PolarizationElements[0];
            }
            set
            {
                this.PolarizationElements[0] = value;
            }
        }

        [XmlIgnore]
        public PolarizationElementClass Cross_Polarization
        {
            get
            {
                return PolarizationElements[1];
            }
            set
            {
                this.PolarizationElements[1] = value;
            }
        }

        /// <summary>
        /// какую поляризацию возврвщать при обращении к SelectedPolarization
        /// </summary>
        public SelectedPolarizationEnum ChangeSelectedPolarization = SelectedPolarizationEnum.Sum;

        public int SelectedPolarizationIndex
        {
            get
            {
                int ret = -1;
                switch (ChangeSelectedPolarization)
                {
                    case SelectedPolarizationEnum.Sum:
                        ret = 2;
                        break;
                    case SelectedPolarizationEnum.Main:
                        ret = 0;
                        break;
                    case SelectedPolarizationEnum.Cross:
                        ret = 1;
                        break;
                }
                return ret;
            }
        }

        protected bool IsNotCalculatetSUM_Polarization = true;

        #region вспомогательные переменные

        PolarizationElementClass tempMain = null;
        PolarizationElementClass tempCross = null;

        #endregion

        #region override

        /// <summary>
        /// перегрузки выбранной поляризации переведена на суммарную поляризацию
        /// </summary>
        [XmlIgnore]
        public override PolarizationElementClass SelectedPolarization
        {
            get
            {
                switch (ChangeSelectedPolarization)
                {
                    case SelectedPolarizationEnum.Sum:
                        return this.SUM_Polarization;
                    case SelectedPolarizationEnum.Main:
                        return this.Main_Polarization;
                    case SelectedPolarizationEnum.Cross:
                        return this.Cross_Polarization;
                }
                //до сюда никогда не дойдёт, это только чтоб функция не ругалась
                return this.SUM_Polarization;
            }
        }

        public override void ReCalculateData()
        {
            IsNotCalculatetSUM_Polarization = true;
            base.ReCalculateData();
        }

        public override void CalculateSpesialPolarizations()
        {
            if (IsNotCalculatetSUM_Polarization)
            {
                double Step;
                double Start;
                double Stop;

                ResultType_MAINClass.GetCoordinatForInterpolation(this.MainOptions, out Start, out Stop, out Step);

                tempMain = PolarizationElementClass.InterpolationByStep(this.Main_Polarization, Step, Start, Stop);
                tempCross = PolarizationElementClass.InterpolationByStep(this.Cross_Polarization, Step, Start, Stop);
                
                PolarizationElements[2] = PolarizationElementClass.CalculatePolarization(PolarizationElementClass.SumRaznPolarizEnum.SumByPower, tempMain, tempCross);
                PolarizationElements[2].Polarization = SelectedPolarizationEnum.Sum;
                
                //копируем фазу main
                for (int i = 0; i < tempMain.FrequencyElements.Count; i++)
                {
                    //копируем сокрытие частот
                    PolarizationElements[2].FrequencyElements[i].IsHideFrequency = this.Main_Polarization.SelectedFrequency.IsHideFrequency;

                    for (int j = 0; j < tempMain.FrequencyElements[i].ResultAmpl_PhaseElements.Count; j++)
                    {
                        PolarizationElements[2].FrequencyElements[i].ResultAmpl_PhaseElements[j].Phase_degree = tempMain.FrequencyElements[i].ResultAmpl_PhaseElements[j].Phase_degree;
                    }
                }

                IsNotCalculatetSUM_Polarization = false;

                //копируем все рассчитанные данные из main  в SUM (ФЦ тоже копируется)
                CopyAllCalculationData(PolarizationElements[2], this.Main_Polarization);

                //рассчитываем независящие от частоты данные
                CalculationResultsClass.CalculateAll_NO_Constant(PolarizationElements[2].FrequencyElements);

#warning удалить тестовые функции
                //тестовое
                //foreach (FrequencyElementClass fe in PolarizationElements[2].FrequencyElements)
                //{
                //    CalculationResultsClass.CalculateAllConstant(fe, this, ref fe._CalculationResults);
                //}
                

                //подправляем выбранную частоту, та же что и у Main
                if (PolarizationElements[2].SelectedFrequencyIndex < 0)
                    PolarizationElements[2].SelectedFrequencyIndex = this.Main_Polarization.SelectedFrequencyIndex;
            }

            base.CalculateSpesialPolarizations();
        }

        protected override void FillMeasurementData()
        {
            this.AddPolarization(3);
            PolarizationElements[0].Polarization = SelectedPolarizationEnum.Main;
            PolarizationElements[1].Polarization = SelectedPolarizationEnum.Cross;
        }

        //public override string ToStringFullName()
        //{
        //    return CreateFullName(this.ToString(), this.SelectedPolarization.SelectedFrequency.ToString(), this.MainOptions.Date.ToShortDateString(), this.Antenn.ToString()," "+ChangeSelectedPolarization.ToString());
        //}
        #endregion

        #region генерация интерфейсов для протоколов

        public ISumDiagramDirection GetSelectedResult_for_Report_SUMDiagram()
        {
            return new ResultTypeClassСДНМ_for_Report_SUMDiagram(this, this.SelectedPolarization.SelectedFrequencyIndex);
        }

        public List<ISumDiagramDirection> GetListResult_for_Report_SUMDiagram()
        {
            List<ISumDiagramDirection> ret = new List<ISumDiagramDirection>();

            for (int i = 0; i < this.SUM_Polarization.FrequencyElements.Count; i++)
            {
                ret.Add(new ResultTypeClassСДНМ_for_Report_SUMDiagram(this, i));
            }
            return ret;
        }
        
        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IAmplitudeDiagramDirection GetSelectedResult_for_Report_AmplitudeDiagram()
        {
            return new ResultTypeClassДН_for_Report_AmplitudeDiagram(this, SelectedPolarizationIndex, this.SelectedPolarization.SelectedFrequencyIndex);
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IPhaseDiagram GetSelectedResult_for_Report_PhaseDiagram()
        {
            return new ResultTypeClassДН_for_Report_PhaseDiagram(this, SelectedPolarizationIndex, this.SelectedPolarization.SelectedFrequencyIndex);
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IAmplitudeDiagramDirection> GetListResult_for_Report_AmplitudeDiagram()
        {
            List<IAmplitudeDiagramDirection> ret = new List<IAmplitudeDiagramDirection>();

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
                {
                    ret.Add(new ResultTypeClassДН_for_Report_AmplitudeDiagram(this, j, i));
                }
            }
            return ret;
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IPhaseDiagram> GetListResult_for_Report_PhaseDiagram()
        {
            List<IPhaseDiagram> ret = new List<IPhaseDiagram>();
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
                {
                    ret.Add(new ResultTypeClassДН_for_Report_PhaseDiagram(this, j, i));
                }
            }
            return ret;
        }

        #endregion
    }
}
