using System;
using System.Collections.Generic;
using System.Text;

namespace ResultOptionsClassLibrary
{
#warning Тип измерения  При изменении id_Типа у типов измерений - править сдесь
    /// <summary>
    /// Тип измерения  При изменении id_Типа у типов измерений - править сдесь
    /// </summary>
    public enum MeasurementTypeEnum
    {
        Коэффицент_усиления = 0,
        Поляризационная_диаграмма = 1,
        ДН_Меридиан = 2,
        ДН_Азимут = 3,
        Суммарная_ДН_Меридиан = 4,
        Суммарная_ДН_Азимут = 5
    }

    /// <summary>
    /// описание типов измерения (глобальный справочник описания типов измерений)
    /// </summary>
    public static class MeasurementTypeDescription
    {
        private static Dictionary<MeasurementTypeEnum, string> _MeasurementType_Name = null;
        public static Dictionary<MeasurementTypeEnum, string> MeasurementType_Name
        {
            get
            {
                if (_MeasurementType_Name == null)
                    FillDictionary();
                return _MeasurementType_Name;
            }
        }


        private static Dictionary<MeasurementTypeEnum, string> _MeasurementType_Description = null;
        public static Dictionary<MeasurementTypeEnum, string> MeasurementType_Description
        {
            get
            {
                if (_MeasurementType_Description == null)
                    FillDictionary();
                return _MeasurementType_Description;
            }
        }



        private static void FillDictionary()
        {
            _MeasurementType_Name = new Dictionary<MeasurementTypeEnum, string>();
            _MeasurementType_Description = new Dictionary<MeasurementTypeEnum, string>();


            Array arr = (Enum.GetValues(typeof(MeasurementTypeEnum)));

            foreach (object obj in arr)
            {
                if (obj is MeasurementTypeEnum)
                {
                    MeasurementTypeEnum TempType = (MeasurementTypeEnum)obj;

                    switch (TempType)
                    {
                        case MeasurementTypeEnum.Коэффицент_усиления:
                            {
                                _MeasurementType_Name.Add(TempType, "Коэффициент усиления");
                                _MeasurementType_Description.Add(TempType, "Измерение коэффициента усиления по полной мощности при двух ортогональных положениях ТА (W' - поляризация)");
                                break;
                            }
                        case MeasurementTypeEnum.Поляризационная_диаграмма:
                            {
                                _MeasurementType_Name.Add(TempType, "Поляризационная диаграмма");
                                _MeasurementType_Description.Add(TempType, "Измерение поляризационной диаграммы при вращении ТА  (W' - поляризация)");
                                break;
                            }
                        case MeasurementTypeEnum.ДН_Меридиан:
                            {
                                _MeasurementType_Name.Add(TempType, "Диаграмма направленности (Меридиан)");
                                _MeasurementType_Description.Add(TempType, "Измерение диаграммы направленности при вращении испытуемой АФС в меридиональной плоскости  (Y -меридиан)");
                                break;
                            }
                        case MeasurementTypeEnum.ДН_Азимут:
                            {
                                _MeasurementType_Name.Add(TempType, "Диаграмма направленности (Азимут)");
                                _MeasurementType_Description.Add(TempType, "Измерение диаграммы направленности при вращении испытуемой АФС в азимутальной плоскости  (W - азимут)");
                                break;
                            }
                        case MeasurementTypeEnum.Суммарная_ДН_Меридиан:
                            {
                                _MeasurementType_Name.Add(TempType, "Суммарная диаграмма направленности по мощности (Меридиан)");
                                _MeasurementType_Description.Add(TempType, "Измерение диаграммы направленности по полной мощности при вращении испытуемой АФС в меридиональной плоскости  (Y - меридиан) при двух ортогональных положениях ТА (W' - поляризация)");
                                break;
                            }
                        case MeasurementTypeEnum.Суммарная_ДН_Азимут:
                            {
                                _MeasurementType_Name.Add(TempType, "Суммарная диаграмма направленности по мощности (Азимут)");
                                _MeasurementType_Description.Add(TempType, "Измерение диаграммы направленности по полной мощности при вращении испытуемой АФС в азимутальной плоскости  (W -азимут) при двух ортогональных положениях ТА (W' - поляризация)");
                                break;
                            }

                        default:
                            {
                                _MeasurementType_Name.Add(TempType, "Название не указано");
                                _MeasurementType_Description.Add(TempType, "Описания нет, обратитесь к разработчику");

                                break;
                            }
                    }

                }
            }
        }

    }

    /// <summary>
    /// Тип рассчитываемого результата
    /// </summary>
    public enum CalculationResultTypeEnum
    {
        None = 0,
        Sum_by_RAZ = 1,
        Difference = 2,
        Averaged = 3,
        Sum_by_DB = 4
    }


    /// <summary>
    /// Тип слагаемого в рассчитываемом результатате
    /// </summary>
    public enum CalculationComponentTypeEnum
    {
        Слагаемое = 0,
        Уменьшаемое = 1,
        Вычитаемое = 2,
        Усреднённое = 3
    }
}
