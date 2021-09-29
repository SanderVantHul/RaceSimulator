using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public Dictionary<(IParticipant, int), TimeSpan> RaceTimes;

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            RaceTimes = new Dictionary<(IParticipant, int), TimeSpan>();
        }

        public Track NextTrack() => Tracks.Any() ? Tracks.Dequeue() : null;
    }
}
