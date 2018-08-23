using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class MonthCard
    {
        public int Status { get; set; }
        public List<MonthCardResult> Result { get; set; }
    }
    public class MonthCardResult
    {
        public string ParkName { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxMonth { get; set; }
        public int MaxValue { get; set; }
        public int State { get; set; }
        public int Day { get; set; }
        public string CardID { get; set; }
        public string LicensePlate { get; set; }
    }
}
