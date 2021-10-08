using System;
using System.Windows;
using System.Windows.Threading;
using Controller;
using Model;

namespace VisualisationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //WindowState = WindowState.Maximized;
            Data.Initialize();
            Data.NextRaceEvent += OnNextRace;
            Data.NextRace();
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            this.ImageComponent.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.ImageComponent.Source = null;
                    this.ImageComponent.Source = VisualizeWPF.DrawTrack(e.Track);
                }));
        }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            EditImage.ClearCache();
            VisualizeWPF.Initialize(e.Race);

            e.Race.DriversChanged += OnDriversChanged;
        }
    }
}
