
using Common.Entities;
using Common.Entities.Parking;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Entities.Validation
{
    public class InputAgs
    {
        /// <summary>
        /// 识别的车牌
        /// </summary>
        public string DistinguishPlatenumber = "";
        /// <summary>
        ///  是否需要操作设备 目前只用在异常放行
        /// </summary>
        public bool IsOperatorDevice = true;
        /// <summary>
        /// 放行类型 1 正常放行 2 异常放行
        /// </summary>
        public int SourceType = 1;
        /// <summary>
        /// 必填 - 车场信息
        /// </summary>
        public ParkArea AreadInfo { set; get; }

        /// <summary>
        /// 必填 - 车牌信息
        /// </summary>
        public PlateInfo Plateinfo { set; get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ParkGrant CardInfo { set; get; }
        /// <summary>
        /// 转临停前月卡信息
        /// </summary>
        public ParkGrant LastCardInfo { set; get; }

        /// <summary>
        /// 必填 - 卡片类型
        /// </summary>
        public ParkCarType CarTypeInfo { set; get; }

        /// <summary>
        ///必填 -  通道信息
        /// </summary>
        public ParkGate GateInfo { set; get; }

        /// <summary>
        /// 必填 - 车类型
        /// </summary>
        public ParkCarModel CarModel { set; get; }


        /// <summary>
        /// 读卡器读取的卡号
        /// </summary>
        public string InCardNo { set; get; }

        /// <summary>
        /// 小车场对应的通行记录
        /// </summary>
        public ParkIORecord NestAreaIORecord { set; get; }

        /// <summary>
        /// 进场记录
        /// </summary>
        public ParkIORecord IORecord { set; get; }

        /// <summary>
        /// 进场记录(小车场 内场 时序记录)
        /// </summary>
        public ParkTimeseries Timeseries { set; get; }

        /// <summary>
        /// 是否更改车类型
        /// </summary>
        public bool IsChangCarType { set; get; }

        /// <summary>
        /// 人工处理
        /// </summary>
        public bool Artificial { set; get; }

        /// <summary>
        /// 人工处理原因
        /// </summary>
        public string ArtificialRemarks { set; get; }

        #region 优免相关(消费打折)

        /// <summary>
        /// 身份证信息
        /// </summary>
        public IdentityInfo IdentityInfo { set; get; }
        #endregion

        public  ObservableCollection<PKCarBitNum> CarBitNumes { set; get; }

        /// <summary>
        /// 是否是扫码进出
        /// </summary>
        public bool IsScanQRCode = false;

        #region 称重
        /// <summary>
        /// 净重
        /// </summary>
        public decimal NetWeight { set; get; }

        /// <summary>
        /// 皮重
        /// </summary>
        public decimal Tare { set; get; }
        /// <summary>
        /// 物品
        /// </summary>
        public string Goods { set; get; } 
         /// <summary>
         /// 货主
         /// </summary>
        public string Shipper { set; get; } 
        /// <summary>
        /// 仓位号
        /// </summary>
        public string Shippingspace { set; get; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string DocumentsNo { set; get; }
         
        #endregion
    }
}
