using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Model;

namespace Controller
{
    public class CompetitionDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Driver> Participants => Data.Competition.Participants.Select(x => (Driver)x).ToList();

        public CompetitionDataContext()
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.NextRaceEvent += OnNextRace;
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }


    }
}