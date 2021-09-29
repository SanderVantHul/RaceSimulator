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
    public class Controller_Race_UpdateSectionData
    {
        private Race _race;
        
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void UpdateSectionData_NoSectionAvailable()
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
                new Driver("e", new Car(0, 0), TeamColors.Blue),
                new Driver("f", new Car(0, 0), TeamColors.Blue)
            };
            _race = new Race(elburg, list, new Dictionary<(IParticipant, int), TimeSpan>(), 0);

            foreach (var section in _race.Track.Sections)
            {
                if (_race.GetSectionData(section).Left?.Name == "e")
                {
                    Assert.IsNull(_race.GetSectionData(_race.GetNextSection(section)));
                }
            }
        }
    }
}