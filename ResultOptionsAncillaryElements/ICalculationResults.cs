using System;
namespace ResultOptionsClassLibrary
{
    public interface ICalculationResults
    {
       // MeasurementTypeEnum MeasurementType { get; }

        PointDouble Координаты_фазового_центра_Decart { get; }
        PointDouble Координаты_фазового_центра_Polar { get; }
        double Коэффициент_усиления_в_максимуме_диаграммы_направленности { get; }
        double Коэффициент_усиления_в_минимуме_диаграммы_направленности { get; }
        double Коэффициент_Эллиптичности { get; }
        double Направление_максимума_диаграммы_направленности { get; }
        ComplexClass Поляризационное_отношение { get; }
        double Смещение_первого_бокового_лепестка_относительно_максимума_диаграммы_направленности { get; }
        double Угол_наклона_эллипса_поляризации { get; }
        double Уровень_боковых_лепестков { get; }
        double Ширина_диаграммы_направленности_по_половине_мощности { get; }
        double Отношение_MaxMin { get; }

        MistakeClass Погрешность_КУ { get; }
        MistakeClass Погрешность_ДН { get; }
        MistakeClass Погрешность_ПХ { get; }
        MistakeClass Погрешность_ФД { get; }
        MistakeClass Погрешность_КУ_в_МАХ { get; }
        MistakeClass Погрешность_Напр_МАХ_ДН { get; }
        MistakeClass Погрешность_Ширина_ДН { get; }
        MistakeClass Погрешность_УБЛ { get; }
        MistakeClass Погрешность_Смещения_лепестка { get; }
        MistakeClass Погрешность_Степени_кросс_поляизации { get; }
    }
}
