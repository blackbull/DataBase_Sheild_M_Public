using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataBase_Sheild_M;
using ResultOptionsClassLibrary;
using ResultTypesClassLibrary;
using System.Windows.Forms;
using System.Drawing;
using DB_Interface;

namespace XML_Sheild_M_Loader
{
    public class XML_ResultLoaderClass : I_ARM_Form
    {
        public XML_ResultLoaderClass(List<ResultType_MAINClass> NewResults)
        {
            this.AllResults = NewResults;
            this.LoadAllAntens(this.AllResults);
        }


        void I_ARM_Form.LoadNodesToTree(ref System.Windows.Forms.TreeView NewTreeSortByAnten, ref System.Windows.Forms.TreeView NewTreeSortByMeasurement, DateTime StartDate, DateTime StopDate, bool LoadAllAntens = false)
        {
            TreeSortByAnten = NewTreeSortByAnten;
            TreeSortByMeasurement = NewTreeSortByMeasurement;


            FillAntenTreeView();

            foreach (ResultType_MAINClass res in this.AllResults)
            {
                this.AddMainRowToTreeView(res);
            }
        }

        #region Функции загрузки общей структуры дерева

        protected void FillAntenTreeView()
        {
            foreach (AntennOptionsClass Anten in this.AllAntenn)
            {
                AddAntennTreeViewAnten(Anten);
            }
        }

        protected void AddAntennTreeViewAnten(AntennOptionsClass Anten)
        {
            if (TreeSortByAnten != null)
            {
                TreeSortByAnten.Nodes.Add(Anten.id.ToString(), Anten.Name + " зав. №" + Anten.ZAVNumber);

                DBLoaderPackClass Pack = new DBLoaderPackClass();
                Pack.FormShowType = FormShowTypeEnum.Antenn;
                Pack.ObjectForLoad = Anten;
                Pack.SpesialText = "Это антенна " + Anten.id.ToString();
                TreeSortByAnten.Nodes[Anten.id.ToString()].Tag = Pack;

                //выделение нода
                TreeSortByAnten.Nodes[Anten.id.ToString()].BackColor = Color.LightGray;
            }
        }


        #endregion

        #region Основыные переменные

        public TreeView TreeSortByAnten = null;
        public TreeView TreeSortByMeasurement = null;

        public List<AntennOptionsClass> AllAntenn = new List<AntennOptionsClass>();
        public List<ResultType_MAINClass> AllResults = new List<ResultType_MAINClass>();

        bool NonameAntenIsUsing = false;
        #endregion

        protected void LoadAllAntens(List<ResultType_MAINClass> NewResults)
        {
            foreach (ResultType_MAINClass res in NewResults)
            {
                this.LoadAllAntens(res.Antenn);
                this.LoadAllAntens(res.Zond);
            }
        }

        protected void LoadAllAntens(AntennOptionsClass NewAnten)
        {
            bool AddAnten = true;

            foreach (AntennOptionsClass ant in AllAntenn)
            {
                if (NewAnten.id == ant.id)
                {
                    AddAnten = false;
                    break;
                }
            }

            if (AddAnten)
            {
                AllAntenn.Add(NewAnten);
            }
        }

        #region Создание нодов Main

        protected void AddMainRowToTreeView(ResultType_MAINClass mainRow)
        {
            TreeNode tempMain = this.CreateMainTreeNode(mainRow);
            TreeNode tempMain2 = tempMain.Clone() as TreeNode;

            this.AddMainRowToTreeViewMeasurementType(tempMain);
            this.AddMainRowToTreeViewAntennType(mainRow, tempMain2);
        }

        protected void AddMainRowToTreeViewMeasurementType(TreeNode tempMain)
        {
            TreeSortByMeasurement.Nodes.Add(tempMain);
        }

        protected void AddMainRowToTreeViewAntennType(ResultType_MAINClass mainRow, TreeNode tempMain)
        {
            if (TreeSortByAnten != null)
            {
                TreeNode AntennNode = TreeSortByAnten.Nodes[mainRow.Antenn.id.ToString()];

                if (AntennNode != null)
                {
                    AntennNode.Nodes.Add(tempMain);
                }
                else
                {
                    if (mainRow.Antenn.id >= 0)
                    {
                        this.AddAntennTreeViewAnten(mainRow.Antenn);
                        this.AddMainRowToTreeViewAntennType(mainRow, tempMain);
                    }
                    else
                    {
                        if (!NonameAntenIsUsing)
                        {
                            TreeSortByAnten.Nodes.Add("NoAntenn", "Без антенны");
                            NonameAntenIsUsing = true;
                        }
                        TreeSortByAnten.Nodes["NoAntenn"].Nodes.Add(tempMain);
                    }
                }
            }
        }

        protected TreeNode CreateMainTreeNode(ResultType_MAINClass mainRow)
        {
            TreeNode tempMain = new TreeNode(mainRow.MainOptions.Date.ToShortDateString() + " | " + mainRow.ToString() + " | " + mainRow.MainOptions.Descriptions);

            DBLoaderPackClass Pack = new DBLoaderPackClass();
            Pack.FormShowType = FormShowTypeEnum.Main;
            Pack.ObjectForLoad = mainRow;
            Pack.SpesialText = string.Format("Это Main {0}", mainRow.id);

            tempMain.Tag = Pack; ;



            #region получаем список результатов

            List<TreeNode> SpisokResultatovNode;

            //проверка: является ли измерение рассчётным
            if (mainRow.MainOptions.CalculationResultType != CalculationResultTypeEnum.None)
            {
                if (this.CreateCalculationResultNode(mainRow, out SpisokResultatovNode))
                {
                    foreach (TreeNode tr in SpisokResultatovNode)
                    {
                        tempMain.Nodes.Add(tr);
                    }
                }
            }
            else
            {
                if (this.CreateResultNode(mainRow, out SpisokResultatovNode))
                {
                    foreach (TreeNode tr in SpisokResultatovNode)
                    {
                        tempMain.Nodes.Add(tr);
                    }
                }
            }

            #endregion

            return tempMain;
        }

        #endregion

        #region создание нодов с результатами измерений

        protected bool CreateResultNode(ResultType_MAINClass res, out List<TreeNode> SpisokResultatovNode)
        {
            SpisokResultatovNode = new List<TreeNode>();

            bool needAddResultNode = true;


            //выбираем как считывать результаты в соответствии с типом измерения.
            if (res.MainOptions.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма || res.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || res.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
            {
                TreeNode temp = this.GetPolarizationNode(res, res.SelectedPolarization.FrequencyElements, "Результат", MeasurementTagTypeEnum.DN_Normal);

                for (int i = 0; i < temp.Nodes.Count; i++)
                {
                    SpisokResultatovNode.Add(temp.Nodes[i]);
                }
            }
            else
            {
                if (res.MainOptions.MeasurementResultType == MeasurementTypeEnum.Коэффицент_усиления)
                {
                    TreeNode temp = this.GetPolarizationNode(res, res.SelectedPolarization.FrequencyElements, "Результат КУ", MeasurementTagTypeEnum.KY);

                    if (temp.Nodes.Count != 0)
                    {
                        temp.Nodes[0].Text = "Результат КУ";
                        SpisokResultatovNode.Add(temp.Nodes[0]);
                    }
                }
                else
                {
                    if (res.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан || res.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                    {
                        ResultTypeClassСДНМ resSDNM = res as ResultTypeClassСДНМ;

                        SpisokResultatovNode.Add(this.GetPolarizationNode(resSDNM, resSDNM.SUM_Polarization.FrequencyElements, "Polarization Sum", MeasurementTagTypeEnum.SDNM_Sum));
                        SpisokResultatovNode.Add(this.GetPolarizationNode(resSDNM, resSDNM.Main_Polarization.FrequencyElements, "Polarization Main", MeasurementTagTypeEnum.SDNM_Main));
                        SpisokResultatovNode.Add(this.GetPolarizationNode(resSDNM, resSDNM.Cross_Polarization.FrequencyElements, "Polarization Cross", MeasurementTagTypeEnum.SDNM_Cross));
                    }
                    else
                    {
                        //неизвестный тип измерения или ошибка в БД
                    }
                }
            }

            return needAddResultNode;
        }

        protected bool CreateCalculationResultNode(ResultType_MAINClass res, out List<TreeNode> SpisokResultatovNode)
        {
            bool needAddResultNode = false;
            SpisokResultatovNode = new List<TreeNode>();

            if (res is ResultTypeClassUnion)
            {
                ResultTypeClassUnion resUnion = res as ResultTypeClassUnion;

                needAddResultNode = true;

                string addText = res.MainOptions.CalculationResultType.ToString();
                SpisokResultatovNode.Add(this.GetPolarizationNode(res, res.SelectedPolarization.FrequencyElements, addText, MeasurementTagTypeEnum.Union));

                //добавляем исходные результаты

                if (resUnion.InitialResults.Count != 0)
                {
                    needAddResultNode = true;

                    TreeNode Ishodnie = new TreeNode("Исходные данные");
                    SpisokResultatovNode.Add(Ishodnie);

                    foreach (ResultType_MAINClass row in resUnion.InitialResults)
                    {
                        string addtext = row.ToString() + " " + row.SelectedPolarization.SelectedFrequency.ToString();

                        TreeNode[] Ish = this.GetFrequencyNodes(row, row.SelectedPolarization.FrequencyElements, MeasurementTagTypeEnum.DN_Normal);

                        foreach (TreeNode tr in Ish)
                        {
                            tr.Text = addtext + tr.Text;
                        }

                        Ishodnie.Nodes.AddRange(Ish);
                    }
                }
            }
            return needAddResultNode;
        }


        #endregion

        #region получение разноообразных нодов частот

        /// <summary>
        /// преобразовать строки в таблшице частот в ноды
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected TreeNode[] GetFrequencyNodes(ResultType_MAINClass res, List<FrequencyElementClass> freqList, MeasurementTagTypeEnum NodeType)
        {
            List<TreeNode> ret = new List<TreeNode>();
            //заполняем ноды
            foreach (FrequencyElementClass freg in freqList)
            {
                ret.Add(this.GetFrequencyNodes(res, freg, NodeType));
            }
            return ret.ToArray();
        }


        /// <summary>
        /// преобразовать строку в таблшице частот в нод
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected TreeNode GetFrequencyNodes(ResultType_MAINClass res, FrequencyElementClass freq, MeasurementTagTypeEnum NodeType)
        {
            TreeNode fregNode = new TreeNode(freq.ToString());

            XML_LoaderPack XMLPACK = new XML_LoaderPack();
            XMLPACK.Result = res;
            XMLPACK.Freq = freq;

            DBLoaderPackClass Pack = new DBLoaderPackClass();
            Pack.FormShowType = FormShowTypeEnum.Result;
            Pack.MeasurementTagType = NodeType;
            Pack.ObjectForLoad = XMLPACK;
            Pack.SpesialText = string.Format("Это частота {0} ({1})", freq.id, NodeType);
            fregNode.Tag = Pack;

            return fregNode;
        }

        protected TreeNode GetPolarizationNode(ResultType_MAINClass res, List<FrequencyElementClass> freq, string NodeName, MeasurementTagTypeEnum NodeType)
        {
            TreeNode ret = new TreeNode(NodeName);

            ret.Nodes.AddRange(this.GetFrequencyNodes(res, freq, NodeType));

            return ret;
        }

        #endregion

        ResultOptionsClassLibrary.MainOptionsClass I_ARM_Form.LoadOnlyOptions(DBLoaderPackClass Pack)
        {
            ResultType_MAINClass res = Pack.ObjectForLoad as ResultType_MAINClass;

            return res.MainOptions;
        }

        AntennOptionsClass I_ARM_Form.LoadIA(DBLoaderPackClass Pack)
        {
            AntennOptionsClass ret = null;

            if (Pack.ObjectForLoad is IResultType_MAIN)
            {
                IResultType_MAIN result = Pack.ObjectForLoad as IResultType_MAIN;
                ret = result.Antenn;
            }
            else
            {
                throw new Exception("Объект для загрузки не является строкой Результата");
            }

            return ret;
        }

        ResultOptionsClassLibrary.AntennOptionsClass I_ARM_Form.LoadTA(DBLoaderPackClass Pack)
        {
            AntennOptionsClass ret = null;

            if (Pack.ObjectForLoad is IResultType_MAIN)
            {
                IResultType_MAIN result = Pack.ObjectForLoad as IResultType_MAIN;
                ret = result.Zond;
            }
            else
            {
                throw new Exception("Объект для загрузки не является строкой Результата");
            }

            return ret;
        }

        ResultType_MAINClass I_ARM_Form.LoadResult(DBLoaderPackClass Pack, bool LoadFullResult, bool LoadResultData = true)
        {
            ResultType_MAINClass ret = null;

            if (Pack.FormShowType == FormShowTypeEnum.Main)
            {
                ResultType_MAINClass temp = Pack.ObjectForLoad as ResultType_MAINClass;
                ret = temp.Clone() as ResultType_MAINClass;
            }
            else
            {
                XML_LoaderPack loadPack = Pack.ObjectForLoad as XML_LoaderPack;
                ret = loadPack.Result.Clone() as ResultType_MAINClass;

                if (!LoadFullResult)
                {
                    if (ret is ResultTypeClassСДНМ)
                    {
                        ResultTypeClassСДНМ tempSDNM = ret as ResultTypeClassСДНМ;

                        FrequencyElementClass tempMain = tempSDNM.Main_Polarization.FindFrequencyElement(loadPack.Freq.Frequency);
                        FrequencyElementClass tempCross = tempSDNM.Cross_Polarization.FindFrequencyElement(loadPack.Freq.Frequency);

                        tempSDNM.SUM_Polarization.FrequencyElements.Clear();
                        tempSDNM.Main_Polarization.FrequencyElements.Clear();
                        tempSDNM.Cross_Polarization.FrequencyElements.Clear();

                        tempSDNM.SUM_Polarization.SelectedFrequencyIndex = 0;
                        tempSDNM.Main_Polarization.SelectedFrequencyIndex = 0;
                        tempSDNM.Cross_Polarization.SelectedFrequencyIndex = 0;

                        tempSDNM.Main_Polarization.SelectedFrequency = tempMain;
                        tempSDNM.Cross_Polarization.SelectedFrequency = tempCross;
                        tempSDNM.ReCalculateData();

                        switch (Pack.MeasurementTagType)
                        {
                            case MeasurementTagTypeEnum.SDNM_Sum:
                                {
                                    tempSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Sum;
                                    break;
                                }
                            case MeasurementTagTypeEnum.SDNM_Main:
                                {
                                    tempSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Main;
                                    break;
                                }
                            case MeasurementTagTypeEnum.SDNM_Cross:
                                {
                                    tempSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Cross;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        ret.SelectedPolarization.FrequencyElements.Clear();
                        ret.SelectedPolarization.SelectedFrequency = loadPack.Freq;
                    }
                }
            }

            return ret;
        }

        string I_ARM_Form.GetFullNameByResult(ResultTypesClassLibrary.ResultType_MAINClass Result)
        {
            throw new NotImplementedException("Эта функция не реализована в загрузщике XML");
        }
        List<ResultType_MAINClass> I_ARM_Form.LoadALLResult(DBLoaderPackClass Pack)
        {
            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            if (Pack.ObjectForLoad is AntennOptionsClass)
            {
                AntennOptionsClass ant = Pack.ObjectForLoad as AntennOptionsClass;
                foreach (ResultType_MAINClass res in AllResults)
                {
                    if (res.Antenn.id == ant.id)
                    {
                        ret.Add(res);
                    }
                }
            }

            return ret;
        }

        #region ISaver_ToDataBase

        List<ResultOptionsClassLibrary.AntennOptionsClass> ResultOptionsClassLibrary.ISaver_ToDataBase.LoadAllAntenn()
        {
            return this.AllAntenn;
        }

        void ResultOptionsClassLibrary.ISaver_ToDataBase.Save_Antenn(object Sender, ResultOptionsClassLibrary.AntennOptionsClass SaveObj)
        {
            throw new NotImplementedException();
        }

        event ResultOptionsClassLibrary.AddNewAntennDelegate ResultOptionsClassLibrary.ISaver_ToDataBase.AddNewAntennEvent
        {
            add { /*throw new NotImplementedException();*/ }
            remove { /*throw new NotImplementedException();*/ }
        }

        ResultOptionsClassLibrary.Cables_Sheild_M_Class ResultOptionsClassLibrary.ISaver_ToDataBase.LoadCablesParametrs()
        {
            throw new NotImplementedException();
        }

        ResultOptionsClassLibrary.AdjustOptionsClass ResultOptionsClassLibrary.ISaver_ToDataBase.LoadAdjustOptions()
        {
            throw new NotImplementedException();
        }

        ResultOptionsClassLibrary.ZVB14_ParametrsClass ResultOptionsClassLibrary.ISaver_ToDataBase.LoadZVB_Parametrs()
        {
            throw new NotImplementedException();
        }

        void ResultOptionsClassLibrary.ISaver_ToDataBase.CreateMeasurementTable(string Name)
        {
            throw new NotImplementedException();
        }

        void ResultOptionsClassLibrary.ISaver_ToDataBase.AddToMeasurementTable(string Name, int id_freq, float coord, float Data, float Data2)
        {
            throw new NotImplementedException();
        }

        int ResultOptionsClassLibrary.ISaver_ToDataBase.SaveMainTable(ResultOptionsClassLibrary.IResultType_MAIN SaveObj)
        {
            throw new NotImplementedException();
        }

        int ResultOptionsClassLibrary.ISaver_ToDataBase.SaveFrequencyElement(ResultOptionsClassLibrary.FrequencyElementClass SaveObj, int idMain, int PolarizationType, ref bool CreateResultTable)
        {
            throw new NotImplementedException();
        }

        void ResultOptionsClassLibrary.ISaver_ToDataBase.SaveFrequencyElement(IList<FrequencyElementClass> Frequency, int idMain, int PolarizationType, ref bool CreateResultTable)
        {
            throw new NotImplementedException();
        }

        int ResultOptionsClassLibrary.ISaver_ToDataBase.SavePositioningParameters(ResultOptionsClassLibrary.MeasurementParametrsClass SaveObj)
        {
            throw new NotImplementedException();
        }

        int ResultOptionsClassLibrary.ISaver_ToDataBase.SaveZVBParameters(ResultOptionsClassLibrary.MeasurementParametrsClass SaveObj)
        {
            throw new NotImplementedException();
        }

        int ResultOptionsClassLibrary.ISaver_ToDataBase.SaveSegmentTable(GeneralInterfaces.ISegmentTableElementClass SaveObj, int idMain)
        {
            throw new NotImplementedException();
        }

        void ResultOptionsClassLibrary.ISaver_ToDataBase.SaveSegmentTable(List<GeneralInterfaces.ISegmentTableElementClass> SaveObj, int idMain)
        {
            //throw new NotImplementedException();
        }

        void ISaver_ToDataBase.DeleteFullResult(int idMain)
        {
            //throw new NotImplementedException();
        }

        void ISaver_ToDataBase.DeleteAntenn(int idAnten)
        {
            // throw new NotImplementedException();
        }

        void ISaver_ToDataBase.SaveResultElementS(List<ResultElementClass> ResultAmpl_PhaseElements, string TableName, int id_freq)
        {
            for (int i = 0; i < AllResults.Count;i++ )
            {
                for (int j = 0; j < AllResults[i].PolarizationElements.Count;j++ )
                {
                    for (int f = 0; f < AllResults[i].PolarizationElements[j].FrequencyElements.Count;f++ )
                    {
                        if (AllResults[i].PolarizationElements[j].FrequencyElements[f].id == id_freq)
                        {
                            AllResults[i].PolarizationElements[j].FrequencyElements[f].ResultAmpl_PhaseElements = ResultAmpl_PhaseElements;
                            return;
                        }
                    }
                }
            }

            //throw new NotImplementedException();
        }

        void ISaver_ToDataBase.ClearMeasurementTable(string Name, int freqID)
        {
            // throw new NotImplementedException();
        }

        void I_ARM_Form.HideFrequency(DBLoaderPackClass Pack)
        {
            //throw new NotImplementedException();
        }

        public List<ResultType_MAINClass> LoadALLResult()
        {
            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            return ret;
        }

        public int GetMainID(DBLoaderPackClass Pack) { return -1; }

        #endregion


    }

    public class XML_LoaderPack
    {
        public ResultType_MAINClass Result = null;
        public FrequencyElementClass Freq = null;

        public SelectedPolarizationEnum SelectedPolarization;
    }
}
