using System;
using System.Collections.Generic;
using NUnit.Framework;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Race_RemoveFromSectionTests
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
        public void RemoveFromSection_LeftParticipant_SectionDataLeftEmpty()
        {
            var section = _race.Track.Sections.First?.Value;
            var participant = _race.Participants[0];

            _race.GetSectionData(section).Left = participant;
            _race.RemoveFromSection(section, true, participant, 0, new DateTime());

            Assert.That(_race.GetSectionData(section).DistanceLeft == 0);
            Assert.That(_race.GetSectionData(section).Left == null);
        }

        [Test]
        public void RemoveFromSection_RightParticipant_SectionDataRightEmpty()
        {
            var section = _race.Track.Sections.First?.Value;
            var participant = _race.Participants[0];

            _race.GetSectionData(section).Right = participant;
            _race.RemoveFromSection(section, true, participant, 0, new DateTime());

            Assert.That(_race.GetSectionData(section).DistanceLeft == 0);
            Assert.That(_race.GetSectionData(section).Right == null);
        }
    }
}