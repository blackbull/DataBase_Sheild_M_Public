using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using ResultTypesClassLibrary;
using System.Windows.Forms;

namespace DB_Interface
{
    public interface I_ARM_Form : ISaver_ToDataBase
    {
        void LoadNodesToTree(ref TreeView NewTreeSortByAnten, ref TreeView NewTreeSortByMeasurement, DateTime StartDate, DateTime StopDate, bool LoadAllAntens);

        MainOptionsClass LoadOnlyOptions(DBLoaderPackClass Pack);
        AntennOptionsClass LoadIA(DBLoaderPackClass Pack);
        AntennOptionsClass LoadTA(DBLoaderPackClass Pack);
        ResultType_MAINClass LoadResult(DBLoaderPackClass Pack, bool LoadFullResult, bool LoadResultData = true);
        List<ResultType_MAINClass> LoadALLResult(DBLoaderPackClass Pack);
        string GetFullNameByResult(ResultType_MAINClass Result);
        int GetMainID(DBLoaderPackClass Pack);

        void HideFrequency(DBLoaderPackClass Pack);

        /// <summary>
        /// Загрузить все!! измрения
        /// </summary>
        /// <returns></returns>
        List<ResultType_MAINClass> LoadALLResult();
    }
}
