using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultOptionsClassLibrary;

namespace DB_Forms
{
    public partial class ResultDescriptionForm : Form
    {
        public ResultDescriptionForm()
        {
            InitializeComponent();
        }


        public List<IResultType_MAIN> resuslts = null;
               

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> dis = new List<string>(richTextBox1.Text.Split("#\n".ToCharArray()));

            string check = "";

            try
            {
                for (int i = 0; i < resuslts.Count; i++)
                {
                    string tempstr1 = dis[i * 2 + 1];
                   tempstr1= tempstr1.Trim(' ');

                   List<string> dis2 = new List<string>(tempstr1.Split('%'));

                   dis2[0] = dis2[0].Trim(' ');
                   dis2[1] = dis2[1].Trim(' ');

                    List<string> port_TB = new List<string>(dis2[0].Split('t'));

                    string port = port_TB[0];
                   string angle = port_TB[1];

                    List<string> disF = new List<string>(dis2[1].Split('f'));

                   string start = disF[0];
                   string stop = disF[1];

                   check += string.Format("{0} | {1} № {2} - port {3} tb {4} {5} - {6}\n", this.resuslts[i].MainOptions.Date.ToShortDateString(), this.resuslts[i].MainOptions.Name, this.resuslts[i].id, port, angle, start, stop);
                }

                if (MessageBox.Show(this, check, "Верно?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < resuslts.Count; i++)
                    {
                        string tempstr = dis[i * 2 + 1];
                        tempstr = tempstr.Trim(' ');
                        /*
                        string port = tempstr.Substring(0, 1);
                        string angle = tempstr.Substring(1);
                        */
                        this.resuslts[i].MainOptions.Descriptions = tempstr;
                    }


                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch 
            {
               MessageBox.Show(this, "Проверьте рзмерность обоих колонок, они должны быть одинаковые", "косяк", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ResultDescriptionForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < resuslts.Count; i++)
            {
                string addtext = string.Format("{0} | {1} № {2} # {3}\n", this.resuslts[i].MainOptions.Date.ToShortDateString(), this.resuslts[i].MainOptions.Name, this.resuslts[i].id, this.resuslts[i].MainOptions.Descriptions);
                this.richTextBox1.AppendText(addtext);

                //this.resuslts[i].MainOptions.Descriptions;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
