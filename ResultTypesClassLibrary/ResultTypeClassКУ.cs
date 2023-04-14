using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using System.Xml.Serialization;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Результат измерения КУ
    /// </summary>
    [Serializable]
    public class ResultTypeClassКУ : ResultType_MAINClass, Report_Sheild_M_Interfaces.IGainFactor, IResultType_КУ
    {
        public ResultTypeClassКУ() { }

        public ResultTypeClassКУ(bool FillMeasurementData)
            : base(FillMeasurementData) { }

        public PolarizationElementClass SUM_Polarization
        {
            get
            {
                this.CalculateSpesialPolarizations();

                return PolarizationElements[2];
            }
            set
            {
                this.PolarizationElements[2] = value;
            }
        }

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

        protected bool IsNotCalculatetSUM_Polarization = true;

        /// <summary>
        /// какую поляризацию возврвщать при обращении к SelectedPolarization
        /// </summary>
        public SelectedPolarizationEnum ChangeSelectedPolarization = SelectedPolarizationEnum.Sum;

        #region override

        protected PolarizationElementClass SUM_Temp = null;

        protected PolarizationElementClass Main_Temp = null;

        protected PolarizationElementClass Cross_Temp = null;


        /// <summary>
        /// перегрузки выбранной поляризации переведена на суммарную поляризацию
        /// </summary>
        public override PolarizationElementClass SelectedPolarization
        {
            get
            {
                switch (ChangeSelectedPolarization)
                {
                    case SelectedPolarizationEnum.Sum:
                        {
                            if (SUM_Temp == null)
                            {
                                SUM_Temp = CreateSpesialPolarizationForKY(this.SUM_Polarization);
                            }
                            return SUM_Temp;
                        }
                    case SelectedPolarizationEnum.Main:
                        {
                            if (Main_Temp == null)
                            {
                                Main_Temp = CreateSpesialPolarizationForKY(this.Main_Polarization);
                            }
                            return Main_Temp;
                        }
                    case SelectedPolarizationEnum.Cross:
                        {
                            if (Cross_Temp == null)
                            {
                                Cross_Temp = CreateSpesialPolarizationForKY(this.Cross_Polarization);
                            }
                            return Cross_Temp;
                        }
                }
                //до сюда никогда не дойдёт, это только чтоб функция не ругалась
                return this.SUM_Polarization;
            }
        }

        /// <summary>
        /// создание поляризации удобной для отображения ку на графиках
        /// </summary>
        /// <param name="PolKY"></param>
        /// <returns></returns>
        protected PolarizationElementClass CreateSpesialPolarizationForKY(PolarizationElementClass PolKY)
        {
            PolarizationElementClass ret = new PolarizationElementClass();
            ret.SelectedFrequency = new FrequencyElementClass();

            foreach (FrequencyElementClass freq in PolKY.FrequencyElements)
            {
                ret.SelectedFrequency.ResultAmpl_PhaseElements.Add(new ResultElementClass(freq.Frequency, freq.ResultAmpl_PhaseElements[0].Ampl_dB, double.NaN));
            }

            return ret;
        }

        protected override void FillMeasurementData()
        {
            this.AddPolarization(3);
            PolarizationElements[0].Polarization = SelectedPolarizationEnum.Main;
            PolarizationElements[1].Polarization = SelectedPolarizationEnum.Cross;
        }

        public override void ReCalculateData()
        {
            IsNotCalculatetSUM_Polarization = true;
            DataKY.Clear();
            SUM_Temp = null;
            Main_Temp = null;
            Cross_Temp = null;

            base.ReCalculateData();
        }

        public override void CalculateSpesialPolarizations()
        {
            if (IsNotCalculatetSUM_Polarization)
            {
                PolarizationElements[2] = PolarizationElementClass.CalculatePolarization(PolarizationElementClass.SumRaznPolarizEnum.Sum, this.Main_Polarization, this.Cross_Polarization);
                PolarizationElements[2].Polarization = SelectedPolarizationEnum.Sum;

                IsNotCalculatetSUM_Polarization = false;

                for (int i = 0; i < PolarizationElements[2].FrequencyElements.Count; i++)
                {
                    //копируем сокрытие частот
                    PolarizationElements[2].FrequencyElements[i].IsHideFrequency = this.Main_Polarization.SelectedFrequency.IsHideFrequency;
                }

                //копируем все рассчитанные данные из main  в SUM
                CopyAllCalculationData(this.SUM_Polarization, this.Main_Polarization);

            }

            base.CalculateSpesialPolarizations();
        }

        public override string ToStringFullName()
        {
            return CreateFullName(this.ToString(), "", this.MainOptions.Date.ToShortDateString(), this.Antenn.ToString());
        }

        #endregion
        
        #region IGainFactor

        [XmlIgnore]
        List<Report_Sheild_M_Interfaces.ISrokaGainFactor> DataKY = new List<Report_Sheild_M_Interfaces.ISrokaGainFactor>();

        [XmlIgnore]
        List<Report_Sheild_M_Interfaces.ISrokaGainFactor> Report_Sheild_M_Interfaces.IGainFactor.DataTable
        {
            get
            {
                if (DataKY.Count == 0)
                {
                    for (int i = 0; i < this.Main_Polarization.FrequencyElements.Count; i++)
                    {

                        ResultTypeClassКУ_Stroka temp = new ResultTypeClassКУ_Stroka();

                        temp._Frequency = Math.Round(this.Main_Polarization.FrequencyElements[i].Frequency,2);
                        temp._FrequencyText = this.Main_Polarization.FrequencyElements[i].ToString();
                        temp._DataSUM = Math.Round(this.SUM_Polarization.FrequencyElements[i].ResultAmpl_PhaseElements[0].Ampl_dB,2);
                        temp._BorderToInaccuracyGainSum = this.SUM_Polarization.FrequencyElements[i].CalculationResults.Погрешность_КУ.ToString();
                        temp._PolarizationRelation = this.SUM_Polarization.FrequencyElements[i].CalculationResults.Поляризационное_отношение.ToString();
                        temp._EllipticityCoefficient = Math.Round(this.SUM_Polarization.FrequencyElements[i].CalculationResults.Коэффициент_Эллиптичности,2).ToString();
                        temp._BorderToInaccuracyEllipticityCoefficient = this.SUM_Polarization.FrequencyElements[i].CalculationResults.Погрешность_Степени_кросс_поляизации.ToString();
                        temp._LevelInclination = Math.Round(this.SUM_Polarization.FrequencyElements[i].CalculationResults.Угол_наклона_эллипса_поляризации,2).ToString();
                        
                        DataKY.Add(temp);
                    }
                }
                return DataKY;
            }
        }


        protected bool _NeedGraph = true;

        bool Report_Sheild_M_Interfaces.IGainFactor.NeedGraph
        {
            get
            {
                return _NeedGraph;
            }
            set
            {
                _NeedGraph = value;
            }
        }

        protected bool _NeedOnlySUM = true;

        bool Report_Sheild_M_Interfaces.IGainFactor.NeedOnlySUM
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

    #region Специализированные классы для отчётов

    /// <summary>
    /// Спец класс для создания отчётов КУ
    /// </summary>
    [Serializable]
    internal class ResultTypeClassКУ_Stroka : Report_Sheild_M_Interfaces.ISrokaGainFactor
    {
        public double _Frequency = 0;
        public string _FrequencyText = "";
        public double _DataSUM = 0;
        public string _BorderToInaccuracyGainSum = "";
        public string _EllipticityCoefficient = "";
        public string _BorderToInaccuracyEllipticityCoefficient = "";
        public string _PolarizationRelation = "";
        public string _LevelInclination = "";


        uint? Report_Sheild_M_Interfaces.ISrokaGainFactor.Frequency
        {
            get { return Convert.ToUInt32( _Frequency); }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.FrequencyText
        {
            get { return _FrequencyText; }
        }

        float Report_Sheild_M_Interfaces.ISrokaGainFactor.DataSUM
        {
            get { return Convert.ToSingle(_DataSUM); }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.BorderToInaccuracyGainSum
        {
            get { return _BorderToInaccuracyGainSum; }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.EllipticityCoefficient
        {
            get { return _EllipticityCoefficient; }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.BorderToInaccuracyEllipticityCoefficient
        {
            get { return _BorderToInaccuracyEllipticityCoefficient; }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.PolarizationRelation
        {
            get { return _PolarizationRelation; }
        }

        string Report_Sheild_M_Interfaces.ISrokaGainFactor.LevelInclination
        {
            get { return _LevelInclination; }
        }
    }

    #endregion
}
