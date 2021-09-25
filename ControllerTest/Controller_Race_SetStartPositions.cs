using System;
using System.Collections.Generic;
using System.Text;
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
            _race = new Race(Data.CurrentRace.Track, Data.Competition.Participants);
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
                var sectiondata = _race.GetSectionData(trackSection);

                if (numberOfParticipants == 1)
                {
                    Assert.IsNotNull(sectiondata.Left);
                    Assert.IsNull(sectiondata.Right);
                }
                else if (numberOfParticipants <= 0)
                {
                    Assert.IsNull(sectiondata.Left);
                    Assert.IsNull(sectiondata.Right);
                }
                else
                {
                    Assert.IsNotNull(sectiondata.Left);
                    Assert.IsNotNull(sectiondata.Right);
                }

                numberOfParticipants -= 2;
            }
        }
    }
}