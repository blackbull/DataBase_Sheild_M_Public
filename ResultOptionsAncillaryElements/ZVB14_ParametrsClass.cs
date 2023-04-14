using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// класс параметров ZVB14
    /// </summary>
    public class ZVB14_ParametrsClass
    {
        /// <summary>
        /// набор погрешностей, распределенных по диапазонам
        /// </summary>
        public List<ZVB_Parametrs_ElementClass> ZVB_Parametrs_ElementsList = new List<ZVB_Parametrs_ElementClass>();
        
        /// <summary>
        /// найти значения погрешности по указанным частоте и амплитуде
        /// </summary>
        /// <param name="FindFreq"></param>
        /// <param name="FindAMpl"></param>
        /// <returns></returns>
        public ZVB_Parametrs_ElementClass Find_ZVB_Parametrs(double FindFreq, double FindAMpl)
        {
            ZVB_Parametrs_ElementClass ret = null;

            if (ZVB_Parametrs_ElementsList != null)
            {
                foreach (ZVB_Parametrs_ElementClass El in ZVB_Parametrs_ElementsList)
                {
                    if (FindFreq >= El.FreqDown && FindFreq <= El.FreqUp
                        && FindAMpl >= El.AmplDown && FindAMpl <= El.AmplUp)
                    {
                        return El;
                    }
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// элемент диапазона частот и амплитуд ZVB14
    /// </summary>
    public class ZVB_Parametrs_ElementClass
    {
        public double FreqDown = 0;
        public double FreqUp = 0;

        public double AmplDown = 0;
        public double AmplUp = 0;


        public double delta_Ampl = 0;
        public double delta_Phase = 0;
        public double delta_Freq = 0;
    }
}
