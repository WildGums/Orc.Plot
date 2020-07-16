namespace Orc.Plot.Animations
{
    using System;
    using System.Threading.Tasks;

    public interface IAnimatable
    {
        bool SupportsEasingFunction { get; }

        Task AnimateAsync();
        Task AnimateAsync(AnimationSettings animationSettings);
    }
}
