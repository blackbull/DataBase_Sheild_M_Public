using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report_Sheild_M_Interfaces;
using ResultOptionsClassLibrary;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Спец класс для создания отчётов амплитудной диаграммы из ResultTypeClassДН
    /// </summary>
    [Serializable]
    internal class ResultTypeClassДН_for_Report_AmplitudeDiagram : IAmplitudeDiagramDirection
    {
        public ResultTypeClassДН_for_Report_AmplitudeDiagram(ResultType_MAINClass result1, int SelectPolarizationIndex, int SelectedFrequencyIndex)
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
            DiagramDirection = null;
            OtherOptionsIsFill = false;
        }

        protected FrequencyElementClass Freq;
        protected ResultType_MAINClass result;
        protected IBaseReportResult baseResult;
        protected int PolarizationIndex = 0;
        protected int FrequencyIndex = 0;


        #region IAmplitudeDiagramDirection
        string IAmplitudeDiagramDirection.GainFactor
        {
            get
            {
                return Math.Round(Freq.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности, 2).ToString();
            }
        }

        string IAmplitudeDiagramDirection.MaxDiagramDirection
        {
            get
            {
                return Math.Round(Freq.CalculationResults.Направление_максимума_диаграммы_направленности, 2).ToString();
            }
        }

        string IAmplitudeDiagramDirection.WidthDiagramDirection
        {
            get
            {
                return Math.Round(Freq.CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности, 2).ToString();
            }
        }

        string IAmplitudeDiagramDirection.LevelLateralPetal
        {
            get
            {
                return Math.Round(Freq.CalculationResults.Уровень_боковых_лепестков, 2).ToString();
            }
        }

        string IAmplitudeDiagramDirection.RemovalFistLateralPetal
        {
            get
            {
                return Math.Round(Freq.CalculationResults.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности, 2).ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyDiagramDirection
        {
            get
            {
                return Freq.CalculationResults.Погрешность_ДН.ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyGainFactor
        {
            get
            {
                return Freq.CalculationResults.Погрешность_КУ_в_МАХ.ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyMaxDiagramDirection
        {
            get
            {
                return Freq.CalculationResults.Погрешность_Напр_МАХ_ДН.ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyWidthDiagramDirection
        {
            get
            {
                return Freq.CalculationResults.Погрешность_Ширина_ДН.ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyLevelLateralPetal
        {
            get
            {
                return Freq.CalculationResults.Погрешность_УБЛ.ToString();
            }
        }

        string IAmplitudeDiagramDirection.BorderToInaccuracyRemovalFistLateralPetal
        {
            get
            {
                return Freq.CalculationResults.Погрешность_Смещения_лепестка.ToString();
            }
        }

        protected float?[] DiagramDirection = null;

        float?[] IAmplitudeDiagramDirection.DiagramDirection
        {
            get
            {
                if (DiagramDirection == null)
                {                   
                    DiagramDirection = ResultType_MAINClass.GetMassivForReport(Freq.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Amplitude, result.NeedRemoval);
                }

                return DiagramDirection;
            }
        }

        bool _NeedTable = true;
        bool IAmplitudeDiagramDirection.NeedTable
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

        bool IAmplitudeDiagramDirection.NeedDetailedTable
        {
            get
            {
                return this.result.NeedDetailedTable;
            }
        }

        bool _NeedCartesianGraph = true;
        bool IAmplitudeDiagramDirection.NeedCartesianGraph
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
        bool IAmplitudeDiagramDirection.NeedPolarGraph
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

        bool IAmplitudeDiagramDirection.NeedRemoval
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
            get 
            {
                return Freq.ToString();
            }
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
            if(!OtherOptionsIsFill)
            {
                baseResult.OtherOptions[0].Name_Description = "Max/Min";
                baseResult.OtherOptions[0].Data = Math.Round(Freq.CalculationResults.Отношение_MaxMin, 2).ToString();                                

                baseResult.OtherOptions[1].Name_Description = "Коэффициент_усиления_в_минимуме_диаграммы_направленности";
                baseResult.OtherOptions[1].Data = Math.Round(Freq.CalculationResults.Коэффициент_усиления_в_минимуме_диаграммы_направленности, 2).ToString();

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
