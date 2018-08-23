namespace Common.Utilities
{
    using System;
    public class Floor : AmountCalculator
    {
        public Floor(int digits)
            : base(digits) {
        }

        public override decimal Execute(decimal amount) {
            return Common.Utilities.Calculator.Floor(amount, Digits);
        }
    }
}
