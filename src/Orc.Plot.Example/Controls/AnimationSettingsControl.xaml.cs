namespace Orc.Plot.Example.Controls
{
    using System.Windows;
    using Orc.Plot.Animations;

    public partial class AnimationSettingsControl
    {
        public AnimationSettingsControl()
        {
            InitializeComponent();
        }

        private async void OnAnimateClick(object sender, RoutedEventArgs e)
        {
            var animatable = DataContext as IAnimatable;
            if (animatable is not null)
            {
                await animatable.AnimateAsync();
            }
        }
    }
}
