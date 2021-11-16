namespace Orc.Plot.Example.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Orc.Plot.Animations;
    using OxyPlot;
    using OxyPlot.Series;

    public class LinearBarViewModel : AnimationViewModelBase
    {
        public override bool SupportsEasingFunction { get { return true; } }

        public override async Task AnimateAsync(AnimationSettings animationSettings)
        {
            var plotModel = PlotModel;
            var series = plotModel.Series.First() as LinearBarSeries;
            if (series is not null)
            {
                await plotModel.AnimateSeriesAsync(series, animationSettings);
            }
        }

        protected override ItemsSeries GetPlotSeries()
        {
            return new LinearBarSeries
            {
                Title = "P & L",
                DataFieldX = "Time",
                DataFieldY = "Value",
                FillColor = OxyColor.Parse("#454CAF50"),
                StrokeColor = OxyColor.Parse("#4CAF50"),
                StrokeThickness = 1,
                BarWidth = 5
            };
        }

        protected override string GetPlotTitle() => "Linear Bar Series Animation Demo";
    }
}
