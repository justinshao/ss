using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 人员类型
    /// </summary>
    public enum EmployeeType
    {
        /// <summary>
        /// 业主
        /// </summary>
        [Description("业主")]
        Owner = 1,
        /// <summary>
        /// 职员
        /// </summary>
        [Description("职员")]
        Staff = 2,
        /// <summary>
        /// 业主和职员
        /// </summary>
        [Description("业主和职员")]
        OwnerAndStaff = 3
    }
}
