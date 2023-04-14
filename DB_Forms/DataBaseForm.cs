using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
//using System.IO;
//using GeneralInterfaces;
using ResultOptionsClassLibrary;
using ResultTypesClassLibrary;
using AncillaryDBForms;
using DB_Interface;
using HendBandFormApplication;
using ChartScaleCalculatorClassLibrary;
//using ZVBOptionsClassLibrary;

namespace DataBase_Sheild_M
{
    public partial class DataBaseForm : Form
    {
        #region конструкторы и глобальные переменные

        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public DataBaseForm()
        {
            InitializeComponent();

            //выводим версию в заголовок 
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            this.Text += " от " + buildDate.ToString();
        }

        public DataBaseForm(I_ARM_Form NewDB_Loader)
            : this()
        {
            DB_Loader = NewDB_Loader;
        }

        public void DataBaseForm_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                InitializeForm();
            }
        }

        protected bool _IsInitializForm = false;

        public void InitializeForm()
        {
            if (!_IsInitializForm)
            {
                _IsInitializForm = true;

                HeandBand.TextBig = "Загрузка базы данных\r\nЭкран-М";
                HeandBand.BackColor = Color.Yellow;

                AddOwnedForm(HeandBand);
                HeandBand.Show();
                Application.DoEvents();

                // DB_Loader.InitializeDB();
                try
                {
                    #region Создание форм сравнения

                    //для КУ заблокировать полярный график и рассчётный модуль
                    CompareGraphForm CompareKY = new CompareGraphForm("КУ", true);

                    //для ПХ заблокировать рассчётный модуль
                    CompareGraphForm CompareПХ = new CompareGraphForm("ПХ", false, DB_Loader, true);

                    //все остальные графики
                    CompareGraphForm CompareALL = new CompareGraphForm("ДН", false, DB_Loader, false);


                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.Коэффицент_усиления, CompareKY);
                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.Поляризационная_диаграмма, CompareПХ);

                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.ДН_Азимут, CompareALL);
                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.ДН_Меридиан, CompareALL);
                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.Суммарная_ДН_Азимут, CompareALL);
                    this.WathFormDictionary.Add((int)MeasurementTypeEnum.Суммарная_ДН_Меридиан, CompareALL);

                    #endregion

                    this.mainOptionsUserControl1.SaverToDB = DB_Loader;

                    DateTime StopDate = DateTime.Now;
                    DateTime StartDate = StopDate.AddMonths(-3);

                    this.DTPickerStart.Value = StartDate;
                    this.DTPickerStop.Value = StopDate;

                    DB_Loader.LoadNodesToTree(ref treeView2, ref treeView1, StartDate, StopDate, false);


                    #region подпись на события сохранения и выбора нодов
                    this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
                    this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

                    this.treeView1.BeforeSelect += new TreeViewCancelEventHandler(treeView1_BeforeSelect);
                    this.treeView2.BeforeSelect += new TreeViewCancelEventHandler(treeView1_BeforeSelect);
                    #endregion

                    #region подпись на события загрузки спец графика

                    mainOptionsUserControl1.linkLabelGraphLoad.Click += new EventHandler(linkLabelGraphLoad_Click);

                    #endregion

                    this.ModifyMode(this, new EventArgs());
                    HeandBand.Hide();
                }
                catch (Exception ex)
                {
                    HeandBand.Hide();
                    MessageBox.Show(this, string.Format("Не удалось загрузить результаты измерений. \n{0}", ex.Message), "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private HeadBandForm HeandBand = new HeadBandForm();

        #endregion

        //Интерфейс связи с загрузщиком
        public I_ARM_Form DB_Loader = null;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.ForeColor = Color.Red;
            this.FactoryNew(e.Node.Tag);
        }

        void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.TreeView.SelectedNode != null)
                e.Node.TreeView.SelectedNode.ForeColor = Color.Black;
        }


        #region Вспомогательные переменные и функции

        private void UnvizibleElements()
        {
            this.splitContainerGraph.Visible = false;
            this.mainOptionsUserControl1.Visible = false;
            this.resultDNUserControl1.Visible = false;
            this.resultDNUserControl1.Dock = DockStyle.None;
            this.resultPHUserControl1.Visible = false;
            this.resultPHUserControl1.Dock = DockStyle.None;
            this.resultSDNMUserControl1.Visible = false;
            this.resultSDNMUserControl1.Dock = DockStyle.None;
            this.resultKYUserControl1.Visible = false;
            this.resultKYUserControl1.Dock = DockStyle.None;
            this.splitGraph.Visible = false;
            this.splitGraph.Dock = DockStyle.None;

            this.buttonCompare.Visible = false;
            this.buttonAddtoWatch.Visible = false;
            this.buttonShowTable.Visible = false;

            this.groupBoxReport.Visible = false;
            this.buttonПХ_Ампл.Visible = false;
            this.buttonПХ_Фаза.Visible = false;
            this.buttonАДН.Visible = false;
            this.buttonКУ.Visible = false;
            this.buttonФДН.Visible = false;
            this.buttonСДНМ.Visible = false;

            this.buttonXML.Visible = false;
            this.buttonTXT.Visible = false;
            this.buttonExel.Visible = false;
            this.buttonDelResult.Visible = false;
            this.buttonDelAnten.Visible = false;
            this.buttonPDF.Visible = false;


            this.buttonHidefreq.Enabled = false;
        }

        protected void ShowCalculationResult(ResultType_MAINClass Result)
        {
            #region отображение контролов с рассчитываемыми данными и отображение кнопок создания отчётов

            switch (Result.MainOptions.MeasurementResultType)
            {
                case MeasurementTypeEnum.ДН_Меридиан:
                    {
                        if (!resultDNUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultDNUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultDNUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultDNUserControl1.Dock = DockStyle.Fill;
                            this.resultDNUserControl1.Visible = true;
                        }


                        //заполнение контрола с рзультатами
                        this.resultDNUserControl1.Result = Result;

                        break;
                    }
                case MeasurementTypeEnum.ДН_Азимут:
                    {
                        if (!resultDNUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultDNUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultDNUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultDNUserControl1.Dock = DockStyle.Fill;
                            this.resultDNUserControl1.Visible = true;
                        }


                        //заполнение контрола с рзультатами
                        this.resultDNUserControl1.Result = Result;

                        break;
                    }
                case MeasurementTypeEnum.Поляризационная_диаграмма:
                    {
                        if (!resultPHUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultPHUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultPHUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultPHUserControl1.Dock = DockStyle.Fill;
                            this.resultPHUserControl1.Visible = true;
                        }


                        //заполнение контрола с рзультатами
                        this.resultPHUserControl1.Result = Result;
                        break;
                    }
                case MeasurementTypeEnum.Суммарная_ДН_Меридиан:
                    {
                        if (!resultSDNMUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultSDNMUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultSDNMUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultSDNMUserControl1.Dock = DockStyle.Fill;
                            this.resultSDNMUserControl1.Visible = true;
                        }

                        //заполнение контрола с рзультатами
                        this.resultSDNMUserControl1.Result = Result;
                        break;
                    }
                case MeasurementTypeEnum.Суммарная_ДН_Азимут:
                    {
                        if (!resultSDNMUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultSDNMUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultSDNMUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultSDNMUserControl1.Dock = DockStyle.Fill;
                            this.resultSDNMUserControl1.Visible = true;
                        }


                        //заполнение контрола с рзультатами
                        this.resultSDNMUserControl1.Result = Result;
                        break;
                    }
                case MeasurementTypeEnum.Коэффицент_усиления:
                    {
                        if (!resultKYUserControl1.Visible)
                        {
                            this.UnvizibleElements();

                            this.splitGraph.Panel2MinSize = this.resultKYUserControl1.MinimumSize.Width;
                            this.splitGraph.SplitterDistance = this.splitGraph.Size.Width - this.resultKYUserControl1.MinimumSize.Width;

                            this.splitContainerGraph.Dock = DockStyle.Fill;
                            this.splitContainerGraph.Visible = true;

                            this.splitGraph.Dock = DockStyle.Fill;
                            this.splitGraph.Visible = true;

                            this.resultKYUserControl1.Dock = DockStyle.Fill;
                            this.resultKYUserControl1.Visible = true;
                        }

                        this.resultKYUserControl1.Result = Result;

                        break;
                    }
                default:
                    if (!this.splitContainerGraph.Visible || this.resultDNUserControl1.Visible || this.resultPHUserControl1.Visible || this.resultSDNMUserControl1.Visible)
                    {
                        this.UnvizibleElements();

                        this.splitGraph.Panel2MinSize = 0;
                        this.splitGraph.SplitterDistance = this.splitGraph.Size.Width;

                        this.splitContainerGraph.Dock = DockStyle.Fill;
                        this.splitContainerGraph.Visible = true;

                        this.splitGraph.Dock = DockStyle.Fill;
                        this.splitGraph.Visible = true;
                    }
                    break;
            }
            #endregion

            ShowCompareButtons(Result);
        }

        protected void ShowCompareButtons(ResultType_MAINClass Result)
        {
            #region получить тип результата для добавление в форму сравнения


            this.TupeGraphNumber = (int)Result.MainOptions.MeasurementResultType;
            this.TempResult = Result;

            if (WathFormDictionary[(int)Result.MainOptions.MeasurementResultType].ContainsInCompareResults(this.TempResult))
            {
                buttonCompare.Visible = true;
                buttonAddtoWatch.Visible = false;
            }
            else
            {
                buttonCompare.Visible = false;
                buttonAddtoWatch.Visible = true;
            }

            #endregion

            buttonShowTable.Visible = true;
        }

        protected void ShowReportButtons(MainOptionsClass Result, DBLoaderPackClass Pack)
        {
            #region отображение контролов с рассчитываемыми данными и отображение кнопок создания отчётов

            switch (Result.MeasurementResultType)
            {
                case MeasurementTypeEnum.ДН_Меридиан:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = false;
                        this.buttonПХ_Фаза.Visible = false;
                        this.buttonАДН.Visible = true;
                        this.buttonКУ.Visible = false;
                        this.buttonФДН.Visible = true;
                        this.buttonСДНМ.Visible = false;
                        this.buttonUnionDN.Visible = false;

                        this.buttonАДН.Tag = Pack;
                        this.buttonФДН.Tag = Pack;
                        #endregion
                        break;
                    }
                case MeasurementTypeEnum.ДН_Азимут:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = false;
                        this.buttonПХ_Фаза.Visible = false;
                        this.buttonАДН.Visible = true;
                        this.buttonКУ.Visible = false;
                        this.buttonФДН.Visible = true;
                        this.buttonСДНМ.Visible = false;
                        this.buttonUnionDN.Visible = false;

                        this.buttonАДН.Tag = Pack;
                        this.buttonФДН.Tag = Pack;
                        #endregion
                        break;
                    }
                case MeasurementTypeEnum.Поляризационная_диаграмма:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = true;
                        this.buttonПХ_Фаза.Visible = true;
                        this.buttonАДН.Visible = false;
                        this.buttonКУ.Visible = false;
                        this.buttonФДН.Visible = false;
                        this.buttonСДНМ.Visible = false;
                        this.buttonUnionDN.Visible = false;

                        buttonПХ_Ампл.Tag = Pack;
                        buttonПХ_Фаза.Tag = Pack;
                        #endregion
                        break;
                    }
                case MeasurementTypeEnum.Суммарная_ДН_Меридиан:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = false;
                        this.buttonПХ_Фаза.Visible = false;
                        this.buttonАДН.Visible = false;
                        this.buttonКУ.Visible = false;
                        this.buttonФДН.Visible = false;
                        this.buttonСДНМ.Visible = true;
                        this.buttonUnionDN.Visible = false;

                        this.buttonСДНМ.Tag = Pack;
                        #endregion
                        break;
                    }
                case MeasurementTypeEnum.Суммарная_ДН_Азимут:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = false;
                        this.buttonПХ_Фаза.Visible = false;
                        this.buttonАДН.Visible = false;
                        this.buttonКУ.Visible = false;
                        this.buttonФДН.Visible = false;
                        this.buttonСДНМ.Visible = true;
                        this.buttonUnionDN.Visible = false;

                        this.buttonСДНМ.Tag = Pack;
                        #endregion
                        break;
                    }
                case MeasurementTypeEnum.Коэффицент_усиления:
                    {
                        #region отображаем кнопки создания отчёта
                        this.groupBoxReport.Visible = true;
                        this.buttonПХ_Ампл.Visible = false;
                        this.buttonПХ_Фаза.Visible = false;
                        this.buttonАДН.Visible = false;
                        this.buttonКУ.Visible = true;
                        this.buttonФДН.Visible = false;
                        this.buttonСДНМ.Visible = false;
                        this.buttonUnionDN.Visible = false;

                        buttonКУ.Tag = Pack;
                        #endregion

                        break;
                    }
                default:
                    break;
            }
            #endregion

            #region если это рассчитанный результат

            if (Result.CalculationResultType != CalculationResultTypeEnum.None)
            {
                #region отображаем кнопки создания отчёта
                this.groupBoxReport.Visible = true;
                this.buttonПХ_Ампл.Visible = false;
                this.buttonПХ_Фаза.Visible = false;
                this.buttonАДН.Visible = false;
                this.buttonКУ.Visible = false;
                this.buttonФДН.Visible = false;
                this.buttonСДНМ.Visible = false;
                this.buttonUnionDN.Visible = true;

                this.buttonUnionDN.Tag = Pack;

                #endregion
            }
            #endregion
        }


        private void ModifyMode(object sender, EventArgs e)
        {
            this.mainOptionsUserControl1.LockUnLockAll(!this.checkBoxEditMode.Checked);

            buttonCLON.Visible = checkBoxEditMode.Checked;
        }


        /// <summary>
        /// массив форм для предпросмотра графиков
        /// </summary>
        Dictionary<int, CompareGraphForm> WathFormDictionary = new Dictionary<int, CompareGraphForm>();
        /// <summary>
        /// номер типа результата для добавления к нужной форме предпросмотра
        /// </summary>
        int? TupeGraphNumber = null;
        /// <summary>
        /// используется для временного хранение данных для формы сравнения
        /// </summary>
        ResultType_MAINClass TempResult = null;
        #endregion

        private void FactoryNew(object Tag)
        {
            try
            {
                if (Tag is DBLoaderPackClass)
                {
                    DBLoaderPackClass Pack = Tag as DBLoaderPackClass;
                    this.label1.Text = Pack.SpesialText;
                    //чтоб не отображалась лишний раз
                    this.buttonEditResult.Enabled = false;

                    switch (Pack.FormShowType)
                    {
                        case FormShowTypeEnum.Main:
                            {
                                this.label1.Text = Pack.SpesialText;

                                MainOptionsClass MainOptions = DB_Loader.LoadOnlyOptions(Pack);

                                this.mainOptionsUserControl1.MainResult = MainOptions;
                                this.mainOptionsUserControl1.antennIA.SelectedAntenn = DB_Loader.LoadIA(Pack);
                                this.mainOptionsUserControl1.antennTA.SelectedAntenn = DB_Loader.LoadTA(Pack);

                                #region работа со спец графиком

                                if (MainOptions.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма)
                                {
                                    this.mainOptionsUserControl1.groupBoxSpesialGraph.Text = "График степеней кросс поляризаций";
                                }
                                else if (MainOptions.MeasurementResultType == MeasurementTypeEnum.Коэффицент_усиления)
                                {
                                    this.mainOptionsUserControl1.groupBoxSpesialGraph.Text = "Для КУ нет спец графика";
                                    this.mainOptionsUserControl1.linkLabelGraphLoad.Visible = false;
                                }
                                else
                                {
                                    this.mainOptionsUserControl1.groupBoxSpesialGraph.Text = "График максимумов диаграмм направленности";
                                }

                                #endregion

                                //из за костыля в сравнении спец графиков надо скрывать кнопки каждый раз
                                this.buttonCompare.Visible = false;
                                this.buttonAddtoWatch.Visible = false;
                                this.buttonShowTable.Visible = false;

                                if (!this.mainOptionsUserControl1.Visible)
                                {
                                    this.UnvizibleElements();
                                    this.mainOptionsUserControl1.Dock = DockStyle.Fill;
                                    this.mainOptionsUserControl1.Visible = true;
                                    this.buttonXML.Visible = true;
                                    this.buttonExel.Visible = true;
                                    this.buttonTXT.Visible = true;
                                    this.buttonDelResult.Visible = true;


                                    this.buttonHidefreq.Enabled = true;
                                }

                                #region для экспорта и протоколов

                                this.ShowReportButtons(MainOptions, Pack);

                                this.buttonTXT.Tag = Pack;
                                this.buttonXML.Tag = Pack;
                                this.buttonExel.Tag = Pack;

                                this.buttonDelResult.Tag = Pack;
                                this.buttonHidefreq.Tag = Pack;

                                mainOptionsUserControl1.linkLabelGraphLoad.Tag = Pack;
                                #endregion

                                break;
                            }
                        case FormShowTypeEnum.Result:
                            {
                                #region общие переменные для графика
                                bool lockPolarCraph = false;
                                List<Series> serColAmpl = new List<Series>();
                                List<Series> serColPhase = new List<Series>();

                                //Загрузка результата
                                ResultType_MAINClass result = DB_Loader.LoadResult(Pack, false);

                                bool ErrorInGraph = false;

                                //добавление полного имени
                                string FulName = result.ToStringFullName();
                                #endregion

                                #region настройка графика в зависимости от типа измерения

                                switch (Pack.MeasurementTagType)
                                {
                                    case MeasurementTagTypeEnum.Union:
                                        {
                                            ResultTypeClassUnion resultUnion = result as ResultTypeClassUnion;

                                            Series TempAmpl = resultUnion.SelectedPolarization.SelectedFrequency.GetAmplSeries();

                                            //для возможности редактировать измерение
                                            this.buttonEditResult.Enabled = true;
                                            this.buttonEditResult.Tag = resultUnion.SelectedPolarization.SelectedFrequency;

                                            TempAmpl.Name = FulName;

                                            //добавление в массив графиков
                                            serColAmpl.Add(TempAmpl);

                                            #region заполняем графики из исходных результатов

                                            bool ErrorInitialResults = false;

                                            foreach (ResultType_MAINClass IshRes in resultUnion.InitialResults)
                                            {
                                                try
                                                {
                                                    Series TempPhase = IshRes.SelectedPolarization.SelectedFrequency.GetAmplSeries();
                                                    TempPhase.Name = DB_Loader.GetFullNameByResult(IshRes);
                                                    serColPhase.Add(TempPhase);
                                                }
                                                catch
                                                {
                                                    ErrorInitialResults = true;
                                                }
                                            }

                                            if (ErrorInitialResults)
                                            {
                                                MessageBox.Show(this, "Не удалось загрузить 1 или более исходных результатов измерения", "Ошибка загрузки исходных результатов", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }

                                            #endregion

                                            #region подписываем графики

                                            groupBoxAMPL.Text = "Амплитуда рассчитанного результата";
                                            groupBoxPHASE.Text = "Амплитуда исходных данных";

                                            #endregion

                                            break;
                                        }
                                    case MeasurementTagTypeEnum.DN_Normal:
                                        {
                                            Series TempAmpl = result.SelectedPolarization.SelectedFrequency.GetAmplSeries();
                                            Series TempPhase = result.SelectedPolarization.SelectedFrequency.GetPhaseSeries();

                                            //для возможности редактировать измерение
                                            this.buttonEditResult.Enabled = true;
                                            this.buttonEditResult.Tag = result.SelectedPolarization.SelectedFrequency;

                                            TempAmpl.Name = FulName;
                                            TempPhase.Name = FulName;

                                            //добавление в массив графиков
                                            serColAmpl.Add(TempAmpl);
                                            serColPhase.Add(TempPhase);

                                            #region подписываем графики

                                            groupBoxAMPL.Text = "Амплитуда";
                                            groupBoxPHASE.Text = "Фаза";

                                            #endregion

                                            break;
                                        }
                                    case MeasurementTagTypeEnum.SDNM_Sum:
                                        {
                                            ResultTypeClassСДНМ ResultSDNM = result as ResultTypeClassСДНМ;

                                            try
                                            {
                                                ResultSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Sum;

                                                Series TempSUM = ResultSDNM.SUM_Polarization.SelectedFrequency.GetAmplSeries();
                                                Series TempMain = ResultSDNM.Main_Polarization.SelectedFrequency.GetAmplSeries();
                                                Series TempCross = ResultSDNM.Cross_Polarization.SelectedFrequency.GetAmplSeries();

                                                TempSUM.Name = "Sum; " + FulName;
                                                TempMain.Name = "Main; " + FulName;
                                                TempCross.Name = "Cross; " + FulName;

                                                //добавление в массив графиков
                                                serColAmpl.Add(TempSUM);
                                                serColPhase.Add(TempMain);
                                                serColPhase.Add(TempCross);

                                                #region подписываем графики

                                                groupBoxAMPL.Text = "Амплитуда SUM";
                                                groupBoxPHASE.Text = "Амплитуда Main и Cross";

                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(this, ex.Message, "Ошибка рассчёта суммы по мощности", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                ErrorInGraph = true;
                                            }
                                            break;
                                        }
                                    case MeasurementTagTypeEnum.SDNM_Main:
                                        {
                                            ResultTypeClassСДНМ ResultSDNM = result as ResultTypeClassСДНМ;
                                            ResultSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Main;



                                            Series TempAmpl = result.SelectedPolarization.SelectedFrequency.GetAmplSeries();
                                            Series TempPhase = result.SelectedPolarization.SelectedFrequency.GetPhaseSeries();

                                            //для возможности редактировать измерение
                                            this.buttonEditResult.Enabled = true;
                                            this.buttonEditResult.Tag = result.SelectedPolarization.SelectedFrequency;

                                            TempAmpl.Name = FulName;
                                            TempPhase.Name = FulName;

                                            //добавление в массив графиков
                                            serColAmpl.Add(TempAmpl);
                                            serColPhase.Add(TempPhase);

                                            #region подписываем графики

                                            groupBoxAMPL.Text = "Амплитуда";
                                            groupBoxPHASE.Text = "Фаза";

                                            #endregion

                                            break;
                                        }
                                    case MeasurementTagTypeEnum.SDNM_Cross:
                                        {
                                            ResultTypeClassСДНМ ResultSDNM = result as ResultTypeClassСДНМ;
                                            ResultSDNM.ChangeSelectedPolarization = SelectedPolarizationEnum.Cross;



                                            Series TempAmpl = result.SelectedPolarization.SelectedFrequency.GetAmplSeries();
                                            Series TempPhase = result.SelectedPolarization.SelectedFrequency.GetPhaseSeries();

                                            //для возможности редактировать измерение
                                            this.buttonEditResult.Enabled = true;
                                            this.buttonEditResult.Tag = result.SelectedPolarization.SelectedFrequency;

                                            TempAmpl.Name = FulName;
                                            TempPhase.Name = FulName;

                                            //добавление в массив графиков
                                            serColAmpl.Add(TempAmpl);
                                            serColPhase.Add(TempPhase);

                                            #region подписываем графики

                                            groupBoxAMPL.Text = "Амплитуда";
                                            groupBoxPHASE.Text = "Фаза";

                                            #endregion

                                            break;
                                        }
                                    case MeasurementTagTypeEnum.KY:
                                        {
                                            //нужна реализация суммы для ку наподобии SDNM + переделать загрузку КУ
                                            ResultTypeClassКУ ResultKY = result as ResultTypeClassКУ;
                                            lockPolarCraph = true;

                                            try
                                            {
                                                FrequencyElementClass Sum = new FrequencyElementClass();
                                                FrequencyElementClass Main = new FrequencyElementClass();
                                                FrequencyElementClass Cross = new FrequencyElementClass();

                                                ResultKY.ChangeSelectedPolarization = SelectedPolarizationEnum.Main;
                                                Series TempMain = ResultKY.SelectedPolarization.SelectedFrequency.GetAmplSeries();

                                                ResultKY.ChangeSelectedPolarization = SelectedPolarizationEnum.Cross;
                                                Series TempCross = ResultKY.SelectedPolarization.SelectedFrequency.GetAmplSeries();

                                                ResultKY.ChangeSelectedPolarization = SelectedPolarizationEnum.Sum;
                                                Series TempSUM = ResultKY.SelectedPolarization.SelectedFrequency.GetAmplSeries();

                                                TempSUM.Name = "Sum; " + FulName;
                                                TempMain.Name = "Main; " + FulName;
                                                TempCross.Name = "Cross; " + FulName;

                                                //добавление в массив графиков
                                                serColAmpl.Add(TempSUM);
                                                serColPhase.Add(TempMain);
                                                serColPhase.Add(TempCross);

                                                #region подписываем графики

                                                groupBoxAMPL.Text = "КУ SUM";
                                                groupBoxPHASE.Text = "КУ Main и Cross";

                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(this, ex.Message, "Ошибка рассчёта суммы КУ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                ErrorInGraph = true;
                                            }


                                            break;
                                        }
                                }

                                #endregion

                                if (!ErrorInGraph)
                                {
                                    this.previewGraphControl1.CreateGraph(serColAmpl, lockPolarCraph);
                                    this.previewGraphControl2.CreateGraph(serColPhase, lockPolarCraph);


                                    this.ShowCalculationResult(result);
                                    this.ShowReportButtons(result.MainOptions, Pack);
                                }
                                else
                                {
                                    this.UnvizibleElements();
                                }

                                break;
                            }
                        case FormShowTypeEnum.Antenn:
                            {
                                this.UnvizibleElements();

                                this.buttonDelAnten.Tag = Pack;
                                this.buttonDelAnten.Visible = true;

                                this.buttonXML.Tag = Pack;
                                this.buttonXML.Visible = true;
                                this.buttonExel.Visible = true;
                                this.buttonPDF.Visible = true;

                                break;
                            }
                        case FormShowTypeEnum.None:
                            {
                                this.UnvizibleElements();

                                break;
                            }
                        default:
                            {
                                MessageBox.Show(this, "Ошибка формы", "Неизвестный объект для отображения, обратитесь к разработчику", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region работа с предпросмотром
        private void buttonAddtoWatch_Click(object sender, EventArgs e)
        {
            AddToGraphFormDictionary(this.TempResult, Convert.ToInt32(TupeGraphNumber));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WathFormDictionary[Convert.ToInt32(this.TupeGraphNumber)].ShowDialog();
        }

        protected void AddToGraphFormDictionary(ResultType_MAINClass result, int ResultTupe)
        {
            if (this.WathFormDictionary[ResultTupe].AddToWath(result))
            {
                this.buttonAddtoWatch.Visible = false;
                this.buttonCompare.Visible = true;
            }
        }

        private void buttonShowTable_Click(object sender, EventArgs e)
        {
            string tex = string.Format("{0}; зав.№ {1}; F={2} ({3})", this.TempResult.Antenn.Name, this.TempResult.Antenn.ZAVNumber, this.TempResult.SelectedPolarization.SelectedFrequency.ToString(), this.TempResult.SelectedPolarization.Polarization);

            ShowTabl(false, this.TempResult.SelectedPolarization.SelectedFrequency, tex);
        }

        private void ShowTabl(bool EnabledEditing, FrequencyElementClass freq, string text)
        {
            DataTableForm DTF = new DataTableForm();
            DTF.DataList = freq.ResultAmpl_PhaseElements;

            DTF.Text = text;

            if (EnabledEditing)
            {
                DTF.EditMode = EnabledEditing;

                if (DTF.ShowDialog(this) == DialogResult.Yes)
                {
                    if (MessageBox.Show(this, string.Format("Вы действительно хотите перезаписать результат измерения {0} ?\n\nДля обновления графиков выбирите результат снова", freq.TableResultName), "Обновление результата", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        freq.ResultAmpl_PhaseElements = DTF.EditResult;

                        DB_Loader.ClearMeasurementTable(freq.TableResultName, freq.id);
                        DB_Loader.SaveResultElementS(freq.ResultAmpl_PhaseElements, freq.TableResultName, freq.id);

                        // MessageBox.Show(this, "Результат успешно перезаписан. Для обновления графиков выбирите результат снова", "Обновление результата", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                DTF.Show(this);
            }
        }

        #endregion

        #region работа с отчётами

        protected ResultType_MAINClass LoadResultForProtokol(DBLoaderPackClass Pack)
        {
            ResultType_MAINClass Result = null;

            #region Загружаем результат полный или частичный

            switch (Pack.FormShowType)
            {
                case FormShowTypeEnum.Main:
                    {
                        Result = DB_Loader.LoadResult(Pack, true);
                        break;
                    }
                case FormShowTypeEnum.Result:
                    {
                        Result = DB_Loader.LoadResult(Pack, false);
                        break;
                    }
            }

            #endregion

            return Result;
        }


        private void buttonПХ_Ампл_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultTypeПХ result = this.LoadResultForProtokol(buttonПХ_Ампл.Tag as DBLoaderPackClass) as ResultTypeПХ;

            Report_Sheild_M_Interfaces.IBaseReportResult[] reslist = result.GetListResult_for_Report_AmplitudeDiagram().ToArray();

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(reslist);
            protForm.ShowDialog();
        }

        private void buttonПХ_Фаза_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultTypeПХ result = this.LoadResultForProtokol(buttonПХ_Фаза.Tag as DBLoaderPackClass) as ResultTypeПХ;

            Report_Sheild_M_Interfaces.IBaseReportResult[] reslist = result.GetListResult_for_Report_PhaseDiagram().ToArray();

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(reslist);
            protForm.ShowDialog();
        }

        private void buttonАДН_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultTypeClassДН result = this.LoadResultForProtokol(buttonАДН.Tag as DBLoaderPackClass) as ResultTypeClassДН;

            Report_Sheild_M_Interfaces.IBaseReportResult[] reslist = result.GetListResult_for_Report_AmplitudeDiagram().ToArray();

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(reslist);
            protForm.ShowDialog();
        }

        private void buttonФДН_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultTypeClassДН result = this.LoadResultForProtokol(buttonФДН.Tag as DBLoaderPackClass) as ResultTypeClassДН;

            Report_Sheild_M_Interfaces.IBaseReportResult[] reslist = result.GetListResult_for_Report_PhaseDiagram().ToArray();

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(reslist);
            protForm.ShowDialog();
        }

        private void buttonСДНМ_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultTypeClassСДНМ result = this.LoadResultForProtokol(buttonСДНМ.Tag as DBLoaderPackClass) as ResultTypeClassСДНМ;

            Report_Sheild_M_Interfaces.IBaseReportResult[] reslist = result.GetListResult_for_Report_SUMDiagram().ToArray();

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(reslist);
            protForm.ShowDialog();
        }

        private void buttonКУ_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultType_MAINClass result = this.LoadResultForProtokol(buttonКУ.Tag as DBLoaderPackClass) as ResultType_MAINClass;

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result);
            protForm.ShowDialog();
        }

        private void buttonUnionDN_Click(object sender, EventArgs e)
        {
            //создаём объект результата
            ResultType_MAINClass result = this.LoadResultForProtokol(buttonUnionDN.Tag as DBLoaderPackClass) as ResultType_MAINClass;

            //создаём отчёт
            ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(result);
            protForm.ShowDialog();
        }

        #endregion


        #region Экспорт

        private void buttonTXT_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonTXT.Tag as DBLoaderPackClass;

            //Загрузка результата
            ResultType_MAINClass result = DB_Loader.LoadResult(Pack, false);
            SaverResultForm.SaveResultToTextFileInFolder(this, result);
        }

        private void buttonXML_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonXML.Tag as DBLoaderPackClass;





            if (Pack.FormShowType == FormShowTypeEnum.Antenn)
            {
                if (MessageBox.Show(this, "Экспорт измерений может занять продолжительное время \n Произвести экспорт?", "Экспорт", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<ResultType_MAINClass> results = DB_Loader.LoadALLResult(Pack);
                    List<IResultType_MAIN> res = new List<IResultType_MAIN>(results.ToArray());

                    if (this.checkBoxEditMode.Checked)
                    {
                        #region Подправить результаты при экспорте усиление

                        if (MessageBox.Show(this, "Подправить результат перед экспортом, исправления уже прописаны в коде, УВЕЛИЧЕНИЕ АМПЛИТУДЫ", "Секретная функция))", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {


                            for (int i = 0; i < res.Count; i++)
                            {
                                int id = res[i].id;

                                for (int j = 0; j < res[i].SelectedPolarization.FrequencyElements.Count; j++)
                                {
                                    double freq = res[i].SelectedPolarization.FrequencyElements[j].Frequency;

                                    bool needAdd = false;
                                    double AddData = 0;

                                    #region проверка на необходимость исправления результата для 8портовой антены

                                    if (freq == 794.8d)
                                    {
                                        needAdd = true;
                                        AddData = 4.79;
                                    }
                                    else if (freq == 835.8d)
                                    {
                                        needAdd = true;
                                        AddData = 6.1;
                                    }
                                    else if (freq == 892d)
                                    {
                                        needAdd = true;
                                        AddData = 4.21;
                                    }
                                    else if (freq == 937d)
                                    {
                                        needAdd = true;
                                        AddData = 3.26;
                                    }
                                    else if (freq == 1743.8d)
                                    {
                                        needAdd = true;
                                        AddData = 1;
                                    }


                                    else if (freq == 1838.8d)
                                    {
                                        needAdd = true;
                                        AddData = 1.33;
                                    }
                                    else if (freq == 1927.4d)
                                    {
                                        needAdd = true;
                                        AddData = 2.1;
                                    }
                                    else if (freq == 2117.4d)
                                    {
                                        needAdd = true;
                                        AddData = 0.91;
                                    }
                                    else if (freq == 2333d)
                                    {
                                        needAdd = true;
                                        AddData = 0.71;
                                    }
                                    else if (freq == 2366d)
                                    {
                                        needAdd = true;
                                        AddData = 0.58;
                                    }
                                    else if (freq == 2565d)
                                    {
                                        needAdd = true;
                                        AddData = 1.85;
                                    }
                                    else if (freq == 2685d)
                                    {
                                        needAdd = true;
                                        AddData = 1.2;
                                    }

                                    #endregion

                                    #region проверка на необходимость исправления результата для 12портовой антены
                                    /*
                                    if (freq == 794.8d)
                                    {
                                        needAdd = true;
                                        AddData = 2.43;
                                    }
                                    else if (freq == 835.8d)
                                    {
                                        needAdd = true;
                                        AddData = 3.91;
                                    }
                                    else if (freq == 892d)
                                    {
                                        needAdd = true;
                                        AddData = 3.79;
                                    }
                                    else if (freq == 937d)
                                    {
                                        needAdd = true;
                                        AddData = 4.74;
                                    }
                                    else if (freq == 1743.8d)
                                    {
                                        needAdd = true;
                                        AddData = 1.54;
                                    }


                                    else if (freq == 1838.8d)
                                    {
                                        needAdd = true;
                                        AddData = 1.88;
                                    }
                                    else if (freq == 1927.4d)
                                    {
                                        needAdd = true;
                                        AddData = 2.6;
                                    }
                                    else if (freq == 2117.4d)
                                    {
                                        needAdd = true;
                                        AddData = 1.9;
                                    }
                                    else if (freq == 2333d)
                                    {
                                        needAdd = true;
                                        AddData = 2.19;
                                    }
                                    else if (freq == 2366d)
                                    {
                                        needAdd = true;
                                        AddData = 2.07;
                                    }
                                    else if (freq == 2565d)
                                    {
                                        needAdd = true;
                                        AddData = 2.61;
                                    }
                                    else if (freq == 2685d)
                                    {
                                        needAdd = true;
                                        AddData = 1.27;
                                    }
                                    */
                                    #endregion

                                    if (needAdd)
                                    {
                                        for (int z = 0; z < res[i].PolarizationElements[0].FrequencyElements[j].ResultAmpl_PhaseElements.Count; z++)
                                        {
                                            //res[i].SelectedPolarization.FrequencyElements[j].ResultAmpl_PhaseElements[z].Ampl_dB += AddData;
                                            res[i].PolarizationElements[0].FrequencyElements[j].ResultAmpl_PhaseElements[z].Ampl_dB += AddData;
                                        }



                                    }
                                }
                            }

                        }

                        #endregion


                        #region Подправить результаты при экспорте усреднение

                        if (MessageBox.Show(this, "Подправить результат перед экспортом, УСРЕДНЕНИЕ", "Секретная функция))", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //измененные результаты
                            List<IResultType_MAIN> resNEW = new List<IResultType_MAIN>();


                            for (int i = 0; i < res.Count; i++)
                            {
                                int id_Result = res[i].id;
                                bool needAdd = false;

                                int Find_Result_Number_1 = i;
                                int FindFreq_Number_1 = -1;


                                for (int j = 0; j < res[i].SelectedPolarization.FrequencyElements.Count; j++)
                                {
                                    double freq = res[i].SelectedPolarization.FrequencyElements[j].Frequency;
                                    FindFreq_Number_1 = j;

                                    int Find_Result_ID_2 = -1;
                                    double FindFreq_2 = -1;

                                    int Find_Result_Number_2 = -1;
                                    int FindFreq_Number_2 = -1;



                                    #region проверка на необходимость редактирования 1
                                    /*
                                    if (freq == 892)
                                    {
                                        FindFreq_2 = 937;

                                        if (id_Result == 7330 || id_Result == 7333 || id_Result == 7349 || id_Result == 7355 || id_Result == 7353 || id_Result == 7351 ||
                                            id_Result == 7401 || id_Result == 7404 || id_Result == 7406 || id_Result == 7412 || id_Result == 7410 || id_Result == 7408)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = id_Result;
                                            needAdd = true;
                                        }
                                    }


                                    else if (freq == 2565)
                                    {
                                        FindFreq_2 = 2666;

                                        if (id_Result == 7368 || id_Result == 7374 || id_Result == 7372||
                                            id_Result == 7442 || id_Result == 7439 || id_Result == 7456 || id_Result == 7445 || id_Result == 7448)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = id_Result;
                                            needAdd = true;
                                        }
                                    }


                                    else if (freq == 2666)
                                    {
                                        FindFreq_2 = 2685;

                                        if (id_Result == 7325)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = id_Result;
                                            needAdd = true;
                                        }
                                    }


                                    else if (freq == 1838.8d)
                                    {
                                        FindFreq_2 = freq;

                                        if (id_Result == 7326)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7387;
                                            needAdd = true;
                                        }

                                        else if (id_Result == 7338)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7386;
                                            needAdd = true;
                                        }

                                        else if (id_Result == 7345)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7385;
                                            needAdd = true;
                                        }

                                        else if (id_Result == 7368)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7382;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7366)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7383;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7364)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7384;
                                            needAdd = true;
                                        }

                                        //------------------------------------------------------

                                        else if (id_Result == 7426)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7427;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7429)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7430;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7432)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7433;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7442)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7443;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7439)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7440;
                                            needAdd = true;
                                        }
                                        else if (id_Result == 7436)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = 7437;
                                            needAdd = true;
                                        }
                                    }
                                    
                                    */
                                    #endregion

                                    #region проверка на необходимость редактирования 2

                                    if (freq == 2565d)
                                    {
                                        FindFreq_2 = 2666;

                                        if (id_Result == 7325)
                                        {
                                            //нашли нужное измерение и нужную частоту, ищем парную частоту и измерение
                                            Find_Result_ID_2 = id_Result;
                                            needAdd = true;
                                        }
                                    }

                                    #endregion


                                    if (needAdd)
                                    {
                                        needAdd = false;

                                        #region ищем парное ищмерение

                                        if (id_Result != Find_Result_ID_2)
                                        {
                                            //делаем поиск ещё одного измрения

                                            for (int n = 0; n < res.Count; n++)
                                            {
                                                if (res[n].id == Find_Result_ID_2)
                                                {
                                                    //нашли пару
                                                    Find_Result_Number_2 = n;

                                                    break;
                                                }
                                            }


                                        }
                                        else
                                        {
                                            //усредняем с парной частотой
                                            Find_Result_Number_2 = Find_Result_Number_1;
                                        }

                                        #endregion

                                        #region ищем парную частоту

                                        for (int m = 0; m < res.Count; m++)
                                        {
                                            if (res[Find_Result_Number_2].SelectedPolarization.FrequencyElements[m].Frequency == FindFreq_2)
                                            {
                                                //нашли пару
                                                FindFreq_Number_2 = m;
                                                break;
                                            }
                                        }

                                        #endregion

                                        #region делаем усреднение или разворот

                                        //если необходим разворот измерения и замена
                                        if (id_Result == 7326 && Find_Result_ID_2 == 7387 ||
                                            id_Result == 7338 && Find_Result_ID_2 == 7386 ||
                                            id_Result == 7345 && Find_Result_ID_2 == 7385 ||
                                            id_Result == 7368 && Find_Result_ID_2 == 7382 ||
                                            id_Result == 7366 && Find_Result_ID_2 == 7383 ||
                                            id_Result == 7364 && Find_Result_ID_2 == 7384 ||


                                            id_Result == 7426 && Find_Result_ID_2 == 7427 ||
                                            id_Result == 7429 && Find_Result_ID_2 == 7430 ||
                                            id_Result == 7432 && Find_Result_ID_2 == 7433 ||
                                            id_Result == 7442 && Find_Result_ID_2 == 7443 ||
                                            id_Result == 7439 && Find_Result_ID_2 == 7440 ||
                                            id_Result == 7436 && Find_Result_ID_2 == 7437)
                                        {
                                            //делаем разворот

                                            FrequencyElementClass tempInterpolFreq = ResultType_MAINClass.GetInterpolationFreq(res[Find_Result_Number_2].SelectedPolarization.FrequencyElements[FindFreq_Number_2], res[Find_Result_Number_1].MainOptions);

                                            res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1].ResultAmpl_PhaseElements.Clear();


                                            double coord = Convert.ToDouble(res[Find_Result_Number_1].MainOptions.Parameters.StepMeasurement) * -1;
                                            for (int c = 0; c < tempInterpolFreq.ResultAmpl_PhaseElements.Count; c++)
                                            {
                                                if (tempInterpolFreq.ResultAmpl_PhaseElements[c].Cordinate >= 180)
                                                {
                                                    coord += Convert.ToDouble(res[Find_Result_Number_1].MainOptions.Parameters.StepMeasurement);

                                                    //начинаем забивку с разворотом
                                                    ResultOptionsClassLibrary.ResultElementClass el = new ResultElementClass(coord, tempInterpolFreq.ResultAmpl_PhaseElements[c].Ampl_dB, tempInterpolFreq.ResultAmpl_PhaseElements[c].Phase_degree);

                                                    res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1].ResultAmpl_PhaseElements.Add(el);
                                                }

                                                if (c == tempInterpolFreq.ResultAmpl_PhaseElements.Count - 1)
                                                {
                                                    /*пауза*/
                                                }
                                            }




                                            //добавляем вторую половину диаграммы
                                            for (int c = 0; c < tempInterpolFreq.ResultAmpl_PhaseElements.Count; c++)
                                            {
                                                if (tempInterpolFreq.ResultAmpl_PhaseElements[c].Cordinate < 180)
                                                {
                                                    coord += Convert.ToDouble(res[Find_Result_Number_1].MainOptions.Parameters.StepMeasurement);

                                                    ResultOptionsClassLibrary.ResultElementClass el = new ResultElementClass(coord, tempInterpolFreq.ResultAmpl_PhaseElements[c].Ampl_dB, tempInterpolFreq.ResultAmpl_PhaseElements[c].Phase_degree);
                                                    res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1].ResultAmpl_PhaseElements.Add(el);

                                                }
                                                else { break; }
                                            }

                                            //пересчитываем результаты
                                            CalculationResultsClass.Calculate_PH_Part(res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1]);
                                            CalculationResultsClass.Calculate_DN_Part(res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1]);


                                        }
                                        else
                                        {
                                            List<FrequencyElementClass> tempInterpolFreq = new List<FrequencyElementClass>();

                                            tempInterpolFreq.Add(ResultType_MAINClass.GetInterpolationFreq(res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1],
                                                res[Find_Result_Number_1].MainOptions)); //делаем интерполящию первой частоты в первом измрении

                                            tempInterpolFreq.Add(ResultType_MAINClass.GetInterpolationFreq(res[Find_Result_Number_2].SelectedPolarization.FrequencyElements[FindFreq_Number_2],
                                               res[Find_Result_Number_1].MainOptions)); //делаем интерполящию первой частоты во втором измрении


                                            FrequencyElementClass retfreq = FrequencyElementClass.SrednFrequency(tempInterpolFreq); //усредняем

                                            //делаем замену частоты
                                            res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1].ResultAmpl_PhaseElements = retfreq.ResultAmpl_PhaseElements;

                                            //пересчитываем результаты
                                            CalculationResultsClass.Calculate_PH_Part(res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1]);
                                            CalculationResultsClass.Calculate_DN_Part(res[Find_Result_Number_1].SelectedPolarization.FrequencyElements[FindFreq_Number_1]);
                                        }

                                        #endregion

                                        //добавляем в массив измененных

                                        resNEW.Add(res[Find_Result_Number_1]);
                                    }





                                }
                            }

                            //подменяем возвращаемый результат
                            res = resNEW;
                        }

                        #endregion


                        #region Поправить параметры измерения стартовые координаты

                        if (MessageBox.Show(this, "Подправить результат перед экспортом, Стартовые координаты", "Секретная функция))", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //измененные результаты
                            List<IResultType_MAIN> resP12 = new List<IResultType_MAIN>();
                            List<IResultType_MAIN> resP34 = new List<IResultType_MAIN>();
                            List<IResultType_MAIN> resP56 = new List<IResultType_MAIN>();
                            List<IResultType_MAIN> resP78 = new List<IResultType_MAIN>();

                            while (res.Count > 0)
                            {

                                for (int i = 1; i < res.Count; i++)
                                {
                                    if (res[0].MainOptions.Descriptions == res[i].MainOptions.Descriptions)
                                    {
                                        //нашли пару, меняем параметры

                                        ResultType_MAINClass temp1 = res[0] as ResultType_MAINClass;
                                        ResultType_MAINClass temp2 = res[i] as ResultType_MAINClass;

                                        int portN = -1;
                                        int tbN = -1;

                                        int startF = 0;
                                        int stopF = temp1.SelectedPolarization.FrequencyElements.Count;

                                        SaverResultForm.DecodingDescription(temp1, out portN, out tbN, out startF, out stopF);


                                        for (int j = startF; j <= stopF; j++)
                                        {
                                            ResultType_MAINClass res1 = temp1.Clone() as ResultType_MAINClass;
                                            ResultType_MAINClass res2 = temp2.Clone() as ResultType_MAINClass;

                                            res1.PolarizationElements[0].FrequencyElements.Clear();
                                            res2.PolarizationElements[0].FrequencyElements.Clear();


                                            res1.PolarizationElements[0].FrequencyElements.Add(temp1.PolarizationElements[0].FrequencyElements[j].Clone() as FrequencyElementClass);
                                            res2.PolarizationElements[0].FrequencyElements.Add(temp2.PolarizationElements[0].FrequencyElements[j].Clone() as FrequencyElementClass);


                                            //меняем стартовую позицию на максимальную

                                            if (res1.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
                                            {
                                                res1.MainOptions.Parameters.StartOPU_Y = Convert.ToDecimal(Math.Round(res2.PolarizationElements[0].FrequencyElements[0].CalculationResults.Направление_максимума_диаграммы_направленности, 1));

                                                res2.MainOptions.Parameters.StartOPU_W = Convert.ToDecimal(Math.Round(res1.PolarizationElements[0].FrequencyElements[0].CalculationResults.Направление_максимума_диаграммы_направленности, 1));

                                            }
                                            else
                                            {
                                                res1.MainOptions.Parameters.StartOPU_W = Convert.ToDecimal(Math.Round(res2.PolarizationElements[0].FrequencyElements[0].CalculationResults.Направление_максимума_диаграммы_направленности, 1));

                                                res2.MainOptions.Parameters.StartOPU_Y = Convert.ToDecimal(Math.Round(res1.PolarizationElements[0].FrequencyElements[0].CalculationResults.Направление_максимума_диаграммы_направленности, 1));
                                            }




                                            res1.MainOptions.Name += " " + res1.PolarizationElements[0].FrequencyElements[0].ToString();
                                            res2.MainOptions.Name += " " + res2.PolarizationElements[0].FrequencyElements[0].ToString();

                                            //добавляем оба в выводные результаты

                                            switch (portN)
                                            {
                                                case 1:
                                                    {
                                                        resP12.Add(res1);
                                                        resP12.Add(res2);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        resP12.Add(res1);
                                                        resP12.Add(res2);
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        resP34.Add(res1);
                                                        resP34.Add(res2);
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        resP34.Add(res1);
                                                        resP34.Add(res2);
                                                        break;
                                                    }
                                                case 5:
                                                    {
                                                        resP56.Add(res1);
                                                        resP56.Add(res2);
                                                        break;
                                                    }
                                                case 6:
                                                    {
                                                        resP56.Add(res1);
                                                        resP56.Add(res2);
                                                        break;
                                                    }
                                                case 7:
                                                    {
                                                        resP78.Add(res1);
                                                        resP78.Add(res2);
                                                        break;
                                                    }
                                                case 8:
                                                    {
                                                        resP78.Add(res1);
                                                        resP78.Add(res2);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        break;
                                                    }
                                            }



                                        }




                                        //удаляем из исходных
                                        res.RemoveAt(i);
                                        res.RemoveAt(0);

                                        break;
                                    }
                                }

                            }




                            for (int z = 0; z < 4; z++)
                            {
                                List<IResultType_MAIN> tempResp = null;
                                switch (z)
                                {
                                    case 0:
                                        {
                                            tempResp = resP12;
                                            break;
                                        }
                                    case 1:
                                        {
                                            tempResp = resP34;
                                            break;
                                        }
                                    case 2:
                                        {
                                            tempResp = resP56;
                                            break;
                                        }
                                    case 3:
                                        {
                                            tempResp = resP78;
                                            break;
                                        }
                                }



                                List<Report_Sheild_M_Interfaces.IBaseReportResult> reslist = new List<Report_Sheild_M_Interfaces.IBaseReportResult>();

                                foreach (IResultType_MAIN r in tempResp)
                                {
                                    ResultTypeClassДН resP = r as ResultTypeClassДН;

                                    reslist.AddRange(resP.GetListResult_for_Report_AmplitudeDiagram());
                                }

                                Report_Sheild_M_Interfaces.IBaseReportResult[] ResMass = reslist.ToArray();

                                //создаём отчёт
                                ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(ResMass);
                                protForm.ShowDialog();

                            }

                        }

                        #endregion

                        #region Подправить результаты при экспорте шаг

                        if (MessageBox.Show(this, "Подправить результат перед экспортом, исправления уже прописаны в коде, ШАГ ИГМЕРЕНИЯ", "Секретная функция))", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            for (int i = 0; i < res.Count; i++)
                            {
                                res[i].MainOptions.Parameters.StepMeasurement = 0.5m;
                            }
                        }

                        #endregion



                        DB_Forms.ResultDescriptionForm DF = new DB_Forms.ResultDescriptionForm();
                        DF.resuslts = res;
                        //обработчик описания
                        DF.ShowDialog(this);
                    }
                    SaverResultForm.SaveResultToXMLFileInFolder(this, res);
                }
            }
            else
            {
                //Загрузка результата
                ResultType_MAINClass result = DB_Loader.LoadResult(Pack, false);

                if (this.checkBoxEditMode.Checked)
                {
                    DB_Forms.ResultDescriptionForm DF = new DB_Forms.ResultDescriptionForm();
                    List<IResultType_MAIN> res = new List<IResultType_MAIN>();
                    res.Add(result);
                    DF.resuslts = res;
                    //обработчик описания
                    DF.ShowDialog(this);
                }

                SaverResultForm.SaveResultToXMLFileInFolder(this, result);
            }
        }


        private void ButtonPDF_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonXML.Tag as DBLoaderPackClass;
            if (Pack.FormShowType == FormShowTypeEnum.Antenn)
            {
                List<ResultType_MAINClass> results = DB_Loader.LoadALLResult(Pack);
                List<IResultType_MAIN> res = new List<IResultType_MAIN>(results.ToArray());

                if (MessageBox.Show(this, "Создание протоколов может занять продолжительное время \n Генерировать протоколы по портам?", "Генерировать протолколы по портам))", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<Report_Sheild_M_Interfaces.IBaseReportResult> reslist = new List<Report_Sheild_M_Interfaces.IBaseReportResult>();

                    foreach (IResultType_MAIN r in res)
                    {
                        ResultTypeClassСДНМ resP = r as ResultTypeClassСДНМ;

                        int portN = -1;
                        int tbN = -1;

                        int startF = 0;
                        int stopF = resP.SelectedPolarization.FrequencyElements.Count;

                        SaverResultForm.DecodingDescription(resP, out portN, out tbN, out startF, out stopF);

                        //получаем данные для протокола только нужных частот
                        for (int j = startF; j <= stopF; j++)
                        {
                            resP.SUM_Polarization.SelectedFrequencyIndex = j;
                            resP.Main_Polarization.SelectedFrequencyIndex = j;
                            resP.Cross_Polarization.SelectedFrequencyIndex = j;

                            string VH = "";
                            if (resP.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут || resP.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                            {
                                VH = "Horizontal plane";
                            }
                            else if (resP.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан || resP.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                            {
                                VH = "Vertical plane";
                            }

                            string NewDescription = string.Format("Port {0} TB {1} {2}", portN, tbN, VH);

                            Report_Sheild_M_Interfaces.ISumDiagramDirection temp = resP.GetSelectedResult_for_Report_SUMDiagram();
                            temp.ResultDescription = NewDescription;

                            reslist.Add(temp);
                        }

                    }

                    Report_Sheild_M_Interfaces.IBaseReportResult[] ResMass = reslist.ToArray();

                    //создаём отчёт
                    ReportForm.ProtocolForm protForm = new ReportForm.ProtocolForm(ResMass);
                    protForm.ShowDialog();
                }
            }
        }
    

        private void buttonALLToXML_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Экспорт измерений может занять ОЧЕНЬ продолжительное время \n Проврьте и почистите всю базу от бракованных результатов.\n\n Произвести экспорт?"), "Экспорт", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<ResultType_MAINClass> results = DB_Loader.LoadALLResult();
                List<IResultType_MAIN> res = new List<IResultType_MAIN>(results.ToArray());
                SaverResultForm.SaveResultToXMLFileInFolder(this, res);
            }
        }

        private void buttonExel_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonXML.Tag as DBLoaderPackClass;


            if (Pack.FormShowType == FormShowTypeEnum.Antenn)
            {
                if (MessageBox.Show(this, "Экспорт измерений может занять продолжительное время \n Произвести экспорт?", "Экспорт", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<ResultType_MAINClass> results = DB_Loader.LoadALLResult(Pack);
                    List<IResultType_MAIN> res = new List<IResultType_MAIN>(results.ToArray());


                    if (MessageBox.Show(this, "Сохранить в один файл?", "Экспорт в Exel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaverResultForm.SaveResultToXLSX_ONE_FileInFolder(this, res);
                    }
                    else
                    {
                        SaverResultForm.SaveResultToXLSXFileInFolder(this, res);
                    }
                }
            }
            else
            {
                //Загрузка результата
                ResultType_MAINClass result = DB_Loader.LoadResult(Pack, false);
                SaverResultForm.SaveResultToXLSXFileInFolder(this, result);
            }

            /*

            DBLoaderPackClass Pack = buttonExel.Tag as DBLoaderPackClass;

            //Загрузка результата
            ResultType_MAINClass result = DB_Loader.LoadResult(Pack, false);
            SaverResultForm.SaveResultToXLSXFileInFolder(this, result);*/
        }

        #endregion

        #region удаление результатов

        private void buttonDelResult_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonDelResult.Tag as DBLoaderPackClass;


            int Idmain = DB_Loader.GetMainID(Pack);


            if (MessageBox.Show(this, "Удалить результат " + Idmain.ToString(), "Удаление результата", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //удаляем ноды результатов костыль
                this.DellNodes(treeView1.Nodes.Find("main " + Idmain.ToString(), true));
                this.DellNodes(treeView2.Nodes.Find("main " + Idmain.ToString(), true));

                DB_Loader.DeleteFullResult(Idmain);
            }
        }


        private void buttonDelAnten_Click(object sender, EventArgs e)
        {
            try
            {
                DBLoaderPackClass Pack = buttonDelAnten.Tag as DBLoaderPackClass;
                AntennOptionsClass res = DB_Loader.LoadIA(Pack);

                if (MessageBox.Show(this, "Удалить Антенну " + res.ToString(), "Удаление антенны", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //удаляем ноды результатов костыль
                    this.DellNodes(treeView2.Nodes.Find("zav" + res.id.ToString(), true));

                    DB_Loader.DeleteAntenn(res.id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Пак антенн удаляется сам, при удалении всех антенн и перезапуске"), "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected void DellNodes(TreeNode[] nodes)
        {
            foreach (TreeNode n in nodes)
            {
                n.Remove();
            }
        }

        private void buttonEditResult_Click(object sender, EventArgs e)
        {
            FrequencyElementClass fr = buttonEditResult.Tag as FrequencyElementClass;

            this.ShowTabl(true, fr, fr.ToString());
        }

        private void buttonHideFreq_Click_1(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonHidefreq.Tag as DBLoaderPackClass;

            DB_Loader.HideFrequency(Pack);
        }

        #endregion

        #region работа со спец графиком

        void linkLabelGraphLoad_Click(object sender, EventArgs e)
        {
            try
            {
                mainOptionsUserControl1.linkLabelGraphLoad.Visible = false;
                mainOptionsUserControl1.chartSpesialGraph.Visible = true;

                DBLoaderPackClass Pack = mainOptionsUserControl1.linkLabelGraphLoad.Tag as DBLoaderPackClass;

                //Загрузка результата
                ResultType_MAINClass result = DB_Loader.LoadResult(Pack, true);

                //для КУ график не строится
                if (result.MainOptions.MeasurementResultType == MeasurementTypeEnum.Коэффицент_усиления) return;

                //забиваем массив для графика
                List<PointDouble> Data = new List<PointDouble>();
                foreach (FrequencyElementClass freq in result.SelectedPolarization.FrequencyElements)
                {
                    PointDouble pointNew = new PointDouble();
                    pointNew.X = freq.Frequency;

                    if (result.MainOptions.MeasurementResultType == MeasurementTypeEnum.Поляризационная_диаграмма)
                    {
                        pointNew.Y = freq.CalculationResults.Коэффициент_Эллиптичности;
                    }
                    else
                    {
                        pointNew.Y = freq.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности;
                    }

                    Data.Add(pointNew);
                }

                //затираем прошлый график
                mainOptionsUserControl1.chartSpesialGraph.Series[0].Points.Clear();

                //некоторый костыль для возможности сравнивать эти графики
                result.MainOptions.MeasurementResultType = MeasurementTypeEnum.Коэффицент_усиления;
                result.SelectedPolarization.FrequencyElements.Clear();
                result.SelectedPolarization.FrequencyElements.Add(new FrequencyElementClass());
                //result.SelectedPolarization.SelectedFrequency.

                //заполняем график
                ChartScaleCalculatorClass ChartScaleCalculator = new ChartScaleCalculatorClass(mainOptionsUserControl1.Markers);
                foreach (PointDouble point in Data)
                {
                    ChartScaleCalculator.AddPointNew(0, point.X, point.Y, double.NaN, mainOptionsUserControl1.chartSpesialGraph);

                    //заполняем результат для сравнения
                    result.SelectedPolarization.SelectedFrequency.ResultAmpl_PhaseElements.Add(new ResultElementClass(point.X, point.Y, 0));
                }


                #region Для возможности сравнения графиков

                ShowCompareButtons(result);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка загрузки графика", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void buttonLOADFORDATE_Click(object sender, EventArgs e)
        {
            this.UnvizibleElements();

            this.treeView1.Nodes.Clear();
            this.treeView2.Nodes.Clear();

            HeandBand.TextBig = "Загрузка базы данных\r\nЭкран-М";
            HeandBand.BackColor = Color.Yellow;

            AddOwnedForm(HeandBand);
            HeandBand.Show();

            DB_Loader.LoadNodesToTree(ref treeView2, ref treeView1, DTPickerStart.Value, DTPickerStop.Value, this.checkBoxLoadAntenn.Checked);

            HeandBand.Hide();
        }

        private void SecretPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            checkBoxEditMode.Visible = !checkBoxEditMode.Visible;
            buttonALLToXML.Visible = !buttonALLToXML.Visible;
            buttonEditResult.Visible = !buttonEditResult.Visible;
            buttonHidefreq.Visible = !buttonHidefreq.Visible;

            SecretflowLayoutPanel.Visible = !SecretflowLayoutPanel.Visible;
        }



        private void buttonClone(object sender, EventArgs e)
        {
            // клонирование результата
            DBLoaderPackClass Pack = buttonHidefreq.Tag as DBLoaderPackClass;
            ResultType_MAINClass res = this.DB_Loader.LoadResult(Pack, true);


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            this.DB_Loader.SaveMainTable(res);

            sw.Stop();

            MessageBox.Show(string.Format("{1} частоты, сохранено за {0}", sw.Elapsed, res.PolarizationElements[0].FrequencyElements.Count));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (buttonXML.Tag != null)
            {
                DBLoaderPackClass Pack = buttonXML.Tag as DBLoaderPackClass;

                if (Pack.FormShowType == FormShowTypeEnum.Antenn)
                {
                    List<ResultType_MAINClass> results = DB_Loader.LoadALLResult(Pack);
                    List<IResultType_MAIN> res = new List<IResultType_MAIN>(results.ToArray());
                }
                else
                {
                    //Загрузка результата
                    ResultType_MAINClass result = DB_Loader.LoadResult(Pack, true);

                    if (result is ResultTypeClassСДНМ)
                    {
                        ResultTypeClassСДНМ resSDNM = result as ResultTypeClassСДНМ;


                        for (int i = 0; i < resSDNM.Main_Polarization.FrequencyElements.Count - 1; i++)
                        {
                            if (resSDNM.Main_Polarization.FrequencyElements[i].Frequency == 892)
                            {

                                //интерполируем результат
                                double Start, Stop, Step;
                                ResultType_MAINClass.GetCoordinatForInterpolation(resSDNM.MainOptions, out Start, out Stop, out Step);
                                Step = 0.5;
                                FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(resSDNM.Main_Polarization.FrequencyElements[i], Step, Start, Stop);

                                CalculationResultsClass.Calculate_DN_Part(tempMain);


                                //вырезаем -20 градусов
                                double напрмакс = tempMain.CalculationResults.Направление_максимума_диаграммы_направленности;
                                double напрмакс_20 = напрмакс - 20;
                                FrequencyElementClass Main_Max20 = FrequencyElementClass.InterpolationByStep(tempMain, Step, напрмакс_20, напрмакс);



                                CalculationResultsClass calNew = new CalculationResultsClass();
                                CalculationResultsClass.Calculate_DN_Part(Main_Max20, ref calNew);
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("buttonXML.Tag == null выберите результат измерения а не частоту");
            }
        }

        private void buttonAddMainCross_Click(object sender, EventArgs e)
        {
            DBLoaderPackClass Pack = buttonXML.Tag as DBLoaderPackClass;

            if (Pack.FormShowType == FormShowTypeEnum.Antenn)
            {
                //получили полный список измерений антенн
                List<ResultType_MAINClass> results = DB_Loader.LoadALLResult(Pack);

                List<IResultType_MAIN> resEnd = new List<IResultType_MAIN>();

                #region нужно найти парные измернения
                ResultType_MAINClass MainPolarization_2 = null;
                ResultType_MAINClass CrossPolarization_1 = null;

                List<PairsClass> PairsList = new List<PairsClass>();  

                #region забиваемм массив парами
                PairsList.Add(new PairsClass(7120, 7331));
                PairsList.Add(new PairsClass(7121, 7332));
                PairsList.Add(new PairsClass(7124, 7348));
                PairsList.Add(new PairsClass(7119, 7330));
                PairsList.Add(new PairsClass(7122, 7333));
                PairsList.Add(new PairsClass(7123, 7349));


                PairsList.Add(new PairsClass(7129, 7354));
                PairsList.Add(new PairsClass(7128, 7352));
                PairsList.Add(new PairsClass(7125, 7350));
                PairsList.Add(new PairsClass(7130, 7355));
                PairsList.Add(new PairsClass(7127, 7353));
                PairsList.Add(new PairsClass(7126, 7351));

                PairsList.Add(new PairsClass(7132, 7328));
                PairsList.Add(new PairsClass(7133, 7335));
                PairsList.Add(new PairsClass(7136, 7346));
                PairsList.Add(new PairsClass(7131, 7329));
                PairsList.Add(new PairsClass(7134, 7336));
                PairsList.Add(new PairsClass(7135, 7347));

                PairsList.Add(new PairsClass(7141, 7361));
                PairsList.Add(new PairsClass(7140, 7359));
                PairsList.Add(new PairsClass(7137, 7357));
                PairsList.Add(new PairsClass(7142, 7362));
                PairsList.Add(new PairsClass(7139, 7360));
                PairsList.Add(new PairsClass(7138, 7358));

                PairsList.Add(new PairsClass(7144, 7327));
                PairsList.Add(new PairsClass(7145, 7337));
                PairsList.Add(new PairsClass(7148, 7344));
                PairsList.Add(new PairsClass(7143, 7326));
                PairsList.Add(new PairsClass(7146, 7338));
                PairsList.Add(new PairsClass(7147, 7345));

                PairsList.Add(new PairsClass(7153, 7367));
                PairsList.Add(new PairsClass(7152, 7365));
                PairsList.Add(new PairsClass(7149, 7363));
                PairsList.Add(new PairsClass(7154, 7368));
                PairsList.Add(new PairsClass(7151, 7366));
                PairsList.Add(new PairsClass(7150, 7364));

                PairsList.Add(new PairsClass(7156, 7324));
                PairsList.Add(new PairsClass(7158, 7340));
                PairsList.Add(new PairsClass(7161, 7342));
                PairsList.Add(new PairsClass(7155, 7325));
                PairsList.Add(new PairsClass(7159, 7341));
                PairsList.Add(new PairsClass(7160, 7343));

                PairsList.Add(new PairsClass(7166, 7373));
                PairsList.Add(new PairsClass(7165, 7371));
                PairsList.Add(new PairsClass(7162, 7369));
                PairsList.Add(new PairsClass(7167, 7374));
                PairsList.Add(new PairsClass(7164, 7372));
                PairsList.Add(new PairsClass(7163, 7370));

                #endregion

                int pairCount = 0;

                foreach (PairsClass pair in PairsList)
                {
                    MainPolarization_2 = null;
                    CrossPolarization_1 = null;

                    foreach (ResultType_MAINClass resTemp in results)
                    {
                        if (resTemp.id == pair.N1 && CrossPolarization_1 == null)
                        {
                            CrossPolarization_1 = resTemp;
                        }

                        if (resTemp.id == pair.N2 && MainPolarization_2 == null)
                        {
                            MainPolarization_2 = resTemp;
                        }


                        if (CrossPolarization_1 != null && MainPolarization_2 != null)
                        {
                            //нашли пару считаем и выходим из цикла
                            pairCount++;

                            //переносим поляризацию майн в кроссовое измерение, тк в нем уже есть введенные параметры описания портов

                            CrossPolarization_1.PolarizationElements[0] = MainPolarization_2.PolarizationElements[0];
                            CrossPolarization_1.PolarizationElements[0].Polarization = ResultOptionsClassLibrary.SelectedPolarizationEnum.Main;
                            //делаем перерасчет суммы
                            CrossPolarization_1.ReCalculateData();
                            ResultTypeClassСДНМ sum = CrossPolarization_1 as ResultTypeClassСДНМ;
                            sum.CalculateSpesialPolarizations();

                            resEnd.Add(CrossPolarization_1); //набираем массив готовых измерений
                                                     

                            results.Remove(CrossPolarization_1);
                            results.Remove(MainPolarization_2);
                            break;
                        }

                    }
                }

                //сохраняем полученное измерения в XML в папку                
                SaverResultForm.SaveResultToXMLFileInFolder(this, resEnd);

                #endregion

            }

        }

        private class PairsClass
        {
            public PairsClass(int NN1, int NN2)
            {
                N1 = NN1;
                N2 = NN2;
            }


            public int N1 = -1;
            public int N2 = -1;
        }

       
    }

}
