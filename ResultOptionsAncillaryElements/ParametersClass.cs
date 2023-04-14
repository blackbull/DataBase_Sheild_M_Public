using System;
using System.Collections.Generic;
using System.Text;

using GeneralInterfaces;
using System.Xml.Serialization;
using ZVBOptionsClassLibrary;

namespace ResultOptionsClassLibrary
{
    /// <summary>
    /// параметры измерения
    /// </summary>
    [Serializable]
    public class MeasurementParametrsClass : ICloneable
    {
        public MeasurementParametrsClass() { }

        public decimal StartOPU_Y = 0;
        public decimal StartOPU_W = 0;
        public decimal StartTower_Y = 0;
        public decimal StartTower_W = 0;

        public decimal StopOPU_Y = 0;
        public decimal StopOPU_W = 0;
        public decimal StopTower_Y = 0;
        public decimal StopTower_W = 0;

        public decimal StepMeasurement = 1;

        public decimal Power = 0;
        public decimal SweepTime = 0;
        public decimal Bandwidth = 0;
        public bool SweepTimeAuto = false;
        public MeasurementS__Enum MesurementS__Type;

        public decimal VOROTA = 0;
        
        /// <summary>
        /// таблица сегменотов
        /// </summary>
        //[XmlIgnore]
        public List<SegmentTableElementOptionsClass> SegmentTable = new List<SegmentTableElementOptionsClass>();

        public object Clone()
        {
            return this.MemberwiseClone(); 
        }
    }
}
