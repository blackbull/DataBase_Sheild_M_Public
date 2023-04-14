using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResultOptionsClassLibrary;

namespace DB_Controls
{
    /// <summary>
    /// контрол для отображения инфы об антеннах
    /// </summary>
    public partial class AntennOptionsUserControl : UserControl
    {
        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public AntennOptionsUserControl()
        {
            InitializeComponent();
        }

        protected List<AntennOptionsClass> _AllAntenn = new List<AntennOptionsClass>();
        protected AntennOptionsClass _SelectedAntenn = new AntennOptionsClass();

        protected ISaver_ToDataBase _Saver_ToDataBase = null;

        public ISaver_ToDataBase Saver_ToDataBase
        {
            get { return _Saver_ToDataBase; }
            set 
            {
                if (value != null)
                {
                    _Saver_ToDataBase = value;

                    if (!this.DesignMode)
                    {
                        this.ClearList();
                        this.AddAntenn(this.Saver_ToDataBase.LoadAllAntenn());
                        this.NeedToSaveAntenEvent+=new NeedToSaveAntenDelegate(this.Saver_ToDataBase.Save_Antenn);
                        this.Saver_ToDataBase.AddNewAntennEvent += new AddNewAntennDelegate(Saver_ToDataBase_AddNewAntennEvent);

                    }
                }
            }
        }

        void Saver_ToDataBase_AddNewAntennEvent(object Sender, AntennOptionsClass Obj)
        {
            this.AddAntenn(Obj, false);
        }

        /// <summary>
        /// Класс опций антеннны
        /// </summary>
public AntennOptionsClass SelectedAntenn
        {
            get { return this._SelectedAntenn; }
            set
            {
                _SelectedAntenn = value;
                if (!this.DesignMode)
                {
                    this.dontUpdateIndexChange = true;
                    this.updateList();
                    this.dontUpdateIndexChange = false;
                }
            }
        }

        /// <summary>
        /// Все антенны
        /// get
        /// </summary>
        public List<AntennOptionsClass> AllAntenn
        {
            get { return _AllAntenn; }
        }

        /// <summary>
        /// добавить антенну в массив
        /// </summary>
        /// <param name="newConnector"></param>
        /// <param name="SelectThis">выбрать этот конектор в списке</param>
        public void AddAntenn(AntennOptionsClass newAnten, bool SelectThis)
        {
            //чтоб по несколько раз не обновлял
            dontUpdateIndexChange = true;
            this._AllAntenn.Add(newAnten);
            if (SelectThis)
            {
                this._SelectedAntenn = newAnten;
            }
            updateList();
            dontUpdateIndexChange = false;
        }

        /// <summary>
        /// добавить массив антенн
        /// </summary>
        /// <param name="newConnectors"></param>
        public void AddAntenn(List<AntennOptionsClass> newConnectors)
        {
            this._AllAntenn.AddRange(newConnectors);
            //чтоб по несколько раз не обновлял
            dontUpdateIndexChange = true;
            updateList();
            dontUpdateIndexChange = false;
        }

        /// <summary>
        /// удалить все добавленные антены
        /// </summary>
        public void ClearList()
        {
            //чтоб по несколько раз не обновлял
            dontUpdateIndexChange = true;
            this._AllAntenn.Clear();
            updateList();
            dontUpdateIndexChange = false;
        }

        #region блокировка от изменения
        public bool LockControl = false;

        /// <summary>
        /// заблокировать/разблокировать весь контрол от изменения true - lock
        /// </summary>
        /// <param name="Lock"></param>
       public void LockUnlockAll(bool Lock)
       {
           LockControl = Lock;
           EditMode = !Lock;

           checkBoxUsingAsZond.AutoCheck = !Lock;

           //comboBoxName.Enabled = !Lock;
           textBoxDescriptions.ReadOnly = Lock;
           textBoxZavNumber.ReadOnly = Lock;
       }

        #endregion

       #region флаги и вспомогательные переменные
       /// <summary>
       /// true - запрет обновления
       /// </summary>
       bool dontUpdateIndexChange = false;
       /// <summary>
       /// true - запрет генерации
       /// </summary>
       bool dontGenerateEvent = false;
       /// <summary>
       /// опции антенны были изменены
       /// </summary>
       bool DataChanged = false;
       /// <summary>
       /// индекс изменённой антенны
       /// </summary>
       protected int? Editindex = null;
       /// <summary>
       /// редактирование данных разрешено
       /// </summary>
       protected bool EditMode = true;
       protected string SaveName = "";
       #endregion

       /// <summary>
       /// обновить список Антенн
       /// </summary>
       void updateList()
       {
           this.SuspendLayout();

          bool SelectConnectorIsSet = false;
            comboBoxName.Items.Clear();
            if (_AllAntenn.Count > 0)
            {
                for (int i = 0; i < _AllAntenn.Count; i++)
                {
                    comboBoxName.Items.Add(_AllAntenn[i].Name);
                    if (_SelectedAntenn != null)
                    {
                        if (_AllAntenn[i].id == _SelectedAntenn.id)
                        {
                            SelectConnectorIsSet = true;
                            comboBoxName.SelectedItem = comboBoxName.Items[i];

                            #region отображаем данные о выбранной антенне

                            checkBoxUsingAsZond.Checked = this._AllAntenn[i].UsingAsZond;

                            comboBoxName.SelectedItem = this._AllAntenn[i].Name;

                            textBoxDescriptions.Text = this._AllAntenn[i].Description;
                            textBoxZavNumber.Text = this._AllAntenn[i].ZAVNumber;

                            this.comboBoxName.DropDownStyle = ComboBoxStyle.DropDownList;
                            #endregion
                        }
                    }
                }
                this.comboBoxName.Items.Add("<Добавить новый>");
                if (!SelectConnectorIsSet)
                {
                    this.dontUpdateIndexChange = false;
                    this.dontGenerateEvent = true;
                    this.comboBoxName.SelectedIndex = this.comboBoxName.Items.Count - 1;
                    this.dontGenerateEvent = false;
                }
            }
            else
            {
                this.comboBoxName.Items.Add("<Добавить новый>");
                this.dontUpdateIndexChange = false;
                this.dontGenerateEvent = true;
                this.comboBoxName.SelectedIndex = this.comboBoxName.Items.Count - 1;
                this.dontGenerateEvent = false;
            }

            this.ResumeLayout();
       }

       /// <summary>
       /// событие изменения выбранной антенны
       /// при изменении выбранной антенны кодом событие не генерируется
       /// </summary>
       public event SelectedAntennChangeDelegate SelectedAntennChangeEvent;

       public delegate void SelectedAntennChangeDelegate(object sender, AntennOptionsClass newSelectAntenn);
        
       private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (!dontUpdateIndexChange)
           {
               dontUpdateIndexChange = true;
               this.Save();

               //если выбран пункт "добавить новый"
               if (comboBoxName.SelectedIndex == comboBoxName.Items.Count - 1)
               {
                   //добавление нового
                   #region отображаем данные о новой антенне
                   this.comboBoxName.DropDownStyle = ComboBoxStyle.DropDown;

                   checkBoxUsingAsZond.Checked = false;
                   textBoxDescriptions.Text = "";
                   textBoxZavNumber.Text = "";
                   #endregion

                   this._SelectedAntenn = null;
               }
               else
               {
                   this.comboBoxName.DropDownStyle = ComboBoxStyle.DropDownList;
                   this._SelectedAntenn = this._AllAntenn[this.comboBoxName.SelectedIndex];
                   dontUpdateIndexChange = true;
                   this.updateList();
                   dontUpdateIndexChange = false;
               }
               try
               {
                   //генерируем событие об изменении выбранного конектора
                   if (!dontGenerateEvent)
                   {
                       this.SelectedAntennChangeEvent(this, this._SelectedAntenn);
                   }
               }
               catch(NullReferenceException) { }
               dontUpdateIndexChange = false;
           }
       }

       protected void Save()
       {
           if (DataChanged)
           {
               DataChanged = false;

               if (Editindex == -1 || comboBoxName.SelectedIndex == -1 || comboBoxName.SelectedItem.ToString() == "<Добавить новый>" || Editindex == this._AllAntenn.Count)
               {
                   if (MessageBox.Show(string.Format("Была добавлена новая антенна.\n Сохранить изменения?"), "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                   {
                       AntennOptionsClass anten = new AntennOptionsClass();

                       if (this.SaveName == "")
                       {
                           this.SaveName = "Без имени";
                       }
                       anten.Name = this.SaveName;

                       #region получить опции антенны с контрола

                       anten.UsingAsZond = checkBoxUsingAsZond.Checked;
                       anten.Description = textBoxDescriptions.Text;
                       anten.ZAVNumber = textBoxZavNumber.Text;
                       #endregion

                       this._SelectedAntenn = anten;
                       _AllAntenn.Add(anten);
                       try
                       {
                           this.SelectedAntennChangeEvent(this, anten);
                       }
                       catch { }
                       #region генерация события сохранения
                       try
                       {
                           this.NeedToSaveAntenEvent(this, anten);
                       }
                       catch
                       {
                           MessageBox.Show("Сохранение данных в базу данных не реализовано. Изменения не сохранены", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       }
                       #endregion

                       this.comboBoxName.DropDownStyle = ComboBoxStyle.DropDownList;
                   }
                   else
                   {
                       this.dontGenerateEvent = true;
                   }

               }
               else
               {

                   if (MessageBox.Show(string.Format("Были изменены данные антенны {0}.\n Сохранить изменения?", _AllAntenn[Convert.ToInt32(Editindex)].Name), "Изменение данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                   {
                       AntennOptionsClass anten = _AllAntenn[Convert.ToInt32(Editindex)];

                       #region получить опции антенны с контрола

                       anten.UsingAsZond = checkBoxUsingAsZond.Checked;
                       anten.Description = textBoxDescriptions.Text;
                       anten.ZAVNumber = textBoxZavNumber.Text;
                       #endregion

                       #region генерация события сохранения
                       try
                       {
                           this.NeedToSaveAntenEvent(this, anten);
                       }
                       catch
                       {
                           MessageBox.Show("Сохранение данных в базу данных не реализовано. Изменения не сохранены", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       }
                       #endregion

                       this._SelectedAntenn = _AllAntenn[comboBoxName.SelectedIndex];
                   }
               }

               this.dontUpdateIndexChange = true;
               this.updateList();
               this.dontUpdateIndexChange = false;
               this.dontGenerateEvent = false;
           }

       }

       /// <summary>
       /// Реализовать это событие, чтобы антенны могли сохранятся в бд и нормально обновлять свои id - для новых добавленных
       /// если id - отрицательное значит нодо сохранить в бд как новый и после сохранения в бд( и её обновления) записать присвоенный id переменной 
       /// если id положительный значит просто обновить запись, соответственного id
       /// </summary>
       public event NeedToSaveAntenDelegate NeedToSaveAntenEvent;

       /// <summary>
       /// делегат для события созранения конекторов
       /// </summary>
       /// <param name="Sender"></param>
       /// <param name="SaveConnector"></param>
       public delegate void NeedToSaveAntenDelegate(object Sender, AntennOptionsClass SaveObj);

       #region обработка событий изменения данных в контроле
      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!dontUpdateIndexChange)
            {
                if (EditMode)
                {
                    DataChanged = true;
                    Editindex = comboBoxName.SelectedIndex;
                }
            }
        }

        private void textBoxOpisanie_TextChanged(object sender, EventArgs e)
        {
            if (!dontUpdateIndexChange)
            {
                if (EditMode)
                {
                    DataChanged = true;
                    Editindex = comboBoxName.SelectedIndex;
                }
            }
        }

        private void textBoxZavNumber_TextChanged(object sender, EventArgs e)
        {
            if (!dontUpdateIndexChange)
            {
                if (EditMode)
                {
                    DataChanged = true;
                    Editindex = comboBoxName.SelectedIndex;
                }
            }
        }

        private void comboBoxName_TextUpdate(object sender, EventArgs e)
        {
            if (!dontUpdateIndexChange)
            {
                if (EditMode)
                {
                    DataChanged = true;
                    Editindex = comboBoxName.SelectedIndex;
                    this.SaveName = this.comboBoxName.Text;
                }
            }
        }

        private void AntennOptionsUserControl_Leave(object sender, EventArgs e)
        {
            this.Save();
        }
            
       #endregion
    }
}
