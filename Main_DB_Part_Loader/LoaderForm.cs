using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase_Sheild_M;
using DB_Loader;
using XML_Sheild_M_Loader;
using ResultOptionsClassLibrary;
using ResultTypesClassLibrary;
using ComboBox_LoaderOptions;

namespace Main_DB_Part_Loader
{
    public partial class LoaderForm : Form
    {
        public LoaderForm()
        {
            InitializeComponent();
        }

        #region Обработчики загрузки БД

        protected void SetOptions(DB_LoaderOptions opt)
        {
            LoaderCBOptionsClass.SetComboBoxStrings(ref comboBoxAnten, opt.BD_Anten, opt.SelectedBD_Anten);
            LoaderCBOptionsClass.SetComboBoxStrings(ref comboBoxMeasurement, opt.BD_Measurement, opt.SelectedBD_Measurement);
        }

        protected DB_LoaderOptions GetOptions()
        {
            DB_LoaderOptions ret = new DB_LoaderOptions();

            ret.SelectedBD_Anten = LoaderCBOptionsClass.GetComboBoxStrings(ref comboBoxAnten, out ret.BD_Anten);
            ret.SelectedBD_Measurement = LoaderCBOptionsClass.GetComboBoxStrings(ref comboBoxMeasurement, out ret.BD_Measurement);

            return ret;
        }

        #region функции загрузки и сохранения опций в XML

        /// <summary>
        /// Сохранить настройки в файл
        /// </summary>
        /// <param name="SaveName">Имя для сохранения опции ("***_Options.xml")</param>
        public static void SaveOptions(DB_LoaderOptions SaveObject, string SaveName)
        {
            UniversalXmlSerializer.UnivrsalXmlSerializerClass.SaveOptions<DB_LoaderOptions>(SaveObject, SaveName);
        }

        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        /// <param name="FileName">имя и путь к файлу ("***_Options.xml")</param>
        public static DB_LoaderOptions LoadOptions(string FileName)
        {
            return UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadOptions<DB_LoaderOptions>(FileName) as DB_LoaderOptions;
        }
        #endregion

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            DB_LoaderOptions opt = LoadOptions("DB_Options.xml");

            SetOptions(opt);
        }

        private void buttonConnectToBD_Click(object sender, EventArgs e)
        {
            DB_LoaderOptions opt = GetOptions();
            DB_LoaderClass DB_Load = GetLoader(opt);


            try
            {
                DataBaseForm databaseform = new DataBaseForm(DB_Load);
                databaseform.ShowDialog(this);

                SaveOptions(opt, "DB_Options.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка подключения к БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DB_Load.Dispose();
        }

        protected DB_LoaderClass GetLoader(DB_LoaderOptions opt)
        {
            DB_LoaderClass DBLoader = new DB_LoaderClass(opt.SelectedBD_Anten, opt.SelectedBD_Measurement);
            DBLoader.InitializeDB();
            return DBLoader;
        }

        #endregion

        #region Функции работы с XML

        private void buttonXML_Click(object sender, EventArgs e)
        {
            new XML_Sheild_M_Form().ShowDialog(this);
        }

        private void buttonXMLToBD_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Перед переносом проверьте соединение с БД и загрузку результатов из БД.\n В следующем окне выберите XML файлы с измерениями (их тоже лучше проверить на корректность)"), "Перенос измерений", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                XML_Sheild_M_Form XMLForm = new XML_Sheild_M_Form(true);
                if (XMLForm.ShowDialog() == DialogResult.OK)
                {
                    string ErrorText = "";
                    string AddingText = "";
                    HendBandFormApplication.HeadBandForm HeandBand = new HendBandFormApplication.HeadBandForm();


                    try
                    {
                        ErrorText = "Ошибка загрузки XML результата";
                        XML_ResultLoaderClass xmls = XMLForm.XMLLoader;

                        ErrorText = "Ошибка подключения к БД";
                        DB_LoaderClass DBLoader = this.GetLoader(this.GetOptions());

                        ErrorText = "Ошибка проверки результатов";
                        List<AntennOptionsClass> AddAntensToDB;
                        List<ChangeAntenID> ChangeIDList;

                        this.CheckAntens(xmls.AllAntenn, DBLoader.LoadAllAntenn(), out AddAntensToDB, out ChangeIDList);

                        this.UpdateIdAntensInMeasurement(ChangeIDList, xmls.AllResults);

                        #region сообщение о добавлении результатов

                        string AddingMesage = "";

                        if (AddAntensToDB.Count != 0)
                        {
                            AddingMesage += string.Format("Будут добавлены следующие антенны в БД:\n");

                            foreach (AntennOptionsClass ant in AddAntensToDB)
                            {
                                AddingMesage += string.Format("{0}\n", ant.ToString());
                            }
                        }

                        if (AddAntensToDB.Count != 0)
                        {
                            AddingMesage += string.Format("\nБудут добавлены следующие измерения в БД:\n");

                            foreach (ResultType_MAINClass res in xmls.AllResults)
                            {
                                AddingMesage += string.Format("{0}\n", res.ToString());
                            }
                        }

                        MessageBox.Show(this, string.Format("Процес сохранения результата занимает длятельное время, дождитесь сообщение о завершении переноса.\n\n{0}", AddingMesage), "Перенос измерений", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        #endregion

                        HeandBand.TextBig = "Перенос измерний\nЭкран-М";
                        HeandBand.BackColor = Color.AliceBlue;

                        AddOwnedForm(HeandBand);
                        HeandBand.Show();
                        Application.DoEvents();

                        //клонируем массив антенн
                        ChangeIDList.Clear();
                        for (int i = 0; i < AddAntensToDB.Count; i++)
                        {
                            ChangeIDList.Add(new ChangeAntenID(AddAntensToDB[i].id, -1));
                        }

                        ErrorText = "Ошибка добавления антен в БД";
                        foreach (AntennOptionsClass ant in AddAntensToDB)
                        {
                            HeandBand.TextSmall = "Добавление " + ant.ToString();
                            Application.DoEvents();

                            ant.id = -1;
                            DBLoader.Save_Antenn(this, ant);
                            AddingText += string.Format("Добавлена антенна {0} \n", ant.ToString());
                        }

                        //обновляем id добавленных антенн
                        for (int i = 0; i < AddAntensToDB.Count; i++)
                        {
                            ChangeIDList[i].NewID = AddAntensToDB[i].id;
                        }

                        this.UpdateIdAntensInMeasurement(ChangeIDList, xmls.AllResults);

                        ErrorText = "Ошибка добавления результатов в БД";
                        foreach (ResultType_MAINClass res in xmls.AllResults)
                        {
                            HeandBand.TextSmall = "Добавление " + res.ToString();
                            Application.DoEvents();
                                                       

                            //ErrorText = "Чистка суммарных результатов";
                            //for(int i=0;i<res.PolarizationElements.Count;i++)
                            //{
                            //    if (res.PolarizationElements[i].Polarization == SelectedPolarizationEnum.Sum)
                            //    {
                            //        res.PolarizationElements.Remove(res.PolarizationElements[i]);
                            //    }
                            //}

                            int oldID = res.id;

                            try
                            {
                                if (!DBLoader.FindInResultByID(res.id))
                                {
                                    DBLoader.SaveMainTable(res);

                                    AddingText += string.Format("№{0} -> №{1} \t", oldID, res.id);
                                }
                                else
                                {
                                    AddingText += string.Format("№{0} - есть дубликат в базе \t", oldID);
                                }
                            }
                            catch (Exception ex)
                            {
                                AddingText += string.Format("Ошибка добавления Измерения №{0} \n \"{1}\"", oldID, ex.Message);
                            }
                        }

                        DBLoader.Dispose();

                        HeandBand.Close();

                        MessageBox.Show(this, AddingText, "Измерения перенесены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        HeandBand.Close();
                        MessageBox.Show(this, ex.Message, ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        protected void CheckAntens(List<AntennOptionsClass> InXML, List<AntennOptionsClass> InDB, out List<AntennOptionsClass> AddAntensToDB, out List<ChangeAntenID> ChangeIDList)
        {
            AddAntensToDB = new List<AntennOptionsClass>();
            ChangeIDList = new List<ChangeAntenID>();

            foreach (AntennOptionsClass antXML in InXML)
            {
                string ant_XML_KEYName = DB_LoaderClass.GetSplitKey(antXML.Name);
                string ant_XML_KEYZAV = DB_LoaderClass.GetSplitKey(antXML.ZAVNumber);

                bool FindAntenInDB = false;

                foreach (AntennOptionsClass antDB in InDB)
                {
                    string ant_DB_KEYName = DB_LoaderClass.GetSplitKey(antDB.Name);
                    string ant_DB_KEYZAV = DB_LoaderClass.GetSplitKey(antDB.ZAVNumber);

                    if (ant_XML_KEYName == ant_DB_KEYName)
                    {
                        if (ant_XML_KEYZAV == ant_DB_KEYZAV)
                        {
                            FindAntenInDB = true;

                            if (antXML.id != antDB.id)
                            {
                                ChangeIDList.Add(new ChangeAntenID(antXML.id, antDB.id));
                            }

                            break;
                        }
                    }
                }

                if (!FindAntenInDB)
                {
                    AddAntensToDB.Add(antXML);
                }
            }
        }

        protected class ChangeAntenID
        {
            public ChangeAntenID(int OLD, int NEW)
            {
                OldID = OLD;
                NewID = NEW;
            }

            public int OldID = -1;
            public int NewID = -1;
        }

        protected void UpdateIdAntensInMeasurement(List<ChangeAntenID> ChangeIDList, List<ResultType_MAINClass> results)
        {
            foreach (ResultType_MAINClass res in results)
            {
                AntennOptionsClass ant = res.Antenn;
                AntennOptionsClass zond = res.Zond;

                bool antCh = false;
                bool zondCh = false;

                foreach (ChangeAntenID CNid in ChangeIDList)
                {
                    if (ant.id == CNid.OldID && !antCh)
                    {
                        ant.id = CNid.NewID;
                        antCh = true;
                    }

                    if (zond.id == CNid.OldID && !zondCh)
                    {
                        zond.id = CNid.NewID;
                        zondCh = true;
                    }

                    if (antCh && zondCh)
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        private void buttonDelDublicates_Click(object sender, EventArgs e)
        {
            DB_LoaderClass DBLoader = this.GetLoader(this.GetOptions());
            List<AntennOptionsClass> ant = DBLoader.LoadAllAntenn();

            List<DB_Loader.DB_LoaderClass.DublicateAntennasClass> dubl = DBLoader.CheckAntens(ant);

            if (dubl.Count != 0)
            {
                string RepText = "";

                foreach (DB_Loader.DB_LoaderClass.DublicateAntennasClass d in dubl)
                {
                    string DublID = "";

                    foreach (AntennOptionsClass antd in d.Dublicates)
                    {
                        DublID += antd.id.ToString() + "; ";
                    }

                    RepText += string.Format("{0}\tid={1};\t дубликаты: {2}\n", d.MainAnten.ToString(), d.MainAnten.id.ToString(), DublID);
                }

                RepText += string.Format("\nВыподнить очистку?");
                if (MessageBox.Show(this, RepText, "Дубликаты антенн", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    DBLoader.DeleteAntennDuplicates(dubl);
                }
            }
            else
            {
                MessageBox.Show(this, "Дубликатов не найдено", "Дубликаты антенн", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DBLoader.Dispose();
        }

        private void buttonDELNullAntens_Click(object sender, EventArgs e)
        {
            DB_LoaderClass DBLoader = this.GetLoader(this.GetOptions());
            DBLoader.DeleteNullAntenns();
        }

        private void buttonPreobraz_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Перед изменением формата проверьте соединение с БД и загрузку результатов из БД.\n\nПроцес форматировния результатов занимает длятельное время, дождитесь сообщение о завершении."), "Изменением формата измерений", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DB_LoaderClass DBLoader = this.GetLoader(this.GetOptions());

                DBLoader.ReformatMeasurements();

                MessageBox.Show(this, "Формат изменён", "Выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonTestLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Перед тестом проверьте соединение с БД и загрузку результатов из БД."), "Тест измерений", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DB_LoaderClass DBLoader = this.GetLoader(this.GetOptions());

                List<ResultType_MAINClass> temp;

                string ret = DBLoader.TestLoadAllResult(out temp);

                MessageBox.Show(this, ret, "Выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }



    public class DB_LoaderOptions
    {
        public List<string> BD_Anten = new List<string>();
        public string SelectedBD_Anten = " Data Source=CA;Initial Catalog=MAIN_Sheild_M;Persist Security Info=True;User ID=sa;Password=sa";

        public List<string> BD_Measurement = new List<string>();
        public string SelectedBD_Measurement = "Data Source=CA;Initial Catalog=Sheild_M_measurement;Persist Security Info=True;User ID=sa;Password=sa";
    }
}
