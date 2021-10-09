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
        private CompetitionStatsWindow _competitionStats;
        private RaceStatsWindow _raceStats;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
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

        private void MenuItem_OpenCompetitionStatsWindow_Click(object sender, RoutedEventArgs e)
        {
            _competitionStats = new CompetitionStatsWindow();
            _competitionStats.Show();
        }

        private void MenuItem_OpenRaceStatsWindow_Click(object sender, RoutedEventArgs e)
        {
            _raceStats = new RaceStatsWindow();
            _raceStats.Show();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
