using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using Report_Sheild_M_Interfaces;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Результат измерения ПХ (базовый)
    /// </summary>
    [Serializable]
    public class ResultTypeПХ : ResultType_MAINClass, IResultType_ПХ
    {
        public ResultTypeПХ() { }

        public ResultTypeПХ(bool FillMeasurementData)
            : base(FillMeasurementData) { }

        #region генерация интерфейсов для протоколов

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IPolarizationDiagram GetSelectedResult_for_Report_AmplitudeDiagram()
        {
            ResultTypeClassПХ_for_Report_PolarizationDiagram ret = new ResultTypeClassПХ_for_Report_PolarizationDiagram(this, 0, this.SelectedPolarization.SelectedFrequencyIndex);

            ret._IsPhaseDiagram = false;

            return ret;
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IPolarizationDiagram GetSelectedResult_for_Report_PhaseDiagram()
        {
            ResultTypeClassПХ_for_Report_PolarizationDiagram ret=new ResultTypeClassПХ_for_Report_PolarizationDiagram(this, 0, this.SelectedPolarization.SelectedFrequencyIndex);

            ret._IsPhaseDiagram = true;

            return ret;
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IPolarizationDiagram> GetListResult_for_Report_AmplitudeDiagram()
        {
            List<IPolarizationDiagram> ret = new List<IPolarizationDiagram>();

            for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
            {
                ResultTypeClassПХ_for_Report_PolarizationDiagram temp = new ResultTypeClassПХ_for_Report_PolarizationDiagram(this, 0, i);

                temp._IsPhaseDiagram = false;

                ret.Add(temp);
            }
            return ret;
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IPolarizationDiagram> GetListResult_for_Report_PhaseDiagram()
        {
            List<IPolarizationDiagram> ret = new List<IPolarizationDiagram>();

            for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
            {
                ResultTypeClassПХ_for_Report_PolarizationDiagram temp = new ResultTypeClassПХ_for_Report_PolarizationDiagram(this, 0, i);

                temp._IsPhaseDiagram = true;

                ret.Add(temp);
            }
            return ret;
        }

        #endregion
    }

    /// <summary>
    /// Спец класс для создания отчётов из ResultTypeClass
    /// </summary>
    [Serializable]
    public class ResultTypeClassПХ_for_Report_PolarizationDiagram : IPolarizationDiagram
    {
        public ResultTypeClassПХ_for_Report_PolarizationDiagram(ResultType_MAINClass result1,int SelectPolarizationIndex, int SelectedFrequencyIndex)
        {
            result = result1;
            baseResult = result1 as IBaseReportResult;

            PolarizationIndex = SelectPolarizationIndex;
            FrequencyIndex = SelectedFrequencyIndex;

            CalculationResultsClass.Calculate_DN_Part(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex]);
            CalculationResultsClass.Calculate_PH_Part(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex]);
        }

        public void ClearData()
        {
            DiagramDirection = null;
        }


        protected ResultType_MAINClass result;
        protected IBaseReportResult baseResult;
        protected int PolarizationIndex = 0;
        protected int FrequencyIndex = 0;

        #region IPolarizationDiagram
        string Report_Sheild_M_Interfaces.IPolarizationDiagram.PolarizationRelation
        {
            get
            {
                return result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Поляризационное_отношение.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.EllipticityCoefficient
        {
            get
            {
                return Math.Round(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Коэффициент_Эллиптичности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.LevelInclination
        {
            get
            {
                return Math.Round(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Угол_наклона_эллипса_поляризации, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyPolarizationDiagram
        {
            get { return result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Погрешность_ПХ.ToString(); }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyPolarizationRelation
        {
            get { return "Не нормирутся"; }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyEllipticityCoefficient
        {
            get { return result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Погрешность_Степени_кросс_поляизации.ToString(); }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyLevelInclination
        {
            get { return "Не нормирутся"; }
        }

        protected float?[] DiagramDirection = null;
        float?[] Report_Sheild_M_Interfaces.IPolarizationDiagram.PolarizationDiagram
        {
            get
            {
                if (DiagramDirection == null)
                {
                    FrequencyElementClass tempFreq = ResultType_MAINClass.GetInterpolationFreq(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex], result.MainOptions);

                    if (_IsPhaseDiagram)
                    {
                        DiagramDirection = ResultType_MAINClass.GetMassivForReport(tempFreq.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Phase,result.NeedRemoval);
                    }
                    else
                    {
                        DiagramDirection = ResultType_MAINClass.GetMassivForReport(tempFreq.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Amplitude, result.NeedRemoval);
                    }
                }
                
                return DiagramDirection;
            }
        }

        bool _NeedTable = true;
        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedTable
        {
            get
            {
                return _NeedTable;
            }
            set
            {
                _NeedTable = value;
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedDetailedTable
        {
            get
            {
                return result.NeedDetailedTable;
            }
        }
        bool _NeedCartesianGraph = true;
        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedCartesianGraph
        {
            get
            {
                return _NeedCartesianGraph;
            }
            set
            {
                _NeedCartesianGraph = value;
            }
        }
        bool _NeedPolarGraph = true;
        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedPolarGraph
        {
            get
            {
                return _NeedPolarGraph;
            }
            set
            {
                _NeedPolarGraph = value;
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedRemoval
        {
            get
            {
                return result.NeedRemoval;
            }
        }


       public bool _IsPhaseDiagram = false;
        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.IsPhaseDiagram
        {
            get { return _IsPhaseDiagram; }
            set { _IsPhaseDiagram = value; }
        }
        #endregion
        
        #region IBaseReportResult
        string IBaseReportResult.ResultName
        {
            get { return baseResult.ResultName; }
        }

        string IBaseReportResult.ResultDescription
        {
            get { return baseResult.ResultDescription; }
            set { baseResult.ResultDescription = value; }
        }

        bool IBaseReportResult.NeedAdditionalData
        {
            get { return baseResult.NeedAdditionalData; }
            set { baseResult.NeedAdditionalData = value; }
        }

        string IBaseReportResult.Frequency
        {
            get { return this.result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].ToString(); }
        }

        bool IBaseReportResult.ShowAntenName
        {
            get
            {
                return baseResult.ShowAntenName;
            }
            set
            {
                baseResult.ShowAntenName = value;
            }
        }

        bool IBaseReportResult.NeedHeader
        {
            get
            {
                return baseResult.NeedHeader;
            }
            set
            {
                baseResult.NeedHeader = value;
            }
        }

        bool IBaseReportResult.NeedMeasurementError
        {
            get
            {
                return baseResult.NeedMeasurementError;
            }
            set
            {
                baseResult.NeedMeasurementError = value;
            }
        }

        bool OtherOptionsIsFill = false;
        void FillOtherOptions()
        {
            if (!OtherOptionsIsFill)
            {
                baseResult.OtherOptions[0].Name_Description = "Max/Min";
                baseResult.OtherOptions[0].Data = Math.Round(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Отношение_MaxMin, 2).ToString();

                baseResult.OtherOptions[1].Name_Description = "Коэффициент усиления в минимуме диаграммы направленности";
                baseResult.OtherOptions[1].Data = Math.Round(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Коэффициент_усиления_в_минимуме_диаграммы_направленности, 2).ToString();

                baseResult.OtherOptions[2].Name_Description = "Коэффициент усиления в максимуме диаграммы направленности";
                baseResult.OtherOptions[2].Data = Math.Round(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex].CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности, 2).ToString();

                OtherOptionsIsFill = true;
            }
        }
        public List<ReportOptionsElementClass> OtherOptions
        {
            get
            {
                FillOtherOptions();
                return baseResult.OtherOptions;
            }
            set
            {
                baseResult.OtherOptions = value;
            }
        }
        string IBaseReportResult.AntenName
        {
            get { return baseResult.AntenName; }
        }

        string IBaseReportResult.AntenaWorksNumber
        {
            get { return baseResult.AntenaWorksNumber; }
        }

        string IBaseReportResult.ZondName
        {
            get { return baseResult.ZondName; }
        }

        string IBaseReportResult.ZondWorksNumber
        {
            get { return baseResult.ZondWorksNumber; }
        }

        string IBaseReportResult.AntenaPolarizationAngle
        {
            get { return baseResult.AntenaPolarizationAngle; }
        }

        string IBaseReportResult.AntenaPolarizationAngleEnd
        {
            get { return baseResult.AntenaPolarizationAngleEnd; }
        }

        string IBaseReportResult.AntenaHeight
        {
            get { return baseResult.AntenaHeight; }
        }

        string IBaseReportResult.AfsPPolarizationAngle
        {
            get { return baseResult.AfsPPolarizationAngle; }
        }

        string IBaseReportResult.AfsAsimutAngle
        {
            get { return baseResult.AfsAsimutAngle; }
        }

        string IBaseReportResult.Parameters_of_the_Measurement
        {
            get
            {
                return this.baseResult.Parameters_of_the_Measurement;
            }
        }
        #endregion
    }
}
