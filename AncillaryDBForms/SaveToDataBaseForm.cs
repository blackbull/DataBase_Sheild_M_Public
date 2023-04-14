using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResultOptionsClassLibrary;

namespace AncillaryDBForms
{
    public partial class SaveToDataBaseForm : Form
    {
        public SaveToDataBaseForm()
        {
            InitializeComponent();

            this.antennIA.LockUnlockAll(true);
            this.antennTA.LockUnlockAll(true);
        }
               
        private ISaver_ToDataBase _saver;

        public ISaver_ToDataBase Saver
        {
            get { return _saver; }
            set
            {
                _saver = value;

                this.antennIA.Saver_ToDataBase = _saver;
                this.antennTA.Saver_ToDataBase = _saver;
            }
        }

        public AntennOptionsClass Antenn
        {
            get
            {
                return this.antennIA.SelectedAntenn;
            }
             set
            {
                this.antennIA.SelectedAntenn = value;
            }
        }
        public AntennOptionsClass Zond
        {
            get
            {
                return this.antennTA.SelectedAntenn;
            }
            set
            {
                this.antennTA.SelectedAntenn = value;
            }
        }
          
        public double Frequency
        {
            get
            {
                return Convert.ToDouble(this.numericUpDown1.Value);
            }
            set
            {
                this.numericUpDown1.Value = Convert.ToDecimal(value);
            }
        }

        public bool IsHideFrequency
        {
            get
            {
                return this.checkBoxHide.Checked;
            }
            set
            {
                this.checkBoxHide.Checked = value;
            }
        }
          
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxHide_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.Enabled = !checkBoxHide.Checked;
        }
      
    }
}
