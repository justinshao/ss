using Common.Entities.Other;
using Common.Entities.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinInerface
{
    public class ParkingFeeService
    {

        private static Dictionary<string, TempParkingFeeResult> FeeData = new Dictionary<string, TempParkingFeeResult>();

        public static bool SetParkingFee(string key, TempParkingFeeResult info)
        {
            //if (FeeData.ContainsKey(key))
            //{
            //    FeeData[key] = info;
            //}
            //else
            //{
            //    FeeData.Add(key,info);
            //}
            FeeData[key] = info;

            return true;
        }

        public static bool DeleteParkingFee(string key)
        {
            if (FeeData.ContainsKey(key))
            {
                FeeData.Remove(key);
            }

            return true;
        }

        public static TempParkingFeeResult GetParkingFee(string key)
        {
            if (FeeData.ContainsKey(key))
            {
                return FeeData[key];
            }
            else
            {
               return null;
            }
        }

        /// <summary>
        /// 删除过期的数据
        /// </summary>
        /// <returns></returns>
        private static bool DeleteExpireData()
        {
            var result = (from fee in FeeData
                         where (DateTime.Now - fee.Value.PayDate).TotalMinutes > 5
                         select fee).ToList();

            if (result != null)
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    FeeData.Remove(result[i].Key);
                }
            }

            return true;
        }

        /// <summary>
        /// 根据车场ID 和 车牌  获取费用
        /// </summary>
        /// <param name="pid">车场ID</param>
        /// <param name="plateNo">车牌</param>
        /// <returns></returns>
        public static TempParkingFeeResult GetParkingFeeByParkingID(string pid, string plateNo)
        {
            DeleteExpireData();

            var result = from fee in FeeData
                         where fee.Value.ParkingID == pid && fee.Value.PlateNumber == plateNo
                         orderby fee.Value.PayDate descending
                         select fee;

            if (result == null || result.Count() == 0)
            {
                return null;
            }
            else
            {
                var pair = result.First();
                return pair.Value;
            }


        }

        /// <summary>
        /// 获取车场停车费用
        /// </summary>
        /// <param name="gid">通道ID</param>
        /// <returns></returns>
        public static TempParkingFeeResult GetParkingFeeByGateID(string pid, string gid)
        {
            DeleteExpireData();

            var result = from fee in FeeData
                         where fee.Value.GateID == gid && fee.Value.ParkingID == pid
                         orderby fee.Value.PayDate descending
                         select fee;

            if (result == null || result.Count() == 0)
            {
                return null;
            }
            else
            {
                var pair = result.First();
                return pair.Value;
            }

        }

        /// <summary>
        /// 获取停车费用
        /// </summary>
        /// <param name="pid">车场ID</param>
        /// <param name="bid">岗亭ID</param>
        /// <returns></returns>
        public static TempParkingFeeResult GetParkingFeeByBoxID(string bid)
        {

            DeleteExpireData();

            var result = from fee in FeeData
                         where fee.Value.BoxID == bid
                         orderby fee.Value.PayDate descending
                         select fee;

            if (result == null || result.Count() == 0)
            {
                return null;
            }
            else
            {
                var pair = result.First();
                return pair.Value;
            }
        }


    }
}
