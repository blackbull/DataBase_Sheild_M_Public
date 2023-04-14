using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResultTypesClassLibrary;
using ResultOptionsClassLibrary;

namespace AncillaryDBForms
{
    public partial class PreviewUnionDNForm : Form
    {
        public PreviewUnionDNForm(ResultTypeClassUnion UnionDN,ISaver_ToDataBase DBSaver)
        {
            InitializeComponent();

            Union = UnionDN;
            SaveDBForm.Saver = DBSaver;

            #region устанавливаем параметры из первого исходного результата

            if(Union.InitialResults.Count>0)
            {
                IResultType_MAIN res=Union.InitialResults[0];

                SaveDBForm.Antenn = res.Antenn;
                SaveDBForm.Zond = res.Zond;

                SaveDBForm.IsHideFrequency = res.SelectedPolarization.SelectedFrequency.IsHideFrequency;
                SaveDBForm.Frequency = res.SelectedPolarization.SelectedFrequency.Frequency;
            }

            #endregion
        }

        ResultTypeClassUnion Union = null;
        SaveToDataBaseForm SaveDBForm = new SaveToDataBaseForm();
        bool ShowOptionsOnes = false;

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            CompareGraphForm form = new CompareGraphForm("Результат рассчёта ДН", false,this.SaveDBForm.Saver,false);
            form.AddToWath(Union);

            foreach (ResultType_MAINClass res in Union.InitialResults)
            {
                form.AddToWath(res);
            }

            form.ShowDialog(this);
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            if (CheckOptions())
            {
                ReportForm.ProtocolForm report = new ReportForm.ProtocolForm(Union);
                report.ShowDialog();
            }
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            this.ShowOptionsForm();
        }

        private bool ShowOptionsForm()
        {
            bool ret = false;

            if (this.SaveDBForm.ShowDialog(this) == DialogResult.OK)
            {
                ShowOptionsOnes = true;

                if (SaveDBForm.Antenn == null)
                {
                    this.Union.Antenn = new AntennOptionsClass();
                }
                else
                {
                    this.Union.Antenn = SaveDBForm.Antenn;
                }
                if (SaveDBForm.Zond == null)
                {
                    this.Union.Zond = new AntennOptionsClass();
                }
                else
                {
                    this.Union.Zond = SaveDBForm.Zond;
                }

                this.Union.SelectedPolarization.SelectedFrequency.IsHideFrequency = SaveDBForm.IsHideFrequency;
                this.Union.SelectedPolarization.SelectedFrequency.Frequency = SaveDBForm.Frequency;

                ret = true;
            }

            return ret;
        }

        private void buttonSaveDB_Click(object sender, EventArgs e)
        {
            if (CheckOptions())
            {
                buttonSaveDB.Enabled = false;
                buttonOptions.Enabled = false;

                if (SaveDBForm.Saver.SaveMainTable(Union) > 0)
                {
                    MessageBox.Show(string.Format("Успешно сохранено под именем \n{0}", Union), "Сохранение в БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка сохранения", "Сохранение в БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CheckOptions()
        {
            bool Create = false;


            if (SaveDBForm.Antenn == null || SaveDBForm.Zond == null || !ShowOptionsOnes)
            {
                if (this.ShowOptionsForm())
                {
                    if (SaveDBForm.Antenn == null || SaveDBForm.Zond == null)
                    {
                        if (MessageBox.Show("Введены не все опции рассчёта. \n Всё равно создать отчёт?", "Создание отчёта", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Create = true;
                        }
                    }
                    else
                    {
                        Create = true;
                    }
                }
            }
            else
            {
                Create = true;
            }

            return Create;
        }
    }
}
