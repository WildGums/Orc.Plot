namespace Orc.Plot.Animations
{
    using System;

    public class PowerEase : IEasingFunction
    {
        public PowerEase()
        {
            Power = 2d;
        }

        public double Power { get; set; }

        public double Ease(double value)
        {
            double y = Math.Max(0.0, Power);
            return Math.Pow(value, y);
        }
    }
}
