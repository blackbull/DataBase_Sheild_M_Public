namespace DB_Controls
{
    partial class ResultDNUserControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBoxMaxMin = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBoxFullMistake = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxКоординаты_фазового_центра_2 = new System.Windows.Forms.TextBox();
            this.textBoxКоординаты_фазового_центра_1 = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxНаправление_максимума_диаграммы_направленности = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxУровень_боковых_лепестков = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности = new System.Windows.Forms.TextBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.textBoxMIN = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox13);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox8);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 626);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список результатов ДН";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.textBoxMaxMin);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox9.Location = new System.Drawing.Point(0, 419);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(230, 47);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Max/Min";
            // 
            // textBoxMaxMin
            // 
            this.textBoxMaxMin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMaxMin.Location = new System.Drawing.Point(3, 21);
            this.textBoxMaxMin.Name = "textBoxMaxMin";
            this.textBoxMaxMin.ReadOnly = true;
            this.textBoxMaxMin.Size = new System.Drawing.Size(224, 23);
            this.textBoxMaxMin.TabIndex = 1;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.textBoxFullMistake);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox8.Location = new System.Drawing.Point(0, 22);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(230, 47);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Диаграмма направленности";
            // 
            // textBoxFullMistake
            // 
            this.textBoxFullMistake.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxFullMistake.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxFullMistake.Location = new System.Drawing.Point(3, 21);
            this.textBoxFullMistake.Name = "textBoxFullMistake";
            this.textBoxFullMistake.ReadOnly = true;
            this.textBoxFullMistake.Size = new System.Drawing.Size(224, 23);
            this.textBoxFullMistake.TabIndex = 1;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.tableLayoutPanel1);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox7.Location = new System.Drawing.Point(0, 538);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(230, 85);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Координаты фазового центра";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxКоординаты_фазового_центра_2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxКоординаты_фазового_центра_1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(224, 60);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // textBoxКоординаты_фазового_центра_2
            // 
            this.textBoxКоординаты_фазового_центра_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxКоординаты_фазового_центра_2.Location = new System.Drawing.Point(3, 33);
            this.textBoxКоординаты_фазового_центра_2.Name = "textBoxКоординаты_фазового_центра_2";
            this.textBoxКоординаты_фазового_центра_2.ReadOnly = true;
            this.textBoxКоординаты_фазового_центра_2.Size = new System.Drawing.Size(218, 23);
            this.textBoxКоординаты_фазового_центра_2.TabIndex = 10;
            // 
            // textBoxКоординаты_фазового_центра_1
            // 
            this.textBoxКоординаты_фазового_центра_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxКоординаты_фазового_центра_1.Location = new System.Drawing.Point(3, 3);
            this.textBoxКоординаты_фазового_центра_1.Name = "textBoxКоординаты_фазового_центра_1";
            this.textBoxКоординаты_фазового_центра_1.ReadOnly = true;
            this.textBoxКоординаты_фазового_центра_1.Size = new System.Drawing.Size(218, 23);
            this.textBoxКоординаты_фазового_центра_1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox6.Location = new System.Drawing.Point(0, 334);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(230, 82);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Смещение первого бокового лепестка относительно максимума ДН";
            // 
            // textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности
            // 
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Location = new System.Drawing.Point(3, 56);
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Name = "textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направ" +
    "ленности";
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.ReadOnly = true;
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Size = new System.Drawing.Size(224, 23);
            this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.textBoxНаправление_максимума_диаграммы_направленности);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox5.Location = new System.Drawing.Point(0, 149);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(230, 50);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Направление максимума ДН";
            // 
            // textBoxНаправление_максимума_диаграммы_направленности
            // 
            this.textBoxНаправление_максимума_диаграммы_направленности.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxНаправление_максимума_диаграммы_направленности.Location = new System.Drawing.Point(3, 24);
            this.textBoxНаправление_максимума_диаграммы_направленности.Name = "textBoxНаправление_максимума_диаграммы_направленности";
            this.textBoxНаправление_максимума_диаграммы_направленности.ReadOnly = true;
            this.textBoxНаправление_максимума_диаграммы_направленности.Size = new System.Drawing.Size(224, 23);
            this.textBoxНаправление_максимума_диаграммы_направленности.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(0, 75);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 68);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Коэффициент усиления в максимуме ДН";
            // 
            // textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности
            // 
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Location = new System.Drawing.Point(3, 42);
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Name = "textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности";
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.ReadOnly = true;
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Size = new System.Drawing.Size(224, 23);
            this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxУровень_боковых_лепестков);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 278);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 50);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Уровень боковых лепестков";
            // 
            // textBoxУровень_боковых_лепестков
            // 
            this.textBoxУровень_боковых_лепестков.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxУровень_боковых_лепестков.Location = new System.Drawing.Point(3, 24);
            this.textBoxУровень_боковых_лепестков.Name = "textBoxУровень_боковых_лепестков";
            this.textBoxУровень_боковых_лепестков.ReadOnly = true;
            this.textBoxУровень_боковых_лепестков.Size = new System.Drawing.Size(224, 23);
            this.textBoxУровень_боковых_лепестков.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxШирина_диаграммы_направленности_по_половине_мощности);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(0, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 67);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ширина ДН по половине мощности";
            // 
            // textBoxШирина_диаграммы_направленности_по_половине_мощности
            // 
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Location = new System.Drawing.Point(3, 41);
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Name = "textBoxШирина_диаграммы_направленности_по_половине_мощности";
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.ReadOnly = true;
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Size = new System.Drawing.Size(224, 23);
            this.textBoxШирина_диаграммы_направленности_по_половине_мощности.TabIndex = 1;
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.textBoxMIN);
            this.groupBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox13.Location = new System.Drawing.Point(0, 474);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(230, 61);
            this.groupBox13.TabIndex = 22;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Коэффициент усиления в минимуме ДН";
            // 
            // textBoxMIN
            // 
            this.textBoxMIN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMIN.Location = new System.Drawing.Point(3, 35);
            this.textBoxMIN.Name = "textBoxMIN";
            this.textBoxMIN.ReadOnly = true;
            this.textBoxMIN.Size = new System.Drawing.Size(224, 23);
            this.textBoxMIN.TabIndex = 1;
            // 
            // ResultDNUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(230, 511);
            this.Name = "ResultDNUserControl";
            this.Size = new System.Drawing.Size(230, 626);
            this.groupBox1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxКоординаты_фазового_центра_1;
        private System.Windows.Forms.TextBox textBoxКоординаты_фазового_центра_2;
        private System.Windows.Forms.TextBox textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности;
        private System.Windows.Forms.TextBox textBoxНаправление_максимума_диаграммы_направленности;
        private System.Windows.Forms.TextBox textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности;
        private System.Windows.Forms.TextBox textBoxУровень_боковых_лепестков;
        private System.Windows.Forms.TextBox textBoxШирина_диаграммы_направленности_по_половине_мощности;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox textBoxFullMistake;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox textBoxMaxMin;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.TextBox textBoxMIN;
    }
}
