using System;
using System.Collections.Generic;
using System.Text;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    public class AnechoicClass:IFinder
    {
         /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public AnechoicClass() { }

        /// <summary>
        /// конструктор
        /// </summary>
       /// <param name="NewFreq">частота</param>
        /// <param name="NewRo">коэффициент безэховости</param>
        /// <param name="Newdelta_Ro">погрешность коэффициента безэховости</param>
        public AnechoicClass(double NewFreq, double NewRo, double Newdelta_Ro)
        {
            Frequency = NewFreq;
            Ro = NewRo;
            delta_Ro = Newdelta_Ro;
        }

        /// <summary>
        /// Частота, мГц
        /// </summary>
        public double Frequency = 0;
        
        /// <summary>
        /// коэффициент безэховости
        /// </summary>
        public double Ro = 0;

        /// <summary>
        /// погрешность коэффициента безэховости
        /// </summary>
        public double delta_Ro = 0;

        double IFinder.FindElement
        {
            get { return Frequency; }
        }

    }
}
