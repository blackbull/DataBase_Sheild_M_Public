using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// Частотный элемент
    /// </summary>
    [Serializable]
    public class FrequencyElementClass:ICloneable
    {
        #region конструкторы

        public FrequencyElementClass()
            : this(0) { }

        public FrequencyElementClass(double newFrequency)
        {
            Frequency = newFrequency;
        }
               
        #endregion


        /// <summary>
        /// частота
        /// </summary>
        public double Frequency;

        public bool IsHideFrequency = false;

        /// <summary>
        /// массив значений (амплитуда и фаза) для данной частоты
        /// </summary>
        public List<ResultElementClass> ResultAmpl_PhaseElements = new List<ResultElementClass>();
       

        #region получение Series для графиков

        /// <summary>
        /// получить массив значений №1 для графика 
        /// </summary>
        /// <returns></returns>
        public Series GetAmplSeries()
        {
            Series ret = new Series();

            foreach (ResultElementClass tempElement in ResultAmpl_PhaseElements)
            {
                ret.Points.AddXY(tempElement.Cordinate, tempElement.Ampl_dB);
            }

            return ret;
        }
        /// <summary>
        /// получить массив значений №2 для графика 
        /// </summary>
        /// <returns></returns>
        public Series GetPhaseSeries()
        {
            Series ret = new Series();

            foreach (ResultElementClass tempElement in ResultAmpl_PhaseElements)
            {
                if (!double.IsNaN(tempElement.Phase_degree))
                {
                    ret.Points.AddXY(tempElement.Cordinate, tempElement.Phase_degree);
                }
            }

            return ret;
        }

        #endregion

        #region CalculationResults

        /// <summary>
        /// Рассчитываемые данные для этой частоты
        /// </summary>
        public CalculationResultsClass _CalculationResults = new CalculationResultsClass();

        /// <summary>
        /// Рассчитываемые данные для этой частоты
        /// </summary>
        public ICalculationResults CalculationResults
        {
            get { return _CalculationResults; }
        }

        #endregion

        #region для БД
        public string TableResultName = "";
        public int id = -1;
        #endregion

        #region Static

        #region рассчёт по 2 элемента

        /// <summary>
        /// Среднее двух элементов
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass SrednFrequency(FrequencyElementClass freq1, FrequencyElementClass freq2)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Sredn, freq1, freq2);
        }

        /// <summary>
        /// Сложить значения двух элементов по мощности
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass SumFrequencyByPower(FrequencyElementClass freq1, FrequencyElementClass freq2)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.SumByPower, freq1, freq2);
        }

        /// <summary>
        /// Сложить значения двух элементов 
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass SumFrequency(FrequencyElementClass freq1, FrequencyElementClass freq2)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Sum_in_Raz, freq1, freq2);
        }

        /// <summary>
        /// Разность двух элементов 
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass RaznFrequency(FrequencyElementClass freq1, FrequencyElementClass freq2)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Razn, freq1, freq2);
        }

        /// <summary>
        /// Рассчитать значения двух элементов
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass CalculateFrequency(SumRaznFreqEnum WhatCalculate, FrequencyElementClass freq1, FrequencyElementClass freq2)
        {
            List<FrequencyElementClass> tempList = new List<FrequencyElementClass>();
            tempList.Add(freq1);
            tempList.Add(freq2);

            return FrequencyElementClass.CalculateFrequency(WhatCalculate, tempList);
        }

        #endregion

        #region рассчёт с массивами

        /// <summary>
        /// преобразование массива частот в массив измеренных данных
        /// </summary>
        /// <param name="freqList"></param>
        /// <returns></returns>
        public static IList<IList<ResultElementClass>> GetIListResultElements(List<FrequencyElementClass> freqList)
        {
            IList<IList<ResultElementClass>> ret = new List<IList<ResultElementClass>>();

            foreach (FrequencyElementClass fr in freqList)
            {
                ret.Add(fr.ResultAmpl_PhaseElements);
            }

            return ret;
        }

        /// <summary>
        /// Сложить значения двух элементов по мощности
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass SumFrequencyByPower(List<FrequencyElementClass> freqList)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.SumByPower, freqList);
        }

        /// <summary>
        /// Сложить значения двух элементов 
        /// </summary>
        /// <param name="freq1"></param>
        /// <returns></returns>
        public static FrequencyElementClass SumFrequency(List<FrequencyElementClass> freqList)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Sum_in_Raz, freqList);
        }

        /// <summary>
        /// Разность двух элементов 
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass RaznFrequency(List<FrequencyElementClass> freqList)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Razn, freqList);
        }

        /// <summary>
        /// Среднее элементов 
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass SrednFrequency(List<FrequencyElementClass> freqList)
        {
            return FrequencyElementClass.CalculateFrequency(SumRaznFreqEnum.Sredn, freqList);
        }

        #endregion

        /// <summary>
        /// делегат для рассчёта
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        protected delegate IList<ResultElementClass> CalculateDelegate();

        /// <summary>
        /// Рассчитать массив элементов
        /// </summary>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        public static FrequencyElementClass CalculateFrequency(SumRaznFreqEnum WhatCalculate, List<FrequencyElementClass> freqList)
        {
            #region создаём делегат для рассчёта

            CalculateDelegate Calculate = delegate
            {
                return CalculateResultSumRaznClass.CalculateSumorRazn(FrequencyElementClass.GetIListResultElements(freqList), 0.3d, SumRaznEnum.SumByPower);
            };

            switch (WhatCalculate)
            {
                case SumRaznFreqEnum.SumByPower:
                    {
                        Calculate = delegate
                        {
                            return CalculateResultSumRaznClass.CalculateSumorRazn(FrequencyElementClass.GetIListResultElements(freqList), 0.3d, SumRaznEnum.SumByPower);
                        };
                        break;
                    }
                case SumRaznFreqEnum.Sum_in_Raz:
                    {
                        Calculate = delegate
                        {
                            return CalculateResultSumRaznClass.CalculateSumorRazn(FrequencyElementClass.GetIListResultElements(freqList), 0.3d, SumRaznEnum.Sum_In_Raz);
                        };
                        break;
                    }
                case SumRaznFreqEnum.Sum_in_DB:
                    {
                        Calculate = delegate
                        {
                            return CalculateResultSumRaznClass.CalculateSumorRazn(FrequencyElementClass.GetIListResultElements(freqList), 0.3d, SumRaznEnum.Sum_In_DB);
                        };
                        break;
                    }
                case SumRaznFreqEnum.Razn:
                    {
                        Calculate = delegate
                        {
                            return CalculateResultSumRaznClass.CalculateSumorRazn(FrequencyElementClass.GetIListResultElements(freqList), 0.3d, SumRaznEnum.Razn);
                        };
                        break;
                    }
                case SumRaznFreqEnum.Sredn:
                    {
                        Calculate = delegate
                        {
                            return CalculateResultSumRaznClass.CalculateSredneeArifm(FrequencyElementClass.GetIListResultElements(freqList), 0.3d);
                        };
                        break;
                    }
                default:
                    {
                        throw new Exception("Не определён тип рассчёта: " + WhatCalculate.ToString());
                    }
            }

            #endregion

            //добавляем значение частоты, тк частоты заведомо одинаковые
            FrequencyElementClass ret = new FrequencyElementClass(freqList[0].Frequency);

            #region суммируем погрешности

            List<double> deltaMistakeFull = new List<double>();
            List<double> delta_Fo = new List<double>();
            List<double> delta_Фо = new List<double>();
            List<double> delta_M = new List<double>();

            foreach (FrequencyElementClass fr in freqList)
            {
                if (fr._CalculationResults.deltaMistakeFull != null)
                    deltaMistakeFull.Add(fr._CalculationResults.deltaMistakeFull.Mistake);

                if (fr._CalculationResults.delta_Fo != null)
                    delta_Fo.Add(fr._CalculationResults.delta_Fo.Mistake);
                
                if (fr._CalculationResults.delta_Фо != null)
                    delta_Фо.Add(fr._CalculationResults.delta_Фо.Mistake);

                if (fr._CalculationResults.delta_M != null)
                    delta_M.Add(fr._CalculationResults.delta_M.Mistake);
            }

            ret._CalculationResults.deltaMistakeFull = new MistakeClass(CalculationClass.Calculate_srednee_kvadr(deltaMistakeFull.ToArray()));
            ret._CalculationResults.delta_Fo = new MistakeClass(CalculationClass.Calculate_srednee_kvadr(delta_Fo.ToArray()));
            ret._CalculationResults.delta_Фо = new MistakeClass(CalculationClass.Calculate_srednee_kvadr(delta_Фо.ToArray()));
            ret._CalculationResults.delta_M = new MistakeClass(CalculationClass.Calculate_srednee_kvadr(delta_M.ToArray()));

            #endregion

            IList<ResultElementClass> temp = Calculate();

            foreach (ResultElementClass res in temp)
            {
                ret.ResultAmpl_PhaseElements.Add(res);
            }

            return ret;
        }

        public enum SumRaznFreqEnum
        {
            Sum_in_Raz,
            Sum_in_DB,
            Razn,
            Sredn,
            SumByPower
        }


        public static FrequencyElementClass InterpolationByStep(FrequencyElementClass freq, double Step, double Start, double Stop)
        {
            FrequencyElementClass ret = new FrequencyElementClass(freq.Frequency);

            if (Start > Stop)
            {
                double temp = Start;
                Start = Stop;
                Stop = temp;
            }

            if (Step == 0)
            {
                throw new Exception("Шаг измерения не должен быть 0");
            }


            for (double i = Start; i <= Stop; i += Step)
            {
                ResultElementClass temp=FindOrCalculate_ResultElement_By_Cordinate(i,freq.ResultAmpl_PhaseElements);

                ret.ResultAmpl_PhaseElements.Add(temp);
            }
          
            return ret;
        }

        public static ResultElementClass FindOrCalculate_ResultElement_By_Cordinate(double FindElement, List<ResultElementClass> ResultList)
        {
            ResultElementClass ret = null;
            IFinder left;
            IFinder right;
            IFinder CoincidenceElement;

            FindResultEnum findresult = FinderClass.FindNeighborObj(ResultList.ToArray(), out left, out right, out CoincidenceElement, FindElement);

            bool needError = false;
            string ErrorText = "";

            switch (findresult)
            {
                case FindResultEnum.FindNeighbor:
                    {
                        #region генерируем новые опции из соседних частот

                        ret = new ResultElementClass();

                        ResultElementClass TAO_left = left as ResultElementClass;
                        ResultElementClass TAO_right = right as ResultElementClass;


                        ret.Cordinate = FindElement;
                        ret.Ampl_dB = CalculationClass.LinInterpolation(TAO_left.Cordinate, TAO_right.Cordinate, FindElement, TAO_left.Ampl_dB, TAO_right.Ampl_dB);

                        #region рассчитываем фазу

                        double mn = 0;
                        if (Math.Abs(TAO_left.Phase_degree - TAO_right.Phase_degree) >= 250)
                        {
                            if (TAO_left.Phase_degree < TAO_right.Phase_degree)
                            {
                                mn -= 360;
                            }
                            else
                            {
                                mn += 360;
                            }
                        }

                       double tempPhase_degree= TAO_right.Phase_degree + mn;
                       tempPhase_degree = CalculationClass.LinInterpolation(TAO_left.Cordinate, TAO_right.Cordinate, FindElement, TAO_left.Phase_degree, tempPhase_degree);
                       tempPhase_degree -= mn;

                       ret.Phase_degree = tempPhase_degree;

                        #endregion

                        #endregion

                       break;
                    }
                case FindResultEnum.Coincidence:
                    {
                        ret = CoincidenceElement as ResultElementClass;
                        break;
                    }
                case FindResultEnum.FindOnlyLeft:
                    {
                        ret = left as ResultElementClass;
                        ret.Cordinate = FindElement;

                        break;
                    }
                case FindResultEnum.FindOnlyRight:
                    {
                        ret = right as ResultElementClass;
                        ret.Cordinate = FindElement;

                        break;
                    }
                case FindResultEnum.Error:
                    {
                        needError = true;
                        ErrorText = "Ошибка поиска координаты. Массив координат пустой либо неизвестная ошибка";
                        break;
                    }
            }

            if (needError)
            {
                throw new Exception(ErrorText);
            }

            return ret;
        }

        public static FrequencyElementClass NormalizationFreqElement(FrequencyElementClass FrequencyElement)
        {
            List<PointDouble> dataAmpl = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
            dataAmpl= NormalizationFreqElement(dataAmpl);

            FrequencyElementClass ret = new FrequencyElementClass(FrequencyElement.Frequency);
           

            for (int i = 0; i < dataAmpl.Count; i++)
            {
                ret.ResultAmpl_PhaseElements.Add(new ResultElementClass(FrequencyElement.ResultAmpl_PhaseElements[i].Cordinate, dataAmpl[i].Y, FrequencyElement.ResultAmpl_PhaseElements[i].Phase_degree));
            }

            return ret;
        }

        #region Функции нормализации

        public static List<PointDouble> NormalizationFreqElement(List<PointDouble> dataAmpl)
        {
            PointDouble Max2_Ampl;
            PointDouble Max_LeftAMPL;
            PointDouble Max_RightAMPL;

            int Max_AMPL_index = -1;
            int Max_leftindex = -1;
            int Max_rightindex = -1;
            double WidthDN = 0;


            PointDouble MaxPoint = CalculationClass.CalculationMainMax(dataAmpl, out Max_LeftAMPL, out Max_RightAMPL, out Max2_Ampl, 0, out Max_AMPL_index, out Max_leftindex, out Max_rightindex, out WidthDN);


            return NormalizationFreqElement(dataAmpl, MaxPoint.Y);


        }

        public static List<PointDouble> NormalizationFreqElement(List<PointDouble> dataAmpl, double MaxAmpl)
        {
            for (int i = 0; i < dataAmpl.Count; i++)
            {
                dataAmpl[i].Y -= MaxAmpl;
            }

            return dataAmpl;
        }

        #endregion

        #endregion

        public override string ToString()
        {
            string ret = Math.Round(this.Frequency, 2).ToString();

            if(this.IsHideFrequency)
            {
                ret = "№ " + ret;
            }
            else
            {
                ret += " МГц";
            }

            return ret;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
