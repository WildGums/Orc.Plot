// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Plot.Example.Views
{
    using System.Linq;
    using System.Windows.Controls;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Windows;

    public partial class MainWindow : DataWindow
    {
        #region Constructors
        public MainWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
        }
        #endregion
    }
}
