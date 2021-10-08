using System;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

        private static int _raceNumber;

        public static void Initialize()
        {
            Competition = new Competition();
            _raceNumber = 1;
            AddParticipants();
            AddTracks();
            AddParticipantsAndLaps();
        }

        public static void AddParticipants()
        {
            int speed = 20;
            int performance = 0;
            var d1 = new Driver("Michael", new Car(speed, performance), TeamColors.Blue);
            var d2 = new Driver("Sebastian", new Car(speed, performance), TeamColors.Green);
            var d3 = new Driver("Lewis", new Car(speed, performance), TeamColors.Red);
            var d4 = new Driver("Thomas", new Car(speed, performance), TeamColors.Grey);
            var d5 = new Driver("Albert", new Car(speed, performance), TeamColors.Yellow);
            var d6 = new Driver("Will", new Car(speed, performance), TeamColors.Yellow);

            Competition.Participants.Add(d1);
            //Competition.Participants.Add(d2);
            //Competition.Participants.Add(d3);
            //Competition.Participants.Add(d4);
            //Competition.Participants.Add(d5);
            //Competition.Participants.Add(d6);
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
            Competition.Tracks.Enqueue(amsterdam);
            Competition.Tracks.Enqueue(zwolle);
        }

        public static void AddParticipantsAndLaps()
        {
            for (int j = 1; j <= Competition.Tracks.Count; ++j)
            {
                foreach (var participant in Competition.Participants)
                {
                    Competition.RaceTimes.Add((participant, j), new TimeSpan());
                }
            }
        }

        public static void NextRace()
        {
            CurrentRace?.Dispose();

            var tempTrack = Competition.NextTrack();

            if (tempTrack != null)
            {
                CurrentRace = new Race(tempTrack, Competition.Participants, Competition.RaceTimes, _raceNumber++);
                CurrentRace.RaceFinished += OnRaceFinished;
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs(CurrentRace));
                CurrentRace.StartTimer();
            }
            else
            {
                CurrentRace = null;
                //PrintRaceTimes();
            }
        }

        private static void PrintRaceTimes()
        {
            Console.ResetColor();
            Console.Clear();
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
            for (int i = 1; i < _raceNumber; ++i)
            {
                Console.WriteLine($"Race {i} times\n");
                foreach (var participant in Competition.Participants)
                {
                    Console.WriteLine($"{participant.Name}: {Competition.RaceTimes[(participant, i)]}");
                    
                }
                Console.WriteLine();
            }
        }

        private static void OnRaceFinished(object sender, EventArgs e)
        {
            NextRace();
        }
    }
}
