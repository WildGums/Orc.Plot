using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Plot.Example.ViewModels
{
    using Catel.MVVM;
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public abstract class PlotViewModelBase : ViewModelBase
    {
        public PlotModel PlotModel { get;}

        public PlotViewModelBase()
        {
            PlotModel = new PlotModel();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var pnls = new List<Pnl>();

            var random = new Random(31);
            var dateTime = DateTime.Today.Add(TimeSpan.FromHours(9));
            for (var pointIndex = 0; pointIndex < 50; pointIndex++)
            {
                pnls.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });
                dateTime = dateTime.AddMinutes(1);
            }

            var minimum = pnls.Min(x => x.Value);
            var maximum = pnls.Max(x => x.Value);

            var plotModel = PlotModel;
            plotModel.Title = GetPlotTitle();

            var plotSeries = GetPlotSeries();
            plotSeries.ItemsSource = pnls;
            plotModel.Series.Add(plotSeries);

            var annotation = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 0
            };
            plotModel.Annotations.Add(annotation);

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Hours,
                IntervalLength = 50
            };
            plotModel.Axes.Add(dateTimeAxis);

            var margin = (maximum - minimum) * 0.05;

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = minimum - margin,
                Maximum = maximum + margin,
            };
            plotModel.Axes.Add(valueAxis);
        }

        protected abstract ItemsSeries GetPlotSeries();

        protected abstract string GetPlotTitle();
    }
}
