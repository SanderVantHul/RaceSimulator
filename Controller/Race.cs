using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = new DateTime();
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            SetStartPositions(track, participants);
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
                participant.Equipment.Performance = _random.Next();
                participant.Equipment.Quality = _random.Next();
            }
        }

        public void PrintParticipants()
        {
            var list = new List<Section>();
            foreach (var section in Track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    list.Add(section);
                }
            }

            foreach (var section in list)
            {
                try
                {
                    Console.WriteLine(_positions[section]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Key niet in dictionary");
                }
                
            }
        }

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
            startGridSections.TrimExcess();

            int participantsIndex = participants.Count - 1; 
            for (int i = 0; participantsIndex >= 0 && !startGridSections.Capacity.Equals(i); ++i)
            {
                var temp = new SectionData(null, null);
                if (participantsIndex == 0)
                {
                    _positions.Add(startGridSections[i], new SectionData(participants[participantsIndex--],
                        null));
                }
                else
                {
                    _positions.Add(startGridSections[i], new SectionData(participants[participantsIndex--],
                        participants[participantsIndex--]));
                }
            }
            
        }
    }
}