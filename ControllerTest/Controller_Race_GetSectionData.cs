using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_GetSectionData
    {
        private Race _race;
            
        [SetUp]
        public void Setup()
        {
            _race = new Race(null,null);
        }

        [Test]
        public void GetSectionData_EmptyDictionary_ReturnNewSectionData()
        {
            
        }
        
    }
}
