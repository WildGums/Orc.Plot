namespace Orc.Plot.Animations
{
    public class NoEase : IEasingFunction
    {
        public double Ease(double value)
        {
            return 1d;
        }
    }
}
