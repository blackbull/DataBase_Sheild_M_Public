using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// Элемент значений
    /// </summary>
    [Serializable]
    public class ResultElementClass : IFinder,ICloneable
    {
        public ResultElementClass() { }
        public ResultElementClass(double newCordinate, double newAmpl_dB, double newPhase_degree)
        {
            Cordinate = newCordinate;
            Ampl_dB = newAmpl_dB;
            Phase_degree = newPhase_degree;
        }

        public double Cordinate;
        public double Ampl_dB;
        public double Phase_degree;

        double IFinder.FindElement
        {
            get { return Cordinate; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
