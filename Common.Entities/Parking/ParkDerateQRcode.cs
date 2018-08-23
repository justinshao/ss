using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Common.Utilities.Helpers;

namespace Common.Entities.Parking
{
    public class ParkDerateQRcode
    {
        public int ID { get; set; }
        /// <summary>
        /// 记录编号
        /// </summary>
        public string RecordID { get; set; }
        /// <summary>
        /// 优免规则编号
        /// </summary>
        public string DerateID { get; set; }
        /// <summary>
        /// 优免规则类型
        /// </summary>
        public int DerateType { get; set; }
        /// <summary>
        /// 优免值
        /// </summary>
        public decimal DerateValue { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        public string StartTimeToString { 
            get {
                return StartTime.ToString();
            } 
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        public string EndTimeToString
        {
            get
            {
                return EndTime.ToString();
            }
        }
        /// <summary>
        /// 状态2-无效
        /// </summary>
        public DataStatus DataStatus { get; set; }
        /// <summary>
        /// 能使用测试 0-不限
        /// </summary>
        public int CanUseTimes { get; set; }
        /// <summary>
        /// 已使用次数
        /// </summary>
        public int AlreadyUseTimes { get; set; }
        /// <summary>
        /// 使用次数描述
        /// </summary>
        public string UseTimesDes { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public DerateQRCodeSource DataSource { get; set; }
        /// <summary>
        /// 操作者编号
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public string CreateTimeToString
        {
            get
            {
                return CreateTime.ToString();
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 修改失败
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 是否修改
        /// </summary>
        public int HaveUpdate { get; set; }
        /// <summary>
        /// 二维码数据（base64）
        /// </summary>
        public string ImageData { get; set; }
        /// <summary>
        /// 优免规则名称
        /// </summary>
        public string DerateName { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string SellerName { get; set; }
        /// <summary>
        /// 小区编号
        /// </summary>
        public string VID { get; set; }
        /// <summary>
        /// 车场编号
        /// </summary>
        public string PKID { get; set; }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkName { get; set; }
        /// <summary>
        /// 用户账号（仅用于展示）
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 商家号（仅用于展示）
        /// </summary>
        public string SellerNo { get; set; }
        /// <summary>
        /// 优免二维码类型 0-长期 1-一次性 
        /// </summary>
        public int DerateQRcodeType { get; set; }
        /// <summary>
        /// 最后一次发券二维码的压缩文件路径
        /// </summary>
        public string DerateQRcodeZipFilePath { get; set; }
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperatorAccount {
            get {
                if (DataSource == DerateQRCodeSource.Platefrom) {
                    return UserAccount;
                }
                return SellerNo;
            }
        }
    }
}
