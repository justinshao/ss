using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum CertifType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        IDCard = 1,
        /// <summary>
        /// 驾驶证
        /// </summary>
        [Description("驾驶证")]
        DriverLicense = 2,
        /// <summary>
        /// 居住证
        /// </summary>
        [Description("居住证")]
        ResidencePermit = 3
    }
}
