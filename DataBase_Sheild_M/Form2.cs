using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using DataBase_Sheild_M.OptionsClass.ResultClass;
using DataBase_Sheild_M.OptionsClass;

namespace DataBase_Sheild_M
{
    public partial class Form2 : Form, DataBase_Sheild_M.ISaver_ToDataBase
    {
        public Form2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Соединение с базой данных Sheild_M_measurement
        /// </summary>
        SqlConnection Sheild_M_measurement_Connection;

        /// <summary>
        /// Соединение с базой данных Sheild_M_DataBase
        /// </summary>
        SqlConnection Sheild_M_DataBase_Connection;

        #region Адаптеры подключения к БД
        SqlDataAdapter DataAdapterMAIN;
        SqlDataAdapter DataAdapterAntennas;
        SqlDataAdapter DataAdapterConnectors_type;
        SqlDataAdapter DataAdapterOperators;
        SqlDataAdapter DataAdapterMeasurement_PositionParameters;
        SqlDataAdapter DataAdapterMeasurement_ZVBParameters;
        SqlDataAdapter DataAdapterResultTypes;
        SqlDataAdapter DataAdapterСписокРезультатов;
        SqlDataAdapter DataAdapterЧастоты;
        SqlDataAdapter DataAdapterZondConnectionDevices;
        SqlDataAdapter DataAdapterAntennConnectionDevices;
        SqlDataAdapter DataAdapterALLConnectionDevices;
        SqlDataAdapter DataAdapterSegment_Table;
        #endregion

        public void Form2_Load(object sender, EventArgs e)
        {
            AddOwnedForm(HeandBand);
            HeandBand.Show();
            Application.DoEvents();

            Sheild_M_measurement_Connection = new SqlConnection("Data Source=ARM;Initial Catalog=Sheild_M_measurement;Integrated Security=True");
            Sheild_M_DataBase_Connection = new SqlConnection("Data Source=ARM;Initial Catalog=MAIN_Sheild_M;Integrated Security=True");

            #region Инициализация Адаптеров подключения к БД
            DataAdapterMAIN = new SqlDataAdapter("SELECT * FROM main", this.Sheild_M_DataBase_Connection);
            DataAdapterAntennas = new SqlDataAdapter("select * from Antennas", this.Sheild_M_DataBase_Connection);
            DataAdapterConnectors_type = new SqlDataAdapter("select * from Connectors_type", this.Sheild_M_DataBase_Connection);
            DataAdapterOperators = new SqlDataAdapter("select * from Operators", this.Sheild_M_DataBase_Connection);
            DataAdapterMeasurement_PositionParameters = new SqlDataAdapter("select * from Measurement_PositionParameters", this.Sheild_M_DataBase_Connection);
            DataAdapterMeasurement_ZVBParameters = new SqlDataAdapter("select * from Measurement_ZVBParameters", this.Sheild_M_DataBase_Connection);
            DataAdapterResultTypes = new SqlDataAdapter("select * from ResultTypes", this.Sheild_M_DataBase_Connection);
            DataAdapterСписокРезультатов = new SqlDataAdapter("select * from [Список результатов]", this.Sheild_M_DataBase_Connection);
            DataAdapterЧастоты = new SqlDataAdapter("select * from [Частоты]", this.Sheild_M_DataBase_Connection);
            DataAdapterZondConnectionDevices = new SqlDataAdapter("select * from [Zond_Connection_devices]", this.Sheild_M_DataBase_Connection);
            DataAdapterAntennConnectionDevices = new SqlDataAdapter("select * from [Antenn_Connection_devices]", this.Sheild_M_DataBase_Connection);
            DataAdapterALLConnectionDevices = new SqlDataAdapter("select * from [Connection_devices]", this.Sheild_M_DataBase_Connection);
            DataAdapterSegment_Table = new SqlDataAdapter("select * from [Segment_Table]", this.Sheild_M_DataBase_Connection);
            #endregion

            #region для автоматического создания команд добавлений, обновлений и тд
            SqlCommandBuilder BilderAntennas = new SqlCommandBuilder(DataAdapterAntennas);
            SqlCommandBuilder BilderConnectors_type = new SqlCommandBuilder(DataAdapterConnectors_type);
            SqlCommandBuilder BilderALLConnection_devices = new SqlCommandBuilder(DataAdapterALLConnectionDevices);
            SqlCommandBuilder BilderFrequency = new SqlCommandBuilder(DataAdapterЧастоты);
            SqlCommandBuilder BilderPolarization = new SqlCommandBuilder(DataAdapterСписокРезультатов);
            SqlCommandBuilder BilderPositionParameters = new SqlCommandBuilder(DataAdapterMeasurement_PositionParameters);
            SqlCommandBuilder BilderZVBParameters = new SqlCommandBuilder(DataAdapterMeasurement_ZVBParameters);
            SqlCommandBuilder BilderSegment_Table = new SqlCommandBuilder(DataAdapterSegment_Table);
            SqlCommandBuilder BilderMAIN = new SqlCommandBuilder(DataAdapterMAIN);
            #endregion

            #region загрузка всех таблиц из базы MAIN
            DataAdapterMAIN.Fill(this.Sheild_M_DataBase.main);
            DataAdapterAntennas.Fill(this.Sheild_M_DataBase.Antennas);
            DataAdapterConnectors_type.Fill(this.Sheild_M_DataBase.Connectors_type);
            DataAdapterOperators.Fill(this.Sheild_M_DataBase.Operators);
            DataAdapterMeasurement_PositionParameters.Fill(this.Sheild_M_DataBase.Measurement_PositionParameters);
            DataAdapterMeasurement_ZVBParameters.Fill(this.Sheild_M_DataBase.Measurement_ZVBParameters);
            DataAdapterResultTypes.Fill(this.Sheild_M_DataBase.ResultTypes);
            DataAdapterЧастоты.Fill(this.Sheild_M_DataBase.Частоты);
            DataAdapterZondConnectionDevices.Fill(this.Sheild_M_DataBase.Zond_Connection_devices);
            DataAdapterAntennConnectionDevices.Fill(this.Sheild_M_DataBase.Antenn_Connection_devices);
            DataAdapterALLConnectionDevices.Fill(this.Sheild_M_DataBase.Connection_devices);
            DataAdapterSegment_Table.Fill(this.Sheild_M_DataBase.Segment_Table);
            DataAdapterСписокРезультатов.Fill(this.Sheild_M_DataBase.Список_результатов);
            #endregion

            #region заполнить контролы инфой из базы
            this.antennOptionsUserControl1.Saver_ToDataBase = this;
            this.connection_Devices_UserControl1.Saver_ToDataBase = this;
            
            CreateTreeView();
            #endregion

            #region подпись на события сохранения и выбора нодов
            //подписываемся на события выбора нодов
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            ////подписываемся на события сохранения антенны
            //this.antennOptionsUserControl1.NeedToSaveAntenEvent += new AntennOptionsUserControl.NeedToSaveAntenDelegate(Save_Antenn);
            ////подписываемся на события сохранения конекторов антенны
            //this.antennOptionsUserControl1.connectorsUserControl1.NeedToSaveConnectorEvent += new ConnectorsUserControl.NeedToSaveConnectorDelegate(Save_Connector);

            ////подписываемся на события сохранения устройств
            //this.connection_Devices_UserControl1.NeedToSaveAntenEvent += new Connection_Devices_UserControl.NeedToSaveDeviceDelegate(Save_Connection_Devices);

            ////подписываемся на события сохранения конекторов из контрола устройств
            //this.connection_Devices_UserControl1.connectorsUserControl1.NeedToSaveConnectorEvent += new ConnectorsUserControl.NeedToSaveConnectorDelegate(Save_Connector);
            //this.connection_Devices_UserControl1.connectorsUserControl2.NeedToSaveConnectorEvent += new ConnectorsUserControl.NeedToSaveConnectorDelegate(Save_Connector);
            //this.connection_Devices_UserControl1.connectorsUserControl3.NeedToSaveConnectorEvent += new ConnectorsUserControl.NeedToSaveConnectorDelegate(Save_Connector);
            //this.connection_Devices_UserControl1.connectorsUserControl4.NeedToSaveConnectorEvent += new ConnectorsUserControl.NeedToSaveConnectorDelegate(Save_Connector);

            #endregion

            this.checkBox1_CheckedChanged(this, new EventArgs());

            HeandBand.Hide();
        }

        private HandBandForm HeandBand = new HandBandForm();

        #region Функции загрузки данных в контроллы
        /// <summary>
        /// загрузить все конекторы в контролы конекторов для последующего отображения
        /// </summary>
        private void LoadAllConnectorToControl()
        {
            connection_Devices_UserControl1.ClearConnectors();
            antennOptionsUserControl1.connectorsUserControl1.ClearConnectorList();

            List<ConnectorsTypeClass> loadConnector = this.LoadAllConnector();

            this.connection_Devices_UserControl1.AddConnectors(loadConnector);
            this.antennOptionsUserControl1.connectorsUserControl1.AddConnectors(loadConnector);
        }

        protected List<ConnectorsTypeClass> loadConnector = null;
        /// <summary>
        /// загрузить все конекторы
        /// </summary>
        public List<ConnectorsTypeClass> LoadAllConnector()
        {
            if (loadConnector == null)
            {
                loadConnector = new List<ConnectorsTypeClass>();
                foreach (DataSet1.Connectors_typeRow tempConROW in Sheild_M_DataBase.Connectors_type)
                {
                    loadConnector.Add(ConnectorTypeRowToClass(tempConROW));
                }
            }

            return loadConnector;
        }

        /// <summary>
        /// загрузить все антенны в контролы для последующего отображения
        /// </summary>
        private void LoadAllAntennToControl()
        {
            this.antennOptionsUserControl1.ClearList();
            this.antennOptionsUserControl1.AddAntenn(this.LoadAllAntenn());
        }

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

        /// <summary>
        /// загрузить все устройства в контролы для последующего отображения
        /// </summary>
        private void LoadAllConnectedDeviceToControl()
        {
            this.connection_Devices_UserControl1.AddDevice(this.LoadAllConnectedDevice());
        }

        protected List<Connection_devicesClass> loadDevice = null;

        /// <summary>
        /// загрузить все устройства
        /// </summary>
        public List<Connection_devicesClass> LoadAllConnectedDevice()
        {
            if (loadDevice == null)
            {
                loadDevice = new List<Connection_devicesClass>();
                foreach (DataSet1.Connection_devicesRow tempdevise in Sheild_M_DataBase.Connection_devices)
                {
                    loadDevice.Add(this.ConnectionDeviceRowToClass(tempdevise));
                }
            }

            return loadDevice;
        }
        #endregion

        void CreateTreeView()
        {
            //флаг использования нода для неизвестных типов измерений
            bool NonameNodeIsUsing = false;

            foreach (DataSet1.ResultTypesRow resultRow in this.Sheild_M_DataBase.ResultTypes)
            {
                this.treeView1.Nodes.Add(resultRow.id.ToString(), resultRow.Наименование.TrimEnd(" ".ToCharArray()));
                this.treeView1.Nodes[resultRow.id.ToString()].Tag = resultRow;
                if (!resultRow.IsОписаниеNull())
                {
                    this.treeView1.Nodes[resultRow.id.ToString()].ToolTipText = resultRow.Описание.TrimEnd(" ".ToCharArray());
                }

                //для КУ заблокировать полярный график
                if (resultRow.id == (int)MeasurementTypeEnum.КУ)
                {
                    this.WathFormDictionary.Add(resultRow.id, new CompareGraphForm(resultRow.Наименование.TrimEnd(" ".ToCharArray()), true));
                }
                else
                {
                    this.WathFormDictionary.Add(resultRow.id, new CompareGraphForm(resultRow.Наименование.TrimEnd(" ".ToCharArray()), false));
                }
            }

            foreach (DataSet1.mainRow mainRow in this.Sheild_M_DataBase.main)
            {
                string tempName = "";
                if (!mainRow.IsНаименованиеNull())
                {
                    tempName = " | " + mainRow.Наименование.TrimEnd(" ".ToCharArray());
                }

                TreeNode tempMain = new TreeNode(mainRow.Дата_формирования.ToShortDateString() + tempName);
                tempMain.Tag = mainRow;

                #region Получаем связанные таблицы антен и зондов а так же допонительные девайсы
                DataSet1.AntennasRow[] antenaRows = mainRow.GetAntennasRowsBymain_Antennas();
                DataSet1.AntennasRow[] zondRows = mainRow.GetAntennasRowsBymain_Zond();

                DataSet1.Zond_Connection_devicesRow[] zondConDev = mainRow.GetZond_Connection_devicesRows();
                DataSet1.Antenn_Connection_devicesRow[] antenConDev = mainRow.GetAntenn_Connection_devicesRows();

                TreeNode tempAntena = new TreeNode("Антена: null ");
                TreeNode tempZond = new TreeNode("Зонд: null ");
                //если есть связанные антены и зонды то долбавляем в treeview
                if (antenaRows.Length != 0)
                {
                    tempAntena = new TreeNode("Антена: " + antenaRows[0].Наименование.TrimEnd(" ".ToCharArray()));
                    tempAntena.Tag = antenaRows[0];

                    foreach (DataSet1.Antenn_Connection_devicesRow tempantenCon in antenConDev)
                    {
                        DataSet1.Connection_devicesRow[] tempConnectionDevices = tempantenCon.GetConnection_devicesRows();
                        if (tempConnectionDevices.Length != 0)
                        {
                            TreeNode tempantenconNode = new TreeNode(tempConnectionDevices[0].name.TrimEnd(" ".ToCharArray()));
                            tempantenconNode.Tag = tempConnectionDevices[0];

                            tempAntena.Nodes.Add(tempantenconNode);
                        }
                    }
                }
                if (zondRows.Length != 0)
                {
                    tempZond = new TreeNode("Зонд: " + zondRows[0].Наименование.TrimEnd(" ".ToCharArray()));
                    tempZond.Tag = zondRows[0];

                    foreach (DataSet1.Zond_Connection_devicesRow tempZondCon in zondConDev)
                    {
                        DataSet1.Connection_devicesRow[] tempConnectionDevices = tempZondCon.GetConnection_devicesRows();
                        if (tempConnectionDevices.Length != 0)
                        {
                            TreeNode tempZondconNode = new TreeNode(tempConnectionDevices[0].name.TrimEnd(" ".ToCharArray()));
                            tempZondconNode.Tag = tempConnectionDevices[0];

                            tempZond.Nodes.Add(tempZondconNode);
                        }
                    }
                }
                #endregion

                #region Получаем связанные таблицы результатов
                TreeNode SpisokResultatovNode = new TreeNode("Результат");

                bool needAddResultNode = false;
                //выбираем из какой таблицы считывать результаты в соответствии с типом измерения.
                switch (mainRow.id_Типа)
                {
                    case (int)MeasurementTypeEnum.КУ:

                            SpisokResultatovNode = new TreeNode("Результат КУ (Sum)");

                        #region построение структуры КУ (main, cross, sum)
                        DataSet1.Список_результатовRow[] PolarizationResult0 = mainRow.GetСписок_результатовRows();
                        int insertion = 0;
                        List<object> polarizationList = new List<object>();
                        if (PolarizationResult0.Length!=0)
                        {
                            needAddResultNode = true;
                            DataSet1.Список_результатовRow tempPolarization = PolarizationResult0[0];

                            DataSet1.ЧастотыRow[] FrequencyResult = tempPolarization.GetЧастотыRows();
                            foreach(DataSet1.ЧастотыRow FrequencyResult0 in FrequencyResult)
                            {
                                TreeNode PolarizationNode = new TreeNode("Поляризация");
                                //!!!таблица частоты main должна быть всегда первой (для этого её надо измерять первой и её id должен быть меньше)
                                if (insertion == 0)
                                {
                                    List<object> polarizationList1 = new List<object>();
                                    polarizationList1.Add(KYTupeEnum.main);
                                    polarizationList1.Add(FrequencyResult0);
                                    PolarizationNode.Tag = polarizationList1;
                                    PolarizationNode.Text += " " + FrequencyResult0.Частота.ToString() + " (main)";


                                    polarizationList.Add(KYTupeEnum.Sum);
                                    polarizationList.Add(FrequencyResult0);
                                    SpisokResultatovNode.Tag = polarizationList;
                                }
                                if (insertion == 1)
                                {
                                    List<object> polarizationList1 = new List<object>();
                                    polarizationList1.Add(KYTupeEnum.Cross);
                                    polarizationList1.Add(FrequencyResult0);
                                    PolarizationNode.Tag = polarizationList1;
                                    PolarizationNode.Text += " " + FrequencyResult0.Частота.ToString() + " (cros)";
                                }
                                if (insertion > 1)
                                {
                                    break;
                                }

                                insertion++;
                                SpisokResultatovNode.Nodes.Add(PolarizationNode);
                            }

                        }
                        #endregion
                        break;

                    case (int)MeasurementTypeEnum.ПХ:
                            SpisokResultatovNode = new TreeNode("Результат ПХ");
                            //SpisokResultatovNode.Tag = tempResult1[0];
                        DataSet1.Список_результатовRow[] PolarizationResult1 = mainRow.GetСписок_результатовRows();
                        if (PolarizationResult1.Length != 0)
                        {
                            needAddResultNode = true;
                            DataSet1.ЧастотыRow[] FrequencyResult = PolarizationResult1[0].GetЧастотыRows();
                            SpisokResultatovNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                        }

                        break;
                    case (int)MeasurementTypeEnum.АДН_ФДН_Азимут:
                            SpisokResultatovNode = new TreeNode("Результат АДН и ФДН (Азимут)");
                            //SpisokResultatovNode.Tag = tempResult2[0];
                        DataSet1.Список_результатовRow[] PolarizationResult2 = mainRow.GetСписок_результатовRows();
                        if (PolarizationResult2.Length != 0)
                        {
                            needAddResultNode = true;
                            DataSet1.ЧастотыRow[] FrequencyResult = PolarizationResult2[0].GetЧастотыRows();
                            SpisokResultatovNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                        }

                        break;
                    case (int)MeasurementTypeEnum.АДН_ФДН_Поляриз:
                            SpisokResultatovNode = new TreeNode("Результат АДН и ФДН (Поляриз)");
                            //SpisokResultatovNode.Tag = tempResult3[0];
                        DataSet1.Список_результатовRow[] PolarizationResult3 = mainRow.GetСписок_результатовRows();
                        if (PolarizationResult3.Length != 0)
                        {
                            needAddResultNode = true;
                            DataSet1.ЧастотыRow[] FrequencyResult = PolarizationResult3[0].GetЧастотыRows();
                            SpisokResultatovNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                        }

                        break;
                    case (int)MeasurementTypeEnum.СДНМ_Азимут:
                            SpisokResultatovNode = new TreeNode("Результат СДНМ");
                            //SpisokResultatovNode.Tag = tempResult4[0];
                        DataSet1.Список_результатовRow[] PolarizationResult4 = mainRow.GetСписок_результатовRows();
                        foreach (DataSet1.Список_результатовRow tempPolarization in PolarizationResult4)
                        {
                            needAddResultNode = true;
                            TreeNode PolarizationNode = new TreeNode("Поляризация");
                            PolarizationNode.Tag = tempPolarization;

                            if (!tempPolarization.IsПоляризацияNull())
                            {
                                PolarizationNode = new TreeNode("Поляризация " + tempPolarization.Поляризация.ToString());
                            }

                            DataSet1.ЧастотыRow[] FrequencyResult = tempPolarization.GetЧастотыRows();
                            PolarizationNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                            SpisokResultatovNode.Nodes.Add(PolarizationNode);
                        }

                        break;
                    case (int)MeasurementTypeEnum.СДНМ_Поляриз:
                        SpisokResultatovNode = new TreeNode("Результат СДНМ");
                        //SpisokResultatovNode.Tag = tempResult4[0];
                        DataSet1.Список_результатовRow[] PolarizationResult5 = mainRow.GetСписок_результатовRows();
                        foreach (DataSet1.Список_результатовRow tempPolarization in PolarizationResult5)
                        {
                            needAddResultNode = true;
                            TreeNode PolarizationNode = new TreeNode("Поляризация");
                            PolarizationNode.Tag = tempPolarization;

                            if (!tempPolarization.IsПоляризацияNull())
                            {
                                PolarizationNode = new TreeNode("Поляризация " + tempPolarization.Поляризация.ToString());
                            }

                            DataSet1.ЧастотыRow[] FrequencyResult = tempPolarization.GetЧастотыRows();
                            PolarizationNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                            SpisokResultatovNode.Nodes.Add(PolarizationNode);
                        }

                        break;
                    default:
                        DataSet1.Список_результатовRow[] PolarizationResultdefault = mainRow.GetСписок_результатовRows();
                        if (PolarizationResultdefault.Length != 0)
                        {
                            needAddResultNode = true;
                            DataSet1.ЧастотыRow[] FrequencyResult = PolarizationResultdefault[0].GetЧастотыRows();
                            SpisokResultatovNode.Nodes.AddRange(this.GetFrequencyNodes(FrequencyResult));
                        }
                        break;
                }

                #endregion

                if (needAddResultNode)
                {
                    tempMain.Nodes.Add(SpisokResultatovNode);
                }
                tempMain.Nodes.Add(tempAntena);
                tempMain.Nodes.Add(tempZond);
                try
                {
                    this.treeView1.Nodes[mainRow.id_Типа.ToString()].Nodes.Add(tempMain);
                }
                //сильно замедляется построение нодов при появлении незарегистрированных в БД (в таблице ResultTypes) типов измерений
                catch (NullReferenceException)
                {
                    if (!NonameNodeIsUsing)
                    {
                        this.treeView1.Nodes.Add("NoType", "Неизвестный тип измерения");
                        NonameNodeIsUsing = true;
                    }
                    this.treeView1.Nodes["NoType"].Nodes.Add(tempMain);
                }
            }
        }

        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Factory(e.Node.Tag);
        }

        #region Функции сохранения данных в БД

        public void Save_Connection_Devices(object Sender, Connection_devicesClass SaveObj)
        {
            #region создаём массив id конекторов
            int i = 0;
            List<int> ConnectorsID = new List<int>();
            while (i < SaveObj.Connectors.Count)
            {
                if (SaveObj.Connectors[i].id >= 0)
                {
                    ConnectorsID.Add(SaveObj.Connectors[i].id);
                }
                else
                {
                    ConnectorsID.Add(-1);
                }
                i++;
            }
            while (i < 4)
            {
                ConnectorsID.Add(-1);
                i++;
            }
            #endregion

            if (SaveObj.id < 0)
            {
                //создаем строку в таблице
                DataSet1.Connection_devicesRow AddingRow = Sheild_M_DataBase.Connection_devices.NewConnection_devicesRow();

                AddingRow.name = SaveObj.Name;
                AddingRow.factory_number = SaveObj.ZavNumber;
                AddingRow.id_type_conn1 = ConnectorsID[0];
                AddingRow.id_type_conn2 = ConnectorsID[1];
                AddingRow.id_type_conn3 = ConnectorsID[2];
                AddingRow.id_type_conn4 = ConnectorsID[3];
                AddingRow.description = SaveObj.Description;

                //добавить запись в бд
                Sheild_M_DataBase.Connection_devices.AddConnection_devicesRow(AddingRow);
                //обновляем таблицу (записываем только что добавленные данные)
                DataAdapterALLConnectionDevices.Update(Sheild_M_DataBase.Connection_devices);
                //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
                Sheild_M_DataBase.Connection_devices.RemoveConnection_devicesRow(AddingRow);
                //перезагружаем таблицу
                DataAdapterALLConnectionDevices.Fill(Sheild_M_DataBase.Connection_devices);
                //добавляем выданный id
                SaveObj.id = Sheild_M_DataBase.Connection_devices[Sheild_M_DataBase.Connection_devices.Count - 1].id;

                //разослать всем добаленные объект
                this.SendNewConnection_devices (this, SaveObj, Sender);
            }
            else
            {
                //обновить данные в бд
                DataSet1.Connection_devicesRow temprow = Sheild_M_DataBase.Connection_devices.FindByid(SaveObj.id);

                temprow.name = SaveObj.Name;
                temprow.factory_number = SaveObj.ZavNumber;
                temprow.description = SaveObj.Description;
                temprow.id_type_conn1 = ConnectorsID[0];
                temprow.id_type_conn2 = ConnectorsID[1];
                temprow.id_type_conn3 = ConnectorsID[2];
                temprow.id_type_conn4 = ConnectorsID[3];

                DataAdapterALLConnectionDevices.Update(Sheild_M_DataBase.Connection_devices);
            }
        }

        public void Save_Antenn(object Sender, AntennOptionsClass SaveObj)
        {
            if (SaveObj.id < 0)
            {
                //создаём строку антены
                DataSet1.AntennasRow AddingRow = Sheild_M_DataBase.Antennas.NewAntennasRow();
                AddingRow.Наименование = SaveObj.Name;
                AddingRow.Заводской_номер = SaveObj.ZAVNumber;
                AddingRow.id_Типа_разъема = SaveObj.Connector.id;
                AddingRow.Количество_лучей = SaveObj.NumberOfRay;
                AddingRow.Размер_аппертуры_по_X = SaveObj.AppertureSizeX;
                AddingRow.Размер_аппертуры_по_Y = SaveObj.AppertureSizeY;
                AddingRow.Вынос_аппертуры_от_фланца_по_Z = SaveObj.RemovalAppertureSizeZ;
                AddingRow.Смещение_центра_аппертуры_по_X = SaveObj.RemovalAppertureSizeX;
                AddingRow.Смещение_центра_аппертуры_по_Y = SaveObj.RemovalAppertureSizeY;
                AddingRow.Используется_в_качестве_зонда = SaveObj.UsingAsZond;
                AddingRow.Описание = SaveObj.Description;
                AddingRow.Калибровочная_таблица = "XZ_Neizvestno";

#if relise
#error TODO: вписать калибровочную таблицу
#endif

                //добавить запись в бд
                Sheild_M_DataBase.Antennas.AddAntennasRow(AddingRow);
                //обновляем таблицу (записываем только что добавленные данные)
                DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);

                //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
                Sheild_M_DataBase.Antennas.RemoveAntennasRow(AddingRow);
                //обновляем таблицу
                DataAdapterAntennas.Fill(Sheild_M_DataBase.Antennas);
                //добавляем выданный id
                SaveObj.id = Sheild_M_DataBase.Antennas[Sheild_M_DataBase.Antennas.Count - 1].id;

                //разослать всем добаленные объект
                this.SendNewAntenn(this, SaveObj, Sender);
            }
            else
            {
                //обновить данные в бд
                DataSet1.AntennasRow temprow = Sheild_M_DataBase.Antennas.FindByid(SaveObj.id);

                temprow.Наименование = SaveObj.Name;
                temprow.Заводской_номер = SaveObj.ZAVNumber;
                temprow.id_Типа_разъема = SaveObj.Connector.id;
                temprow.Количество_лучей = SaveObj.NumberOfRay;
                temprow.Размер_аппертуры_по_X = SaveObj.AppertureSizeX;
                temprow.Размер_аппертуры_по_Y = SaveObj.AppertureSizeY;
                temprow.Смещение_центра_аппертуры_по_X = SaveObj.RemovalAppertureSizeX;
                temprow.Смещение_центра_аппертуры_по_Y = SaveObj.RemovalAppertureSizeY;
                temprow.Вынос_аппертуры_от_фланца_по_Z = SaveObj.RemovalAppertureSizeZ;
                temprow.Используется_в_качестве_зонда = SaveObj.UsingAsZond;
                temprow.Описание = SaveObj.Description;

                DataAdapterAntennas.Update(Sheild_M_DataBase.Antennas);
            }
        }

        public void Save_Connector(object Sender, ConnectorsTypeClass SaveObj)
        {
            if (SaveObj.id < 0)
            {
                //создаём строку
                DataSet1.Connectors_typeRow AddingRow = Sheild_M_DataBase.Connectors_type.NewConnectors_typeRow();
                AddingRow.Наименование = SaveObj.Name;
                AddingRow.sex = SaveObj.Conectorsex.ToString();
                AddingRow.Описание = SaveObj.Description;
                                
                #region сохранение изображения
                if (SaveObj.ConnectorImage != null)
                {
                    MemoryStream ms = new MemoryStream();
                    SaveObj.ConnectorImage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                     AddingRow.Изображение= ms.GetBuffer();
                }
                #endregion

                //добавить запись в бд
                Sheild_M_DataBase.Connectors_type.AddConnectors_typeRow(AddingRow);
                //обновляем таблицу (записываем только что добавленные данные)
                DataAdapterConnectors_type.Update(Sheild_M_DataBase.Connectors_type);

                //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
                Sheild_M_DataBase.Connectors_type.RemoveConnectors_typeRow(AddingRow);
                //перезагружаем таблицу
                DataAdapterConnectors_type.Fill(Sheild_M_DataBase.Connectors_type);
                //добавляем выданный id
                SaveObj.id = Sheild_M_DataBase.Connectors_type[Sheild_M_DataBase.Connectors_type.Count - 1].id;


                //разослать всем добаленные объект
                this.SendNewConnectorsType(this, SaveObj, Sender);

                #region добавление нового в каждый контрол этой формы (они не подписаны на событие рассылки)
                if (connection_Devices_UserControl1.connectorsUserControl1 != Sender)
                {
                    connection_Devices_UserControl1.connectorsUserControl1.AddConnectors(SaveObj, false);
                }
                if (connection_Devices_UserControl1.connectorsUserControl2 != Sender)
                {
                    connection_Devices_UserControl1.connectorsUserControl2.AddConnectors(SaveObj, false);
                }
                if (connection_Devices_UserControl1.connectorsUserControl3 != Sender)
                {
                    connection_Devices_UserControl1.connectorsUserControl3.AddConnectors(SaveObj, false);
                }
                if (connection_Devices_UserControl1.connectorsUserControl4 != Sender)
                {
                    connection_Devices_UserControl1.connectorsUserControl4.AddConnectors(SaveObj, false);
                }
                if (antennOptionsUserControl1.connectorsUserControl1 != Sender)
                {
                    antennOptionsUserControl1.connectorsUserControl1.AddConnectors(SaveObj, false);
                }
                #endregion
            }
            else
            {
                //обновить данные в бд
                DataSet1.Connectors_typeRow temprow = Sheild_M_DataBase.Connectors_type.FindByid(SaveObj.id);
                temprow.sex = SaveObj.Conectorsex.ToString();
                temprow.Описание = SaveObj.Description;

                #region сохранение изображения
                if (SaveObj.ConnectorImage != null)
                {
                    MemoryStream ms = new MemoryStream();
                    SaveObj.ConnectorImage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    temprow.Изображение = ms.GetBuffer();
                }
                else
                {
                    temprow.Изображение = null;
                }
                #endregion
                DataAdapterConnectors_type.Update(Sheild_M_DataBase.Connectors_type);

                //если все контролы конекторов имеют общий массив конекторов то никакого особого обновления не надо - они сами подменят свои данные
            }

            //for(int i=0;i<connection_Devices_UserControl1.connectorsUserControl3.AllConnectors.Count;i++)
            //{
            //    if (SaveConnector.id == connection_Devices_UserControl1.connectorsUserControl3.AllConnectors[i].id)
            //    {
            //        //connection_Devices_UserControl1.connectorsUserControl3.AllConnectors[i] = SaveConnector;
            //    }
            //}

        }

        /// <summary>
        /// Отправить команду SQL
        /// </summary>
        /// <param name="DataAdapter">инициализированный Адаптер базы данных</param>
        /// <param name="Comand">текст команды</param>
        /// <returns>DataSet со всеми возвращаемыми командой таблицами</returns>
        public DataSet Send_SQL_Command(SqlDataAdapter DataAdapter, string Comand)
        {
            DataSet returnDataSet = new DataSet();
            DataAdapter.SelectCommand.CommandText = Comand;
            DataAdapter.Fill(returnDataSet);

            return returnDataSet;
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
                    Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format("CREATE TABLE {0} (id int primary key IDENTITY(1, 1),Координата float not null, Значение float NOT NULL, Значение2 float NOT NULL);", Name));
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
        
        public void AddToMeasurementTable(string Name, float coord, float Data, float Data2)
        {
            if (Name != null && Name != "")
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                try
                {
                    Send_SQL_Command(new SqlDataAdapter("", this.Sheild_M_measurement_Connection), string.Format(culture, "INSERT INTO {0} (Координата, Значение, Значение2) VALUES ('{1}','{2}','{3}');", Name, coord, Data, Data2));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new Exception("Не указано имя таблицы");
            }
        }

        public int SaveMainTable(ResultType_MAINClass SaveObj)
        {
            int ret = -1;

            int idPositioningParameters = this.SavePositioningParameters(SaveObj.MainOptions.Parameters);
            int idZVBParameters = this.SaveZVBParameters(SaveObj.MainOptions.Parameters);

            #region определение типа измерения
            #endregion

            #region сохранение main

            DataSet1.mainRow AddingRow = Sheild_M_DataBase.main.NewmainRow();
            AddingRow.id_Типа = (int)SaveObj.MainOptions.Parameters.MeasurementResultType;
            AddingRow.Дата_формирования = SaveObj.MainOptions.Date;
            AddingRow.id_Оператора = SaveObj.MainOptions.Operator.id;
            AddingRow.id_Антенны = SaveObj.Antenn.id;
            AddingRow.id_Зонда = SaveObj.Zond.id;
            AddingRow.id_Исходного_результата = SaveObj.MainOptions.id_InitialResult;
            AddingRow.Наименование = SaveObj.MainOptions.Name;
            AddingRow.Описание = SaveObj.MainOptions.Descriptions;
            AddingRow._id_Параметров_измерения__позиции_осей_ = idPositioningParameters;
            AddingRow.id_Параметров_измерения_ZVB = idZVBParameters;


            //добавить запись в бд
            Sheild_M_DataBase.main.AddmainRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterMAIN.Update(Sheild_M_DataBase.main);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.main.RemovemainRow(AddingRow);
            //обновляем таблицу
            DataAdapterMAIN.Fill(Sheild_M_DataBase.main);
            //добавляем выданный id
            ret = Sheild_M_DataBase.main[Sheild_M_DataBase.main.Count - 1].id;
            #endregion



            this.SavePolarizationElement(SaveObj.MeasurementData.PolarizationElements, ret);
            this.SaveSegmentTable(SaveObj.MainOptions.Parameters.SegmentTable, ret);
            //SaveObj.MainOptions.Parameters.

            return ret;
        }

        public int SavePolarizationElement(PolarizationElementClass SaveObj, int idMain)
        {
            int ret = -1;

            DataSet1.Список_результатовRow AddingRow = Sheild_M_DataBase.Список_результатов.NewСписок_результатовRow();
            AddingRow.Поляризация = SaveObj.Polarization;
            AddingRow.id_main = idMain;


            //добавить запись в бд
            Sheild_M_DataBase.Список_результатов.AddСписок_результатовRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterСписокРезультатов.Update(Sheild_M_DataBase.Список_результатов);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.Список_результатов.RemoveСписок_результатовRow(AddingRow);
            //обновляем таблицу
            DataAdapterСписокРезультатов.Fill(Sheild_M_DataBase.Список_результатов);
            //добавляем выданный id
            ret = Sheild_M_DataBase.Список_результатов[Sheild_M_DataBase.Список_результатов.Count - 1].id;


            this.SaveFrequencyElement(SaveObj.FrequencyElements, ret);
            return ret;
        }

        public void SavePolarizationElement(List<PolarizationElementClass> polarization, int idMain)
        {
            foreach (PolarizationElementClass pol in polarization)
            {
                this.SavePolarizationElement(pol,idMain);
            }
        }

        public int SaveFrequencyElement(FrequencyElementClass SaveObj, int idPolarization)
        {
            int ret = -1;

            string TableName = "Measurement_" + idPolarization.ToString();

            this.CreateMeasurementTable(TableName);

            foreach (ResultElementClass res in SaveObj.ResultElements)
            {
                this.AddToMeasurementTable(TableName,Convert.ToSingle(res.Cordinate), Convert.ToSingle(res.Data1), Convert.ToSingle(res.Data2));
            }

            DataSet1.ЧастотыRow AddingRow = Sheild_M_DataBase.Частоты.NewЧастотыRow();
            AddingRow.id_Списка_результатов = idPolarization;
            AddingRow.Частота = SaveObj.Frequency;
            AddingRow.Наименование_таблицы_с_данными = TableName;

            //добавить запись в бд
            Sheild_M_DataBase.Частоты.AddЧастотыRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterЧастоты.Update(Sheild_M_DataBase.Частоты);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.Частоты.RemoveЧастотыRow(AddingRow);
            //обновляем таблицу
            DataAdapterЧастоты.Fill(Sheild_M_DataBase.Частоты);
            //добавляем выданный id
            ret = Sheild_M_DataBase.Частоты[Sheild_M_DataBase.Частоты.Count - 1].id;

            return ret;
        }

        public void SaveFrequencyElement(List<FrequencyElementClass> Frequency, int idPolarization)
        {
            foreach (FrequencyElementClass fr in Frequency)
            {
                this.SaveFrequencyElement(fr, idPolarization);
            }
        }

        public int SavePositioningParameters(MeasurementParametrsClass SaveObj)
        {
            int ret = -1;

            DataSet1.Measurement_PositionParametersRow AddingRow = Sheild_M_DataBase.Measurement_PositionParameters.NewMeasurement_PositionParametersRow();
            AddingRow.Конечная_координата_Мачты_по_W =Convert.ToDouble(SaveObj.StopTower_W);
            AddingRow.Конечная_координата_Мачты_по_Y = Convert.ToDouble(SaveObj.StopTower_Y);
            AddingRow.Конечная_координата_ОПУ_по_W = Convert.ToDouble(SaveObj.StopOPU_W);
            AddingRow.Конечная_координата_ОПУ_по_Y = Convert.ToDouble(SaveObj.StopOPU_Y);
            AddingRow.Начальная_координата_Мачты_по_W = Convert.ToDouble(SaveObj.StartTower_W);
            AddingRow.Начальная_координата_Мачты_по_Y = Convert.ToDouble(SaveObj.StartTower_Y);
            AddingRow.Начальная_координата_ОПУ_по_W = Convert.ToDouble(SaveObj.StartOPU_W);
            AddingRow.Начальная_координата_ОПУ_по_Y = Convert.ToDouble(SaveObj.StartOPU_Y);
            AddingRow.Скорость_сканирования = Convert.ToDouble(SaveObj.StepMeasurement);

            //добавить запись в бд
            Sheild_M_DataBase.Measurement_PositionParameters.AddMeasurement_PositionParametersRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterMeasurement_PositionParameters.Update(Sheild_M_DataBase.Measurement_PositionParameters);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.Measurement_PositionParameters.RemoveMeasurement_PositionParametersRow(AddingRow);
            //обновляем таблицу
            DataAdapterMeasurement_PositionParameters.Fill(Sheild_M_DataBase.Measurement_PositionParameters);
            //добавляем выданный id
            ret = Sheild_M_DataBase.Measurement_PositionParameters[Sheild_M_DataBase.Measurement_PositionParameters.Count - 1].id;

            return ret;
        }

        public int SaveZVBParameters(MeasurementParametrsClass SaveObj)
        {
            int ret = -1;

            DataSet1.Measurement_ZVBParametersRow AddingRow = Sheild_M_DataBase.Measurement_ZVBParameters.NewMeasurement_ZVBParametersRow();
            AddingRow.Power =Convert.ToDouble( SaveObj.Power);
            AddingRow.SweepTime = Convert.ToDouble(SaveObj.SweepTime);
            AddingRow.SweepTimeAuto = SaveObj.SweepTimeAuto;
            AddingRow.Bandwidth = Convert.ToDouble(SaveObj.Bandwidth);
            AddingRow.Measurement_Tupe_S = (int)SaveObj.MesurementS__Type;


            //добавить запись в бд
            Sheild_M_DataBase.Measurement_ZVBParameters.AddMeasurement_ZVBParametersRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterMeasurement_ZVBParameters.Update(Sheild_M_DataBase.Measurement_ZVBParameters);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.Measurement_ZVBParameters.RemoveMeasurement_ZVBParametersRow(AddingRow);
            //обновляем таблицу
            DataAdapterMeasurement_ZVBParameters.Fill(Sheild_M_DataBase.Measurement_ZVBParameters);
            //добавляем выданный id
            ret = Sheild_M_DataBase.Measurement_ZVBParameters[Sheild_M_DataBase.Measurement_ZVBParameters.Count - 1].id;

            return ret;
        }

        public int SaveSegmentTable(ZVB14_Main.SegmentTableElementOptionsClass SaveObj,int idMain)
        {
            int ret = -1;

            DataSet1.Segment_TableRow AddingRow = Sheild_M_DataBase.Segment_Table.NewSegment_TableRow();
            AddingRow.id_main = idMain;
            AddingRow.Start_Frequency = SaveObj.FrequencieStart;
            AddingRow.Stop_Frequency = SaveObj.FrequencieStop;
            AddingRow.Number_of_points = SaveObj.NumberOfPoint;

            //добавить запись в бд
            Sheild_M_DataBase.Segment_Table.AddSegment_TableRow(AddingRow);
            //обновляем таблицу (записываем только что добавленные данные)
            DataAdapterSegment_Table.Update(Sheild_M_DataBase.Segment_Table);

            //требуется избавиться от локальных фантомных записей(записи без присвоенного id)
            Sheild_M_DataBase.Segment_Table.RemoveSegment_TableRow(AddingRow);
            //обновляем таблицу
            DataAdapterSegment_Table.Fill(Sheild_M_DataBase.Segment_Table);
            //добавляем выданный id
            ret = Sheild_M_DataBase.Segment_Table[Sheild_M_DataBase.Segment_Table.Count - 1].id;

            return ret;
        }

        public void SaveSegmentTable(List<ZVB14_Main.SegmentTableElementOptionsClass> SaveObj, int idMain)
        {
            foreach (ZVB14_Main.SegmentTableElementOptionsClass temp in SaveObj)
            {
                this.SaveSegmentTable(temp, idMain);
            }
        }

        //void resultDNUserControl1_NeedToSaveResultEvent(object Sender, OptionsClass.ResultClass.ResultDNClass SaveObj)
        //{
        //    if (SaveObj.id >= 0)
        //    {
        //        //обновить данные в бд
        //        DataSet1.Список_результатов_ДНRow temprow = Sheild_M_DataBase.Список_результатов_ДН.FindByid(SaveObj.id);
        //        if (temprow != null)
        //        {
        //            if (SaveObj.Координаты_фазового_центра_1 != null)
        //            {
        //                temprow.Координаты_фазового_центра_1 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_1);
        //            }
        //            if (SaveObj.Координаты_фазового_центра_2 != null)
        //            {
        //                temprow.Координаты_фазового_центра_2 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_2);
        //            }
        //            if (SaveObj.Коэффициент_усиления_в_максимуме_диаграммы_направленности != null)
        //            {
        //                temprow.Коэффициент_усиления_в_максимуме_диаграммы_направленности = Convert.ToDouble(SaveObj.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
        //            }
        //            if (SaveObj.Направление_максимума_диаграммы_направленности != null)
        //            {
        //                temprow.Направление_максимума_диаграммы_направленности = Convert.ToDouble(SaveObj.Направление_максимума_диаграммы_направленности);
        //            }
        //            if (SaveObj.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности != null)
        //            {
        //                temprow.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности = Convert.ToDouble(SaveObj.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности);
        //            }
        //            if (SaveObj.Уровень_боковых_лепестков != null)
        //            {
        //                temprow.Уровень_боковых_лепестков = Convert.ToDouble(SaveObj.Уровень_боковых_лепестков);
        //            }
        //            if (SaveObj.Ширина_диаграммы_направленности_по_половине_мощности != null)
        //            {
        //                temprow.Ширина_диаграммы_направленности_по_половине_мощности = Convert.ToDouble(SaveObj.Ширина_диаграммы_направленности_по_половине_мощности);
        //            }
        //            DataAdapterСписокРезультатовДН.Update(Sheild_M_DataBase.Список_результатов_ДН);
        //        }
        //    }
        //}

        //void resultPHUserControl1_NeedToSaveResultEvent(object Sender, OptionsClass.ResultClass.ResultPHClass SaveObj)
        //{
        //    if (SaveObj.id >= 0)
        //    {
        //        //обновить данные в бд
        //        DataSet1.Список_результатов_ПХRow temprow = Sheild_M_DataBase.Список_результатов_ПХ.FindByid(SaveObj.id);
        //        if (temprow != null)
        //        {
        //            if (SaveObj.Координаты_фазового_центра_1 != null)
        //            {
        //                temprow.Координаты_фазового_центра_1 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_1);
        //            }
        //            if (SaveObj.Координаты_фазового_центра_2 != null)
        //            {
        //                temprow.Координаты_фазового_центра_2 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_2);
        //            }
        //            if (SaveObj.Коэффициент_Эллиптичности != null)
        //            {
        //                temprow.Коэффициент_Эллиптичности = Convert.ToDouble(SaveObj.Коэффициент_Эллиптичности);
        //            }
        //            if (SaveObj.Поляризационное_отношение != null)
        //            {
        //                temprow.Поляризационное_отношение = Convert.ToDouble(SaveObj.Поляризационное_отношение);
        //            }
        //            if (SaveObj.Угол_наклона_эллипса_поляризации != null)
        //            {
        //                temprow.Угол_наклона_эллипса_поляризации = Convert.ToDouble(SaveObj.Угол_наклона_эллипса_поляризации);
        //            }

        //            DataAdapterСписокРезультатовПХ.Update(Sheild_M_DataBase.Список_результатов_ПХ);
        //        }
        //    }
        //}

        //void resultSDNMUserControl1_NeedToSaveResultEvent(object Sender, OptionsClass.ResultClass.ResultSDNMClass SaveObj)
        //{
        //    if (SaveObj.id >= 0)
        //    {
        //        //обновить данные в бд
        //        DataSet1.Список_результатов_СДНМRow temprow = Sheild_M_DataBase.Список_результатов_СДНМ.FindByid(SaveObj.id);
        //        if (temprow != null)
        //        {
        //            if (SaveObj.Координаты_фазового_центра_1 != null)
        //            {
        //                temprow.Координаты_фазового_центра_1 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_1);
        //            }
        //            if (SaveObj.Координаты_фазового_центра_2 != null)
        //            {
        //                temprow.Координаты_фазового_центра_2 = Convert.ToDouble(SaveObj.Координаты_фазового_центра_2);
        //            }
        //            if (SaveObj.Коэффициент_усиления_в_максимуме_диаграммы_направленности != null)
        //            {
        //                temprow.Коэффициент_усиления_в_максимуме_диаграммы_направленности = Convert.ToDouble(SaveObj.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
        //            }
        //            if (SaveObj.Направление_максимума_диаграммы_направленности != null)
        //            {
        //                temprow.Направление_максимума_диаграммы_направленности = Convert.ToDouble(SaveObj.Направление_максимума_диаграммы_направленности);
        //            }
        //            if (SaveObj.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности != null)
        //            {
        //                temprow.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности = Convert.ToDouble(SaveObj.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности);
        //            }
        //            if (SaveObj.Уровень_боковых_лепестков != null)
        //            {
        //                temprow.Уровень_боковых_лепестков = Convert.ToDouble(SaveObj.Уровень_боковых_лепестков);
        //            }
        //            if (SaveObj.Ширина_диаграммы_направленности_по_половине_мощности != null)
        //            {
        //                temprow.Ширина_диаграммы_направленности_по_половине_мощности = Convert.ToDouble(SaveObj.Ширина_диаграммы_направленности_по_половине_мощности);
        //            }


        //            if (SaveObj.Коэффициент_Эллиптичности != null)
        //            {
        //                temprow.Коэффициент_Эллиптичности = Convert.ToDouble(SaveObj.Коэффициент_Эллиптичности);
        //            }
        //            if (SaveObj.Поляризационное_отношение != null)
        //            {
        //                temprow.Поляризационное_отношение = Convert.ToDouble(SaveObj.Поляризационное_отношение);
        //            }
        //            if (SaveObj.Угол_наклона_эллипса_поляризации != null)
        //            {
        //                temprow.Угол_наклона_эллипса_поляризации = Convert.ToDouble(SaveObj.Угол_наклона_эллипса_поляризации);
        //            }
                    
        //            DataAdapterСписокРезультатовСДНМ.Update(Sheild_M_DataBase.Список_результатов_СДНМ);
        //        }
        //    }
        //}
        #endregion

        #region рассылка новых данных (антенн, разъёмов, устройств)
        public delegate void AddNewAntennDelegate(object Sender, AntennOptionsClass Obj);
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
            Delegate[] inlist = AddNewAntennEvent.GetInvocationList();

            foreach (AddNewAntennDelegate Event in inlist)
            {
                if (Event.Target.GetHashCode() != NoSend.GetHashCode())
                {
                    Event(Sender, Obj);
                }
            }
        }


        public delegate void AddNewConnection_devicesDelegate(object Sender, Connection_devicesClass Obj);
        /// <summary>
        /// событие добавление нового объекта
        /// </summary>
        public event AddNewConnection_devicesDelegate AddNewConnection_devicesEvent;
        /// <summary>
        /// разослать новые объекты всем кроме кого то
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Obj"></param>
        /// <param name="NoSend"></param>
        protected void SendNewConnection_devices(object Sender, Connection_devicesClass Obj, object NoSend)
        {
            if (AddNewConnection_devicesEvent != null)
            {
                Delegate[] inlist = AddNewConnection_devicesEvent.GetInvocationList();

                foreach (AddNewConnection_devicesDelegate Event in inlist)
                {
                    if (Event.Target.GetHashCode() != NoSend.GetHashCode())
                    {
                        Event(Sender, Obj);
                    }
                }
            }
        }


        public delegate void AddNewConnectorsTypeDelegate(object Sender, ConnectorsTypeClass Obj);
        /// <summary>
        /// событие добавление нового объекта
        /// </summary>
        public event AddNewConnectorsTypeDelegate AddNewConnectorsTypeEvent;
        /// <summary>
        /// разослать новые объекты всем кроме кого то
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Obj"></param>
        /// <param name="NoSend"></param>
        protected void SendNewConnectorsType(object Sender, ConnectorsTypeClass Obj, object NoSend)
        {
            if (AddNewConnectorsTypeEvent != null)
            {
                Delegate[] inlist = AddNewConnectorsTypeEvent.GetInvocationList();

                foreach (AddNewConnectorsTypeDelegate Event in inlist)
                {
                    if (Event.Target.GetHashCode() != NoSend.GetHashCode())
                    {
                        Event(Sender, Obj);
                    }
                }
            }
        }
        #endregion

        #region преобразование ROW to Class
        /// <summary>
        /// загрузка значений из таблицы измерений
        /// </summary>
        /// <param name="tempFreg"></param>
        /// <returns></returns>
        protected Series GetSiriesByЧастотыRow(DataSet1.ЧастотыRow tempFreg)
        {
            Series ser = null;
            //оставляю рабочим старый вариант, тк при другом способе происходит 2 преобразования данных, вместо 1го
            #region старый вариант
            try
            {
                SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(string.Format("SELECT * FROM {0} ORDER BY Координата ASC", tempFreg.Наименование_таблицы_с_данными), this.Sheild_M_measurement_Connection);

                DataSet1.ЗначенияDataTable tempDataTable = new DataSet1.ЗначенияDataTable();
                DataAdaptermeasurement.Fill(tempDataTable);

                ser = new Series(this.GetFullNameByFrequencyRow(tempFreg));
                ser.ChartType = SeriesChartType.Line;

                foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                {
                    ser.Points.AddXY(znRow.Координата, znRow.Значение);
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка загрузки данных из БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
            
            //ser = this.GetFrequencyElementByЧастотыRow(tempFreg).GetAmplSeries();
            //ser.Name = this.GetFullNameByFrequencyRow(tempFreg);
            //ser.ChartType = SeriesChartType.Line;

            return ser;
        }

        protected FrequencyElementClass GetFrequencyElementByЧастотыRow(DataSet1.ЧастотыRow tempFreg)
        {
            FrequencyElementClass ret = new FrequencyElementClass(tempFreg.Частота);
            ret.TableResultName = tempFreg.Наименование_таблицы_с_данными.TrimEnd(" ".ToCharArray());

            try
            {
                SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(string.Format("SELECT * FROM {0} ORDER BY Координата ASC", tempFreg.Наименование_таблицы_с_данными), this.Sheild_M_measurement_Connection);

                DataSet1.ЗначенияDataTable tempDataTable = new DataSet1.ЗначенияDataTable();
                DataAdaptermeasurement.Fill(tempDataTable);

                foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                {
                    ret.ResultElements.Add(new ResultElementClass(znRow.Координата,znRow.Значение,znRow.Значение2));
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка загрузки данных из БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ret;
        }

        protected MeasurementDataClass GetMeasurementDataByMainRow(DataSet1.mainRow tempMain)
        {
            MeasurementDataClass ret = new MeasurementDataClass();

            DataSet1.Список_результатовRow[] tempSpisok = tempMain.GetСписок_результатовRows();

            foreach (DataSet1.Список_результатовRow tempSpisokRow in tempSpisok)
            {
                PolarizationElementClass tempPolarizationElement = new PolarizationElementClass();
                ret.PolarizationElements.Add(tempPolarizationElement);

                if (!tempSpisokRow.IsПоляризацияNull())
                {
                    tempPolarizationElement.Polarization = tempSpisokRow.Поляризация;
                }

                DataSet1.ЧастотыRow[] tempFreq = tempSpisokRow.GetЧастотыRows();

                foreach (DataSet1.ЧастотыRow tempFreqRow in tempFreq)
                {
                    tempPolarizationElement.FrequencyElements.Add(this.GetFrequencyElementByЧастотыRow(tempFreqRow));
                }
            }
            return ret;
        }

        protected MeasurementDataClass GetMeasurementDataByЧастотыRow_for_СДНМ(DataSet1.ЧастотыRow newtempfreq)
        {
            MeasurementDataClass ret = new MeasurementDataClass();

            DataSet1.mainRow tempMain = GetMainRowByFrequencyRow(newtempfreq);

            DataSet1.Список_результатовRow[] tempSpisok = tempMain.GetСписок_результатовRows();

            foreach (DataSet1.Список_результатовRow tempSpisokRow in tempSpisok)
            {
                PolarizationElementClass tempPolarizationElement = new PolarizationElementClass();
                ret.PolarizationElements.Add(tempPolarizationElement);

                if (!tempSpisokRow.IsПоляризацияNull())
                {
                    tempPolarizationElement.Polarization = tempSpisokRow.Поляризация;
                }

                DataSet1.ЧастотыRow[] tempFreq = tempSpisokRow.GetЧастотыRows();

                foreach (DataSet1.ЧастотыRow tempFreqRow in tempFreq)
                {
                    if (tempFreqRow.Частота == newtempfreq.Частота)
                    {
                        FrequencyElementClass tempfrequ = this.GetFrequencyElementByЧастотыRow(tempFreqRow);
                        tempfrequ.CalculationResults = new OptionsClass.ResultClass.ResultSDNMClass();
                        tempPolarizationElement.FrequencyElements.Add(tempfrequ);
                    }
                }
            }
            return ret;
        }

        protected Series GetSiriesKYByTableName(string tableName, string KyType,string name)
        {
            Series ser = null;
            try
            {
                SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(string.Format("SELECT * FROM {0} ORDER BY Координата ASC", tableName), this.Sheild_M_measurement_Connection);

                DataSet1.ЗначенияDataTable tempDataTable = new DataSet1.ЗначенияDataTable();
                DataAdaptermeasurement.Fill(tempDataTable);

                ser = new Series(name);
                ser.ChartType = SeriesChartType.Line;

                if (KyType == KYTupeEnum.main.ToString())
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение);
                    }
                }
                if (KyType == KYTupeEnum.Cross.ToString())
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение2);
                    }
                }
                if (KyType == KYTupeEnum.Sum.ToString())
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение + znRow.Значение2);
                    }
                }
             
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка загрузки данных из БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ser;
        }

        protected Series GetSiriesKYByЧастотыRow(KYTupeEnum KyType, DataSet1.ЧастотыRow tempFreg)
        {
            Series ser = null;
            try
            {
                SqlDataAdapter DataAdaptermeasurement = new SqlDataAdapter(string.Format("SELECT * FROM {0} ORDER BY Координата ASC", tempFreg.Наименование_таблицы_с_данными), this.Sheild_M_measurement_Connection);

                DataSet1.ЗначенияDataTable tempDataTable = new DataSet1.ЗначенияDataTable();
                DataAdaptermeasurement.Fill(tempDataTable);

                ser = new Series(this.GetFullNameKYByFrequencyRow(tempFreg, KyType));
                ser.ChartType = SeriesChartType.Line;

                if (KyType == KYTupeEnum.main)
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение);
                    }
                }
                if (KyType == KYTupeEnum.Cross)
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение2);
                    }
                }
                if (KyType == KYTupeEnum.Sum)
                {
                    foreach (DataSet1.ЗначенияRow znRow in tempDataTable)
                    {
                        ser.Points.AddXY(znRow.Координата, znRow.Значение + znRow.Значение2);
                    }
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка загрузки данных из БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ser;
        }

        #region получение полных имён
        /// <summary>
        /// получить полное имя по частоте
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected string GetFullNameByFrequencyRow(DataSet1.ЧастотыRow FrequencyResult)
        {
            string FullName = "";
            string AntennName = "не указана";
            string mainName = "Без названия";
            string polarization = "не указана";
            string DateTime1="";

            DataSet1.mainRow tempMain = this.GetMainRowByFrequencyRow(FrequencyResult);
            DataSet1.AntennasRow[] tempantenn = tempMain.GetAntennasRowsBymain_Antennas();
            DataSet1.Список_результатовRow tempSpisok = FrequencyResult.GetParentRow("Список результатов_Частоты") as DataSet1.Список_результатовRow;
            if (!tempSpisok.IsПоляризацияNull())
            {
                polarization = tempSpisok.Поляризация.ToString();
            }

            if (tempantenn.Length != 0)
            {
                AntennName = tempantenn[0].Наименование.TrimEnd(" ".ToCharArray());
            }

            if (!tempMain.IsНаименованиеNull())
            {
                mainName = tempMain.Наименование.TrimEnd(" ".ToCharArray());
            }
            DateTime1=tempMain.Дата_формирования.ToString();

            FullName = string.Format("{0}; \n{1}; \nАнтенна: {2}; \nПоляризация={3}; \nF={4}Гц; ", mainName, DateTime1, AntennName, polarization, FrequencyResult.Частота.ToString());
            //FullName = AntennName + "; f=" + FrequencyResult.Частота.ToString() + " Гц; " + "Поляризация " + polarization + "; " + mainName + ";";
            return FullName;
        }
        protected string GetFullNameKYByFrequencyRow(DataSet1.ЧастотыRow FrequencyResult,KYTupeEnum KYTupe)
        {
            string FullName = "";
            string AntennName = "не указана";
            string mainName = "Без названия";
            string polarization = KYTupe.ToString();
            string DateTime1 = "";

            DataSet1.mainRow tempMain = this.GetMainRowByFrequencyRow(FrequencyResult);
            DataSet1.AntennasRow[] tempantenn = tempMain.GetAntennasRowsBymain_Antennas();
            DataSet1.Список_результатовRow tempSpisok = FrequencyResult.GetParentRow("Список результатов_Частоты") as DataSet1.Список_результатовRow;

            if (tempantenn.Length != 0)
            {
                AntennName = tempantenn[0].Наименование.TrimEnd(" ".ToCharArray());
            }

            if (!tempMain.IsНаименованиеNull())
            {
                mainName = tempMain.Наименование.TrimEnd(" ".ToCharArray());
            }
            DateTime1 = tempMain.Дата_формирования.ToString();

            FullName = string.Format("{0}; \n{1}; \nАнтенна: {2}; \nПоляризация={3};", mainName, DateTime1, AntennName, polarization);
            //FullName = AntennName + "; f=" + FrequencyResult.Частота.ToString() + " Гц; " + "Поляризация " + polarization + "; " + mainName + ";";
            return FullName;
        }
        #endregion

        protected DataSet1.mainRow GetMainRowByFrequencyRow(DataSet1.ЧастотыRow FrequencyResult)
        {
            return FrequencyResult.GetParentRow("Список результатов_Частоты").GetParentRow("main_Список результатов") as DataSet1.mainRow;
        }

        /// <summary>
        /// преобразовать строки в таблшице частот в ноды
        /// </summary>
        /// <param name="FrequencyResult"></param>
        /// <returns></returns>
        protected TreeNode[] GetFrequencyNodes(DataSet1.ЧастотыRow[] FrequencyResult)
        {
            List<TreeNode> ret = new List<TreeNode>();
            //заполняем ноды
            foreach (DataSet1.ЧастотыRow freg in FrequencyResult)
            {
                TreeNode fregNode = new TreeNode(freg.Частота.ToString() + " Гц");
                fregNode.Tag = freg;
                ret.Add(fregNode);
            }
            return ret.ToArray();
        }
        /// <summary>
        /// получить класс опций по записи main
        /// </summary>
        /// <param name="tempMain"></param>
        /// <returns></returns>
        MainOptionsClass GetMainOptionsClass(DataSet1.mainRow tempMain)
        {
            MainOptionsClass tempmainOptions = new MainOptionsClass();
            #region Описание типа измерения
            DataSet1.ResultTypesRow[] ResultTypesRows = tempMain.GetResultTypesRows();

            if (ResultTypesRows.Length != 0)
            {
                tempmainOptions.Parameters.MeasurementResultType = (MeasurementTypeEnum)ResultTypesRows[0].id;
                tempmainOptions.Parameters.MeasurementResultTypeName = ResultTypesRows[0].Наименование.TrimEnd(" ".ToCharArray());

                if (!ResultTypesRows[0].IsОписаниеNull())
                    tempmainOptions.Parameters.MeasurementResultTypeDescription = ResultTypesRows[0].Описание.TrimEnd(" ".ToCharArray());
            }
            #endregion

            #region наименование, описание и дата
            tempmainOptions.Date = tempMain.Дата_формирования;

            if (!tempMain.IsОписаниеNull())
            {
                tempmainOptions.Descriptions = tempMain.Описание.TrimEnd(" ".ToCharArray()); ;
            }
            if (!tempMain.IsНаименованиеNull())
            {
                tempmainOptions.Name = tempMain.Наименование.TrimEnd(" ".ToCharArray()); ;
            }

            #endregion

            #region Получаем связанные таблицы параметров позиций
            DataSet1.Measurement_PositionParametersRow[] MeasurementParamRows = tempMain.GetMeasurement_PositionParametersRows();

            if (MeasurementParamRows.Length != 0)
            {
                tempmainOptions.Parameters.StartOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_ОПУ_по_W);
                tempmainOptions.Parameters.StartOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_ОПУ_по_Y);
                tempmainOptions.Parameters.StartTower_W = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_Мачты_по_W);
                tempmainOptions.Parameters.StartTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_Мачты_по_Y);

                if (!MeasurementParamRows[0].IsКонечная_координата_ОПУ_по_WNull())
                {
                    tempmainOptions.Parameters.StopOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Конечная_координата_ОПУ_по_W);
                }
                else
                {
                    tempmainOptions.Parameters.StopOPU_W = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_ОПУ_по_W);
                }

                if (!MeasurementParamRows[0].IsКонечная_координата_ОПУ_по_YNull())
                {
                    tempmainOptions.Parameters.StopOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Конечная_координата_ОПУ_по_Y);
                }
                else
                {
                    tempmainOptions.Parameters.StopOPU_Y = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_ОПУ_по_Y);
                }

                if (!MeasurementParamRows[0].IsКонечная_координата_Мачты_по_WNull())
                {
                    tempmainOptions.Parameters.StopTower_W = Convert.ToDecimal(MeasurementParamRows[0].Конечная_координата_Мачты_по_W);
                }
                else
                {
                    tempmainOptions.Parameters.StopTower_W = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_Мачты_по_W);
                }

                if (!MeasurementParamRows[0].IsКонечная_координата_Мачты_по_YNull())
                {
                    tempmainOptions.Parameters.StopTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Конечная_координата_Мачты_по_Y);
                }
                else
                {
                    tempmainOptions.Parameters.StopTower_Y = Convert.ToDecimal(MeasurementParamRows[0].Начальная_координата_Мачты_по_Y);
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

                switch (MeasurementParamZVBRows[0].Measurement_Tupe_S)
                {
                    case 11:
                        tempmainOptions.Parameters.MesurementS__Type = ZVB14_Main.MeasurementS__Enum.S11;
                        break;
                    case 12:
                        tempmainOptions.Parameters.MesurementS__Type = ZVB14_Main.MeasurementS__Enum.S12;
                        break;
                    case 21:
                        tempmainOptions.Parameters.MesurementS__Type = ZVB14_Main.MeasurementS__Enum.S21;
                        break;
                    case 22:
                        tempmainOptions.Parameters.MesurementS__Type = ZVB14_Main.MeasurementS__Enum.S22;
                        break;
                }
            }
            #endregion

            #region Получаем связанные таблицы операторов
            DataSet1.OperatorsRow[] OperRows = tempMain.GetOperatorsRows();

            if (OperRows.Length != 0)
            {
                tempmainOptions.Operator.Fam = OperRows[0].Фамилия.TrimEnd(" ".ToCharArray());
                tempmainOptions.Operator.Name = OperRows[0].Имя.TrimEnd(" ".ToCharArray());
                tempmainOptions.Operator.Otchestvo = OperRows[0].Отчество.TrimEnd(" ".ToCharArray());
            }
            #endregion

            #region Получаем сегментную таблицу
            DataSet1.Segment_TableRow[] Rows = tempMain.GetSegment_TableRows();

            foreach (DataSet1.Segment_TableRow tempRow in Rows)
            {
                ZVB14_Main.SegmentTableElementOptionsClass tempSegment = new ZVB14_Main.SegmentTableElementOptionsClass();
                tempSegment.FrequencieStart = tempRow.Start_Frequency;
                tempSegment.FrequencieStop = tempRow.Stop_Frequency;
                tempSegment.NumberOfPoint = tempRow.Number_of_points;
                tempmainOptions.Parameters.SegmentTable.Add(tempSegment);
            }
            #endregion

            return tempmainOptions;
        }

        #region для антенн и конекторов

        /// <summary>
        /// перевести записи из БД о конекторах в массив конекторов
        /// </summary>
        /// <param name="tempconnectors"></param>
        /// <returns></returns>
        List<ConnectorsTypeClass> GetConnectors(DataSet1.Connectors_typeRow[] tempconnectors)
        {
            List<ConnectorsTypeClass> retList = new List<ConnectorsTypeClass>();

            foreach (DataSet1.Connectors_typeRow tempConROW in tempconnectors)
            {
                retList.Add(ConnectorTypeRowToClass(tempConROW));
            }

            return retList;
        }
        ConnectorsTypeClass ConnectorTypeRowToClass(DataSet1.Connectors_typeRow tempConROW)
        {
            ConnectorsTypeClass tempconnector = new ConnectorsTypeClass();

                switch (tempConROW.sex.TrimEnd(" ".ToCharArray()))
                {
                    case "Female":
                        {
                            tempconnector.Conectorsex = SEX.Female;
                            break;
                        }
                    case "Male":
                        {
                            tempconnector.Conectorsex = SEX.Male;
                            break;
                        }
                    case "Waveguide":
                        {
                            tempconnector.Conectorsex = SEX.Waveguide;
                            break;
                        }
                }
                if (!tempConROW.IsОписаниеNull())
                {
                    tempconnector.Description = tempConROW.Описание.TrimEnd(" ".ToCharArray());
                }
                tempconnector.Name = tempConROW.Наименование.TrimEnd(" ".ToCharArray());

                if (!tempConROW.IsИзображениеNull())
                {
                    MemoryStream ms = new MemoryStream(tempConROW.Изображение);
                    tempconnector.ConnectorImage = Image.FromStream(ms);
                }
                tempconnector.id = tempConROW.id;

                return tempconnector;
        }
        AntennOptionsClass AntennRowToClass(DataSet1.AntennasRow tempAnten)
        {
            AntennOptionsClass anten = new AntennOptionsClass();
            anten.Name = tempAnten.Наименование.TrimEnd(" ".ToCharArray());

            if (!tempAnten.IsОписаниеNull())
            {
                anten.Description = tempAnten.Описание.TrimEnd(" ".ToCharArray());
            }

            if (!tempAnten.IsЗаводской_номерNull())
            {
                anten.ZAVNumber = tempAnten.Заводской_номер.TrimEnd(" ".ToCharArray());
            }
            anten.NumberOfRay = tempAnten.Количество_лучей;
            anten.UsingAsZond = tempAnten.Используется_в_качестве_зонда;
            anten.RemovalAppertureSizeX = tempAnten.Смещение_центра_аппертуры_по_X;
            anten.RemovalAppertureSizeY = tempAnten.Смещение_центра_аппертуры_по_Y;
            anten.RemovalAppertureSizeZ = tempAnten.Вынос_аппертуры_от_фланца_по_Z;
            anten.AppertureSizeX = tempAnten.Размер_аппертуры_по_X;
            anten.AppertureSizeY = tempAnten.Размер_аппертуры_по_Y;

            anten.id = tempAnten.id;

            //получить массив конекторов
            List<ConnectorsTypeClass> tempconnectors = this.GetConnectors(tempAnten.GetConnectors_typeRows());
            if (tempconnectors.Count != 0)
            {
                anten.Connector = tempconnectors[0];
            }
            return anten;
        }
        AntennOptionsClass MainRowToAntennClass(DataSet1.mainRow tempMain)
        {
            DataSet1.AntennasRow[] antennRows = tempMain.GetAntennasRowsBymain_Antennas();
            AntennOptionsClass retAnten=new AntennOptionsClass();
            if (antennRows.Length != 0)
            {
                retAnten = AntennRowToClass(antennRows[0]);
            }
            return retAnten;
        }
        Connection_devicesClass ConnectionDeviceRowToClass(DataSet1.Connection_devicesRow tempdevise)
        {
            Connection_devicesClass device = new Connection_devicesClass();
            device.Name = tempdevise.name.TrimEnd(" ".ToCharArray());

            if (!tempdevise.IsdescriptionNull())
            {
                device.Description = tempdevise.description.TrimEnd(" ".ToCharArray());
            }

            if (!tempdevise.Isfactory_numberNull())
            {
                device.ZavNumber = tempdevise.factory_number.TrimEnd(" ".ToCharArray());
            }
            device.id = tempdevise.id;
            #region получение конекторов

            List<ConnectorsTypeClass> tempconnectors1 = this.GetConnectors(tempdevise.GetConnectors_typeRowsByConnection_devices_Connectors_type());
            if (tempconnectors1.Count != 0)
            {
                device.Connectors.Add(tempconnectors1[0]);
            }

            List<ConnectorsTypeClass> tempconnectors2 = this.GetConnectors(tempdevise.GetConnectors_typeRowsByConnection_devices_Connectors_type1());
            if (tempconnectors2.Count != 0)
            {
                device.Connectors.Add(tempconnectors2[0]);
            }

            List<ConnectorsTypeClass> tempconnectors3 = this.GetConnectors(tempdevise.GetConnectors_typeRowsByConnection_devices_Connectors_type2());
            if (tempconnectors3.Count != 0)
            {
                device.Connectors.Add(tempconnectors3[0]);
            }

            List<ConnectorsTypeClass> tempconnectors4 = this.GetConnectors(tempdevise.GetConnectors_typeRowsByConnection_devices_Connectors_type3());
            if (tempconnectors4.Count != 0)
            {
                device.Connectors.Add(tempconnectors4[0]);
            }
            #endregion
            return device;
        }
        #endregion
        #endregion

        #region Вспомогательные переменные и функции
        private void UnvizibleElements()
        {
            this.previewGraphControl1.Visible = false;
            this.previewGraphControl1.Dock = DockStyle.None;
            this.antennOptionsUserControl1.Visible = false;
            this.antennOptionsUserControl1.Dock = DockStyle.None;
            this.mainOptionsUserControl1.Visible = false;
            this.connection_Devices_UserControl1.Visible = false;
            this.connection_Devices_UserControl1.Dock = DockStyle.None;
            this.resultDNUserControl1.Visible = false;
            this.resultDNUserControl1.Dock = DockStyle.None;
            this.resultPHUserControl1.Visible = false;
            this.resultPHUserControl1.Dock = DockStyle.None;
            this.resultSDNMUserControl1.Visible = false;
            this.resultSDNMUserControl1.Dock = DockStyle.None;
            this.splitGraph.Visible = false;
            this.splitGraph.Dock = DockStyle.None;

            this.buttonCompare.Visible = false;
            this.buttonAddtoWatch.Visible = false;

            this.groupBoxReport.Visible = false;
            this.buttonПХ_Ампл.Visible = false;
            this.buttonПХ_Фаза.Visible = false;
            this.buttonАДН.Visible = false;
            this.buttonКУ.Visible = false;
            this.buttonФДН.Visible = false;
            this.buttonСДНМ.Visible = false;
        }
        /// <summary>
        /// тип КУ
        /// </summary>
       public enum KYTupeEnum
        {
            Sum=0,
            main=1,
            Cross=2
        }
        /// <summary>
        /// массив форм для предпросмотра графиков
        /// </summary>
        Dictionary<int, CompareGraphForm> WathFormDictionary = new Dictionary<int, CompareGraphForm>();
        /// <summary>
        /// используется для временного хранения загруженных данных для графика
        /// </summary>
        Series TempGraphSeries = null;
        /// <summary>
        /// номер типа результата для добавления к нужной форме предпросмотра
        /// </summary>
        int? TupeGraphNumber = null;
        #endregion

        /// <summary>
        /// отображение, изменение или создание в зависимости от созданой строки
        /// </summary>
        /// <param name="Tag">объект row</param>
        private void Factory(object Tag)
        {
            this.SuspendLayout();

            if (Tag is DataSet1.mainRow)
            {
                this.UnvizibleElements();
                DataSet1.mainRow tempMain = Tag as DataSet1.mainRow;
                this.label1.Text = "это main " + tempMain.id.ToString();

                #region опции main
                this.mainOptionsUserControl1.MainResult = this.GetMainOptionsClass(tempMain);

                if (!this.mainOptionsUserControl1.Visible)
                {
                    this.UnvizibleElements();
                    this.mainOptionsUserControl1.Dock = DockStyle.Fill;
                    this.mainOptionsUserControl1.Visible = true;
                }

                #endregion
            }
            else
            {
                if (Tag is DataSet1.AntennasRow)
                {
                    DataSet1.AntennasRow tempAnten = Tag as DataSet1.AntennasRow;
                    this.label1.Text = "Это Антенна или зонд " + tempAnten.id.ToString();

                    #region загрузка параметров антенны

                    //выбрать антенну
                    this.antennOptionsUserControl1.SelectedAntenn = this.AntennRowToClass(tempAnten);

                    #endregion

                    if (!this.antennOptionsUserControl1.Visible)
                    {
                        this.UnvizibleElements();
                        //this.antennOptionsUserControl1.LockAll();
                        this.antennOptionsUserControl1.Dock = DockStyle.Fill;
                        this.antennOptionsUserControl1.Visible = true;
                    }
                }
                else
                {
                    if (Tag is DataSet1.Connection_devicesRow)
                    {
                        DataSet1.Connection_devicesRow tempdevice = Tag as DataSet1.Connection_devicesRow;
                        this.label1.Text = "Это девайс " + tempdevice.id.ToString();

                        #region загрузка параметров девайса
                        this.connection_Devices_UserControl1.SelectedDevice = this.ConnectionDeviceRowToClass(tempdevice);
                        #endregion

                        if (!this.connection_Devices_UserControl1.Visible)
                        {
                            this.UnvizibleElements();
                            //this.connection_Devices_UserControl1.LockAll();
                            this.connection_Devices_UserControl1.Dock = DockStyle.Fill;
                            this.connection_Devices_UserControl1.Visible = true;
                        }
                    }
                    else
                    {
                        if (Tag is DataSet1.ResultTypesRow)
                        {
                            this.UnvizibleElements();
                            DataSet1.ResultTypesRow tempResultTupe = Tag as DataSet1.ResultTypesRow;
                            this.label1.Text = "Это тип результата " + tempResultTupe.id.ToString();
                        }
                        else
                        {
                            if (Tag is DataSet1.ЧастотыRow)
                            {
                                DataSet1.ЧастотыRow tempFreg = Tag as DataSet1.ЧастотыRow;
                                this.label1.Text = "Это частота " + tempFreg.id.ToString();

                                #region рисование графика
                                List<Series> serCol = new List<Series>();
                                //создание частотного элемента
                                FrequencyElementClass tempFrequencyElement = this.GetFrequencyElementByЧастотыRow(tempFreg);
                                this.TempGraphSeries = tempFrequencyElement.GetAmplSeries();
                                //добавление полного имени
                                TempGraphSeries.Name = this.GetFullNameByFrequencyRow(tempFreg);
                                serCol.Add(this.TempGraphSeries);

                                this.previewGraphControl1.CreateGraph(serCol);

                                DataSet1.mainRow tempmain = this.GetMainRowByFrequencyRow(tempFreg);

                                #region отображение контролов с рассчитываемыми данными и отображение кнопок создания отчётов
                                switch (tempmain.id_Типа)
                                {
                                    case (int)MeasurementTypeEnum.АДН_ФДН_Азимут:
                                        {
                                            if (!resultDNUserControl1.Visible)
                                            {
                                                this.UnvizibleElements();

                                                this.splitGraph.Panel2MinSize = this.resultDNUserControl1.MinimumSize.Width;
                                                this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultDNUserControl1.MinimumSize.Width;

                                                this.previewGraphControl1.Dock = DockStyle.Fill;
                                                this.previewGraphControl1.Visible = true;

                                                this.splitGraph.Dock = DockStyle.Fill;
                                                this.splitGraph.Visible = true;

                                                this.resultDNUserControl1.Dock = DockStyle.Fill;
                                                this.resultDNUserControl1.Visible = true;

                                                //добавление класса расчитываемого результата
                                                tempFrequencyElement.CalculationResults = new ResultDNClass();
                                                //заполнение контрола с рзультатами
                                                this.resultDNUserControl1.ResultDN = tempFrequencyElement.CalculationResults as ResultDNClass;
                                            }
                                            #region отображаем кнопки создания отчёта
                                            this.groupBoxReport.Visible = true;
                                            this.buttonПХ_Ампл.Visible = false;
                                            this.buttonПХ_Фаза.Visible = false;
                                            this.buttonАДН.Visible = true;
                                            this.buttonКУ.Visible = false;
                                            this.buttonФДН.Visible = true;
                                            this.buttonСДНМ.Visible = false;

                                            this.buttonАДН.Tag = tempFreg;
                                            this.buttonФДН.Tag = tempFreg;
                                            #endregion
                                            break;
                                        }
                                    case (int)MeasurementTypeEnum.АДН_ФДН_Поляриз:
                                        {
                                            if (!resultDNUserControl1.Visible)
                                            {
                                                this.UnvizibleElements();

                                                this.splitGraph.Panel2MinSize = this.resultDNUserControl1.MinimumSize.Width;
                                                this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultDNUserControl1.MinimumSize.Width;

                                                this.previewGraphControl1.Dock = DockStyle.Fill;
                                                this.previewGraphControl1.Visible = true;

                                                this.splitGraph.Dock = DockStyle.Fill;
                                                this.splitGraph.Visible = true;

                                                this.resultDNUserControl1.Dock = DockStyle.Fill;
                                                this.resultDNUserControl1.Visible = true;

                                                //добавление класса расчитываемого результата
                                                tempFrequencyElement.CalculationResults = new ResultDNClass();
                                                //заполнение контрола с рзультатами
                                                this.resultDNUserControl1.ResultDN = tempFrequencyElement.CalculationResults as ResultDNClass;
                                            }
                                            #region отображаем кнопки создания отчёта
                                            this.groupBoxReport.Visible = true;
                                            this.buttonПХ_Ампл.Visible = false;
                                            this.buttonПХ_Фаза.Visible = false;
                                            this.buttonАДН.Visible = true;
                                            this.buttonКУ.Visible = false;
                                            this.buttonФДН.Visible = true;
                                            this.buttonСДНМ.Visible = false;

                                            this.buttonАДН.Tag = tempFreg;
                                            this.buttonФДН.Tag = tempFreg;
                                            #endregion
                                            break;
                                        }
                                    case (int)MeasurementTypeEnum.ПХ:
                                        {
                                            if (!resultPHUserControl1.Visible)
                                            {
                                                this.UnvizibleElements();

                                                this.splitGraph.Panel2MinSize = this.resultPHUserControl1.MinimumSize.Width;
                                                this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultPHUserControl1.MinimumSize.Width;

                                                this.previewGraphControl1.Dock = DockStyle.Fill;
                                                this.previewGraphControl1.Visible = true;

                                                this.splitGraph.Dock = DockStyle.Fill;
                                                this.splitGraph.Visible = true;

                                                this.resultPHUserControl1.Dock = DockStyle.Fill;
                                                this.resultPHUserControl1.Visible = true;

                                                //добавление класса расчитываемого результата
                                                tempFrequencyElement.CalculationResults=new ResultPHClass();
                                                //заполнение контрола с рзультатами
                                                this.resultPHUserControl1.ResultPH = tempFrequencyElement.CalculationResults as ResultPHClass;
                                            }
                                            #region отображаем кнопки создания отчёта
                                            this.groupBoxReport.Visible = true;
                                            this.buttonПХ_Ампл.Visible = true;
                                            this.buttonПХ_Фаза.Visible = true;
                                            this.buttonАДН.Visible = false;
                                            this.buttonКУ.Visible = false;
                                            this.buttonФДН.Visible = false;
                                            this.buttonСДНМ.Visible = false;

                                            buttonПХ_Ампл.Tag = tempFreg;
                                            buttonПХ_Фаза.Tag = tempFreg;
                                            #endregion
                                            break;
                                        }
                                    case (int)MeasurementTypeEnum.СДНМ_Азимут:
                                        {
                                            if (!resultSDNMUserControl1.Visible)
                                            {
                                                this.UnvizibleElements();

                                                this.splitGraph.Panel2MinSize = this.resultSDNMUserControl1.MinimumSize.Width;
                                                this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultSDNMUserControl1.MinimumSize.Width;

                                                this.previewGraphControl1.Dock = DockStyle.Fill;
                                                this.previewGraphControl1.Visible = true;

                                                this.splitGraph.Dock = DockStyle.Fill;
                                                this.splitGraph.Visible = true;

                                                this.resultSDNMUserControl1.Dock = DockStyle.Fill;
                                                this.resultSDNMUserControl1.Visible = true;

                                                //добавление класса расчитываемого результата
                                                tempFrequencyElement.CalculationResults = new ResultSDNMClass();
                                                //заполнение контрола с рзультатами
                                                this.resultSDNMUserControl1.ResultSDNM = tempFrequencyElement.CalculationResults as ResultSDNMClass;
                                            }
                                            #region отображаем кнопки создания отчёта
                                            this.groupBoxReport.Visible = true;
                                            this.buttonПХ_Ампл.Visible = false;
                                            this.buttonПХ_Фаза.Visible = false;
                                            this.buttonАДН.Visible = true;
                                            this.buttonКУ.Visible = false;
                                            this.buttonФДН.Visible = true;
                                            this.buttonСДНМ.Visible = true;

                                            this.buttonСДНМ.Tag = tempFreg;
                                            this.buttonФДН.Tag = tempFreg;
                                            this.buttonАДН.Tag = tempFreg;
                                            #endregion
                                            break;
                                        }
                                    case (int)MeasurementTypeEnum.СДНМ_Поляриз:
                                        {
                                            if (!resultSDNMUserControl1.Visible)
                                            {
                                                this.UnvizibleElements();

                                                this.splitGraph.Panel2MinSize = this.resultSDNMUserControl1.MinimumSize.Width;
                                                this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultSDNMUserControl1.MinimumSize.Width;

                                                this.previewGraphControl1.Dock = DockStyle.Fill;
                                                this.previewGraphControl1.Visible = true;

                                                this.splitGraph.Dock = DockStyle.Fill;
                                                this.splitGraph.Visible = true;

                                                this.resultSDNMUserControl1.Dock = DockStyle.Fill;
                                                this.resultSDNMUserControl1.Visible = true;

                                                //добавление класса расчитываемого результата
                                                tempFrequencyElement.CalculationResults = new ResultSDNMClass();
                                                //заполнение контрола с рзультатами
                                                this.resultSDNMUserControl1.ResultSDNM = tempFrequencyElement.CalculationResults as ResultSDNMClass;
                                            }
                                            #region отображаем кнопки создания отчёта
                                            this.groupBoxReport.Visible = true;
                                            this.buttonПХ_Ампл.Visible = false;
                                            this.buttonПХ_Фаза.Visible = false;
                                            this.buttonАДН.Visible = true;
                                            this.buttonКУ.Visible = false;
                                            this.buttonФДН.Visible = true;
                                            this.buttonСДНМ.Visible = true;

                                            this.buttonСДНМ.Tag = tempFreg;
                                            this.buttonФДН.Tag = tempFreg;
                                            this.buttonАДН.Tag = tempFreg;
                                            #endregion
                                            break;
                                        }
                                    default:
                                        if (!this.previewGraphControl1.Visible || this.resultDNUserControl1.Visible || this.resultPHUserControl1.Visible || this.resultSDNMUserControl1.Visible)
                                        {
                                            this.UnvizibleElements();

                                            this.splitGraph.Panel2MinSize = 0;
                                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width;

                                            this.previewGraphControl1.Dock = DockStyle.Fill;
                                            this.previewGraphControl1.Visible = true;

                                            this.splitGraph.Dock = DockStyle.Fill;
                                            this.splitGraph.Visible = true;
                                        }
                                        break;
                                }
                                #endregion

                                #region получить тип результата для добавление в форму сравнения
                                //проверка: зарегистрирован ли тип измерения в базе
                                DataSet1.ResultTypesRow[] tempTupeResult = tempmain.GetResultTypesRows();
                                if (tempTupeResult.Length != 0)
                                {
                                    this.TupeGraphNumber = tempTupeResult[0].id;

                                    bool addFlag = false;
                                    for (int i = 0; i < WathFormDictionary[tempTupeResult[0].id].GraphSeriesList.Count; i++)
                                    {
                                        if (WathFormDictionary[tempTupeResult[0].id].GraphSeriesList[i].Name == TempGraphSeries.Name)
                                        {
                                            buttonCompare.Visible = true;
                                            buttonAddtoWatch.Visible = false;

                                            addFlag = true;
                                            break;
                                        }
                                    }

                                    if (!addFlag)
                                    {
                                        buttonCompare.Visible = false;
                                        buttonAddtoWatch.Visible = true;
                                    }
                                }
                                else
                                {
                                    this.TupeGraphNumber = null;
                                    buttonCompare.Visible = false;
                                    buttonAddtoWatch.Visible = false;
                                }
                                #endregion

                                #endregion
                            }
                            else
                            {

                                if (Tag is DataSet1.Список_результатовRow)
                                {
                                    DataSet1.Список_результатовRow temp = Tag as DataSet1.Список_результатовRow;
                                    this.label1.Text = "Список результатов поляризаций " + temp.id.ToString();
                                }
                                else
                                {
                                    //это КУ
                                    if (Tag is List<object>)
                                    {
                                        List<object> temp = Tag as List<object>;
                                        this.label1.Text = "Список результатов поляризаций " + temp[0];

                                        #region рисование графика
                                        List<Series> serCol = new List<Series>();
                                        this.TempGraphSeries = this.GetSiriesKYByЧастотыRow((KYTupeEnum)temp[0], temp[1] as DataSet1.ЧастотыRow);
                                        //this.TempGraphSeries = this.GetSiriesKYByTableName(temp[1], temp[0], temp[2]);
                                        serCol.Add(this.TempGraphSeries);

                                        this.previewGraphControl1.CreateGraph(serCol, true);

                                        if (!this.previewGraphControl1.Visible||this.resultDNUserControl1.Visible||this.resultPHUserControl1.Visible||this.resultSDNMUserControl1.Visible)
                                        {
                                            this.UnvizibleElements();

                                            this.splitGraph.Panel2MinSize = 0;
                                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width;

                                            this.previewGraphControl1.Dock = DockStyle.Fill;
                                            this.previewGraphControl1.Visible = true;

                                            this.splitGraph.Dock = DockStyle.Fill;
                                            this.splitGraph.Visible = true;
                                        }

                                        #region получить тип результата для отображения кнопок создания отчётов

                                        this.TupeGraphNumber = 0;

                                        bool addFlag = false;
                                        for (int i = 0; i < WathFormDictionary[0].GraphSeriesList.Count; i++)
                                        {
                                            if (WathFormDictionary[0].GraphSeriesList[i].Name == TempGraphSeries.Name)
                                            {
                                                buttonCompare.Visible = true;
                                                buttonAddtoWatch.Visible = false;

                                                addFlag = true;
                                                break;
                                            }
                                        }

                                        if (!addFlag)
                                        {
                                            buttonCompare.Visible = false;
                                            buttonAddtoWatch.Visible = true;
                                        }
                                        #region отображаем кнопки создания отчёта
                                        this.groupBoxReport.Visible = true;
                                        this.buttonПХ_Ампл.Visible = false;
                                        this.buttonПХ_Фаза.Visible = false;
                                        this.buttonАДН.Visible = false;
                                        this.buttonКУ.Visible = true;
                                        this.buttonФДН.Visible = false;
                                        this.buttonСДНМ.Visible = false;

                                        buttonКУ.Tag = temp;
                                        #endregion
                                        #endregion
                                        #endregion
                                    }

                                    else
                                    {
                                        this.label1.Text = "Это неизвестный тип строки";

                                        this.UnvizibleElements();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.ResumeLayout();
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.connection_Devices_UserControl1.UnLockAll();
                this.antennOptionsUserControl1.UnLockAll();
            }
            else
            {
                this.connection_Devices_UserControl1.LockAll();
                this.antennOptionsUserControl1.LockAll();
            }
        }

        #region работа с предпросмотром
        private void buttonAddtoWatch_Click(object sender, EventArgs e)
        {
            AddToGraphFormDictionary(this.TempGraphSeries,Convert.ToInt32(TupeGraphNumber));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WathFormDictionary[Convert.ToInt32(this.TupeGraphNumber)].ShowDialog();
        }

        protected void AddToGraphFormDictionary(Series newSeries,int ResultTupe)
        {
            bool addWatch = true;
            foreach (Series tempser in this.WathFormDictionary[ResultTupe].GraphSeriesList)
            {
                if (tempser.Name == TempGraphSeries.Name)
                {
                    addWatch = false;
                    break;
                }
            }
            if (addWatch)
            {
                this.WathFormDictionary[ResultTupe].GraphSeriesList.Add(newSeries);
                this.buttonAddtoWatch.Visible = false;
                this.buttonCompare.Visible = true;
            }


        }
        #endregion

        #region работа с отчётами
        /// <summary>
        /// заполнить общие (базовые) данные для измерения
        /// </summary>
        /// <param name="ResultType"></param>
        /// <param name="tempFreqRow"></param>
        private void FillResultType_by_ЧастотыRow(ResultType_MAINClass ResultType, DataSet1.ЧастотыRow tempFreqRow)
        {
            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);
            //получить опции
            MainOptionsClass tempmainOptions = this.GetMainOptionsClass(tempMainRow);
            //получить антенну
            AntennOptionsClass tempanten = this.MainRowToAntennClass(tempMainRow);

            //добавть полученные данные
            ResultType.Antenn = tempanten;
            ResultType.MainOptions = tempmainOptions;
        }

        private void buttonПХ_Ампл_Click(object sender, EventArgs e)
        {
            DataSet1.ЧастотыRow tempFreqRow=buttonПХ_Ампл.Tag as DataSet1.ЧастотыRow;

            //создаём объект результата ПХ
            ResultTypeПХ result = new ResultTypeПХ();

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_ЧастотыRow(result, tempFreqRow);

            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            //получить измеренные данные
            PolarizationElementClass tempPolariz = new PolarizationElementClass();
            FrequencyElementClass tempFrequency = this.GetFrequencyElementByЧастотыRow(tempFreqRow);
            //добавляем объект рассчитываемого результата
            tempFrequency.CalculationResults = new OptionsClass.ResultClass.ResultPHClass();
            tempPolariz.FrequencyElements.Add(tempFrequency);
            result.MeasurementData.PolarizationElements.Add(tempPolariz);

            //Указываем что это амплитуда
            Report_Sheild_M_Interfaces.IPolarizationDiagram tempRportInterfase = result as Report_Sheild_M_Interfaces.IPolarizationDiagram;
            tempRportInterfase.IsPhaseDiagram = false;

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result);
            protForm.ShowDialog();
        }


        private void buttonПХ_Фаза_Click(object sender, EventArgs e)
        {
            DataSet1.ЧастотыRow tempFreqRow = buttonПХ_Фаза.Tag as DataSet1.ЧастотыRow;

            //создаём объект результата ПХ
            ResultTypeПХ result = new ResultTypeПХ();

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_ЧастотыRow(result, tempFreqRow);

            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            //получить измеренные данные
            PolarizationElementClass tempPolariz = new PolarizationElementClass();
            FrequencyElementClass tempFrequency = this.GetFrequencyElementByЧастотыRow(tempFreqRow);
            //добавляем объект рассчитываемого результата
            tempFrequency.CalculationResults = new OptionsClass.ResultClass.ResultPHClass();
            tempPolariz.FrequencyElements.Add(tempFrequency);
            result.MeasurementData.PolarizationElements.Add(tempPolariz);

            //Указываем что это фаза
            Report_Sheild_M_Interfaces.IPolarizationDiagram tempRportInterfase = result as Report_Sheild_M_Interfaces.IPolarizationDiagram;
            tempRportInterfase.IsPhaseDiagram = true;

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result);
            protForm.ShowDialog();
        }

        private void buttonАДН_Click(object sender, EventArgs e)
        {
            DataSet1.ЧастотыRow tempFreqRow = buttonАДН.Tag as DataSet1.ЧастотыRow;

            //создаём объект результата ПХ
            ResultTypeClassДН result = new ResultTypeClassДН();

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_ЧастотыRow(result, tempFreqRow);

            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            //получить измеренные данные
            PolarizationElementClass tempPolariz = new PolarizationElementClass();
            FrequencyElementClass tempFrequency = this.GetFrequencyElementByЧастотыRow(tempFreqRow);
            //добавляем объект рассчитываемого результата
            tempFrequency.CalculationResults = new OptionsClass.ResultClass.ResultDNClass();
            tempPolariz.FrequencyElements.Add(tempFrequency);
            result.MeasurementData.PolarizationElements.Add(tempPolariz);

            
            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result.for_Report_AmplitudeDiagram);
            protForm.ShowDialog();
        }

        private void buttonФДН_Click(object sender, EventArgs e)
        {
            DataSet1.ЧастотыRow tempFreqRow = buttonФДН.Tag as DataSet1.ЧастотыRow;

            //создаём объект результата ПХ
            ResultTypeClassДН result = new ResultTypeClassДН();

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_ЧастотыRow(result, tempFreqRow);

            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            //получить измеренные данные
            PolarizationElementClass tempPolariz = new PolarizationElementClass();
            FrequencyElementClass tempFrequency = this.GetFrequencyElementByЧастотыRow(tempFreqRow);
            //добавляем объект рассчитываемого результата
            tempFrequency.CalculationResults = new OptionsClass.ResultClass.ResultDNClass();
            tempPolariz.FrequencyElements.Add(tempFrequency);
            result.MeasurementData.PolarizationElements.Add(tempPolariz);


            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result.for_Report_PhaseDiagram);
            protForm.ShowDialog();
        }


        private void buttonСДНМ_Click(object sender, EventArgs e)
        {
            DataSet1.ЧастотыRow tempFreqRow = buttonСДНМ.Tag as DataSet1.ЧастотыRow;

            //создаём объект результата ПХ
            ResultTypeClassСДНМ result = new ResultTypeClassСДНМ();

            //заполнить общие (базовые) данные для измерения
            this.FillResultType_by_ЧастотыRow(result, tempFreqRow);

            //получить mainRow
            DataSet1.mainRow tempMainRow = this.GetMainRowByFrequencyRow(tempFreqRow);

            //получить измеренные данные
            result.MeasurementData = this.GetMeasurementDataByЧастотыRow_for_СДНМ(tempFreqRow);
            
            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result);
            protForm.ShowDialog();
        }
        #endregion




        FrequencyElementClass getfull()
        {
            FrequencyElementClass fe1 = new FrequencyElementClass(55555);


            for (int i = 0; i < 720; i++)
            {
                fe1.AddResultElement(1, 2, 3);
            }

            return fe1;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            

            PolarizationElementClass po1 = new PolarizationElementClass();
            po1.Polarization = 99;
            po1.FrequencyElements.Add(getfull());

            PolarizationElementClass po2 = new PolarizationElementClass();
            po2.Polarization = 11;
            po2.FrequencyElements.Add(getfull());

            
            ResultTypeClassСДНМ result =new ResultTypeClassСДНМ();
            result.Antenn = this.antennOptionsUserControl1.AllAntenn[1];
            result.Zond = this.antennOptionsUserControl1.AllAntenn[2];
            result.MeasurementData.PolarizationElements.Add(po1);
            result.MeasurementData.PolarizationElements.Add(po2);

            result.MainOptions.Date = DateTime.Now;
            result.MainOptions.Descriptions = "Это тест button1 descriptions";
            result.MainOptions.Name = "Это тест button1 name";
            result.MainOptions.Operator.Fam = "fam";
            result.MainOptions.Operator.id = 1;
            result.MainOptions.Parameters.MeasurementResultType = MeasurementTypeEnum.СДНМ_Поляриз;
            result.MainOptions.Parameters.StartOPU_W = 50;
            result.MainOptions.Parameters.StopTower_W = 100;

            result.MainOptions.Parameters.SegmentTable.Add(new ZVB14_Main.SegmentTableElementOptionsClass(100, 1000, 10));

            this.button1.Text= this.SaveMainTable(result).ToString();
        }

               

        

       
        
    }

#warning Тип измерения  При изменении id_Типа у типов измерений - править сдесь
    /// <summary>
    /// Тип измерения  При изменении id_Типа у типов измерений - править сдесь
    /// </summary>
    public enum MeasurementTypeEnum
    {
        КУ = 0,
        ПХ = 1,
        АДН_ФДН_Азимут = 2,
        АДН_ФДН_Поляриз = 3,
        СДНМ_Азимут = 4,
        СДНМ_Поляриз = 5
    }
}
