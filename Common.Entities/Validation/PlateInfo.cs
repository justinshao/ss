using System;

namespace Common.Entities.Validation
{
    public class PlateInfo
    {
        public string Ip { set; get; }
        public int Port { set; get; }
        public string CarImageName { set; get; }
        /// <summary>
        /// 车辆图片路径 图片存储后返回
        /// </summary>
        public string CarImagePath { set; get; }
        public string LicenseNum { set; get; }
        public DateTime TriggerTime { set; get; }
        public byte[] PicBuffer { set; get; }
        public byte[] PicBufferSmall { set; get; }
        public int Believe { set; get; }


        public string PlateColor { set; get; }

        /// <summary>
        /// 用来播放LED 声音的车牌号
        /// </summary>
        public string PlayPlateNmber
        {
            get
            {
                if (LicenseNum.StartsWith("无车牌")
                    || LicenseNum.StartsWith("无牌车"))
                {
                    return "无牌车";
                }
                return LicenseNum;
            }
        }

        /// <summary>
        /// 特征码
        /// </summary>
        public byte[] ResFeature { set; get; }

        /// <summary>
        /// 车标
        /// </summary>
        public string LogName { set; get; }
        /// <summary>
        /// 车型 1 小车 2 大车
        /// </summary>
        public int CarModelType { set; get; }
    }
}
