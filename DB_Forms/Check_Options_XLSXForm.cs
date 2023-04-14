using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DB_Forms
{
    public partial class Check_Options_XLSXForm : Form
    {
        public Check_Options_XLSXForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool NeedMain
        {
            get
            {
                return this.checkedListBox1.GetItemChecked(0);
            }
        }
        public bool NeedCross
        {
            get
            {
                return this.checkedListBox1.GetItemChecked(1);
            }
        }

        public bool NeedSum
        {
            get
            {
                return this.checkedListBox1.GetItemChecked(2);
            }
        }

        public bool NeedMaxДН
        {
            get
            {
                return this.checkedListBox1.GetItemChecked(3);
            }
        }

        public bool NeedШирина_ДН
        {
            get
            {
                return this.checkedListBox1.GetItemChecked(4);
            }
        }



        private void Check_Options_XLSXForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }

        }
    }
}
