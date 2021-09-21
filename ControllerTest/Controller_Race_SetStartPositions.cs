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
            _race = new Race(null,null);
        }

        [Test]
        public void SetStartPositions()
        {
            var temp = new List<IParticipant>();
            temp.Add(new Driver("Harry", new Car(), TeamColors.Blue));
            temp.Add(new Driver("Paul", new Car(), TeamColors.Red));
            temp.Add(new Driver("Max", new Car(), TeamColors.Green));

        }
        
    }
}
