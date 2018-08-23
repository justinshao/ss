namespace Common.Utilities
{
    using System;

    public static  class Calculator
    {
        public static decimal Round(decimal value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            decimal bits = (decimal)Math.Pow(10, (double)digits);
            return (decimal)(decimal)((int)(value / bits + flg * 0.5M)) * bits;
        }
        public static double Round(double value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            double bits = Math.Pow(10, (double)digits);
            return (double)((int)(value / bits + flg * 0.5)) * bits;
        }
        /// <summary>
        /// 小于等于，舍
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal Floor(decimal value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            decimal bits = (decimal)Math.Pow(10, (double)digits);
            return Math.Floor(Math.Abs(value) / bits) * bits * flg;
        }
        public static double Floor(double value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            double bits = Math.Pow(10, (double)digits);
            return Math.Floor(Math.Abs(value) / bits) * bits * flg;
        }
        /// <summary>
        /// 大于等于，入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal Ceiling(decimal value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            decimal bits = (decimal)Math.Pow(10, (double)digits);
            return Math.Ceiling(Math.Abs(value) / bits) * bits * flg;
        }
        public static double Ceiling(double value, int digits) {
            int flg = value >= 0 ? 1 : -1;
            double bits = Math.Pow(10, (double)digits);
            return Math.Ceiling(Math.Abs(value) / bits) * bits * flg;
        }
        public static AmountCalculator GetPayInterfaceCalculator(string payInterface)
        {
            return new InterfaceAmountCalculator(-2);
        }
        public static AmountCalculator GetRoundTwo() {
            return new InterfaceAmountCalculator(-2);
        }
      
    }
    public abstract class AmountCalculator
    {
        public AmountCalculator(int digits)
        {
            this.Digits = digits;
        }

        protected int Digits { get; private set; }

        public abstract decimal Execute(decimal amount);
    }
    public class InterfaceAmountCalculator : AmountCalculator
    {
        public InterfaceAmountCalculator(int digits)
            : base(digits)
        {
        }

        public override decimal Execute(decimal amount)
        {
            return Common.Utilities.Calculator.Round(amount, Digits);
        }
    }
}
