using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    public class CableClass
    {
        public List<CableOptionsClass> CableOptions = new List<CableOptionsClass>();

        public CableOptionsClass FindOrCalculate_Cable_Options_By_Frequency(double FindFreq)
        {
            CableOptionsClass ret = null;
            IFinder left;
            IFinder right;
            IFinder CoincidenceElement;

            FindResultEnum findresult = FinderClass.FindNeighborObj(CableOptions.ToArray(), out left, out right, out CoincidenceElement, FindFreq);

            bool needError = false;
            string ErrorText = "";

            switch (findresult)
            {
                case FindResultEnum.FindNeighbor:
                    {
                        #region генерируем новые опции из соседних частот

                        ret = new CableOptionsClass();

                        CableOptionsClass TAO_left = left as CableOptionsClass;
                        CableOptionsClass TAO_right = right as CableOptionsClass;


                        ret.Frequency = FindFreq;
                        ret.Loss = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.Loss, TAO_right.Loss);
                        ret.Gamma_Cab = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.Gamma_Cab, TAO_right.Gamma_Cab);

                        #endregion

                        break;
                    }
                case FindResultEnum.Coincidence:
                    {
                        ret = CoincidenceElement as CableOptionsClass;
                        break;
                    }
                case FindResultEnum.FindOnlyLeft:
                    {
                        needError = true;
                        ErrorText = "Ошибка поиска частоты. Найдены частоты только меньше искомой.";

                        break;
                    }
                case FindResultEnum.FindOnlyRight:
                    {
                        needError = true;
                        ErrorText = "Ошибка поиска частоты. Найдены частоты только больше искомой.";

                        break;
                    }
                case FindResultEnum.Error:
                    {
                        needError = true;
                        ErrorText = "Ошибка поиска частоты. Массив частот пустой либо неизвестная ошибка";
                        break;
                    }
            }

            if (needError)
            {
                throw new Exception(ErrorText);
            }

            return ret;
        }
    }
}
