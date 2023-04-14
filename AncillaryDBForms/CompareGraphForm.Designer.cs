using PreviewGraphControlLibrary;
namespace AncillaryDBForms
{
    partial class CompareGraphForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonDel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonProtAmplDecart = new System.Windows.Forms.Button();
            this.groupBoxCreateStart = new System.Windows.Forms.GroupBox();
            this.buttonSUMinDB = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSUMinRAZ = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBoxAMPL = new System.Windows.Forms.GroupBox();
            this.previewGraphControl1 = new PreviewGraphControlLibrary.PreviewGraphControl();
            this.groupBoxPhase = new System.Windows.Forms.GroupBox();
            this.previewGraphControl2 = new PreviewGraphControlLibrary.PreviewGraphControl();
            this.buttonDeselectALL = new System.Windows.Forms.Button();
            this.buttonSelectALL = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxCreateStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBoxAMPL.SuspendLayout();
            this.groupBoxPhase.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1257, 616);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.buttonDeselectALL);
            this.splitContainer2.Panel2.Controls.Add(this.buttonSelectALL);
            this.splitContainer2.Panel2.Controls.Add(this.buttonDel);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.groupBoxCreateStart);
            this.splitContainer2.Size = new System.Drawing.Size(291, 616);
            this.splitContainer2.SplitterDistance = 367;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(291, 367);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // buttonDel
            // 
            this.buttonDel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDel.Location = new System.Drawing.Point(0, 34);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(291, 55);
            this.buttonDel.TabIndex = 0;
            this.buttonDel.Text = "Убрать из сравнения";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonProtAmplDecart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 59);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Создать протокол сравнения:";
            // 
            // buttonProtAmplDecart
            // 
            this.buttonProtAmplDecart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProtAmplDecart.Location = new System.Drawing.Point(3, 16);
            this.buttonProtAmplDecart.Name = "buttonProtAmplDecart";
            this.buttonProtAmplDecart.Size = new System.Drawing.Size(285, 40);
            this.buttonProtAmplDecart.TabIndex = 0;
            this.buttonProtAmplDecart.Text = "Амплитуды (декарт)";
            this.buttonProtAmplDecart.UseVisualStyleBackColor = true;
            this.buttonProtAmplDecart.Click += new System.EventHandler(this.buttonProtAmplDecart_Click);
            // 
            // groupBoxCreateStart
            // 
            this.groupBoxCreateStart.Controls.Add(this.buttonSUMinDB);
            this.groupBoxCreateStart.Controls.Add(this.button2);
            this.groupBoxCreateStart.Controls.Add(this.button1);
            this.groupBoxCreateStart.Controls.Add(this.buttonSUMinRAZ);
            this.groupBoxCreateStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxCreateStart.Enabled = false;
            this.groupBoxCreateStart.Location = new System.Drawing.Point(0, 148);
            this.groupBoxCreateStart.Name = "groupBoxCreateStart";
            this.groupBoxCreateStart.Size = new System.Drawing.Size(291, 97);
            this.groupBoxCreateStart.TabIndex = 0;
            this.groupBoxCreateStart.TabStop = false;
            this.groupBoxCreateStart.Text = "Создать ДН:";
            // 
            // buttonSUMinDB
            // 
            this.buttonSUMinDB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSUMinDB.Location = new System.Drawing.Point(153, 15);
            this.buttonSUMinDB.Name = "buttonSUMinDB";
            this.buttonSUMinDB.Size = new System.Drawing.Size(132, 39);
            this.buttonSUMinDB.TabIndex = 3;
            this.buttonSUMinDB.Text = "Суммы в ДБ";
            this.buttonSUMinDB.UseVisualStyleBackColor = true;
            this.buttonSUMinDB.Click += new System.EventHandler(this.buttonSUMinDB_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button2.Location = new System.Drawing.Point(153, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 39);
            this.button2.TabIndex = 2;
            this.button2.Text = "Среднюю";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonSredn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "Разности";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonRAZN_Click);
            // 
            // buttonSUMinRAZ
            // 
            this.buttonSUMinRAZ.Location = new System.Drawing.Point(10, 15);
            this.buttonSUMinRAZ.Name = "buttonSUMinRAZ";
            this.buttonSUMinRAZ.Size = new System.Drawing.Size(132, 39);
            this.buttonSUMinRAZ.TabIndex = 0;
            this.buttonSUMinRAZ.Text = "Суммы в разах";
            this.buttonSUMinRAZ.UseVisualStyleBackColor = true;
            this.buttonSUMinRAZ.Click += new System.EventHandler(this.buttonSUM_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBoxAMPL);
            this.splitContainer3.Panel1MinSize = 0;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBoxPhase);
            this.splitContainer3.Panel2MinSize = 0;
            this.splitContainer3.Size = new System.Drawing.Size(962, 616);
            this.splitContainer3.SplitterDistance = 464;
            this.splitContainer3.SplitterWidth = 6;
            this.splitContainer3.TabIndex = 1;
            // 
            // groupBoxAMPL
            // 
            this.groupBoxAMPL.Controls.Add(this.previewGraphControl1);
            this.groupBoxAMPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAMPL.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAMPL.Name = "groupBoxAMPL";
            this.groupBoxAMPL.Size = new System.Drawing.Size(464, 616);
            this.groupBoxAMPL.TabIndex = 1;
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
            this.previewGraphControl1.Size = new System.Drawing.Size(458, 597);
            this.previewGraphControl1.TabIndex = 0;
            // 
            // groupBoxPhase
            // 
            this.groupBoxPhase.Controls.Add(this.previewGraphControl2);
            this.groupBoxPhase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPhase.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPhase.Name = "groupBoxPhase";
            this.groupBoxPhase.Size = new System.Drawing.Size(492, 616);
            this.groupBoxPhase.TabIndex = 2;
            this.groupBoxPhase.TabStop = false;
            this.groupBoxPhase.Text = "Фаза";
            // 
            // previewGraphControl2
            // 
            this.previewGraphControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewGraphControl2.Location = new System.Drawing.Point(3, 16);
            this.previewGraphControl2.MinimumSize = new System.Drawing.Size(414, 563);
            this.previewGraphControl2.Name = "previewGraphControl2";
            this.previewGraphControl2.ShowLegend = true;
            this.previewGraphControl2.Size = new System.Drawing.Size(486, 597);
            this.previewGraphControl2.TabIndex = 1;
            // 
            // buttonDeselectALL
            // 
            this.buttonDeselectALL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonDeselectALL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDeselectALL.Location = new System.Drawing.Point(153, 3);
            this.buttonDeselectALL.Name = "buttonDeselectALL";
            this.buttonDeselectALL.Size = new System.Drawing.Size(104, 31);
            this.buttonDeselectALL.TabIndex = 23;
            this.buttonDeselectALL.Text = "Снять выбор";
            this.buttonDeselectALL.UseVisualStyleBackColor = true;
            this.buttonDeselectALL.Click += new System.EventHandler(this.buttonDeselectALL_Click);
            // 
            // buttonSelectALL
            // 
            this.buttonSelectALL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSelectALL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectALL.Location = new System.Drawing.Point(38, 2);
            this.buttonSelectALL.Name = "buttonSelectALL";
            this.buttonSelectALL.Size = new System.Drawing.Size(104, 31);
            this.buttonSelectALL.TabIndex = 22;
            this.buttonSelectALL.Text = "Выбрать все";
            this.buttonSelectALL.UseVisualStyleBackColor = true;
            this.buttonSelectALL.Click += new System.EventHandler(this.buttonSelectALL_Click_1);
            // 
            // CompareGraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 616);
            this.Controls.Add(this.splitContainer1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(889, 654);
            this.Name = "CompareGraphForm";
            this.Text = "CompareGraphForm";
            this.Load += new System.EventHandler(this.CompareGraphForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxCreateStart.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBoxAMPL.ResumeLayout(false);
            this.groupBoxPhase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView1;
        private PreviewGraphControl previewGraphControl1;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBoxAMPL;
        private System.Windows.Forms.GroupBox groupBoxPhase;
        private PreviewGraphControl previewGraphControl2;
        private System.Windows.Forms.GroupBox groupBoxCreateStart;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonSUMinRAZ;
        private System.Windows.Forms.Button buttonSUMinDB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonProtAmplDecart;
        private System.Windows.Forms.Button buttonDeselectALL;
        private System.Windows.Forms.Button buttonSelectALL;
    }
}