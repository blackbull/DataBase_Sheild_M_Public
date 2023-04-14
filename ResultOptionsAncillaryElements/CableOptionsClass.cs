using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// опции кабеля
    /// </summary>
    [Serializable]
    public class CableOptionsClass:IFinder
    {
        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public CableOptionsClass() { }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="NewFreq">Частота</param>
        /// <param name="NewL">потери</param>
        /// <param name="NewГ">КСВ</param>
        public CableOptionsClass(double NewFreq, double NewL, double NewГ)
        {
            Frequency = NewFreq;
            Loss = NewL;
            Gamma_Cab = NewГ;
        }

        /// <summary>
        /// Частота, мГц
        /// </summary>
        public double Frequency = 0;
        
        /// <summary>
        /// потери
        /// </summary>
        public double Loss = 0;

        /// <summary>
        /// КСВ
        /// </summary>
        public double Gamma_Cab = 0;

        double IFinder.FindElement
        {
            get { return Frequency; }
        }
    }
}
