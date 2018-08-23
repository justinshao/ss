using System;
using System.ComponentModel;
using Common.Entities;

namespace Common.Entities.Validation
{
    /// <summary>
    /// 车场车位数量
    /// </summary>
    public class PKCarBitNum : INotifyPropertyChanged
    {
        private ParkArea _area;

        public ParkArea Area
        {
            get { return _area; }
            set
            {
                _area = value;
                NotifyPropertyChanged("AreaName");
            }
        }

        public string AreaName
        {
            get
            {
                if (_area == null)
                {
                    return "";
                }
                return _area.AreaName;
            }
        }
        /// <summary>
        /// 总车位
        /// </summary>
        private uint _CarBitCount;

        public uint CarBitCount
        {
            get { return _CarBitCount; }
            set
            {
                _CarBitCount = value;
                NotifyPropertyChanged("CarBitCount");
            }
        }

        /// <summary>
        /// 在场车数量
        /// </summary>
        private uint _InParkCarNum;

        public uint InParkCarNum
        {
            get { return _InParkCarNum; }
            set
            {
                _InParkCarNum = value;
                NotifyPropertyChanged("InParkCarNum");
                NotifyPropertyChanged("SurplusCount");
            }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        private uint _SurplusCount;

        public uint SurplusCount
        {
            get
            {
                if (CarBitCount < InParkCarNum)
                {
                    SurplusCount = 0;
                }
                else
                {
                    SurplusCount = CarBitCount - InParkCarNum;
                }
                return _SurplusCount;
            }
            private set
            {

                if (_SurplusCount != value)
                {
                    _SurplusCount = value;
                    NotifyPropertyChanged("SurplusCount");
                    NotifyPropertyChanged("SurplusCountStr");
                }
            }
        }

        public string SurplusCountStr
        {
            get
            {
                return string.Format("[{0}]余位[{1}]", Area.AreaName, SurplusCount);
            }
        }
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
