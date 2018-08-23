namespace Common.Utilities
{
    using System;
    public class Round : AmountCalculator
    {
        public Round(int digits)
            : base(digits) {
        }

        public override decimal Execute(decimal amount) {
            return Common.Utilities.Calculator.Round(amount, Digits);
        }
    }
}
