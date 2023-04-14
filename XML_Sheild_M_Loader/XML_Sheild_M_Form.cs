using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultTypesClassLibrary;
using DataBase_Sheild_M;

namespace XML_Sheild_M_Loader
{
    public partial class XML_Sheild_M_Form : Form
    {
        public XML_Sheild_M_Form()
            : this(false) { }

        public XML_Sheild_M_Form(bool OnlyLoadXML)
        {
            InitializeComponent();

            this.buttonLoadXMLAndExit.Visible = OnlyLoadXML;
        }

        private void buttonSelectALL_Click(object sender, EventArgs e)
        {
            foreach (Control contr in flowLayoutPanel1.Controls)
            {
                if (contr is CheckBox)
                {
                    CheckBox CB = contr as CheckBox;
                    CB.Checked = true;
                }
            }
        }

        private void buttonDeselectALL_Click(object sender, EventArgs e)
        {
            foreach (Control contr in flowLayoutPanel1.Controls)
            {
                if (contr is CheckBox)
                {
                    CheckBox CB = contr as CheckBox;
                    CB.Checked = false;
                }
            }
        }

        private void buttonDEL_Click(object sender, EventArgs e)
        {
            for (int i = flowLayoutPanel1.Controls.Count-1; i >= 0; i--)
            {
                if (flowLayoutPanel1.Controls[i] is CheckBox)
                {
                    CheckBox CB = flowLayoutPanel1.Controls[i] as CheckBox;

                    if (CB.Checked)
                    {
                        flowLayoutPanel1.Controls.Remove(flowLayoutPanel1.Controls[i]);
                    }
                }
            }
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
             OpenFileDialog fd = new OpenFileDialog();

           fd.Title = "Выберите файлЫ для загрузки";
           fd.Filter = "XML файлы|*.xml";
           fd.Multiselect = true;

           if (fd.ShowDialog() == DialogResult.OK)
           {
               foreach (string st in fd.FileNames)
               {
                   this.AddTOPanel(st, st);
               }
           }
        }

        private void AddTOPanel(string newName, object Tag)
        {
            CheckBox CB = new CheckBox();
            CB.Tag = Tag;
            CB.Height = 20;
            CB.Margin = new System.Windows.Forms.Padding(0);
            CB.Text = newName;
            CB.Width = flowLayoutPanel1.Width - 28;
            CB.Checked = true;
            flowLayoutPanel1.Controls.Add(CB);
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            DataBaseForm DBF = new DataBaseForm(this.LoadXMLFiles());
            DBF.ShowDialog();
        }

        protected XML_ResultLoaderClass _XMLLoader;
        public XML_ResultLoaderClass XMLLoader
        {
            get { return _XMLLoader; }
        }

        protected XML_ResultLoaderClass LoadXMLFiles()
        {
            List<ResultType_MAINClass> ret = new List<ResultType_MAINClass>();

            string ErrorText = "";

            foreach (Control contr in flowLayoutPanel1.Controls)
            {
                if (contr is CheckBox)
                {
                    CheckBox CB = contr as CheckBox;
                    if (CB.Checked && CB.Tag is string)
                    {
                        string temp = CB.Tag as string;

                        ResultType_MAINClass tempRES = SaverResultForm.LoadResultXMLFile(temp);

                        if (tempRES != null)
                        {
                            ret.Add(tempRES);
                        }
                        else
                        {
                            ErrorText += string.Format("{0} \n", temp);
                        }
                    }
                }
            }

            if (ErrorText != "")
            {
                MessageBox.Show(this, string.Format("Не удалось загрузить следующие файлы:\n{0}", ErrorText), "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _XMLLoader = new XML_ResultLoaderClass(ret);

            return _XMLLoader;
        }

        private void buttonLoadXMLAndExit_Click(object sender, EventArgs e)
        {
            this.LoadXMLFiles();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
