using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report_Sheild_M_Interfaces;
using System.Xml.Serialization;
using ResultOptionsClassLibrary;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Спец класс для создания отчётов
    /// </summary>
    [Serializable]
    internal class ResultTypeClassСДНМ_for_Report_SUMDiagram : ISumDiagramDirection
    {
        public ResultTypeClassСДНМ_for_Report_SUMDiagram(ResultTypeClassСДНМ result1, int SelectedFrequencyIndex)
        {
            result = result1;
            baseResult = result1 as IBaseReportResult;

            FrequencyIndex = SelectedFrequencyIndex;



            FreqMain = ResultType_MAINClass.GetInterpolationFreq(result.Main_Polarization.FrequencyElements[FrequencyIndex], this.result.MainOptions, true);
            FreqCross = ResultType_MAINClass.GetInterpolationFreq(result.Cross_Polarization.FrequencyElements[FrequencyIndex], this.result.MainOptions, true);
            FreqSUM = ResultType_MAINClass.GetInterpolationFreq(result.SUM_Polarization.FrequencyElements[FrequencyIndex], this.result.MainOptions, true);

            CalculationResultsClass.Calculate_DN_Part(FreqMain);
            CalculationResultsClass.Calculate_PH_Part(FreqMain);

            CalculationResultsClass.Calculate_DN_Part(FreqCross);
            CalculationResultsClass.Calculate_PH_Part(FreqCross);

            CalculationResultsClass.Calculate_DN_Part(FreqSUM);
            CalculationResultsClass.Calculate_PH_Part(FreqSUM);
        }

        public void ClearData()
        {
            _DataSUM = null;
            _DataMain = null;
            _DataCros = null;
        }

        protected FrequencyElementClass FreqMain;
        protected FrequencyElementClass FreqCross;
        protected FrequencyElementClass FreqSUM;

        protected ResultTypeClassСДНМ result;
        protected IBaseReportResult baseResult;
        protected int FrequencyIndex = 0;

        #region ISumDiagramDirection

        #region ISumDiagramDirection
        bool _NeedSumTable = true;
        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedSumTable
        {
            get
            {
                return _NeedSumTable;
            }
            set
            {
                _NeedSumTable = value;
            }
        }

        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedDetailedSumTable
        {
            get
            {
                return result.NeedDetailedTable;
            }
        }

        bool _NeedSumCartesianGraph = true;
        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedSumCartesianGraph
        {
            get
            {
                return _NeedSumCartesianGraph;
            }
            set
            {
                _NeedSumCartesianGraph = value;
            }
        }

        bool _NeedSumPolarGraph = true;
        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedSumPolarGraph
        {
            get
            {
                return _NeedSumPolarGraph;
            }
            set
            {
                _NeedSumPolarGraph = value;
            }
        }

        protected bool _NeedSUM = true;
        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedSUM
        {
            get
            {
                return _NeedSUM;
            }
            set
            {
                _NeedSUM = value;
            }
        }

        protected bool _NeedMainCross = true;
        bool Report_Sheild_M_Interfaces.ISumDiagramDirection.NeedMainCross
        {
            get
            {
                return _NeedMainCross;
            }
            set
            {
                _NeedMainCross = value;
            }
        }

        #endregion

        #region IAmpMain
        protected float?[] _DataMain = null;

        [XmlIgnore]
        float?[] Report_Sheild_M_Interfaces.IAmpMain.DataMain
        {
            get
            {
                if (_DataMain == null)
                {
                    _DataMain = ResultType_MAINClass.GetMassivForReport(FreqMain.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Amplitude, result.NeedRemoval);
                }

                return _DataMain;
            }
        }

        bool _NeedTableAmpMain = true;
        bool Report_Sheild_M_Interfaces.IAmpMain.NeedTable
        {
            get
            {
                return _NeedTableAmpMain;
            }
            set
            {
                _NeedTableAmpMain = value;
            }
        }

        bool Report_Sheild_M_Interfaces.IAmpMain.NeedDetailedTable
        {
            get
            {
                return result.NeedDetailedTable;
            }
        }

        #endregion

        #region IAmpCross

        protected float?[] _DataCros = null;

        [XmlIgnore]
        float?[] Report_Sheild_M_Interfaces.IAmpCross.DataCross
        {
            get
            {
                if (_DataCros == null)
                {
                    _DataCros = ResultType_MAINClass.GetMassivForReport(FreqCross.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Amplitude, result.NeedRemoval);
                }

                return _DataCros;
            }
        }

        bool _NeedTableAmpCross = true;
        bool Report_Sheild_M_Interfaces.IAmpCross.NeedTable
        {
            get
            {
                return _NeedTableAmpCross;
            }
            set
            {
                _NeedTableAmpCross = value;
            }
        }

        bool Report_Sheild_M_Interfaces.IAmpCross.NeedDetailedTable
        {
            get
            {
                return result.NeedDetailedTable;
            }
        }

        #endregion

        #region IAmplitudeDiagramDirection

        protected float?[] _DataSUM = null;
        [XmlIgnore]
        float?[] Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.DiagramDirection
        {
            get
            {
                if (_DataSUM == null)
                {
                    _DataSUM = ResultType_MAINClass.GetMassivForReport(FreqSUM.ResultAmpl_PhaseElements, ResultType_MAINClass.AmplOrPhaseEnum.Amplitude, result.NeedRemoval);
                }

                return _DataSUM;
            }
        }


        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.GainFactor
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.MaxDiagramDirection
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Направление_максимума_диаграммы_направленности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.WidthDiagramDirection
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.LevelLateralPetal
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Уровень_боковых_лепестков, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.RemovalFistLateralPetal
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyDiagramDirection
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_ДН.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyGainFactor
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_КУ_в_МАХ.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyMaxDiagramDirection
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_Напр_МАХ_ДН.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyWidthDiagramDirection
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_Ширина_ДН.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyLevelLateralPetal
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_УБЛ.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.BorderToInaccuracyRemovalFistLateralPetal
        {
            get
            {
                return FreqSUM.CalculationResults.Погрешность_Смещения_лепестка.ToString();
            }
        }

        bool Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.NeedRemoval
        {
            get
            {
                return result.NeedRemoval;
            }
        }

        #region не используется

        bool Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.NeedTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.NeedDetailedTable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.NeedCartesianGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IAmplitudeDiagramDirection.NeedPolarGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #endregion

        #region IPhaseDiagram
        string Report_Sheild_M_Interfaces.IPhaseDiagram.CordinatePhaseCentreMetr
        {
            get
            {
                return ResultType_MAINClass.CordinatePhaseCentre(FreqSUM.CalculationResults.Координаты_фазового_центра_Decart, false, result.MainOptions.MeasurementResultType);
            }
        }

        string Report_Sheild_M_Interfaces.IPhaseDiagram.CordinatePhaseCentreGrad
        {
            get
            {
                return ResultType_MAINClass.CordinatePhaseCentre(FreqSUM.CalculationResults.Координаты_фазового_центра_Polar, true, result.MainOptions.MeasurementResultType);
            }
        }


        string Report_Sheild_M_Interfaces.IPhaseDiagram.BorderToInaccuracyCordinatePhaseCentreMetr
        {
            get { return "Не нормируется"; }
        }

        string Report_Sheild_M_Interfaces.IPhaseDiagram.BorderToInaccuracyCordinatePhaseCentreGrad
        {
            get { return "Не нормируется"; }
        }

        #region не используется
        string Report_Sheild_M_Interfaces.IPhaseDiagram.BorderToInaccuracyPhaseDiagram
        {
            get { throw new NotImplementedException(); }
        }

        float?[] Report_Sheild_M_Interfaces.IPhaseDiagram.PhaseDiagram
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IPhaseDiagram.NeedTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IPhaseDiagram.NeedDetailedTable
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IPhaseDiagram.NeedCartesianGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IPhaseDiagram.NeedPolarGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPhaseDiagram.NeedRemoval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
        #endregion

        #region IPolarizationDiagram
        string Report_Sheild_M_Interfaces.IPolarizationDiagram.PolarizationRelation
        {
            get
            {
                return FreqSUM.CalculationResults.Поляризационное_отношение.ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.EllipticityCoefficient
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Коэффициент_Эллиптичности, 2).ToString();
            }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.LevelInclination
        {
            get
            {
                return Math.Round(FreqSUM.CalculationResults.Угол_наклона_эллипса_поляризации, 2).ToString();
            }
        }


        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyPolarizationRelation
        {
            get { return "Не нормирутся"; }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyEllipticityCoefficient
        {
            get { return FreqSUM.CalculationResults.Погрешность_Степени_кросс_поляизации.ToString(); }
        }

        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyLevelInclination
        {
            get { return "Не нормируется"; }
        }

        #region не используется
        string Report_Sheild_M_Interfaces.IPolarizationDiagram.BorderToInaccuracyPolarizationDiagram
        {
            get { throw new NotImplementedException(); }
        }

        float?[] Report_Sheild_M_Interfaces.IPolarizationDiagram.PolarizationDiagram
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedDetailedTable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedCartesianGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedPolarGraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.NeedRemoval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool Report_Sheild_M_Interfaces.IPolarizationDiagram.IsPhaseDiagram
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
        #endregion
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
            get { return this.FreqSUM.ToString(); }
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
                baseResult.OtherOptions[0].Data = Math.Round(FreqSUM.CalculationResults.Отношение_MaxMin, 2).ToString();

                baseResult.OtherOptions[1].Name_Description = "Коэффициент_усиления_в_минимуме_диаграммы_направленности";
                baseResult.OtherOptions[1].Data = Math.Round(FreqSUM.CalculationResults.Коэффициент_усиления_в_минимуме_диаграммы_направленности, 2).ToString();

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
