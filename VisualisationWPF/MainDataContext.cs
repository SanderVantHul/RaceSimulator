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

        public List<Car> Cars => Data.Competition.Participants.Select(x => (Car)x.Equipment).ToList();   
        
        public MainDataContext()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriversChanged;
            }
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}