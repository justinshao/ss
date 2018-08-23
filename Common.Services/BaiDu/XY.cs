using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class XY
    {
        /// <summary>
        /// 1米的经度偏移量
        /// </summary>
        public const double MeterOfLongitudeOffset = 0.000009;

        /// <summary>
        /// 1米的纬度偏移量
        /// </summary>
        public const double MeterOfLatitudeOffset = 0.0000099;
        /// <summary>
        /// 计算两点之前的直线距离
        /// </summary>
        /// <param name="currX">当前位置经度</param>
        /// <param name="currY">当前位置纬度</param>
        /// <param name="toX">需要到达的位置经度</param>
        /// <param name="toY">需要到达的位置纬度</param>
        /// <returns></returns>
        public static double DistanceTo(double currX, double currY, double toX, double toY)
        {
            double distanceX = (currX - toX) / MeterOfLongitudeOffset;//经度距离，单位米
            double distanceY = (currY - toY) / MeterOfLatitudeOffset;//纬度距离，单位米

            return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        }
    }
}
