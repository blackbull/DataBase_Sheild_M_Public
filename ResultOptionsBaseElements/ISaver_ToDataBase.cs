using System;
using System.Collections.Generic;
using ResultOptionsClassLibrary;
using GeneralInterfaces;

namespace ResultOptionsClassLibrary
{
    public interface ISaver_ToDataBase
    {
        System.Collections.Generic.List<AntennOptionsClass> LoadAllAntenn();
        void Save_Antenn(object Sender, AntennOptionsClass SaveObj);

        event AddNewAntennDelegate AddNewAntennEvent;

        Cables_Sheild_M_Class LoadCablesParametrs();
        AdjustOptionsClass LoadAdjustOptions();
        ZVB14_ParametrsClass LoadZVB_Parametrs();

        void CreateMeasurementTable(string Name);
        void AddToMeasurementTable(string Name, int id_freq, float coord, float Data, float Data2);
        int SaveMainTable(IResultType_MAIN SaveObj);
        int SaveFrequencyElement(FrequencyElementClass SaveObj, int idMain, int PolarizationType, ref bool CreateResultTable);
        void SaveFrequencyElement(IList<FrequencyElementClass> Frequency, int idMain, int PolarizationType, ref bool CreateResultTable);
        int SavePositioningParameters(MeasurementParametrsClass SaveObj);
        int SaveZVBParameters(MeasurementParametrsClass SaveObj);
        int SaveSegmentTable(ISegmentTableElementClass SaveObj, int idMain);
        void SaveSegmentTable(List<ISegmentTableElementClass> SaveObj, int idMain);
        void SaveResultElementS(List<ResultElementClass> ResultAmpl_PhaseElements, string TableName, int id_freq);

        void DeleteFullResult(int idMain);
        void DeleteAntenn(int idAnten);
        void ClearMeasurementTable(string Name, int freqID);
    }

    public delegate void AddNewAntennDelegate(object Sender, AntennOptionsClass Obj);
}
