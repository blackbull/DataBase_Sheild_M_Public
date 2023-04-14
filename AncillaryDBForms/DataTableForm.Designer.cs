namespace AncillaryDBForms
{
    partial class DataTableForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonAddAmpl = new System.Windows.Forms.Button();
            this.buttonAddPhase = new System.Windows.Forms.Button();
            this.buttonAddCoord = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonReversDN = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(453, 389);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.form1_KeyUp);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Координата";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Амплитуда";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Фаза";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(9, 28);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 34);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "Сохранить изменения";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(6, 34);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(75, 20);
            this.numericUpDown1.TabIndex = 2;
            // 
            // buttonAddAmpl
            // 
            this.buttonAddAmpl.Location = new System.Drawing.Point(90, 9);
            this.buttonAddAmpl.Name = "buttonAddAmpl";
            this.buttonAddAmpl.Size = new System.Drawing.Size(151, 50);
            this.buttonAddAmpl.TabIndex = 3;
            this.buttonAddAmpl.Text = "Амплитудам";
            this.buttonAddAmpl.UseVisualStyleBackColor = true;
            this.buttonAddAmpl.Click += new System.EventHandler(this.buttonAddData_Click);
            // 
            // buttonAddPhase
            // 
            this.buttonAddPhase.Location = new System.Drawing.Point(184, 65);
            this.buttonAddPhase.Name = "buttonAddPhase";
            this.buttonAddPhase.Size = new System.Drawing.Size(57, 26);
            this.buttonAddPhase.TabIndex = 4;
            this.buttonAddPhase.Text = "Фазам";
            this.buttonAddPhase.UseVisualStyleBackColor = true;
            this.buttonAddPhase.Click += new System.EventHandler(this.buttonAddData_Click);
            // 
            // buttonAddCoord
            // 
            this.buttonAddCoord.Location = new System.Drawing.Point(90, 65);
            this.buttonAddCoord.Name = "buttonAddCoord";
            this.buttonAddCoord.Size = new System.Drawing.Size(88, 26);
            this.buttonAddCoord.TabIndex = 5;
            this.buttonAddCoord.Text = "Координатам";
            this.buttonAddCoord.UseVisualStyleBackColor = true;
            this.buttonAddCoord.Click += new System.EventHandler(this.buttonAddData_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.buttonOK);
            this.splitContainer1.Panel1Collapsed = true;
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(453, 389);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonReversDN);
            this.groupBox1.Controls.Add(this.buttonAddAmpl);
            this.groupBox1.Controls.Add(this.buttonAddCoord);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.buttonAddPhase);
            this.groupBox1.Location = new System.Drawing.Point(95, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 94);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Добавить к";
            // 
            // buttonReversDN
            // 
            this.buttonReversDN.Location = new System.Drawing.Point(247, 25);
            this.buttonReversDN.Name = "buttonReversDN";
            this.buttonReversDN.Size = new System.Drawing.Size(97, 43);
            this.buttonReversDN.TabIndex = 7;
            this.buttonReversDN.Text = "Развернуть ДН";
            this.buttonReversDN.UseVisualStyleBackColor = true;
            this.buttonReversDN.Click += new System.EventHandler(this.buttonReversDN_Click);
            // 
            // DataTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 389);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataTableForm";
            this.Text = "DataTableForm";
            this.Load += new System.EventHandler(this.DataTableForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonAddAmpl;
        private System.Windows.Forms.Button buttonAddPhase;
        private System.Windows.Forms.Button buttonAddCoord;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonReversDN;

    }
}