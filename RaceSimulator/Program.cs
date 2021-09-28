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
            Data.NextRaceEvent += VisualizeConsole.OnNextRace;
            Data.NextRace();

            //VisualizeConsole.Initialize(Data.CurrentRace);
            //VisualizeConsole.DrawTrack(Data.CurrentRace.Track);

            for (;;)
            {
                Thread.Sleep(100);
            }
        }
    }
}