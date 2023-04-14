using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;

namespace ResultOptionsClassLibrary
{

    /// <summary>
    /// Элемент поляризации
    /// </summary>
    [Serializable]
    public class PolarizationElementClass : IPolarizationElement,ICloneable
    {
        public PolarizationElementClass() { }

        public List<FrequencyElementClass> FrequencyElements = new List<FrequencyElementClass>();

        public SelectedPolarizationEnum Polarization =SelectedPolarizationEnum.Main;

        #region поисковые функции

        /// <summary>
        /// Найти элемент по частоте
        /// NullReferenceException
        /// </summary>
        /// <param name="FindFrequency"></param>
        /// <returns></returns>
        public FrequencyElementClass FindFrequencyElement(double FindFrequency,out int index)
        {
            index = -1;

            for (int i = 0; i < FrequencyElements.Count; i++)
            {
                if (FrequencyElements[i].Frequency == FindFrequency)
                {
                    index = i;
                    return FrequencyElements[i];
                }
            }
            
            new NullReferenceException("Не найдено");
            return new FrequencyElementClass();
        }

         /// <summary>
        /// Найти элемент по частоте
        /// NullReferenceException
        /// </summary>
        /// <param name="FindFrequency"></param>
        /// <returns></returns>
        public FrequencyElementClass FindFrequencyElement(double FindFrequency)
        {
            int index;

            return FindFrequencyElement(FindFrequency, out index);
        }

        /// <summary>
        /// Найти элемент по частотному элементу
        /// NullReferenceException
        /// </summary>
        /// <param name="FindFrequency"></param>
        /// <returns></returns>
        public int FindFrequencyElementIndex(FrequencyElementClass FindFrequency)
        {
            for (int i = 0; i < FrequencyElements.Count; i++)
            {
                if (FrequencyElements[i].GetHashCode() == FindFrequency.GetHashCode())
                {
                    return i;
                }
            }

            new NullReferenceException("Не найдено");
            return -1;
        }

        #endregion

        #region SelectedFrequency

        /// <summary>
        /// индекс выбранной частоты
        /// </summary>
        public int SelectedFrequencyIndex = 0;

        /// <summary>
        /// выбранная частота
        /// </summary>
        [XmlIgnore]
        public virtual FrequencyElementClass SelectedFrequency
        {
            get
            {
                return FrequencyElements[SelectedFrequencyIndex];
            }
            set
            {
                //если нет в массиве частотных элементов, то добавляем его
                if (!FrequencyElements.Contains(value))
                {
                    FrequencyElements.Add(value);
                }

                this.SelectedFrequencyIndex = this.FindFrequencyElementIndex(value);
            }
        }

        #endregion

        #region для БД
        public int id = -1;
        #endregion

        #region Static

        /// <summary>
        /// делегат для рассчёта
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        protected delegate FrequencyElementClass CalculateDelegate(List<FrequencyElementClass> s1);

        /// <summary>
        /// Рассчитать поляризационные элементы
        /// </summary>
        public static PolarizationElementClass CalculatePolarization(SumRaznPolarizEnum WhatDoing, List<PolarizationElementClass> dat)
        {
            PolarizationElementClass ret = new PolarizationElementClass();

            #region создаём делегат функции рассчёта

            CalculateDelegate Calculate = FrequencyElementClass.SumFrequencyByPower;
            switch (WhatDoing)
            {
                case SumRaznPolarizEnum.SumByPower:
                    {
                        Calculate = FrequencyElementClass.SumFrequencyByPower;
                        break;
                    }
                case SumRaznPolarizEnum.Sum:
                    {
                        Calculate = FrequencyElementClass.SumFrequency;
                        break;
                    }
                case SumRaznPolarizEnum.Razn:
                    {
                        Calculate = FrequencyElementClass.RaznFrequency;
                        break;
                    }
            }

            #endregion

            #region Проверка на одинаковое количество частотных элементов вовсех поляризациях

            for (int j = 0; j < dat.Count - 1; j++)
            {
                PolarizationElementClass dat1 = dat[j];
                PolarizationElementClass dat2 = dat[j + 1];

                if (dat1.FrequencyElements.Count != dat2.FrequencyElements.Count)
                {
                    throw new Exception("Складываемые Элементы поляризации имеют разное количество частотных элементов");
                }
            }

            #endregion

            #region создаём массив частот для всех поляризаций для последующего рассчёта по частотам

            List<List<FrequencyElementClass>> freqListList = new List<List<FrequencyElementClass>>();

            for (int j = 0; j < dat[0].FrequencyElements.Count; j++)
            {
                List<FrequencyElementClass> freqList = new List<FrequencyElementClass>();

                FrequencyElementClass freq = dat[0].FrequencyElements[j];
                freqList.Add(freq);

                for (int i = 1; i < dat.Count; i++)
                {
                    FrequencyElementClass freq2 = dat[i].FrequencyElements[j];

                    if (freq.Frequency != freq2.Frequency)
                    {
                        try
                        {
                            freq2 = dat[i].FindFrequencyElement(freq.Frequency);
                        }
                        catch
                        {
                            throw new Exception("Складываемые Элементы поляризации имеют разные частоты частотных элементов");
                        }
                    }

                    freqList.Add(freq2);
                }

                freqListList.Add(freqList);
            }

            #endregion

            foreach (List<FrequencyElementClass> freqList in freqListList)
            {
                FrequencyElementClass SUM = Calculate(freqList);

                ret.FrequencyElements.Add(SUM);
            }

            return ret;
        }

        /// <summary>
        /// Рассчитать поляризационные элементы
        /// </summary>
        public static PolarizationElementClass CalculatePolarization(SumRaznPolarizEnum WhatDoing, PolarizationElementClass dat1, PolarizationElementClass dat2)
        {
            List<PolarizationElementClass> tempList = new List<PolarizationElementClass>();
            tempList.Add(dat1);
            tempList.Add(dat2);

            return PolarizationElementClass.CalculatePolarization(WhatDoing, tempList);
        }


        public enum SumRaznPolarizEnum
        {
            Sum,
            Razn,
            Sredn,
            SumByPower
        }

        public static PolarizationElementClass InterpolationByStep(PolarizationElementClass pol, double Step, double Start, double Stop)
        {
            PolarizationElementClass ret = new PolarizationElementClass();

            foreach(FrequencyElementClass freq in pol.FrequencyElements)
            {
                FrequencyElementClass temp = FrequencyElementClass.InterpolationByStep(freq, Step, Start, Stop);

                ret.FrequencyElements.Add(temp);
            }

            return ret;
        }

        #endregion

        #region IPolarizationElement

        List<FrequencyElementClass> IPolarizationElement.FrequencyElements
        {
            get
            {
                return this.FrequencyElements;
            }
            set
            {
                this.FrequencyElements = value;
            }
        }

        SelectedPolarizationEnum IPolarizationElement.Polarization
        {
            get
            {
                return this.Polarization;
            }
            set
            {
                this.Polarization = value;
            }
        }

        #endregion

        public object Clone()
        {
            List<FrequencyElementClass> freq = new List<FrequencyElementClass>();

            foreach (FrequencyElementClass fr in this.FrequencyElements)
            {
                freq.Add(fr.Clone() as FrequencyElementClass);
            }

            PolarizationElementClass ret = this.MemberwiseClone() as PolarizationElementClass;
            ret.FrequencyElements = freq;

            return ret;
        }
    }
}
