using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        public Dictionary<Section, SectionData> _positions;
        private Timer _timer;

        private const int TimerInterval = 200;
        private const int TrackLength = 100;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = new DateTime();
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            SetStartPositions(track, participants);
            RandomizeEquipment();

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTimedEvent;

            StartTimer();
        }

        public SectionData GetSectionData(Section section)
        {
            if (_positions.ContainsKey(section))
            {
                return _positions[section];
            }

            var temp = new SectionData(null, null);
            _positions.Add(section, temp);
            return temp;
        }

        private void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.Performance = _random.Next(1, 6);
                participant.Equipment.Quality = _random.Next();
            }
        }

        //update de sectiondata van elk startpunt 
        private void SetStartPositions(Track track, List<IParticipant> participants)
        {
            var startGridSections = new List<Section>();
            foreach (var section in track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    startGridSections.Add(section);
                }
            }

            startGridSections.Reverse();
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

        private void OnTimedEvent(object source, ElapsedEventArgs args)
        {
            MoveParticipants();
            UpdateSectionData();
        }

        private void UpdateSectionData()
        {
            bool CheckDistance(int distance) => distance > TrackLength;

            foreach (var position in _positions)
            {
                if (CheckDistance(position.Value.DistanceLeft))
                {
                    CheckIfEligibleForNextSection(position.Key, true);
                }

                if (CheckDistance(position.Value.DistanceRight))
                {
                    CheckIfEligibleForNextSection(position.Key, false);
                }
            }
        }

        private void CheckIfEligibleForNextSection(Section section, bool leftParticipant)
        {
            //emptySection kijkt of de volgende section links of rechts leeg is, in het geval dat de linkerkant leeg is wordt
            //emptySection true, in het geval dat de rechterkant leeg is wordt emptySection false. In het geval dat beide
            //vol zijn wordt emptySection null en dan moet de afstand die bij MoveParticipants weer verwijdert worden want dan staat
            //de driver stil.
            bool? emptySection = (_positions[GetNextSection(section)].Left == null) ? true :
                (_positions[GetNextSection(section)].Right == null) ? false : (bool?)null;

            if (leftParticipant)
            {
                if (emptySection != null)
                    RemoveFromSection(section, (bool)emptySection, _positions[section].Left, _positions[section].DistanceLeft);
                else
                    _positions[section].DistanceLeft -= CalculateNewPosition(_positions[section].Left);
            }
            else
            {
                if(emptySection != null)
                    RemoveFromSection(section, (bool)emptySection, _positions[section].Right, _positions[section].DistanceRight);
                else
                    _positions[section].DistanceRight -= CalculateNewPosition(_positions[section].Right);
            }

            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
        }

        private void RemoveFromSection(Section section, bool sectionDataLeft, IParticipant participant, int distance)
        {
            //als de participant van de meegegeven section gelijk is aan de meegegeven participant dan betekent het dat de participant 
            //van de rechterkant komt en dan moet de oude SectionData van rechts verwijdert worden. Als de participant niet gelijk is,
            //dan komt de participant van links en dus moet de oude SectionData van links verwijdert worden.
            if (_positions[section].Right == participant)
            {
                _positions[section].DistanceRight = 0;
                _positions[section].Right = null;
            }
            else
            {
                _positions[section].DistanceLeft = 0;
                _positions[section].Left = null;
            }

            //de bool die mee wordt gegeven geeft aan, aan welke kant de participant komt; links of rechts.
            if (sectionDataLeft)
            {
                _positions[GetNextSection(section)].Left = participant;
                _positions[GetNextSection(section)].DistanceLeft = distance - TrackLength;
            }
            else
            {
                _positions[GetNextSection(section)].Right = participant;
                _positions[GetNextSection(section)].DistanceRight = distance - TrackLength;
            }
        }

        private void MoveParticipants()
        {
            foreach (var section in Track.Sections)
            {
                if (GetSectionData(section).Left != null)
                {
                    GetSectionData(section).DistanceLeft += CalculateNewPosition(GetSectionData(section).Left);
                }

                if (GetSectionData(section).Right != null)
                {
                    GetSectionData(section).DistanceRight += CalculateNewPosition(GetSectionData(section).Right);
                }
            }
        }

        private void StartTimer() { _timer.Start(); }

        public Section GetNextSection(Section thisSection) =>
            Track.Sections.Find(thisSection).Next?.Value ?? Track.Sections.First.Value;

        private int CalculateNewPosition(IParticipant participant) =>
            participant.Equipment.Speed * participant.Equipment.Performance;
    }
}