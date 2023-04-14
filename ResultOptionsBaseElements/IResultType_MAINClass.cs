using System;
using System.Collections.Generic;

namespace ResultOptionsClassLibrary
{
    public interface IResultType_MAIN
    {
        AntennOptionsClass Antenn { get; set; }
        MainOptionsClass MainOptions { get; set; }
        //MeasurementDataClass MeasurementData { get; set; }
        AntennOptionsClass Zond { get; set; }

        PolarizationElementClass SelectedPolarization { get; set; }

        IList<IPolarizationElement> PolarizationElements { get; }

        int id { get; set; }

        //void SetMainToAllFrequencysElements();
    }

    public interface IResultType_КУ : IResultType_MAIN
    {
        PolarizationElementClass SUM_Polarization { get; set; }
        PolarizationElementClass Main_Polarization { get; set; }
        PolarizationElementClass Cross_Polarization { get; set; }
    }

    public interface IResultType_ДН : IResultType_MAIN
    {
       
    }

    public interface IResultType_ПХ : IResultType_MAIN
    {

    }

    public interface IResultType_СДНМ : IResultType_MAIN
    {
        PolarizationElementClass SUM_Polarization { get; }
        PolarizationElementClass Main_Polarization { get; set; }
        PolarizationElementClass Cross_Polarization { get; set; }
    }

    public interface IResultType_Union : IResultType_MAIN
    {
       
    }


    public interface IPolarizationElement
    {
        List<FrequencyElementClass> FrequencyElements { get; set; }
        SelectedPolarizationEnum Polarization { get; set; }
        FrequencyElementClass SelectedFrequency { get; }
    }


    public enum SelectedPolarizationEnum
    {
        None = 0,
        Main = 1,
        Cross = 2,
        Sum = 3
    }
}
