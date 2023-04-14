using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    public class AnechoicChamberClass
    {
        public List<AnechoicClass> AnechoicOptions = new List<AnechoicClass>();


        public AnechoicClass FindOrCalculate_Anechoic_Options_By_Frequency(double FindFreq)
        {
            AnechoicClass ret = null;
            IFinder left;
            IFinder right;
            IFinder CoincidenceElement;

            FindResultEnum findresult = FinderClass.FindNeighborObj(AnechoicOptions.ToArray(), out left, out right, out CoincidenceElement, FindFreq);

            bool needError = false;
            string ErrorText = "";

            switch (findresult)
            {
                case FindResultEnum.FindNeighbor:
                    {
                        #region генерируем новые опции из соседних частот

                        ret = new AnechoicClass();

                        AnechoicClass TAO_left = left as AnechoicClass;
                        AnechoicClass TAO_right = right as AnechoicClass;


                        ret.Frequency = FindFreq;
                        ret.Ro = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.Ro, TAO_right.Ro);
                        ret.delta_Ro = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.delta_Ro, TAO_right.delta_Ro);

                        #endregion

                        break;
                    }
                case FindResultEnum.Coincidence:
                    {
                        ret = CoincidenceElement as AnechoicClass;
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
