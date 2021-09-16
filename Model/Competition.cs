using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }

        public Track NextTrack()
        {
            try
            {
                return Tracks.Peek(); //todo remove track
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
