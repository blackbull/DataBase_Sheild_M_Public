using System;
using System.Collections.Generic;
using System.Text;

using ResultOptionsClassLibrary;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// абстрактный класс для всех рассчитываемых результатов
    /// </summary>
    [Serializable]
    public class CalculationResultsClass : ICalculationResults,ICloneable
    {
        #region конструкторы

        public CalculationResultsClass() { }
        
        #endregion
      

        #region основные переменные

        #region группа ДН

        public double Коэффициент_усиления_в_максимуме_диаграммы_направленности=double.NaN;
        public double Направление_максимума_диаграммы_направленности = double.NaN;
        public double Ширина_диаграммы_направленности_по_половине_мощности = double.NaN;
        public double Уровень_боковых_лепестков = double.NaN;
        public double Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности = double.NaN;
        public double Коэффициент_усиления_в_минимуме_диаграммы_направленности = double.NaN;

        #endregion


        #region группа ПХ

        public ComplexClass Поляризационное_отношение = new ComplexClass(double.NaN, double.NaN);
        public double Коэффициент_Эллиптичности = double.NaN;
        public double Угол_наклона_эллипса_поляризации = double.NaN;
        public double Отношение_MaxMin = double.NaN;


        #endregion


        public PointDouble Координаты_фазового_центра_Decart = new PointDouble(double.NaN,double.NaN);
        public PointDouble Координаты_фазового_центра_Polar = new PointDouble(double.NaN, double.NaN);

        #region Погрешности

        public MistakeClass deltaMistakeFull = new MistakeClass(double.NaN);
        public MistakeClass delta_Fo = new MistakeClass(double.NaN);
        public MistakeClass delta_M = new MistakeClass(double.NaN);
        public MistakeClass delta_Фо = new MistakeClass(double.NaN);

        public MistakeClass delta_O_max = new MistakeClass(double.NaN);
        public MistakeClass delta_Y_05 = new MistakeClass(double.NaN);

        #endregion
        
        #endregion

        #region расчётные статичные функции

        #region Функции рассчёта всего

        /// <summary>
        /// рассчитать все параметры для ДН
        /// </summary>
        /// <returns></returns>
        public static void Calculate_DN_Part(FrequencyElementClass FrequencyElement, ref CalculationResultsClass ret)
        {
            if(ret==null)
                ret = new CalculationResultsClass();

            if (FrequencyElement != null)
            {
                if (FrequencyElement.ResultAmpl_PhaseElements.Count >= 1)
                {
                    //if (FrequencyElement.ResultAmpl_PhaseElements.Count >= 2)
                    //{
                    List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                    PointDouble MaxAMPL;
                    PointDouble Max2_Ampl;
                    PointDouble Max_LeftAMPL;
                    PointDouble Max_RightAMPL;

                    PointDouble MinAMPL;
                    PointDouble MinAMPL2;
                    int MinAMPL_index;
                    

                    int Max_AMPL_index = -1;
                    int Max_leftindex = -1;
                    int Max_rightindex = -1;
                    double WidthDN = 0;

                    // используется только для поиска минимального значения MinAMPL
                    FindMinMax(FrequencyElement, out MaxAMPL, out MinAMPL, out Max_AMPL_index, out MinAMPL_index, out Max2_Ampl, out MinAMPL2);

                    MaxAMPL = CalculationClass.CalculationMainMax(dataAmpl, out Max_LeftAMPL, out Max_RightAMPL, out Max2_Ampl, Smoothing, out Max_AMPL_index, out Max_leftindex, out Max_rightindex, out WidthDN);
                    
                    if (Max_AMPL_index >= 0)
                    {
                        #region группа ДН

                        ret.Коэффициент_усиления_в_максимуме_диаграммы_направленности = Calculate_Коэффициент_усиления_в_максимуме_диаграммы_направленности(MaxAMPL);
                        ret.Направление_максимума_диаграммы_направленности = Calculate_Направление_максимума_диаграммы_направленности(MaxAMPL);
                        ret.Ширина_диаграммы_направленности_по_половине_мощности = WidthDN;
                        ret.Уровень_боковых_лепестков = Calculate_Уровень_боковых_лепестков(MaxAMPL, Max2_Ampl);
                        ret.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности = Calculate_Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности(MaxAMPL, Max2_Ampl);
                        ret.Отношение_MaxMin = Calculate_Отношение_MaxMin(MaxAMPL.Y, MinAMPL.Y);
                        ret.Коэффициент_усиления_в_минимуме_диаграммы_направленности = MinAMPL.Y;


                        #endregion

                        #region фазовый центр

                        ret.Координаты_фазового_центра_Polar = Convert_Координаты_фазового_центра_To_Polar(ret.Координаты_фазового_центра_Decart);

                        #endregion
                    }
                    //}
                }
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }
            //return ret;
        }

        /// <summary>
        /// рассчитать все параметры для ДН
        /// </summary>
        /// <returns></returns>
        public static void Calculate_DN_Part(IList<FrequencyElementClass> FrequencyElement)
        {
            foreach (FrequencyElementClass fe in FrequencyElement)
            {
                Calculate_DN_Part(fe);
            }
        }

        /// <summary>
        /// рассчитать все параметры для ДН
        /// </summary>
        /// <returns></returns>
        public static void Calculate_DN_Part(FrequencyElementClass FrequencyElement)
        {
            Calculate_DN_Part(FrequencyElement, ref FrequencyElement._CalculationResults);
        }

         /// <summary>
        /// рассчитать delta_O_max delta_Y_05
        /// </summary>
        /// <returns></returns>
        public static void Calculate_PH_Part(double delta_Fo, ref CalculationResultsClass ret)
        {
            if (ret == null)
                ret = new CalculationResultsClass();

            ret.delta_O_max = new MistakeClass(Calculate_delta_O_max(delta_Fo));
            ret.delta_Y_05 = new MistakeClass(Calculate_delta_Y_05(delta_Fo));
        }

        /// <summary>
        /// рассчитать delta_O_max delta_Y_05
        /// </summary>
        /// <returns></returns>
        public static void Calculate_PH_Part(IList<FrequencyElementClass> FrequencyElement)
        {
            foreach (FrequencyElementClass fe in FrequencyElement)
            {
                Calculate_PH_Part(fe._CalculationResults.delta_Fo.Mistake, ref fe._CalculationResults);
            }
        }

        /// <summary>
        /// рассчитать delta_O_max delta_Y_05
        /// </summary>
        /// <returns></returns>
        public static void Calculate_PH_Part(FrequencyElementClass FrequencyElement)
        {
            Calculate_PH_Part(FrequencyElement._CalculationResults.delta_Fo.Mistake, ref FrequencyElement._CalculationResults);
        }

        public static void CalculateAll_NO_Constant(IList<FrequencyElementClass> FrequencyElement)
        {
            Calculate_PH_Part(FrequencyElement);
            Calculate_DN_Part(FrequencyElement);
        }

        public static void CalculateAll_NO_Constant(FrequencyElementClass FrequencyElement)
        {
            Calculate_PH_Part(FrequencyElement);
            Calculate_DN_Part(FrequencyElement);
        }

         /// <summary>
        /// рассчитать все зависимые от частоты параметры
        /// </summary>
        /// <returns></returns>
        public static void CalculateAllConstant(FrequencyElementClass FrequencyElement, IResultType_MAIN ResultForCalculation, ref CalculationResultsClass ret)
        {
            if (ret == null)
                ret = new CalculationResultsClass();

            ret.Поляризационное_отношение = Calculate_Поляризационное_отношение(FrequencyElement, ResultForCalculation);
            ret.Коэффициент_Эллиптичности = Calculate_Коэффициент_Эллиптичности(FrequencyElement, ResultForCalculation, ret.Поляризационное_отношение);
            ret.Угол_наклона_эллипса_поляризации = Calculate_Угол_наклона_эллипса_поляризации(ret.Поляризационное_отношение);


            double MistakeFull;
            double delta_M = double.NaN;
            double delta_Fo = double.NaN;
            double delta_Фо = double.NaN;

            MistakeFull = Calculate_Mistakes(ResultForCalculation, FrequencyElement, ret.Поляризационное_отношение.Amplitude, out delta_M, out delta_Fo, out delta_Фо);

            ret.deltaMistakeFull = new MistakeClass(MistakeFull);
            ret.delta_Fo = new MistakeClass(delta_Fo);
            ret.delta_M = new MistakeClass(delta_M);
            ret.delta_Фо = new MistakeClass(delta_Фо);

            ret.Координаты_фазового_центра_Decart = Calculate_Координаты_фазового_центра(FrequencyElement, out ret.Координаты_фазового_центра_Polar);
            
        }

        #endregion

        #region вспомогательные статические функции

        public static List<PointDouble> GetListPoint_Ampl_FromFrequencyElement(FrequencyElementClass FrequencyElement)
        {
            List<PointDouble> ret = new List<PointDouble>();

            if (FrequencyElement != null)
            {
                foreach (ResultElementClass re in FrequencyElement.ResultAmpl_PhaseElements)
                {
                    ret.Add(new PointDouble(re.Cordinate, re.Ampl_dB));
                }
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static List<PointDouble> GetListPoint_Phase_FromFrequencyElement(FrequencyElementClass FrequencyElement)
        {
            List<PointDouble> ret = new List<PointDouble>();

            if (FrequencyElement != null)
            {
                foreach (ResultElementClass re in FrequencyElement.ResultAmpl_PhaseElements)
                {
                    ret.Add(new PointDouble(re.Cordinate, re.Phase_degree));
                }
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        /// <summary>
        /// сглаживание графика при рассчётах (игнорирование выбивающихся точек); 
        /// 0 - без сглаживания; 1 - пропускаем выбившуюся одну точку; 2 - пропускаем выбившуюся две точки и тд; 
        /// </summary>
        public static uint Smoothing = 2;

        /// <summary>
        /// Рассчитать общую погрешность позиционирования установки
        /// </summary>
        /// <param name="TA_Options"></param>
        /// <param name="UH"></param>
        /// <returns></returns>
        public static double Calculate_deltaFY(TA_OptionsClass TA_Options, AdjustOptionsClass UH)
        {
            double DN_WidthRad = CalculationClass.Convert_To_Radian_From_Grad(TA_Options.DN_WidthTA);

            double deltaANGL = CalculationClass.Mistake_delta_angl(DN_WidthRad, UH.delta_Y, UH.delta_W, UH.delta_W_ort, UH.delta_Y1, UH.delta_W1, UH.delta_W1_ort);

            double deltaLine = CalculationClass.Mistake_delta_line(DN_WidthRad, UH.delta_XY, UH.delta_WW1_a, UH.delta_WW1_l, UH.R);

            double deltaPOS = CalculationClass.Mistake_delta_poz(DN_WidthRad, UH.delta_dY, UH.delta_pY, UH.delta_dW, UH.delta_pW, UH.delta_dW1, UH.delta_pW1, UH.delta_dY1, UH.delta_pY1, UH.R);

            double deltaY = CalculationClass.Mistake_delta_y(deltaANGL, deltaLine, deltaPOS);

            double deltaFY = CalculationClass.Mistake_delta_Fy(deltaY);

            return deltaFY;
        }

        #endregion
        
        #region группа ДН

        #region Коэффициент_усиления_в_максимуме_диаграммы_направленности

        public static double Calculate_Коэффициент_усиления_в_максимуме_диаграммы_направленности(FrequencyElementClass FrequencyElement)
        {
            double ret = double.NaN;
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing);

                ret = Calculate_Коэффициент_усиления_в_максимуме_диаграммы_направленности(Max);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static double Calculate_Коэффициент_усиления_в_максимуме_диаграммы_направленности(PointDouble Max)
        {
            return Max.Y;
        }

        #endregion

        #region Направление_максимума_диаграммы_направленности

        public static double Calculate_Направление_максимума_диаграммы_направленности(FrequencyElementClass FrequencyElement)
        {
            double ret = double.NaN;
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing);

                ret = Calculate_Направление_максимума_диаграммы_направленности(Max);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static double Calculate_Направление_максимума_диаграммы_направленности(PointDouble Max)
        {
            return Max.X;
        }

        #endregion

        #region Ширина_диаграммы_направленности_по_половине_мощности

        public static double Calculate_Ширина_диаграммы_направленности_по_половине_мощности(FrequencyElementClass FrequencyElement)
        {
            double ret = double.NaN;
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing);

                ret = Calculate_Ширина_диаграммы_направленности_по_половине_мощности(Left, Right);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static double Calculate_Ширина_диаграммы_направленности_по_половине_мощности(PointDouble Left, PointDouble Right)
        {
            return Math.Abs(Right.X - Left.X);
        }

        #endregion

        #region Уровень_боковых_лепестков

        public static double Calculate_Уровень_боковых_лепестков(FrequencyElementClass FrequencyElement)
        {
            double ret = double.NaN;
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing);

                ret = Calculate_Уровень_боковых_лепестков(Max, Max2);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static double Calculate_Уровень_боковых_лепестков(PointDouble Max, PointDouble Max2)
        {
           // return Math.Abs(Max.Y) -  Math.Abs(Max2.Y);
            return Max2.Y - Max.Y;
        }

        #endregion

        #region Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности

        public static double Calculate_Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности(FrequencyElementClass FrequencyElement)
        {
            double ret = double.NaN;
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing);

                ret = Calculate_Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности(Max, Max2);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static double Calculate_Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности(PointDouble Max, PointDouble Max2)
        {
            return Max2.X - Max.X;
        }

        #endregion

        #endregion

        #region группа ПХ

        #region Поляризационное_отношение

        public static ComplexClass Calculate_Поляризационное_отношение(FrequencyElementClass FrequencyElement, IResultType_MAIN result)
        {
            ComplexClass ret = new ComplexClass();
            if (FrequencyElement != null)
            {

                if (result is IResultType_КУ)
                {
                    IResultType_КУ tempKY = result as IResultType_КУ;

                    //ищем значения ку в двух поляризациях по частоте
                    ResultElementClass GainIAmain = tempKY.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0];
                    ResultElementClass GainIAcros = tempKY.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0];

                    //добавить значение фазы для КУ
                    ComplexClass PolarizRelation = CalculationClass.CalculatePolarizationRelation(GainIAmain.Ampl_dB, GainIAcros.Ampl_dB, GainIAmain.Phase_degree, GainIAcros.Phase_degree);
                    ret = PolarizRelation;

                }
                else
                {
                    if (result is IResultType_ПХ)
                    {
                        double MaxAmpl;
                        double MinAmpl;
                        double MaxPhase;
                        double MinPhase;
                        int MaxIndex;
                        int MinIndex;

                        //ищем максимальные и минимальные элементы
                        if (FindMinMax(FrequencyElement, out MaxAmpl, out MinAmpl, out MaxPhase, out MinPhase, out MaxIndex, out MinIndex))
                        {
                            ComplexClass PolarizRelation = CalculationClass.CalculatePolarizationRelation(MaxAmpl, MinAmpl, MaxPhase, MinPhase);
                            ret = PolarizRelation;
                        }
                    }
                    else
                    {
                        if (result is IResultType_СДНМ)
                        {
                            IResultType_СДНМ tempСДНМ = result as IResultType_СДНМ;

                            FrequencyElementClass SUM = tempСДНМ.SUM_Polarization.FindFrequencyElement(FrequencyElement.Frequency);
                            FrequencyElementClass main = tempСДНМ.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency);
                            FrequencyElementClass cross = tempСДНМ.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency);

                            double MaxAmpl;
                            double MinAmpl;
                            double MaxPhase;
                            double MinPhase;
                            int MaxIndex;
                            int MinIndex;

                            //ищем максимальные и минимальные элементы
                            if (FindMinMax(SUM, out MaxAmpl, out MinAmpl, out MaxPhase, out MinPhase, out MaxIndex, out MinIndex))
                            {
                                ResultElementClass MainData = main.ResultAmpl_PhaseElements[MaxIndex];
                                ResultElementClass CrossData = cross.ResultAmpl_PhaseElements[MaxIndex];

                                ComplexClass PolarizRelation = CalculationClass.CalculatePolarizationRelation(MainData.Ampl_dB, CrossData.Ampl_dB, MainData.Phase_degree, CrossData.Phase_degree);
                                ret = PolarizRelation;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Trace.TraceError("Попытка рассчета Поляризационное_отношение не подходящего типа измерения, CalculationResultsClass");
                            ret.Real = double.NaN;
                            ret.Imaginary = double.NaN;
                        }
                    }
                }

            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }
                

        protected static bool FindMinMax(FrequencyElementClass FrequencyElement, out double MaxAmpl, out  double MinAmpl, out  double MaxPhase, out double MinPhase, out int MaxAMPL_index, out int MinAMPL_index)
        {
            double max2;
            double min2;

            return FindMinMax(FrequencyElement, out  MaxAmpl, out   MinAmpl, out   MaxPhase, out  MinPhase, out  MaxAMPL_index, out  MinAMPL_index, out max2, out min2);
        }

        protected static bool FindMinMax(FrequencyElementClass FrequencyElement, out double MaxAmpl, out  double MinAmpl, out  double MaxPhase, out double MinPhase, out int MaxAMPL_index, out int MinAMPL_index, out double Max_2, out double Min_2)
        {
            bool ret = false;
            MaxAmpl = double.NaN;
            MinAmpl = double.NaN;
            MaxPhase = double.NaN;
            MinPhase = double.NaN;
            Max_2 = double.NaN;
            Min_2 = double.NaN;

            MaxAMPL_index = -1;
            MinAMPL_index = -1;

            //временные переменные
            PointDouble MaxAmpl_temp;
            PointDouble MinAmpl_temp;
            PointDouble Max_2_temp;
            PointDouble Min_2_temp;

            ret = FindMinMax(FrequencyElement, out MaxAmpl_temp, out MinAmpl_temp, out MaxAMPL_index, out MinAMPL_index, out Max_2_temp, out Min_2_temp);

            //вывод необходимых значений
            Max_2 = Max_2_temp.Y;
            MaxAmpl = MaxAmpl_temp.Y;
            MinAmpl = MinAmpl_temp.Y;


            //массив для поиска значений фаз
            List<PointDouble> dataPhase = GetListPoint_Phase_FromFrequencyElement(FrequencyElement);

            MaxPhase = dataPhase[MaxAMPL_index].Y;
            MinPhase = dataPhase[MinAMPL_index].Y;

            #region поиск

            //List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
            //List<PointDouble> dataAmpl2ForMin = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
            //List<PointDouble> dataPhase = GetListPoint_Phase_FromFrequencyElement(FrequencyElement);

            //PointDouble MaxAMPL;
            //PointDouble Max2;
            //PointDouble Min2;
            //PointDouble Left;
            //PointDouble Right;

            //PointDouble MinAMPL;


            //int leftindex = -1;
            //int rightindex = -1;

            //MaxAMPL = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing, out MaxAMPL_index, out leftindex, out rightindex);
            //Max_2 = Max2.Y;

            //if (MaxAMPL_index >= 0)
            //{
            //    //ищем минимальный элемент: для этого переворачиваем массив амплитуд
            //    for (int i = 0; i < dataAmpl2ForMin.Count; i++)
            //    {
            //        dataAmpl2ForMin[i].Y = -dataAmpl2ForMin[i].Y;
            //    }

            //    //инвертируем полученый элемент, тк мы инвертировали все входные
            //    MinAMPL = CalculationClass.CalculationMainMax(dataAmpl2ForMin, out Left, out Right, out Min2, Smoothing, out MinAMPL_index, out leftindex, out rightindex);
            //    MinAMPL.Y = -MinAMPL.Y;

            //    MaxAmpl = MaxAMPL.Y;
            //    MinAmpl = MinAMPL.Y;
            //    MaxPhase = dataPhase[MaxAMPL_index].Y;
            //    MinPhase = dataPhase[MinAMPL_index].Y;

            //    Min_2 = -Min2.Y;

            //    ret = true;
            // }

            #endregion

            return ret;
        }

        protected static bool FindMinMax(FrequencyElementClass FrequencyElement, out PointDouble MaxAmpl, out  PointDouble MinAmpl, out int MaxAMPL_index, out int MinAMPL_index, out PointDouble Max_2, out PointDouble Min_2)
        {
            bool ret = false;
            MaxAmpl = null;
            MinAmpl = null;
            Max_2 = null;
            Min_2 = null;
            MaxAMPL_index = -1;
            MinAMPL_index = -1;


            #region поиск

            List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
            List<PointDouble> dataAmpl2ForMin = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
           

            //временные переменные
            PointDouble Left;
            PointDouble Right;
            int leftindex = -1;
            int rightindex = -1;

            MaxAmpl = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max_2, Smoothing, out MaxAMPL_index, out leftindex, out rightindex);


            if (MaxAMPL_index >= 0)
            {
                //ищем минимальный элемент: для этого переворачиваем массив амплитуд
                for (int i = 0; i < dataAmpl2ForMin.Count; i++)
                {
                    dataAmpl2ForMin[i].Y = -dataAmpl2ForMin[i].Y;
                }


                MinAmpl = CalculationClass.CalculationMainMax(dataAmpl2ForMin, out Left, out Right, out Min_2, Smoothing, out MinAMPL_index, out leftindex, out rightindex);


                //инвертируем полученый элемент, тк мы инвертировали все входные

                double MinY = -MinAmpl.Y;
                double MinY2 = -Min_2.Y;

                MinAmpl.Y = MinY;
                Min_2.Y = MinY2;   

                ret = true;
            }


            #endregion

            return ret;
        }

        #endregion

        #region Коэффициент_Эллиптичности

        public static double Calculate_Коэффициент_Эллиптичности(FrequencyElementClass FrequencyElement, IResultType_MAIN result, ComplexClass pr)
        {
            double ret = double.NaN;

            if (FrequencyElement != null)
            {
                if (result is IResultType_КУ || result is IResultType_СДНМ || result is IResultType_ПХ)
                {
                    //направление вращения true - увеличение координаты по W'
                    bool moving = false;
                    bool sign = false;
                    if (result.MainOptions.Parameters.StartTower_W <= result.MainOptions.Parameters.StopTower_W)
                    {
                        moving = true;
                    }

                    sign = CalculationClass.sign(pr);

                    bool signMia = true;
                    if (moving == sign)
                    {
                        signMia = false;
                    }
                    
                    double Mta = result.Zond.FindOrCalculate_TA_Options_By_Frequency(FrequencyElement.Frequency).MTA;

                    //ищем значения main cross в зависимости от типа измерения

                    if (result is IResultType_КУ)
                    {
                        IResultType_КУ tempRes = result as IResultType_КУ;

                        //ищем значения ку в двух поляризациях по частоте
                        double GainIAmain = tempRes.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0].Ampl_dB;
                        double GainIAcros = tempRes.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0].Ampl_dB;

                        ret = CalculationClass.LevelCrossPolarization(GainIAmain, GainIAcros, Mta, signMia);

                        ret = CalculationClass.Convert_Unity_to_dB(ret);
                    }
                    else
                    {
                        if (result is IResultType_СДНМ)
                        {
                            IResultType_СДНМ tempRes = result as IResultType_СДНМ;

                            //ищем значения ку в двух поляризациях по частоте

                            FrequencyElementClass SUM = tempRes.SUM_Polarization.FindFrequencyElement(FrequencyElement.Frequency);
                            FrequencyElementClass main = tempRes.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency);
                            FrequencyElementClass cross = tempRes.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency);

                            double MaxAmpl;
                            double MinAmpl;
                            double MaxPhase;
                            double MinPhase;
                            int MaxIndex;
                            int MinIndex;

                            //ищем максимальные и минимальные элементы
                            if (FindMinMax(SUM, out MaxAmpl, out MinAmpl, out MaxPhase, out MinPhase, out MaxIndex, out MinIndex))
                            {
                                ResultElementClass MainData = main.ResultAmpl_PhaseElements[MaxIndex];
                                ResultElementClass CrossData = cross.ResultAmpl_PhaseElements[MaxIndex];

                                double GainIAmain = MainData.Ampl_dB;
                                double GainIAcros = CrossData.Ampl_dB;

                                ret = CalculationClass.LevelCrossPolarization(GainIAmain, GainIAcros, Mta, signMia);

                                ret = CalculationClass.Convert_Unity_to_dB(ret);
                            }
                        }
                        else
                        {
                            if (result is IResultType_ПХ)
                            {
                                IResultType_ПХ tempRes = result as IResultType_ПХ;


                                double MaxAmpl;
                                double MinAmpl;
                                double MaxPhase;
                                double MinPhase;
                                int MaxIndex;
                                int MinIndex;
                                double max2;
                                double min2;

                                //ищем максимальные и минимальные элементы
                                if (FindMinMax(FrequencyElement, out MaxAmpl, out MinAmpl, out MaxPhase, out MinPhase, out MaxIndex, out MinIndex,out max2,out min2))
                                {
                                    //ret = CalculationClass.LevelCrossPolarization(MinAmpl, min2, MaxAmpl, max2, Mta, signMia);
#warning изменён расчёт
                                    ret = CalculationClass.LevelCrossPolarization(MinAmpl, MaxAmpl, Mta, signMia);


                                    ret = CalculationClass.Convert_Unity_to_dB(ret);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }
            return ret;
        }

        public static double Calculate_Коэффициент_Эллиптичности(FrequencyElementClass FrequencyElement, IResultType_MAIN result)
        {
            ComplexClass pr = Calculate_Поляризационное_отношение(FrequencyElement, result);

            return Calculate_Коэффициент_Эллиптичности(FrequencyElement, result, pr);
        }

        #endregion

        #region Угол_наклона_эллипса_поляризации

        public static double Calculate_Угол_наклона_эллипса_поляризации(ComplexClass prIA)
        {
            double ret = double.NaN;

            ret = CalculationClass.LevelInclination(prIA);

            return ret;
        }

        public static double Calculate_Угол_наклона_эллипса_поляризации(FrequencyElementClass FrequencyElement, IResultType_MAIN result)
        {
            ComplexClass pr = Calculate_Поляризационное_отношение(FrequencyElement, result);
            return Calculate_Угол_наклона_эллипса_поляризации(pr);
        }

        #endregion

        #region отношение MaxMin

        public static Double Calculate_Отношение_MaxMin(double MaxAmpl, double MinAmpl)
        {
            return MinAmpl - MaxAmpl;
        }

        #endregion

        #endregion

        #region фазовый центр

        public static PointDouble Calculate_Координаты_фазового_центра(FrequencyElementClass FrequencyElement,out PointDouble PolarCordinat)
        {
            PointDouble ret = new PointDouble(double.NaN, double.NaN);
            PolarCordinat = new PointDouble(double.NaN, double.NaN);
            if (FrequencyElement != null)
            {
                List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);
                List<PointDouble> dataPhase = GetListPoint_Phase_FromFrequencyElement(FrequencyElement);

                PointDouble Max;
                PointDouble Max2;
                PointDouble Left;
                PointDouble Right;
                int MaxIndex;
                int LeftIndex;
                int RightIndex;

                Max = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing, out MaxIndex, out LeftIndex, out RightIndex);

                ret = Calculate_Координаты_фазового_центра(FrequencyElement, MaxIndex, LeftIndex, RightIndex, out PolarCordinat);
            }
            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }


        public static PointDouble Calculate_Координаты_фазового_центра(FrequencyElementClass FrequencyElement, int MaxIndex, int LeftIndex, int RightIndex, out PointDouble PolarCordinat)
        {
            PointDouble ret = Calculate_Координаты_фазового_центра(FrequencyElement, MaxIndex, LeftIndex, RightIndex);
            PolarCordinat = Convert_Координаты_фазового_центра_To_Polar(ret);

            return ret;
        }

        public static PointDouble Calculate_Координаты_фазового_центра(FrequencyElementClass FrequencyElement, int MaxIndex, int LeftIndex, int RightIndex)
        {
            PointDouble ret = new PointDouble(double.NaN, double.NaN);

            if (FrequencyElement != null)
            {
                if (MaxIndex >= 0)
                {
                    List<PointDouble> dataPhase = GetListPoint_Phase_FromFrequencyElement(FrequencyElement);

                    dataPhase = CalculationClass.RestorePhase(dataPhase, MaxIndex);
                    dataPhase = CalculationClass.Convert_To_Radian_From_Grad(dataPhase);

                    //обрезаем фазовую диаграмму по ширине диаграммы направленности
                    dataPhase = CalculationClass.CutByIndexes(dataPhase, LeftIndex, RightIndex);


                    double R = SheildM_GlobalParametrsClass.AdjustOptions.R;
                    dataPhase = CalculationClass.ConvertPhase(dataPhase, FrequencyElement.Frequency * 1000000, R);

                    ret = CalculationClass.GetPhaseCentre_ByLocalMaxMetod(dataPhase);
                }
            }

            else
            {
                new NullReferenceException("FrequencyElement==null");
            }

            return ret;
        }

        public static PointDouble Convert_Координаты_фазового_центра_To_Polar(PointDouble DecartCordinat)
        {
            PointDouble ret = new PointDouble(double.NaN, double.NaN);

            //делаем перевод в полярные значения
            ret = CalculationClass.Convert_To_Polar_From_Decart(DecartCordinat);
            ret.X = CalculationClass.Convert_To_Grad_From_Radian(ret.X);

            return ret;
        }


        #endregion


        #region погрешности

       /// <summary>
        /// рассчитать погрешность измерения
       /// </summary>
        /// <param name="tempResult">результат измерения (Обязательно с исходными данными от анализатора, без перерассчёта)</param>
        /// <param name="FrequencyElement">Частотный элемент из этого измерения</param>
        /// <param name="prIA_Ampl">поляризационное отношение на этой частоте (амплитуда)</param>
       /// <param name="delta_M"></param>
       /// <param name="delta_Fo"></param>
       /// <param name="delta_Фо"></param>
       /// <returns></returns>
        public static double Calculate_Mistakes(IResultType_MAIN tempResult, FrequencyElementClass FrequencyElement, double prIA_Ampl, out double delta_M, out double delta_Fo, out double delta_Фо)
        {
            double ret = double.NaN;
            delta_M = double.NaN;
            delta_Fo = double.NaN;
            delta_Фо = double.NaN;

            string ErrorText = "";

            try
            {

                #region Загрузка общих параметров


                //параметры безэховости
                ErrorText = "Невозможно рассчитать параметры безэховости, CalculationResultsClass";
                AnechoicClass Anechoic = SheildM_GlobalParametrsClass.AnechoicChamber.FindOrCalculate_Anechoic_Options_By_Frequency(FrequencyElement.Frequency);



                //параметры ТА
                ErrorText = "Невозможно рассчитать параметры ТА, CalculationResultsClass";
                TA_OptionsClass TA_Options = tempResult.Zond.FindOrCalculate_TA_Options_By_Frequency(FrequencyElement.Frequency);



                //параметры кабеля к ТА
                ErrorText = "Невозможно рассчитать параметры кабеля ТА, CalculationResultsClass";
                CableOptionsClass CableTA = SheildM_GlobalParametrsClass.Cables_Sheild_M.TA_Cable.FindOrCalculate_Cable_Options_By_Frequency(FrequencyElement.Frequency);
                //параметры кабеля к ИА
                ErrorText = "Невозможно рассчитать параметры кабеля ИА, CalculationResultsClass";
                CableOptionsClass CableIA = SheildM_GlobalParametrsClass.Cables_Sheild_M.IA_Cable.FindOrCalculate_Cable_Options_By_Frequency(FrequencyElement.Frequency);

                //юстировочные характеристики
                ErrorText = "Невозможно рассчитать параметры юстировочные характеристики, CalculationResultsClass";
                AdjustOptionsClass UH = SheildM_GlobalParametrsClass.AdjustOptions;

                ErrorText = "Невозможно рассчитать погрешности, CalculationResultsClass";

                double delta_Mta = TA_Options.delta_MTA;

                double Mta = TA_Options.MTA;

                double delta_R = CalculationClass.Mistake_delta_R(UH.abs_delta_R, UH.R);

                double delta_P = Anechoic.delta_Ro;

                //ищем погрешность попадания в ворота
                double DN_WidthRad = CalculationClass.Convert_To_Radian_From_Grad(TA_Options.DN_WidthTA);

                double delta_T = CalculationClass.Mistake_delta_t(Convert.ToDouble(tempResult.MainOptions.Parameters.VOROTA), DN_WidthRad);

                #endregion

                #region рассчёт общих параметров (не зависящих от поляризации)

                //погрешность рассогласования ИА, занулено одно значение, тк у ИА оно неизвестно
                double deltaГIA = CalculationClass.Mistake_delta_Г(0, CableIA.Gamma_Cab, CableIA.Loss);

                //погрешность рассогласования ТА
                double deltaГTA = CalculationClass.Mistake_delta_Г(TA_Options.GammaTA, CableTA.Gamma_Cab, CableTA.Loss);

                //общая погрешность системы позиционирования
                double delta_FY = Calculate_deltaFY(TA_Options, UH);


                ZVB_Parametrs_ElementClass zvbParamForCableIA = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(CableIA.Frequency, CableIA.Loss);
                ZVB_Parametrs_ElementClass zvbParamForCableTA = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(CableTA.Frequency, CableTA.Loss);

                double deltaLia = CalculationClass.Mistake_delta_S21(zvbParamForCableIA.delta_Ampl);
                double deltaLta = CalculationClass.Mistake_delta_S21(zvbParamForCableTA.delta_Ampl);

                #endregion

                #region рассчёт по нескольким поляризациям

                //рассчёт одинаковый для этих типов измерений
                if (tempResult is IResultType_КУ || tempResult is IResultType_СДНМ || tempResult is IResultType_ПХ)
                {
                    //рассчитываем погрешность кроссполяризационной компоненты
                    double delta_KPE_2 = CalculationClass.Mistake_delta_KPE(prIA_Ampl, Mta);


                    #region Рассчёт для ПХ

                    if (tempResult is IResultType_ПХ)
                    {
                        IResultType_ПХ tempRes = tempResult as IResultType_ПХ;

                        #region поиск максимумов в main cross

                        List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                        PointDouble MaxAMPL;
                        PointDouble Left;
                        PointDouble Right;
                        PointDouble Max2;
                        int MaxAMPL_index;
                        int leftindex;
                        int rightindex;

                        MaxAMPL = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing, out MaxAMPL_index, out leftindex, out rightindex);

                        double Phasemain = FrequencyElement.ResultAmpl_PhaseElements[MaxAMPL_index].Phase_degree;
                        double Amplmain = MaxAMPL.Y;

                        #endregion

                        //ищем погрешности zvb по частоте и амплитуде
                        ZVB_Parametrs_ElementClass delta_zvb_ForMain = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(FrequencyElement.Frequency, Amplmain);

                        //рассчитываем общую погрешность
                        double delta_Fф = CalculationClass.Mistake_delta_Fф(delta_zvb_ForMain.delta_Ampl, delta_KPE_2, delta_FY, delta_T, delta_P);

                        ret = delta_Fф;

                        //рассчитать погрешность степени кросс поляризации
                        delta_M = CalculationClass.Mistake_delta_M_for_PH(delta_Fф, TA_Options.delta_MTA);
                    }

                    #endregion

                    #region Рассчёт для СДНМ или КУ

                    if (tempResult is IResultType_КУ || tempResult is IResultType_СДНМ)
                    {
                        #region загружаем статические данные для рассчёта (зависимые от поляризации)

                        //ищем значения ку в двух поляризациях по частоте                      -----------брать измеренные данные с анализатора-------------

                        #region поиск максимумов в main cross
                        double Amplmain = double.NaN;
                        double Amplcros = double.NaN;

                        if (tempResult is IResultType_СДНМ)
                        {
                            IResultType_СДНМ tempRes = tempResult as IResultType_СДНМ;

                            FrequencyElementClass freqSum = tempRes.SUM_Polarization.FindFrequencyElement(FrequencyElement.Frequency);
                            List<PointDouble> dataAmplSUM = GetListPoint_Ampl_FromFrequencyElement(freqSum);

                            PointDouble MaxAMPL;
                            PointDouble Left;
                            PointDouble Right;
                            PointDouble Max2;
                            int MaxAMPL_index;
                            int leftindex;
                            int rightindex;

                            MaxAMPL = CalculationClass.CalculationMainMax(dataAmplSUM, out Left, out Right, out Max2, Smoothing, out MaxAMPL_index, out leftindex, out rightindex);


                            Amplmain = tempRes.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[MaxAMPL_index].Ampl_dB;
                            Amplcros = tempRes.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[MaxAMPL_index].Ampl_dB;
                        }
                        else
                        {
                            if (tempResult is IResultType_КУ)
                            {
                                IResultType_КУ tempRes = tempResult as IResultType_КУ;

                                Amplmain = tempRes.Main_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0].Ampl_dB;
                                Amplcros = tempRes.Cross_Polarization.FindFrequencyElement(FrequencyElement.Frequency).ResultAmpl_PhaseElements[0].Ampl_dB;
                            }
                        }

                        #endregion

                        //ищем погрешности zvb по частоте и амплитуде
                        ZVB_Parametrs_ElementClass delta_zvb_ForMain = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(FrequencyElement.Frequency, Amplmain);
                        ZVB_Parametrs_ElementClass delta_zvb_ForCross = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(FrequencyElement.Frequency, Amplcros);

                        #endregion

                        //рассчитываем погрешность ZVB
                        double deltaZVB_2 = CalculationClass.Mistake_delta_ZVB(delta_zvb_ForMain.delta_Freq, delta_zvb_ForMain.delta_Ampl, delta_zvb_ForCross.delta_Ampl, prIA_Ampl);

                        ///общая погрешность системы измерений
                        double deltaINSTR_2 = CalculationClass.Mistake_delta_instr(TA_Options.delta_GainTA, delta_KPE_2, deltaLia, deltaLta, deltaГIA, deltaГTA, delta_FY, delta_T);

                        ///рассчитываем погрешность КУ
                        double deltaGainFull = CalculationClass.Mistake_delta_G(deltaZVB_2, deltaINSTR_2, delta_R, delta_P);

                        ret = deltaGainFull;

                        //рассчитать погрешность степени кросс поляризации
                        delta_M = CalculationClass.Mistake_delta_M_for_KY(delta_zvb_ForMain.delta_Ampl, delta_zvb_ForCross.delta_Ampl, delta_KPE_2, delta_FY, delta_P, delta_Mta, prIA_Ampl);

                        //рассчитываем погрешность ДН
                        delta_Fo = CalculationClass.Mistake_delta_Fo(delta_zvb_ForMain.delta_Ampl, delta_zvb_ForCross.delta_Ampl, delta_KPE_2, delta_FY, delta_P, prIA_Ampl, delta_T);
                    }

                    #endregion
                }

                #endregion

                #region рассчёт для одной поляризации

                //рассчёт одинаковый для этих типов измерений
                if (tempResult is IResultType_ДН)
                {
                    IResultType_ДН tempRes = tempResult as IResultType_ДН;


                    #region загружаем статические данные для рассчёта (зависимые от поляризации)

                    //ищем значения ку в двух поляризациях по частоте                      -----------брать измеренные данные с анализатора-------------

                    #region поиск максимумов в main cross

                    List<PointDouble> dataAmpl = GetListPoint_Ampl_FromFrequencyElement(FrequencyElement);

                    PointDouble MaxAMPL;
                    PointDouble Left;
                    PointDouble Right;
                    PointDouble Max2;
                    int MaxAMPL_index;
                    int leftindex;
                    int rightindex;

                    MaxAMPL = CalculationClass.CalculationMainMax(dataAmpl, out Left, out Right, out Max2, Smoothing, out MaxAMPL_index, out leftindex, out rightindex);

                    double Phasemain = FrequencyElement.ResultAmpl_PhaseElements[MaxAMPL_index].Phase_degree;
                    double Amplmain = MaxAMPL.Y;

                    #endregion

                    //ищем погрешности zvb по частоте и амплитуде
                    ZVB_Parametrs_ElementClass delta_zvb_ForMain = SheildM_GlobalParametrsClass.ZVB14_Parametrs.Find_ZVB_Parametrs(FrequencyElement.Frequency, Amplmain);

                    #endregion

                    //рассчитываем погрешность ZVB
                    double deltaZVB_1 = CalculationClass.Mistake_delta_ZVB(delta_zvb_ForMain.delta_Freq, delta_zvb_ForMain.delta_Ampl);

                    //рассчитываем погрешность ДН
                    delta_Fo = CalculationClass.Mistake_delta_Fo(delta_zvb_ForMain.delta_Ampl, delta_FY, delta_P, delta_T);

                    //погрешность фазовой диаграммы
                    delta_Фо = CalculationClass.Mistake_delta_Фо(delta_zvb_ForMain.delta_Phase, delta_R, delta_FY, delta_P);

                    ///общая погрешность системы измерений
                    double deltaINSTR_1 = CalculationClass.Mistake_delta_instr(TA_Options.delta_GainTA, 0, deltaLia, deltaLta, deltaГIA, deltaГTA, delta_FY, delta_T);

                    double delta_DG = CalculationClass.Mistake_delta_G(deltaZVB_1, deltaINSTR_1, delta_R, delta_P);
                    ret = delta_DG;
                }

                #endregion

                //#region рассчёт общих погрешностей

                ////погрешность определения ширины ДН
                //delta_Y_05 = CalculationClass.Mistake_delta_Y_05(delta_Fo);

                ////погрешность определения положения максимума ДН
                //delta_O_max = CalculationClass.Mistake_delta_O_max(delta_Fo);

                //#endregion
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ErrorText + ex.Message);
            }

            return ret;
        }

        public static double Calculate_delta_Y_05(double delta_Fo)
        {
           return CalculationClass.Mistake_delta_Y_05(delta_Fo);
        }

        public static double Calculate_delta_O_max(double delta_Fo)
        {
            return CalculationClass.Mistake_delta_O_max(delta_Fo);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #endregion

        #region ICalculationResults

        PointDouble ICalculationResults.Координаты_фазового_центра_Decart
        {
            get { return this.Координаты_фазового_центра_Decart; }
        }

        PointDouble ICalculationResults.Координаты_фазового_центра_Polar
        {
            get { return this.Координаты_фазового_центра_Polar; }
        }

        double ICalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности
        {
            get { return this.Коэффициент_усиления_в_максимуме_диаграммы_направленности; }
        }

        double ICalculationResults.Коэффициент_усиления_в_минимуме_диаграммы_направленности
        {
            get { return this.Коэффициент_усиления_в_минимуме_диаграммы_направленности; }
        }

        double ICalculationResults.Коэффициент_Эллиптичности
        {
            get { return this.Коэффициент_Эллиптичности; }
        }

        double ICalculationResults.Направление_максимума_диаграммы_направленности
        {
            get { return this.Направление_максимума_диаграммы_направленности; }
        }

        ComplexClass ICalculationResults.Поляризационное_отношение
        {
            get { return this.Поляризационное_отношение; }
        }

        double ICalculationResults.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности
        {
            get { return this.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности; }
        }

        double ICalculationResults.Угол_наклона_эллипса_поляризации
        {
            get { return this.Угол_наклона_эллипса_поляризации; }
        }

        double ICalculationResults.Уровень_боковых_лепестков
        {
            get { return this.Уровень_боковых_лепестков; }
        }

        double ICalculationResults.Ширина_диаграммы_направленности_по_половине_мощности
        {
            get { return this.Ширина_диаграммы_направленности_по_половине_мощности; }
        }

        double ICalculationResults.Отношение_MaxMin
        {
            get { return this.Отношение_MaxMin; }
        }


        #region Погрешности по названиям

        public MistakeClass Погрешность_КУ
        {
            get
            {

                return this.deltaMistakeFull;
                ////+++++++++++++++++++Skiffartur 20141118
                //return this.delta_Fo;
                ////===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_ДН
        {
            get
            {
                //-------------------Skiffartur 20141118
                //return this.deltaMistakeFull;
                //+++++++++++++++++++Skiffartur 20141118
                return this.delta_Fo;
                //===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_ПХ
        {
            get
            {
                //-------------------Skiffartur 20141118
                return this.deltaMistakeFull;
                //+++++++++++++++++++Skiffartur 20141118
                //return this.delta_Fo;
                //===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_ФД
        {
            get
            {
                return new MistakeClass(this.delta_Фо.Mistake, MistakeTypeEnum.Grad, 360);
            }
        }

        public MistakeClass Погрешность_КУ_в_МАХ
        {
            get
            {
                //-------------------Skiffartur 20141118
                //return this.delta_Fo;
                //+++++++++++++++++++Skiffartur 20141118
                return this.deltaMistakeFull;
                //===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_Напр_МАХ_ДН
        {
            get
            {
                //-------------------Skiffartur 20141118
                //return new MistakeClass(this.delta_O_max.Mistake,MistakeTypeEnum.Grad,this.Направление_максимума_диаграммы_направленности);
                //+++++++++++++++++++Skiffartur 20141118
                return new MistakeClass(this.delta_O_max.Mistake, MistakeTypeEnum.Grad, this.Ширина_диаграммы_направленности_по_половине_мощности);
                //===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_Ширина_ДН
        {
            get
            {
                return new MistakeClass(this.delta_Y_05.Mistake, MistakeTypeEnum.Grad, this.Ширина_диаграммы_направленности_по_половине_мощности);
            }
        }

        public MistakeClass Погрешность_УБЛ
        {
            get
            {
                return this.delta_Fo;
            }
        }

        public MistakeClass Погрешность_Смещения_лепестка
        {
            get
            {
                //-------------------Skiffartur 20141118
                //return new MistakeClass(this.delta_O_max.Mistake, MistakeTypeEnum.Grad, this.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности);
                //+++++++++++++++++++Skiffartur 20141118
                return new MistakeClass(this.delta_O_max.Mistake, MistakeTypeEnum.Grad, this.Ширина_диаграммы_направленности_по_половине_мощности);
                //===================Skiffartur 20141118
            }
        }

        public MistakeClass Погрешность_Степени_кросс_поляизации
        {
            get
            {
                return this.delta_M;
            }
        }
        
        #endregion

        #endregion

    }
}
