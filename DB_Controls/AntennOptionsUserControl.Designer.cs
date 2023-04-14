using ResultOptionsClassLibrary;
namespace DB_Controls
{
    partial class AntennOptionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDescriptions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxZavNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxUsingAsZond = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comboBoxName
            // 
            this.comboBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxName.FormattingEnabled = true;
            this.comboBoxName.Location = new System.Drawing.Point(0, 18);
            this.comboBoxName.Name = "comboBoxName";
            this.comboBoxName.Size = new System.Drawing.Size(193, 23);
            this.comboBoxName.TabIndex = 12;
            this.comboBoxName.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBoxName.TextUpdate += new System.EventHandler(this.comboBoxName_TextUpdate);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Описание";
            // 
            // textBoxDescriptions
            // 
            this.textBoxDescriptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescriptions.Location = new System.Drawing.Point(0, 135);
            this.textBoxDescriptions.Multiline = true;
            this.textBoxDescriptions.Name = "textBoxDescriptions";
            this.textBoxDescriptions.Size = new System.Drawing.Size(193, 65);
            this.textBoxDescriptions.TabIndex = 4;
            this.textBoxDescriptions.TextChanged += new System.EventHandler(this.textBoxOpisanie_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Заводской номер";
            // 
            // textBoxZavNumber
            // 
            this.textBoxZavNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxZavNumber.Location = new System.Drawing.Point(0, 63);
            this.textBoxZavNumber.Name = "textBoxZavNumber";
            this.textBoxZavNumber.Size = new System.Drawing.Size(193, 21);
            this.textBoxZavNumber.TabIndex = 2;
            this.textBoxZavNumber.TextChanged += new System.EventHandler(this.textBoxZavNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Наименование";
            // 
            // checkBoxUsingAsZond
            // 
            this.checkBoxUsingAsZond.AutoSize = true;
            this.checkBoxUsingAsZond.Location = new System.Drawing.Point(7, 93);
            this.checkBoxUsingAsZond.Name = "checkBoxUsingAsZond";
            this.checkBoxUsingAsZond.Size = new System.Drawing.Size(146, 19);
            this.checkBoxUsingAsZond.TabIndex = 13;
            this.checkBoxUsingAsZond.Text = "Используется как ТА";
            this.checkBoxUsingAsZond.UseVisualStyleBackColor = true;
            this.checkBoxUsingAsZond.Click += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // AntennOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxUsingAsZond);
            this.Controls.Add(this.comboBoxName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDescriptions);
            this.Controls.Add(this.textBoxZavNumber);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MinimumSize = new System.Drawing.Size(194, 201);
            this.Name = "AntennOptionsUserControl";
            this.Size = new System.Drawing.Size(194, 201);
            this.Leave += new System.EventHandler(this.AntennOptionsUserControl_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDescriptions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxZavNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxName;
        private System.Windows.Forms.CheckBox checkBoxUsingAsZond;
    }
}
