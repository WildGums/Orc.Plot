// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Plot.Example.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            Title = "Orc.Plot example";
        }
        #endregion

        #region Properties
        public PlotModel Plot { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            UpdatePlot();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

        private void UpdatePlot()
        {
            // Create some data
            var items = new Collection<Item>
            {
                new Item {Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19},
                new Item {Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9},
                new Item {Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29}
            };

            // Create the plot model
            var plotModel = new PlotModel
            {
                Title = "Column series",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendOrientation = LegendOrientation.Vertical
            };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            plotModel.Axes.Add(new CategoryAxis
            {
                ItemsSource = items,
                LabelField = "Label"
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            });

            // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
            plotModel.Series.Add(new ColumnSeries
            {
                Title = "2009",
                ItemsSource = items,
                ValueField = "Value1"
            });

            plotModel.Series.Add(new ColumnSeries
            {
                Title = "2010",
                ItemsSource = items,
                ValueField = "Value2"
            });

            plotModel.Series.Add(new ColumnSeries
            {
                Title = "2011",
                ItemsSource = items,
                ValueField = "Value3"
            });

            Plot = plotModel;
        }
        #endregion
    }
}
