namespace DataBase_Sheild_M
{
    partial class SaverResultForm
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
            this.buttonTXT = new System.Windows.Forms.Button();
            this.buttonXML = new System.Windows.Forms.Button();
            this.buttonXLSX = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTXT
            // 
            this.buttonTXT.Location = new System.Drawing.Point(55, 37);
            this.buttonTXT.Name = "buttonTXT";
            this.buttonTXT.Size = new System.Drawing.Size(96, 30);
            this.buttonTXT.TabIndex = 1;
            this.buttonTXT.Text = "Экспорт в TXT";
            this.buttonTXT.UseVisualStyleBackColor = true;
            this.buttonTXT.Click += new System.EventHandler(this.buttonTXT_Click);
            // 
            // buttonXML
            // 
            this.buttonXML.Location = new System.Drawing.Point(55, 82);
            this.buttonXML.Name = "buttonXML";
            this.buttonXML.Size = new System.Drawing.Size(96, 30);
            this.buttonXML.TabIndex = 2;
            this.buttonXML.Text = "Экспорт в XML";
            this.buttonXML.UseVisualStyleBackColor = true;
            this.buttonXML.Click += new System.EventHandler(this.buttonXML_Click);
            // 
            // buttonXLSX
            // 
            this.buttonXLSX.Location = new System.Drawing.Point(55, 128);
            this.buttonXLSX.Name = "buttonXLSX";
            this.buttonXLSX.Size = new System.Drawing.Size(96, 30);
            this.buttonXLSX.TabIndex = 3;
            this.buttonXLSX.Text = "Экспорт в XLSX";
            this.buttonXLSX.UseVisualStyleBackColor = true;
            this.buttonXLSX.Click += new System.EventHandler(this.buttonXLSX_Click);
            // 
            // SaverResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 184);
            this.Controls.Add(this.buttonXLSX);
            this.Controls.Add(this.buttonXML);
            this.Controls.Add(this.buttonTXT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SaverResultForm";
            this.Text = "TestSaverForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTXT;
        private System.Windows.Forms.Button buttonXML;
        private System.Windows.Forms.Button buttonXLSX;
    }
}