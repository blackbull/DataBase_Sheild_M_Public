using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using ResultOptionsClassLibrary;
using System.Data.SqlClient;
using System.Data;
using ResultTypesClassLibrary;
using GeneralInterfaces;
using ZVBOptionsClassLibrary;
using DB_Controls;
using DB_Interface;
using DataBase_Sheild_M;

namespace DB_Loader
{
    public class DB_LoaderClass : I_ARM_Form, IDisposable
    {
        #region конструкторы и деструкторы

        /// <summary>
        /// конструктор с указанием строк инициализации БД
        /// </summary>
        public DB_LoaderClass(string DataBase_Connection_String, string measurement_Connection_String)
            : this(new SqlConnection(DataBase_Connection_String), new SqlConnection(measurement_Connection_String))
        {

        }

        /// <summary>
        /// конструктор с указанием подключений к БД
        /// </summary>
        public DB_LoaderClass(SqlConnection NewSheild_M_DataBase_Connection, SqlConnection NewSheild_M_measurement_Connection)
        {
            Sheild_M_DataBase_Connection = NewSheild_M_DataBase_Connection;
            Sheild_M_measurement_Connection = NewSheild_M_measurement_Connection;
        }

        public void Dispose()
        {
            if (this._IsInitializ)
            {
                if (this.Sheild_M_DataBase_Connection.State != ConnectionState.Closed)
                {
                    this.Sheild_M_DataBase_Connection.Close();
                }

                if (this.Sheild_M_measurement_Connection.State != ConnectionState.Closed)
                {
                    this.Sheild_M_measurement_Connection.Close();
                }

                this._IsInitializ = false;
            }
        }

        ~DB_LoaderClass()
        {
            Dispose();
        }

        #endregion

        #region Объекты связи с БД

        protected DataSet1 Sheild_M_DataBase;

        /// <summary>
        /// Соединение с базой данных Sheild_M_measurement
        /// </summary>
        protected SqlConnection Sheild_M_measurement_Connection;

        /// <summary>
        /// Соединение с базой данных Sheild_M_DataBase
        /// </summary>
        protected SqlConnection Sheild_M_DataBase_Connection;


        #region Адаптеры подключения к БД
        protected SqlDataAdapter DataAdapterMAIN;
        protected SqlDataAdapter DataAdapterAntennas;
        protected SqlDataAdapter DataAdapterMeasurement_PositionParameters;
        protected SqlDataAdapter DataAdapterMeasurement_ZVBParameters;
        protected SqlDataAdapter DataAdapterFrequencies;
        protected SqlDataAdapter DataAdapterSegment_Table;
        protected SqlDataAdapter DataAdapterInitial_Results;
        protected SqlDataAdapter DataAdapterCable_Parametrs;
        protected SqlDataAdapter DataAdapterZVB14_Parametrs;
        protected SqlDataAdapter DataAdapterTech_Antenn_Parameters;
        protected SqlDataAdapter DataAdapterAdjustment_Parametrs;
        protected SqlDataAdapter DataAdapterAnechoic_Chamber;
        protected SqlDataAdapter DataAdapterCalculateDataTable;
        #endregion

        #endregion

        #region Основыные переменные

        public TreeView TreeSortByAnten = null;
        public TreeView TreeSortByMeasurement = null;

        protected bool _IsInitializ = false;

        //для формирования нормальных строк запросов SQL
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

        #endregion

        public void InitializeDB()
        {
            if (!_IsInitializ)
            {
                _IsInitializ = true;

                Sheild_M_DataBase_Connection.Open();
                Sheild_M_measurement_Connection.Open();

                this.Sheild_M_DataBase = new DataBase_Sheild_M.DataSet1();
                this.Sheild_M_DataBase.DataSetName = "Sheild_M_DataBase";


                #region Инициализация Адаптеров подключения к БД
                DataAdapterMAIN = FillBilderTable(this.Sheild_M_DataBase.main);
                DataAdapterAntennas = FillBilderTable(this.Sheild_M_DataBase.Antennas);
                DataAdapterMeasurement_PositionParameters = FillBilderTable(this.Sheild_M_DataBase.Measurement_PositionParameters);
                DataAdapterMeasurement_ZVBParameters = FillBilderTable(this.Sheild_M_DataBase.Measurement_ZVBParameters);
                DataAdapterFrequencies = FillBilderTable(this.Sheild_M_DataBase.Frequencies);
                DataAdapterSegment_Table = FillBilderTable(this.Sheild_M_DataBase.Segment_Table);
                DataAdapterInitial_Results = FillBilderTable(this.Sheild_M_DataBase.Initial_Results);
                DataAdapterCable_Parametrs = FillBilderTable(this.Sheild_M_DataBase.Cable_Parametrs);
                DataAdapterZVB14_Parametrs = FillBilderTable(this.Sheild_M_DataBase.ZVB14_Parametrs);
                DataAdapterTech_Antenn_Parameters = FillBilderTable(this.Sheild_M_DataBase.Tech_Antenn_Parameters);
                DataAdapterAdjustment_Parametrs = FillBilderTable(this.Sheild_M_DataBase.Adjustment_Parametrs);
                DataAdapterAnechoic_Chamber = FillBilderTable(this.Sheild_M_DataBase.Anechoic_Chamber);
                DataAdapterCalculateDataTable = FillBilderTable(this.Sheild_M_DataBase.CalculateDataTable);
                #endregion


                #region загрузка глобальных параметров из БД

                SheildM_GlobalParametrsClass.ZVB14_Parametrs = this.LoadZVB_Parametrs();
                SheildM_GlobalParametrsClass.Cables_Sheild_M = this.LoadCablesParametrs();
                SheildM_GlobalParametrsClass.AdjustOptions = this.LoadAdjustOptions();
                SheildM_GlobalParametrsClass.AnechoicChamber = this.LoadAnechoicChamberParametrs();
                #endregion
            }

        }

        /// <summary>
        /// Автоматическое создание команд чтения, записи и загрузка таблицы
        /// </summary>
        /// <param name="DT"></param>
        /// <returns></returns>
        protected SqlDataAdapter FillBilderTable(DataTable DT)
        {
            SqlDataAdapter DA = new SqlDataAdapter(string.Format(culture, "SELECT * FROM {0}", DT.TableName), this.Sheild_M_DataBase_Connection);

            new SqlCommandBuilder(DA);
            DA.Fill(DT);

            return DA;
        }

        public void LoadNodesToTree(ref TreeView NewTreeSortByAnten, ref TreeView NewTreeSortByMeasurement, DateTime StartDate, DateTime StopDate, bool LoadAllAntens = false)
        {
            TreeSortByAnten = NewTreeSortByAnten;
            TreeSortByMeasurement = NewTreeSortByMeasurement;

            if (LoadAllAntens)
            {
                FillAllAntenTreeView();
            }

            FillMeasurementTreeView();

            foreach (DataSet1.mainRow mainRow in this.Sheild_M_DataBase.main)
            {
                if (mainRow.Date.Date > StartDate.Date && mainRow.Date.Date <= StopDate.Date)
                {
                    try
                    {
                        this.AddMainRowToTreeView(mainRow, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Измрений №{0} \n {1}", mainRow.id,ex.Message), "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //флаг использования нода для неизвестных типов измерений
                NonameNodeIsUsing = false;
            }

            //расскрываем антенны
            //foreach (TreeNode node in TreeSortByAnten.Nodes)
            //{
            //    node.Expand();
            //}

            if (TreeSortByAnten.Nodes.Count == 0)
            {
                TreeSortByAnten.Nodes.Add("За выбранный диапазон измерений нет");
            }
        }

        #region заполнение базовой структуры TreeView

        protected void FillAllAntenTreeView()
        {
            foreach (DataSet1.AntennasRow Row in this.Sheild_M_DataBase.Antennas)
            {
                FindOrCreateAntenninTreeViewAnten(Row);
            }
        }

        protected void FillMeasurementTreeView()
        {
            if (TreeSortByMeasurement != null)
            {
                Array arr = (Enum.GetValues(typeof(MeasurementTypeEnum)));

                foreach (object obj in arr)
                {
                    if (obj is MeasurementTypeEnum)
                    {
                        MeasurementTypeEnum TempType = (MeasurementTypeEnum)obj;

                        TreeSortByMeasurement.Nodes.Add(((int)TempType).ToString(), MeasurementTypeDescription.MeasurementType_Name[TempType]);

                        //выделение нода
                        TreeSortByMeasurement.Nodes[((int)TempType).ToString()].BackColor = Color.LightGray;
                        TreeSortByMeasurement.Nodes[((int)TempType).ToString()].ToolTipText = MeasurementTypeDescription.MeasurementType_Description[TempType];
                    }
                }
            }
        }

        /// <summary>
        /// Найти или создать ноды антенны
        /// </summary>
        /// <param name="Row"></param>
        protected TreeNode FindOrCreateAntenninTreeViewAnten(DataSet1.AntennasRow Row)
        {
            TreeNode ret = null;
            if (TreeSortByAnten != null)
            {
                if (Row == null)
                {
                    TreeNode[] AntennNodes = TreeSortByAnten.Nodes.Find("Без_Антенны", true);
                    if (AntennNodes.Length != 0)
                    {
                        ret = AntennNodes[0];
                    }
                    else
                    {
                        TreeNode Без_Антенны = new TreeNode("Без_Антенны");
                        TreeSortByAnten.Nodes.Add(Без_Антенны);
                        ret = Без_Антенны;
                    }
                }
                else
                {
                    TreeNode[] AntennNodes = TreeSortByAnten.Nodes.Find("zav" + Row.id.ToString(), true);
                    if (AntennNodes.Length != 0)
                    {
                        ret = AntennNodes[0];
                    }

                    if (ret == null)
                    {
                        DBLoaderPackClass Pack = new DBLoaderPackClass();
                        Pack.FormShowType = FormShowTypeEnum.Antenn;
                        Pack.ObjectForLoad = Row;
                        Pack.SpesialText = "Это антенна " + Row.id.ToString();

                        string AntenName = Row.Name.Trim();

                        #region сортировка для более качественного отображения дерева антенн

                        #region Обработка ключа для сортировки

                        string AntenKEYName = GetSplitKey(AntenName);
                        #endregion

                        TreeNode AntenNode = TreeSortByAnten.Nodes[AntenKEYName];

                        if (AntenNode == null)
                        {
                            AntenNode = new TreeNode(AntenName);
                            AntenNode.Name = AntenKEYName;

                            #region спец пак для групп антенн

                            DBLoaderPackClass PackList = new DBLoaderPackClass();
                            PackList.FormShowType = FormShowTypeEnum.Antenn;
                            PackList.SpesialText = "Это пак антенн ";

                            List<DataSet1.AntennasRow> rowsList = new List<DataSet1.AntennasRow>();
                            rowsList.Add(Row);

                            PackList.ObjectForLoad = rowsList;
                            #endregion

                            AntenNode.Tag = PackList;
                            AntenNode.BackColor = Color.LightGray;

                            TreeSortByAnten.Nodes.Add(AntenNode);
                        }
                        else
                        {
                            #region пополняем пак для загрузки

                            DBLoaderPackClass antPack = AntenNode.Tag as DBLoaderPackClass;
                            List<DataSet1.AntennasRow> antRow = antPack.ObjectForLoad as List<DataSet1.AntennasRow>;
                            antRow.Add(Row);

                            #endregion
                        }

                        #endregion

                        string ZaVNumber = "зав. №" + Row.Serial_Number.TrimEnd(" ".ToCharArray());

                        TreeNode ZavAnt = new TreeNode(ZaVNumber);
                        //костыль
                        ZavAnt.Name = "zav" + Row.id.ToString();
                        ZavAnt.Tag = Pack;
                        ZavAnt.BackColor = Color.WhiteSmoke;
                        AntenNode.Nodes.Add(ZavAnt);

                        ret = ZavAnt;
                    }
                }
            }
            return ret;
        }

        #endregion

        //флаг использования нода для неизвестных типов измерений
        bool NonameNodeIsUsing = false;
        //флаг использования нода для неизвестных типов измерений при сортировке по антенам
        bool NonameAntenIsUsing = false;


        #region Функции загрузки статических данных

        protected List<AntennOptionsClass> loadAnten = null;

        /// <summary>
        /// загрузить все антенны
        /// </summary>
        public List<AntennOptionsClass> LoadAllAntenn()
        {
            if (loadAnten == null)
            {
                loadAnten = new List<AntennOptionsClass>();

                foreach (DataSet1.AntennasRow tempAnten in Sheild_M_DataBase.Antennas)
                {
                    loadAnten.Add(AntennRowToClass(tempAnten));
                }
            }

            return loadAnten;
        }

        protected Cables_Sheild_M_Class Cables_Sheild_M = null;

        /// <summary>
        /// загрузить все кабели
        /// </summary>
        public Cables_Sheild_M_Class LoadCablesParametrs()
        {
            if (Cables_Sheild_M == null)
            {
                Cables_Sheild_M = new Cables_Sheild_M_Class();

                foreach (DataSet1.Cable_ParametrsRow temp in Sheild_M_DataBase.Cable_Parametrs)
                {
                    //определяем тип кабеля до технической антены или испытуемой
                    if (temp._CableType_True_TA_False_UA)
                    {
                        Cables_Sheild_M.TA_Cable.CableOptions.Add(new CableOptionsClass(temp.Freq_mHz, temp.Loss_dB, temp.Reflection));
                    }
                    else
                    {
                        Cables_Sheild_M.IA_Cable.CableOptions.Add(new CableOptionsClass(temp.Freq_mHz, temp.Loss_dB, temp.Reflection));
                    }
                }
            }

            return Cables_Sheild_M;
        }

        protected AnechoicChamberClass AnechoicChamber = null;

        /// <summary>
        /// загрузить все характеристики безэховой камеры
        /// </summary>
        public AnechoicChamberClass LoadAnechoicChamberParametrs()
        {
            if (AnechoicChamber == null)
            {
                AnechoicChamber = new AnechoicChamberClass();

                foreach (DataSet1.Anechoic_ChamberRow temp in Sheild_M_DataBase.Anechoic_Chamber)
                {
                    AnechoicChamber.AnechoicOptions.Add(new AnechoicClass(temp.Freq_MHz, temp.Ro, temp.delta_Ro));
                }
            }

            return AnechoicChamber;
        }

        protected AdjustOptionsClass AdjustOptions = null;

        /// <summary>
        /// загрузить все юстировочные характеристики
        /// </summary>
        public AdjustOptionsClass LoadAdjustOptions()
        {
            if (AdjustOptions == null)
            {
                AdjustOptions = new AdjustOptionsClass();

                if (Sheild_M_DataBase.Adjustment_Parametrs.Count >= 1)
                {
                    AdjustOptions.Date = Sheild_M_DataBase.Adjustment_Parametrs[0].Date;
                    AdjustOptions.R = Sheild_M_DataBase.Adjustment_Parametrs[0].R_m;
                    AdjustOptions.abs_delta_R = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_R_m;

                    AdjustOptions.delta_dW = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_dW;
                    AdjustOptions.delta_dW1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_dW_;
                    AdjustOptions.delta_dY = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_dY;
                    AdjustOptions.delta_dY1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_dY_;

                    AdjustOptions.delta_pW = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_pW;
                    AdjustOptions.delta_pW1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_pW_;
                    AdjustOptions.delta_pY = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_pY;
                    AdjustOptions.delta_pY1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_pY_;

                    AdjustOptions.delta_W = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_W;
                    AdjustOptions.delta_W_ort = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_W_ort;
                    AdjustOptions.delta_W1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_W_;
                    AdjustOptions.delta_W1_ort = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_W__ort;

                    AdjustOptions.delta_WW1_a = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_WW__a;
                    AdjustOptions.delta_WW1_l = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_WW__l;
                    AdjustOptions.delta_XY = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_XY;
                    AdjustOptions.delta_Y = Sheild_M_DataBase.Adjustment_Parametrs[0].delta_Y;

                    AdjustOptions.delta_Y1 = Sheild_M_DataBase.Adjustment_Parametrs[0]._delta_Y_;
                }
            }

            return AdjustOptions;
        }

        protected ZVB14_ParametrsClass _ZVB_Parametrs = null;

        /// <summary>
        /// загрузить параметры ZVB14
        /// </summary>
        public ZVB14_ParametrsClass LoadZVB_Parametrs()
        {
            if (_ZVB_Parametrs == null)
            {
                _ZVB_Parametrs = new ZVB14_ParametrsClass();

                foreach (DataSet1.ZVB14_ParametrsRow temp in Sheild_M_DataBase.ZVB14_Parametrs)
                {
                    ZVB_Parametrs_ElementClass zvb = new ZVB_Parametrs_ElementClass();
                    zvb.FreqDown = temp.Freq_Down_MHz;
                    zvb.FreqUp = temp.Freq_Up_MHz;
                    zvb.AmplDown = temp.Ampl_Down_dB;
                    zvb.AmplUp = temp.Ampl_Up_dB;

                    zvb.delta_Freq = temp.delta_Freq;
                    zvb.delta_Ampl = temp.delta_Ampl_dB;
                    zvb.delta_Phase = temp.delta_Phase_Degree;

                    _ZVB_Parametrs.ZVB_Parametrs_ElementsList.Add(zvb);
                }
            }

            return _ZVB_Parametrs;
        }

        #endregion


        #region создание нодов с результатами измерений

        protected bool CreateResultNode(DataSet1.mainRow mainRow, out List<TreeNode> SpisokResultatovNode)
        {
            SpisokResultatovNode = new List<TreeNode>();

            bool needAddResultNode = true;


            //выбираем как считывать результаты в соответствии с типом измерения.
            if (mainRow.id_Type == (int)MeasurementTypeEnum.Поляризационная_диаграмма || mainRow.id_Type == (int)MeasurementTypeEnum.ДН_Меридиан || mainRow.id_Type == (int)MeasurementTypeEnum.ДН_Азимут)
            {
                TreeNode temp = this.GetPolarizationNode(mainRow, "Результат " + ((MeasurementTypeEnum)mainRow.id_Type).ToString(), MeasurementTagTypeEnum.DN_Normal);

                for (int i = 0; i < temp.Nodes.Count; i++)
                {
                    SpisokResultatovNode.Add(temp.Nodes[i]);
                }
            }
            else
            {
                if (mainRow.id_Type == (int)MeasurementTypeEnum.Коэффицент_усиления)
                {
                    TreeNode temp = this.GetPolarizationNode(mainRow, "Результат " + ((MeasurementTypeEnum)mainRow.id_Type).ToString(), MeasurementTagTypeEnum.KY);

                    if (temp.Nodes.Count != 0)
                    {
                        temp.Nodes[0].Text = "Результат КУ";
                        SpisokResultatovNode.Add(temp.Nodes[0]);
                    }
                }
                else
                {
                    if (mainRow.id_Type == (int)MeasurementTypeEnum.Суммарная_ДН_Меридиан || mainRow.id_Type == (int)MeasurementTypeEnum.Суммарная_ДН_Азимут)
                    {
                        SpisokResultatovNode.Add(this.GetPolarizationNode(mainRow, "Polarization Sum", MeasurementTagTypeEnum.SDNM_Sum));
                        SpisokResultatovNode.Add(this.GetPolarizationNode(mainRow, "Polarization Main", MeasurementTagTypeEnum.SDNM_Main));
                        SpisokResultatovNode.Add(this.GetPolarizationNode(mainRow, "Polarization Cross", MeasurementTagTypeEnum.SDNM_Cross));
                    }
                    else
                    {
                        //неизвестный тип измерения или ошибка в БД

                        SpisokResultatovNode.Add(this.GetPolarizationNode(mainRow, "Polarization ", MeasurementTagTypeEnum.DN_Normal));
                    }
                }
            }

            return needAddResultNode;
        }

        protected bool CreateCalculationResultNode(DataSet1.mainRow mainRow, out List<TreeNode> SpisokResultatovNode)
        {
            bool needAddResultNode = false;

            SpisokResultatovNode = new List<TreeNode>();


            needAddResultNode = true;

            string addText = ((CalculationResultTypeEnum)mainRow.id_Type_CalculationResult).ToString();
            SpisokResultatovNode.Add(this.GetPolarizationNode(mainRow, addText, MeasurementTagTypeEnum.Union));

            //добавляем исходные результаты

            DataSet1.Initial_ResultsRow[] InitialRows = mainRow.GetInitial_ResultsRows();

            if (InitialRows.Length != 0)
            {
                needAddResultNode = true;

                TreeNode Ishodnie = new TreeNode("Исходные данные");
                SpisokResultatovNode.Add(Ishodnie);

                foreach (DataSet1.Initial_ResultsRow row in InitialRows)
                {
                    string addtext = "";
                    DataSet1.mainRow[] mains = row.GetmainRows();
                    if (mains.Length > 0)
                    {
                        addtext = mains[0].Name.TrimEnd(" ".ToCharArray()) + " " + mains[0].id.ToString() + ": ";
                    }

                    DataSet1.FrequenciesRow[] freqRows = row.GetFrequenciesRows();

                    TreeNode[] Ish = this.GetFrequencyNodes(freqRows, MeasurementTagTypeEnum.DN_Normal);

                    foreach (TreeNode tr in Ish)
                    {
                        tr.Text = addtext + tr.Text;
                    }

                    Ishodnie.Nodes.AddRange(Ish);
                }
            }

            return needAddResultNode;
        }

        #endregion


        #region Создание нодов Main

        protected void AddMainRowToTreeView(DataSet1.mainRow mainRow, bool NeedSelectNode = false)
        {
            TreeNode tempMain = this.CreateMainTreeNode(mainRow);
            TreeNode tempMain2 = tempMain.Clone() as TreeNode;

            this.AddMainRowToTreeViewMeasurementType(mainRow, tempMain);
            this.AddMainRowToTreeViewAntennType(mainRow, tempMain2, NeedSelectNode);
        }

        protected void AddMainRowToTreeViewMeasurementType(DataSet1.mainRow mainRow, TreeNode tempMain)
        {
            if (TreeSortByMeasurement != null)
            {
                //проверка: является ли измерение рассчётным
                bool IsCalculationResult = false;
                if (!mainRow.Isid_Type_CalculationResultNull())
                {
                    if (mainRow.id_Type_CalculationResult != -1 && mainRow.id_Type_CalculationResult != 0)
                    {
                        IsCalculationResult = true;
                    }
                }



                TreeNode AddtoThisResultTypeNode = TreeSortByMeasurement.Nodes[mainRow.id_Type.ToString()];

                if (AddtoThisResultTypeNode != null)
                {
                    if (IsCalculationResult)
                    {
                        //если это рассчитанный результат, то добавляем дополнительный нод в нот типа результата
                        TreeNode temp = AddtoThisResultTypeNode.Nodes["Рассчитанные результаты"];
                        if (temp == null)
                        {
                            temp = new TreeNode("Рассчитанные результаты");
                            temp.Name = "Рассчитанные результаты";

                            AddtoThisResultTypeNode.Nodes.Insert(0, temp);
                        }

                        AddtoThisResultTypeNode = temp;
                    }

                    AddtoThisResultTypeNode.Nodes.Add(tempMain);
                }
                else
                {
                    if (!NonameNodeIsUsing)
                    {
                        TreeSortByMeasurement.Nodes.Add("NoType", "Неизвестный тип измерения");
                        NonameNodeIsUsing = true;
                    }
                    TreeSortByMeasurement.Nodes["NoType"].Nodes.Add(tempMain);
                }
            }
        }

        protected void AddMainRowToTreeViewAntennType(DataSet1.mainRow mainRow, TreeNode tempMain, bool NeedSelectNode = false)
        {
            if (TreeSortByAnten != null)
            {
                TreeNode AntennNode = FindOrCreateAntenninTreeViewAnten(mainRow.AntennasRow);

                if (AntennNode != null)
                {
                    TreeNode AddtoThisResultTypeNode;

                    AddtoThisResultTypeNode = AntennNode.Nodes[mainRow.id_Type.ToString()];

                    if (AddtoThisResultTypeNode == null)
                    {
                        AntennNode.Nodes.Add(mainRow.id_Type.ToString(), MeasurementTypeDescription.MeasurementType_Name[(MeasurementTypeEnum)mainRow.id_Type]);

                        AddtoThisResultTypeNode = AntennNode.Nodes[mainRow.id_Type.ToString()];
                    }


                    AddtoThisResultTypeNode.Nodes.Add(tempMain);
                }
                else
                {
                    this.FindOrCreateAntenninTreeViewAnten(mainRow.AntennasRow);

                    if (!NonameAntenIsUsing)
                    {
                        TreeSortByAnten.Nodes.Add("NoAntenn", "Без антенны");
                        NonameAntenIsUsing = true;
                    }
                    TreeSortByAnten.Nodes["NoAntenn"].Nodes.Add(tempMain);
                }


                //выбираем измерение (тк подпись на событие выбора нода в форме происходит позже загрузки, то обрабатываться будут только вновь добавленные измерения)
                if (NeedSelectNode)
                    TreeSortByAnten.SelectedNode = tempMain;
            }
        }

        protected TreeNode CreateMainTreeNode(DataSet1.mainRow mainRow)
        {
            string tempName = "";
            if (!mainRow.IsNameNull())
            {
                tempName = " | " + mainRow.Name.TrimEnd(" ".ToCharArray()) + " " + mainRow.id.ToString();
            }

            TreeNode tempMain = new TreeNode(mainRow.Date.ToShortDateString() + tempName);
            tempMain.Name = "main " + mainRow.id.ToString();

            DBLoaderPackClass Pack = new DBLoaderPackClass();
            Pack.FormShowType = FormShowTypeEnum.Main;
            Pack.ObjectForLoad = mainRow;
            Pack.SpesialText = string.Format("Это Main {0}", mainRow.id);

            tempMain.Tag = Pack;


            #region получаем список результатов

            List<TreeNode> SpisokResultatovNode;

            bool IsCalculationResult = false;
            //проверка: является ли измерение рассчётным
            if (!mainRow.Isid_Type_CalculationResultNull())
            {
                if (mainRow.id_Type_CalculationResult != -1 && mainRow.id_Type_CalculationResult != 0)
                {
                    IsCalculationResult = true;

                    if (this.CreateCalculationResultNode(mainRow, out SpisokResultatovNode))
                    {
                        foreach (TreeNode tr in SpisokResultatovNode)
                        {
                            tempMain.Nodes.Add(tr);
                        }
                    }
                }
            }

            if (!IsCalculationResult)
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

        #region Удаление из БД

        public void DeleteFullResult(int idMain)
        {
            DeleteRow(Sheild_M_DataBase.main.FindByid(idMain));
        }

        public void DeleteAntenn(int idAnten)
        {
            DataSet1.AntennasRow row = Sheild_M_DataBase.Antennas.FindByid(idAnten);
            row.Delete();
            DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);
        }

        public void DeleteRow(DataSet1.mainRow row)
        {
            if (row != null)
            {
                //удаляем записи частот
                DeleteRow(row.GetFrequenciesRows());
                //удаляем записи исходных результатов
                DeleteRow(row.GetInitial_ResultsRows());
                //опции измерения
                DeleteRow(row.GetMeasurement_PositionParametersRows());
                //опции измерения ZVB
                DeleteRow(row.GetMeasurement_ZVBParametersRows());
                //сегментная таблица
                DeleteRow(row.GetSegment_TableRows());

                row.Delete();
                DataAdapterMAIN.Update(Sheild_M_DataBase.main);
            }
        }

        public void DeleteRow(DataSet1.FrequenciesRow[] row)
        {
            foreach (DataSet1.FrequenciesRow del in row)
            {
                DeleteRow(del.GetCalculateDataTableRows());
                DeleteMeasurementTable(del.Result_Table_Name);

                del.Delete();
            }
            DataAdapterFrequencies.Update(Sheild_M_DataBase.Frequencies);
        }

        public void DeleteRow(DataSet1.Initial_ResultsRow[] row)
        {
            foreach (DataSet1.Initial_ResultsRow del in row)
            {
                del.Delete();
            }
            DataAdapterInitial_Results.Update(Sheild_M_DataBase.Initial_Results);
        }

        public void DeleteRow(DataSet1.Measurement_PositionParametersRow[] row)
        {
            foreach (DataSet1.Measurement_PositionParametersRow del in row)
            {
                del.Delete();
            }
            DataAdapterMeasurement_PositionParameters.Update(Sheild_M_DataBase.Measurement_PositionParameters);
        }

        public void DeleteRow(DataSet1.Measurement_ZVBParametersRow[] row)
        {
            foreach (DataSet1.Measurement_ZVBParametersRow del in row)
            {
                del.Delete();
            }
            DataAdapterMeasurement_ZVBParameters.Update(Sheild_M_DataBase.Measurement_ZVBParameters);
        }

        public void DeleteRow(DataSet1.Segment_TableRow[] row)
        {
            foreach (DataSet1.Segment_TableRow del in row)
            {
                del.Delete();
            }
            DataAdapterSegment_Table.Update(Sheild_M_DataBase.Segment_Table);
        }

        public void DeleteRow(DataSet1.CalculateDataTableRow[] row)
        {
            foreach (DataSet1.CalculateDataTableRow del in row)
            {
                del.Delete();
            }
            DataAdapterCalculateDataTable.Update(Sheild_M_DataBase.CalculateDataTable);
        }

        /// <summary>
        /// Удалить таблицу значений измерений
        /// </summary>
        /// <param name="Name">Measurement_"id Result"
        /// Пример: Measurement_1 </param>
        public void DeleteMeasurementTable(string Name)
        {
            if (Name != null && Name != "")
            {
                try
                {
                    //Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("DROP TABLE {0};", Name));
                    SqlCommand command = Sheild_M_measurement_Connection.CreateCommand();
                    command.CommandText = string.Format("DROP TABLE {0};", Name);
                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Ошибка удаления таблицы " + ex.Message);
                    //MessageBox.Show(ex.Message, "Ошибка удаления таблицы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new Exception("Не указано имя таблицы");
            }
        }

        /// <summary>
        /// Очистить таблицу значений измерений
        /// </summary>
        /// <param name="Name">Measurement_"id Result"
        /// Пример: Measurement_1 </param>
        public void ClearMeasurementTable(string Name, int freqID)
        {
            if (Name != null && Name != "")
            {
                //обнуляем нумерацию, уже не надо
                //Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("DBCC CHECKIDENT ('{0}', RESEED, 0) WITH NO_INFOMSGS", Name));

                string CommandFist = string.Format("Delete {0} where id_Measurement_Result={1};", Name, freqID);
                string CommandSecond = string.Format("Delete {0};", Name);

                Check_Old_or_New_Table(Name, freqID, ref CommandFist, ref CommandSecond);
                SqlCommand command = Sheild_M_measurement_Connection.CreateCommand();

                try
                {
                    command.CommandText = CommandFist;
                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    try
                    {
                        command.CommandText = CommandSecond;
                        command.ExecuteScalar();
                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show(ex1.Message, "Ошибка очистки таблицы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                throw new Exception("Не указано имя таблицы");
            }
        }

        #endregion

        #region Функции сохранения данных в БД

        public void Save_Antenn(object Sender, AntennOptionsClass SaveObj)
        {
            if (SaveObj.id < 0)
            {
                //создаём строку антены
                DataSet1.AntennasRow AddingRow = Sheild_M_DataBase.Antennas.NewAntennasRow();
                AddingRow.Name = SaveObj.Name;
                AddingRow.Serial_Number = SaveObj.ZAVNumber;
                AddingRow.Using_as_TA = SaveObj.UsingAsZond;
                AddingRow.Description = SaveObj.Description;

                SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
                command.CommandText = string.Format(culture, "INSERT INTO {0}  ([Name],[Serial_Number],[Using_as_TA],[Description])VALUES ('{1}','{2}','{3}','{4}'); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.Name, AddingRow.Serial_Number, AddingRow.Using_as_TA, AddingRow.Description);
                
                //получаем Id из БД
                AddingRow.id = Convert.ToInt32(command.ExecuteScalar());
                //добавить запись в бд
                Sheild_M_DataBase.Antennas.AddAntennasRow(AddingRow);
                //добавляем выданный id
                SaveObj.id = AddingRow.id;

                //добавляем антенну в массив уже загруженных
                this.loadAnten.Add(SaveObj);



                //добавляем в treeview
                FindOrCreateAntenninTreeViewAnten(Sheild_M_DataBase.Antennas[Sheild_M_DataBase.Antennas.Count - 1]);

                //разослать всем добаленные объект
                this.SendNewAntenn(this, SaveObj, Sender);
            }
            else
            {
                //обновить данные в бд
                DataSet1.AntennasRow temprow = Sheild_M_DataBase.Antennas.FindByid(SaveObj.id);

                temprow.Name = SaveObj.Name;
                temprow.Serial_Number = SaveObj.ZAVNumber;
                temprow.Using_as_TA = SaveObj.UsingAsZond;
                temprow.Description = SaveObj.Description;

                DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);
            }
        }
        
        /// <summary>
        /// создать таблицу значений измерений
        /// </summary>
        /// <param name="Name">Measurement_"id Result"
        /// Пример: Measurement_1 </param>
        public void CreateMeasurementTable(string Name)
        {
            if (Name != null && Name != "")
            {
                try
                {
                    //Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("CREATE TABLE {0} (id int primary key IDENTITY(1, 1),Координата float, Значение float, Значение2 float);", Name));
                    //Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("CREATE TABLE {0} (id int primary key IDENTITY(1, 1),Coord float, Ampl float, Phase float);", Name));
                    //Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("CREATE TABLE {0} (id int primary key IDENTITY(1, 1), id_Measurement_Result int, Coord float, Ampl float, Phase float);", Name));

                    SqlCommand command = Sheild_M_measurement_Connection.CreateCommand();
                    command.CommandText = string.Format(culture, "CREATE TABLE {0} (id int primary key IDENTITY(1, 1), id_Measurement_Result int, Coord float, Ampl float, Phase float);", Name);
                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка создания таблицы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new Exception("Не указано имя таблицы");
            }
        }

        public void AddToMeasurementTable(string Name, int id_freq, float coord, float Data, float Data2)
        {
            #region проверка на NaN

            object AddCoord = null;
            object AddData = null;
            object AddData2 = null;
            bool ErrorData = false;


            if (!Double.IsNaN(coord))
            {
                if (!double.IsInfinity(coord))
                {
                    AddCoord = coord;
                }
                else
                {
                    if (double.IsPositiveInfinity(coord))
                    {
                        AddCoord = float.MaxValue;
                    }
                    else
                    {
                        AddCoord = float.MinValue;
                    }
                }


                if (!Double.IsNaN(Data))
                {
                    if (!double.IsInfinity(Data))
                    {
                        AddData = Data;
                    }
                    else
                    {
                        if (double.IsPositiveInfinity(Data))
                        {
                            AddData = float.MaxValue;
                        }
                        else
                        {
                            AddData = float.MinValue;
                        }
                    }


                    if (!Double.IsNaN(Data2))
                    {
                        if (!double.IsInfinity(Data2))
                        {
                            AddData2 = Data2;

                            ErrorData = false;
                        }
                        else
                        {
                            if (double.IsPositiveInfinity(Data2))
                            {
                                AddData2 = float.MaxValue;
                            }
                            else
                            {
                                AddData2 = float.MinValue;
                            }
                        }
                    }
                }
            }

            #endregion


            if (Name != null && Name != "")
            {
                if (!ErrorData)
                {
                    string CommandFist = string.Format(culture, "INSERT INTO {0} (id_Measurement_Result, Coord, Ampl, Phase) VALUES ('{4}', '{1}','{2}','{3}');", Name, AddCoord, AddData, AddData2, id_freq);
                    string CommandSecond = string.Format(culture, "INSERT INTO {0} (Coord, Ampl, Phase) VALUES ('{1}','{2}','{3}');", Name, AddCoord, AddData, AddData2);

                    this.Check_Old_or_New_Table(Name, id_freq, ref CommandFist, ref CommandSecond);
                    SqlCommand command = Sheild_M_measurement_Connection.CreateCommand();
                   

                    try
                    {
                        command.CommandText = CommandFist;
                        command.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            command.CommandText = CommandSecond;
                            command.ExecuteScalar();
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Trace.TraceError(string.Format("Попытка сохранения ошибочных данных в таблицу измерений {0}, DataBaseForm", Name));
                }
            }
            else
            {
                throw new Exception("Не указано имя таблицы");
            }
        }



        public int SaveMainTable(IResultType_MAIN SaveObj)
        {
            int ret = -1;

            int idPositioningParameters = this.SavePositioningParameters(SaveObj.MainOptions.Parameters);
            int idZVBParameters = this.SaveZVBParameters(SaveObj.MainOptions.Parameters);

            #region сохранение main

            DataSet1.mainRow AddingRow = Sheild_M_DataBase.main.NewmainRow();
            AddingRow.id_Type = (int)SaveObj.MainOptions.MeasurementResultType;
            AddingRow.Date = SaveObj.MainOptions.Date;
            AddingRow.Name = SaveObj.MainOptions.Name;
            AddingRow.id_Type_CalculationResult = (int)SaveObj.MainOptions.CalculationResultType;
            AddingRow.id_PositionParameters = idPositioningParameters;
            AddingRow.id_ZVB_Parameters = idZVBParameters;
            
            if (SaveObj.Antenn != null)
            {
                AddingRow.id_Antenn = SaveObj.Antenn.id;
            }
            if (SaveObj.Zond != null)
            {
                AddingRow.id_Tech_Antenn = SaveObj.Zond.id;
            }


            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();

            // Проверяем включена ли автонумерация id в БД
            command.CommandText = string.Format("SELECT IDENT_INCR('{0}')", AddingRow.Table.TableName);
           object INCR = command.ExecuteScalar();
           command.CommandText = string.Format("SELECT IDENT_SEED('{0}')", AddingRow.Table.TableName);
           object SEED = command.ExecuteScalar();


           if (INCR.ToString() != "" && SEED.ToString() != "")
           {
               //автонумерация включена - используем стандартную команду

               command.CommandText = string.Format(culture, "INSERT INTO {0}  ([id_Type],[Date],[Name],[id_Type_CalculationResult],[id_PositionParameters],[id_ZVB_Parameters],[id_Antenn],[id_Tech_Antenn]) VALUES ({1},'{2}','{3}',{4},{5},{6},{7},{8}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.id_Type, AddingRow.Date.ToString(), AddingRow.Name, AddingRow.id_Type_CalculationResult, AddingRow.id_PositionParameters, AddingRow.id_ZVB_Parameters, AddingRow.id_Antenn, AddingRow.id_Tech_Antenn);

               //получаем Id из БД
               AddingRow.id = Convert.ToInt32(command.ExecuteScalar());
           }
           else
           {
               //автонумерация выключена - используем команду с ручным id
               AddingRow.id = SaveObj.id;

               command.CommandText = string.Format(culture, "INSERT INTO {0}  ([id_Type],[Date],[Name],[id_Type_CalculationResult],[id_PositionParameters],[id_ZVB_Parameters],[id_Antenn],[id_Tech_Antenn],[id]) VALUES ({1},'{2}','{3}',{4},{5},{6},{7},{8},{9});",
                    AddingRow.Table.TableName,
                    AddingRow.id_Type, AddingRow.Date.ToString(), AddingRow.Name, AddingRow.id_Type_CalculationResult, AddingRow.id_PositionParameters, AddingRow.id_ZVB_Parameters, AddingRow.id_Antenn, AddingRow.id_Tech_Antenn, AddingRow.id);

               command.ExecuteScalar();
           }
             

            //добавить запись в бд
            Sheild_M_DataBase.main.AddmainRow(AddingRow);
            
            //добавляем выданный id
            ret = AddingRow.id;
            SaveObj.id = ret;
            #endregion


            this.SaveSegmentTable(new List<ISegmentTableElementClass>(SaveObj.MainOptions.Parameters.SegmentTable.ToArray()), ret);

            bool CreateResultTable = true;

            if (SaveObj is IResultType_КУ)
            {
                IResultType_КУ tempResult = SaveObj as IResultType_КУ;

                this.SaveFrequencyElement(tempResult.Main_Polarization.FrequencyElements, ret, 0, ref CreateResultTable);
                this.SaveFrequencyElement(tempResult.Cross_Polarization.FrequencyElements, ret, 1, ref CreateResultTable);
            }
            else
            {
                if (SaveObj is IResultType_СДНМ)
                {
                    IResultType_СДНМ tempResult = SaveObj as IResultType_СДНМ;

                    this.SaveFrequencyElement(tempResult.Main_Polarization.FrequencyElements, ret, 0, ref CreateResultTable);
                    this.SaveFrequencyElement(tempResult.Cross_Polarization.FrequencyElements, ret, 1, ref CreateResultTable);
                }
                else
                {
                    this.SaveFrequencyElement(SaveObj.SelectedPolarization.FrequencyElements, ret, 0, ref CreateResultTable);
                }
            }

            //сохраняем исходный результат, если необходимо
            if (SaveObj.MainOptions.CalculationResultType != CalculationResultTypeEnum.None)
            {
                ResultTypeClassUnion Union = SaveObj as ResultTypeClassUnion;

                if (Union != null)
                {
                    this.SaveInitial_Results(Union.InitialResults.ToArray<IResultType_MAIN>(), ret);
                }
            }

            //добавляем в treeview
            this.AddMainRowToTreeView(Sheild_M_DataBase.main[Sheild_M_DataBase.main.Count - 1], true);


            return ret;
        }

        public int SaveFrequencyElement(FrequencyElementClass SaveObj, int idMain, int PolarizationType, ref bool CreateResultTable)
        {
            int ret = -1;
                       
            DataSet1.FrequenciesRow AddingRow = Sheild_M_DataBase.Frequencies.NewFrequenciesRow();
            AddingRow.Freq_MHz = SaveObj.Frequency;
            AddingRow.Result_Table_Name = "Measurement_" + idMain.ToString();
            AddingRow.Polarization_Type = PolarizationType;
            AddingRow.id_main = idMain;

            AddingRow.id_Calculation_Table = SaveCalculationElement(SaveObj._CalculationResults);


            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ([Freq_MHz],[Result_Table_Name],[Polarization_Type],[id_main],[id_Calculation_Table])VALUES ({1},'{2}',{3},{4},{5}); SELECT IDENT_CURRENT('{0}');",
                AddingRow.Table.TableName,
                AddingRow.Freq_MHz, AddingRow.Result_Table_Name, AddingRow.Polarization_Type, AddingRow.id_main, AddingRow.id_Calculation_Table);

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());            

            //добавить запись в бд
            Sheild_M_DataBase.Frequencies.AddFrequenciesRow(AddingRow);
            //добавляем выданный id
            ret = AddingRow.id;
                       
            if (CreateResultTable)
            {
                this.CreateMeasurementTable(AddingRow.Result_Table_Name);
                CreateResultTable = false;
            }

            this.SaveResultElementS(SaveObj.ResultAmpl_PhaseElements, AddingRow.Result_Table_Name, ret);

            return ret;
        }

        public void SaveResultElementS(List<ResultElementClass> ResultAmpl_PhaseElements, string TableName, int id_freq)
        {
            foreach (ResultElementClass res in ResultAmpl_PhaseElements)
            {
                this.AddToMeasurementTable(TableName, id_freq, Convert.ToSingle(res.Cordinate), Convert.ToSingle(res.Ampl_dB), Convert.ToSingle(res.Phase_degree));
            }
        }

        public void SaveFrequencyElement(IList<FrequencyElementClass> Frequency, int idMain, int PolarizationType, ref bool CreateResultTable)
        {
            foreach (FrequencyElementClass fr in Frequency)
            {
                this.SaveFrequencyElement(fr, idMain, PolarizationType, ref CreateResultTable);
            }
        }

        public int SaveCalculationElement(CalculationResultsClass SaveObj)
        {
            int ret = -1;

            DataSet1.CalculateDataTableRow AddingRow = Sheild_M_DataBase.CalculateDataTable.NewCalculateDataTableRow();


            string FistPart = "";
            string SecondPart = "";

            if (CheckDataClass.CheckForBad(SaveObj.Поляризационное_отношение))
            {
                AddingRow.Polarizatio_Relation_Real = SaveObj.Поляризационное_отношение.Real;
                AddingRow.Polarizatio_Relation_Imag = SaveObj.Поляризационное_отношение.Imaginary;

                FistPart += "[Polarizatio_Relation_Real],[Polarizatio_Relation_Imag]";
                SecondPart += string.Format(culture, "{0},{1}", AddingRow.Polarizatio_Relation_Real, AddingRow.Polarizatio_Relation_Imag);
            }
            if (CheckDataClass.CheckForBad(SaveObj.Угол_наклона_эллипса_поляризации))
            {
                AddingRow.Angle_Ratio = SaveObj.Угол_наклона_эллипса_поляризации;

                FistPart += ",[Angle_Ratio]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.Angle_Ratio);
            }
            if (CheckDataClass.CheckForBad(SaveObj.Коэффициент_Эллиптичности))
            {
                AddingRow.Axial_Ratio = SaveObj.Коэффициент_Эллиптичности;

                FistPart += ",[Axial_Ratio]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.Axial_Ratio);
            }
            if (CheckDataClass.CheckForBad(SaveObj.delta_M.Mistake))
            {
                AddingRow.delta_Axial_Ratio = SaveObj.delta_M.Mistake;

                FistPart += ",[delta_Axial_Ratio]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.delta_Axial_Ratio);
            }
            if (CheckDataClass.CheckForBad(SaveObj.deltaMistakeFull.Mistake))
            {
                AddingRow.delta_Gain = SaveObj.deltaMistakeFull.Mistake;

                FistPart += ",[delta_Gain]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.delta_Gain);
            }
            if (CheckDataClass.CheckForBad(SaveObj.delta_Fo.Mistake))
            {
                AddingRow.delta_Dir_Part = SaveObj.delta_Fo.Mistake;

                FistPart += ",[delta_Dir_Part]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.delta_Dir_Part);
            }
            if (CheckDataClass.CheckForBad(SaveObj.delta_Фо.Mistake))
            {
                AddingRow.delta_Phase = SaveObj.delta_Фо.Mistake;

                FistPart += ",[delta_Phase]";
                SecondPart += string.Format(culture, ",{0}", AddingRow.delta_Phase);
            }

            if (CheckDataClass.CheckForBad(SaveObj.Координаты_фазового_центра_Decart))
            {
                AddingRow.PhazeCenter_Decart_X = SaveObj.Координаты_фазового_центра_Decart.X;
                AddingRow.PhazeCenter_Decart_Y = SaveObj.Координаты_фазового_центра_Decart.Y;

                FistPart += ",[PhazeCenter_Decart_X],[PhazeCenter_Decart_Y]";
                SecondPart += string.Format(culture, ",{0},{1}", AddingRow.PhazeCenter_Decart_X, AddingRow.PhazeCenter_Decart_Y);
            }


            FistPart = FistPart.TrimStart(',');
            SecondPart = SecondPart.TrimStart(',');

            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();

            if (FistPart == "")
            {
                command.CommandText = string.Format(culture, "INSERT INTO {0} (Angle_Ratio) values (null); SELECT IDENT_CURRENT('{0}');",
                     AddingRow.Table.TableName);
            }
            else
            {
            
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ({1}) VALUES ({2}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                   FistPart, SecondPart);
            }

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());

            //добавить запись в бд
            Sheild_M_DataBase.CalculateDataTable.AddCalculateDataTableRow(AddingRow);

            //добавляем выданный id
            ret = AddingRow.id;

            return ret;
        }

        public int SavePositioningParameters(MeasurementParametrsClass SaveObj)
        {
            int ret = -1;

            DataSet1.Measurement_PositionParametersRow AddingRow = Sheild_M_DataBase.Measurement_PositionParameters.NewMeasurement_PositionParametersRow();
            AddingRow.Stop_Tower_W_degree = Convert.ToDouble(SaveObj.StopTower_W);
            AddingRow.Stop_Tower_Y_m = Convert.ToDouble(SaveObj.StopTower_Y);
            AddingRow.Stop_OPU_W_degree = Convert.ToDouble(SaveObj.StopOPU_W);
            AddingRow.Stop_OPU_Y_degree = Convert.ToDouble(SaveObj.StopOPU_Y);
            AddingRow.Start_Tower_W_degree = Convert.ToDouble(SaveObj.StartTower_W);
            AddingRow.Start_Tower_Y_m = Convert.ToDouble(SaveObj.StartTower_Y);
            AddingRow.Start_OPU_W_degree = Convert.ToDouble(SaveObj.StartOPU_W);
            AddingRow.Start_OPU_Y_degree = Convert.ToDouble(SaveObj.StartOPU_Y);
            AddingRow.Step_degree = Convert.ToDouble(SaveObj.StepMeasurement);


            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ([Stop_Tower_W_degree],[Stop_Tower_Y_m],[Stop_OPU_W_degree],[Stop_OPU_Y_degree],[Start_Tower_W_degree],[Start_Tower_Y_m],[Start_OPU_W_degree],[Start_OPU_Y_degree],[Step_degree]) VALUES ({1},{2},{3},{4},{5},{6},{7},{8},{9}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.Stop_Tower_W_degree, AddingRow.Stop_Tower_Y_m, AddingRow.Stop_OPU_W_degree, AddingRow.Stop_OPU_Y_degree, AddingRow.Start_Tower_W_degree, AddingRow.Start_Tower_Y_m, AddingRow.Start_OPU_W_degree, AddingRow.Start_OPU_Y_degree, AddingRow.Step_degree);

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());


            //добавить запись в бд
            Sheild_M_DataBase.Measurement_PositionParameters.AddMeasurement_PositionParametersRow(AddingRow);
            //добавляем выданный id
            ret = AddingRow.id;

            return ret;
        }

        public int SaveZVBParameters(MeasurementParametrsClass SaveObj)
        {
            int ret = -1;

            DataSet1.Measurement_ZVBParametersRow AddingRow = Sheild_M_DataBase.Measurement_ZVBParameters.NewMeasurement_ZVBParametersRow();
            AddingRow.Power = Convert.ToDouble(SaveObj.Power);
            AddingRow.SweepTime = Convert.ToDouble(SaveObj.SweepTime);
            AddingRow.SweepTimeAuto = SaveObj.SweepTimeAuto;
            AddingRow.Bandwidth = Convert.ToDouble(SaveObj.Bandwidth);
            AddingRow.Measurement_Tupe_S = (int)SaveObj.MesurementS__Type;


            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ([Power],[SweepTime],[SweepTimeAuto],[Bandwidth],[Measurement_Tupe_S]) VALUES ({1},{2},{3},{4},{5}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.Power, AddingRow.SweepTime, Convert.ToInt32(AddingRow.SweepTimeAuto), AddingRow.Bandwidth, AddingRow.Measurement_Tupe_S);

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());

            //добавить запись в бд
            Sheild_M_DataBase.Measurement_ZVBParameters.AddMeasurement_ZVBParametersRow(AddingRow);
            //добавляем выданный id
            ret = AddingRow.id;

            return ret;
        }

        public int SaveSegmentTable(ISegmentTableElementClass SaveObj, int idMain)
        {
            int ret = -1;

            DataSet1.Segment_TableRow AddingRow = Sheild_M_DataBase.Segment_Table.NewSegment_TableRow();
            AddingRow.id_main = idMain;
            AddingRow.Start_Frequency_MHz = SaveObj.FrequencieStart;
            AddingRow.Stop_Frequency_MHz = SaveObj.FrequencieStop;
            AddingRow.Number_of_points = SaveObj.NumberOfPoint;
            
            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ([id main],[Start_Frequency_MHz],[Stop_Frequency_MHz],[Number_of_points]) VALUES ({1},{2},{3},{4}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.id_main, AddingRow.Start_Frequency_MHz, AddingRow.Stop_Frequency_MHz, AddingRow.Number_of_points);

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());

            //добавить запись в бд
            Sheild_M_DataBase.Segment_Table.AddSegment_TableRow(AddingRow);
            //добавляем выданный id
            ret = AddingRow.id;

            return ret;
        }

        public void SaveSegmentTable(List<ISegmentTableElementClass> SaveObj, int idMain)
        {
            foreach (ISegmentTableElementClass temp in SaveObj)
            {
                this.SaveSegmentTable(temp, idMain);
            }
        }

        protected void SaveInitial_Results(IResultType_MAIN SaveObj, int idMain)
        {
            DataSet1.Initial_ResultsRow AddingRow = Sheild_M_DataBase.Initial_Results.NewInitial_ResultsRow();
            AddingRow.id_Output_Result = idMain;
            AddingRow.id_Input_Result = SaveObj.id;
            AddingRow.id_Frequency = SaveObj.SelectedPolarization.SelectedFrequency.id;

            SqlCommand command = Sheild_M_DataBase_Connection.CreateCommand();
            command.CommandText = string.Format(culture, "INSERT INTO {0}  ([id_Output_Result],[id_Input_Result],[id_Frequency]) VALUES ({1},{2},{3}); SELECT IDENT_CURRENT('{0}');",
                    AddingRow.Table.TableName,
                    AddingRow.id_Output_Result, AddingRow.id_Input_Result, AddingRow.id_Frequency);

            //получаем Id из БД
            AddingRow.id = Convert.ToInt32(command.ExecuteScalar());

            //добавить запись в бд
            Sheild_M_DataBase.Initial_Results.AddInitial_ResultsRow(AddingRow);
        }

        protected void SaveInitial_Results(IResultType_MAIN[] SaveObj, int idMain)
        {
            foreach (IResultType_MAIN res in SaveObj)
            {
                SaveInitial_Results(res, idMain);
            }
        }

        #endregion



        #region преобразование ROW to Class

        protected List<ResultType_MAINClass> LoadAllResultsByAnten(List<DataSet1.AntennasRow> ant)
        {
            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            foreach (DataSet1.AntennasRow anten in ant)
            {
                ret.AddRange(LoadAllResultsByAnten(anten));
            }

            return ret;
        }

        protected List<ResultType_MAINClass> LoadAllResultsByAnten(DataSet1.AntennasRow ant)
        {
            DataSet1.mainRow[] mainRows = ant.GetmainRows();

            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            foreach (DataSet1.mainRow mainRow in mainRows)
            {
                ret.Add(this.GetResultMainByFrequenciesRow(mainRow, null, false));
            }

            return ret;
        }

        protected ResultType_MAINClass GetResultMainByFrequenciesRow(DataSet1.FrequenciesRow tempFreqRow)
        {
            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            return this.GetResultMainByFrequenciesRow(tempMainRow, tempFreqRow, false);
        }

        /// <summary>
        /// Загрузить весь результат
        /// </summary>
        /// <param name="tempMainRow">Главная запись в таблице</param>
        /// <param name="tempFreqRow">необходимая частота, если Null - будут загружены все частоты</param>
        /// <returns></returns>
        protected ResultType_MAINClass GetResultMainByFrequenciesRow(DataSet1.mainRow tempMainRow, DataSet1.FrequenciesRow tempFreqRow, bool LoadOnlyOptions, bool LoadResultData = true)
        {
            //создаём объект результата
            ResultType_MAINClass result = null;


            bool IsCalculationResult = false;
            if (!tempMainRow.Isid_Type_CalculationResultNull())
            {
                if (tempMainRow.id_Type_CalculationResult != (int)CalculationResultTypeEnum.None)
                {
                    IsCalculationResult = true;
                    result = new ResultTypeClassUnion(true);
                }
            }

            if (!IsCalculationResult)
            {
                switch (tempMainRow.id_Type)
                {
                    case (int)MeasurementTypeEnum.Поляризационная_диаграмма:
                        {
                            result = new ResultTypeПХ(true);
                            break;
                        }
                    case (int)MeasurementTypeEnum.ДН_Меридиан:
                        {
                            result = new ResultTypeClassДН(true);
                            break;
                        }
                    case (int)MeasurementTypeEnum.ДН_Азимут:
                        {
                            result = new ResultTypeClassДН(true);
                            break;
                        }
                    case (int)MeasurementTypeEnum.Коэффицент_усиления:
                        {
                            result = new ResultTypeClassКУ(true);
                            break;
                        }
                    case (int)MeasurementTypeEnum.Суммарная_ДН_Меридиан:
                        {
                            result = new ResultTypeClassСДНМ(true);
                            break;
                        }
                    case (int)MeasurementTypeEnum.Суммарная_ДН_Азимут:
                        {
                            result = new ResultTypeClassСДНМ(true);
                            break;
                        }
                }
            }

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_MainRow(result, tempMainRow);

            bool HideFreq = false;

            if (result.MainOptions.Parameters.SegmentTable.Count == 0)
            {
                HideFreq = true;
            }

            if (!LoadOnlyOptions)
            {
                //грузим результаты
                if (result.MainOptions.CalculationResultType == CalculationResultTypeEnum.None)
                {
                    //получить измеренные данные
                    if (result.MainOptions.MeasurementResultType == MeasurementTypeEnum.Коэффицент_усиления)
                    {
                        this.FillMeasurementData(tempMainRow, null, ref result, HideFreq, LoadResultData);
                    }
                    else
                    {
                        this.FillMeasurementData(tempMainRow, tempFreqRow, ref result, HideFreq, LoadResultData);
                    }
                }
                else
                {
                    this.GetMeasurementDataByFrequenciesRow_for_Union(tempMainRow, result as ResultTypeClassUnion);
                }

            }


            return result;
        }

        /// <summary>
        /// заполнить общие (базовые) данные для измерения
        /// </summary>
        /// <param name="ResultType"></param>
        /// <param name="tempFreqRow"></param>
        protected void FillResultType_by_MainRow(ResultType_MAINClass ResultType, DataSet1.mainRow tempMainRow)
        {
            //получить опции
            MainOptionsClass tempmainOptions = this.GetMainOptionsClass(tempMainRow);
            //получить антенну
            AntennOptionsClass tempanten = this.MainRowToAntennClassIA(tempMainRow);

            //получить тех антенну
            AntennOptionsClass tempZond = this.MainRowToAntennClassTA(tempMainRow);

            //добавть полученные данные
            ResultType.Antenn = tempanten;
            ResultType.Zond = tempZond;
            ResultType.MainOptions = tempmainOptions;
            ResultType.id = tempMainRow.id;
        }

        /// <summary>
        /// получить частотный элемент из записи в таблице частот
        /// </summary>
        /// <param name="tempFreg"></param>
        /// <returns></returns>
        protected FrequencyElementClass GetFrequencyElementByFrequenciesRow(DataSet1.FrequenciesRow tempFreg, bool LoadResultData = true)
        {
            FrequencyElementClass ret = new FrequencyElementClass(tempFreg.Freq_MHz);
            ret.id = tempFreg.id;
            ret.TableResultName = tempFreg.Result_Table_Name.TrimEnd(" ".ToCharArray());


            if (LoadResultData)
            {
                //костыль для ускорния загрузки
                string CommandFist = string.Format("SELECT * FROM {0} WHERE id_Measurement_Result={1} ORDER BY Coord ASC", ret.TableResultName, ret.id);
                string CommandSecond = string.Format("SELECT * FROM {0} ORDER BY Coord ASC", ret.TableResultName);

                Check_Old_or_New_Table(ret.TableResultName, ret.id, ref CommandFist, ref CommandSecond);

                try
                {
                    DataSet1.DataDataTable tempDataTable = new DataSet1.DataDataTable();

                    try
                    {
                        SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(CommandFist, this.Sheild_M_measurement_Connection);
                        DataAdaptermeasurement.Fill(tempDataTable);
                    }
                    catch (Exception)
                    {
                        SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(CommandSecond, this.Sheild_M_measurement_Connection);
                        DataAdaptermeasurement.Fill(tempDataTable);
                    }

                    foreach (DataSet1.DataRow znRow in tempDataTable)
                    {
                        ret.ResultAmpl_PhaseElements.Add(new ResultElementClass(znRow.Coord, znRow.Ampl, znRow.Phase));
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(string.Format("Ошибка загрузки данных из БД результатов измерения \n{0}", ex.Message), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            #region загружаем рассчитанные данные

            DataSet1.CalculateDataTableRow[] calTables = tempFreg.GetCalculateDataTableRows();

            if (calTables.Length != 0)
            {
                //используем только первую записть, тк больше их не может быть по структуре связи в бд
                DataSet1.CalculateDataTableRow cal = calTables[0];

                if (!cal.IsPolarizatio_Relation_RealNull() && !cal.IsPolarizatio_Relation_ImagNull())
                {
                    ComplexClass pol = new ComplexClass(cal.Polarizatio_Relation_Real, cal.Polarizatio_Relation_Imag);
                    ret._CalculationResults.Поляризационное_отношение = pol;
                }

                if (!cal.IsAngle_RatioNull())
                {
                    ret._CalculationResults.Угол_наклона_эллипса_поляризации = cal.Angle_Ratio;
                }

                if (!cal.IsAxial_RatioNull())
                {
                    ret._CalculationResults.Коэффициент_Эллиптичности = cal.Axial_Ratio;
                }
                if (!cal.Isdelta_Axial_RatioNull())
                {
                    ret._CalculationResults.delta_M = new MistakeClass(cal.delta_Axial_Ratio);
                }
                if (!cal.Isdelta_GainNull())
                {
                    ret._CalculationResults.deltaMistakeFull = new MistakeClass(cal.delta_Gain);
                }
                if (!cal.Isdelta_Dir_PartNull())
                {
                    ret._CalculationResults.delta_Fo = new MistakeClass(cal.delta_Dir_Part);
                }
                if (!cal.Isdelta_PhaseNull())
                {
                    ret._CalculationResults.delta_Фо = new MistakeClass(cal.delta_Phase);
                }

                if (!cal.IsPhazeCenter_Decart_XNull())
                {
                    ret._CalculationResults.Координаты_фазового_центра_Decart.X = cal.PhazeCenter_Decart_X;
                }
                if (!cal.IsPhazeCenter_Decart_YNull())
                {
                    ret._CalculationResults.Координаты_фазового_центра_Decart.Y = cal.PhazeCenter_Decart_Y;
                }
            }


            CalculationResultsClass.CalculateAll_NO_Constant(ret);

            #endregion

            return ret;
        }

        /// <summary>
        /// заполнить измеренные данные
        /// </summary>
        /// <param name="newtempfreq"></param>
        /// <param name="result"></param>
        protected void FillMeasurementData(DataSet1.mainRow tempMain, DataSet1.FrequenciesRow newtempfreq, ref ResultType_MAINClass result, bool HideFreq, bool LoadResulData = true)
        {
            #region заполняем Frequencies

            List<DataSet1.FrequenciesRow> TempListFreq = this.GetAllFrequenciesRow(tempMain, newtempfreq);

            List<FrequencyElementClass> Main = new List<FrequencyElementClass>();
            List<FrequencyElementClass> Cros = new List<FrequencyElementClass>();

            //разбиваем по поляризациям
            foreach (DataSet1.FrequenciesRow tempFreqRow in TempListFreq)
            {
                FrequencyElementClass temp = this.GetFrequencyElementByFrequenciesRow(tempFreqRow, LoadResulData);
                temp.IsHideFrequency = HideFreq;

                if (tempFreqRow.Polarization_Type == 0)
                {
                    Main.Add(temp);
                }

                if (tempFreqRow.Polarization_Type == 1)
                {
                    Cros.Add(temp);
                }
            }
            #endregion

            if (result is ResultTypeClassСДНМ)
            {
                ResultTypeClassСДНМ tempRes = result as ResultTypeClassСДНМ;

                tempRes.Main_Polarization.FrequencyElements = Main;
                tempRes.Cross_Polarization.FrequencyElements = Cros;
            }
            else
            {
                if (result is ResultTypeClassКУ)
                {
                    ResultTypeClassКУ tempRes = result as ResultTypeClassКУ;

                    tempRes.Main_Polarization.FrequencyElements = Main;
                    tempRes.Cross_Polarization.FrequencyElements = Cros;
                }
                else
                {
                    result.SelectedPolarization.FrequencyElements = Main;
                }
            }

            result.CalculateSpesialPolarizations();
        }

        protected void GetMeasurementDataByFrequenciesRow_for_Union(DataSet1.mainRow mainRow, ResultTypeClassUnion Union)
        {
            //добавляем исходные результаты

            DataSet1.Initial_ResultsRow[] InitialRows = mainRow.GetInitial_ResultsRows();

            if (InitialRows.Length != 0)
            {
                foreach (DataSet1.Initial_ResultsRow row in InitialRows)
                {
                    DataSet1.FrequenciesRow[] freqList = row.GetFrequenciesRows();
                    if (freqList.Length > 0)
                    {
                        ResultType_MAINClass result = this.GetResultMainByFrequenciesRow(freqList[0]);

                        Union.AddToInitialResults(result);
                    }
                }
            }

            //блокируем авто рассчёт
            Union.UserSetCalculationData = true;

            //получить рассчитанные данные
            //Union.SelectedPolarization.SelectedFrequency=this.GetFrequencyElementByFrequenciesRow(tempFreqRow);
            ResultType_MAINClass res = Union as ResultType_MAINClass;
            this.FillMeasurementData(mainRow, null, ref res, true);
        }

        #region получение полных имён
        /// <summary>
        /// получить полное имя по частоте
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected string GetFullNameByFrequencyRow(DataSet1.FrequenciesRow FrequencyResult)
        {
            string FullName = "";
            string AntennName = "не указана";
            string mainName = "Без названия";
            //string polarization = "не указана";
            string DateTime1 = "";
            string frequencyName = Math.Round(FrequencyResult.Freq_MHz, 1).ToString();

            DataSet1.mainRow tempMain = this.GetMainRowByFrequencyRow(FrequencyResult);
            DataSet1.AntennasRow tempantenn = tempMain.AntennasRow;

            //if (FrequencyResult.Polarization_Type == 0)
            //{
            //    polarization = "Main";
            //}
            //else
            //{
            //    polarization = "Cross";
            //}

            if (tempantenn != null)
            {
                AntennName = tempantenn.Name.TrimEnd(" ".ToCharArray());
            }

            if (!tempMain.IsNameNull())
            {
                mainName = tempMain.Name.TrimEnd(" ".ToCharArray()) + " " + tempMain.id.ToString();
            }
            DateTime1 = tempMain.Date.ToShortDateString();

            FullName = ResultType_MAINClass.CreateFullName(mainName, frequencyName, DateTime1, AntennName);

            return FullName;
        }

        public string GetFullNameByResult(ResultType_MAINClass Result)
        {
            string FullName = "";
            string AntennName = "не указана";
            string mainName = "Без названия";
            string polarization = "не указана";
            string DateTime1 = "";
            string frequencyName = Math.Round(Result.SelectedPolarization.SelectedFrequency.Frequency, 1).ToString();

            polarization = " " + Result.SelectedPolarization.Polarization.ToString();


            AntennName = Result.Antenn.Name;

            mainName = Result.MainOptions.Name;
            DateTime1 = Result.MainOptions.Date.ToString();

            FullName = ResultType_MAINClass.CreateFullName(mainName, frequencyName, DateTime1, AntennName, polarization);
            return FullName;
        }

        #endregion

        protected DataSet1.mainRow GetMainRowByFrequencyRow(DataSet1.FrequenciesRow FrequencyResult)
        {
            return FrequencyResult.mainRow;
        }

        #region получение разноообразных нодов частот

        /// <summary>
        /// преобразовать строки в таблшице частот в ноды
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected TreeNode[] GetFrequencyNodes(DataSet1.FrequenciesRow[] FrequencyResult, MeasurementTagTypeEnum NodeType)
        {
            List<TreeNode> ret = new List<TreeNode>();
            //заполняем ноды
            foreach (DataSet1.FrequenciesRow freg in FrequencyResult)
            {
                if (NodeType == MeasurementTagTypeEnum.DN_Normal || NodeType == MeasurementTagTypeEnum.Union || NodeType == MeasurementTagTypeEnum.KY)
                {
                    ret.Add(this.GetFrequencyNodes(freg, NodeType));
                }
                else
                {
                    if (NodeType == MeasurementTagTypeEnum.SDNM_Main || NodeType == MeasurementTagTypeEnum.SDNM_Sum)
                    {
                        if (freg.Polarization_Type == 0)
                        {
                            ret.Add(this.GetFrequencyNodes(freg, NodeType));
                        }
                    }
                    else
                    {
                        if (NodeType == MeasurementTagTypeEnum.SDNM_Cross)
                        {
                            if (freg.Polarization_Type == 1)
                            {
                                ret.Add(this.GetFrequencyNodes(freg, NodeType));
                            }
                        }
                    }
                }
            }
            return ret.ToArray();
        }


        /// <summary>
        /// преобразовать строку в таблшице частот в нод
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected TreeNode GetFrequencyNodes(DataSet1.FrequenciesRow FrequencyResult, MeasurementTagTypeEnum NodeType)
        {
            string freqStr = " МГц";

            //Это костыль 
            if (FrequencyResult.Freq_MHz <= 50)
            {
                freqStr = "№ " + Math.Round(FrequencyResult.Freq_MHz, 1).ToString();
            }
            else
            {
                freqStr = Math.Round(FrequencyResult.Freq_MHz, 1).ToString() + " МГц";
            }

            TreeNode fregNode = new TreeNode(freqStr);

            DBLoaderPackClass Pack = new DBLoaderPackClass();
            Pack.FormShowType = FormShowTypeEnum.Result;
            Pack.MeasurementTagType = NodeType;
            Pack.ObjectForLoad = FrequencyResult;
            Pack.SpesialText = string.Format("Это частота {0} ({1}); {2}", FrequencyResult.id, NodeType, FrequencyResult.Result_Table_Name.TrimEnd(" ".ToCharArray()));
            fregNode.Tag = Pack;

            return fregNode;
        }

        protected TreeNode GetPolarizationNode(DataSet1.mainRow main, string NodeName, MeasurementTagTypeEnum NodeType)
        {
            TreeNode ret = new TreeNode(NodeName);

            ret.Nodes.AddRange(this.GetFrequencyNodes(main.GetFrequenciesRows(), NodeType));

            return ret;
        }

        #endregion

        /// <summary>
        /// получить класс опций по записи main
        /// </summary>
        /// <param name="tempMain"></param>
        /// <returns></returns>
        MainOptionsClass GetMainOptionsClass(DataSet1.mainRow tempMain)
        {
            MainOptionsClass tempmainOptions = new MainOptionsClass();
            #region Описание типа измерения

            tempmainOptions.MeasurementResultType = (MeasurementTypeEnum)tempMain.id_Type;
            tempmainOptions.MeasurementResultTypeName = MeasurementTypeDescription.MeasurementType_Name[tempmainOptions.MeasurementResultType];
            tempmainOptions.MeasurementResultTypeDescription = MeasurementTypeDescription.MeasurementType_Description[tempmainOptions.MeasurementResultType];


            if (!tempMain.Isid_Type_CalculationResultNull())
            {
                tempmainOptions.CalculationResultType = (CalculationResultTypeEnum)tempMain.id_Type_CalculationResult;
            }

            #endregion

            #region наименование, описание и дата
            tempmainOptions.Date = tempMain.Date;

            if (!tempMain.IsNameNull())
            {
                tempmainOptions.Name = tempMain.Name.TrimEnd(" ".ToCharArray()); ;
            }

            #endregion

            #region Получаем связанные таблицы параметров позиций
            DataSet1.Measurement_PositionParametersRow[] MeasurementParamRows = tempMain.GetMeasurement_PositionParametersRows();

            if (MeasurementParamRows.Length != 0)
            {
                tempmainOptions.Parameters.StepMeasurement = Convert.ToDecimal(MeasurementParamRows[0].Step_degree);

                tempmainOptions.Parameters.StartOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Start_OPU_W_degree);
                tempmainOptions.Parameters.StartOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Start_OPU_Y_degree);
                tempmainOptions.Parameters.StartTower_W = Convert.ToDecimal(MeasurementParamRows[0].Start_Tower_W_degree);
                tempmainOptions.Parameters.StartTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Start_Tower_Y_m);

                if (!MeasurementParamRows[0].IsStop_OPU_W_degreeNull())
                {
                    tempmainOptions.Parameters.StopOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Stop_OPU_W_degree);
                }
                else
                {
                    tempmainOptions.Parameters.StopOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Start_OPU_W_degree);
                }

                if (!MeasurementParamRows[0].IsStop_OPU_Y_degreeNull())
                {
                    tempmainOptions.Parameters.StopOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Stop_OPU_Y_degree);
                }
                else
                {
                    tempmainOptions.Parameters.StopOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Start_OPU_Y_degree);
                }

                if (!MeasurementParamRows[0].IsStop_Tower_W_degreeNull())
                {
                    tempmainOptions.Parameters.StopTower_W = Convert.ToDecimal(MeasurementParamRows[0].Stop_Tower_W_degree);
                }
                else
                {
                    tempmainOptions.Parameters.StopTower_W = Convert.ToDecimal(MeasurementParamRows[0].Start_Tower_W_degree);
                }

                if (!MeasurementParamRows[0].IsStop_Tower_Y_mNull())
                {
                    tempmainOptions.Parameters.StopTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Stop_Tower_Y_m);
                }
                else
                {
                    tempmainOptions.Parameters.StopTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Start_Tower_Y_m);
                }
            }
            #endregion

            #region Получаем связанные таблицы параметров ZVB
            DataSet1.Measurement_ZVBParametersRow[] MeasurementParamZVBRows = tempMain.GetMeasurement_ZVBParametersRows();

            if (MeasurementParamZVBRows.Length != 0)
            {
                tempmainOptions.Parameters.Bandwidth = Convert.ToDecimal(MeasurementParamZVBRows[0].Bandwidth);
                tempmainOptions.Parameters.Power = Convert.ToDecimal(MeasurementParamZVBRows[0].Power);
                tempmainOptions.Parameters.SweepTime = Convert.ToDecimal(MeasurementParamZVBRows[0].SweepTime);
                tempmainOptions.Parameters.SweepTimeAuto = MeasurementParamZVBRows[0].SweepTimeAuto;

                tempmainOptions.Parameters.MesurementS__Type = (MeasurementS__Enum)MeasurementParamZVBRows[0].Measurement_Tupe_S;

                //switch (MeasurementParamZVBRows[0].Measurement_Tupe_S)
                //{
                //    case (int)MeasurementS__Enum.S11:
                //        tempmainOptions.Parameters.MesurementS__Type = MeasurementS__Enum.S11;
                //        break;
                //    case (int)MeasurementS__Enum.S12:
                //        tempmainOptions.Parameters.MesurementS__Type = MeasurementS__Enum.S12;
                //        break;
                //    case (int)MeasurementS__Enum.S21:
                //        tempmainOptions.Parameters.MesurementS__Type = MeasurementS__Enum.S21;
                //        break;
                //    case (int)MeasurementS__Enum.S22:
                //        tempmainOptions.Parameters.MesurementS__Type = MeasurementS__Enum.S22;
                //        break;
                //}
            }
            #endregion

            #region Получаем сегментную таблицу
            DataSet1.Segment_TableRow[] Rows = tempMain.GetSegment_TableRows();

            foreach (DataSet1.Segment_TableRow tempRow in Rows)
            {
                SegmentTableElementOptionsClass tempSegment = new SegmentTableElementOptionsClass();
                tempSegment.FrequencieStart = tempRow.Start_Frequency_MHz;
                tempSegment.FrequencieStop = tempRow.Stop_Frequency_MHz;
                tempSegment.NumberOfPoint = tempRow.Number_of_points;
                tempmainOptions.Parameters.SegmentTable.Add(tempSegment);
            }
            #endregion

            return tempmainOptions;
        }

        /// <summary>
        /// Загрузить из базы все записи частот
        /// </summary>
        /// <param name="tempMain"></param>
        /// <param name="newtempfreq">если здесь null то будут загружены все часттные элементы, иначе только с равной частотой</param>
        /// <returns></returns>
        protected List<DataSet1.FrequenciesRow> GetAllFrequenciesRow(DataSet1.mainRow tempMain, DataSet1.FrequenciesRow newtempfreq)
        {
            List<DataSet1.FrequenciesRow> ret = new List<DataSet1.FrequenciesRow>();

            DataSet1.FrequenciesRow[] tempFreq = tempMain.GetFrequenciesRows();

            if (newtempfreq != null)
            {
                foreach (DataSet1.FrequenciesRow tempFreqRow in tempFreq)
                {
                    if (tempFreqRow.Freq_MHz == newtempfreq.Freq_MHz)
                    {
                        ret.Add(tempFreqRow);
                    }
                }
            }
            else
            {
                ret.AddRange(tempFreq);
            }

            return ret;
        }

        #region для антенн

        protected AntennOptionsClass AntennRowToClass(DataSet1.AntennasRow tempAnten)
        {
            AntennOptionsClass anten = new AntennOptionsClass();
            anten.Name = tempAnten.Name.TrimEnd(" ".ToCharArray());


            anten.Description = tempAnten.Description.TrimEnd(" ".ToCharArray());

            anten.ZAVNumber = tempAnten.Serial_Number.TrimEnd(" ".ToCharArray());
            anten.UsingAsZond = tempAnten.Using_as_TA;

            anten.id = tempAnten.id;

            #region получить частотные характеристики технической антенны

            DataSet1.Tech_Antenn_ParametersRow[] TA_List = tempAnten.GetTech_Antenn_ParametersRows();
            foreach (DataSet1.Tech_Antenn_ParametersRow TA in TA_List)
            {
                TA_OptionsClass TATemp = new TA_OptionsClass();
                TATemp.DN_WidthTA = TA.Width_DN;
                TATemp.Frequency = TA.Frequency;
                TATemp.GammaTA = TA.Reflection;
                TATemp.delta_MTA = TA.delta_Axial_Ratio;
                TATemp.GainTA = TA.Gain_dB;
                TATemp.delta_GainTA = TA.delta_Gain;
                TATemp.MTA = TA.Axial_Ratio;

                anten.TA_OptionsList.Add(TATemp);
            }

            #endregion

            return anten;
        }

        AntennOptionsClass MainRowToAntennClassIA(DataSet1.mainRow tempMain)
        {
            AntennOptionsClass retAnten = new AntennOptionsClass();
            if (tempMain.AntennasRow != null)
            {
                retAnten = AntennRowToClass(tempMain.AntennasRow);
            }
            return retAnten;
        }

        AntennOptionsClass MainRowToAntennClassTA(DataSet1.mainRow tempMain)
        {
            AntennOptionsClass retAnten = new AntennOptionsClass();
            if (tempMain.AntennasRowBymain_AntennasTA != null)
            {
                retAnten = AntennRowToClass(tempMain.AntennasRowBymain_AntennasTA);
            }
            return retAnten;
        }

        #endregion

        #endregion

        #region Вспомогательные функции

        /// <summary>
        /// костыль для работы со старыми и новыми таблицами данных
        /// </summary>
        /// <param name="TableName">Имя таблицы для загрузки</param>
        /// <param name="freqID"></param>
        /// <param name="CommandFist"></param>
        /// <param name="CommandSecond"></param>
        protected void Check_Old_or_New_Table(string TableName, int freqID, ref string CommandFist, ref string CommandSecond)
        {
            string KoS = "Measurement_" + freqID.ToString();

            if (KoS == TableName)
            {
                //старый способ
                string temp = CommandFist;
                CommandFist = CommandSecond;
                CommandSecond = temp;
            }
        }

        #endregion

        #region рассылка новых данных (антенн, разъёмов, устройств)

        /// <summary>
        /// событие добавление новой антенны
        /// </summary>
        public event AddNewAntennDelegate AddNewAntennEvent;
        /// <summary>
        /// разослать новые антенны всем кроме кого то
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Obj"></param>
        /// <param name="NoSend"></param>
        protected void SendNewAntenn(object Sender, AntennOptionsClass Obj, object NoSend)
        {
            if (AddNewAntennEvent != null)
            {
                Delegate[] inlist = AddNewAntennEvent.GetInvocationList();

                foreach (AddNewAntennDelegate Event in inlist)
                {
                    if (Event.Target.GetHashCode() != NoSend.GetHashCode())
                    {
                        Event(Sender, Obj);
                    }
                }
            }
        }


        #endregion


        #region NEW Loaders

        /// <summary>
        /// Разобрать пакет для загрузки на составляющие и произвести проверки
        /// </summary>
        /// <param name="Pack"></param>
        /// <param name="mainRow"></param>
        /// <param name="freqRow"></param>
        /// <returns>True - успешная загрузка</returns>
        protected bool UnpackLoadData(DBLoaderPackClass Pack, out DataSet1.mainRow mainRow, out DataSet1.FrequenciesRow freqRow, out  List<DataSet1.AntennasRow> anten)
        {
            bool ret = false;
            mainRow = null;
            freqRow = null;
            anten = new List<DataSet1.AntennasRow>();
            string ErrorText = "";

            if (Pack.ObjectForLoad is DataSet1.mainRow)
            {
                mainRow = Pack.ObjectForLoad as DataSet1.mainRow;
                anten.Add(mainRow.AntennasRow);
            }
            else
            {
                if (Pack.ObjectForLoad is DataSet1.FrequenciesRow)
                {
                    freqRow = Pack.ObjectForLoad as DataSet1.FrequenciesRow;
                    mainRow = GetMainRowByFrequencyRow(freqRow);
                    anten.Add(mainRow.AntennasRow);
                }
                else
                {
                    if (Pack.ObjectForLoad is DataSet1.AntennasRow)
                    {
                        anten.Add(Pack.ObjectForLoad as DataSet1.AntennasRow);
                    }
                    else
                    {
                        if (Pack.ObjectForLoad is List<DataSet1.AntennasRow>)
                        {
                            anten = Pack.ObjectForLoad as List<DataSet1.AntennasRow>;
                        }
                        else
                        {
                            ErrorText = "Объект для загрузки не является строкой в БД";
                        }
                    }
                }
            }

            if (ErrorText != "")
            {
                throw new Exception("Ошибка загрузки " + ErrorText);
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        public ResultType_MAINClass LoadResult(DBLoaderPackClass Pack, bool LoadFullFrquencyResult, bool LoadResultData = true)
        {
            ResultType_MAINClass ret = null;

            DataSet1.mainRow mainRow = null;
            DataSet1.FrequenciesRow freqRow = null;
            List<DataSet1.AntennasRow> anten = null;

            if (UnpackLoadData(Pack, out mainRow, out freqRow, out anten))
            {
                //производим загрузку
                if (LoadFullFrquencyResult)
                {
                    ret = this.GetResultMainByFrequenciesRow(mainRow, null, false, LoadResultData);
                }
                else
                {
                    ret = this.GetResultMainByFrequenciesRow(mainRow, freqRow, false, LoadResultData);
                }
            }

            return ret;
        }

        public List<ResultType_MAINClass> LoadALLResult(DBLoaderPackClass Pack)
        {
            List<ResultType_MAINClass> ret = null;
            DataSet1.mainRow mainRow = null;
            DataSet1.FrequenciesRow freqRow = null;
            List<DataSet1.AntennasRow> anten = null;

            if (UnpackLoadData(Pack, out mainRow, out freqRow, out anten))
            {
                ret = this.LoadAllResultsByAnten(anten);
            }

            return ret;
        }

        public MainOptionsClass LoadOnlyOptions(DBLoaderPackClass Pack)
        {
            MainOptionsClass ret = null;

            if (Pack.ObjectForLoad is DataSet1.mainRow)
            {
                DataSet1.mainRow mainRow = Pack.ObjectForLoad as DataSet1.mainRow;
                ret = this.GetMainOptionsClass(mainRow);
            }
            else
            {
                throw new Exception("Объект для загрузки не является строкой Main в БД");
            }

            return ret;
        }

        public AntennOptionsClass LoadIA(DBLoaderPackClass Pack)
        {
            AntennOptionsClass ret = null;

            if (Pack.ObjectForLoad is DataSet1.mainRow)
            {
                DataSet1.mainRow mainRow = Pack.ObjectForLoad as DataSet1.mainRow;
                ret = MainRowToAntennClassIA(mainRow);
            }
            else if (Pack.ObjectForLoad is DataSet1.AntennasRow)
            {
                ret = AntennRowToClass(Pack.ObjectForLoad as DataSet1.AntennasRow);
            }
            else
            {
                throw new Exception("Объект для загрузки не является строкой Main в БД");
            }

            return ret;
        }

        public AntennOptionsClass LoadTA(DBLoaderPackClass Pack)
        {
            AntennOptionsClass ret = null;

            if (Pack.ObjectForLoad is DataSet1.mainRow)
            {
                DataSet1.mainRow mainRow = Pack.ObjectForLoad as DataSet1.mainRow;
                ret = MainRowToAntennClassTA(mainRow);
            }
            else if (Pack.ObjectForLoad is DataSet1.AntennasRow)
            {
                ret = AntennRowToClass(Pack.ObjectForLoad as DataSet1.AntennasRow);
            }
            else
            {
                throw new Exception("Объект для загрузки не является строкой Main в БД");
            }

            return ret;
        }

        public List<ResultType_MAINClass> LoadALLResult()
        {
            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            this.TestLoadAllResult(out ret);

            return ret;
        }

        public int GetMainID(DBLoaderPackClass Pack)
        {
            int ret = -1;

            DataSet1.mainRow mainRow = null;
            DataSet1.FrequenciesRow freqRow = null;
            List<DataSet1.AntennasRow> anten = null;

            if (UnpackLoadData(Pack, out mainRow, out freqRow, out anten))
            {
                ret = mainRow.id;
            }

            return ret;
        }
        #endregion

        #region Для обработки дубликатов в базе и сокрытие частот

        public static string GetSplitKey(string Text)
        {
            string KEYName = Text.ToUpper();
            string[] AntNameMas = KEYName.Split(" -/\"\'\\;,.".ToCharArray());

            KEYName = "";
            foreach (string temp in AntNameMas)
            {
                KEYName += temp;
            }

            return KEYName;
        }

        public class DublicateAntennasClass
        {
            public AntennOptionsClass MainAnten = null;
            public List<AntennOptionsClass> Dublicates = new List<AntennOptionsClass>();
        }

        public List<DublicateAntennasClass> CheckAntens(List<AntennOptionsClass> In)
        {
            List<DublicateAntennasClass> ret = new List<DublicateAntennasClass>();

            List<AntennOptionsClass> InDB = new List<AntennOptionsClass>(In);

            for (int i = 0; i < InDB.Count; i++)
            {
                DublicateAntennasClass dubl = new DublicateAntennasClass();
                dubl.MainAnten = InDB[i];

                string ant_1_KEYName = DB_LoaderClass.GetSplitKey(InDB[i].Name);
                string ant_1_KEYZAV = DB_LoaderClass.GetSplitKey(InDB[i].ZAVNumber);

                for (int j = i; j < InDB.Count; j++)
                {
                    string ant_2_KEYName = DB_LoaderClass.GetSplitKey(InDB[j].Name);
                    string ant_2_KEYZAV = DB_LoaderClass.GetSplitKey(InDB[j].ZAVNumber);

                    if (ant_1_KEYName == ant_2_KEYName)
                    {
                        if (ant_1_KEYZAV == ant_2_KEYZAV)
                        {
                            if (InDB[i].Description == InDB[j].Description)
                            {
                                if (InDB[i].id != InDB[j].id)
                                {
                                    dubl.Dublicates.Add(InDB[j]);

                                    InDB.RemoveAt(j);
                                    j--;
                                }
                            }
                        }
                    }
                }

                if (dubl.Dublicates.Count != 0)
                {
                    for (int t = 0; t < dubl.Dublicates.Count; t++)
                    {
                        if (dubl.Dublicates[t].UsingAsZond)
                        {
                            if (!dubl.MainAnten.UsingAsZond)
                            {
                                AntennOptionsClass temp = dubl.Dublicates[t];
                                dubl.Dublicates.RemoveAt(t);
                                dubl.Dublicates.Add(dubl.MainAnten);

                                dubl.MainAnten = temp;
                            }
                            else
                            {
                                //дубликат зонда

                            }
                        }
                    }

                    ret.Add(dubl);
                }
            }

            return ret;
        }

        /// <summary>
        /// удаление дубликатов антенн и востановление связей с реальными антенами
        /// </summary>
        /// <param name="DublicatesList"></param>
        public void DeleteAntennDuplicates(List<DublicateAntennasClass> DublicatesList)
        {
            #region певодим связи на реальные антены

            string ret = string.Format("Обновлено измерение\n");

            foreach (DataSet1.mainRow main in this.Sheild_M_DataBase.main)
            {
                foreach (DublicateAntennasClass Dublicate in DublicatesList)
                {
                    foreach (AntennOptionsClass DublAntenn in Dublicate.Dublicates)
                    {
                        if (main.id_Tech_Antenn == DublAntenn.id)
                        {
                            main.id_Tech_Antenn = Dublicate.MainAnten.id;
                            ret += string.Format("{0} - Tech_Antenn\n", main.id);
                        }

                        if (main.id_Antenn == DublAntenn.id)
                        {
                            main.id_Antenn = Dublicate.MainAnten.id;
                            ret += string.Format("{0} - Antenn\n", main.id);
                        }
                    }
                }
            }

            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterMAIN.Update(Sheild_M_DataBase.main);
            #endregion


            foreach (DublicateAntennasClass Dublicate in DublicatesList)
            {
                foreach (AntennOptionsClass DublAntenn in Dublicate.Dublicates)
                {
                    DataSet1.AntennasRow delAnten = Sheild_M_DataBase.Antennas.FindByid(DublAntenn.id);
                    delAnten.Delete();
                }
            }

            DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);
        }

        public void HideFrequency(DBLoaderPackClass Pack)
        {
            ResultType_MAINClass result = LoadResult(Pack, true);

            string retText = result.ToString();

            foreach (PolarizationElementClass pol in result.PolarizationElements)
            {
                if (pol.Polarization != SelectedPolarizationEnum.Sum)
                {
                    retText += string.Format("\nПоляризация {0} \n", pol.Polarization);

                    for (int i = 0; i < pol.FrequencyElements.Count; i++)
                    {
                        if (pol.FrequencyElements[i].id >= 0)
                        {
                            DataSet1.FrequenciesRow freqRow = Sheild_M_DataBase.Frequencies.FindByid(pol.FrequencyElements[i].id);

                            if (freqRow != null)
                            {
                                retText += string.Format("{0} - №{1}\n", pol.FrequencyElements[i], i);

                                freqRow.Freq_MHz = i;
                            }
                        }
                    }
                }
            }

            retText += string.Format("\n\nСкрыть Частоты?");
            if (MessageBox.Show(retText, "Скрытие частот", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataAdapterFrequencies.Update(Sheild_M_DataBase.Frequencies);

                DataSet1.mainRow mainRow = Sheild_M_DataBase.main.FindByid(result.id);

                DataSet1.Segment_TableRow[] segments = mainRow.GetSegment_TableRows();

                foreach (DataSet1.Segment_TableRow segm in segments)
                {
                    segm.Delete();
                }

                DataAdapterSegment_Table.Update(Sheild_M_DataBase.Segment_Table);

                MessageBox.Show("Перезапустите программу для полного сокрытия", "Скрытие частот", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DeleteNullAntenns()
        {
            List<DataSet1.AntennasRow> antens = new List<DataSet1.AntennasRow>();
            string Texts = "";
            bool IsFind = false;
            int i = 0;

            foreach (DataSet1.AntennasRow ant in Sheild_M_DataBase.Antennas)
            {
                IsFind = false;

                foreach (DataSet1.mainRow main in Sheild_M_DataBase.main)
                {
                    if (ant.Using_as_TA || main.id_Antenn == ant.id || main.id_Tech_Antenn == ant.id)
                    {
                        IsFind = true;
                        break;
                    }
                }

                if (!IsFind)
                {
                    //пустая антенна
                    antens.Add(ant);
                    Texts = string.Format("{0}{1} зав. {2}", Texts, ant.Name.TrimEnd(), ant.Serial_Number.TrimEnd());

                    i++;
                    if (i == 5)
                    {
                        i = 0;
                        Texts = string.Format("{0}\n", Texts);
                    }
                    else
                    {
                        Texts = string.Format("{0}\t\t\t", Texts);
                    }
                }
            }

            if (MessageBox.Show(string.Format("Найдены антенны без измерений: \n{0}\nУдалить?", Texts), "Поиск завершён", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataSet1.AntennasRow ant in antens)
                {
                    ant.Delete();
                }

                DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);
            }
        }

        #endregion

        #region Сервисные функции

        public void ReformatMeasurements()
        {
            foreach (DataSet1.mainRow mainRow in this.Sheild_M_DataBase.main)
            {
                try
                {

                    Application.DoEvents();
                    System.Diagnostics.Trace.TraceInformation("Измерение " + mainRow.id.ToString());

                    List<DataSet1.FrequenciesRow> freqList = GetAllFrequenciesRow(mainRow, null);
                    bool CreateResultTable = true;
                    string TableName = "Measurement_" + mainRow.id.ToString();

                    foreach (DataSet1.FrequenciesRow freq in freqList)
                    {
                        if (freq.Result_Table_Name.TrimEnd() != TableName)
                        {
                            System.Diagnostics.Trace.TraceInformation("Измерение " + mainRow.id.ToString() + "; частота " + freq.Freq_MHz.ToString());

                            //загружаем результат
                            FrequencyElementClass SaveObj = GetFrequencyElementByFrequenciesRow(freq);

                            if (CreateResultTable)
                            {
                                this.CreateMeasurementTable(TableName);
                                CreateResultTable = false;
                            }

                            this.SaveResultElementS(SaveObj.ResultAmpl_PhaseElements, TableName, freq.id);

                            //удаляем старую таблицу
                            this.DeleteMeasurementTable(freq.Result_Table_Name);

                            //записываем новую таблицу
                            freq.Result_Table_Name = TableName;
                            
                        }
                        else
                        {
                            System.Diagnostics.Trace.TraceInformation("Измерение " + mainRow.id.ToString() + " - уже новое");
                            break;
                        }
                    }

                    //обновляем таблицу поссле каждого main
                    DataAdapterFrequencies.Update(Sheild_M_DataBase.Frequencies);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Измерение " + mainRow.id.ToString() + " Ошибка, нужна ручная проверка" + ex.Message);

                    MessageBox.Show("Измерение " + mainRow.id.ToString() + " Ошибка, нужна ручная проверка" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            //обновляем таблицу
            DataAdapterFrequencies.Fill(Sheild_M_DataBase.Frequencies);
        }

        public string TestLoadAllResult(out List<ResultType_MAINClass> results)
        {
            string ret = "";
            results = new List<ResultType_MAINClass>();

            foreach (DataSet1.mainRow mainRow in this.Sheild_M_DataBase.main)
            {
                try
                {
                    ResultType_MAINClass result = GetResultMainByFrequenciesRow(mainRow, null, false);
                    results.Add(result);
                    System.Diagnostics.Trace.TraceInformation(string.Format("Измерение {0} - OK", mainRow.id));
                }
                catch
                {
                    string temp = string.Format("Ошибка в измерении {0}\n", mainRow.id);
                    ret += temp;
                    System.Diagnostics.Trace.TraceError(temp);
                }
            }

            return ret;
        }

        public bool FindInResultByID(int id)
        {
            bool ret = false;

            DataSet1.mainRow temp = this.Sheild_M_DataBase.main.FindByid(id);

            if (temp != null)
            {
                ret = true;
            }

            return ret;
        }

        #endregion

    }
}
