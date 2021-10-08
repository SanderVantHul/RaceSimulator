using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace VisualisationWPF
{
    public static class Visualize
    {
        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static readonly Vector2 _sectionSize = new Vector2(64, 64);

        private static Vector2 _drawPosition = new Vector2(40, 40);
        private static Vector2 _trackSize;
        private static Direction _currentDirection = Direction.North;
        private static Race _currentRace;

        #region graphics

        private static string _straight =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\road_straight.png";

        private static string _cornerRight =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\road_cornerRight.png";

        private static string _cornerLeft =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\road_cornerLeft.png";

        private static string _finish =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\road_finish.png";

        private static string _startGrid =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\road_startgrid.png";

        #endregion

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _trackSize = CalculateTrackSize(race);
        }

        private static Vector2 CalculateTrackSize(Race race)
        {
            var temp = new Vector2(3, 3);
            foreach (var section in race.Track.Sections)
            {
                if (_currentDirection == Direction.East)
                    temp.X++;

                if (_currentDirection == Direction.South)
                    temp.Y++;

                UpdateDirection(section.SectionType);
            }

            temp.X *= _sectionSize.X;
            temp.Y *= _sectionSize.Y;
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

        public static BitmapSource DrawTrack(Track track)
        {
            Bitmap background = EditImage.CreateBitmap((int)_trackSize.X, (int)_trackSize.Y);
            Graphics g = Graphics.FromImage(background);

            foreach (var section in track.Sections)
            {
                DrawSections(GetSectionImage(section.SectionType), g);
                UpdateDirection(section.SectionType);
                UpdateDrawPosition();
            }

            return EditImage.CreateBitmapSourceFromGdiBitmap(background);
        }

        public static void UpdateDrawPosition()
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    _drawPosition.Y -= _sectionSize.Y;
                    break;
                case Direction.East:
                    _drawPosition.X += _sectionSize.X;
                    break;
                case Direction.South:
                    _drawPosition.Y += _sectionSize.Y;
                    break;
                case Direction.West:
                    _drawPosition.X -= _sectionSize.X;
                    break;
            }
        }

        private static void DrawSections(Bitmap sectionImage, Graphics g)
        {
            Bitmap bm = new Bitmap(sectionImage);
            bm.RotateFlip(GetRotation());
            g.DrawImage(bm, _drawPosition.X, _drawPosition.Y, _sectionSize.X, _sectionSize.Y);
        }

        private static Bitmap GetSectionImage(SectionTypes sectionType) => sectionType switch
        {
            SectionTypes.Straight => EditImage.GetBitmap(_straight),
            SectionTypes.LeftCorner => EditImage.GetBitmap(_cornerLeft),
            SectionTypes.RightCorner => EditImage.GetBitmap(_cornerRight),
            SectionTypes.StartGrid => EditImage.GetBitmap(_startGrid),
            SectionTypes.Finish => EditImage.GetBitmap(_finish),
        };

        public static RotateFlipType GetRotation() => _currentDirection switch
        {
            Direction.North => RotateFlipType.RotateNoneFlipNone,
            Direction.East => RotateFlipType.Rotate90FlipNone,
            Direction.South => RotateFlipType.Rotate180FlipNone,
            Direction.West => RotateFlipType.Rotate270FlipNone,
        };
    }
}