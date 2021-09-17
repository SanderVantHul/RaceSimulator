using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var temp = new Track("test", new SectionTypes[1]);
            _competition.Tracks.Enqueue(temp);
            var result = _competition.NextTrack();
            Assert.AreEqual(temp, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            var temp = new Track("test", new SectionTypes[1]);
            _competition.Tracks.Enqueue(temp);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            var temp1 = new Track("test1", new SectionTypes[1]);
            var temp2 = new Track("test2", new SectionTypes[1]);
            _competition.Tracks.Enqueue(temp1);
            _competition.Tracks.Enqueue(temp2);
            var result1 = _competition.NextTrack();
            var result2 = _competition.NextTrack();
            Assert.AreEqual(result1, temp1);
            Assert.AreEqual(result2, temp2);
        }
    }
}
