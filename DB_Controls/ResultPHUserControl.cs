using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using ResultOptionsClassLibrary;

namespace DB_Controls
{
    public partial class ResultPHUserControl : UserControl
    {
        public ResultPHUserControl()
        {
            InitializeComponent();
        }

        protected ICalculationResults _CalculationResult = null;

        protected IResultType_MAIN _Result = null;

        public IResultType_MAIN Result
        {
            get { return _Result; }
            set
            {
                if (value != null)
                {
                    _Result = value;
                    _CalculationResult = _Result.SelectedPolarization.SelectedFrequency._CalculationResults;
                    this.FillControl();
                }
            }
        }


        #region функции загрузки данных в контрол
        delegate void voidFunc();

        protected void FillControl()
        {
            voidFunc vd = delegate
            {
                try
                {
                    //чистим все поля
                    this.textBoxFullMistake.Text = "";
                    this.textBoxКоэффициент_Эллиптичности.Text = "";
                    this.textBoxПоляризационное_отношение.Text = "";
                    this.textBoxУгол_наклона_эллипса_поляризации.Text = "";
                    this.textBoxMaxMin.Text = "";
                    this.textBoxMIN.Text = "";

                    #region загрузка данных

                    this.textBoxFullMistake.Text = string.Format("-----\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_ПХ);

                    this.textBoxКоэффициент_Эллиптичности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Коэффициент_Эллиптичности) + string.Format("\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_Степени_кросс_поляизации);

                    if (CheckDataClass.CheckForBad(_CalculationResult.Поляризационное_отношение))
                    {
                        this.textBoxПоляризационное_отношение.Text = _CalculationResult.Поляризационное_отношение.ToString();
                    }
                    else
                    {
                        this.textBoxПоляризационное_отношение.Text = "NaN";
                    }

                    this.textBoxУгол_наклона_эллипса_поляризации.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Угол_наклона_эллипса_поляризации);

                    this.textBoxMaxMin.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Отношение_MaxMin) + " дБ";
                    this.textBoxMIN.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Коэффициент_усиления_в_минимуме_диаграммы_направленности) + string.Format(" дБ\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_КУ_в_МАХ);

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Не удалось загрузить рассчитанные характеристики, ResultUserControl \n {0}", ex.Message), ex);
                }
                    #endregion
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
    }
}
