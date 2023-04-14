using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using ResultOptionsClassLibrary;

namespace DB_Controls
{
    public partial class ResultKYUserControl : UserControl
    {
        public ResultKYUserControl()
        {
            InitializeComponent();
        }

        protected IResultType_КУ _Result = null;

        public IResultType_MAIN Result
        {
            get { return _Result; }
            set
            {
                if (value != null)
                {
                    if (value is IResultType_КУ)
                    {
                        _Result = value as IResultType_КУ;
                        this.FillControl();
                    }
                }
            }
        }


        #region функции загрузки данных в контрол
        delegate void voidFunc();

        protected void FillControl()
        {
            voidFunc vd = delegate
            {
                DontUpdate = true;
                this.comboBoxFreq.Items.Clear();

                this.comboBoxFreq.Items.AddRange(_Result.SUM_Polarization.FrequencyElements.ToArray());
                DontUpdate = false;

                if (this.comboBoxFreq.Items.Count != 0)
                {
                    this.comboBoxFreq.SelectedIndex = 0;
                }
            };

            if (this.InvokeRequired)
            {
                this.Invoke(vd);
            }
            else
            {
                vd();
            }
        }

        #endregion


        bool DontUpdate = false;

        private void comboBoxFreq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DontUpdate)
            {
                try
                {
                    //чистим все поля
                    this.textBoxSUM.Text = "";
                    this.textBoxКоэффициент_Эллиптичности.Text = "";
                    this.textBoxПоляризационное_отношение.Text = "";
                    this.textBoxУгол_наклона_эллипса_поляризации.Text = "";

                    #region загрузка данных

                    //берём значение напрямик из измрения ку

                    FrequencyElementClass FreqSum = comboBoxFreq.SelectedItem as FrequencyElementClass;

                    double Freq = FreqSum.Frequency;

                    //FrequencyElementClass FreqSum = _Result.Main_Polarization.FindFrequencyElement(Freq);
                    FrequencyElementClass FreqMain = _Result.Main_Polarization.FindFrequencyElement(Freq);
                    FrequencyElementClass FreqCros = _Result.Cross_Polarization.FindFrequencyElement(Freq);

                    string DataSum = CheckDataClass.CheckAndConvertToString(Math.Round(FreqSum.ResultAmpl_PhaseElements[0].Ampl_dB, 2));
                    string DataMain = CheckDataClass.CheckAndConvertToString(Math.Round(FreqMain.ResultAmpl_PhaseElements[0].Ampl_dB, 2));
                    string DataCros = CheckDataClass.CheckAndConvertToString(Math.Round(FreqCros.ResultAmpl_PhaseElements[0].Ampl_dB, 2));

                    string deltaMistakeFull = CheckDataClass.CheckAndConvertToString(FreqSum._CalculationResults.Погрешность_КУ);

                    this.textBoxSUM.Text = string.Format("{0} дБ \t{1}", DataSum, deltaMistakeFull);
                    this.textBoxMain.Text = string.Format("{0} дБ \t{1}", DataMain, deltaMistakeFull);
                    this.textBoxCross.Text = string.Format("{0} дБ \t{1}", DataCros, deltaMistakeFull);



                    this.textBoxКоэффициент_Эллиптичности.Text = CheckDataClass.CheckAndConvertToString(FreqSum._CalculationResults.Коэффициент_Эллиптичности) + string.Format("\t ") + CheckDataClass.CheckAndConvertToString(FreqSum._CalculationResults.Погрешность_Степени_кросс_поляизации);

                    if (CheckDataClass.CheckForBad(FreqSum._CalculationResults.Поляризационное_отношение))
                    {
                        this.textBoxПоляризационное_отношение.Text = FreqSum._CalculationResults.Поляризационное_отношение.ToString();
                    }
                    else
                    {
                        this.textBoxПоляризационное_отношение.Text = "NaN";
                    }

                    this.textBoxУгол_наклона_эллипса_поляризации.Text = CheckDataClass.CheckAndConvertToString(FreqSum._CalculationResults.Угол_наклона_эллипса_поляризации);


                }
                catch (Exception ex)
                {
                    //throw new Exception(string.Format("Не удалось загрузить рассчитанные характеристики, ResultUserControl \n {0}", ex.Message), ex);
                }
                    #endregion
            }
        }
    }
}
