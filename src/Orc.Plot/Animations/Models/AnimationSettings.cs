namespace Orc.Plot.Animations
{
    using System;

    public class AnimationSettings
    {
        private static readonly IEasingFunction DefaultEasingFunction = new NoEase();

        public AnimationSettings()
        {
            HorizontalPercentage = 70;
            VerticalPercentage = 30;

            EasingFunction = DefaultEasingFunction;
            Delay = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationDelay);
            Duration = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationDuration);
            FrameDuration = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationFrameDuration);
        }

        public double? MinimumValue { get; set; }

        public double HorizontalPercentage { get; set; }

        public double VerticalPercentage { get; set; }

        public TimeSpan Delay { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan FrameDuration { get; set; }

        public IEasingFunction EasingFunction { get; set; }
    }
}
