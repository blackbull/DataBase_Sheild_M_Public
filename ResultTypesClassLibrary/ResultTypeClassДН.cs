using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResultOptionsClassLibrary;
using System.Xml.Serialization;
using Report_Sheild_M_Interfaces;

namespace ResultTypesClassLibrary
{
    /// <summary>
    /// Результат измерения ДН
    /// </summary>
    [Serializable]
    public class ResultTypeClassДН : ResultType_MAINClass, IResultType_ДН
    {
        public ResultTypeClassДН() { }

        public ResultTypeClassДН(bool FillMeasurementData)
            : base(FillMeasurementData)
        {
        }

        #region генерация интерфейсов для протоколов

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IAmplitudeDiagramDirection GetSelectedResult_for_Report_AmplitudeDiagram()
        {
            return new ResultTypeClassДН_for_Report_AmplitudeDiagram(this,0,this.SelectedPolarization.SelectedFrequencyIndex);
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public IPhaseDiagram GetSelectedResult_for_Report_PhaseDiagram()
        {
            return new ResultTypeClassДН_for_Report_PhaseDiagram(this, 0, this.SelectedPolarization.SelectedFrequencyIndex);
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IAmplitudeDiagramDirection> GetListResult_for_Report_AmplitudeDiagram()
        {
            List<IAmplitudeDiagramDirection> ret = new List<IAmplitudeDiagramDirection>();

            for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
            {
                ret.Add(new ResultTypeClassДН_for_Report_AmplitudeDiagram(this, 0, i));
            }
            return ret;
        }

        /// <summary>
        /// Получить интерфейс для генерации протокола текущей частоты и поляризации (Интерфейс остаётся привязанным к этому объекту, тч не изменяйте его до окончания работы с интерфейсом)
        /// </summary>
        public List<IPhaseDiagram> GetListResult_for_Report_PhaseDiagram()
        {
            List<IPhaseDiagram> ret = new List<IPhaseDiagram>();

            for (int i = 0; i < this.SelectedPolarization.FrequencyElements.Count; i++)
            {
                ret.Add(new ResultTypeClassДН_for_Report_PhaseDiagram(this, 0, i));
            }
            return ret;
        }

        #endregion
    }
}
