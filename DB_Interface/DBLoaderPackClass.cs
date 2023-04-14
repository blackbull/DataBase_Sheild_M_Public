using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB_Interface
{
    public class DBLoaderPackClass
    {
        public FormShowTypeEnum FormShowType = FormShowTypeEnum.None;

        public MeasurementTagTypeEnum MeasurementTagType = MeasurementTagTypeEnum.None;

        public object ObjectForLoad = null;

        public string SpesialText = "";
    }

    public enum FormShowTypeEnum
    {
        None,
        Main,
        Result,
        Antenn
    }


    /// <summary>
    /// тип измерения записанного в таг LIST
    /// </summary>
    public enum MeasurementTagTypeEnum
    {
        None = 0,
        DN_Normal = 1,
        SDNM_Sum = 2,
        SDNM_Main = 3,
        SDNM_Cross = 4,
        KY = 5,
        Union = 6
    }
}
