using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 车辆基础类型
    /// </summary>
    public enum BaseCarType
    {
        /// <summary>
        /// VIP车
        /// </summary>
        [Description("VIP车")]
        VIPCar = 0,
        /// <summary>
        /// 储值车
        /// </summary>
        [Description("储值车")]
        StoredValueCar = 1,
        /// <summary>
        /// 月租车
        /// </summary>
        [Description("月租车")]
        MonthlyRent = 2,
        /// <summary>
        /// 临时车
        /// </summary>
        [Description("临时车")]
        TempCar = 3,
        /// <summary>
        /// 工作车
        /// </summary>
        //[Description("工作车")]
        //WorkCar = 4
         /// <summary>
        /// 季租车
        /// </summary>
        [Description("季租车")]
        SeasonRent = 5,
        /// <summary>
        ///年租车 
        /// </summary>
        [Description("年租车")]
        YearRent = 6,
        /// <summary>
        ///自定义月租车 
        /// </summary>
        [Description("自定义月租车")]
        CustomRent = 7,
    }
}
