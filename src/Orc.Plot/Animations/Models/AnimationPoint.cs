namespace Orc.Plot.Animations
{
    using System.Diagnostics;

    [DebuggerDisplay("{X} / {Y} (IsVisible = {IsVisible})")]
    public class AnimationPoint
    {
        public AnimationPoint()
        {
            IsVisible = true;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public bool IsVisible { get; set; }
    }
}
