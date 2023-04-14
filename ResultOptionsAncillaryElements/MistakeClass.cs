using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
    [Serializable]
    public class MistakeClass
    {
        public MistakeClass() { }

        public MistakeClass(double NewMistake)
            : this(NewMistake,MistakeTypeEnum.dB, 1) { }

        public MistakeClass(double NewMistake,MistakeTypeEnum NewMT, double NewValue)
        {
            Mistake = NewMistake;
            Value = NewValue;
            MT = NewMT;
        }

        /// <summary>
        /// Погрешность 
        /// </summary>
        public double Mistake = double.NaN;

        public double Value = double.NaN;

        MistakeTypeEnum MT = MistakeTypeEnum.dB;

        string _AddingText = "";

        public string AddingText
        {
            get
            {
                if (MT == MistakeTypeEnum.dB)
                {
                    _AddingText = "дБ";
                }
                else
                {
                    _AddingText = "°";
                }

                return _AddingText;
            }
        }

        public double MistakeProcent
        {
            get
            {
                return Mistake * 100;
            }
        }

        public double MistakeMinus
        {
            get
            {
                if (MT == MistakeTypeEnum.dB)
                {
                    return CalculationClass.Convert_Unity_to_dB(1 - Mistake);
                }
                else
                {
                    return Math.Round(Value * Mistake, 2);
                }
            }
        }

        public double MistakePlus
        {
            get
            {
                if (MT == MistakeTypeEnum.dB)
                {
                    return CalculationClass.Convert_Unity_to_dB(1 + Mistake);
                }
                else
                {
                    return Math.Round(Value * Mistake, 2);
                }
            }
        }

        public string MistakeString
        {
            get
            {
                string proc = Math.Round(Math.Abs(this.MistakeProcent), 1).ToString();
                string Min = Math.Round(Math.Abs(this.MistakeMinus), 2).ToString();
                string Plus = Math.Round(Math.Abs(this.MistakePlus), 2).ToString();

                string ret = string.Format("±{0}% (+{1} {3}/-{2} {3})", proc, Plus, Min, AddingText);
                return ret;
            }
        }

        public override string ToString()
        {
            return this.MistakeString;
        }

        //public override string ToString(double newValue)
        //{
        //    this.Value = newValue;
        //    this.MT = MistakeTypeEnum.Grad;

        //    string tempstring = this.MistakeString;

        //    this.MT = MistakeTypeEnum.dB;

        //    return tempstring;
        //}
    }

    public enum MistakeTypeEnum
    {
        dB,
        Grad
    }
}
