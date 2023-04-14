namespace XML_Sheild_M_Loader
{
    partial class XML_Sheild_M_Form
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonADD = new System.Windows.Forms.Button();
            this.buttonShow = new System.Windows.Forms.Button();
            this.buttonDEL = new System.Windows.Forms.Button();
            this.buttonDeselectALL = new System.Windows.Forms.Button();
            this.buttonSelectALL = new System.Windows.Forms.Button();
            this.buttonLoadXMLAndExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonLoadXMLAndExit);
            this.splitContainer1.Panel2.Controls.Add(this.buttonADD);
            this.splitContainer1.Panel2.Controls.Add(this.buttonShow);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDEL);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDeselectALL);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSelectALL);
            this.splitContainer1.Size = new System.Drawing.Size(606, 356);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(606, 270);
            this.flowLayoutPanel1.TabIndex = 18;
            // 
            // buttonADD
            // 
            this.buttonADD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonADD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonADD.Location = new System.Drawing.Point(262, 8);
            this.buttonADD.Name = "buttonADD";
            this.buttonADD.Size = new System.Drawing.Size(109, 47);
            this.buttonADD.TabIndex = 24;
            this.buttonADD.Text = "Добавить XML файлЫ";
            this.buttonADD.UseVisualStyleBackColor = false;
            this.buttonADD.Click += new System.EventHandler(this.buttonADD_Click);
            // 
            // buttonShow
            // 
            this.buttonShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShow.Location = new System.Drawing.Point(377, 8);
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(109, 47);
            this.buttonShow.TabIndex = 23;
            this.buttonShow.Text = "Просмотр";
            this.buttonShow.UseVisualStyleBackColor = false;
            this.buttonShow.Click += new System.EventHandler(this.buttonShow_Click);
            // 
            // buttonDEL
            // 
            this.buttonDEL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonDEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDEL.Location = new System.Drawing.Point(147, 8);
            this.buttonDEL.Name = "buttonDEL";
            this.buttonDEL.Size = new System.Drawing.Size(109, 47);
            this.buttonDEL.TabIndex = 22;
            this.buttonDEL.Text = "Удалить выбранное";
            this.buttonDEL.UseVisualStyleBackColor = false;
            this.buttonDEL.Click += new System.EventHandler(this.buttonDEL_Click);
            // 
            // buttonDeselectALL
            // 
            this.buttonDeselectALL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDeselectALL.Location = new System.Drawing.Point(12, 39);
            this.buttonDeselectALL.Name = "buttonDeselectALL";
            this.buttonDeselectALL.Size = new System.Drawing.Size(104, 31);
            this.buttonDeselectALL.TabIndex = 21;
            this.buttonDeselectALL.Text = "Снять выбор";
            this.buttonDeselectALL.UseVisualStyleBackColor = true;
            this.buttonDeselectALL.Click += new System.EventHandler(this.buttonDeselectALL_Click);
            // 
            // buttonSelectALL
            // 
            this.buttonSelectALL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectALL.Location = new System.Drawing.Point(12, 8);
            this.buttonSelectALL.Name = "buttonSelectALL";
            this.buttonSelectALL.Size = new System.Drawing.Size(104, 31);
            this.buttonSelectALL.TabIndex = 20;
            this.buttonSelectALL.Text = "Выбрать все";
            this.buttonSelectALL.UseVisualStyleBackColor = true;
            this.buttonSelectALL.Click += new System.EventHandler(this.buttonSelectALL_Click);
            // 
            // buttonLoadXMLAndExit
            // 
            this.buttonLoadXMLAndExit.Location = new System.Drawing.Point(492, 8);
            this.buttonLoadXMLAndExit.Name = "buttonLoadXMLAndExit";
            this.buttonLoadXMLAndExit.Size = new System.Drawing.Size(109, 47);
            this.buttonLoadXMLAndExit.TabIndex = 25;
            this.buttonLoadXMLAndExit.Text = "Загрузить эти XML и выйти";
            this.buttonLoadXMLAndExit.UseVisualStyleBackColor = true;
            this.buttonLoadXMLAndExit.Visible = false;
            this.buttonLoadXMLAndExit.Click += new System.EventHandler(this.buttonLoadXMLAndExit_Click);
            // 
            // XML_Sheild_M_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 356);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "XML_Sheild_M_Form";
            this.Text = "XML резульататы Экран-М";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonADD;
        private System.Windows.Forms.Button buttonShow;
        private System.Windows.Forms.Button buttonDEL;
        private System.Windows.Forms.Button buttonDeselectALL;
        private System.Windows.Forms.Button buttonSelectALL;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonLoadXMLAndExit;
    }
}

