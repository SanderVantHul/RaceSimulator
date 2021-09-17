using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Model;

namespace RaceSimulator
{
    public static class VisualizeConsole
    {
        private enum Direction
        {
            North,
            East,
            South,
            West
        }

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

        private static Vector2 _cursorPosition;
        private static Direction _currentDirection = Direction.North;

        public static void Initialize()
        {
            _cursorPosition.X = Console.CursorTop;
            _cursorPosition.Y = Console.CursorLeft;
        }

        public static void DrawTrack(Track track)
        {
            Console.Clear();
            Console.WriteLine(track.Name);
            _cursorPosition.Y += 4;
            _cursorPosition.X += 12;
            foreach (var section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Finish:
                        switch (_currentDirection)
                        {
                            case Direction.East:
                                PrintTrack(_finishHorizontal);
                                UpdateCursorPosition();
                                break;
                            case Direction.West:
                                PrintTrack(_finishHorizontal);
                                UpdateCursorPosition();
                                break;
                        }
                        break;
                    case SectionTypes.LeftCorner:
                        switch (_currentDirection)
                        {
                            case Direction.North:
                                PrintTrack(_corner2);
                                _currentDirection = Direction.West;
                                UpdateCursorPosition();
                                break;
                            case Direction.East:
                                PrintTrack(_corner3);
                                _currentDirection = Direction.North;
                                UpdateCursorPosition();
                                break;
                            case Direction.South:
                                PrintTrack(_corner4);
                                _currentDirection = Direction.East;
                                UpdateCursorPosition();
                                break;
                            case Direction.West:
                                PrintTrack(_corner1);
                                _currentDirection = Direction.South;
                                UpdateCursorPosition();
                                break;
                        }
                        break;
                    case SectionTypes.RightCorner:
                        switch (_currentDirection)
                        {
                            case Direction.North:
                                PrintTrack(_corner1);
                                _currentDirection = Direction.East;
                                UpdateCursorPosition();
                                break;
                            case Direction.East:
                                PrintTrack(_corner2);
                                _currentDirection = Direction.South;
                                UpdateCursorPosition();
                                break;
                            case Direction.South:
                                PrintTrack(_corner3);
                                _currentDirection = Direction.West;
                                UpdateCursorPosition();
                                break;
                            case Direction.West:
                                PrintTrack(_corner4);
                                _currentDirection = Direction.North;
                                UpdateCursorPosition();
                                break;
                        }
                        break;
                    case SectionTypes.StartGrid:
                        switch (_currentDirection)
                        {
                            case Direction.East:
                                PrintTrack(_startGridHorizontal);
                                UpdateCursorPosition();
                                break;
                            case Direction.West:
                                PrintTrack(_startGridHorizontal);
                                UpdateCursorPosition();
                                break;
                        }
                        break;
                    case SectionTypes.Straight:
                        switch (_currentDirection)
                        {
                            case Direction.South:
                            case Direction.North:
                                PrintTrack(_straightVertical);
                                UpdateCursorPosition();
                                break;
                            case Direction.West:
                            case Direction.East:
                                PrintTrack(_straightHorizontal);
                                UpdateCursorPosition();
                                break;
                        }
                        break;
                }
            }
        }

        private static void UpdateCursorPosition()
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    _cursorPosition.X -= 0;
                    _cursorPosition.Y -= 8;
                    break;
                case Direction.East:
                    _cursorPosition.X += 4;
                    _cursorPosition.Y -= 4;
                    break;
                case Direction.South:
                    _cursorPosition.X += 0;
                    _cursorPosition.Y += 0;
                    break;
                case Direction.West:
                    _cursorPosition.X -= 4;
                    _cursorPosition.Y -= 4;
                    break;
            }
        }

        //print elke regel in de string array onder elkaar
        private static void PrintTrack(string[] strings)
        {
            for (int i = 0; i < strings.Length; ++i)
            {
                WriteAt(strings[i], (int)_cursorPosition.X, (int)_cursorPosition.Y++);
            }
        }

        private static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}