using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report_Sheild_M_Interfaces;
using ResultOptionsClassLibrary;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Спец класс для создания отчётов фазовой диаграммы из ResultTypeClassДН
    /// </summary>
    [Serializable]
    internal class ResultTypeClassДН_for_Report_PhaseDiagram : IPhaseDiagram
    {
        public ResultTypeClassДН_for_Report_PhaseDiagram(ResultType_MAINClass result1, int SelectPolarizationIndex, int SelectedFrequencyIndex)
        {
            result = result1;
            baseResult = result1 as IBaseReportResult;

            PolarizationIndex = SelectPolarizationIndex;
            FrequencyIndex = SelectedFrequencyIndex;

            Freq = ResultType_MAINClass.GetInterpolationFreq(result.PolarizationElements[PolarizationIndex].FrequencyElements[FrequencyIndex], this.result.MainOptions, true);

            CalculationResultsClass.Calculate_DN_Part(Freq);
            CalculationResultsClass.Calculate_PH_Part(Freq);
        }

        public void ClearData()
        {
            PhaseDiagram = null;
        }

        protected FrequencyElementClass Freq;
        protected ResultType_MAINClass result;
        protected IBaseReportResult baseResult;
        protected int PolarizationIndex = 0;
        protected int FrequencyIndex = 0;

        #region IPhaseDiagram
        string IPhaseDiagram.CordinatePhaseCentreMetr
        {
            get
            {
                return ResultType_MAINClass.CordinatePhaseCentre(Freq.CalculationResults.Координаты_фазового_центра_Decart, false, result.MainOptions.MeasurementResultType);
            }
        }

        string IPhaseDiagram.CordinatePhaseCentreGrad
        {
            get
            {
                return ResultType_MAINClass.CordinatePhaseCentre(Freq.CalculationResults.Координаты_фазового_центра_Polar, true, result.MainOptions.MeasurementResultType);
            }
        }

        string IPhaseDiagram.BorderToInaccuracyPhaseDiagram
        {
            get { return this.Freq.CalculationResults.Погрешность_ФД.ToString(); }
        }

        string IPhaseDiagram.BorderToInaccuracyCordinatePhaseCentreMetr
        {
            get { return "Не нормируется"; }
        }

        string IPhaseDiagram.BorderToInaccuracyCordinatePhaseCentreGrad
        {
            get { return "Не нормируется"; }
        }

        float?[] PhaseDiagram = null;

        float?[] IPhaseDiagram.PhaseDiagram
        {
            get
            {
                if (PhaseDiagram == null)
                {
                    
                    PhaseDiagram = ResultType_MAINClass.GetMassivForReport(Freq.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Phase,result.NeedRemoval);
                }
                return PhaseDiagram;
            }
        }

        bool _NeedTable = true;
        bool IPhaseDiagram.NeedTable
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

        bool IPhaseDiagram.NeedDetailedTable
        {
            get
            {
                return this.result.NeedDetailedTable;
            }
        }

        bool _NeedCartesianGraph = true;
        bool IPhaseDiagram.NeedCartesianGraph
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
        bool IPhaseDiagram.NeedPolarGraph
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

        bool IPhaseDiagram.NeedRemoval
        {
            get
            {
                return this.result.NeedRemoval;
            }
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
            get { return this.Freq.ToString(); }
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

        public List<ReportOptionsElementClass> OtherOptions
        {
            get
            {
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
