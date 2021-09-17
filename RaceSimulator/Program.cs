using System;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();
            Track zwolle = new Track("Circuit Zwolle", new SectionTypes[]
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner
            });
            Visualize.DrawTrack(zwolle);

            for (; ; )
            {
                Thread.Sleep(100);
            }
            /*
                Track zwolle = new Track("Circuit Zwolle", new Section.SectionTypes[] { SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid });
                Track elburg = new Track("Circuit Elburg", new Section.SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid });
                Track amsterdam = new Track("Rondje Amsterdam", new Section.SectionTypes[] { SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid });
             */
        }
    }
}
