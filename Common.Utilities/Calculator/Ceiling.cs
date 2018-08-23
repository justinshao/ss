namespace Common.Utilities
{
    using System;
    public class Ceiling : AmountCalculator
    {
        public Ceiling(int digits)
            : base(digits) {
        }

        public override decimal Execute(decimal amount) {
            return Common.Utilities.Calculator.Ceiling(amount, Digits);
        }
    }
}
