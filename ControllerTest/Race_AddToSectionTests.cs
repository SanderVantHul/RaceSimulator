using System;
using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Race_AddToSectionTests
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
            _race = new Race(elburg, list, new Dictionary<IParticipant, TimeSpan>());
        }

        [Test]
        public void AddToSection_ParticipantLeft_SectionDataLeftEqualParticipant()
        {
            var section = _race.Track.Sections.First.Value;
            var participant = _race.Participants[0];

            _race.AddToSection(section, true, participant, 120, new DateTime());

            Assert.That(_race.GetSectionData(section).DistanceLeft == 20);
            Assert.That(_race.GetSectionData(section).Left == participant);
        }

        [Test]
        public void AddToSection_ParticipantRight_SectionDataRightEqualParticipant()
        {
            var section = _race.Track.Sections.First.Value;
            var participant = _race.Participants[0];

            _race.AddToSection(section, false, participant, 120, new DateTime());

            Assert.That(_race.GetSectionData(section).DistanceRight == 20);
            Assert.That(_race.GetSectionData(section).Right == participant);
        }
    }
}