namespace Orc.Plot.Animations
{
    public class QuinticEase : IEasingFunction
    {
        public double Ease(double value)
        {
            double num = value;
            return num * num * value * value * value;
        }
    }
}
