namespace Orc.Plot.Animations
{
    using System;

    public class BackEase : IEasingFunction
    {
        public BackEase()
        {
            Amplitude = 1d;
        }

        public double Amplitude { get; set; }

        public double Ease(double value)
        {
            var num = Math.Max(0.0, Amplitude);
            return Math.Pow(value, 3.0) - value * num * Math.Sin(Math.PI * value);
        }
    }
}
