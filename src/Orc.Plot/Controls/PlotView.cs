namespace Orc.Plot
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Controls;
    using OxyPlot;
    using OxyPlot.Axes;

    [TemplatePart(Name = ResetButtonName, Type = typeof(Button))]
    [TemplatePart(Name = HorizontalRangeSliderName, Type = typeof(RangeSlider))]
    [TemplatePart(Name = VerticalRangeSliderName, Type = typeof(RangeSlider))]
    public class PlotView : OxyPlot.Wpf.PlotView
    {
        private const string ResetButtonName = "PART_ResetButton";
        private const string HorizontalRangeSliderName = "PART_HorizontalRangeSlider";
        private const string VerticalRangeSliderName = "PART_VerticalRangeSlider";

        /// <summary>Identifies the <see cref="ShowYAxisSlider" /> dependency property.</summary>
        public static readonly DependencyProperty ShowYAxisSliderProperty = DependencyProperty.Register(nameof(ShowYAxisSlider), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>Identifies the <see cref="ShowXAxisSlider" /> dependency property.</summary>
        public static readonly DependencyProperty ShowXAxisSliderProperty = DependencyProperty.Register(nameof(ShowXAxisSlider), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>Identifies the <see cref="ShowResetButton" /> dependency property.</summary>
        public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(nameof(ShowResetButton), typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        private Button _resetButton;
        private SliderAxisSynchronizer _verticalSliderAxisSynchronizer;
        private SliderAxisSynchronizer _horizontalSliderAxisSynchronizer;

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

        private void OnModelChanged(PlotModel oldModel, PlotModel newModel)
        {
            if (oldModel != null)
            {
                oldModel.Updated -= ModelOnUpdated;
            }

            if (newModel != null)
            {
                newModel.Updated += ModelOnUpdated;
            }

            UpdateSliders();

            void ModelOnUpdated(object sender, EventArgs args) => UpdateSliders();
        }

        private void UpdateSliders()
        {
            if (ActualModel == null)
            {
                return;
            }

            if (_horizontalSliderAxisSynchronizer != null)
            {
                _horizontalSliderAxisSynchronizer.Axis = ActualModel.DefaultXAxis;
            }

            if (_verticalSliderAxisSynchronizer != null)
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

            if (_resetButton != null)
            {
                _resetButton.Click -= ResetButtonOnClick;
            }

            _resetButton = GetTemplateChild(ResetButtonName) as Button;

            if (_resetButton != null)
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

        private void ResetButtonOnClick(object sender, RoutedEventArgs e) => ResetAllAxes();

        private class SliderAxisSynchronizer
        {
            private readonly RangeSlider _rangeSlider;
            private Axis _axis;
            private bool _ignoreSlide;

            public SliderAxisSynchronizer(RangeSlider rangeSlider)
            {
                _rangeSlider = rangeSlider;
                _rangeSlider.LowerValueChanged += HandleSlide;
                _rangeSlider.UpperValueChanged += HandleSlide;
            }

            public Axis Axis
            {
                get => _axis;
                set
                {
                    if (ReferenceEquals(_axis, value))
                    {
                        return;
                    }

                    if (_axis != null)
                    {
                        _axis.AxisChanged -= OnAxisChanged;
                    }

                    _axis = value;

                    if (_axis != null)
                    {
                        _rangeSlider.SetCurrentValue(IsEnabledProperty, true);
                        _axis.AxisChanged += OnAxisChanged;
                        UpdateBounds();
                        UpdateValues();
                    }
                    else
                    {
                        _rangeSlider.SetCurrentValue(IsEnabledProperty, false);
                    }

                    void UpdateBounds()
                    {
                        var minimum = Axis.Minimum;
                        if (double.IsNaN(minimum))
                        {
                            minimum = (double)typeof(Axis).GetMethod("CalculateActualMinimum", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Axis, null);
                        }

                        var maximum = Axis.Maximum;
                        if (double.IsNaN(maximum))
                        {
                            maximum = (double)typeof(Axis).GetMethod("CalculateActualMaximum", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Axis, null);
                        }

                        _rangeSlider.SetCurrentValue(RangeBase.MinimumProperty, minimum);
                        _rangeSlider.SetCurrentValue(RangeBase.MaximumProperty, maximum);
                    }

                    void OnAxisChanged(object o, AxisChangedEventArgs eventArgs)
                    {
                        if (eventArgs.ChangeType == AxisChangeTypes.Reset)
                        {
                            UpdateBounds();
                        }

                        UpdateValues();
                    }

                    void UpdateValues()
                    {
                        _ignoreSlide = true;
                        _rangeSlider.SetCurrentValue(RangeSlider.LowerValueProperty, Axis.ActualMinimum);
                        _rangeSlider.SetCurrentValue(RangeSlider.UpperValueProperty, Axis.ActualMaximum);
                        _ignoreSlide = false;
                    }
                }
            }

            public void Unsubscribe()
            {
                _rangeSlider.LowerValueChanged -= HandleSlide;
                _rangeSlider.UpperValueChanged -= HandleSlide;
            }

            private void HandleSlide(object sender, EventArgs args)
            {
                if (_ignoreSlide || Axis?.PlotModel == null)
                {
                    return;
                }
                
                Axis.Zoom(_rangeSlider.LowerValue, _rangeSlider.UpperValue);
                Axis.PlotModel.InvalidatePlot(false);
            }
        }
    }
}
