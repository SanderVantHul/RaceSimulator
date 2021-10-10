using System.Threading;
using Controller;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRaceEvent += VisualizeConsole.OnNextRace;
            Data.NextRace();

            for (;;)
            {
                Thread.Sleep(100);
            }
        }
    }
}