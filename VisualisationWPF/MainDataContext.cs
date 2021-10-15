using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Controller;
using Model;

namespace VisualisationWPF
{
    public class MainDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackName => Data.CurrentRace.Track.Name;

        //maak een list van alle participants
        public List<Driver> Participants => Data.Competition.Participants.Select(x => (Driver)x).ToList();

        public Dictionary<IParticipant, TimeSpan> RaceTimes =>
            Data.Competition.RaceTimes.OrderBy(x => x.Key.Points).ThenBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        //public List<IParticipant> ParticipantPoints =>
        //    Data.Competition.Participants.OrderByDescending(x => x.Points).ToList();

        public MainDataContext()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriversChanged;
            }
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}