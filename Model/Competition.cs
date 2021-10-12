using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public Dictionary<IParticipant, TimeSpan> RaceTimes;

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            RaceTimes = new Dictionary<IParticipant, TimeSpan>();
        }

        public Track NextTrack() => Tracks.Any() ? Tracks.Dequeue() : null;
    }
}
