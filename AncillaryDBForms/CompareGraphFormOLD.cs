using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace AncillaryDBForms
{
    public partial class CompareGraphFormOLD : Form
    {
        public CompareGraphFormOLD()
        {
            InitializeComponent();
        }

        /// <summary>
        /// конструктор с возможностью изменения заголовка
        /// </summary>
        /// <param name="NewName"></param>
        public CompareGraphFormOLD(string NewName, bool LockPolarGraph)
        {
            InitializeComponent();
            this.Text = NewName;
            this.LockPolarGraph = LockPolarGraph;
            this.previewGraphControl2.ShowLegend = false;
        }

        private List<Series> _GraphAmplSeries = new List<Series>();

        private List<Series> _GraphPhaseSeries = new List<Series>();

        /// <summary>
        /// массив данных амплитуды для графика
        /// </summary>
        public List<Series> GraphAmplSeriesList
        {
            get { return _GraphAmplSeries; }
            set { _GraphAmplSeries = value; }
        }

        /// <summary>
        /// массив данных фазы для графика
        /// </summary>
        public List<Series> GraphPhaseSeriesList
        {
            get { return _GraphPhaseSeries; }
            set { _GraphPhaseSeries = value; }
        }

        /// <summary>
        /// добавить к просмотру данные амплитуды и фазы
        /// </summary>
        /// <param name="newAmplSeries"></param>
        /// <param name="ResultTupe"></param>
        public bool AddToWath(Series newAmplSeries, Series newPhaseSeries)
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
                    this.previewGraphControl2.Visible = false;
                    this.splitContainer3.SplitterDistance = this.splitContainer3.Size.Width;
                }
            }
        }

        /// <summary>
        /// создать или обновить график
        /// </summary>
        public void CreateGraph()
        {
            this.previewGraphControl1.CreateGraph(this._GraphAmplSeries, this._LockPolarGraph);
            this.previewGraphControl2.CreateGraph(this._GraphPhaseSeries, this._LockPolarGraph);

            this.treeView1.Nodes.Clear();
            for (int i = 0; i < _GraphAmplSeries.Count; i++)
            {
                Series tempseries = _GraphAmplSeries[i];

                TreeNode tempNode = new TreeNode(tempseries.Name);

                List<Series> tempList = new List<Series>(2);
                tempList.Add(tempseries);
                tempList.Add(_GraphPhaseSeries[i]);

                tempNode.Tag = tempList;
                treeView1.Nodes.Add(tempNode);
            }
        }

        private void CompareGraphForm_Load(object sender, EventArgs e)
        {
            this.CreateGraph();
        }


        private void buttonDel_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tempNode in treeView1.Nodes)
            {
                if (tempNode.Checked)
                {
                    List<Series> tempList = tempNode.Tag as List<Series>;

                    _GraphAmplSeries.Remove(tempList[0]);
                    _GraphPhaseSeries.Remove(tempList[1]);
                }
            }

            this.CreateGraph();
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<Series> tempList = e.Node.Tag as List<Series>;

            for (int i = 0; i < previewGraphControl1.DecartChart.Series.Count; i++)
            {
                previewGraphControl1.DecartChart.Series[i].BorderWidth = 2;
                previewGraphControl1.PolarChart.Series[i].BorderWidth = 2;

                previewGraphControl2.DecartChart.Series[i].BorderWidth = 2;
                previewGraphControl2.PolarChart.Series[i].BorderWidth = 2;
            }

            try
            {
                previewGraphControl1.DecartChart.Series.FindByName(tempList[0].Name).BorderWidth = 5;
                previewGraphControl1.PolarChart.Series.FindByName(tempList[1].Name).BorderWidth = 5;

                previewGraphControl2.DecartChart.Series.FindByName(tempList[0].Name).BorderWidth = 5;
                previewGraphControl2.PolarChart.Series.FindByName(tempList[1].Name).BorderWidth = 5;
            }
            catch (NullReferenceException)
            { }
        }
    }
}
