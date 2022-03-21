namespace Orc.Plot.Example.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Animations;
    using OxyPlot;
    using OxyPlot.Series;

    public class LineSeriesViewModel : AnimationViewModelBase
    {
        public override bool SupportsEasingFunction { get { return true; } }

        public override async Task AnimateAsync(AnimationSettings animationSettings)
        {
            var plotModel = PlotModel;
            var series = plotModel.Series.First() as LineSeries;
            if (series is not null)
            {
                await plotModel.AnimateSeriesAsync(series, animationSettings);
            }
        }

        protected override ItemsSeries GetPlotSeries()
        {
            return new LineSeries
            {
                Title = "P & L",
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse("#4CAF50"),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#4CAF50"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
            };
        }

        protected override string GetPlotTitle() => "Line Series Animation Demo";
    }
}
