using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            int speed = 10;
            var d1 = new Driver("Michael", new Car(speed), TeamColors.Blue);
            var d2 = new Driver("Sebastian", new Car(speed), TeamColors.Green);
            var d3 = new Driver("Lewis", new Car(speed), TeamColors.Red);
            var d4 = new Driver("Thomas", new Car(speed), TeamColors.Grey);
            var d5 = new Driver("Albert", new Car(speed), TeamColors.Yellow);
            var d6 = new Driver("Will", new Car(speed), TeamColors.Yellow);

            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
            Competition.Participants.Add(d4);
            Competition.Participants.Add(d5);
            Competition.Participants.Add(d6);
        }

        public static void AddTracks()
        {
            #region Tracks
            Track zwolle = new Track("Circuit Zwolle", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight
            });

            Track elburg = new Track("Circuit Elburg", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid,
                SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner,
                SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight
            });

            Track amsterdam = new Track("Rondje Amsterdam", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish,
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            });
            #endregion

            Competition.Tracks.Enqueue(elburg);
            Competition.Tracks.Enqueue(zwolle);
            Competition.Tracks.Enqueue(amsterdam);
        }

        public static void NextRace()
        {
            var tempTrack = Competition.NextTrack();
            CurrentRace = tempTrack != null ? 
                new Race(tempTrack, Competition.Participants) : null;
        }
    }
}
