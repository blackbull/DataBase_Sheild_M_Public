using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Xml.Serialization;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// опции антенн
    /// </summary>
    [Serializable]
    public class AntennOptionsClass:IShowObject
    {
        public AntennOptionsClass()
        {
        }

        public int id = -1;

        public string Name = "";
        public string Description = "";
        public string ZAVNumber = "";
        public bool UsingAsZond = false;

        [XmlIgnore]//не нужна лишняя инфа в XML
        public List<TA_OptionsClass> TA_OptionsList = new List<TA_OptionsClass>();

        public TA_OptionsClass FindOrCalculate_TA_Options_By_Frequency(double FindFreq)
        {
            TA_OptionsClass ret = null;
            IFinder left;
            IFinder right;
            IFinder CoincidenceElement;

            FindResultEnum findresult = FinderClass.FindNeighborObj(TA_OptionsList.ToArray(), out left, out right, out CoincidenceElement, FindFreq);

            bool needError = false;
            string ErrorText = "";

            switch (findresult)
            {
                case FindResultEnum.FindNeighbor:
                    {
                        #region генерируем новые опции из соседних частот

                        ret = new TA_OptionsClass();

                        TA_OptionsClass TAO_left = left as TA_OptionsClass;
                        TA_OptionsClass TAO_right = right as TA_OptionsClass;


                        ret.Frequency = FindFreq;
                        ret.DN_WidthTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.DN_WidthTA, TAO_right.DN_WidthTA);
                        ret.GammaTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.GammaTA, TAO_right.GammaTA);
                        ret.delta_MTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.delta_MTA, TAO_right.delta_MTA);
                        ret.GainTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.GainTA, TAO_right.GainTA);
                        ret.delta_GainTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.delta_GainTA, TAO_right.delta_GainTA);
                        ret.MTA = CalculationClass.LinInterpolation(TAO_left.Frequency, TAO_right.Frequency, FindFreq, TAO_left.MTA, TAO_right.MTA);

                        #endregion

                        break;
                    }
                case FindResultEnum.Coincidence:
                    {
                        ret = CoincidenceElement as TA_OptionsClass;
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

        public override string ToString()
        {
            string ret = "";

            if (ZAVNumber != "")
            {
                ret = Name + " зав. №" + ZAVNumber;
            }
            else
            {
                ret = Name;
            }

            return ret;
        }


        #region IShowObject

        string IShowObject.Name
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        string IShowObject.ZavNumber
        {
            get
            {
                return ZAVNumber;
            }
            set
            {
                ZAVNumber = value;
            }
        }

        string IShowObject.Discription
        {
            get
            {
               return Description;
            }
            set
            {
                Description = value;
            }
        }

        string IShowObject.FullName
        {
            get { return ToString(); }
        }

        #endregion
    }
}
