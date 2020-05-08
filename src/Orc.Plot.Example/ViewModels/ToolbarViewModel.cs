namespace Orc.Plot.Example.ViewModels
{
    using OxyPlot;
    using OxyPlot.Series;

    public class ToolbarViewModel : PlotViewModelBase
    {
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

        protected override string GetPlotTitle() => "Plot ToolBar Demo";
    }
}
