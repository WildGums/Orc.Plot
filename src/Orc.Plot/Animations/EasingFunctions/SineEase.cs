namespace Orc.Plot.Animations
{
    using System;

    public class SineEase : IEasingFunction
    {
        public double Ease(double value)
        {
            return 1.0 - Math.Sin(Math.PI / 2.0 * (1.0 - value));
        }
    }
}
