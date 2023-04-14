namespace DataBase_Sheild_M
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataBase_Sheild_M.OptionsClass.ResultClass.ResultDNClass resultDNClass12 = new DataBase_Sheild_M.OptionsClass.ResultClass.ResultDNClass();
            DataBase_Sheild_M.OptionsClass.ResultClass.ResultPHClass resultPHClass12 = new DataBase_Sheild_M.OptionsClass.ResultClass.ResultPHClass();
            DataBase_Sheild_M.OptionsClass.ResultClass.ResultSDNMClass resultSDNMClass12 = new DataBase_Sheild_M.OptionsClass.ResultClass.ResultSDNMClass();
            DataBase_Sheild_M.Connection_devicesClass connection_devicesClass12 = new DataBase_Sheild_M.Connection_devicesClass();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.Sheild_M_DataBase = new DataBase_Sheild_M.DataSet1();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitGraph = new System.Windows.Forms.SplitContainer();
            this.previewGraphControl1 = new DataBase_Sheild_M.PreviewGraphControl();
            this.resultDNUserControl1 = new DataBase_Sheild_M.UserControls.ResultControls.ResultDNUserControl();
            this.resultPHUserControl1 = new DataBase_Sheild_M.UserControls.ResultControls.ResultPHUserControl();
            this.resultSDNMUserControl1 = new DataBase_Sheild_M.UserControls.ResultControls.ResultSDNMUserControl();
            this.connection_Devices_UserControl1 = new DataBase_Sheild_M.Connection_Devices_UserControl();
            this.antennOptionsUserControl1 = new DataBase_Sheild_M.AntennOptionsUserControl();
            this.mainOptionsUserControl1 = new DataBase_Sheild_M.MainOptionsUserControl();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBoxReport = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonСДНМ = new System.Windows.Forms.Button();
            this.buttonАДН = new System.Windows.Forms.Button();
            this.buttonКУ = new System.Windows.Forms.Button();
            this.buttonФДН = new System.Windows.Forms.Button();
            this.buttonПХ_Фаза = new System.Windows.Forms.Button();
            this.buttonПХ_Ампл = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAddtoWatch = new System.Windows.Forms.Button();
            this.buttonCompare = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Sheild_M_DataBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGraph)).BeginInit();
            this.splitGraph.Panel1.SuspendLayout();
            this.splitGraph.Panel2.SuspendLayout();
            this.splitGraph.SuspendLayout();
            this.groupBoxReport.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Sheild_M_DataBase
            // 
            this.Sheild_M_DataBase.DataSetName = "Sheild_M_DataBase";
            this.Sheild_M_DataBase.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(819, 795);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(267, 795);
            this.treeView1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitGraph);
            this.splitContainer2.Panel1.Controls.Add(this.connection_Devices_UserControl1);
            this.splitContainer2.Panel1.Controls.Add(this.antennOptionsUserControl1);
            this.splitContainer2.Panel1.Controls.Add(this.mainOptionsUserControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.groupBoxReport);
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(548, 795);
            this.splitContainer2.SplitterDistance = 680;
            this.splitContainer2.TabIndex = 6;
            // 
            // splitGraph
            // 
            this.splitGraph.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitGraph.Location = new System.Drawing.Point(6, 12);
            this.splitGraph.Name = "splitGraph";
            // 
            // splitGraph.Panel1
            // 
            this.splitGraph.Panel1.Controls.Add(this.previewGraphControl1);
            // 
            // splitGraph.Panel2
            // 
            this.splitGraph.Panel2.Controls.Add(this.resultDNUserControl1);
            this.splitGraph.Panel2.Controls.Add(this.resultPHUserControl1);
            this.splitGraph.Panel2.Controls.Add(this.resultSDNMUserControl1);
            this.splitGraph.Size = new System.Drawing.Size(340, 243);
            this.splitGraph.SplitterDistance = 179;
            this.splitGraph.TabIndex = 9;
            this.splitGraph.Visible = false;
            // 
            // previewGraphControl1
            // 
            this.previewGraphControl1.Location = new System.Drawing.Point(-281, 3);
            this.previewGraphControl1.MinimumSize = new System.Drawing.Size(414, 563);
            this.previewGraphControl1.Name = "previewGraphControl1";
            this.previewGraphControl1.Size = new System.Drawing.Size(414, 563);
            this.previewGraphControl1.TabIndex = 1;
            this.previewGraphControl1.Visible = false;
            // 
            // resultDNUserControl1
            // 
            this.resultDNUserControl1.Location = new System.Drawing.Point(9, 41);
            this.resultDNUserControl1.MinimumSize = new System.Drawing.Size(227, 393);
            this.resultDNUserControl1.Name = "resultDNUserControl1";
            resultDNClass12.FrequencyElement = null;
            this.resultDNUserControl1.ResultDN = resultDNClass12;
            this.resultDNUserControl1.Size = new System.Drawing.Size(228, 393);
            this.resultDNUserControl1.TabIndex = 6;
            this.resultDNUserControl1.Visible = false;
            // 
            // resultPHUserControl1
            // 
            this.resultPHUserControl1.Location = new System.Drawing.Point(3, 41);
            this.resultPHUserControl1.MinimumSize = new System.Drawing.Size(227, 266);
            this.resultPHUserControl1.Name = "resultPHUserControl1";
            resultPHClass12.FrequencyElement = null;
            this.resultPHUserControl1.ResultPH = resultPHClass12;
            this.resultPHUserControl1.Size = new System.Drawing.Size(227, 266);
            this.resultPHUserControl1.TabIndex = 7;
            this.resultPHUserControl1.Visible = false;
            // 
            // resultSDNMUserControl1
            // 
            this.resultSDNMUserControl1.Location = new System.Drawing.Point(-3, 74);
            this.resultSDNMUserControl1.MinimumSize = new System.Drawing.Size(227, 581);
            this.resultSDNMUserControl1.Name = "resultSDNMUserControl1";
            resultSDNMClass12.FrequencyElement = null;
            this.resultSDNMUserControl1.ResultSDNM = resultSDNMClass12;
            this.resultSDNMUserControl1.Size = new System.Drawing.Size(227, 581);
            this.resultSDNMUserControl1.TabIndex = 8;
            this.resultSDNMUserControl1.Visible = false;
            // 
            // connection_Devices_UserControl1
            // 
            this.connection_Devices_UserControl1.Location = new System.Drawing.Point(123, 125);
            this.connection_Devices_UserControl1.Name = "connection_Devices_UserControl1";
            this.connection_Devices_UserControl1.Saver_ToDataBase = null;
            this.connection_Devices_UserControl1.SelectedDevice = connection_devicesClass12;
            this.connection_Devices_UserControl1.Size = new System.Drawing.Size(290, 393);
            this.connection_Devices_UserControl1.TabIndex = 5;
            this.connection_Devices_UserControl1.Visible = false;
            // 
            // antennOptionsUserControl1
            // 
            this.antennOptionsUserControl1.Location = new System.Drawing.Point(-86, 287);
            this.antennOptionsUserControl1.MinimumSize = new System.Drawing.Size(278, 561);
            this.antennOptionsUserControl1.Name = "antennOptionsUserControl1";
            this.antennOptionsUserControl1.Saver_ToDataBase = null;
            this.antennOptionsUserControl1.SelectedAntenn = null;
            this.antennOptionsUserControl1.Size = new System.Drawing.Size(278, 561);
            this.antennOptionsUserControl1.TabIndex = 0;
            this.antennOptionsUserControl1.Visible = false;
            // 
            // mainOptionsUserControl1
            // 
            this.mainOptionsUserControl1.Location = new System.Drawing.Point(400, 166);
            this.mainOptionsUserControl1.MinimumSize = new System.Drawing.Size(531, 511);
            this.mainOptionsUserControl1.Name = "mainOptionsUserControl1";
            this.mainOptionsUserControl1.Size = new System.Drawing.Size(580, 511);
            this.mainOptionsUserControl1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(435, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBoxReport
            // 
            this.groupBoxReport.Controls.Add(this.flowLayoutPanel2);
            this.groupBoxReport.Location = new System.Drawing.Point(87, 3);
            this.groupBoxReport.Name = "groupBoxReport";
            this.groupBoxReport.Size = new System.Drawing.Size(218, 96);
            this.groupBoxReport.TabIndex = 10;
            this.groupBoxReport.TabStop = false;
            this.groupBoxReport.Text = "Создать отчёт";
            this.groupBoxReport.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.buttonСДНМ);
            this.flowLayoutPanel2.Controls.Add(this.buttonАДН);
            this.flowLayoutPanel2.Controls.Add(this.buttonКУ);
            this.flowLayoutPanel2.Controls.Add(this.buttonФДН);
            this.flowLayoutPanel2.Controls.Add(this.buttonПХ_Фаза);
            this.flowLayoutPanel2.Controls.Add(this.buttonПХ_Ампл);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(19, 13);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(195, 83);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // buttonСДНМ
            // 
            this.buttonСДНМ.Location = new System.Drawing.Point(3, 3);
            this.buttonСДНМ.Name = "buttonСДНМ";
            this.buttonСДНМ.Size = new System.Drawing.Size(172, 35);
            this.buttonСДНМ.TabIndex = 14;
            this.buttonСДНМ.Text = "Суммарная диаграмма направленности по мощности";
            this.buttonСДНМ.UseVisualStyleBackColor = true;
            this.buttonСДНМ.Visible = false;
            this.buttonСДНМ.Click += new System.EventHandler(this.buttonСДНМ_Click);
            // 
            // buttonАДН
            // 
            this.buttonАДН.Location = new System.Drawing.Point(3, 44);
            this.buttonАДН.Name = "buttonАДН";
            this.buttonАДН.Size = new System.Drawing.Size(172, 35);
            this.buttonАДН.TabIndex = 11;
            this.buttonАДН.Text = "Амплитудная диаграмма направленности";
            this.buttonАДН.UseVisualStyleBackColor = true;
            this.buttonАДН.Visible = false;
            this.buttonАДН.Click += new System.EventHandler(this.buttonАДН_Click);
            // 
            // buttonКУ
            // 
            this.buttonКУ.Location = new System.Drawing.Point(3, 85);
            this.buttonКУ.Name = "buttonКУ";
            this.buttonКУ.Size = new System.Drawing.Size(172, 35);
            this.buttonКУ.TabIndex = 13;
            this.buttonКУ.Text = "Коэффициент усиления";
            this.buttonКУ.UseVisualStyleBackColor = true;
            this.buttonКУ.Visible = false;
            // 
            // buttonФДН
            // 
            this.buttonФДН.Location = new System.Drawing.Point(3, 126);
            this.buttonФДН.Name = "buttonФДН";
            this.buttonФДН.Size = new System.Drawing.Size(172, 35);
            this.buttonФДН.TabIndex = 12;
            this.buttonФДН.Text = "Фазовая диаграмма направленности";
            this.buttonФДН.UseVisualStyleBackColor = true;
            this.buttonФДН.Visible = false;
            this.buttonФДН.Click += new System.EventHandler(this.buttonФДН_Click);
            // 
            // buttonПХ_Фаза
            // 
            this.buttonПХ_Фаза.Location = new System.Drawing.Point(3, 167);
            this.buttonПХ_Фаза.Name = "buttonПХ_Фаза";
            this.buttonПХ_Фаза.Size = new System.Drawing.Size(172, 35);
            this.buttonПХ_Фаза.TabIndex = 9;
            this.buttonПХ_Фаза.Text = "Фазовая поляризационная диаграмма";
            this.buttonПХ_Фаза.UseVisualStyleBackColor = true;
            this.buttonПХ_Фаза.Visible = false;
            this.buttonПХ_Фаза.Click += new System.EventHandler(this.buttonПХ_Фаза_Click);
            // 
            // buttonПХ_Ампл
            // 
            this.buttonПХ_Ампл.Location = new System.Drawing.Point(3, 208);
            this.buttonПХ_Ампл.Name = "buttonПХ_Ампл";
            this.buttonПХ_Ампл.Size = new System.Drawing.Size(172, 35);
            this.buttonПХ_Ампл.TabIndex = 8;
            this.buttonПХ_Ампл.Text = "Амплитудная поляризационная диаграмма";
            this.buttonПХ_Ампл.UseVisualStyleBackColor = true;
            this.buttonПХ_Ампл.Visible = false;
            this.buttonПХ_Ампл.Click += new System.EventHandler(this.buttonПХ_Ампл_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonAddtoWatch);
            this.flowLayoutPanel1.Controls.Add(this.buttonCompare);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(80, 81);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // buttonAddtoWatch
            // 
            this.buttonAddtoWatch.Location = new System.Drawing.Point(3, 3);
            this.buttonAddtoWatch.Name = "buttonAddtoWatch";
            this.buttonAddtoWatch.Size = new System.Drawing.Size(75, 36);
            this.buttonAddtoWatch.TabIndex = 5;
            this.buttonAddtoWatch.Text = "Добавить к сравнению";
            this.buttonAddtoWatch.UseVisualStyleBackColor = true;
            this.buttonAddtoWatch.Visible = false;
            this.buttonAddtoWatch.Click += new System.EventHandler(this.buttonAddtoWatch_Click);
            // 
            // buttonCompare
            // 
            this.buttonCompare.Location = new System.Drawing.Point(3, 45);
            this.buttonCompare.Name = "buttonCompare";
            this.buttonCompare.Size = new System.Drawing.Size(75, 36);
            this.buttonCompare.TabIndex = 6;
            this.buttonCompare.Text = "Сравнить выбранное";
            this.buttonCompare.UseVisualStyleBackColor = true;
            this.buttonCompare.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(398, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "EditMode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 795);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Sheild_M_DataBase)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitGraph.Panel1.ResumeLayout(false);
            this.splitGraph.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGraph)).EndInit();
            this.splitGraph.ResumeLayout(false);
            this.groupBoxReport.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataSet1 Sheild_M_DataBase;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private AntennOptionsUserControl antennOptionsUserControl1;
        private PreviewGraphControl previewGraphControl1;
        private MainOptionsUserControl mainOptionsUserControl1;
        private Connection_Devices_UserControl connection_Devices_UserControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonAddtoWatch;
        private System.Windows.Forms.Button buttonCompare;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.ResultControls.ResultDNUserControl resultDNUserControl1;
        private UserControls.ResultControls.ResultPHUserControl resultPHUserControl1;
        private UserControls.ResultControls.ResultSDNMUserControl resultSDNMUserControl1;
        private System.Windows.Forms.Button buttonПХ_Ампл;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonПХ_Фаза;
        private System.Windows.Forms.GroupBox groupBoxReport;
        private System.Windows.Forms.Button buttonКУ;
        private System.Windows.Forms.Button buttonАДН;
        private System.Windows.Forms.Button buttonФДН;
        private System.Windows.Forms.Button buttonСДНМ;
        private System.Windows.Forms.SplitContainer splitGraph;
        private System.Windows.Forms.Button button1;
    }
}