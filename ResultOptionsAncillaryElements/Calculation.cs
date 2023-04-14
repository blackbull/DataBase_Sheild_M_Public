using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// Точка в системе координат
    /// </summary>
    [Serializable]
    public class PointDouble
    {
        /// <summary>
        /// В декартовой системе - Х
        /// в полярной - Угол вектора (Ф)
        /// </summary>
        public Double X;
        /// <summary>
        /// В декартовой системе - Y
        /// в полярной - Длина вектора (r)
        /// </summary>
        public Double Y;

        //конструктор по умолчанию
        public PointDouble() { }
        public PointDouble(double newX, double newY)
        {
            this.X = newX;
            this.Y = newY;
        }
    }

    public static class CalculationClass
    {
        public static double CalculateAmplitude(double realPart, double imaginaryPart)
        {
            double ret;

            ret = Math.Pow(Math.Pow(realPart, 2) + Math.Pow(imaginaryPart, 2), 0.5d);

            return ret;
        }

        public static double CalculatePhase(double realPart, double imaginaryPart)
        {
            double ret;

            ret = Math.Atan(imaginaryPart / realPart);

            return ret;
        }

        /// <summary>
        /// Найти абсолютный максимум в графике по Y, ширину диаграммы направленности и боковой лепесток
        /// Exceptions - если нет максимума или ещё чего
        /// </summary>
        /// <param name="Data">исходные данные для рассчёта</param>
        /// <param name="Left">возвращаемая точка левой границы ДН по половине мощности</param>
        /// <param name="Right">возвращаемая точка правой границы ДН по половине мощности</param>
        /// <param name="Max2">Вершина бокового лепестка</param>
        /// <param name="smoothing">сглаживание значение (убирает выпадающие точки)
        /// 0 - без сглаживания, 1 - исключаются из рассчёта точки, если нет плавного возрастания хотя бы одной точки с каждой стороны локального максимума</param>
        /// <param name="WidthDN">Ширина диаграммы направленности по половине мощности -3 ДБ</param>
        /// <returns></returns>
        public static PointDouble CalculationMainMax(List<PointDouble> Data, out PointDouble Left, out PointDouble Right, out PointDouble Max2, uint smoothing, out int MaxIndex, out int LeftIndex, out int RightIndex)
        {
            double WidthDN = 0;
            return CalculationMainMax(Data, out Left, out Right, out Max2, smoothing, out MaxIndex, out LeftIndex, out RightIndex, out WidthDN);
        }


        /// <summary>
        /// Найти абсолютный максимум в графике по Y, ширину диаграммы направленности и боковой лепесток
        /// Exceptions - если нет максимума или ещё чего
        /// </summary>
        /// <param name="Data">исходные данные для рассчёта</param>
        /// <param name="Left">возвращаемая точка левой границы ДН по половине мощности</param>
        /// <param name="Right">возвращаемая точка правой границы ДН по половине мощности</param>
        /// <param name="Max2">Вершина бокового лепестка</param>
        /// <param name="smoothing">сглаживание значение (убирает выпадающие точки)
        /// 0 - без сглаживания, 1 - исключаются из рассчёта точки, если нет плавного возрастания хотя бы одной точки с каждой стороны локального максимума</param>
        /// <param name="WidthDN">Ширина диаграммы направленности по половине мощности -3 ДБ</param>
        /// <param name="LeftLepestok">Левый боковой лепесток</param>
        /// <param name="RightLepestok">Правый боковой лепесток</param>
        /// <returns></returns>
        public static PointDouble CalculationMainMax(List<PointDouble> Data, out PointDouble Left, out PointDouble Right, out PointDouble Max2, uint smoothing, out int MaxIndex, out int LeftIndex, out int RightIndex, out double WidthDN, out PointDouble LeftLepestok, out PointDouble RightLepestok, double smoothing_dB=0.4d)
        {
            #region проверка принятых даннных

            LeftLepestok = new PointDouble(double.NaN, double.NaN);
            RightLepestok = new PointDouble(double.NaN, double.NaN);


            //если данных в массиве мало, то отключаем сглаживание
            if (Data.Count <= 5)
            {
                smoothing = 0;
            }


            if (Data.Count == 0)
            {
                PointDouble ret = new PointDouble(double.NaN, double.NaN);
                Left = new PointDouble(double.NaN, double.NaN);
                Right = new PointDouble(double.NaN, double.NaN);
                Max2 = new PointDouble(double.NaN, double.NaN);
                MaxIndex = -1;
                LeftIndex = -1;
                RightIndex = -1;
                WidthDN = double.NaN;

                return ret;
            }

            if (Data.Count == 1)
            {
                PointDouble ret = Data[0];
                Left = Data[0];
                Right = Data[0];
                Max2 = Data[0];
                MaxIndex = 0;
                LeftIndex = 0;
                RightIndex = 0;
                WidthDN = 0;

                return ret;
            }


            if (Data.Count == 2)
            {
                PointDouble ret = new PointDouble(double.NaN, double.NaN);

                if (Data[0].Y >= Data[1].Y)
                {
                    ret = Data[0];
                    Left = Data[1];
                    Right = Data[1];
                    Max2 = Data[1];
                    MaxIndex = 0;
                    LeftIndex = 1;
                    RightIndex = 1;
                }
                else
                {
                    ret = Data[1];
                    Left = Data[0];
                    Right = Data[0];
                    Max2 = Data[0];
                    MaxIndex = 1;
                    LeftIndex = 0;
                    RightIndex = 0;
                }

                WidthDN = 0;

                return ret;
            }

            #endregion

            ///глобальный максимум
            PointDouble Max = Data[0];
            Max2 = new PointDouble(double.NaN,double.NaN);
            MaxIndex = 0;
            LeftIndex = 0;
            RightIndex = 0;

            Left = new PointDouble();
            Right = new PointDouble();
            //вспомогательная переменная для определения ширины ДН
            PointDouble LastPoint = new PointDouble();

            #region ищем глобальный максимум

            for (int i = 1; i < Data.Count; i++)
            {
                if (Data[i].Y > Max.Y)
                {
                    Max = Data[i];
                    MaxIndex = i;
                }
            }

            #endregion

            #region ищем ширину ДН от максимума -3 ДБ

            double WidthDN_Right = double.NaN;
            double WidthDN_Left = double.NaN;

            bool FingRight = false;
            bool FingLeft = false;



            #region ищем в правую сторону

            for (int i = MaxIndex + 1; i < Data.Count; i++)
            {
                //предыдущая точка
                LastPoint = Data[i - 1];

                if (Data[i].Y <= Max.Y - 3)
                {
                    Right = Data[i];
                    RightIndex = i;

                    //считаем интерполированное значение
                    double Interpolation_3_DB = LinInterpolation(LastPoint.Y, Right.Y, Max.Y - 3, LastPoint.X, Right.X);

                    WidthDN_Right = Math.Abs(Interpolation_3_DB - Max.X);
                    FingRight = true;
                    break;
                }
            }

            if (!FingRight)
            {
                for (int i = 0; i < MaxIndex; i++)
                {
                    //предыдущая точка
                    if (i == 0) LastPoint = Data[0];
                    else LastPoint = Data[i - 1];

                    if (Data[i].Y <= Max.Y - 3)
                    {
                        Right = Data[i];
                        RightIndex = i;

                        //считаем интерполированное значение
                        double Interpolation_3_DB = LinInterpolation(LastPoint.Y, Right.Y, Max.Y - 3, LastPoint.X, Right.X);

                        WidthDN_Right = Math.Abs(Data[Data.Count - 1].X - Max.X) + Math.Abs(Interpolation_3_DB - Data[0].X);
                        FingRight = true;
                        break;
                    }
                }
            }
            #endregion

            if (!FingRight)
            {
                #region не найдено ширины ДН -3 ДБ - это круговая ДН

                RightIndex = Data.Count - 1;
                Right = Data[RightIndex];

                LeftIndex = 0;
                Left = Data[LeftIndex];

                WidthDN = Math.Abs(Data[Data.Count - 1].X - Data[0].X);

                #endregion
            }
            else
            {
                #region Ищем в левую сторону

                for (int i = MaxIndex - 1; i >= 0; i--)
                {
                    //предыдущая точка
                    LastPoint = Data[i + 1];

                    if (Data[i].Y <= Max.Y - 3)
                    {
                        Left = Data[i];
                        LeftIndex = i;

                        //считаем интерполированное значение
                        double Interpolation_3_DB = LinInterpolation(Left.Y, LastPoint.Y, Max.Y - 3, Left.X, LastPoint.X);

                        WidthDN_Left = Math.Abs(Max.X - Interpolation_3_DB);
                        FingLeft = true;
                        break;
                    }
                }

                if (!FingLeft)
                {
                    for (int i = Data.Count - 1; i > MaxIndex; i--)
                    {
                        //предыдущая точка
                        if (i == Data.Count - 1) LastPoint = Data[Data.Count - 1];
                        else LastPoint = Data[i + 1];

                        if (Data[i].Y <= Max.Y - 3)
                        {
                            Left = Data[i];
                            LeftIndex = i;
                            //считаем интерполированное значение
                            double Interpolation_3_DB = LinInterpolation(Left.Y, LastPoint.Y, Max.Y - 3, Left.X, LastPoint.X);

                            WidthDN_Left = Math.Abs(Max.X - Data[0].X) + Math.Abs(Data[Data.Count - 1].X - Interpolation_3_DB);
                            FingLeft = true;
                            break;
                        }
                    }
                }

                #endregion

                WidthDN = WidthDN_Left + WidthDN_Right;
            }

            #endregion

            #region Ищем боковики

            List<PointDouble> LeftPoints, RightPoints;
            DeleteMassiv_By_Max(Data, MaxIndex, out LeftPoints, out RightPoints);

            System.Diagnostics.Trace.TraceInformation("Поиск вправо, CalculationClass");
            RightLepestok = FindRightLepestok(RightPoints,smoothing,smoothing_dB);

            System.Diagnostics.Trace.TraceInformation("Поиск влево, CalculationClass");
            LeftPoints.Reverse();
            LeftLepestok = FindRightLepestok(LeftPoints, smoothing, smoothing_dB);


            if (RightLepestok.Y > LeftLepestok.Y)
                Max2 = RightLepestok;
            else
                Max2 = LeftLepestok;

            #endregion

            #region старые варианты поиска боковика
            /*
            #region ищем боковик
            
            #region поиск вправо от правой границы МАХ-3дБ

            double smoothing_dB = 0.5d;  //необходимый перепад между точками для сглаживания помех
            int UppingCountNow = 0;     //для расчета повышения/повышения после предполаемого минимума/максимума
            int UppingCountNeed = 2;    //необходимое количество точек повышения/понижения для признания точки минимумом/максимумом
            
            bool FlagCanFindRightMin = false;
            bool FlagCanFindRightMax = false;

            double tempMinR = Right.Y;  //временный локальный минимум
            int tempMinRIndex = RightIndex;
                       

            //идем вправо для поиска минимального провала
            for (int i = RightIndex; i < Data.Count; i++)
            {
                #region Поиск провала

                    //если минимум больше нового значения
                    if (tempMinR > Data[i].Y)
                    {
                        //то запоминаем новый минимум
                        tempMinR = Data[i].Y;
                        tempMinRIndex = i;
                        UppingCountNow = 0;
                    }

                    else if (tempMinR + smoothing_dB < Data[i].Y) //если текущая точка выше прошлого минимума + smoothing_dB
                    {
                        if (UppingCountNow >= UppingCountNeed)
                        {
                            //то мы нашли логальный минимум (перегиб)
                            System.Diagnostics.Trace.TraceInformation("Точка минимума вправо = x {0} y{1}", Data[tempMinRIndex].X, Data[tempMinRIndex].Y);

                            FlagCanFindRightMin = true;
                            break;
                        }
                        else
                        {
                            UppingCountNow++;  //считаем количество точек повышения
                        }
                    }
                }
                #endregion

            double tempMaxR = tempMinR;  //временный локальный максимум
            int tempMaxRIndex = tempMinRIndex;
            UppingCountNow = 0;

            if (FlagCanFindRightMin)
            {
                #region нашли провал, далее ищем локальный максимум
                for (int i = tempMinRIndex; i < Data.Count; i++)
                {
                    //если максимум меньше нового значения
                    if (tempMaxR < Data[i].Y)
                    {
                        //то запоминаем новый максимум
                        tempMaxR = Data[i].Y;
                        tempMaxRIndex = i;
                        UppingCountNow = 0;
                    }

                    else if (tempMaxR - smoothing_dB > Data[i].Y) //если текущая точка ниже прошлого максимума + smoothing_dB
                    {
                        if (UppingCountNow >= UppingCountNeed)
                        {
                            //то мы нашли логальный минимум (перегиб)
                            System.Diagnostics.Trace.TraceInformation("Точка максимума вправо = x {0} y{1}", Data[tempMaxRIndex].X, Data[tempMaxRIndex].Y);
                            FlagCanFindRightMax = true;

                            break;
                        }
                        else
                        {
                            UppingCountNow++;  //считаем количество точек повышения
                        }
                    }
                }

                #endregion
            }
            else System.Diagnostics.Trace.TraceInformation("Точка минимума вправо не найдена, то и локальный максимум не найти");
            if (!FlagCanFindRightMax) System.Diagnostics.Trace.TraceInformation("Точка максимума вправо не найдена");

            #endregion
            
            #endregion

            #region ищем боковой лепесток устаревший алгоритм

           
                double tempMin = Right.Y;
                double tempLeft = Right.Y;

                //тк поиск идет от границы ДН, то мы находимся на снижении
                bool ToDown = true;
                bool FindMin = false;

                int LeftLepestokData = 0;
                int RightLepestokData = 0;

                for (int i = RightIndex + 1; i < Data.Count; i++)
                {
                    if (FindMin)
                    {
                        //ищем следующее понижение
                        if (Data[i].Y <= tempMin)
                        {
                            RightLepestokData = i;
                            break;
                        }
                    }
                    else
                    {

                        // ищем подьем
                        if (Data[i].Y > tempMin)
                        {
                            //пошли вверх по графику
                            ToDown = false;

                            if (Data[i].Y > tempMin + 0.3) //должно быть значительно изменение иначе это всего лишь дребезг
                            {
                                //нашли провал - значит следующий максимум - это боковой лепесток
                                FindMin = true;
                            }
                        }
                        else
                        {
                            // ищем глобальный провал провал

                            if (Data[i].Y < tempMin)
                            {
                                tempMin = Data[i].Y;
                                LeftLepestokData = i;
                            }
                        }
                    }
                }

                if (FindMin)
                {
                    Max2 = Data[LeftLepestokData];
                    MaxIndex = LeftLepestokData;

                    for (int i = LeftLepestokData; i < RightLepestokData; i++)
                    {
                        if (Data[i].Y > Max2.Y)
                        {
                            Max2 = Data[i];
                            MaxIndex = i;
                        }
                    }
                }
                
            #endregion

            //пока будет работать по старому поиск боковиков

            PointDouble Left_old;
            PointDouble right_old;
            int maxInd_old;
            int leftInd_old;
            int rightInd_old;

            CalculationMainMax_old(Data, out Left_old, out right_old, out Max2, 1, out maxInd_old, out leftInd_old, out rightInd_old);
            */
            #endregion

            return Max;
        }

        /// <summary>
        /// Найти абсолютный максимум в графике по Y, ширину диаграммы направленности и боковой лепесток
        /// Exceptions - если нет максимума или ещё чего
        /// </summary>
        /// <param name="Data">исходные данные для рассчёта</param>
        /// <param name="Left">возвращаемая точка левой границы ДН по половине мощности</param>
        /// <param name="Right">возвращаемая точка правой границы ДН по половине мощности</param>
        /// <param name="Max2">Вершина бокового лепестка</param>
        /// <param name="smoothing">сглаживание значение (убирает выпадающие точки)
        /// 0 - без сглаживания, 1 - исключаются из рассчёта точки, если нет плавного возрастания хотя бы одной точки с каждой стороны локального максимума</param>
        /// <param name="WidthDN">Ширина диаграммы направленности по половине мощности -3 ДБ</param>
        /// <returns></returns>
        public static PointDouble CalculationMainMax(List<PointDouble> Data, out PointDouble Left, out PointDouble Right, out PointDouble Max2, uint smoothing, out int MaxIndex, out int LeftIndex, out int RightIndex, out double WidthDN)
        {
            PointDouble LeftLepestok;
            PointDouble RightLepestok;
            
            return CalculationMainMax(Data, out Left, out Right, out  Max2, smoothing, out  MaxIndex, out  LeftIndex, out  RightIndex, out  WidthDN, out LeftLepestok, out RightLepestok);
        }

       public static PointDouble FindRightLepestok(List<PointDouble> Data, uint smoothing = 2, double smoothing_dB = 0.5d)
        {
            PointDouble RightLepestok = new PointDouble(double.NaN, double.NaN);

            #region поиск вправо от правой границы МАХ-3дБ
                       
            int UppingCountNow = 0;     //для расчета повышения/повышения после предполаемого минимума/максимума
            int UppingCountNeed = 2;    //необходимое количество точек повышения/понижения для признания точки минимумом/максимумом

            bool FlagCanFindRightMin = false;
            bool FlagCanFindRightMax = false;

            double tempMinR = Data[0].Y;  //временный локальный минимум
            int tempMinRIndex = 0;

            #region Ищем индекс для старта от -3дБ от максимума
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].Y < Data[0].Y - 3)
                {
                    tempMinRIndex = i;
                    break;
                }
            }
            #endregion


            //идем вправо для поиска минимального провала
            for (int i = tempMinRIndex; i < Data.Count; i++)
            {
                #region Поиск провала

                //если минимум больше нового значения
                if (tempMinR > Data[i].Y)
                {
                    //то запоминаем новый минимум
                    tempMinR = Data[i].Y;
                    tempMinRIndex = i;
                    UppingCountNow = 0;
                }

                else if (tempMinR + smoothing_dB < Data[i].Y) //если текущая точка выше прошлого минимума + smoothing_dB
                {
                    if (UppingCountNow >= UppingCountNeed)
                    {
                        //то мы нашли логальный минимум (перегиб)
                        System.Diagnostics.Trace.TraceInformation("Точка минимума = x {0} y{1} , CalculationClass", Data[tempMinRIndex].X, Data[tempMinRIndex].Y);

                        FlagCanFindRightMin = true;
                        break;
                    }
                    else
                    {
                        UppingCountNow++;  //считаем количество точек повышения
                    }
                }
            }
                #endregion

            double tempMaxR = tempMinR;  //временный локальный максимум
            int tempMaxRIndex = tempMinRIndex;
            UppingCountNow = 0;

            if (FlagCanFindRightMin)
            {
                #region нашли провал, далее ищем локальный максимум
                for (int i = tempMinRIndex; i < Data.Count; i++)
                {
                    //если максимум меньше нового значения
                    if (tempMaxR < Data[i].Y)
                    {
                        //то запоминаем новый максимум
                        tempMaxR = Data[i].Y;
                        tempMaxRIndex = i;
                        UppingCountNow = 0;
                    }

                    else if (tempMaxR - smoothing_dB > Data[i].Y) //если текущая точка ниже прошлого максимума + smoothing_dB
                    {
                        if (UppingCountNow >= UppingCountNeed)
                        {
                            //то мы нашли логальный максимум (перегиб)
                            RightLepestok = Data[tempMaxRIndex];
                            System.Diagnostics.Trace.TraceInformation("Точка максимума = x {0} y{1}", Data[tempMaxRIndex].X, Data[tempMaxRIndex].Y);
                            FlagCanFindRightMax = true;

                            break;
                        }
                        else
                        {
                            UppingCountNow++;  //считаем количество точек повышения
                        }
                    }
                }

                #endregion
            }
            else System.Diagnostics.Trace.TraceInformation("Точка минимума не найдена, то и локальный максимум не найти");
            if (!FlagCanFindRightMax) System.Diagnostics.Trace.TraceInformation("Точка максимума не найдена");

            #endregion


            return RightLepestok;
        }

       public static void DeleteMassiv_By_Max(List<PointDouble> Data, int MaxIndex, out  List<PointDouble> LeftPoints, out List<PointDouble> RightPoints)
        {
            #region режем диаграмму по максимому

            PointDouble Max = Data[MaxIndex];

             int NalfOfDataCont=Data.Count / 2;
             int Smeshenie = MaxIndex - NalfOfDataCont;

             LeftPoints = new List<PointDouble>();
             RightPoints = new List<PointDouble>();

             if (MaxIndex >= NalfOfDataCont)
             {
                 for (int i = MaxIndex; i < Data.Count; i++)
                 {
                     RightPoints.Add(Data[i]);
                 }

                 for (int i = 0; i < Smeshenie; i++)
                 {
                     RightPoints.Add(Data[i]);
                 }

                 for (int i = Smeshenie; i <= MaxIndex; i++)
                 {
                     LeftPoints.Add(Data[i]);
                 }
             }
             else
             {
                 for (int i = MaxIndex; i <= MaxIndex + NalfOfDataCont; i++)
                 {
                     RightPoints.Add(Data[i]);
                 }

                 for (int i = MaxIndex+ NalfOfDataCont+1 ; i < Data.Count; i++)
                 {
                     LeftPoints.Add(Data[i]);
                 }

                 for (int i = 0; i <= MaxIndex; i++)
                 {
                     LeftPoints.Add(Data[i]);
                 }
             }
                 
            #endregion

        }



         /// <summary>
         /// Найти абсолютный максимум в графике по Y, ширину диаграммы направленности и боковой лепесток
         /// Exceptions - если нет максимума или ещё чего
         /// </summary>
         /// <param name="Data">исходные данные для рассчёта</param>
         /// <param name="Left">возвращаемая точка левой границы ДН по половине мощности</param>
         /// <param name="Right">возвращаемая точка правой границы ДН по половине мощности</param>
         /// <param name="Max2">Вершина бокового лепестка</param>
         /// <param name="smoothing">сглаживание значение (убирает выпадающие точки)
         /// 0 - без сглаживания, 1 - исключаются из рассчёта точки, если нет плавного возрастания хотя бы одной точки с каждой стороны локального максимума</param>
         /// <returns></returns>
         public static PointDouble CalculationMainMax_old(List<PointDouble> Data, out PointDouble Left, out PointDouble Right, out PointDouble Max2, uint smoothing, out int MaxIndex, out int LeftIndex, out int RightIndex)
        {
            

            #region проверка принятых даннных

            //если данных в массиве мало, то отключаем сглаживание
            if (Data.Count <= 5)
            {
                smoothing = 0;
            }


            if (Data.Count == 0)
            {
                PointDouble ret = new PointDouble(double.NaN, double.NaN);
                Left = new PointDouble(double.NaN, double.NaN);
                Right = new PointDouble(double.NaN, double.NaN);
                Max2 = new PointDouble(double.NaN, double.NaN);
                MaxIndex = -1;
                LeftIndex = -1;
                RightIndex = -1;

                return ret;
            }

            if (Data.Count == 1)
            {
                PointDouble ret = Data[0];
                Left = Data[0];
                Right = Data[0];
                Max2 = Data[0];
                MaxIndex = 0;
                LeftIndex = 0;
                RightIndex = 0;

                return ret;
            }


            if (Data.Count == 2)
            {
                PointDouble ret = new PointDouble(double.NaN, double.NaN);

                if (Data[0].Y >= Data[1].Y)
                {
                    ret = Data[0];
                    Left = Data[1];
                    Right = Data[1];
                    Max2 = Data[1];
                    MaxIndex = 0;
                    LeftIndex = 1;
                    RightIndex = 1;
                }
                else
                {
                    ret = Data[1];
                    Left = Data[0];
                    Right = Data[0];
                    Max2 = Data[0];
                    MaxIndex = 1;
                    LeftIndex = 0;
                    RightIndex = 0;
                }

                return ret;
            }

            #endregion

            ///глобальный максимум
            PointDouble Max = new PointDouble(double.MinValue,double.MinValue);
            MaxIndex = -1;
            LeftIndex = -1;
            RightIndex = -1;

            Max2 = new PointDouble(double.MinValue, double.MinValue);
            Left = new PointDouble(double.MinValue, double.MinValue);
            Right = new PointDouble(double.MinValue, double.MinValue);

            bool WaitForAddingMAXFlag = false;
            uint leftPoints = 0;
            uint RightPoints = 0;

            PointDouble WaitMax = new PointDouble();
            int WaitMaxID = 0;

            bool leftGood = false;
            bool rightGood = false;

            List<PointDouble> LocalMax = new List<PointDouble>();

            PointDouble currentMax = Data[0];
            Boolean ToHigh = true;

            #region проверка первого элемента
            int h = Data.Count;
            while ((Data.Count - 1) - h < smoothing)
            {
                if (currentMax.Y >= Data[h - 1].Y)
                {
                    leftPoints++;
                }

                currentMax = Data[h - 1];
                h--;
            }

            if (Data[0].Y >= Data[Data.Count - 1].Y)
            {
                ToHigh = true;
            }
            else
            {
                ToHigh = false;
            }

            #endregion

            List<int> IDlist = new List<int>();
            currentMax = Data[0];

            #region заполнение массива локальных максимумов
            //смотрим до пред предпоследнего
            for (Int32 i = 1; i < Data.Count - 1; i++)
            {
                if (currentMax.Y > Data[i].Y && ToHigh)
                {
                    //найден перегиб в максимуме
                    ToHigh = false;

                    WaitMax = currentMax;
                    WaitMaxID = i - 1;

                    WaitForAddingMAXFlag = true;
                    leftGood = false;
                    rightGood = false;
                }
                if (currentMax.Y < Data[i].Y && !ToHigh)
                {
                    //найден перегиб в минимуме
                    ToHigh = true;
                    WaitForAddingMAXFlag = false;
                }

                //прибавляем количество пройденных точек
                if (ToHigh)
                {
                    leftPoints++;
                }
                else
                {
                    RightPoints++;
                }

                //проверяем количество пройденных точек и добавляем точку, если условие выполнилось
                if (WaitForAddingMAXFlag)
                {
                    if (leftPoints > smoothing)
                    {
                        leftGood = true;
                    }
                    if (RightPoints > smoothing)
                    {
                        rightGood = true;
                    }

                    if (leftGood && rightGood)
                    {
                        leftGood = false;
                        rightGood = false;
                        WaitForAddingMAXFlag = false;


                        LocalMax.Add(WaitMax);
                        IDlist.Add(WaitMaxID);
                    }
                }

                //обнуляем пройденные точки при переходе через точки перегиба
                if (ToHigh)
                {
                    RightPoints = 0;
                }
                else
                {
                    leftPoints = 0;
                }

                currentMax = Data[i];
            }
            #endregion

            #region проверка последнего элемента
            if (leftPoints > (smoothing - 1))
            {
                int tempright = 0;
                WaitMax = Data[Data.Count - 1];

                for (int i = 0; i <= smoothing; i++)
                {
                    if (WaitMax.Y > Data[i].Y)
                    {
                        tempright++;

                        if (tempright > smoothing)
                        {
                            LocalMax.Add(Data[Data.Count - 1]);
                            IDlist.Add(Data.Count - 1);
                            break;
                        }
                    }

                    WaitMax = Data[i];
                }
            }

            if (Data[Data.Count - 1].Y > Data[Data.Count - 2].Y && Data[Data.Count - 1].Y > Data[0].Y)
            {
                //LocalMax.Add(Data[Data.Count-1]);
                //IDlist.Add(Data.Count-1);
            }
            #endregion

            #region поиск максимума в локальных максимумах с учетом сглаживания

            #region проверка единичных случаев
            if (LocalMax.Count == 2)
            {
                if (LocalMax[0].Y > LocalMax[1].Y)
                {
                    Max = LocalMax[0];
                    Max2 = new PointDouble(double.NaN,double.NaN); //бокового лепестка нету 
                    MaxIndex = IDlist[0];
                }
                else
                {
                    Max = LocalMax[1];
                    Max2 = new PointDouble(double.NaN, double.NaN); //бокового лепестка нету 
                    MaxIndex = IDlist[1];
                }
            }
            else if (LocalMax.Count == 0)
            {
                //локальных максимумов нету, хрен че найдешь, максимум тоже не найти
                Max = new PointDouble(double.NaN, double.NaN); //бокового лепестка нету 
                Max2 = new PointDouble(double.NaN, double.NaN); //бокового лепестка нету 
            }
           
            #endregion


            #region поиск абсолютного максимума во всех значениях

            for (int j = 0; j < Data.Count; j++)
            {
                if (Data[j].Y > Max.Y)
                {
                    Max = Data[j];
                    MaxIndex = j;
                }
            }

            #endregion
            

            for (int i = 0; i < LocalMax.Count; i++)
            {
                if (Max.Y < LocalMax[i].Y)
                {
                    Max2 = Max;
                    Max = LocalMax[i];
                    MaxIndex = IDlist[i];
                }
                else
                {
                    if (Max2.Y < LocalMax[i].Y && Max.Y != LocalMax[i].Y)
                    {
                        Max2 = LocalMax[i];
                    }
                }
            }
            #endregion

            



            #region поиск ширины диаграммы направленности

            int z = MaxIndex - 1;
            int end = 0;
            ///Что бы не было зависаний -когда нельзя найти ширину диаграммы
            bool Circle = false;
            ///Что бы не делать лишний цикл -когда нельзя найти ширину диаграммы
            bool NoWight = false;

            //поиск левой границы
            while (z >= end)
            {
                if (Data[z].Y < Max.Y - 3)
                {
                    Left = Data[z];
                    LeftIndex = z;
                    break;
                }
                if (z == end)
                {
                    if (Circle)
                    {
                        Left.X = double.NaN;
                        Left.Y = double.NaN;
                        break;
                    }
                    z = Data.Count - 1;
                    end = MaxIndex + 1;

                    Circle = true;
                }
                z--;
            }



            if (!NoWight)
            {
                z = MaxIndex + 1;
                end = Data.Count - 1;
                Circle = false;

                //поиск правой границы
                while (z <= end)
                {
                    if (Data[z].Y < Max.Y - 3)
                    {
                        Right = Data[z];
                        RightIndex = z;
                        break;
                    }
                    if (z == end)
                    {
                        if (Circle)
                        {
                            Right.X = double.NaN;
                            Right.Y = double.NaN;
                            break;
                        }
                        z = 0;
                        end = MaxIndex - 1;

                        Circle = true;
                    }
                    z++;
                }
            }

            #endregion


            #region если исходные данные прямая - невозможно найти максимум
            // если не найти, то будут наны а не нулевой элемент
            if (double.IsNaN(Max.X) && double.IsNaN(Max.Y))
            {
                Max2 = new PointDouble(double.NaN, double.NaN); //бокового лепестка нету 
                /*
                Max = Data[0];
                Left = Data[0];
                Right = Data[Data.Count - 1];
                Max2 = Data[0];
                MaxIndex = 0;
                LeftIndex = 0;
                RightIndex = Data.Count - 1;*/
            }
            
            #endregion

            return Max;
        }

        /// <summary>
        /// Найти абсолютный максимум в графике по Y, ширину диаграммы направленности и боковой лепесток
        /// Exceptions - если нет максимума или ещё чего
        /// </summary>
        /// <param name="Data">исходные данные для рассчёта</param>
        /// <param name="Left">возвращаемая точка левой границы ДН по половине мощности</param>
        /// <param name="Right">возвращаемая точка правой границы ДН по половине мощности</param>
        /// <param name="Max2">Вершина бокового лепестка</param>
        /// <param name="smoothing">сглаживание значение (убирает выпадающие точки)
        /// 0 - без сглаживания, 1 - исключаются из рассчёта точки, если нет плавного возрастания хотя бы одной точки с каждой стороны локального максимума</param>
        /// <returns></returns>
        public static PointDouble CalculationMainMax(List<PointDouble> Data, out PointDouble Left, out PointDouble Right, out PointDouble Max2, uint smoothing)
        {
            int MaxIndex = 0;
            int LeftIndex = 0;
            int RightIndex = 0;            

            return CalculationMainMax(Data, out Left, out Right, out Max2, smoothing, out MaxIndex, out LeftIndex, out RightIndex);
        }

        public static List<PointDouble> RestorePhase(List<PointDouble> Data, int MaxIndex)
        {
            List<PointDouble> ret = new List<PointDouble>();

            if (MaxIndex >= 0)
            {
                int mn = 0;

                #region проверка 1й точки
                if (Math.Abs(Data[Data.Count - 1].Y - Data[0].Y) >= 250)
                {
                    if (Data[Data.Count - 1].Y < Data[0].Y)
                    {
                        mn -= 360;
                    }
                    else
                    {
                        mn += 360;
                    }
                }
                ret.Add(new PointDouble(Data[0].X, Data[0].Y + mn));
                #endregion


                for (int i = 1; i < Data.Count; i++)
                {
                    if (Math.Abs(Data[i - 1].Y - Data[i].Y) >= 250)
                    {
                        if (Data[i - 1].Y < Data[i].Y)
                        {
                            mn -= 360;
                        }
                        else
                        {
                            mn += 360;
                        }
                    }

                    ret.Add(new PointDouble(Data[i].X, Data[i].Y + mn));
                }


                if (Data[MaxIndex].Y != ret[MaxIndex].Y)
                {
                    double temp = ret[MaxIndex].Y - Data[MaxIndex].Y;

                    for (int i = 0; i < Data.Count; i++)
                    {
                        ret[i].Y -= temp;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Скорость света
        /// </summary>
        public const double C = 299792458;

        /// <summary>
        /// Перевести значения фазы из градусов в метры
        /// </summary>
        /// <param name="Data">Значения фазы</param>
        /// <param name="Frequency">Частота в Гц</param>
        /// <param name="R">Растояние между точками вращения антенн в метрах</param>
        /// <returns></returns>
        public static List<PointDouble> ConvertPhase(List<PointDouble> Data, double Frequency, double R)
        {
            List<PointDouble> ret = new List<PointDouble>();
            //double A = C / (Frequency * 360);

            //double tempK = (2 * Math.PI * Frequency) / C;

            double Lambda = C / Frequency;
            double K = Lambda / (2 * Math.PI);

            foreach (PointDouble temp in Data)
            {
                //ret.Add(new PointDouble(temp.X, temp.Y * A + R));
                PointDouble tempPoint = new PointDouble();
                //tempPoint.X = (R - temp.Y / tempK) * Math.Cos(temp.X);
                //tempPoint.Y = (R - temp.Y / tempK) * Math.Sin(temp.X);

                tempPoint.X = Math.Abs((temp.Y * K + R) * Math.Cos(temp.X));
                tempPoint.Y = Math.Abs((temp.Y * K + R) * Math.Sin(temp.X));

                ret.Add(tempPoint);
            }




            return ret;
        }

        /// <summary>
        /// вырезать из набора данных вложение ширины ДН
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="LeftIndex"></param>
        /// <param name="RightIndex"></param>
        /// <returns></returns>
        public static List<PointDouble> CutByIndexes(List<PointDouble> Data, int LeftIndex, int RightIndex)
        {
            List<PointDouble> ret = new List<PointDouble>();

            if (Data.Count != 0)
            {
                if (LeftIndex <= RightIndex)
                {
                    for (int i = LeftIndex; i <= RightIndex; i++)
                    {
                        ret.Add(Data[i]);
                    }
                }
                else
                {
                    for (int i = LeftIndex; i < Data.Count; i++)
                    {
                        ret.Add(Data[i]);
                    }

                    for (int i = 0; i <= RightIndex; i++)
                    {
                        ret.Add(Data[i]);
                    }
                }
            }
            return ret;
        }


        /// <summary>
        /// временное хранение данных для рассчётов
        /// </summary>
        private static List<PointDouble> Data = new List<PointDouble>();
        public static Double K = 1;

        #region метод перебора
        /// <summary>
        /// Рассчитать фазовый центр методом перебора
        /// </summary>
        /// <param name="newData">Востановленная фаза в радианах и угол поворота в радианах</param>
        /// <param name="Frequency">Частота в Гц</param>
        /// <param name="Fi0"></param>
        /// <returns></returns>
        static public PointDouble GetPhaseCentr2(List<PointDouble> newData, double Frequency, out double Fi0)
        {
            Data = newData;

            K = (2 * Math.PI) / (C / Frequency);

            PointDouble ret = new PointDouble();
            Fi0 = 0;

            double min = double.MaxValue;

            //double end = Math.PI * 2;
            //double step = Math.PI / 100;

            double x = 0d;
            double y = 0d;
            double f = 0.036578631156141794d;

            for (x = -1; x <= 1; x += 0.01)
            {
                for (y = -1; y <= 1; y += 0.01)
                {
                    for (f = 60; f <= 65; f += 0.001d)
                    {
                        double tempcalculate = CalculateFunction(f, x, y);

                        if (min > tempcalculate)
                        {
                            min = tempcalculate;

                            ret.X = x;
                            ret.Y = y;
                            Fi0 = f;
                        }
                    }
                }
            }


            return ret;
        }
        #endregion

        #region метод минимизации

        static public PointDouble GetPhaseCentr3(List<PointDouble> newData, double Frequency, out double Fi0)
        {
            Data = newData;

            K = (2 * Math.PI) / (C / Frequency);

            PointDouble ret = new PointDouble();
            Fi0 = double.NaN;
            double min;

            MetodKoordinatClass.CalculationFunctionDelegate_3elementa function = delegate(double x, double y, double fi)
            {
                return CalculateFunction(fi, x, y);
            };

            MetodKoordinatClass.GetMinimum_3elementa(function, -1, 1, -1, 1, 3, 4, 0.000001, out ret.X, out ret.Y, out Fi0, out min);

            return ret;
        }
        #endregion


        static double CalculateFunction(double Fi0, double X, double Y)
        {
            double ret = 0;

            double Sum2 = 0;

            foreach (PointDouble tempData in Data)
            {
                double Ksi = 3 * K - tempData.Y;
                double tt = Ksi - (Fi0 - K * X * Math.Cos(tempData.X) - K * Y * Math.Sin(tempData.X));


                Sum2 += Math.Pow(tt, 2);
            }

            ret = Math.Pow(Sum2 / Data.Count, 0.5);

            return ret;
        }

        #region метод локальных фазовый центров

        /// <summary>
        /// Рассчёт центра описаной окружности по координатам точек треугольника в декартовой системе координат
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        static public PointDouble GetCenterOut(PointDouble A, PointDouble B, PointDouble C)
        {
            //Рассчёт взят отсюда http://www.obychalki.ru/node/324

            //Коэффиценты наклона
            double ma, mb;

            PointDouble ret = new PointDouble();


            if (B.X != A.X)
            {
                ma = (B.Y - A.Y) / (B.X - A.X);
            }
            else
            {
                ma = 1e95d;
            }

            if (C.X != B.X)
            {
                mb = (C.Y - B.Y) / (C.X - B.X);
            }
            else
            {
                mb = 1e95d;
            }


            ret.X = (ma * mb * (A.Y - C.Y) + mb * (A.X + B.X) - ma * (B.X + C.X)) / (2 * (mb - ma));

            if (ma != 0)
            {
                ret.Y = -1 / ma * (ret.X - (A.X + B.X) / 2) + (A.Y + B.Y) / 2;
            }
            else
            {
                ret.Y = -1 / mb * (ret.X - (B.X + C.X) / 2) + (B.Y + C.Y) / 2;
            }


            return ret;
        }

        static public double GetDist(PointDouble A, PointDouble B)
        {
            //Result := sqrt((ax2-ax1)*(ax2-ax1)+(ay2-ay1)*(ay2-ay1));

            return Math.Sqrt((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
        }

        /// <summary>
        /// Найти фазовый центр методом локальных максимумов
        /// </summary>
        /// <param name="PhaseData">Данные фазы в декартовой системе координат</param>
        /// <param name="LocalPhaseCenters"></param>
        /// <returns></returns>
        static public PointDouble GetPhaseCentre_ByLocalMaxMetod(List<PointDouble> PhaseData, out List<PointDouble> LocalPhaseCenters)
        {
            PointDouble ret = new PointDouble(0, 0);

            LocalPhaseCenters = new List<PointDouble>();

            for (int i = 0; i <= PhaseData.Count - 3; i++)
            {
                PointDouble local = GetCenterOut(PhaseData[i], PhaseData[i + 1], PhaseData[i + 2]);
                LocalPhaseCenters.Add(local);

                ret.X += local.X;
                ret.Y += local.Y;
            }

            ret.X /= LocalPhaseCenters.Count;
            ret.Y /= LocalPhaseCenters.Count;

            return ret;
        }

        /// <summary>
        /// Найти фазовый центр методом локальных максимумов
        /// </summary>
        /// <param name="PhaseData">Данные фазы в декартовой системе координат</param>
        /// <param name="LocalPhaseCenters"></param>
        /// <returns></returns>
        static public PointDouble GetPhaseCentre_ByLocalMaxMetod(List<PointDouble> PhaseData)
        {
            List<PointDouble> LocalPhaseCenters;

            return GetPhaseCentre_ByLocalMaxMetod(PhaseData, out LocalPhaseCenters);
        }
        #endregion


        #region Переводчики в системы координат

        /// <summary>
        /// Перевести из декартовой в полярную систему координат
        /// </summary>
        /// <param name="Point">переводимая точка</param>
        /// <returns>переведённая точка в радианах!!</returns>
        public static PointDouble Convert_To_Polar_From_Decart(PointDouble Point)
        {
            PointDouble ret = new PointDouble();

            ret.X = Math.Atan2(Point.Y, Point.X);

            //если выдаёт отрицательный угол
            if (ret.X < 0)
            {
                ret.X = Math.Atan2(Point.X, Point.Y);
            }

            ret.Y = Math.Pow(Math.Pow(Point.X, 2) + Math.Pow(Point.Y, 2), 0.5);

            return ret;
        }

        /// <summary>
        /// Перевести из декартовой в полярную систему координат
        /// </summary>
        /// <param name="Point">переводимая точка</param>
        /// <returns>переведённая точка в радианах!!</returns>
        public static List<PointDouble> Convert_To_Polar_From_Decart(List<PointDouble> Point)
        {
            List<PointDouble> ret = new List<PointDouble>();

            foreach (PointDouble temp in Point)
            {
                ret.Add(Convert_To_Polar_From_Decart(temp));
            }
            return ret;
        }

        //phazeCentr_Полярн.X = Math.Atan2(phazeCentr_СреднееАрифметическое.Y, phazeCentr_СреднееАрифметическое.X);

        //    phazeCentr_Полярн.Y = Math.Pow(Math.Pow(phazeCentr_СреднееАрифметическое.X, 2) + Math.Pow(phazeCentr_СреднееАрифметическое.Y, 2), 0.5);

        //    phazeCentr_Полярн.X=(phazeCentr_Полярн.X*360)/(2*Math.PI);


        //перевод в декартовую систему координат
        //double X = temp.Y * Math.Cos((temp.X / 360) * 2 * Math.PI);
        //double Y = temp.Y * Math.Sin((temp.X / 360) * 2 * Math.PI);

        /// <summary>
        /// Перевести из полярной в декартовую систему координат
        /// </summary>
        /// <param name="Point">переводимая точка в радианах!!</param>
        /// <returns></returns>
        public static PointDouble Convert_To_Decart_From_Polar(PointDouble Point)
        {
            PointDouble ret = new PointDouble();

            ret.X = Point.Y * Math.Cos(Point.X);
            ret.Y = Point.Y * Math.Sin(Point.X);

            return ret;
        }

        /// <summary>
        /// Перевести из полярной в декартовую систему координат
        /// </summary>
        /// <param name="Point">переводимая точка в радианах!!</param>
        /// <returns></returns>
        public static List<PointDouble> Convert_To_Decart_From_Polar(List<PointDouble> Point)
        {
            List<PointDouble> ret = new List<PointDouble>();

            foreach (PointDouble temp in Point)
            {
                ret.Add(Convert_To_Decart_From_Polar(temp));
            }
            return ret;
        }

        public static Double Convert_To_Radian_From_Grad(Double Point)
        {
            return (Point * Math.PI) / 180d;
        }

        public static PointDouble Convert_To_Radian_From_Grad(PointDouble Point)
        {
            PointDouble ret = new PointDouble();

            ret.X = Convert_To_Radian_From_Grad(Point.X);
            ret.Y = Convert_To_Radian_From_Grad(Point.Y);

            return ret;
        }

        public static List<PointDouble> Convert_To_Radian_From_Grad(List<PointDouble> Point)
        {
            List<PointDouble> ret = new List<PointDouble>();

            foreach (PointDouble temp in Point)
            {
                ret.Add(Convert_To_Radian_From_Grad(temp));
            }
            return ret;
        }

        public static Double Convert_To_Grad_From_Radian(Double Point)
        {
            return (Point * 180d) / Math.PI;
        }

        public static PointDouble Convert_To_Grad_From_Radian(PointDouble Point)
        {
            PointDouble ret = new PointDouble();

            ret.X = Convert_To_Grad_From_Radian(Point.X);
            ret.Y = Convert_To_Grad_From_Radian(Point.Y);

            return ret;
        }

        public static List<PointDouble> Convert_To_Grad_From_Radian(List<PointDouble> Point)
        {
            List<PointDouble> ret = new List<PointDouble>();

            foreach (PointDouble temp in Point)
            {
                ret.Add(Convert_To_Grad_From_Radian(temp));
            }
            return ret;
        }

        public static double Convert_Unity_to_dB(double Unity)
        {
            double templog= Math.Log10(Unity);
            return 10 * templog;
        }
        
        #endregion

        #region линейная интерполяция

        /// <summary>
        /// линейная интерполяция (поиск среднего по пропорции)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="xc">искомое соответствие</param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double LinInterpolation(double x1, double x2, double xc, double y1, double y2)
        {
            double ret;

            ret = y1 + ((y2 - y1) / (x2 - x1)) * (xc - x1);

            return ret;
        }

        #endregion

        #region перерассчёт КУ в зависимости от ТА

        public static double Calculate_GainIA_dB(double Ampl_dB, double R, double Freq, double GainTA, double LossTA, double LossIA)
        {
            return Convert_Unity_to_dB(Calculate_GainIA_Unity(Ampl_dB, R, Freq, GainTA, LossTA, LossIA));
        }

        public static double Calculate_GainIA_dB(double Ampl_dB, double Coficient)
        {
            return Convert_Unity_to_dB(Calculate_GainIA_Unity(Ampl_dB, Coficient));
        }

        public static double Calculate_GainIA_Unity(double Ampl_dB, double R, double Freq, double GainTA, double LossTA, double LossIA)
        {
            return Calculate_GainIA_Unity(Ampl_dB, Calculate_GainIA_Coficient1_Afs(R, Freq, GainTA, LossTA, LossIA));
        }

        public static double Calculate_GainIA_Unity(double Ampl_dB, double Coficient)
        {
            double temp1 = Math.Pow((Math.Pow(10, Ampl_dB / 20)), 2);

            return temp1 * Coficient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Freq">частота в герцах</param>
        /// <param name="GainTA"></param>
        /// <param name="LossTA"></param>
        /// <param name="LossIA"></param>
        /// <returns></returns>
        public static double Calculate_GainIA_Coficient1_Afs(double R, double Freq, double GainTA, double LossTA, double LossIA)
        {

            double qw1 = Math.Pow(10, GainTA / 10);
            double qw2 = Math.Pow(10, LossTA / 10);
            double qw3 = Math.Pow(10, LossIA / 10);


            double temp2 = (4 * Math.PI * R * Freq) / CalculationClass.C;
            temp2 = Math.Pow(temp2, 2);

            double temp3 = 1 / (Math.Pow(10, GainTA / 10) * Math.Pow(10, LossTA / 10) * Math.Pow(10, LossIA / 10));

            

            return temp2 * temp3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Freq">частота в герцах</param>
        /// <param name="GainTA"></param>
        /// <param name="LossTA"></param>
        /// <param name="LossIA"></param>
        /// <returns></returns>
        public static double Calculate_GainIA_Coficient1(double R, double Freq, double GainTA, double LossTA, double LossIA)
        {
            double temp2 = (4 * Math.PI * R * Freq) / CalculationClass.C;
            temp2 = Math.Pow(temp2, 2);

            double temp3 = 1 / (Math.Pow(10, GainTA / 10) * Math.Pow(10, -Math.Abs(LossTA) / 10) * Math.Pow(10, -Math.Abs(LossIA) / 10));

            return temp2 * temp3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mia">в ДБ</param>
        /// <param name="Mta">в ДБ</param>
        /// <returns></returns>
        public static double Calculate_GainIA_Coficient2(double Mia, double Mta,bool sign)
        {
            double ret = double.NaN;

            double tempMia = Math.Pow(10, -(Math.Abs(Mia) / 10));
            double tempMta = Math.Pow(10, -(Math.Abs(Mta) / 10));

            double tempTop = (1 + tempMia) * (1 + tempMta);
            double tempDown1 = Math.Pow(1 + Math.Pow(tempMia, 0.5d) * Math.Pow(tempMta, 0.5d), 2);
            double tempDown2 = Math.Pow(Math.Pow(tempMia, 0.5d) + Math.Pow(tempMta, 0.5d), 2);


#warning !!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //if (sign)
            if(true)
            {
                ret = tempTop / (tempDown1 + tempDown2);
            }
            else
            {
                ret = tempTop / (tempDown1 - tempDown2);
            }

            return ret;
        }

        #endregion
        
        #region рассчёт поляризационного отношения

        /// <summary>
        /// рассчёт поляризационного отношения
        /// </summary>
        /// <param name="Ampl_main">(для сумарной диаграммы это MAX)</param>
        /// <param name="Ampl_cross">(для сумарной диаграммы это MIN)</param>
        /// <param name="Phase_main">фаза в градусах (для сумарной диаграммы это MAX)</param>
        /// <param name="Phase_cross">фаза в градусах (для сумарной диаграммы это MIN)</param>
        /// <returns></returns>
        public static ComplexClass CalculatePolarizationRelation(double Ampl_main, double Ampl_cross, double Phase_main, double Phase_cross)
        {
            double temp2 = Math.Pow(10, (Ampl_cross - Ampl_main)/20);

            ComplexClass temp = (((Phase_cross - Phase_main) / 180) * Math.PI) * ComplexClass.î;

            ComplexClass ret = MathComplex.Exp(temp) * temp2;

            return ret;
        }

        public static double CalculatePolarizationRelation(double Ampl_max, double Ampl_min, double Phase_max, double Phase_min, out double Ei_part)
        {
            Ei_part = CalculatePolarizationRelation_Ei_part(Phase_max, Phase_min);
            return CalculatePolarizationRatio_MainPart(Ampl_max, Ampl_min);
        }

        public static double CalculatePolarizationRelation_Ei_part(double Phase_max, double Phase_min)
        {
            return (Phase_min - Phase_max) * (Math.PI / 180);
        }

        public static double CalculatePolarizationRatio_MainPart(double Ampl_max, double Ampl_min)
        {
            return Math.Pow(10, Ampl_min - Ampl_max) / Math.Pow(10, 10);
        }

        #endregion

        #region рассчёт угла наклона

        public static double LevelInclination(double Ampl_max, double Ampl_min, double Phase_max, double Phase_min)
        {
            double ret = LevelInclination(CalculatePolarizationRelation(Ampl_max, Ampl_min, Phase_max, Phase_min));
            return ret;
        }

        public static double LevelInclination(ComplexClass PolarizRelation)
        {
            ComplexClass dd = PolarizRelation + ComplexClass.î;
            ComplexClass dd2 = PolarizRelation - ComplexClass.î;



            ComplexClass dd3 = dd / dd2;

            double dd4 = dd3.Phase;

            double ret = dd4 * (180 / (2 * Math.PI));

            return ret;
        }

        #endregion

        #region рассчёт степени кросс поляризации

        /// <summary>
        /// определить знак true-положительный
        /// </summary>
        /// <param name="prIA"></param>
        /// <returns></returns>
        public static bool sign(ComplexClass prIA)
        {
            bool ret = false;

            ComplexClass tempTop = prIA + ComplexClass.î;
            ComplexClass tempBottom = prIA - ComplexClass.î;

            ComplexClass temp1 = tempTop / tempBottom;

            //temp1 = (tempTop.Real / tempBottom.Real) * (MathComplex.Exp(tempTop.Phase - tempBottom.Phase));


            //модуль
            double temp6 = temp1.Amplitude;

            double temp2 = (1 + temp6) / (1 - temp6);

            if (temp2 >= 0)
            {
                ret = true;
            }

            return ret;
        }

        public static double LevelCrossPolarization(double AMPLmain, double AMPLcross, double Mta, bool signMia)
        {
            double ret = double.NaN;
            
            double tempMta = Math.Pow(Math.Pow(10, (Math.Abs(Mta) * (-1)) / 10), 0.5d);
                        
            double tempAm = Math.Pow(10, AMPLmain / 20);
            double tempAc = Math.Pow(10, AMPLcross / 20);

            double tempAmc = double.NaN;

            if (AMPLmain >= AMPLcross)
            {
                tempAmc = tempAc / tempAm;
            }
            else
            {
                tempAmc = tempAm / tempAc;
            }

            //проверка знаков для рассчёта
            bool signMta = true;

            if (Mta < 0)
            {
                signMta = false;
            }

#warning !!!!!!!!!!!
            //if (signMia == signMta)
            if(true)
            {
#warning изменены знаки
                double top = Math.Pow(tempAmc - tempMta, 2);
                double bottom = Math.Pow(1 - tempAmc * tempMta, 2);
                ret = top / bottom;
            }
            else
            {
                double top = Math.Pow(tempAmc - tempMta, 2);
                double bottom = Math.Pow(1 - tempAmc * tempMta, 2);
                ret = top / bottom;
            }

            //ret = Math.Abs( 10 * Math.Log10(ret));


#warning !!!!!!!!!!!
            //if (!signMia)
            //{
            //    ret = -ret;
            //}

            //if (ret > 0)
            //{
            //    ret = -ret;
            //}

            return ret;
        }

        public static double LevelCrossPolarization(double Amplmin1, double Amplmin2, double Amplmax1,double Amplmax2, double Mta, bool signMia)
        {
            double ret = double.NaN;

            double tempMta = Math.Pow(Math.Pow(10, (Math.Abs(Mta) * (-1)) / 10), 0.5d);

#warning формулу поменяли

            double tempAmin = Math.Pow(10, Amplmin1 / 20);// +Math.Pow(10, Amplmin2 / 20);
            double tempAmax = Math.Pow(10, Amplmax1 / 20);// +Math.Pow(10, Amplmax2 / 20);
            double tempA = tempAmin / tempAmax;

                    
            //проверка знаков для рассчёта
            bool signMta = true;

            if (Mta < 0)
            {
                signMta = false;
            }

#warning !!!!!!!!!!!!!!!!!!!!!!!!

            //if (signMia && signMta || !signMia && !signMta)
            if(true)
            {
#warning изменены знаки
                double top = Math.Pow(tempA - tempMta, 2);
                double bottom = Math.Pow(1 - tempA * tempMta, 2);
                ret = top / bottom;
            }
            else
            {
                double top = Math.Pow(tempA - tempMta, 2);
                double bottom = Math.Pow(1 - tempA * tempMta, 2);
                ret = top / bottom;
            }

            return ret;
        }

        #endregion

        #region погрешности

        #region погрешности для КУ

        /// <summary>
        /// погрешность КУ
        /// </summary>
        /// <param name="deltaZVB"></param>
        /// <param name="deltaInstr"></param>
        /// <param name="deltaR"></param>
        /// <param name="deltaP"></param>
        /// <returns></returns>
        public static double Mistake_delta_G(double deltaZVB, double deltaInstr, double deltaR, double deltaP)
        {
            return Math.Pow(Math.Pow(deltaZVB, 2) + Math.Pow(deltaInstr, 2) + Math.Pow(deltaP, 2) + Math.Pow((1.960d * 2 * deltaR / 1.902d), 2), 0.5d);
        }

        /// <summary>
        /// рассчитать оющую погрешность ZVB для двух поляризаций
        /// </summary>
        /// <param name="delta_FreqZVB">погрешность абсолютной установки частоты</param>
        /// <param name="delta_Ampl_Main">погрешност измерения значения амплитуды на главной поляризации</param>
        /// <param name="delta_Ampl_Cross">погрешност измерения значения амплитуды на кроссовой поляризации</param>
        /// <param name="prIA_Ampl">амплитуда поляризационного отношения</param>
        /// <returns></returns>
        public static double Mistake_delta_ZVB(double delta_FreqZVB, double delta_Ampl_Main, double delta_Ampl_Cross, double prIA_Ampl)
        {
            double prIA2 = Math.Pow(Math.Abs(prIA_Ampl), 2);

            double temp1 = 2 * Mistake_delta_S21(delta_Ampl_Main) / (1 + prIA2);

            double temp2 = 2 * Mistake_delta_S21(delta_Ampl_Main) / (1 + (1 / prIA2));

            double temp3 = 2 * delta_FreqZVB;

            double ret = Calculate_srednee_kvadr(new double[] { temp1, temp2, temp3 });

            return ret;
        }

        /// <summary>
        /// рассчитать оющую погрешность ZVB для одной поляризации
        /// </summary>
        public static double Mistake_delta_ZVB(double delta_FreqZVB, double delta_Ampl)
        {
            double temp1 = Mistake_delta_S21(delta_Ampl) * 2;
            double temp2 = delta_FreqZVB * 2;

            return Calculate_srednee_kvadr(new double[] { temp1, temp2 });
        }


        /// <summary>
        /// рассчитать модуль погрешност измерения значения амплитуды ZVB
        /// </summary>
        /// <param name="delta_Ampl">погрешност измерения значения амплитуды ZVB</param>
        /// <returns>модуль</returns>
        public static double Mistake_delta_S21(double delta_Ampl)
        {
            double temp1 = delta_Ampl / 20;

            double temp2 = Math.Pow(10, temp1) - 1;
            double temp3 = 1 - Math.Pow(10, -temp1);

            double ret=(temp2 + temp3) / 2;

            return ret;
        }

        /// <summary>
        /// рассчёт инструментальной погрешности
        /// </summary>
        /// <param name="deltaGta"></param>
        /// <param name="deltaKPE"></param>
        /// <param name="Lia"></param>
        /// <param name="Гia"></param>
        /// <param name="Гta"></param>
        /// <param name="Fy"></param>
        /// <returns></returns>
        public static double Mistake_delta_instr(double delta_Gta, double delta_KPE, double delta_Lia, double delta_Lta, double delta_Гia, double delta_Гta, double delta_Fy, double delta_t)
        {
            double tempLia = (1.96 * delta_Lia) / 1.902;
            double tempLta = (1.96 * delta_Lta) / 1.902;

            return Calculate_srednee_kvadr(new double[] { delta_Gta, delta_KPE, tempLia, tempLta, delta_Гia, delta_Гta, delta_Fy, delta_t });
        }

        /// <summary>
        /// рассчитать погрешность, обусловленную наличием кроссполяризационной компоненты
        /// </summary>
        /// <param name="prIA"></param>
        /// <param name="Mta"></param>
        /// <returns></returns>
        public static double Mistake_delta_KPE(double prIA, double Mta)
        {
            double PR2 = Math.Pow(Math.Abs(prIA), 2);
            double MtaTemp = Math.Pow(10, (-(Math.Abs(Mta) / 10)));

            double tempTop = 4 * Math.Pow((PR2 * MtaTemp), 0.5d);
            double tempDown = (1 + PR2) * (1 + MtaTemp);

            return tempTop / tempDown;
        }

        /// <summary>
        /// погрешностьизмерения амплитуды коэффициента передачи анализатором
        /// </summary>
        /// <param name="Gamma_Ant"></param>
        /// <param name="Gamma_Cab"></param>
        /// <param name="Loss_Cab"></param>
        /// <returns></returns>
        public static double Mistake_delta_Г(double Gamma_Ant, double Gamma_Cab, double Loss_Cab)
        {
            double Mod_Gamma_Ant = Math.Abs(Gamma_Ant);
            double Mod_Gamma_Cab = Math.Abs(Gamma_Cab);
            double temp_Loss_Cab = Math.Pow(10, (Loss_Cab) / 10);

            double tempTop = (1 - Math.Pow(Mod_Gamma_Ant, 2)) * (1 - Math.Pow(Mod_Gamma_Cab, 2));

            double tempDown = Math.Pow((1 - (Mod_Gamma_Ant * Mod_Gamma_Cab) / temp_Loss_Cab), 2);

            return Math.Abs(tempTop / tempDown - 1);
        }

        /// <summary>
        /// погрешность установки угла
        /// </summary>
        /// <param name="Yeh">ширина ДН ТА в радианах</param>
        /// <param name="Y"></param>
        /// <param name="W"></param>
        /// <param name="Wort"></param>
        /// <param name="Y1"></param>
        /// <param name="W1"></param>
        /// <param name="W1ort"></param>
        /// <returns></returns>
        public static double Mistake_delta_angl(double Yeh, double Y, double W, double Wort, double Y1, double W1, double W1ort)
        {
            return (1.12d / Yeh) * Math.Pow((Math.Pow(Y, 2) + Math.Pow(W, 2) + Math.Pow(Wort, 2) + Math.Pow(Y1, 2) + Math.Pow(W1, 2) + Math.Pow(W1ort, 2)), 0.5d);
        }

        /// <summary>
        /// погрешность установки позиции
        /// </summary>
        /// <param name="Yeh">ширина ДН ТА</param>
        /// <param name="XY"></param>
        /// <param name="WW1a"></param>
        /// <param name="WW1l"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static double Mistake_delta_line(double Yeh, double XY, double WW1a, double WW1l, double R)
        {
            return Math.Abs((XY / R + WW1a + WW1l / R) / Yeh);
        }

        /// <summary>
        /// погрешность установки позиции
        /// </summary>
        /// <param name="Yeh">ширина ДН ТА</param>
        /// <param name="dY"></param>
        /// <param name="pY"></param>
        /// <param name="dW"></param>
        /// <param name="pW"></param>
        /// <param name="dW1"></param>
        /// <param name="pW1"></param>
        /// <param name="dY1"></param>
        /// <param name="pY1"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static double Mistake_delta_poz(double Yeh, double dY, double pY, double dW, double pW, double dW1, double pW1, double dY1, double pY1, double R)
        {
            return (1.12d/Yeh) * Math.Pow((Math.Pow(dY, 2) + Math.Pow(pY, 2) + Math.Pow(dW, 2) + Math.Pow(pW, 2) + Math.Pow(dW1, 2) + Math.Pow(pW1, 2) + Math.Pow(dY1 / R, 2) + Math.Pow(pY1 / R, 2)), 0.5d) ;
        }

        /// <summary>
        /// общая погрешность установки позиции
        /// </summary>
        /// <param name="angl"></param>
        /// <param name="line"></param>
        /// <param name="poz"></param>
        /// <returns></returns>
        public static double Mistake_delta_y(double angl, double line, double poz)
        {
            return Math.Pow(Math.Pow(angl, 2) + Math.Pow(line, 2) + Math.Pow(poz, 2), 0.5d);
        }

        /// <summary>
        /// общая погрешность ОПУ и ТМ
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Mistake_delta_Fy(double y)
        {
            return 8 * Math.Log(2) * Math.Pow(y, 2);
        }

        public static double Mistake_delta_R(double deltaR, double R)
        {
            return deltaR / R;
        }

        public static double Mistake_delta_M_for_KY(double delta_Ampl_Main, double delta_Ampl_Cross, double delta_KPE, double delta_Fy, double delta_P, double delta_Mta, double prIA_Ampl)
        {
            return Mistake_delta_M_for_SDNM(delta_Ampl_Main, delta_Ampl_Cross, delta_KPE, delta_Fy, delta_P, delta_Mta, prIA_Ampl, 0);
        }

        #endregion

        #region ПХ

        /// <summary>
        /// общая погрешность ПХ
        /// </summary>
        /// <param name="delta_Ampl"></param>
        /// <param name="delta_KPE"></param>
        /// <param name="delta_Fy"></param>
        /// <param name="delta_t"></param>
        /// <param name="delta_P"></param>
        /// <returns></returns>
        public static double Mistake_delta_Fф(double delta_Ampl, double delta_KPE, double delta_Fy, double delta_t, double delta_P)
        {
            double tempS21 = 2* Mistake_delta_S21(delta_Ampl);

            return Calculate_srednee_kvadr(new double[] { tempS21, delta_KPE, delta_Fy, delta_t, delta_P });
        }

        public static double Mistake_delta_t(double delta_t,double Yeh)
        {
            double temp=delta_t/Yeh;

            return Math.Abs(8 * Math.Log(2) * temp);
        }

        /// <summary>
        /// погрешность степени кроссполяризации M
        /// </summary>
        /// <param name="delta_Fф"></param>
        /// <param name="delta_Mta"></param>
        /// <returns></returns>
        public static double Mistake_delta_M_for_PH(double delta_Fф, double delta_Mta)
        {
            return Calculate_srednee_kvadr( new double[] { delta_Fф, delta_Mta });
        }

       
        
        #endregion


        #region СДНМ

        public static double Mistake_delta_M_for_SDNM(double delta_Ampl_Main, double delta_Ampl_Cross, double delta_KPE, double delta_Fy, double delta_P, double delta_Mta, double prIA_Ampl, double delta_t)
        {
            double prIA2 = Math.Pow(Math.Abs(prIA_Ampl), 2);

            double sAmpl = Mistake_delta_S21(delta_Ampl_Main);

            double tempS21_Main = 2 * Mistake_delta_S21(delta_Ampl_Main) / (1 + prIA2);

            double tempS21_Cross = 2 * Mistake_delta_S21(delta_Ampl_Main) / (1 + (1 / prIA2));

            double ret = Calculate_srednee_kvadr(new double[] { tempS21_Main, tempS21_Cross, delta_KPE, delta_Fy, delta_P, delta_Mta, delta_t });

            return ret;
        }

        public static double Mistake_delta_Fo(double delta_Ampl_Main, double delta_Ampl_Cross, double delta_KPE, double delta_Fy, double delta_P, double prIA_Ampl, double delta_t)
        {
            return Mistake_delta_M_for_SDNM(delta_Ampl_Main, delta_Ampl_Cross, delta_KPE, delta_Fy, delta_P, 0, prIA_Ampl, delta_t);
        }

        public static double Mistake_delta_Fo(double delta_Ampl, double delta_Fy, double delta_P, double delta_t)
        {
            double temp1 = 2 * Mistake_delta_S21(delta_Ampl);

            return Calculate_srednee_kvadr(new double[] { temp1, delta_Fy, delta_P, delta_t });
        }
        #endregion

        private static double Mistake_delta_Y_special(double delta_Fo)
        {
            double tempLeft = (1 + delta_Fo) / 0.5d;
            double tempRight = (1 - delta_Fo) / 0.5d;

            tempLeft = (1 / (4 * Math.Log(2))) * Math.Log(tempLeft);
            tempRight = (1 / (4 * Math.Log(2))) * Math.Log(tempRight);

            double ret = Math.Pow(tempLeft, 0.5d) - Math.Pow(tempRight, 0.5d);

            return ret;
        }

        public static double Mistake_delta_Y_05(double delta_Fo)
        {
            return 0.5d * Mistake_delta_Y_special(delta_Fo);
        }

        public static double Mistake_delta_O_max(double delta_Fo)
        {
            return 0.25d * Mistake_delta_Y_special(delta_Fo);
        }

        public static double Mistake_delta_Фо(double delta_Phase, double delta_R, double delta_Fy, double delta_P)
        {
            double temp_R = Math.Atan(delta_R)/(2*Math.PI);
            double temp_Fy = Math.Atan(delta_Fy) / (2 * Math.PI);
            double temp_P = Math.Atan(delta_P) / (2 * Math.PI);

            double tempPhase = delta_Phase / 360;

            return Calculate_srednee_kvadr(new double[] { tempPhase, temp_R, temp_Fy, temp_P });
        }

        /// <summary>
        /// рассчёт среднего квадратичного
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static double Calculate_srednee_kvadr(double[] elements)
        {
            double temp_Kvadr_SUM = 0;

            foreach (double el in elements)
            {
                temp_Kvadr_SUM += Math.Pow(el, 2);
            }

            return Math.Pow(temp_Kvadr_SUM, 0.5d);
        }
        #endregion
    }



    public static class MetodDrobleniaClass
    {
        /// <summary>
        /// найти минимум методом дробления
        /// </summary>
        /// <param name="Left">левая граница</param>
        /// <param name="Right">правая граница</param>
        /// <param name="Step">шаг</param>
        /// <param name="eps">точность рассчётов</param>
        /// <returns></returns>
        public static void GetMinimum(CalculationFunctionDelegate function, float Left, float Right, double eps, out double outX, out double Funct/*, bool needCalsulateAndSendStatus*/)
        {
            outX = double.NaN;
            Funct = double.NaN;
            if (Left != Right)
            {
                double F;           //значение функции на текущем шаге
                double Fpred;       //значение функции на предыдущем шаге
                double X = Left;      //значение X на текущем шаге
                double Xpred;       //значение X на предыдущем шаге

                int NumberOfDrob = 100;//на сколько частей дробить отрезки для поиска
                double Step = Math.Abs(Right - Left) / NumberOfDrob;

                #region для рассчётов статуса
                //int FullnumberOfSteps = 100;// Convert.ToInt32(Math.Abs(Right - Left) / Step);
                //int FullnumberOfDeepening = Convert.ToInt32(Math.Sqrt(Step / eps));
                //int numberOfDeepening = 1;
                //int numberOfSteps = 1;
                //bool IsDeepening = false;
                //int procentStatus = 0;
                #endregion

                #region Рассчитываем первый шаг
                Fpred = function(Left);
                Xpred = Left;

                //StatusOfCalculationEvent(1, string.Format("Первый шаг F={0}; X={1}\n\n", Fpred, Xpred));
                #endregion


                while (true)
                {
                    while (X + Step >= Right)
                    {
                        Step /= NumberOfDrob;
                    }

                    X += Step;    //делаем шаг
                    F = function(X);

                    //условие выхода
                    if (Math.Abs(F - Fpred) < eps && Math.Abs(X - Xpred) < eps)
                        break;

                    //проверка на возрастание или убывание функции
                    if (Fpred < F)
                    {
                        X -= Step;
                        if (X <= Left)
                            X = Left;
                        Step /= NumberOfDrob;


                        #region Генерация событий статуса расчёта
                        //if (needCalsulateAndSendStatus)
                        //{
                        //    //нашли зону перегиба углубляемся
                        //    IsDeepening = true;
                        //    procentStatus = Convert.ToInt32(30 + (Convert.ToDouble(numberOfDeepening) / Convert.ToDouble(FullnumberOfDeepening)) * 100);
                        //    numberOfDeepening++;

                        //    StatusOfCalculationEvent(procentStatus, string.Format("F<Fpred:\nF={0}; X={1}\nFpred={2}; Xpred={3}\nNextStep={4}\n\n", F, X + Step * NumberOfDrob, Fpred, Xpred, Step));
                        //}
                        #endregion
                    }
                    else
                    {
                        #region Генерация событий статуса расчёта
                        //if (needCalsulateAndSendStatus)
                        //{
                        //    if (!IsDeepening)
                        //    {
                        //        procentStatus = Convert.ToInt32((Convert.ToDouble(numberOfSteps) / Convert.ToDouble(FullnumberOfSteps)) * 30);
                        //        numberOfSteps++;
                        //    }

                        //    StatusOfCalculationEvent(procentStatus, string.Format("F>Fpred:\nF={0}; X={1}\nFpred={2}; Xpred={3}\nNextStep={4}\n\n", F, X, Fpred, Xpred, Step));
                        //}
                        #endregion

                        Fpred = F;
                        Xpred = X;
                    }
                }

                //StatusOfCalculationEvent(100, string.Format("Минимум найден\nF={0}; X={1}", Fpred, Xpred));
                Funct = Fpred;
                outX = Xpred;
            }
        }


        //public static event StatusOfCalculationDelegate StatusOfCalculationEvent;

        //public delegate void StatusOfCalculationDelegate(int ProcentStatus, string Text);

        public delegate double CalculationFunctionDelegate(double x);
    }



    public static class MetodKoordinatClass
    {
        public delegate double CalculationFunctionDelegate_2elementa(double x, double y);

        public static void GetMinimum_2elementa(CalculationFunctionDelegate_2elementa function, float LeftX, float RightX, float LeftY, float RightY, double eps, out double Xcoord, out double Ycoord, out double Funct)
        {
            Xcoord = double.NaN;
            Ycoord = double.NaN;
            Funct = double.NaN;

            double F;           //значение функции на текущем шаге
            double Fpred;       //значение функции на предыдущем шаге
            double X = LeftX;      //значение X на текущем шаге
            double Y = LeftY;
            double Xpred;       //значение X на предыдущем шаге
            double Ypred;


            #region первый шаг

            Fpred = function(LeftX, LeftY);
            Xpred = LeftX;
            Ypred = LeftY;

            #endregion

            MetodDrobleniaClass.CalculationFunctionDelegate funnct_constY = delegate(double x)
            {
                return function(x, Ypred);
            };

            MetodDrobleniaClass.CalculationFunctionDelegate funnct_constX = delegate(double y)
            {
                return function(Xpred, y);
            };

            while (true)
            {
                #region для Х
                MetodDrobleniaClass.GetMinimum(funnct_constY, LeftX, RightX, eps, out X, out F);

                //условие выхода
                if (Math.Abs(F - Fpred) < eps || Math.Abs(X - Xpred) < eps)
                {
                    Funct = F;
                    Xcoord = X;
                    Ycoord = Ypred;
                    break;
                }

                Xpred = X;
                Fpred = F;
                #endregion

                #region для У
                MetodDrobleniaClass.GetMinimum(funnct_constX, LeftY, RightY, eps, out Y, out F);

                //условие выхода
                if (Math.Abs(F - Fpred) < eps || Math.Abs(Y - Ypred) < eps)
                {
                    Funct = F;
                    Xcoord = Xpred;
                    Ycoord = Y;
                    break;
                }

                Ypred = Y;
                Fpred = F;

                #endregion
            }
        }

        public delegate double CalculationFunctionDelegate_3elementa(double x, double y, double z);

        public static void GetMinimum_3elementa(CalculationFunctionDelegate_3elementa function, float LeftX, float RightX, float LeftY, float RightY, float LeftZ, float RightZ, double eps, out double Xcoord, out double Ycoord, out double Zcoord, out double Funct)
        {
            Xcoord = double.NaN;
            Ycoord = double.NaN;
            Zcoord = double.NaN;
            Funct = double.NaN;

            double F;           //значение функции на текущем шаге
            double Fpred;       //значение функции на предыдущем шаге
            double X = LeftX;      //значение X на текущем шаге
            double Xpred;       //значение X на предыдущем шаге
            double Y = LeftY;
            double Ypred;
            double Z = LeftZ;
            double Zpred;

            bool FindX = false;
            bool FindY = false;
            bool FindZ = false;


            #region первый шаг

            Fpred = function(LeftX, LeftY, LeftZ);
            Xpred = LeftX;
            Ypred = LeftY;
            Zpred = LeftZ;

            #endregion

            MetodDrobleniaClass.CalculationFunctionDelegate funnct_constYZ = delegate(double x)
            {
                return function(x, Ypred, Zpred);
            };

            MetodDrobleniaClass.CalculationFunctionDelegate funnct_constXZ = delegate(double y)
            {
                return function(Xpred, y, Zpred);
            };

            MetodDrobleniaClass.CalculationFunctionDelegate funnct_constXY = delegate(double z)
            {
                return function(Xpred, Ypred, z);
            };

            while (true)
            {
                #region для Х
                MetodDrobleniaClass.GetMinimum(funnct_constYZ, LeftX, RightX, eps, out X, out F);

                //условие выхода
                if (Math.Abs(F - Fpred) < eps && Math.Abs(X - Xpred) < eps)
                {
                    Funct = F;
                    Xcoord = X;
                    Ycoord = Ypred;
                    Zcoord = Zpred;

                    FindX = true;
                }
                else
                {
                    FindX = false;
                }

                Xpred = X;
                Fpred = F;
                #endregion

                #region для У
                MetodDrobleniaClass.GetMinimum(funnct_constXZ, LeftY, RightY, eps, out Y, out F);

                //условие выхода
                if (Math.Abs(F - Fpred) < eps && Math.Abs(Y - Ypred) < eps)
                {
                    Funct = F;
                    Xcoord = Xpred;
                    Ycoord = Y;
                    Zcoord = Zpred;

                    FindY = true;
                }
                else
                {
                    FindY = false;
                }

                Ypred = Y;
                Fpred = F;

                #endregion

                #region для Z
                MetodDrobleniaClass.GetMinimum(funnct_constXY, LeftZ, RightZ, eps, out Z, out F);

                //условие выхода
                if (Math.Abs(F - Fpred) < eps && Math.Abs(Z - Zpred) < eps)
                {
                    Funct = F;
                    Xcoord = Xpred;
                    Ycoord = Ypred;
                    Zcoord = Z;

                    FindZ = true;
                }
                else
                {
                    FindZ = false;
                }

                Zpred = Z;
                Fpred = F;

                #endregion


                if (FindX && FindY && FindZ)
                    break;
            }
        }
    }
}
