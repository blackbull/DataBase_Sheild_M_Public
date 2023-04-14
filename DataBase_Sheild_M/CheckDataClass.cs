using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBase_Sheild_M
{
    public static class CheckDataClass
    {
        /// <summary>
        /// True - good
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static bool CheckForBad(double D)
        {
            bool ret = false;

            if (D != null)
            {
                if (!double.IsNaN(D) && !double.IsInfinity(D))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// True - good
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static bool CheckForBad(ResultOptionsClassLibrary.ComplexClass D)
        {
            bool ret = false;

            if (D != null)
            {
                if (!double.IsNaN(D.Real) && !double.IsInfinity(D.Real) && !double.IsNaN(D.Imaginary) && !double.IsInfinity(D.Imaginary))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// True - good
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static bool CheckForBad(ResultOptionsClassLibrary.MistakeClass D)
        {
            bool ret = false;

            if (D != null)
            {
                if (!double.IsNaN(D.Mistake) && !double.IsInfinity(D.Mistake))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// True - good
        /// </summary>
        /// <param name="D"></param>
        /// <returns></returns>
        public static bool CheckForBad(ResultOptionsClassLibrary.PointDouble D)
        {
            bool ret = false;

            if (D != null)
            {
                if (CheckForBad(D.X) && CheckForBad(D.Y))
                {
                    ret = true;
                }
            }

            return ret;
        }


        public static string CheckAndConvertToString(double D)
        {
            string ret = "NaN";

            if (CheckForBad(D))
            {
                double temp = Math.Round(D, 2);
                ret = temp.ToString();
            }

            return ret;
        }

        public static string CheckAndConvertToString(ResultOptionsClassLibrary.MistakeClass D)
        {
            string ret = "(NaN)";

            if (CheckForBad(D))
            {
                ret = D.ToString();
            }

            return ret;
        }
    }
}
