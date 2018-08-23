namespace Common.Utilities
{
    using System;
    public class Default : AmountCalculator
    {
        public Default()
            : base(-2) {
        }

        public override decimal Execute(decimal amount) {
            return amount;
        }
    }
}
