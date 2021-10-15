using System;
using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_SetStartPositions
    {
        private Race _race;

        [SetUp]
        public void Setup()
        {
            Data.Initialize();
            Data.NextRace();
            _race = new Race(Data.CurrentRace.Track, Data.Competition.Participants,
                Data.Competition.RaceTimes);
        }

        [Test]
        public void Dictionary_notEmpty()
        {
            Assert.NotNull(_race.RaceTimes.Keys);
        }

        [Test]
        public void SetStartPositions_CheckParticipants()
        {
            var numberOfParticipants = Data.Competition.Participants.Count;
            var startGrids = new List<Section>();
            foreach (var trackSection in _race.Track.Sections)
            {
                if (trackSection.SectionType == SectionTypes.StartGrid)
                {
                    startGrids.Add(trackSection);
                }
            }

            startGrids.Reverse();
            foreach (var trackSection in startGrids)
            {
                var sectionData = _race.GetSectionData(trackSection);

                if (numberOfParticipants == 1)
                {
                    Assert.IsNotNull(sectionData.Left);
                    Assert.IsNull(sectionData.Right);
                }
                else if (numberOfParticipants <= 0)
                {
                    Assert.IsNull(sectionData.Left);
                    Assert.IsNull(sectionData.Right);
                }
                else
                {
                    Assert.IsNotNull(sectionData.Left);
                    Assert.IsNotNull(sectionData.Right);
                }

                numberOfParticipants -= 2;
            }
        }
    }
}