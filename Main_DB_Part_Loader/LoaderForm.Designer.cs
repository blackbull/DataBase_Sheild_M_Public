namespace Main_DB_Part_Loader
{
    partial class LoaderForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonConnectToBD = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMeasurement = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxAnten = new System.Windows.Forms.ComboBox();
            this.buttonXML = new System.Windows.Forms.Button();
            this.buttonXMLToBD = new System.Windows.Forms.Button();
            this.buttonDelDublicates = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonTestLoad = new System.Windows.Forms.Button();
            this.buttonPreobraz = new System.Windows.Forms.Button();
            this.buttonDELNullAntens = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonConnectToBD);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxMeasurement);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxAnten);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(861, 170);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение к Базе Данных";
            // 
            // buttonConnectToBD
            // 
            this.buttonConnectToBD.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonConnectToBD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonConnectToBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonConnectToBD.Location = new System.Drawing.Point(326, 127);
            this.buttonConnectToBD.Name = "buttonConnectToBD";
            this.buttonConnectToBD.Size = new System.Drawing.Size(202, 38);
            this.buttonConnectToBD.TabIndex = 6;
            this.buttonConnectToBD.Text = "Подключиться к серверу БД";
            this.buttonConnectToBD.UseVisualStyleBackColor = false;
            this.buttonConnectToBD.Click += new System.EventHandler(this.buttonConnectToBD_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(21, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Строка инициализации БД измерений";
            // 
            // comboBoxMeasurement
            // 
            this.comboBoxMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxMeasurement.FormattingEnabled = true;
            this.comboBoxMeasurement.Location = new System.Drawing.Point(6, 97);
            this.comboBoxMeasurement.Name = "comboBoxMeasurement";
            this.comboBoxMeasurement.Size = new System.Drawing.Size(849, 24);
            this.comboBoxMeasurement.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Строка инициализации БД антенн";
            // 
            // comboBoxAnten
            // 
            this.comboBoxAnten.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnten.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxAnten.FormattingEnabled = true;
            this.comboBoxAnten.Location = new System.Drawing.Point(6, 42);
            this.comboBoxAnten.Name = "comboBoxAnten";
            this.comboBoxAnten.Size = new System.Drawing.Size(849, 24);
            this.comboBoxAnten.TabIndex = 1;
            // 
            // buttonXML
            // 
            this.buttonXML.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonXML.Location = new System.Drawing.Point(7, 200);
            this.buttonXML.Name = "buttonXML";
            this.buttonXML.Size = new System.Drawing.Size(126, 62);
            this.buttonXML.TabIndex = 3;
            this.buttonXML.Text = "Загрузить измерения из XML файлов";
            this.buttonXML.UseVisualStyleBackColor = false;
            this.buttonXML.Click += new System.EventHandler(this.buttonXML_Click);
            // 
            // buttonXMLToBD
            // 
            this.buttonXMLToBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonXMLToBD.Location = new System.Drawing.Point(154, 16);
            this.buttonXMLToBD.Name = "buttonXMLToBD";
            this.buttonXMLToBD.Size = new System.Drawing.Size(136, 62);
            this.buttonXMLToBD.TabIndex = 4;
            this.buttonXMLToBD.Text = "Перенос измерений между БД";
            this.buttonXMLToBD.UseVisualStyleBackColor = true;
            this.buttonXMLToBD.Click += new System.EventHandler(this.buttonXMLToBD_Click);
            // 
            // buttonDelDublicates
            // 
            this.buttonDelDublicates.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDelDublicates.Location = new System.Drawing.Point(10, 16);
            this.buttonDelDublicates.Name = "buttonDelDublicates";
            this.buttonDelDublicates.Size = new System.Drawing.Size(136, 62);
            this.buttonDelDublicates.TabIndex = 5;
            this.buttonDelDublicates.Text = "Удалить дубликаты антенн из БД";
            this.buttonDelDublicates.UseVisualStyleBackColor = true;
            this.buttonDelDublicates.Click += new System.EventHandler(this.buttonDelDublicates_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonTestLoad);
            this.groupBox2.Controls.Add(this.buttonPreobraz);
            this.groupBox2.Controls.Add(this.buttonDELNullAntens);
            this.groupBox2.Controls.Add(this.buttonDelDublicates);
            this.groupBox2.Controls.Add(this.buttonXMLToBD);
            this.groupBox2.Location = new System.Drawing.Point(139, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(729, 84);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сервисные функции";
            // 
            // buttonTestLoad
            // 
            this.buttonTestLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTestLoad.Location = new System.Drawing.Point(586, 16);
            this.buttonTestLoad.Name = "buttonTestLoad";
            this.buttonTestLoad.Size = new System.Drawing.Size(136, 62);
            this.buttonTestLoad.TabIndex = 8;
            this.buttonTestLoad.Text = "Тест загрузки всех измерений";
            this.buttonTestLoad.UseVisualStyleBackColor = true;
            this.buttonTestLoad.Click += new System.EventHandler(this.buttonTestLoad_Click);
            // 
            // buttonPreobraz
            // 
            this.buttonPreobraz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPreobraz.Location = new System.Drawing.Point(442, 16);
            this.buttonPreobraz.Name = "buttonPreobraz";
            this.buttonPreobraz.Size = new System.Drawing.Size(136, 62);
            this.buttonPreobraz.TabIndex = 7;
            this.buttonPreobraz.Text = "Преобразование измерений в новый формат";
            this.buttonPreobraz.UseVisualStyleBackColor = true;
            this.buttonPreobraz.Click += new System.EventHandler(this.buttonPreobraz_Click);
            // 
            // buttonDELNullAntens
            // 
            this.buttonDELNullAntens.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDELNullAntens.Location = new System.Drawing.Point(298, 16);
            this.buttonDELNullAntens.Name = "buttonDELNullAntens";
            this.buttonDELNullAntens.Size = new System.Drawing.Size(136, 62);
            this.buttonDELNullAntens.TabIndex = 6;
            this.buttonDELNullAntens.Text = "Удалить пустые антенны из БД";
            this.buttonDELNullAntens.UseVisualStyleBackColor = true;
            this.buttonDELNullAntens.Click += new System.EventHandler(this.buttonDELNullAntens_Click);
            // 
            // LoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 281);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonXML);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoaderForm";
            this.Text = "Загрузчик БД";
            this.Load += new System.EventHandler(this.LoaderForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxAnten;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMeasurement;
        private System.Windows.Forms.Button buttonConnectToBD;
        private System.Windows.Forms.Button buttonXML;
        private System.Windows.Forms.Button buttonXMLToBD;
        private System.Windows.Forms.Button buttonDelDublicates;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonDELNullAntens;
        private System.Windows.Forms.Button buttonPreobraz;
        private System.Windows.Forms.Button buttonTestLoad;
    }
}