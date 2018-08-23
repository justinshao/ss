using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum PortType
    {
        /// <summary>
        /// 串口
        /// </summary>
        [Description("串口")]
        SerialPort = 0,
        /// <summary>
        /// TCP/IP
        /// </summary>
        [Description("TCP/IP")]
        TCPIP = 1
    }
}
