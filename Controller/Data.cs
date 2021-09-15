using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    static class Data
    {
        static Competition Competition { get; set; }

        static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        static void AddParticipants()
        {
            var d1 = new Driver("Michael", new Car(), TeamColors.Blue);
            var d2 = new Driver("Sebastian", new Car(), TeamColors.Green);
            var d3 = new Driver("Lewis", new Car(), TeamColors.Red);

            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
        }

        static void AddTracks()
        {
            var track = new Track("Circuit de Monaco", new SectionTypes[] 
                {SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, 
                    SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner});

            Competition.Tracks.Enqueue(track);
        }
    }
}
