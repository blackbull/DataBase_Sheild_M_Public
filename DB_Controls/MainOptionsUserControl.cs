using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResultOptionsClassLibrary;
using ChartScaleCalculatorClassLibrary;

namespace DB_Controls
{
    public partial class MainOptionsUserControl : UserControl
    {
        public MainOptionsUserControl()
        {
            InitializeComponent();
            this.LockAll();

            ChartScaleCalculatorClass.SetOptimalOptionsFor_DecartChart(chartSpesialGraph.ChartAreas[0]);
            Markers = new SelectedMarkerChartClass(chartSpesialGraph);            
        }

       public SelectedMarkerChartClass Markers;
        
        MainOptionsClass _MainResult = new MainOptionsClass();

        protected ISaver_ToDataBase _SaverToDB = null;

        public ISaver_ToDataBase SaverToDB
        {
            get
            {
                return _SaverToDB;
            }
            set
            {
                _SaverToDB = value;

                this.antennIA.Saver_ToDataBase = _SaverToDB;
                this.antennTA.Saver_ToDataBase = _SaverToDB;
            }
        }

        /// <summary>
        /// данные для отображения в этом контролле
        /// </summary>
        public MainOptionsClass MainResult
        {
            get { return _MainResult; }
            set
            {
                if (value != null)
                {
                    _MainResult = value;

                    this.textBoxName.Text = _MainResult.Name;
                    this.dateTimePicker1.Value = _MainResult.Date;

                    #region тип результата
                    this.textBoxResultTypeName.Text = _MainResult.MeasurementResultTypeName;
                    this.textBoxResutTypeDescriptions.Text = _MainResult.MeasurementResultTypeDescription;
                    #endregion

                    #region параметры
                    this.textBoxStartOPUY.Text = Math.Round(_MainResult.Parameters.StartOPU_Y, 2).ToString() + " °";
                    this.textBoxStartOpuW.Text = Math.Round(_MainResult.Parameters.StartOPU_W, 2).ToString() + " °";
                    this.textBoxStartTowerY.Text = Math.Round(_MainResult.Parameters.StartTower_Y, 2).ToString() + " м";
                    this.textBoxStartTowerW.Text = Math.Round(_MainResult.Parameters.StartTower_W, 2).ToString() + " °";

                    this.textBoxStopOPUY.Text = Math.Round(_MainResult.Parameters.StopOPU_Y, 2).ToString() + " °";
                    this.textBoxStopOPUW.Text = Math.Round(_MainResult.Parameters.StopOPU_W, 2).ToString() + " °";
                    this.textBoxStopTowerY.Text = Math.Round(_MainResult.Parameters.StopTower_Y, 2).ToString() + " м";
                    this.textBoxStopTowerW.Text = Math.Round(_MainResult.Parameters.StopTower_W, 2).ToString() + " °";

                    this.textBoxStep.Text = Math.Round(_MainResult.Parameters.StepMeasurement, 2).ToString() + " °";

                    this.textBoxPower.Text = _MainResult.Parameters.Power.ToString();
                    this.textBoxBandwidth.Text = _MainResult.Parameters.Bandwidth.ToString();


                    this.textBoxS11_S22.Text = _MainResult.Parameters.MesurementS__Type.ToString();

                    if (_MainResult.Parameters.SweepTimeAuto)
                    {
                        this.textBoxSweepTime.Text = Math.Round(_MainResult.Parameters.SweepTime, 2).ToString() + " (авто)";
                    }
                    else
                    {
                        this.textBoxSweepTime.Text = Math.Round(_MainResult.Parameters.SweepTime, 2).ToString();
                    }
                    #endregion

                    #region сегментная таблица

                    this.dataGridView2.Rows.Clear();

                    DataGridViewRow temprow;
                    DataGridViewTextBoxCell tempsell1;
                    DataGridViewTextBoxCell tempsell2;
                    DataGridViewTextBoxCell tempsell3;
                    DataGridViewTextBoxCell tempsell4;
                    for (int i = 0; i < _MainResult.Parameters.SegmentTable.Count; i++)
                    {
                        #region заполнение строки
                        temprow = new DataGridViewRow();
                        tempsell1 = new DataGridViewTextBoxCell();
                        tempsell2 = new DataGridViewTextBoxCell();
                        tempsell3 = new DataGridViewTextBoxCell();
                        tempsell4 = new DataGridViewTextBoxCell();

                        tempsell1.Value = i + 1;
                        tempsell2.Value = Math.Round(_MainResult.Parameters.SegmentTable[i].FrequencieStart, 2);
                        tempsell3.Value = Math.Round(_MainResult.Parameters.SegmentTable[i].FrequencieStop, 2);
                        tempsell4.Value = _MainResult.Parameters.SegmentTable[i].NumberOfPoint;


                        temprow.Cells.Add(tempsell1);
                        temprow.Cells.Add(tempsell2);
                        temprow.Cells.Add(tempsell3);
                        temprow.Cells.Add(tempsell4);

                        #endregion

                        this.dataGridView2.Rows.Add(temprow);
                    }
                    #endregion

                    linkLabelGraphLoad.Visible = true;
                    chartSpesialGraph.Visible = false;
                }
            }
        }

        /// <summary>
        /// заблокировать контрол от изменения
        /// </summary>
        /// <param name="Lock">true - заблокировать</param>
        public void LockUnLockAll(bool Lock)
        {
            this.dateTimePicker1.Enabled = !Lock;
            this.textBoxName.ReadOnly = Lock;

            this.textBoxStartOpuW.ReadOnly = Lock;
            this.textBoxStartOPUY.ReadOnly = Lock;
            this.textBoxStartTowerW.ReadOnly = Lock;
            this.textBoxStartTowerY.ReadOnly = Lock;

            this.textBoxStopOPUW.ReadOnly = Lock;
            this.textBoxStopOPUY.ReadOnly = Lock;
            this.textBoxStopTowerW.ReadOnly = Lock;
            this.textBoxStopTowerY.ReadOnly = Lock;

            this.textBoxStep.ReadOnly = Lock;

            this.textBoxSweepTime.ReadOnly = Lock;
            this.textBoxS11_S22.ReadOnly = Lock;
            this.textBoxPower.ReadOnly = Lock;
            this.textBoxBandwidth.ReadOnly = Lock;

            this.textBoxResultTypeName.ReadOnly = Lock;
            this.textBoxResutTypeDescriptions.ReadOnly = Lock;

            this.dataGridView2.ReadOnly = Lock;

            this.antennIA.LockUnlockAll(Lock);
            this.antennTA.LockUnlockAll(Lock);
        }
        public void LockAll()
        {
            this.LockUnLockAll(true);
        }
        public void UnLockAll()
        {
            this.LockUnLockAll(false);
        }

    }
}
