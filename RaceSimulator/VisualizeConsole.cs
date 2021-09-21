using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static string[] _finishNorth = { "|  |", "|##|", "|  |", "|  |" };
        private static string[] _finishEast = { "----", "  # ", "  # ", "----" };
        private static string[] _finishSouth = { "|  |", "|  |", "|##|", "|  |" };
        private static string[] _finishWest = { "----", " #  ", " #  ", "----" };

        private static string[] _startGridNorth = { "|  |", "|^^|", "|  |", "|  |" };
        private static string[] _startGridEast = { "----", "  > ", "  > ", "----" };
        private static string[] _startGridSouth = { "|  |", "|  |", "|vv|", "|  |" };
        private static string[] _startGridWest = { "----", " <  ", " <  ", "----" };

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
            ResetConsole(track);

            foreach (var section in track.Sections)
            {
                PrintTrack(GetStrings(section.SectionType));
                UpdateDirection(section.SectionType);
                UpdateCursorPosition();
            }
        }

        private static void ResetConsole(Track track)
        {
            Console.Clear();
            Console.WriteLine(track.Name);
            _cursorPosition.Y += 4;
            _cursorPosition.X += 12;
        }

        private static string[] GetStrings(SectionTypes sectionType) => sectionType switch
        {
            SectionTypes.Straight => _currentDirection switch
            {
                Direction.North => _straightVertical,
                Direction.East => _straightHorizontal,
                Direction.South => _straightVertical,
                Direction.West => _straightHorizontal,
            },
            SectionTypes.LeftCorner => _currentDirection switch
            {
                Direction.North => _corner2,
                Direction.East => _corner3,
                Direction.South => _corner4,
                Direction.West => _corner1,
            },
            SectionTypes.RightCorner => _currentDirection switch
            {
                Direction.North => _corner1,
                Direction.East => _corner2,
                Direction.South => _corner3,
                Direction.West => _corner4,
            },
            SectionTypes.StartGrid => _currentDirection switch
            {
                Direction.North => _startGridNorth,
                Direction.East => _startGridEast,
                Direction.South => _startGridSouth,
                Direction.West => _startGridWest,
            },
            SectionTypes.Finish => _currentDirection switch
            {
                Direction.North => _finishNorth, 
                Direction.East => _finishEast,
                Direction.South => _finishSouth,
                Direction.West => _finishWest,
            },
        };

        private static void UpdateCursorPosition()
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    _cursorPosition.Y -= 8;
                    break;
                case Direction.East:
                    _cursorPosition.X += 4;
                    _cursorPosition.Y -= 4;
                    break;
                case Direction.West:
                    _cursorPosition.X -= 4;
                    _cursorPosition.Y -= 4;
                    break;
            }
        }

        //Verandert de richting 
        private static void UpdateDirection(SectionTypes sectionType) 
        {
            switch (sectionType)
            {
                case SectionTypes.RightCorner:
                    _currentDirection = (int)_currentDirection >= 3 ? Direction.North : ++_currentDirection;
                    break;
                case SectionTypes.LeftCorner:
                    _currentDirection = (int)_currentDirection <= 0 ? Direction.West : --_currentDirection;
                    break;
            }
        }

        //print elke regel in de string array onder elkaar
        private static void PrintTrack(string[] strings)
        {
            for (int i = 0; i < strings.Length; ++i)
            {
                WriteAt(strings[i], (int)_cursorPosition.X, (int)++_cursorPosition.Y);
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
        /*
        private static void GetStrings(Track track)
        {
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
        }*/
    }

}