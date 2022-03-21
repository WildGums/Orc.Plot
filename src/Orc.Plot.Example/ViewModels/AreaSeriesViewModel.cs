namespace Orc.Plot.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Orc.Plot.Animations;
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class AreaSeriesViewModel : AnimationViewModelBase
    {
        public override bool SupportsEasingFunction { get { return true; } }

        public override async Task AnimateAsync(AnimationSettings animationSettings)
        {
            var plotModel = PlotModel;
            var series = plotModel.Series.First() as AreaSeries;
            if (series is not null)
            {
                await plotModel.AnimateSeriesAsync(series, animationSettings);
            }
        }

        protected override ItemsSeries GetPlotSeries()
        {
            return new AreaSeries
            {
                Title = "P & L",
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse("#4CAF50"),
                Fill = OxyColor.Parse("#454CAF50"),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#4CAF50"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
            };
        }

        protected override string GetPlotTitle() => "Area Series Animation Demo";
    }
}
