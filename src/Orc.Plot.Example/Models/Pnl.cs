namespace Orc.Plot.Example
{
    using System;
    using System.Diagnostics;
    using Orc.Plot.Animations;
    using OxyPlot.Axes;

    [DebuggerDisplay("X: {X} => {FinalX} | Y: {Y} => {FinalY}")]
    public class Pnl : IAnimatablePoint
    {
        public double FinalX { get; set; }
        public double FinalY { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public DateTime Time
        {
            get { return DateTimeAxis.ToDateTime(X); }
            set
            {
                var finalX = DateTimeAxis.ToDouble(value);

                FinalX = finalX;
                X = finalX;
            }
        }

        public double Value
        {
            get { return Y;}
            set
            {
                FinalY = value;
                Y = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:HH:mm} {1:0.0}", Time, Value);
        }
    }
}
