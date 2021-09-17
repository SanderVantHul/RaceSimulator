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
        private static string[] _leftCorner = { @"---\", "   |", "   |", @"\   |" };
        private static string[] _rightCorner = { @"/   |", "   |", "   |", @"---/" };
        private static string[] _leftCornerMirrored = { @"/---", "   |", "   |", @"|   /" };
        private static string[] _rightCornerMirrored = { @"|   \", "   |", "   |", @"\---" };

        #endregion

        public static void Initialize()
        {
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Finish:
                        PrintTrack(_finishHorizontal);
                        break;
                    case SectionTypes.LeftCorner:
                        PrintTrack(_leftCorner);
                        break;
                    case SectionTypes.RightCorner:
                        PrintTrack(_rightCorner);
                        break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startGridHorizontal);
                        break;
                    case SectionTypes.Straight:
                        PrintTrack(_straightHorizontal);
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