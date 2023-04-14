using ResultOptionsClassLibrary;
using PreviewGraphControlLibrary;
namespace DataBase_Sheild_M
{
    partial class DataBaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataBaseForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainerMENU = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxLoadAntenn = new System.Windows.Forms.CheckBox();
            this.buttonLOADFORDATE = new System.Windows.Forms.Button();
            this.DTPickerStop = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.DTPickerStart = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitGraph = new System.Windows.Forms.SplitContainer();
            this.splitContainerGraph = new System.Windows.Forms.SplitContainer();
            this.groupBoxAMPL = new System.Windows.Forms.GroupBox();
            this.previewGraphControl1 = new PreviewGraphControlLibrary.PreviewGraphControl();
            this.groupBoxPHASE = new System.Windows.Forms.GroupBox();
            this.previewGraphControl2 = new PreviewGraphControlLibrary.PreviewGraphControl();
            this.resultKYUserControl1 = new DB_Controls.ResultKYUserControl();
            this.resultDNUserControl1 = new DB_Controls.ResultDNUserControl();
            this.resultPHUserControl1 = new DB_Controls.ResultPHUserControl();
            this.resultSDNMUserControl1 = new DB_Controls.ResultSDNMUserControl();
            this.mainOptionsUserControl1 = new DB_Controls.MainOptionsUserControl();
            this.SecretflowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonEditResult = new System.Windows.Forms.Button();
            this.buttonHidefreq = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonALLToXML = new System.Windows.Forms.Button();
            this.buttonCLON = new System.Windows.Forms.Button();
            this.checkBoxEditMode = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonDelResult = new System.Windows.Forms.Button();
            this.buttonDelAnten = new System.Windows.Forms.Button();
            this.SecretPanel = new System.Windows.Forms.Panel();
            this.groupBoxReport = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonСДНМ = new System.Windows.Forms.Button();
            this.buttonАДН = new System.Windows.Forms.Button();
            this.buttonПХ_Фаза = new System.Windows.Forms.Button();
            this.buttonКУ = new System.Windows.Forms.Button();
            this.buttonФДН = new System.Windows.Forms.Button();
            this.buttonUnionDN = new System.Windows.Forms.Button();
            this.buttonПХ_Ампл = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonTXT = new System.Windows.Forms.Button();
            this.buttonXML = new System.Windows.Forms.Button();
            this.buttonExel = new System.Windows.Forms.Button();
            this.buttonCompare = new System.Windows.Forms.Button();
            this.buttonAddtoWatch = new System.Windows.Forms.Button();
            this.buttonShowTable = new System.Windows.Forms.Button();
            this.buttonPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMENU)).BeginInit();
            this.splitContainerMENU.Panel1.SuspendLayout();
            this.splitContainerMENU.Panel2.SuspendLayout();
            this.splitContainerMENU.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGraph)).BeginInit();
            this.splitGraph.Panel1.SuspendLayout();
            this.splitGraph.Panel2.SuspendLayout();
            this.splitGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGraph)).BeginInit();
            this.splitContainerGraph.Panel1.SuspendLayout();
            this.splitContainerGraph.Panel2.SuspendLayout();
            this.splitContainerGraph.SuspendLayout();
            this.groupBoxAMPL.SuspendLayout();
            this.groupBoxPHASE.SuspendLayout();
            this.SecretflowLayoutPanel.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBoxReport.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainerMENU);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1496, 787);
            this.splitContainer1.SplitterDistance = 332;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainerMENU
            // 
            this.splitContainerMENU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMENU.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMENU.IsSplitterFixed = true;
            this.splitContainerMENU.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMENU.Name = "splitContainerMENU";
            this.splitContainerMENU.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMENU.Panel1
            // 
            this.splitContainerMENU.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainerMENU.Panel2
            // 
            this.splitContainerMENU.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerMENU.Size = new System.Drawing.Size(332, 787);
            this.splitContainerMENU.SplitterDistance = 75;
            this.splitContainerMENU.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxLoadAntenn);
            this.groupBox1.Controls.Add(this.buttonLOADFORDATE);
            this.groupBox1.Controls.Add(this.DTPickerStop);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DTPickerStart);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 75);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор измерений";
            // 
            // checkBoxLoadAntenn
            // 
            this.checkBoxLoadAntenn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxLoadAntenn.AutoSize = true;
            this.checkBoxLoadAntenn.Location = new System.Drawing.Point(6, 49);
            this.checkBoxLoadAntenn.Name = "checkBoxLoadAntenn";
            this.checkBoxLoadAntenn.Size = new System.Drawing.Size(145, 17);
            this.checkBoxLoadAntenn.TabIndex = 8;
            this.checkBoxLoadAntenn.Text = "Загрузить все антенны";
            this.checkBoxLoadAntenn.UseVisualStyleBackColor = true;
            // 
            // buttonLOADFORDATE
            // 
            this.buttonLOADFORDATE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLOADFORDATE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLOADFORDATE.Location = new System.Drawing.Point(231, 45);
            this.buttonLOADFORDATE.Name = "buttonLOADFORDATE";
            this.buttonLOADFORDATE.Size = new System.Drawing.Size(92, 23);
            this.buttonLOADFORDATE.TabIndex = 7;
            this.buttonLOADFORDATE.Text = "Загрузить";
            this.buttonLOADFORDATE.UseVisualStyleBackColor = true;
            this.buttonLOADFORDATE.Click += new System.EventHandler(this.buttonLOADFORDATE_Click);
            // 
            // DTPickerStop
            // 
            this.DTPickerStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DTPickerStop.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPickerStop.Location = new System.Drawing.Point(231, 19);
            this.DTPickerStop.Name = "DTPickerStop";
            this.DTPickerStop.Size = new System.Drawing.Size(92, 20);
            this.DTPickerStop.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "С";
            // 
            // DTPickerStart
            // 
            this.DTPickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPickerStart.Location = new System.Drawing.Point(22, 19);
            this.DTPickerStart.Name = "DTPickerStart";
            this.DTPickerStart.Size = new System.Drawing.Size(92, 20);
            this.DTPickerStart.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "ПО";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(332, 708);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(324, 682);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "По антенне";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView2.Location = new System.Drawing.Point(3, 3);
            this.treeView2.Name = "treeView2";
            this.treeView2.ShowNodeToolTips = true;
            this.treeView2.Size = new System.Drawing.Size(318, 676);
            this.treeView2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(324, 682);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "По типу измерения";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(318, 676);
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
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.AutoScrollMinSize = new System.Drawing.Size(1000, 660);
            this.splitContainer2.Panel1.Controls.Add(this.splitGraph);
            this.splitContainer2.Panel1.Controls.Add(this.mainOptionsUserControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.SecretflowLayoutPanel);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel3);
            this.splitContainer2.Panel2.Controls.Add(this.SecretPanel);
            this.splitContainer2.Panel2.Controls.Add(this.groupBoxReport);
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer2.Panel2MinSize = 100;
            this.splitContainer2.Size = new System.Drawing.Size(1160, 787);
            this.splitContainer2.SplitterDistance = 663;
            this.splitContainer2.TabIndex = 6;
            // 
            // splitGraph
            // 
            this.splitGraph.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitGraph.Location = new System.Drawing.Point(19, 81);
            this.splitGraph.Name = "splitGraph";
            // 
            // splitGraph.Panel1
            // 
            this.splitGraph.Panel1.Controls.Add(this.splitContainerGraph);
            // 
            // splitGraph.Panel2
            // 
            this.splitGraph.Panel2.Controls.Add(this.resultKYUserControl1);
            this.splitGraph.Panel2.Controls.Add(this.resultDNUserControl1);
            this.splitGraph.Panel2.Controls.Add(this.resultPHUserControl1);
            this.splitGraph.Panel2.Controls.Add(this.resultSDNMUserControl1);
            this.splitGraph.Size = new System.Drawing.Size(467, 413);
            this.splitGraph.SplitterDistance = 361;
            this.splitGraph.TabIndex = 9;
            this.splitGraph.Visible = false;
            // 
            // splitContainerGraph
            // 
            this.splitContainerGraph.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitContainerGraph.Location = new System.Drawing.Point(3, 18);
            this.splitContainerGraph.Name = "splitContainerGraph";
            // 
            // splitContainerGraph.Panel1
            // 
            this.splitContainerGraph.Panel1.Controls.Add(this.groupBoxAMPL);
            // 
            // splitContainerGraph.Panel2
            // 
            this.splitContainerGraph.Panel2.Controls.Add(this.groupBoxPHASE);
            this.splitContainerGraph.Size = new System.Drawing.Size(353, 392);
            this.splitContainerGraph.SplitterDistance = 171;
            this.splitContainerGraph.TabIndex = 11;
            this.splitContainerGraph.Visible = false;
            // 
            // groupBoxAMPL
            // 
            this.groupBoxAMPL.Controls.Add(this.previewGraphControl1);
            this.groupBoxAMPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAMPL.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAMPL.Name = "groupBoxAMPL";
            this.groupBoxAMPL.Size = new System.Drawing.Size(171, 392);
            this.groupBoxAMPL.TabIndex = 2;
            this.groupBoxAMPL.TabStop = false;
            this.groupBoxAMPL.Text = "Амплитуда";
            // 
            // previewGraphControl1
            // 
            this.previewGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewGraphControl1.Location = new System.Drawing.Point(3, 16);
            this.previewGraphControl1.MinimumSize = new System.Drawing.Size(414, 563);
            this.previewGraphControl1.Name = "previewGraphControl1";
            this.previewGraphControl1.ShowLegend = true;
            this.previewGraphControl1.Size = new System.Drawing.Size(414, 563);
            this.previewGraphControl1.TabIndex = 1;
            // 
            // groupBoxPHASE
            // 
            this.groupBoxPHASE.Controls.Add(this.previewGraphControl2);
            this.groupBoxPHASE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPHASE.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPHASE.Name = "groupBoxPHASE";
            this.groupBoxPHASE.Size = new System.Drawing.Size(178, 392);
            this.groupBoxPHASE.TabIndex = 11;
            this.groupBoxPHASE.TabStop = false;
            this.groupBoxPHASE.Text = "Фаза";
            // 
            // previewGraphControl2
            // 
            this.previewGraphControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewGraphControl2.Location = new System.Drawing.Point(3, 16);
            this.previewGraphControl2.MinimumSize = new System.Drawing.Size(414, 563);
            this.previewGraphControl2.Name = "previewGraphControl2";
            this.previewGraphControl2.ShowLegend = true;
            this.previewGraphControl2.Size = new System.Drawing.Size(414, 563);
            this.previewGraphControl2.TabIndex = 10;
            // 
            // resultKYUserControl1
            // 
            this.resultKYUserControl1.Location = new System.Drawing.Point(14, 18);
            this.resultKYUserControl1.MinimumSize = new System.Drawing.Size(230, 300);
            this.resultKYUserControl1.Name = "resultKYUserControl1";
            this.resultKYUserControl1.Result = null;
            this.resultKYUserControl1.Size = new System.Drawing.Size(230, 300);
            this.resultKYUserControl1.TabIndex = 9;
            // 
            // resultDNUserControl1
            // 
            this.resultDNUserControl1.Location = new System.Drawing.Point(14, 96);
            this.resultDNUserControl1.MinimumSize = new System.Drawing.Size(227, 393);
            this.resultDNUserControl1.Name = "resultDNUserControl1";
            this.resultDNUserControl1.Result = null;
            this.resultDNUserControl1.Size = new System.Drawing.Size(228, 393);
            this.resultDNUserControl1.TabIndex = 6;
            this.resultDNUserControl1.Visible = false;
            // 
            // resultPHUserControl1
            // 
            this.resultPHUserControl1.Location = new System.Drawing.Point(3, 41);
            this.resultPHUserControl1.MinimumSize = new System.Drawing.Size(227, 266);
            this.resultPHUserControl1.Name = "resultPHUserControl1";
            this.resultPHUserControl1.Result = null;
            this.resultPHUserControl1.Size = new System.Drawing.Size(227, 266);
            this.resultPHUserControl1.TabIndex = 7;
            this.resultPHUserControl1.Visible = false;
            // 
            // resultSDNMUserControl1
            // 
            this.resultSDNMUserControl1.Location = new System.Drawing.Point(-3, 74);
            this.resultSDNMUserControl1.MinimumSize = new System.Drawing.Size(227, 581);
            this.resultSDNMUserControl1.Name = "resultSDNMUserControl1";
            this.resultSDNMUserControl1.Result = null;
            this.resultSDNMUserControl1.Size = new System.Drawing.Size(227, 581);
            this.resultSDNMUserControl1.TabIndex = 8;
            this.resultSDNMUserControl1.Visible = false;
            // 
            // mainOptionsUserControl1
            // 
            this.mainOptionsUserControl1.Location = new System.Drawing.Point(531, 30);
            this.mainOptionsUserControl1.MainResult = ((ResultOptionsClassLibrary.MainOptionsClass)(resources.GetObject("mainOptionsUserControl1.MainResult")));
            this.mainOptionsUserControl1.MinimumSize = new System.Drawing.Size(531, 511);
            this.mainOptionsUserControl1.Name = "mainOptionsUserControl1";
            this.mainOptionsUserControl1.SaverToDB = null;
            this.mainOptionsUserControl1.Size = new System.Drawing.Size(580, 511);
            this.mainOptionsUserControl1.TabIndex = 4;
            this.mainOptionsUserControl1.Visible = false;
            // 
            // SecretflowLayoutPanel
            // 
            this.SecretflowLayoutPanel.AutoScroll = true;
            this.SecretflowLayoutPanel.Controls.Add(this.buttonEditResult);
            this.SecretflowLayoutPanel.Controls.Add(this.buttonHidefreq);
            this.SecretflowLayoutPanel.Controls.Add(this.button1);
            this.SecretflowLayoutPanel.Controls.Add(this.buttonALLToXML);
            this.SecretflowLayoutPanel.Controls.Add(this.buttonCLON);
            this.SecretflowLayoutPanel.Controls.Add(this.checkBoxEditMode);
            this.SecretflowLayoutPanel.Controls.Add(this.button2);
            this.SecretflowLayoutPanel.Location = new System.Drawing.Point(801, 3);
            this.SecretflowLayoutPanel.Name = "SecretflowLayoutPanel";
            this.SecretflowLayoutPanel.Size = new System.Drawing.Size(264, 100);
            this.SecretflowLayoutPanel.TabIndex = 18;
            this.SecretflowLayoutPanel.Visible = false;
            // 
            // buttonEditResult
            // 
            this.buttonEditResult.Enabled = false;
            this.buttonEditResult.Location = new System.Drawing.Point(3, 3);
            this.buttonEditResult.Name = "buttonEditResult";
            this.buttonEditResult.Size = new System.Drawing.Size(113, 48);
            this.buttonEditResult.TabIndex = 14;
            this.buttonEditResult.Text = "Редактировать результат";
            this.buttonEditResult.UseVisualStyleBackColor = true;
            this.buttonEditResult.Visible = false;
            this.buttonEditResult.Click += new System.EventHandler(this.buttonEditResult_Click);
            // 
            // buttonHidefreq
            // 
            this.buttonHidefreq.Enabled = false;
            this.buttonHidefreq.Location = new System.Drawing.Point(122, 3);
            this.buttonHidefreq.Name = "buttonHidefreq";
            this.buttonHidefreq.Size = new System.Drawing.Size(113, 48);
            this.buttonHidefreq.TabIndex = 15;
            this.buttonHidefreq.Text = "Скрыть чатоты измерния";
            this.buttonHidefreq.UseVisualStyleBackColor = true;
            this.buttonHidefreq.Visible = false;
            this.buttonHidefreq.Click += new System.EventHandler(this.buttonHideFreq_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 48);
            this.button1.TabIndex = 18;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // buttonALLToXML
            // 
            this.buttonALLToXML.Location = new System.Drawing.Point(122, 57);
            this.buttonALLToXML.Name = "buttonALLToXML";
            this.buttonALLToXML.Size = new System.Drawing.Size(113, 48);
            this.buttonALLToXML.TabIndex = 12;
            this.buttonALLToXML.Text = "Всю базу в XML";
            this.buttonALLToXML.UseVisualStyleBackColor = true;
            this.buttonALLToXML.Visible = false;
            this.buttonALLToXML.Click += new System.EventHandler(this.buttonALLToXML_Click);
            // 
            // buttonCLON
            // 
            this.buttonCLON.Location = new System.Drawing.Point(3, 111);
            this.buttonCLON.Name = "buttonCLON";
            this.buttonCLON.Size = new System.Drawing.Size(113, 48);
            this.buttonCLON.TabIndex = 17;
            this.buttonCLON.Text = "Копировать результат";
            this.buttonCLON.UseVisualStyleBackColor = true;
            this.buttonCLON.Visible = false;
            this.buttonCLON.Click += new System.EventHandler(this.buttonClone);
            // 
            // checkBoxEditMode
            // 
            this.checkBoxEditMode.AutoSize = true;
            this.checkBoxEditMode.Location = new System.Drawing.Point(122, 111);
            this.checkBoxEditMode.Name = "checkBoxEditMode";
            this.checkBoxEditMode.Size = new System.Drawing.Size(71, 17);
            this.checkBoxEditMode.TabIndex = 4;
            this.checkBoxEditMode.Text = "EditMode";
            this.checkBoxEditMode.UseVisualStyleBackColor = true;
            this.checkBoxEditMode.Visible = false;
            this.checkBoxEditMode.CheckedChanged += new System.EventHandler(this.ModifyMode);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 48);
            this.button2.TabIndex = 19;
            this.button2.Text = "Соединить измерения main+cross";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonAddMainCross_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(865, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.buttonDelResult);
            this.flowLayoutPanel3.Controls.Add(this.buttonDelAnten);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(659, 8);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(118, 100);
            this.flowLayoutPanel3.TabIndex = 16;
            // 
            // buttonDelResult
            // 
            this.buttonDelResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonDelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDelResult.Location = new System.Drawing.Point(3, 3);
            this.buttonDelResult.Name = "buttonDelResult";
            this.buttonDelResult.Size = new System.Drawing.Size(113, 48);
            this.buttonDelResult.TabIndex = 13;
            this.buttonDelResult.Text = "Удалить результат";
            this.buttonDelResult.UseVisualStyleBackColor = false;
            this.buttonDelResult.Visible = false;
            this.buttonDelResult.Click += new System.EventHandler(this.buttonDelResult_Click);
            // 
            // buttonDelAnten
            // 
            this.buttonDelAnten.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonDelAnten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDelAnten.Location = new System.Drawing.Point(3, 57);
            this.buttonDelAnten.Name = "buttonDelAnten";
            this.buttonDelAnten.Size = new System.Drawing.Size(113, 48);
            this.buttonDelAnten.TabIndex = 14;
            this.buttonDelAnten.Text = "Удалить Антенну";
            this.buttonDelAnten.UseVisualStyleBackColor = false;
            this.buttonDelAnten.Visible = false;
            this.buttonDelAnten.Click += new System.EventHandler(this.buttonDelAnten_Click);
            // 
            // SecretPanel
            // 
            this.SecretPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SecretPanel.Location = new System.Drawing.Point(1110, 3);
            this.SecretPanel.Name = "SecretPanel";
            this.SecretPanel.Size = new System.Drawing.Size(47, 40);
            this.SecretPanel.TabIndex = 11;
            this.SecretPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SecretPanel_MouseDoubleClick);
            // 
            // groupBoxReport
            // 
            this.groupBoxReport.Controls.Add(this.flowLayoutPanel2);
            this.groupBoxReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxReport.Location = new System.Drawing.Point(265, 3);
            this.groupBoxReport.Name = "groupBoxReport";
            this.groupBoxReport.Size = new System.Drawing.Size(388, 111);
            this.groupBoxReport.TabIndex = 10;
            this.groupBoxReport.TabStop = false;
            this.groupBoxReport.Text = "Сформировать протокол";
            this.groupBoxReport.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.buttonАДН);
            this.flowLayoutPanel2.Controls.Add(this.buttonПХ_Ампл);
            this.flowLayoutPanel2.Controls.Add(this.buttonСДНМ);
            this.flowLayoutPanel2.Controls.Add(this.buttonUnionDN);
            this.flowLayoutPanel2.Controls.Add(this.buttonПХ_Фаза);
            this.flowLayoutPanel2.Controls.Add(this.buttonФДН);
            this.flowLayoutPanel2.Controls.Add(this.buttonКУ);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(6, 22);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(375, 80);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // buttonСДНМ
            // 
            this.buttonСДНМ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonСДНМ.Location = new System.Drawing.Point(3, 55);
            this.buttonСДНМ.Name = "buttonСДНМ";
            this.buttonСДНМ.Size = new System.Drawing.Size(172, 46);
            this.buttonСДНМ.TabIndex = 1;
            this.buttonСДНМ.Text = "Суммарная ДН по мощности";
            this.buttonСДНМ.UseVisualStyleBackColor = true;
            this.buttonСДНМ.Visible = false;
            this.buttonСДНМ.Click += new System.EventHandler(this.buttonСДНМ_Click);
            // 
            // buttonАДН
            // 
            this.buttonАДН.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonАДН.Location = new System.Drawing.Point(3, 3);
            this.buttonАДН.Name = "buttonАДН";
            this.buttonАДН.Size = new System.Drawing.Size(172, 46);
            this.buttonАДН.TabIndex = 2;
            this.buttonАДН.Text = "Амплитудная ДН";
            this.buttonАДН.UseVisualStyleBackColor = true;
            this.buttonАДН.Visible = false;
            this.buttonАДН.Click += new System.EventHandler(this.buttonАДН_Click);
            // 
            // buttonПХ_Фаза
            // 
            this.buttonПХ_Фаза.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonПХ_Фаза.Location = new System.Drawing.Point(3, 107);
            this.buttonПХ_Фаза.Name = "buttonПХ_Фаза";
            this.buttonПХ_Фаза.Size = new System.Drawing.Size(172, 46);
            this.buttonПХ_Фаза.TabIndex = 3;
            this.buttonПХ_Фаза.Text = "Фазовая ПД";
            this.buttonПХ_Фаза.UseVisualStyleBackColor = true;
            this.buttonПХ_Фаза.Visible = false;
            this.buttonПХ_Фаза.Click += new System.EventHandler(this.buttonПХ_Фаза_Click);
            // 
            // buttonКУ
            // 
            this.buttonКУ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonКУ.Location = new System.Drawing.Point(3, 159);
            this.buttonКУ.Name = "buttonКУ";
            this.buttonКУ.Size = new System.Drawing.Size(172, 46);
            this.buttonКУ.TabIndex = 13;
            this.buttonКУ.Text = "Коэффициент усиления";
            this.buttonКУ.UseVisualStyleBackColor = true;
            this.buttonКУ.Visible = false;
            this.buttonКУ.Click += new System.EventHandler(this.buttonКУ_Click);
            // 
            // buttonФДН
            // 
            this.buttonФДН.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonФДН.Location = new System.Drawing.Point(181, 107);
            this.buttonФДН.Name = "buttonФДН";
            this.buttonФДН.Size = new System.Drawing.Size(172, 46);
            this.buttonФДН.TabIndex = 5;
            this.buttonФДН.Text = "Фазовая ДН";
            this.buttonФДН.UseVisualStyleBackColor = true;
            this.buttonФДН.Visible = false;
            this.buttonФДН.Click += new System.EventHandler(this.buttonФДН_Click);
            // 
            // buttonUnionDN
            // 
            this.buttonUnionDN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUnionDN.Location = new System.Drawing.Point(181, 55);
            this.buttonUnionDN.Name = "buttonUnionDN";
            this.buttonUnionDN.Size = new System.Drawing.Size(172, 46);
            this.buttonUnionDN.TabIndex = 6;
            this.buttonUnionDN.Text = "Суммарная ДН";
            this.buttonUnionDN.UseVisualStyleBackColor = true;
            this.buttonUnionDN.Visible = false;
            this.buttonUnionDN.Click += new System.EventHandler(this.buttonUnionDN_Click);
            // 
            // buttonПХ_Ампл
            // 
            this.buttonПХ_Ампл.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonПХ_Ампл.Location = new System.Drawing.Point(181, 3);
            this.buttonПХ_Ампл.Name = "buttonПХ_Ампл";
            this.buttonПХ_Ампл.Size = new System.Drawing.Size(172, 46);
            this.buttonПХ_Ампл.TabIndex = 4;
            this.buttonПХ_Ампл.Text = "Амплитудная ПД";
            this.buttonПХ_Ампл.UseVisualStyleBackColor = true;
            this.buttonПХ_Ампл.Visible = false;
            this.buttonПХ_Ампл.Click += new System.EventHandler(this.buttonПХ_Ампл_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.buttonTXT);
            this.flowLayoutPanel1.Controls.Add(this.buttonXML);
            this.flowLayoutPanel1.Controls.Add(this.buttonExel);
            this.flowLayoutPanel1.Controls.Add(this.buttonCompare);
            this.flowLayoutPanel1.Controls.Add(this.buttonAddtoWatch);
            this.flowLayoutPanel1.Controls.Add(this.buttonShowTable);
            this.flowLayoutPanel1.Controls.Add(this.buttonPDF);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(256, 111);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // buttonTXT
            // 
            this.buttonTXT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTXT.Location = new System.Drawing.Point(3, 3);
            this.buttonTXT.Name = "buttonTXT";
            this.buttonTXT.Size = new System.Drawing.Size(113, 48);
            this.buttonTXT.TabIndex = 12;
            this.buttonTXT.Text = "Экспорт в TXT";
            this.buttonTXT.UseVisualStyleBackColor = true;
            this.buttonTXT.Visible = false;
            this.buttonTXT.Click += new System.EventHandler(this.buttonTXT_Click);
            // 
            // buttonXML
            // 
            this.buttonXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonXML.Location = new System.Drawing.Point(122, 3);
            this.buttonXML.Name = "buttonXML";
            this.buttonXML.Size = new System.Drawing.Size(113, 48);
            this.buttonXML.TabIndex = 13;
            this.buttonXML.Text = "Экспорт в XML";
            this.buttonXML.UseVisualStyleBackColor = true;
            this.buttonXML.Visible = false;
            this.buttonXML.Click += new System.EventHandler(this.buttonXML_Click);
            // 
            // buttonExel
            // 
            this.buttonExel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExel.Location = new System.Drawing.Point(3, 57);
            this.buttonExel.Name = "buttonExel";
            this.buttonExel.Size = new System.Drawing.Size(113, 48);
            this.buttonExel.TabIndex = 18;
            this.buttonExel.Text = "Экспорт в Excel";
            this.buttonExel.UseVisualStyleBackColor = true;
            this.buttonExel.Visible = false;
            this.buttonExel.Click += new System.EventHandler(this.buttonExel_Click);
            // 
            // buttonCompare
            // 
            this.buttonCompare.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCompare.Location = new System.Drawing.Point(122, 57);
            this.buttonCompare.Name = "buttonCompare";
            this.buttonCompare.Size = new System.Drawing.Size(113, 48);
            this.buttonCompare.TabIndex = 6;
            this.buttonCompare.Text = "Сравнить выбранное";
            this.buttonCompare.UseVisualStyleBackColor = true;
            this.buttonCompare.Visible = false;
            this.buttonCompare.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonAddtoWatch
            // 
            this.buttonAddtoWatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddtoWatch.Location = new System.Drawing.Point(3, 111);
            this.buttonAddtoWatch.Name = "buttonAddtoWatch";
            this.buttonAddtoWatch.Size = new System.Drawing.Size(113, 48);
            this.buttonAddtoWatch.TabIndex = 5;
            this.buttonAddtoWatch.Text = "Добавить к сравнению";
            this.buttonAddtoWatch.UseVisualStyleBackColor = true;
            this.buttonAddtoWatch.Visible = false;
            this.buttonAddtoWatch.Click += new System.EventHandler(this.buttonAddtoWatch_Click);
            // 
            // buttonShowTable
            // 
            this.buttonShowTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShowTable.Location = new System.Drawing.Point(122, 111);
            this.buttonShowTable.Name = "buttonShowTable";
            this.buttonShowTable.Size = new System.Drawing.Size(113, 48);
            this.buttonShowTable.TabIndex = 7;
            this.buttonShowTable.Text = "Таблица значений";
            this.buttonShowTable.UseVisualStyleBackColor = true;
            this.buttonShowTable.Visible = false;
            this.buttonShowTable.Click += new System.EventHandler(this.buttonShowTable_Click);
            // 
            // buttonPDF
            // 
            this.buttonPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPDF.Location = new System.Drawing.Point(3, 165);
            this.buttonPDF.Name = "buttonPDF";
            this.buttonPDF.Size = new System.Drawing.Size(113, 48);
            this.buttonPDF.TabIndex = 19;
            this.buttonPDF.Text = "Протоколы в PDF";
            this.buttonPDF.UseVisualStyleBackColor = true;
            this.buttonPDF.Visible = false;
            this.buttonPDF.Click += new System.EventHandler(this.ButtonPDF_Click);
            // 
            // DataBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1496, 787);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataBaseForm";
            this.Text = "АРМ";
            this.Load += new System.EventHandler(this.DataBaseForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainerMENU.Panel1.ResumeLayout(false);
            this.splitContainerMENU.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMENU)).EndInit();
            this.splitContainerMENU.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitGraph.Panel1.ResumeLayout(false);
            this.splitGraph.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGraph)).EndInit();
            this.splitGraph.ResumeLayout(false);
            this.splitContainerGraph.Panel1.ResumeLayout(false);
            this.splitContainerGraph.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGraph)).EndInit();
            this.splitContainerGraph.ResumeLayout(false);
            this.groupBoxAMPL.ResumeLayout(false);
            this.groupBoxPHASE.ResumeLayout(false);
            this.SecretflowLayoutPanel.ResumeLayout(false);
            this.SecretflowLayoutPanel.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.groupBoxReport.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private PreviewGraphControl previewGraphControl1;
        private DB_Controls.MainOptionsUserControl mainOptionsUserControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox checkBoxEditMode;
        private System.Windows.Forms.Button buttonAddtoWatch;
        private System.Windows.Forms.Button buttonCompare;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DB_Controls.ResultDNUserControl resultDNUserControl1;
        private DB_Controls.ResultPHUserControl resultPHUserControl1;
        private DB_Controls.ResultSDNMUserControl resultSDNMUserControl1;
        private System.Windows.Forms.Button buttonПХ_Ампл;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonПХ_Фаза;
        private System.Windows.Forms.GroupBox groupBoxReport;
        private System.Windows.Forms.Button buttonКУ;
        private System.Windows.Forms.Button buttonАДН;
        private System.Windows.Forms.Button buttonФДН;
        private System.Windows.Forms.Button buttonСДНМ;
        private System.Windows.Forms.SplitContainer splitGraph;
        private PreviewGraphControl previewGraphControl2;
        private System.Windows.Forms.SplitContainer splitContainerGraph;
        private System.Windows.Forms.GroupBox groupBoxAMPL;
        private System.Windows.Forms.GroupBox groupBoxPHASE;
        private System.Windows.Forms.Button buttonUnionDN;
        private DB_Controls.ResultKYUserControl resultKYUserControl1;
        private System.Windows.Forms.Panel SecretPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonTXT;
        private System.Windows.Forms.Button buttonShowTable;
        private System.Windows.Forms.Button buttonXML;
        private System.Windows.Forms.Button buttonDelResult;
        private System.Windows.Forms.Button buttonALLToXML;
        private System.Windows.Forms.Button buttonEditResult;
        private System.Windows.Forms.Button buttonHidefreq;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button buttonDelAnten;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxLoadAntenn;
        private System.Windows.Forms.Button buttonLOADFORDATE;
        private System.Windows.Forms.DateTimePicker DTPickerStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DTPickerStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainerMENU;
        private System.Windows.Forms.Button buttonCLON;
        private System.Windows.Forms.Button buttonExel;
        private System.Windows.Forms.FlowLayoutPanel SecretflowLayoutPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonPDF;
    }
}