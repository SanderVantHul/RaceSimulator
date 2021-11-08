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
        private RaceStats _raceStats;
        private CompetitionStats _competitionStats;

        public MainWindow()
        {
            WindowState = WindowState.Maximized;
            Data.Initialize();
            Data.NextRaceEvent += OnNextRace;
            Data.NextRace();
            InitializeComponent();
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

            Data.CurrentRace.StartTimer();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_ShowRaceStatsWindow_Click(object sender, RoutedEventArgs e)
        {
            _raceStats = new RaceStats();
            _raceStats.Show();
        }

        private void MenuItem_ShowCompetitionStats_Click(object sender, RoutedEventArgs e)
        {
            _competitionStats = new CompetitionStats();
            _competitionStats.Show();
        }
    }
}
