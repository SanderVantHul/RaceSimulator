using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; }
        public List<IParticipant> Participants { get; set; }
        public Dictionary<IParticipant, TimeSpan> RaceTimes;
        public DateTime StartTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        private int _numberOfLaps;
        private int _points;

        private const int TimerInterval = 200;
        private const int SectionLength = 100;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler RaceFinished;

        public Race(Track track, List<IParticipant> participants, Dictionary<IParticipant, TimeSpan> raceTimes)
        {
            Track = track;
            Participants = participants;
            RaceTimes = raceTimes;
            StartTime = new DateTime();
            _numberOfLaps = track.Sections.Count >= 15 ? 2 :
                track.Sections.Count >= 10 ? 3 :
                track.Sections.Count >= 5 ? 4 : 5;

            //_numberOfLaps = 2; //testing purposes 

            _points = track.Sections.Count(section => section.SectionType == SectionTypes.StartGrid) * 2;

            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            SetStartPositions(track, participants);
            RandomizeEquipment();

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //breekt de equipment randomly
            UpdateEquipment();

            //neem de afstand toe voor de participants die de section hebben uitgereden
            MoveParticipants();

            //zet de participants op de volgende section
            UpdateSectionData(e.SignalTime);

            UpdateTimes(e.SignalTime);

            //update visualisatie
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));

            //kijk of de race moet stoppen
            CheckRaceFinished();
        }

        public void UpdateTimes(DateTime elapsedTime)
        {
            foreach (var participant in Participants)
            {
                if (_positions.Values.Any(x => x.Left == participant || x.Right == participant))
                {
                    RaceTimes[participant] = elapsedTime.Subtract(participant.StartTime);
                }
            }
        }

        private void UpdateEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.IsBroken = _random.Next(-15, 9) > participant.Equipment.Quality &&
                                                 participant.Equipment.Speed > 5 &&
                                                 participant.Equipment.Performance > 3;

                if (participant.Equipment.IsBroken)
                {
                    participant.Equipment.Speed--;
                }
            }
        }

        private void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.Performance = _random.Next(4, 6);
                participant.Equipment.Quality = _random.Next(6, 20 - participant.Equipment.Performance * 2);
            }
        }

        public void CheckRaceFinished()
        {
            if (_positions.Values.All(p => p.Left == null && p.Right == null))
            {
                RaceFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        public SectionData GetSectionData(Section section)
        {
            var temp = new SectionData(null, null);
            if (!_positions.TryAdd(section, temp))
            {
                return _positions[section];
            }
            return temp;
        }

        //update de sectiondata van elk startpunt 
        private void SetStartPositions(Track track, List<IParticipant> participants)
        {
            var startGridSections =
                track.Sections.Where(section => section.SectionType == SectionTypes.StartGrid).ToList();

            //draai de volgorde om zodat de participants op de voorste startsection worden geplaats
            startGridSections.Reverse();

            //haal de excess ruimte van de list weg zodat de capacity van de list overeenkomt met het aantal startgrids
            startGridSections.TrimExcess();

            int participantsIndex = participants.Count - 1;
            for (int i = 0; participantsIndex >= 0 && !startGridSections.Capacity.Equals(i); ++i)
            {
                if (participantsIndex == 0)
                {
                    _positions.Add(startGridSections[i], new SectionData(participants[participantsIndex--], null));
                }

                else
                {
                    _positions.Add(startGridSections[i], new SectionData(participants[participantsIndex--],
                        participants[participantsIndex--]));
                }
            }
        }

        private void CheckIfEnoughLaps(Section section, IParticipant participant, DateTime elapsedTime)
        {
            if (participant.NumberOfLaps >= _numberOfLaps)
            {
                if (participant == GetSectionData(section).Left)
                {
                    GetSectionData(section).Left = null;
                    participant.Points += _points--;
                }
                else if (participant == GetSectionData(section).Right)
                {
                    GetSectionData(section).Right = null;
                    participant.Points += _points--;
                }
            }
        }

        private void UpdateLaps(Section section, IParticipant participant, DateTime elapsedTime)
        {
            if (section.SectionType == SectionTypes.Finish)
            {
                if (GetSectionData(section).Left == participant)
                {
                    participant.NumberOfLaps += 1;
                    if (participant.NumberOfLaps == 0)
                    {
                        participant.StartTime = elapsedTime;
                    }
                    CheckIfEnoughLaps(section, GetSectionData(section).Left, elapsedTime);
                }
                else if (GetSectionData(section).Right == participant)
                {
                    participant.NumberOfLaps += 1;
                    if (participant.NumberOfLaps == 0)
                    {
                        participant.StartTime = elapsedTime;
                    }
                    CheckIfEnoughLaps(section, GetSectionData(section).Right, elapsedTime);
                }
            }
        }

        public void UpdateSectionData(DateTime elapsedTime)
        {
            bool CheckDistance(int distance) => distance > SectionLength;

            foreach (var position in _positions)
            {
                if (CheckDistance(position.Value.DistanceLeft))
                {
                    CheckIfEligibleForNextSection(position.Key, true, elapsedTime);
                }

                if (CheckDistance(position.Value.DistanceRight))
                {
                    CheckIfEligibleForNextSection(position.Key, false, elapsedTime);
                }
            }
        }

        public void CheckIfEligibleForNextSection(Section section, bool leftParticipant, DateTime elapsedTime)
        {
            //emptySection kijkt of de volgende section links of rechts leeg is, in het geval dat de linkerkant leeg is wordt
            //emptySection true, in het geval dat de rechterkant leeg is wordt emptySection false. In het geval dat beide
            //vol zijn wordt emptySection null en dan moet de afstand die bij MoveParticipants weer verwijdert worden want dan staat
            //de driver stil.
            bool? emptySection = (GetSectionData(GetNextSection(section)).Left == null) ? true :
                (GetSectionData(GetNextSection(section)).Right == null) ? false : (bool?)null;

            if (leftParticipant)
            {
                if (emptySection != null)
                {
                    RemoveFromSection(section, (bool)emptySection, GetSectionData(section).Left,
                        GetSectionData(section).DistanceLeft, elapsedTime);
                }
                else
                {
                    GetSectionData(section).DistanceLeft -= CalculateNewPosition(GetSectionData(section).Left);
                }
            }
            else
            {
                if (emptySection != null)
                {
                    RemoveFromSection(section, (bool)emptySection, GetSectionData(section).Right,
                        GetSectionData(section).DistanceRight, elapsedTime);
                }
                else
                {
                    GetSectionData(section).DistanceRight -= CalculateNewPosition(GetSectionData(section).Right);
                }
            }
        }

        public void RemoveFromSection(Section section, bool sectionDataLeft, IParticipant participant, int distance, DateTime elapsedTime)
        {
            //als de participant van de meegegeven section gelijk is aan de meegegeven participant dan betekent het dat de participant 
            //van de rechterkant komt en dan moet de oude SectionData van rechts verwijdert worden. Als de participant niet gelijk is,
            //dan komt de participant van links en dus moet de oude SectionData van links verwijdert worden.
            if (GetSectionData(section).Right == participant)
            {
                GetSectionData(section).DistanceRight = 0;
                GetSectionData(section).Right = null;
            }
            else
            {
                GetSectionData(section).DistanceLeft = 0;
                GetSectionData(section).Left = null;
            }

            AddToSection(GetNextSection(section), sectionDataLeft, participant, distance, elapsedTime);
        }

        public void AddToSection(Section section, bool sectionDataLeft, IParticipant participant, int distance,
            DateTime elapsedTime)
        {
            //de bool die mee wordt gegeven geeft aan, aan welke kant de participant komt; links of rechts.
            if (sectionDataLeft)
            {
                GetSectionData(section).Left = participant;
                GetSectionData(section).DistanceLeft = distance - SectionLength;
            }
            else
            {
                GetSectionData(section).Right = participant;
                GetSectionData(section).DistanceRight = distance - SectionLength;
            }

            UpdateLaps(section, participant, elapsedTime);
        }

        public void MoveParticipants()
        {
            foreach (var section in Track.Sections)
            {
                if (GetSectionData(section).Left != null && !GetSectionData(section).Left.Equipment.IsBroken)
                {
                    GetSectionData(section).DistanceLeft += CalculateNewPosition(GetSectionData(section).Left);
                }

                if (GetSectionData(section).Right != null && !GetSectionData(section).Right.Equipment.IsBroken)
                {
                    GetSectionData(section).DistanceRight += CalculateNewPosition(GetSectionData(section).Right);
                }
            }
        }

        public void Dispose()
        {
            DriversChanged = null;
            _timer.Stop();
            _timer.Dispose();
            RaceFinished = null;
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        //return de volgende sectie in de linkedlist. Als die niet bestaat dan betekent het dat de driver momenteel op de laatste
        //sectie staat, dus return dan de eerste sectie van de linkedlist.
        public Section GetNextSection(Section thisSection) =>
            Track.Sections.Find(thisSection)?.Next?.Value ?? Track.Sections.First?.Value;

        private int CalculateNewPosition(IParticipant participant) =>
            participant.Equipment.Speed * participant.Equipment.Performance;
    }
}