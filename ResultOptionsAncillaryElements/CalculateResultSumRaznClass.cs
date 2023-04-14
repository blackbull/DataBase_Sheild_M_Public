using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
    static public class CalculateResultSumRaznClass
    {
        /// <summary>
        /// Рассчитать сумму/разность
        /// </summary>
        /// <param name="res1">слагаемое/уменьшаемое (массив амплитуды)</param>
        /// <param name="res2">слагаемое/вычитаемое (массив амплитуды)</param>
        /// <param name="PogreshnostCoordinate">допуск по координатам</param>
        /// <param name="WhatCalculate">что рассчитывать</param>
        public static IList<ResultElementClass> CalculateSumorRazn(IList<ResultElementClass> res1, IList<ResultElementClass> res2, double PogreshnostCoordinate, SumRaznEnum WhatCalculate)
        {
            List<ResultElementClass> ret = new List<ResultElementClass>();


            if (res1.Count != res2.Count)
            {
                throw new Exception("Разный размер массивов исходных данных");
            }


            for (int i = 0; i < res1.Count; i++)
            {
                if (res1[i].Cordinate > res2[i].Cordinate + PogreshnostCoordinate || res1[i].Cordinate < res2[i].Cordinate - PogreshnostCoordinate)
                {
                    throw new Exception("Исходные данные имеют разный (не интерполируемый) шаг измерения");
                }
                else
                {
                    double NewData1 = double.NaN;

                    switch (WhatCalculate)
                    {
                        case SumRaznEnum.Sum_In_Raz:
                            {
                                //-------------------Skiffartur 20141118
                                //NewData1 = res1[i].Ampl_dB + res2[i].Ampl_dB;
                                //+++++++++++++++++++Skiffartur 20141118
                                NewData1 = 10 * Math.Log10(Math.Pow(10, res1[i].Ampl_dB / 10) + Math.Pow(10, res2[i].Ampl_dB / 10));
                                //===================Skiffartur 20141118
                                break;
                            }
                        case SumRaznEnum.Razn:
                            {
                                NewData1 = res1[i].Ampl_dB - res2[i].Ampl_dB;
                                break;
                            }
                        case SumRaznEnum.SumByPower:
                            {
                                NewData1 = 10 * Math.Log10(Math.Pow(10, res1[i].Ampl_dB / 10) + Math.Pow(10, res2[i].Ampl_dB / 10));
                                //NewData1 = Math.Pow(Math.Pow(res1[i].Ampl_dB, 2) + Math.Pow(res2[i].Ampl_dB, 2), 0.5d);
                                break;
                            }
                        case SumRaznEnum.Sum_In_DB:
                            {
                                NewData1 = res1[i].Ampl_dB + res2[i].Ampl_dB;
                                break;
                            }
                        default:
                            {
                                throw new Exception("Не определённый тип рассчёта: " + WhatCalculate.ToString());
                            }
                    }

                    ResultElementClass temp = new ResultElementClass(res1[i].Cordinate, NewData1, double.NaN);
                    ret.Add(temp);
                }
            }

            return ret;
        }


        /// <summary>
        /// Рассчитать сумму элементов массива, при выборе рассчёта вычитания - все элементы массива будут вычтены из первого
        /// </summary>
        /// <param name="SumList">слагаемые  (массив амплитуд)</param>
        /// <param name="PogreshnostCoordinate">допуск по координатам</param>
        /// <param name="SumType"> Что рассчитать </param>
        public static IList<ResultElementClass> CalculateSumorRazn(IList<IList<ResultElementClass>> SumList, double PogreshnostCoordinate, SumRaznEnum SumType)
        {
            IList<ResultElementClass> ret = new List<ResultElementClass>();

            if (SumList.Count < 2)
            {
                throw new Exception("Рассчитываемых массивов меньше 2");
            }
            else
            {
                ret = SumList[0];

                for (int i = 1; i < SumList.Count; i++)
                {
                    ret = CalculateResultSumRaznClass.CalculateSumorRazn(ret, SumList[i], PogreshnostCoordinate, SumType);
                }
            }

            return ret;
        }

        /// <summary>
        /// Рассчитать среднее арифметическое 
        /// </summary>
        /// <param name="SumList"></param>
        /// <param name="PogreshnostCoordinate">допуск по координатам</param>
        /// <returns></returns>
        public static IList<ResultElementClass> CalculateSredneeArifm(IList<IList<ResultElementClass>> SumList, double PogreshnostCoordinate)
        {
            IList<ResultElementClass> ret = CalculateResultSumRaznClass.CalculateSumorRazn(SumList, PogreshnostCoordinate, SumRaznEnum.Sum_In_Raz);

            for (int i = 0; i < ret.Count; i++)
            {
                //-------------------Skiffartur 20141118
                //ret[i].Ampl_dB = ret[i].Ampl_dB / SumList.Count;
                //+++++++++++++++++++Skiffartur 20141118
                double temp1 =10*  Math.Log10(SumList.Count);

                ret[i].Ampl_dB = ret[i].Ampl_dB - temp1;
                //===================Skiffartur 20141118
            }

            return ret;
        }
    }

    public enum SumRaznEnum
    {
        Sum_In_Raz = 0,
        Razn = 1,
        SumByPower = 2,
        Sum_In_DB = 3
    }
}
