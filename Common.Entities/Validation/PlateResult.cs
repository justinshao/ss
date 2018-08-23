using System;

namespace Common.Entities.Validation
{
    public class PlateResult
    {
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { set; get; }

        /// <summary>
        /// 设备IP
        /// </summary>
        public string Ip { set; get; }

        /// <summary>
        /// 设备端口
        /// </summary>
        public int Port { set; get; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string LicenseNum { set; get; }
        /// <summary>
        /// 车辆照片
        /// </summary>
        public string CarImageName { set; get; }

        /// <summary>
        /// 车牌类型
        /// </summary>
        public string PlateType { set; get; }

        /// <summary>
        /// 车身颜色
        /// </summary>
        public string CarColor { set; get; }

        /// <summary>
        /// 车型 1小车 2大车
        /// </summary>
        public int CarModelType { set; get; }

        /// <summary>
        /// 车辆大图
        /// </summary>
        public byte[] PicBuffer { set; get; }

        /// <summary>
        /// 车辆小图
        /// </summary>
        public byte[] PicBufferSmall { set; get; }

        public int Believe { set; get; }

        /// <summary>
        /// 特征码
        /// </summary>
        public byte[] ResFeature { set; get; }

        /// <summary>
        /// 车标
        /// </summary>
        public string LogName { set; get; }

    }
}
