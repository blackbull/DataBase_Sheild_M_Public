using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
    [Serializable]
    public class MainOptionsClass : ICloneable
    {
        public MainOptionsClass() { }

        public MeasurementParametrsClass Parameters = new MeasurementParametrsClass();

        public string Name = "";
        public string Descriptions = "";
        public DateTime Date = DateTime.Now;
        public CalculationResultTypeEnum CalculationResultType = CalculationResultTypeEnum.None;

        /// <summary>
        /// Имя типа измерения
        /// </summary>
        public string MeasurementResultTypeName = "";
        /// <summary>
        /// тип измерения
        /// </summary>
        public MeasurementTypeEnum MeasurementResultType;
        /// <summary>
        /// Описание типа измерения
        /// </summary>
        public string MeasurementResultTypeDescription = "";


        #region обработка описания измерений

        public static bool DecodingDescription(MainOptionsClass restemp, out int portN, out int tbN, out int startF, out int stopF)
        {
            bool ret = false;

            string port = "";
            string tb = "";
            portN = -1; tbN = -1; startF = -1; stopF = -1;

            try
            {
                if (restemp.Descriptions != "")
                {
                    List<string> dis2 = new List<string>(restemp.Descriptions.Split('%'));

                    dis2[0] = dis2[0].Trim(' ');
                    dis2[1] = dis2[1].Trim(' ');

                    port = dis2[0].Substring(0, 1);
                    tb = dis2[0].Substring(1);

                    portN = Convert.ToInt32(port);
                    tbN = Convert.ToInt32(tb);

                    List<string> disF = new List<string>(dis2[1].Split('f'));

                    startF = Convert.ToInt32(disF[0]);
                    stopF = Convert.ToInt32(disF[1]);

                    ret = true;
                }
            }

            catch { }

            return ret;
        }

        public static bool DecodingDescription(MainOptionsClass restemp, out string Decode)
        {
            int portN, tbN, startF, stopF;
            bool ret = false;
            Decode = restemp.Descriptions;

            ret = DecodingDescription(restemp, out portN, out tbN, out startF, out stopF);

            if(ret)
            {
                string HV = "ХЗ";

                if (restemp.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || restemp.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут) HV = "Horizontal plane";
               else if (restemp.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || restemp.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан) HV = "Vertical plane";

                Decode = string.Format("Port{0}, {1}, TB{2}", portN, HV, tbN);
            }

            return ret;
        }

        #endregion


        public object Clone()
        {
            MainOptionsClass ret=this.MemberwiseClone() as MainOptionsClass;
            ret.Parameters = this.Parameters.Clone() as MeasurementParametrsClass;
            return ret;
        }
    }
}
