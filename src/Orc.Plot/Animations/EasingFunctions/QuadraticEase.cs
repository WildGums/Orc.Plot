namespace Orc.Plot.Animations
{
    public class QuadraticEase : IEasingFunction
    {
        public double Ease(double value)
        {
            double num = value;
            return num * num;
        }
    }
}
