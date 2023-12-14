namespace Orc.Plot
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Catel.Logging;
    using Controls;
    using OxyPlot;
    using OxyPlot.Axes;

    [TemplatePart(Name = ResetButtonName, Type = typeof(Button))]
    [TemplatePart(Name = HorizontalRangeSliderName, Type = typeof(RangeSlider))]
    [TemplatePart(Name = VerticalRangeSliderName, Type = typeof(RangeSlider))]
    public class PlotView : OxyPlot.Wpf.PlotView
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string ResetButtonName = "PART_ResetButton";
        private const string HorizontalRangeSliderName = "PART_HorizontalRangeSlider";
        private const string VerticalRangeSliderName = "PART_VerticalRangeSlider";

        /// <summary>Identifies the <see cref="ShowYAxisSlider" /> dependency property.</summary>
        public static readonly DependencyProperty ShowYAxisSliderProperty = DependencyProperty.Register(nameof(ShowYAxisSlider), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>Identifies the <see cref="ShowXAxisSlider" /> dependency property.</summary>
        public static readonly DependencyProperty ShowXAxisSliderProperty = DependencyProperty.Register(nameof(ShowXAxisSlider), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>Identifies the <see cref="ShowResetButton" /> dependency property.</summary>
        public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(nameof(ShowResetButton), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        private Button? _resetButton;
        private SliderAxisSynchronizer? _verticalSliderAxisSynchronizer;
        private SliderAxisSynchronizer? _horizontalSliderAxisSynchronizer;

        static PlotView()
        {
            ModelProperty.OverrideMetadata(typeof(PlotView), new PropertyMetadata((d, e) =>
            {
                var self = (PlotView)d;
                self.OnModelChanged((PlotModel)e.OldValue, (PlotModel)e.NewValue);
            }));
        }

        public bool ShowYAxisSlider
        {
            get => (bool)GetValue(ShowYAxisSliderProperty);
            set => SetValue(ShowYAxisSliderProperty, value);
        }

        public bool ShowResetButton
        {
            get => (bool)GetValue(ShowResetButtonProperty);
            set => SetValue(ShowResetButtonProperty, value);
        }

        public bool ShowXAxisSlider
        {
            get => (bool)GetValue(ShowXAxisSliderProperty);
            set => SetValue(ShowXAxisSliderProperty, value);
        }

        private void OnModelChanged(PlotModel? oldModel, PlotModel? newModel)
        {
            if (oldModel is not null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                oldModel.Updated -= ModelOnUpdated;
#pragma warning restore CS0618 // Type or member is obsolete
            }

            if (newModel is not null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                newModel.Updated += ModelOnUpdated;
#pragma warning restore CS0618 // Type or member is obsolete
            }

            _ = Dispatcher.BeginInvoke(new Action(() => UpdateSliders()));

            void ModelOnUpdated(object? sender, EventArgs args) => UpdateSliders();
        }

        private void UpdateSliders()
        {
            if (ActualModel is null)
            {
                return;
            }

            if (_horizontalSliderAxisSynchronizer is not null)
            {
                _horizontalSliderAxisSynchronizer.Axis = ActualModel.DefaultXAxis;
            }

            if (_verticalSliderAxisSynchronizer is not null)
            {
                _verticalSliderAxisSynchronizer.Axis = ActualModel.DefaultYAxis;
            }
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes (such as a
        ///     rebuilding layout pass)
        ///     call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" /> . In simplest terms, this means the method is
        ///     called
        ///     just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_resetButton is not null)
            {
                _resetButton.Click -= ResetButtonOnClick;
            }

            _resetButton = GetTemplateChild(ResetButtonName) as Button;

            if (_resetButton is not null)
            {
                _resetButton.Click += ResetButtonOnClick;
            }

            _horizontalSliderAxisSynchronizer?.Unsubscribe();
            _verticalSliderAxisSynchronizer?.Unsubscribe();

            if (GetTemplateChild(HorizontalRangeSliderName) is RangeSlider horizontalRangeSlider)
            {
                _horizontalSliderAxisSynchronizer = new SliderAxisSynchronizer(horizontalRangeSlider);
            }
            else
            {
                _horizontalSliderAxisSynchronizer = null;
            }

            if (GetTemplateChild(VerticalRangeSliderName) is RangeSlider verticalRangeSlider)
            {
                _verticalSliderAxisSynchronizer = new SliderAxisSynchronizer(verticalRangeSlider);
            }
            else
            {
                _verticalSliderAxisSynchronizer = null;
            }
        }

        private void ResetButtonOnClick(object? sender, RoutedEventArgs e) => ResetAllAxes();

        private class SliderAxisSynchronizer
        {
            private readonly RangeSlider _rangeSlider;
            private Axis? _axis;
            private bool _ignoreSlide;

            public SliderAxisSynchronizer(RangeSlider rangeSlider)
            {
                ArgumentNullException.ThrowIfNull(rangeSlider);

                _rangeSlider = rangeSlider;
                _rangeSlider.LowerValueChanged += HandleSlide;
                _rangeSlider.UpperValueChanged += HandleSlide;
            }

            public Axis? Axis
            {
                get => _axis;
                set
                {
                    if (ReferenceEquals(_axis, value))
                    {
                        return;
                    }

                    if (_axis is not null)
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        _axis.AxisChanged -= OnAxisChanged;
#pragma warning restore CS0618 // Type or member is obsolete
                    }

                    _axis = value;

                    if (_axis is not null)
                    {
                        _rangeSlider.SetCurrentValue(IsEnabledProperty, true);
#pragma warning disable CS0618 // Type or member is obsolete
                        _axis.AxisChanged += OnAxisChanged;
#pragma warning restore CS0618 // Type or member is obsolete

                        UpdateBounds();
                        UpdateValues();
                    }
                    else
                    {
                        _rangeSlider.SetCurrentValue(IsEnabledProperty, false);
                    }

                    bool UpdateBounds()
                    {
                        var axis = Axis;
                        if (axis is null)
                        {
                            return false;
                        }

                        var minimum = axis.Minimum;
                        if (double.IsNaN(minimum))
                        {
                            var methodInfo = typeof(Axis).GetMethod("CalculateActualMinimum", BindingFlags.Instance | BindingFlags.NonPublic);
                            if (methodInfo is not null)
                            {
                                minimum = (double?)methodInfo.Invoke(Axis, null) ?? minimum;
                            }
                        }

                        if (double.IsNaN(minimum))
                        {
                            Log.Debug($"Can't update minimum, no value available (yet)");
                            return false;
                        }

                        var maximum = axis.Maximum;
                        if (double.IsNaN(maximum))
                        {
                            var methodInfo = typeof(Axis).GetMethod("CalculateActualMaximum", BindingFlags.Instance | BindingFlags.NonPublic);
                            if (methodInfo is not null)
                            {
                                maximum = (double?)methodInfo.Invoke(Axis, null) ?? maximum;
                            }
                        }

                        if (double.IsNaN(maximum))
                        {
                            Log.Debug($"Can't update maximum, no value available (yet)");
                            return false;
                        }

                        _rangeSlider.SetCurrentValue(RangeBase.MinimumProperty, minimum);
                        _rangeSlider.SetCurrentValue(RangeBase.MaximumProperty, maximum);

                        return true;
                    }

                    void OnAxisChanged(object? o, AxisChangedEventArgs eventArgs)
                    {
                        if (eventArgs.ChangeType == AxisChangeTypes.Reset)
                        {
                            UpdateBounds();
                        }

                        UpdateValues();
                    }

                    void UpdateValues()
                    {
                        var axis = Axis;
                        if (axis is null)
                        {
                            return;
                        }

                        _ignoreSlide = true;
                        _rangeSlider.SetCurrentValue(RangeSlider.LowerValueProperty, axis.ActualMinimum);
                        _rangeSlider.SetCurrentValue(RangeSlider.UpperValueProperty, axis.ActualMaximum);
                        _ignoreSlide = false;
                    }
                }
            }

            public void Unsubscribe()
            {
                _rangeSlider.LowerValueChanged -= HandleSlide;
                _rangeSlider.UpperValueChanged -= HandleSlide;
            }

            private void HandleSlide(object? sender, EventArgs args)
            {
                if (_ignoreSlide || Axis?.PlotModel is null)
                {
                    return;
                }
                
                Axis.Zoom(_rangeSlider.LowerValue, _rangeSlider.UpperValue);
                Axis.PlotModel.InvalidatePlot(false);
            }
        }
    }
}
