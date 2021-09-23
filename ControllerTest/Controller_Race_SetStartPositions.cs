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
            var NumberOfParticipants = Data.Competition.Participants.Count;
            foreach (var trackSection in _race.Track.Sections)
            {
                var sectiondata = _race.GetSectionData(trackSection);

                if (trackSection.SectionType == SectionTypes.StartGrid)
                {
                    if (NumberOfParticipants == 1)
                    {
                        Assert.IsNotNull(sectiondata.Left);
                        Assert.IsNull(sectiondata.Right);
                    }
                    else if(NumberOfParticipants <= 0)
                    {
                        Assert.IsNull(sectiondata.Left);
                        Assert.IsNull(sectiondata.Right);
                    }
                    else
                    {
                        Assert.IsNotNull(sectiondata.Left);
                        Assert.IsNotNull(sectiondata.Right);
                    }

                    NumberOfParticipants -= 2;
                }
            }
        }

    }
}
