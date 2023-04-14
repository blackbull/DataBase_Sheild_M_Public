using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;
using ResultTypesClassLibrary;
using ResultOptionsClassLibrary;
using Report_Sheild_M_Interfaces;
using System.Xml.Serialization;
using Reports;
using ReportForm;

namespace AncillaryDBForms
{
    public partial class CompareGraphForm : Form, ICompareReport
    {
        #region конструкторы и загрузщик

        public CompareGraphForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// конструктор с возможностью изменения заголовка
        /// </summary>
        /// <param name="NewName"></param>
        public CompareGraphForm(string NewName, bool LockPolarGraph)
        {
            InitializeComponent();
            this.Text = NewName;
            this.LockPolarGraph = LockPolarGraph;
            this.previewGraphControl2.ShowLegend = false;
        }

        public CompareGraphForm(string NewName, bool LockPolarGraph, ISaver_ToDataBase newDBSaver, bool LockCalculation)
            : this(NewName, LockPolarGraph)
        {
            this.DBSaver = newDBSaver;
            this.LockPCalculation = LockCalculation;
        }

        private void CompareGraphForm_Load(object sender, EventArgs e)
        {
            this.CreateGraphFromResults();
        }

        #endregion

        #region Внутренние переменные

        private List<Series> _GraphAmplSeries = new List<Series>();

        private List<Series> _GraphPhaseSeries = new List<Series>();

        /// <summary>
        /// массив данных амплитуды для графика
        /// </summary>
        protected List<Series> GraphAmplSeriesList
        {
            get { return _GraphAmplSeries; }
            set { _GraphAmplSeries = value; }
        }

        /// <summary>
        /// массив данных фазы для графика
        /// </summary>
        protected List<Series> GraphPhaseSeriesList
        {
            get { return _GraphPhaseSeries; }
            set { _GraphPhaseSeries = value; }
        }

        protected List<ResultType_MAINClass> Results = new List<ResultType_MAINClass>();

        #endregion

        #region Вспомогательные поля

        /// <summary>
        /// заголовок
        /// </summary>
        public string Text1
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        protected bool _LockPolarGraph = false;
        public bool LockPolarGraph
        {
            get { return this._LockPolarGraph; }
            set
            {
                this._LockPolarGraph = value;

                if (_LockPolarGraph)
                {
                    this.groupBoxCreateStart.Enabled = false;
                    this.previewGraphControl2.Visible = false;
                    this.splitContainer3.SplitterDistance = this.splitContainer3.Size.Width;
                }
            }
        }

        protected bool _LockPCalculation = false;
        public bool LockPCalculation
        {
            get { return this._LockPCalculation; }
            set
            {
                this._LockPCalculation = value;

                this.groupBoxCreateStart.Enabled = !_LockPCalculation;
            }
        }

        #endregion

        #region Функции добавления и лтображения графиков

        protected void CreateGraphFromResults()
        {
            this.treeView1.Nodes.Clear();
            this._GraphAmplSeries.Clear();
            this._GraphPhaseSeries.Clear();

            if (Results.Count != 0)
            {
                foreach (ResultType_MAINClass main in Results)
                {
                    string FullName = main.ToStringFullName();

                    Series tempAmpl = main.SelectedPolarization.SelectedFrequency.GetAmplSeries();
                    Series tempPhase = main.SelectedPolarization.SelectedFrequency.GetPhaseSeries();
                    tempAmpl.Name = FullName;
                    tempPhase.Name = FullName;

                    if (AddToWath(tempAmpl, tempPhase))
                    {
                        TreeNode tempNode = new TreeNode(FullName);

                        List<object> tempList = new List<object>();
                        tempList.Add(tempAmpl);
                        tempList.Add(tempPhase);
                        tempList.Add(main);

                        tempNode.Tag = tempList;
                        treeView1.Nodes.Add(tempNode);
                    }
                    else
                    {
                        //this.DeletefromResult(main, pol, freq);
                    }
                }

                this.previewGraphControl1.CreateGraph(this._GraphAmplSeries, this._LockPolarGraph);
                this.previewGraphControl2.CreateGraph(this._GraphPhaseSeries, this._LockPolarGraph);
            }
        }

        /// <summary>
        /// добавить к просмотру данные амплитуды и фазы
        /// </summary>
        /// <param name="newAmplSeries"></param>
        /// <param name="ResultTupe"></param>
        protected bool AddToWath(Series newAmplSeries, Series newPhaseSeries)
        {
            bool addWatch = true;
            foreach (Series tempser in this.GraphAmplSeriesList)
            {
                if (tempser.Name == newAmplSeries.Name)
                {
                    addWatch = false;
                    break;
                }
            }
            if (addWatch)
            {
                this.GraphAmplSeriesList.Add(newAmplSeries);
                this.GraphPhaseSeriesList.Add(newPhaseSeries);
            }

            return addWatch;
        }

        /// <summary>
        /// добавить к просмотру данные результата измерения
        /// (добавлять только по одной частоте и по одной поляризации)
        /// true - добавлено
        /// </summary>
        /// <param name="newAmplSeries"></param>
        /// <param name="ResultTupe"></param>
        public bool AddToWath(ResultType_MAINClass main)
        {
            bool addWatch = false;

            if (!this.ContainsInCompareResults(main))
            {
                addWatch = true;

                Results.Add(main);
            }

            return addWatch;
        }

        /// <summary>
        /// Содержится ли в форме сравнения
        /// (добавлять только по одной частоте и по одной поляризации)
        /// true - Содержится
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
        public bool ContainsInCompareResults(ResultType_MAINClass main)
        {
            bool ret = false;

            foreach (ResultType_MAINClass res in Results)
            {
                if (res.id == main.id)
                {
                    if (res.SelectedPolarization.id == main.SelectedPolarization.id)
                    {
                        if (res.SelectedPolarization.Polarization == main.SelectedPolarization.Polarization)
                        {
                            if (res.SelectedPolarization.SelectedFrequency.id == main.SelectedPolarization.SelectedFrequency.id)
                            {
                                if (res.SelectedPolarization.SelectedFrequency.Frequency == main.SelectedPolarization.SelectedFrequency.Frequency)
                                {
                                    ret = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }


            return ret;
        }

        #endregion

        #region обработка удаления

        /// <summary>
        /// удалить частоту из результатов измерения
        /// </summary>
        /// <param name="Main"></param>
        /// <param name="pol"></param>
        /// <param name="freq"></param>
        protected void DeletefromResult(ResultType_MAINClass Main)
        {
            this.Results.Remove(Main);
        }


        /// <summary>
        /// удаление графика с формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDel_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tempNode in treeView1.Nodes)
            {
                if (tempNode.Checked)
                {
                    List<object> tempList = tempNode.Tag as List<object>;
                    Series ampl = tempList[0] as Series;
                    Series Phase = tempList[1] as Series;
                    ResultType_MAINClass Main = tempList[2] as ResultType_MAINClass;

                    this.DeletefromResult(Main);

                    _GraphAmplSeries.Remove(ampl);
                    _GraphPhaseSeries.Remove(Phase);
                }
            }

            this.CreateGraphFromResults();

            //закрываем форму если все результаты удалены
            if (treeView1.Nodes.Count == 0)
                this.Close();
        }

        #endregion

        #region Обработка утолщения графика

        /// <summary>
        /// Создание утолщений на графике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<object> tempList = e.Node.Tag as List<object>;

            Series Ampl = tempList[0] as Series;
            Series Phase = tempList[1] as Series;

            for (int i = 0; i < previewGraphControl1.DecartChart.Series.Count; i++)
            {
                previewGraphControl1.DecartChart.Series[i].BorderWidth = 2;
                previewGraphControl1.PolarChart.Series[i].BorderWidth = 2;

                previewGraphControl2.DecartChart.Series[i].BorderWidth = 2;
                previewGraphControl2.PolarChart.Series[i].BorderWidth = 2;
            }

            try
            {
                previewGraphControl1.DecartChart.Series.FindByName(Ampl.Name).BorderWidth = 5;
                previewGraphControl1.PolarChart.Series.FindByName(Phase.Name).BorderWidth = 5;

                previewGraphControl2.DecartChart.Series.FindByName(Ampl.Name).BorderWidth = 5;
                previewGraphControl2.PolarChart.Series.FindByName(Phase.Name).BorderWidth = 5;
            }
            catch (NullReferenceException)
            { }
        }

        #endregion


        protected List<TreeNode> GetSelectedNodes()
        {
            List<TreeNode> ret = new List<TreeNode>();

            foreach (TreeNode tempNode in treeView1.Nodes)
            {
                if (tempNode.Checked)
                {
                    ret.Add(tempNode);
                }
            }

            return ret;
        }

        #region обработчики нажатия кнопок

        private void buttonSUM_Click(object sender, EventArgs e)
        {
            List<TreeNode> SelectedNodes = this.GetSelectedNodes();

            if (SelectedNodes.Count < 2)
            {
                MessageBox.Show("Выбирите 2 или более измерения ДН", "Создание ДН", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show(string.Format("Создать ДН суммы из {0} ДН ?", SelectedNodes.Count), "Создание ДН", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResultTypeClassUnion Union = new ResultTypeClassUnion(true);
                    Union.WhatIs = CalculationResultTypeEnum.Sum_by_RAZ;
                    Union.MainOptions.Name = "Суммарная ДН";

                    foreach (TreeNode tempNode in SelectedNodes)
                    {
                        List<object> tempList = tempNode.Tag as List<object>;
                        ResultType_MAINClass Main = tempList[2] as ResultType_MAINClass;

                        Union.AddToInitialResults(Main);
                    }


                    //проверяем возможность рассчёта
                    try
                    {
                        Union.ReCalculateData();
                        Union.CalculateSpesialPolarizations();
                        this.ShowInNewCompareForm(Union);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка рассчёта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonSredn_Click(object sender, EventArgs e)
        {
            List<TreeNode> SelectedNodes = this.GetSelectedNodes();

            if (SelectedNodes.Count < 2)
            {
                MessageBox.Show("Выбирите 2 или более измерения ДН", "Создание ДН", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show(string.Format("Создать усреднённую ДН из {0} ДН ?", SelectedNodes.Count), "Создание ДН", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResultTypeClassUnion Union = new ResultTypeClassUnion(true);
                    Union.WhatIs = CalculationResultTypeEnum.Averaged;
                    Union.MainOptions.Name = "Среднее ДН";

                    foreach (TreeNode tempNode in SelectedNodes)
                    {
                        List<object> tempList = tempNode.Tag as List<object>;
                        ResultType_MAINClass Main = tempList[2] as ResultType_MAINClass;

                        Union.AddToInitialResults(Main);
                    }

                    //проверяем возможность рассчёта
                    try
                    {
                        Union.ReCalculateData();
                        Union.CalculateSpesialPolarizations();
                        this.ShowInNewCompareForm(Union);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка рассчёта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonRAZN_Click(object sender, EventArgs e)
        {
            List<TreeNode> SelectedNodes = this.GetSelectedNodes();

            if (SelectedNodes.Count != 2)
            {
                MessageBox.Show("Выбирите только 2 измерения ДН", "Создание ДН", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dr = MessageBox.Show(string.Format("Произвести рассчёт \n \"{0}\" \n\n -----МИНУС----- \n\n \"{1}\" ? \n\n Нет - поменять местами", SelectedNodes[0].Text, SelectedNodes[1].Text), "Создание ДН", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes || dr == DialogResult.No)
                {
                    ResultTypeClassUnion Union = new ResultTypeClassUnion(true);
                    Union.WhatIs = CalculationResultTypeEnum.Difference;
                    Union.MainOptions.Name = "Разность ДН";

                    List<object> tempList1 = SelectedNodes[0].Tag as List<object>;
                    ResultType_MAINClass Main1 = tempList1[2] as ResultType_MAINClass;

                    List<object> tempList2 = SelectedNodes[1].Tag as List<object>;
                    ResultType_MAINClass Main2 = tempList2[2] as ResultType_MAINClass;


                    switch (dr)
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            {
                                Union.AddToInitialResults(Main1);
                                Union.AddToInitialResults(Main2);
                                break;
                            }
                        case System.Windows.Forms.DialogResult.No:
                            {
                                Union.AddToInitialResults(Main2);
                                Union.AddToInitialResults(Main1);
                                break;
                            }
                    }

                    //проверяем возможность рассчёта
                    try
                    {
                        Union.ReCalculateData();
                        Union.CalculateSpesialPolarizations();
                        this.ShowInNewCompareForm(Union);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка рассчёта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        ISaver_ToDataBase DBSaver = null;

        private void ShowInNewCompareForm(ResultTypeClassUnion Union)
        {
            PreviewUnionDNForm form = new PreviewUnionDNForm(Union, DBSaver);
            form.ShowDialog(this);
        }

        private void buttonSUMinDB_Click(object sender, EventArgs e)
        {
            List<TreeNode> SelectedNodes = this.GetSelectedNodes();

            if (SelectedNodes.Count < 2)
            {
                MessageBox.Show("Выбирите 2 или более измерения ДН", "Создание ДН", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show(string.Format("Создать ДН суммы из {0} ДН ?", SelectedNodes.Count), "Создание ДН", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResultTypeClassUnion Union = new ResultTypeClassUnion(true);
                    Union.WhatIs = CalculationResultTypeEnum.Sum_by_DB;
                    Union.MainOptions.Name = "Суммарная ДН";

                    foreach (TreeNode tempNode in SelectedNodes)
                    {
                        List<object> tempList = tempNode.Tag as List<object>;
                        ResultType_MAINClass Main = tempList[2] as ResultType_MAINClass;

                        Union.AddToInitialResults(Main);
                    }


                    //проверяем возможность рассчёта
                    try
                    {
                        Union.ReCalculateData();
                        Union.CalculateSpesialPolarizations();
                        this.ShowInNewCompareForm(Union);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка рассчёта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonProtAmplDecart_Click(object sender, EventArgs e)
        {
            ProtocolForm prForm = new ProtocolForm(this);
            prForm.ShowDialog(this);
        }

        #endregion

        #region IBaseReportResult

        string IBaseReportResult.ResultName
        {
            get
            {
                string ret = "";

                foreach (IBaseReportResult tempRes in Results)
                {
                    ret += string.Format("{0} ({1}); ", tempRes.ResultName, tempRes.Frequency);
                }

                return ret;
            }
        }

        protected string _ResultDescription = "";

        string Report_Sheild_M_Interfaces.IBaseReportResult.ResultDescription
        {
            get
            {
                return _ResultDescription;
            }
            set
            {
                _ResultDescription = value;
            }
        }

        string IBaseReportResult.Frequency
        {
            get { return ""; }
        }

        bool IBaseReportResult.ShowAntenName
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        bool IBaseReportResult.NeedHeader
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        bool IBaseReportResult.NeedMeasurementError
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        public List<ReportOptionsElementClass> OtherOptions
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        bool IBaseReportResult.NeedAdditionalData
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        string IBaseReportResult.AntenName
        {
            get { return ""; }
        }

        string IBaseReportResult.AntenaWorksNumber
        {
            get { return ""; }
        }

        string IBaseReportResult.ZondName
        {
            get { return ""; }
        }

        string IBaseReportResult.ZondWorksNumber
        {
            get { return ""; }
        }

        string IBaseReportResult.AntenaPolarizationAngle
        {
            get { return ""; }
        }

        string IBaseReportResult.AntenaPolarizationAngleEnd
        {
            get { return ""; }
        }

        string IBaseReportResult.AntenaHeight
        {
            get { return ""; }
        }

        string IBaseReportResult.AfsPPolarizationAngle
        {
            get { return ""; }
        }

        string IBaseReportResult.AfsAsimutAngle
        {
            get { return ""; }
        }

        string IBaseReportResult.Parameters_of_the_Measurement
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICompareReport

        System.IO.MemoryStream ICompareReport.ImageStreamDecartAmpl
        {
            get
            {
                return this.previewGraphControl1.GetImageStreamDecart(true, 1040, 800);
            }
        }

        #endregion

        private void buttonSelectALL_Click(object sender, EventArgs e)
        {

        }

        private void buttonSelectALL_Click_1(object sender, EventArgs e)
        {
            foreach (TreeNode tempNode in treeView1.Nodes)
            {
                tempNode.Checked = true;
            }
        }

        private void buttonDeselectALL_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tempNode in treeView1.Nodes)
            {
                tempNode.Checked = false;
            }
        }
    }
}
