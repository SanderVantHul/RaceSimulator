using System;
using System.Collections.Generic;
using NUnit.Framework;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Race_GetSectionDataTests
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
                new Driver("e", new Car(0, 0), TeamColors.Blue),
                new Driver("f", new Car(0, 0), TeamColors.Blue)
            };
            _race = new Race(elburg, list, new Dictionary<(IParticipant, int), TimeSpan>(), 0);
        }

        [Test]
        public void GetSectionData_SectionDoesNotExist_OfTypeSectionData()
        {
            var section = new Section(SectionTypes.RightCorner);

            var result = _race.GetSectionData(section);

            Assert.IsInstanceOf<SectionData>(result);
        }

        [Test]
        public void GetSectionData_SectionDoesNotExist_ReturnEmptySectionData()
        {
            var section = new Section(SectionTypes.RightCorner);

            var result = _race.GetSectionData(section);

            Assert.That(result.DistanceLeft == 0);
            Assert.That(result.DistanceRight == 0);
            Assert.That(result.Right == null);
            Assert.That(result.Left == null);
        }

        [Test]
        public void GetSectionData_SectionDoesExist_ReturnSectionData()
        {
            var section = new Section(SectionTypes.Straight);
            var driver = new Driver("H", new Car(20, 1), TeamColors.Red);
            _race.GetSectionData(section).Left = driver;

            var result = _race.GetSectionData(section);

            Assert.That(result.DistanceLeft == 0);
            Assert.That(result.DistanceRight == 0);
            Assert.That(result.Right == null);
            Assert.That(result.Left == driver);
        }
    }
}