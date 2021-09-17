using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace RaceSimulator
{
    public static class Visualize
    {
        #region graphics

        private static string[] _finishHorizontal = { "----", "  # ", "  # ", "----" };
        private static string[] _startGridHorizontal = { "----", "  ] ", "  ] ", "----" };
        private static string[] _straightHorizontal = { "----", "    ", "    ", "----" };
        private static string[] _straightVertical = { "|  |", "|  |", "|  |", "|  |" };
        
        private static string[] _corner1 = { @"/---", "|   ", "|   ", @"|  /" };
        private static string[] _corner2 = { @"---\", "   |", "   |", @"\  |" };
        private static string[] _corner3 = { @"/  |", "   |", "   |", @"---/" };
        private static string[] _corner4 = { @"|  \", "|   ", "|   ", @"\---" };
        #endregion
        private static int _cursorRow;
        private static int _cursorCol;

        private enum direction
        {
            North,
            East,
            South,
            West
        }

        private static direction currentDirection = direction.North;

        public static void Initialize()
        {
            _cursorRow = Console.CursorTop;
            _cursorCol = Console.CursorLeft;
        }

        public static void DrawTrack(Track track)
        {
            Console.Clear();
            foreach (var section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Finish:
                        PrintTrack(_finishHorizontal);
                        break;
                    case SectionTypes.LeftCorner:
                        switch (currentDirection)
                        {
                            case direction.North:
                                PrintTrack(_corner2);
                                currentDirection = direction.West;
                                break;
                            case direction.East:
                                PrintTrack(_corner3);
                                currentDirection = direction.North;
                                break;
                            case direction.South:
                                PrintTrack(_corner4);
                                currentDirection = direction.East;
                                break;
                            case direction.West:
                                PrintTrack(_corner1);
                                currentDirection = direction.South;
                                break;
                        }
                        break;
                    case SectionTypes.RightCorner:
                        switch (currentDirection)
                        {
                            case direction.North:
                                PrintTrack(_corner1);
                                currentDirection = direction.East;
                                break;
                            case direction.East:
                                PrintTrack(_corner2);
                                currentDirection = direction.South;
                                break;
                            case direction.South:
                                PrintTrack(_corner3);
                                currentDirection = direction.West;
                                break;
                            case direction.West:
                                PrintTrack(_corner4);
                                currentDirection = direction.North;
                                break;
                        }
                        break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startGridHorizontal);
                        break;
                    case SectionTypes.Straight:
                        switch (currentDirection)
                        {
                            case direction.South:
                            case direction.North:
                                PrintTrack(_straightVertical);
                                break;
                            case direction.West:
                            case direction.East:
                                PrintTrack(_straightHorizontal);
                                break;
                        }
                        break;
                }
            }
        }

        private static void PrintTrack(string[] strings)
        {
            for (int i = 0; i < strings.Length; ++i)
            {
                Console.WriteLine(strings[i]);
            }
        }

    }
}