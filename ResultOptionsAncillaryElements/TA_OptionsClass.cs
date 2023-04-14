using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// характеристики технической антенны на одной частоте
    /// </summary>
    [Serializable]
    public class TA_OptionsClass : IFinder
    {
        public double Frequency = 1;
        public double GainTA = 1;
        public double delta_GainTA = 1;
        public double MTA = 1;
        public double GammaTA = 1;
        public double delta_MTA = 1;
        public double DN_WidthTA = 1;


        double IFinder.FindElement
        {
            get { return Frequency; }
        }
    }
}
