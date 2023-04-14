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
    public partial class ResultSDNMUserControl : UserControl
    {
        public ResultSDNMUserControl()
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
                    this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Text = "";
                    this.textBoxНаправление_максимума_диаграммы_направленности.Text = "";
                    this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Text = "";
                    this.textBoxУровень_боковых_лепестков.Text = "";
                    this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Text = "";
                    this.textBoxКоординаты_фазового_центра_1.Text = "";
                    this.textBoxКоординаты_фазового_центра_2.Text = "";

                    this.textBoxКоэффициент_Эллиптичности.Text = "";
                    this.textBoxПоляризационное_отношение.Text = "";
                    this.textBoxУгол_наклона_эллипса_поляризации.Text = "";

                    this.textBoxMaxMin.Text = "";
                    this.textBoxMIN.Text = "";

                    #region загрузка данных

                    this.textBoxFullMistake.Text = string.Format("-----\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_ДН);

                    this.textBoxКоэффициент_усиления_в_максимуме_диаграммы_направленности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Коэффициент_усиления_в_максимуме_диаграммы_направленности) + string.Format(" дБ\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_КУ_в_МАХ);

                    this.textBoxНаправление_максимума_диаграммы_направленности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Направление_максимума_диаграммы_направленности) + string.Format(" °\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_Напр_МАХ_ДН);

                    this.textBoxСмещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности) + string.Format(" °\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_Смещения_лепестка);

                    this.textBoxУровень_боковых_лепестков.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Уровень_боковых_лепестков) + string.Format(" дБ\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_УБЛ);

                    this.textBoxШирина_диаграммы_направленности_по_половине_мощности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Ширина_диаграммы_направленности_по_половине_мощности) + string.Format(" °\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_Ширина_ДН);




                    this.textBoxКоэффициент_Эллиптичности.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Коэффициент_Эллиптичности) + string.Format("\t ") + CheckDataClass.CheckAndConvertToString(_CalculationResult.Погрешность_Степени_кросс_поляизации);

                    if (CheckDataClass.CheckForBad(this._CalculationResult.Поляризационное_отношение))
                    {
                        this.textBoxПоляризационное_отношение.Text = this._CalculationResult.Поляризационное_отношение.ToString();
                    }
                    else
                    {
                        this.textBoxПоляризационное_отношение.Text = "NaN";
                    }


                    this.textBoxУгол_наклона_эллипса_поляризации.Text = CheckDataClass.CheckAndConvertToString(_CalculationResult.Угол_наклона_эллипса_поляризации);





                    if (CheckDataClass.CheckForBad(this._CalculationResult.Координаты_фазового_центра_Decart))
                    {
                        textBoxКоординаты_фазового_центра_1.Text = ResultTypesClassLibrary.ResultType_MAINClass.CordinatePhaseCentre(this._CalculationResult.Координаты_фазового_центра_Decart, false, this._Result.MainOptions.MeasurementResultType);
                    }
                    else
                    {
                        this.textBoxКоординаты_фазового_центра_1.Text = "Не удалось рассчитать";
                    }

                    if (CheckDataClass.CheckForBad(this._CalculationResult.Координаты_фазового_центра_Polar))
                    {
                        this.textBoxКоординаты_фазового_центра_2.Text = ResultTypesClassLibrary.ResultType_MAINClass.CordinatePhaseCentre(this._CalculationResult.Координаты_фазового_центра_Polar, true, this._Result.MainOptions.MeasurementResultType);
                    }
                    else
                    {
                        this.textBoxКоординаты_фазового_центра_2.Text = "Не удалось рассчитать";
                    }

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
