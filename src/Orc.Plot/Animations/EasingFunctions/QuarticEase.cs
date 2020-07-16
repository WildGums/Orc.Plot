namespace Orc.Plot.Animations
{
    public class QuarticEase : IEasingFunction
    {
        public double Ease(double value)
        {
            double num = value;
            return num * num * value * value;
        }
    }
}
