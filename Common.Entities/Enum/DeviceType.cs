using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum DeviceType
    {
       
        /// <summary>
        /// 大华
        /// </summary>
        [Description("相机_TDH7")]
        DH_NVR = 0,
        /// <summary>
        /// 海康
        /// </summary>
        [Description("相机-THK9")]
        HK_NVR = 1,
        /// <summary>
        /// 火眼
        /// </summary>
        [Description("相机-THY2")]
        HY_NVR = 2,
        /// <summary>
        /// SW
        /// </summary>
        [Description("相机-THX8")]
        SW_NVR = 3,
        /// <summary>
        /// 欧冠LED
        /// </summary>
        [Description("显示屏-TOG1")]
        OG_LED = 4,
        /// <summary>
        /// 车场控制器
        /// </summary>
        //[Description("车场控制器")]
        //PK_CLR = 5,
        /// <summary>
        /// 抓拍相机
        /// </summary>
        [Description("XM抓拍相机")]
        XM_DVR = 6,
        /// <summary>
        /// 芊熠相机
        /// </summary>
        [Description("相机-TQY6")]
        QY_CAMERA = 7,
        /// <summary>
        /// TKFLED
        /// </summary>
        [Description("TKF_显示屏")]
        TNL_CONTROL = 8,
        /// <summary>
        /// 大华_EN
        /// </summary>
        [Description("相机_TDH_EN7")]
        DH_EN_NVR = 9,
        /// <summary>
        /// 外接显示屏
        /// </summary>
        [Description("ED_WIN_SCREEN")]
        ED_WIN_SCREEN = 10,
        /// <summary>
        /// 
        /// </summary>
        [Description("相机-ZK")]
        ZK_NVR = 11,
      
        /// <summary>
        /// 
        /// </summary>
        [Description("NZ-显示屏")]
        NZ_CONTROL = 12,

        /// <summary>
        /// 自助缴费机
        /// </summary>
        [Description("自助缴费机")]
        ED_SELPPAYMENT = 13,

        /// <summary>
        /// 科发相机
        /// </summary>
        [Description("相机-KF")]
        NVR_KF = 14,
        /// <summary>
        /// 科发相机
        /// </summary>
        [Description("相机-TSZ3")]
        HY_SZ_NVR = 15,

        /// <summary>
        /// 门禁控制器
        /// </summary>
        [Description("CARD_ID_RZ1")]
        CARD_ID_RZ = 16,

        /// <summary>
        /// 华安相机
        /// </summary>
        [Description("相机-HA112")]
        NVR_HA = 17,
        /// <summary>
        /// FACEID
        /// </summary>
        [Description("相机-FACEID")]
        XF_FACEID = 18,

        /// <summary>
        /// KST
        /// </summary>
        [Description("KST-显示屏")]
        KST_CONTROL = 19,

        /// <summary>
        /// 自助通道
        /// </summary>
        [Description("ED_SELFGATE_SCREEN")]
        ED_SELFGATE_SCREEN = 20,

        /// <summary>
        /// 真地
        /// </summary>
        [Description("ZD_CONTROL")]
        ZD_CONTROL = 21,
        /// <summary>
        /// 地磅
        /// </summary>
        [Description("地磅")]
        DiBan = 22,
        /// <summary>
        /// 相机-通富港
        /// </summarys>
        [Description("相机-TFG")]
        NVR_TFG = 23,


        /// <summary>
        /// 相机-蓝卡
        /// </summarys>
        [Description("相机-蓝卡")]
        LK_CAMERA = 25,


        /// <summary>
        /// 相机-捷安顺
        /// </summarys>
        [Description("相机-捷安顺")]
        JAS_CAMERA = 26,


        /// <summary>
        /// 相机-德亚
        /// </summarys>
        [Description("相机-TS")]
        TS_NVR = 27,
        /// <summary>
        /// 屏卡-红门
        /// </summarys>
        [Description("红门_LED")]
        HM_LED = 28,

        /// <summary>
        /// 相机-新赛菲姆
        /// </summarys>
        [Description("相机-新赛菲姆")]
        SF_NVR = 29,
    }

    public enum DeviceTypeBK
    { 
        /// <summary>
        /// 未设置
        /// </summary>
        [Description("未设置")]
        UnKnow = -1,
        /// <summary>
        /// 科发
        /// </summary>
        [Description("BK_KF")]
        BK_KF = 0,
        /// <summary>
        /// 真地
        /// </summary>
        [Description("BK_ZD")]
        BK_ZD = 1,
        /// <summary>
        /// 通富港
        /// </summary>
        [Description("BK_TFG")]
        BK_TFG = 2,

        /// <summary>
        /// 欧冠
        /// </summary>
        [Description("BK_OG")]
        BK_OG = 3,

        /// <summary>
        /// 赛菲姆
        /// </summary>
        [Description("BK_SF")]
        BK_SF = 4,


        /// <summary>
        /// 睿泰科
        /// </summary>
        [Description("BK_RTK")]
        BK_RTK = 5,
    }
}
