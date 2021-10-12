using System;
using System.Numerics;
using Model;
using Controller;

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

        private static string[] _finishNorth = { "|  |", "|##|", "|12|", "|  |" };
        private static string[] _finishEast = { "----", " 1# ", " 2# ", "----" };
        private static string[] _finishSouth = { "|  |", "|12|", "|##|", "|  |" };
        private static string[] _finishWest = { "----", " #1 ", " #2 ", "----" };

        private static string[] _startGridNorth = { "|  |", "|^^|", "|12|", "|  |" };
        private static string[] _startGridEast = { "----", " 1> ", " 2> ", "----" };
        private static string[] _startGridSouth = { "|  |", "|12|", "|vv|", "|  |" };
        private static string[] _startGridWest = { "----", " <1 ", " <2 ", "----" };

        private static string[] _straightNorth = { "|  |", "|  |", "|12|", "|  |" };
        private static string[] _straightEast = { "----", "  1 ", "  2 ", "----" };
        private static string[] _straightSouth = { "|  |", "|21|", "|  |", "|  |" };
        private static string[] _straightWest = { "----", "  1 ", "  2 ", "----" };

        private static string[] _corner1 = { @"/---", "| 1 ", "|  2", @"|  /" };
        private static string[] _corner2 = { @"---\", " 1 |", "2  |", @"\  |" };
        private static string[] _corner3 = { @"/  |", "1  |", " 2 |", @"---/" };
        private static string[] _corner4 = { @"|  \", "|  1", "| 2 ", @"\---" };

        #endregion

        private static Vector2 _trackSize;
        private static Vector2 _cursorPosition;
        private static Direction _currentDirection = Direction.North;
        private static Race _currentRace;

        public static void OnNextRace(object sender, NextRaceEventArgs e)
        {
            Initialize(e.Race);

            DrawTrack(_currentRace.Track);
        }

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _trackSize = CalculateTrackSize(race);
            ResetConsole(_currentRace.Track);

            _currentRace.DriversChanged += OnDriversChanged;
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                PrintTrack(GetStrings(section.SectionType), section);
                UpdateDirection(section.SectionType);
                UpdateCursorPosition();
            }
        }

        private static void ResetConsole(Track track)
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
            Console.WriteLine(track.Name);
            Console.WriteLine(_trackSize);
            _cursorPosition.Y = 4;
            _cursorPosition.X = 12;
        }

        private static string[] GetStrings(SectionTypes sectionType) => sectionType switch
        {
            SectionTypes.Straight => _currentDirection switch
            {
                Direction.North => _straightNorth,
                Direction.East => _straightEast,
                Direction.South => _straightSouth,
                Direction.West => _straightWest,
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

        //deze methode is alleen hier gezet om te kijken of het wel werkt voor de 
        //wpf-visualizatie en wordt verder niet in de console gebruikt.
        private static Vector2 CalculateTrackSize(Race race)
        {
            var temp = new Vector2(1, 1);
            foreach (var section in race.Track.Sections)
            {
                if (_currentDirection == Direction.East)
                {
                    temp.X++;
                }

                if (_currentDirection == Direction.South)
                {
                    temp.Y++;
                }
                UpdateDirection(section.SectionType);
            }

            return temp;
        }

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
        private static void PrintTrack(string[] strings, Section section)
        {
            for (int i = 0; i < strings.Length; ++i)
            {
                WriteAt(UpdateString(strings[i], section), (int)_cursorPosition.X, (int)++_cursorPosition.Y);
            }
        }

        //print elke regel van de string in de console met een meegegeven y en x coordinaat
        private static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        private static string UpdateString(string sectionString, Section section) =>
            ReplaceWithParticipant(sectionString, _currentRace.GetSectionData(section).Left, 
                _currentRace.GetSectionData(section).Right);

        //als de participant niet null is dan wordt de placeholder verandert naar de eerste letter van de participants naam 
        //als de participant well null is dan wordt de placeholder verandert naar " "
        private static string ReplaceWithParticipant(string sectionString, IParticipant left, IParticipant right) =>
            sectionString.Replace("1", left != null ? left.Equipment.IsBroken ? "@" : left.Name.Substring(0, 1) : " ")
                .Replace("2", right != null ? right.Equipment.IsBroken ? "@" : right.Name.Substring(0, 1) : " ");

        public static void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }
    }
}