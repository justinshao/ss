using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKFee
{
    public class Userdefined : IFeeRule
    {
        XDocument _docxml;

        public Userdefined()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isFirstFree"></param>
        /// <param name="docxml">自定义算费XML</param>
        public Userdefined(int isFirstFree, XDocument docxml)
        {
            IsFirstFree = isFirstFree;
            _docxml = docxml;
        }
        Dictionary<int, decimal> DichourFeeDetails = new Dictionary<int, decimal>();
        public override decimal CalcFee()
        {
            int FirstDayFee = 0;
            int freeTime = 0;
            foreach (XElement item in _docxml.Root.Elements())
            {
                if (IsFirstFree == 1)
                {
                    freeTime = int.Parse(item.Attribute("FreeTime").Value);
                }
                if (item.Attribute("FirstDayFee") != null)
                {
                    FirstDayFee = int.Parse(item.Attribute("FirstDayFee").Value);
                }
                if (int.Parse(item.Attribute("Type").Value) == 1)//分时计费
                {
                    foreach (var detailItem in item.Elements("RuleDetail"))
                    {
                        int hour = int.Parse(detailItem.Attribute("Hours").Value);
                        if (!DichourFeeDetails.ContainsKey(hour))
                        {
                            DichourFeeDetails.Add(int.Parse(detailItem.Attribute("Hours").Value), decimal.Parse(detailItem.Attribute("Fee").Value));
                        }
                    }
                }
            }
            if (ParkingBeginTime.AddMinutes(freeTime) >= ParkingEndTime)
            {
                return 0;
            }
            decimal oneDayTotalFee = 0;
            foreach (var item in DichourFeeDetails)
            {
                oneDayTotalFee = oneDayTotalFee + item.Value;
            }
            TimeSpan span = ParkingEndTime - ParkingBeginTime;
            if (span.TotalDays < 1)
            {
                if (FirstDayFee > 0)
                {
                    return FirstDayFee;
                }
                return GetHoursFee(span.TotalHours);
            }
            else if (span.TotalDays == 1)
            {
                if (FirstDayFee > 0)
                {
                    return FirstDayFee;
                }
                return oneDayTotalFee;
            }
            else
            {
                if (FirstDayFee > 0)
                { 
                    span = new TimeSpan(span.Days - 1, span.Hours, span.Minutes, span.Seconds);
                    var fee = ((int)span.TotalDays) * oneDayTotalFee + GetHoursFee(span.TotalHours - (int)span.TotalDays * 24);
                    return FirstDayFee + fee;
                }
                else
                { 
                    return ((int)span.TotalDays) * oneDayTotalFee + GetHoursFee(span.TotalHours - (int)span.TotalDays * 24);
                } 
            }
        }

        private decimal GetHoursFee(double hours)
        {
            decimal fee = 0;
            int maxHours = 0;
            foreach (var item in DichourFeeDetails)
            {
                if (item.Key <= hours)
                {
                    fee = fee + item.Value;
                    if (item.Key > maxHours)
                    {
                        maxHours = item.Key;
                    }
                }
            }
            if ((hours - (int)hours) > 0)
            {
                maxHours++;
                if (maxHours <= 24 && DichourFeeDetails.ContainsKey(maxHours))
                {
                    fee = fee + DichourFeeDetails[maxHours];
                }
            }
            return fee;
        }
    }
}
