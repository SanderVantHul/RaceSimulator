using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Race_UpdateSectionDataTests
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
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

            var list = new List<IParticipant>
            {
                new Driver("m", new Car(10, 5), TeamColors.Blue),
            };
            _race = new Race(elburg, list, new Dictionary<(IParticipant, int), TimeSpan>(), 0);
        }

        [Test]
        public void UpdateSectionData_LeftParticipant_ParticipantMovedToNextSection()
        {
            var section = _race.Track.Sections.First.Value;
            var participant = _race.Participants[0];

            _race.GetSectionData(section).Left = participant;
            _race.GetSectionData(section).DistanceLeft = 100;

            _race.MoveParticipants();
            _race.UpdateSectionData(new DateTime());

            Assert.That(_race.GetSectionData(_race.GetNextSection(section)).DistanceLeft ==
                        participant.Equipment.Speed * participant.Equipment.Performance);
            Assert.That(_race.GetSectionData(_race.GetNextSection(section)).Left == participant);
            Assert.That(_race.GetSectionData(section).DistanceLeft == 0);
            Assert.That(_race.GetSectionData(section).Left == null);
        }

        [Test]
        public void UpdateSectionData_RightParticipant_ParticipantMovedToNextSection()
        {
            var section = _race.Track.Sections.First.Value;
            var participant = _race.Participants[0];

            _race.GetSectionData(section).Right = participant;
            _race.GetSectionData(section).DistanceRight = 100;

            _race.MoveParticipants();
            _race.UpdateSectionData(new DateTime());

            Assert.That(_race.GetSectionData(_race.GetNextSection(section)).DistanceLeft ==
                        participant.Equipment.Speed * participant.Equipment.Performance);
            Assert.That(_race.GetSectionData(_race.GetNextSection(section)).Left == participant);
            Assert.That(_race.GetSectionData(section).DistanceRight== 0);
            Assert.That(_race.GetSectionData(section).Right == null);
        }
    }
}