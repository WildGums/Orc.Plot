namespace Orc.Plot.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Animations;

    public abstract class AnimationViewModelBase : PlotViewModelBase, IAnimatable
    {
        protected AnimationViewModelBase()
        {
            EasingFunctions = (from type in typeof(CircleEase).Assembly.GetTypes()
                               where type.GetInterfaces().Any(x => x == typeof(IEasingFunction)) && !type.IsAbstract
                               orderby type.Name
                               select type).ToList();
            SelectedEasingFunction = EasingFunctions.FirstOrDefault();

            if (!SupportsEasingFunction)
            {
                SelectedEasingFunction = typeof(NoEase);
            }

            AnimationDuration = 1000;
            AnimationFrameDuration = AnimationExtensions.DefaultAnimationFrameDuration;
            HorizontalPercentage = 70;
            VerticalPercentage = 30;
        }

        public abstract bool SupportsEasingFunction { get; }

        public List<Type> EasingFunctions { get; private set; }

        public Type SelectedEasingFunction { get; set; }

        public double HorizontalPercentage { get; set; }

        public double VerticalPercentage { get; set; }

        public int AnimationDelay { get; set; }

        public int AnimationDuration { get; set; }

        public int AnimationFrameDuration { get; set; }

        public Task AnimateAsync()
        {
            var easingFunction = (IEasingFunction)Activator.CreateInstance(SelectedEasingFunction);

            var animationSettings = new AnimationSettings
            {
                EasingFunction = easingFunction,
                Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                FrameDuration = TimeSpan.FromMilliseconds(AnimationFrameDuration),
                Delay = TimeSpan.FromMilliseconds(AnimationDelay),
                HorizontalPercentage = HorizontalPercentage,
                VerticalPercentage = VerticalPercentage
            };

            return AnimateAsync(animationSettings);
        }

        public abstract Task AnimateAsync(AnimationSettings animationSettings);
    }
}
