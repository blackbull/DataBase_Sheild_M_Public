using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms.DataVisualization.Charting;

using System.Xml.Serialization;
using ResultOptionsClassLibrary;
using Report_Sheild_M_Interfaces;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// базовый абстрактный класс для наследующих результатов измерений
    /// </summary>
    [Serializable]
    public abstract class ResultType_MAINClass : IBaseReportResult, IResultType_MAIN,ICloneable
    {
        public ResultType_MAINClass() { }

        public ResultType_MAINClass(bool FillMeasurementData)
        {
            if (FillMeasurementData)
                this.FillMeasurementData();
        }

        #region основные переменные

        /// <summary>
        /// базовые опции измерений
        /// </summary>
        public MainOptionsClass MainOptions = new MainOptionsClass();

        /// <summary>
        /// Испытуемая антенна
        /// </summary>
        public AntennOptionsClass Antenn = new AntennOptionsClass();

        /// <summary>
        /// зонд
        /// </summary>
        public AntennOptionsClass Zond = new AntennOptionsClass();

        /// <summary>
        /// измеренные данные
        /// </summary>
        public List<PolarizationElementClass> PolarizationElements = new List<PolarizationElementClass>();

        #endregion

        #region вспомогательные функции

        /// <summary>
        /// копировать рассчитанные данные всех частот одной поляризации в другую
        /// </summary>
        /// <param name="To"></param>
        /// <param name="From"></param>
        protected static void CopyAllCalculationData(PolarizationElementClass To, PolarizationElementClass From)
        {
            for(int i=0;i<From.FrequencyElements.Count;i++)
            {
                To.FrequencyElements[i]._CalculationResults.Поляризационное_отношение = From.FrequencyElements[i]._CalculationResults.Поляризационное_отношение;
                To.FrequencyElements[i]._CalculationResults.Коэффициент_Эллиптичности = From.FrequencyElements[i]._CalculationResults.Коэффициент_Эллиптичности;
                To.FrequencyElements[i]._CalculationResults.Угол_наклона_эллипса_поляризации = From.FrequencyElements[i]._CalculationResults.Угол_наклона_эллипса_поляризации;

                To.FrequencyElements[i]._CalculationResults.delta_M = From.FrequencyElements[i]._CalculationResults.delta_M;
                To.FrequencyElements[i]._CalculationResults.deltaMistakeFull = From.FrequencyElements[i]._CalculationResults.deltaMistakeFull;
                To.FrequencyElements[i]._CalculationResults.delta_Fo = From.FrequencyElements[i]._CalculationResults.delta_Fo;
                To.FrequencyElements[i]._CalculationResults.delta_Фо = From.FrequencyElements[i]._CalculationResults.delta_Фо;


                //To.FrequencyElements[i]._CalculationResults.delta_O_max = From.FrequencyElements[i]._CalculationResults.delta_O_max;
                //To.FrequencyElements[i]._CalculationResults.delta_Y_05 = From.FrequencyElements[i]._CalculationResults.delta_Y_05;

                //копируем фазовый центр

                To.FrequencyElements[i]._CalculationResults.Координаты_фазового_центра_Decart = From.FrequencyElements[i]._CalculationResults.Координаты_фазового_центра_Decart;
                To.FrequencyElements[i]._CalculationResults.Координаты_фазового_центра_Polar = From.FrequencyElements[i]._CalculationResults.Координаты_фазового_центра_Polar;
            }
        }

        /// <summary>
        /// добавить в массив поляризации
        /// </summary>
        /// <param name="NowMath"></param>
        protected void AddPolarization(uint NowMath)
        {
            while (this.PolarizationElements.Count < NowMath)
            {
                this.PolarizationElements.Add(new PolarizationElementClass());
            }
        }

        #endregion

        #region перегружаемые функции

        public override string ToString()
        {
            return this.ToString(false);
        }

        virtual public string ToString(bool NeedParams)
        {
            string ret = "";

            if (NeedParams)
            {
                ret = "Не указан тип измерения";


                if (MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                {
                    ret = string.Format("Меридиональное сечение от {0}° до {1}° с шагом {2}°", Math.Round(MainOptions.Parameters.StartOPU_Y, 1), Math.Round(MainOptions.Parameters.StopOPU_Y, 1), Math.Round(MainOptions.Parameters.StepMeasurement, 1));
                }
                else
                {
                    if (MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                    {
                        ret = string.Format("Азимутальное сечение от {0}° до {1}° с шагом {2}°", Math.Round(MainOptions.Parameters.StartOPU_W, 1), Math.Round(MainOptions.Parameters.StopOPU_W, 1), Math.Round(MainOptions.Parameters.StepMeasurement, 1));
                    }
                    else
                    {
                        if (MainOptions.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма)
                        {
                            ret = string.Format("Поляризационное сечение от {0}° до {1}° с шагом {2}°", Math.Round(MainOptions.Parameters.StartTower_W, 1), Math.Round(MainOptions.Parameters.StopTower_W, 1), Math.Round(MainOptions.Parameters.StepMeasurement, 1));
                        }
                        else
                        {
                            if (MainOptions.MeasurementResultType == MeasurementTypeEnum.Коэффицент_усиления)
                            {
                                ret = "";
                            }
                        }
                    }
                }
            }
            else
            {
                ret = this.MainOptions.Name + " №" + this.id;
            }

            return ret;
        }

        virtual public string ToStringFullName()
        {
            return CreateFullName(this.ToString(), this.SelectedPolarization.SelectedFrequency.ToString(), this.MainOptions.Date.ToShortDateString(), this.Antenn.ToString());

           // return string.Format("{0} ({1})\n от {2} \n{3};", this.ToString(), this.SelectedPolarization.SelectedFrequency.ToString(), this.MainOptions.Date.ToShortDateString(), this.Antenn.ToString());
        }

        /// <summary>
        /// рассчитать спец поляризации
        /// </summary>
        virtual public void CalculateSpesialPolarizations() 
        {

        }

        public virtual void ReCalculateData() { }
            
        [XmlIgnore]
        public virtual PolarizationElementClass SelectedPolarization
        {
            get
            {
                return PolarizationElements[0];
            }
            set
            {
                PolarizationElements[0] = value;
            }
        }

        /// <summary>
        /// запонить массив поляризаций
        /// </summary>
        protected virtual void FillMeasurementData()
        {
            this.AddPolarization(1); 
            PolarizationElements[0].Polarization = SelectedPolarizationEnum.None;
        }

        #endregion

        #region для БД

        public int id = -1;

        #endregion

        #region IBaseReportResult

        string Report_Sheild_M_Interfaces.IBaseReportResult.ResultName
        {
            get 
            {
                return string.Format("№ {0} от {1}", this.id, this.MainOptions.Date.ToShortDateString());
            }
        }

        

        string Report_Sheild_M_Interfaces.IBaseReportResult.ResultDescription
        {
            get
            {
                string ret=MainOptions.Descriptions;
                MainOptionsClass.DecodingDescription(MainOptions,out ret);

                return ret;
            }
            set
            {
                MainOptions.Descriptions = value;
            }
        }

        bool _NeedAdditionalData = false;
        bool Report_Sheild_M_Interfaces.IBaseReportResult.NeedAdditionalData
        {
            get
            {
                return _NeedAdditionalData;
            }
            set
            {
                _NeedAdditionalData = value;
            }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.Frequency
        {
            get 
            {
                string ret = "";

                if (!(this is ResultTypeClassКУ))
                {
                    ret = Math.Round(this.SelectedPolarization.SelectedFrequency.Frequency, 2).ToString();

                    if (this.MainOptions.Parameters.SegmentTable.Count == 0)
                    {
                        ret = "№ " + ret;
                    }
                    else
                    {
                        ret = ret + " МГц";
                    }
                }
                
                return ret; 
            }
        }

        protected bool _ShowAntenName = true;
        bool IBaseReportResult.ShowAntenName
        {
            get
            {
                return _ShowAntenName;
            }
            set
            {
                _ShowAntenName = value;
            }
        }

        protected bool _NeedHeader = true;
        bool IBaseReportResult.NeedHeader
        {
            get
            {
                return _NeedHeader;
            }
            set
            {
                _NeedHeader = value;
            }
        }

        protected bool _NeedMeasurementError = true;
        bool IBaseReportResult.NeedMeasurementError
        {
            get
            {
                return _NeedMeasurementError;
            }
            set
            {
                _NeedMeasurementError = value;
            }
        }


        [XmlIgnore]
        protected List<ReportOptionsElementClass> OtherOptions = new List<ReportOptionsElementClass>();
        protected void CheckFillOtherOptions()
        {
            if (OtherOptions == null)
                OtherOptions = new List<ReportOptionsElementClass>();
            while (OtherOptions.Count<4)
            {
                OtherOptions.Add(new ReportOptionsElementClass());
            }

        }

        /// <summary>
        /// 0-max/min
        /// 1-min
        /// 2-max
        /// </summary>
        List<ReportOptionsElementClass> IBaseReportResult.OtherOptions
        {
            get
            {
                CheckFillOtherOptions();
                return OtherOptions;
            }
            set
            {
                CheckFillOtherOptions();
                OtherOptions = value;
            }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AntenName
        {
            get { return Antenn.Name; }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AntenaWorksNumber
        {
            get { return Antenn.ZAVNumber; }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.ZondName
        {
            get { return Zond.Name; }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.ZondWorksNumber
        {
            get { return Zond.ZAVNumber; }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AntenaPolarizationAngle
        {
            get { return Math.Round(MainOptions.Parameters.StartTower_W, 2).ToString(); }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AntenaPolarizationAngleEnd
        {
            get { return Math.Round(MainOptions.Parameters.StopTower_W, 2).ToString(); }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AntenaHeight
        {
            get { return Math.Round(MainOptions.Parameters.StartTower_Y, 2).ToString(); }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AfsPPolarizationAngle
        {
            get { return Math.Round(MainOptions.Parameters.StartOPU_W, 2).ToString(); }
        }

        string Report_Sheild_M_Interfaces.IBaseReportResult.AfsAsimutAngle
        {
            get { return Math.Round(MainOptions.Parameters.StartOPU_Y, 2).ToString(); }
        }

       string Report_Sheild_M_Interfaces.IBaseReportResult.Parameters_of_the_Measurement
        {
            get
            {
                return this.ToString(true);
            }
        }
        #endregion

        #region Вспомогательные функции для протоколов

        public enum AmplOrPhaseEnum
        {
            Amplitude,
            Phase
        }

        public static float?[] GetMassivForReport(IList<ResultElementClass> result, AmplOrPhaseEnum ampOrPhase,bool needRemoval)
        {
            float?[] Datadiagramm = new float?[720];
            double?[] coordinats = new double?[720];
            int add = 0;

            if (needRemoval)
                add = 180;

            ////Проверяем на наличие отрицательных координат
            //foreach (ResultElementClass r in result)
            //{
            //    if (r.Cordinate < -1)
            //    {
            //        add = 180;
            //        break;
            //    }
            //}

            foreach (ResultElementClass res in result)
            {
                int i = Convert.ToInt32((res.Cordinate + add) / 0.5d);

                if (i < 720 && i >= 0)
                {
                    bool addData = false;

                    if (coordinats[i] != null)
                    {
                        double lastCoord = Convert.ToDouble(coordinats[i]);

                        if (Math.Abs(i * 0.5d - lastCoord) > Math.Abs(i * 0.5d - res.Cordinate))
                        {
                            addData = true;
                        }
                    }
                    else
                    {
                        addData = true;
                    }

                    if (addData)
                    {
                        coordinats[i] = res.Cordinate;

                        switch (ampOrPhase)
                        {
                            case AmplOrPhaseEnum.Amplitude:
                                {
                                    Datadiagramm[i] = Convert.ToSingle(res.Ampl_dB);
                                    break;
                                }
                            case AmplOrPhaseEnum.Phase:
                                {
                                    Datadiagramm[i] = Convert.ToSingle(res.Phase_degree);
                                    break;
                                }
                        }
                    }
                }
            }

            return Datadiagramm;
        }

        public static float?[] GetMassivForReport(IList<PointDouble> result,bool NeedRemoval)
        {
            List<ResultElementClass> re = new List<ResultElementClass>();

            foreach (PointDouble pd in result)
            {
                re.Add(new ResultElementClass(pd.X, pd.Y, pd.Y));
            }

            return GetMassivForReport(re, AmplOrPhaseEnum.Amplitude, NeedRemoval);
        }

        public string CordinatePhaseCentreMetr
        {
            get
            {
                PointDouble temp=this.SelectedPolarization.SelectedFrequency.CalculationResults.Координаты_фазового_центра_Decart;
                return CordinatePhaseCentre(temp, false);
            }
        }

        public string CordinatePhaseCentreGrad
        {
            get
            {
                PointDouble temp = this.SelectedPolarization.SelectedFrequency.CalculationResults.Координаты_фазового_центра_Polar;
                return CordinatePhaseCentre(temp, true);
            }
        }

        public string CordinatePhaseCentre(PointDouble Data, bool IsPolar)
        {
            return CordinatePhaseCentre(Data, IsPolar, this.MainOptions.MeasurementResultType);
        }

        public static string CordinatePhaseCentre(PointDouble Data, bool IsPolar, MeasurementTypeEnum MeasurementType)
        {
            string ret = "Не удалось сгенерировать";


            if (MeasurementType == MeasurementTypeEnum.ДН_Меридиан || MeasurementType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
            {
                if (IsPolar)
                {
                    ret = string.Format("Ѳ= {0}° ρ= {1}мм", Math.Round(Data.X, 2), Math.Round(Data.Y, 3));
                }
                else
                {
                    ret = string.Format("Z= {0}мм X= {1}мм", Math.Round(Data.X, 3), Math.Round(Data.Y, 3));
                }
            }
            else
            {
                if (MeasurementType == MeasurementTypeEnum.ДН_Азимут || MeasurementType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                {
                    if (IsPolar)
                    {
                        ret = string.Format("φ= {0}° ρ= {1}мм", Math.Round(Data.X, 2), Math.Round(Data.Y, 3));
                    }
                    else
                    {
                        ret = string.Format("X= {0}мм Y= {1}мм", Math.Round(Data.X, 3), Math.Round(Data.Y, 3));
                    }
                }
            }

            return ret;
        }

        public static void GetCoordinatForInterpolation(MainOptionsClass Options, out double Start, out double Stop, out double Step, bool NeedRound=false)
        {
            Step = Convert.ToDouble(Options.Parameters.StepMeasurement);
            Start = double.NaN;
            Stop = double.NaN;

            if (Options.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан || Options.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан)
            {
                Start = Convert.ToDouble(Options.Parameters.StartOPU_Y);
                Stop = Convert.ToDouble(Options.Parameters.StopOPU_Y);
            }
            else
            {
                if (Options.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут || Options.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
                {
                    Start = Convert.ToDouble(Options.Parameters.StartOPU_W);
                    Stop = Convert.ToDouble(Options.Parameters.StopOPU_W);
                }
                else
                {
                    if (Options.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма)
                    {
                        Start = Convert.ToDouble(Options.Parameters.StartTower_W);
                        Stop = Convert.ToDouble(Options.Parameters.StopTower_W);
                    }
                }
            }
            if (NeedRound)
            {
                Start = Math.Round(Start, 0);
                Stop = Math.Round(Stop, 0);
            }
        }
        
        /// <summary>
        /// интерполировать частотный элемент
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static FrequencyElementClass GetInterpolationFreq(FrequencyElementClass freq, MainOptionsClass options, bool NeedRound=false)
        {
            double Start;
            double Stop;
            double Step;

            ResultType_MAINClass.GetCoordinatForInterpolation(options, out Start, out Stop, out Step, NeedRound);

            FrequencyElementClass tempFreq = FrequencyElementClass.InterpolationByStep(freq, Step, Start, Stop);

            tempFreq._CalculationResults = freq._CalculationResults;

            return tempFreq;
        }

        public bool NeedDetailedTable
        {
            get
            {
                bool ret = false;

                if (this.MainOptions.Parameters.StepMeasurement <= 0.5m)
                {
                    ret = true;
                }

                return ret;
            }
        }

        public bool NeedRemoval
        {
            get
            {
                bool ret = false;

                if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || this.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                {
                    if (this.MainOptions.Parameters.StartOPU_Y < -2 && this.MainOptions.Parameters.StopOPU_Y < 182 || this.MainOptions.Parameters.StopOPU_Y < -2 && this.MainOptions.Parameters.StartOPU_Y <182)
                    {
                        ret = true;
                    }
                }
                else
                {
                    if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || this.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                    {
                        if (this.MainOptions.Parameters.StartOPU_W < -2 && this.MainOptions.Parameters.StopOPU_W <182 || this.MainOptions.Parameters.StopOPU_W < -2 && this.MainOptions.Parameters.StartOPU_W <182)
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (this.MainOptions.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма)
                        {
                            if (this.MainOptions.Parameters.StartTower_W < -2 && this.MainOptions.Parameters.StopTower_W <182 || this.MainOptions.Parameters.StopTower_W < -2 && this.MainOptions.Parameters.StartTower_W <182)
                            {
                                ret = true;
                            }
                        }
                    }
                }


                return ret;
            }
        }

        public static string CreateFullName(string mainName = "", string frequencyName = "", string DateTime = "", string AntennName = "", string polarization = "")
        {
            string ret = "";

            ret = string.Format("{0} ({1}{4})\n от {2} \n{3};", mainName, frequencyName, DateTime, AntennName, polarization);

            return ret;
        }

        #endregion

        #region IResultType_MAINClass

        PolarizationElementClass IResultType_MAIN.SelectedPolarization
        {
            get { return this.SelectedPolarization; }
            set { this.SelectedPolarization = value; }
        }

        /// <summary>
        /// выдача массива всех элементов
        /// </summary>
        IList<IPolarizationElement> IResultType_MAIN.PolarizationElements
        {
            get { return this.PolarizationElements.ToList<IPolarizationElement>(); }
        }

        /// <summary>
        /// Испытуемая антенна
        /// </summary>
        AntennOptionsClass IResultType_MAIN.Antenn
        {
            get { return Antenn; }
            set { Antenn = value; }
        }

        /// <summary>
        /// зонд
        /// </summary>
        AntennOptionsClass IResultType_MAIN.Zond
        {
            get { return Zond; }
            set { Zond = value; }
        }

        /// <summary>
        /// базовые опции измерений
        /// </summary>
        MainOptionsClass IResultType_MAIN.MainOptions
        {
            get { return MainOptions; }
            set
            {
                MainOptions = value;
            }
        }

        int IResultType_MAIN.id
        {
            get { return id; }
            set { id = value; }
        }

        #endregion

        public object Clone()
        {
            ResultType_MAINClass ret = this.MemberwiseClone() as ResultType_MAINClass;
            //ret.PolarizationElements.Clear();
            ret.MainOptions = this.MainOptions.Clone() as MainOptionsClass;
            List<PolarizationElementClass> NewPol = new List<PolarizationElementClass>();

            foreach (PolarizationElementClass pol in this.PolarizationElements)
            {
                NewPol.Add(pol.Clone() as PolarizationElementClass);
            }

            ret.PolarizationElements = NewPol;

           return ret;
        }
    }
}