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
    public static class VisualizeWPF
    {
        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static readonly Vector2 SectionSize = new Vector2(128, 128);
        private static readonly Vector2 ParticipantSize = new Vector2(SectionSize.X / 3, SectionSize.Y / 6);

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

        #region participants

        private static string _participantBlue =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\motorcycle_blue.png";

        private static string _participantYellow =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\motorcycle_yellow.png";

        private static string _participantRed =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\motorcycle_red.png";

        private static string _participantGreen =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\motorcycle_green.png";

        private static string _participantGrey =
            @"C:\Users\Sander\source\repos\RaceSimulator\VisualisationWPF\Resources\motorcycle_black.png";

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

            temp.X *= SectionSize.X;
            temp.Y *= SectionSize.Y;
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
                DrawSection(GetSectionImage(section.SectionType), g);
                DrawParticipants(_currentRace.GetSectionData(section), g);
                UpdateDirection(section.SectionType);
                UpdateDrawPosition();
            }

            return EditImage.CreateBitmapSourceFromGdiBitmap(background);
        }

        public static void DrawParticipants(SectionData sectionData, Graphics g)
        {
            if (sectionData.Left != null)
            {
                Bitmap temp = new Bitmap(GetParticipantImage(sectionData.Left.TeamColor));
                temp.RotateFlip(GetRotation());
                g.DrawImage(temp, _drawPosition.X + (SectionSize.X / 2), _drawPosition.Y + (SectionSize.Y / 3),
                    ((int)_currentDirection % 2 == 0) ? ParticipantSize.Y : ParticipantSize.X,
                    ((int)_currentDirection % 2 != 0) ? ParticipantSize.Y : ParticipantSize.X);

            }

            if (sectionData.Right != null)
            {
                Bitmap temp = new Bitmap(GetParticipantImage(sectionData.Right.TeamColor));
                temp.RotateFlip(GetRotation());
                g.DrawImage(temp, _drawPosition.X + (SectionSize.X / 3), _drawPosition.Y + (SectionSize.Y / 2),
                    ((int)_currentDirection % 2 == 0) ? ParticipantSize.Y : ParticipantSize.X,
                    ((int)_currentDirection % 2 != 0) ? ParticipantSize.Y : ParticipantSize.X);
            }
        }

        public static void UpdateDrawPosition()
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    _drawPosition.Y -= SectionSize.Y;
                    break;
                case Direction.East:
                    _drawPosition.X += SectionSize.X;
                    break;
                case Direction.South:
                    _drawPosition.Y += SectionSize.Y;
                    break;
                case Direction.West:
                    _drawPosition.X -= SectionSize.X;
                    break;
            }
        }

        private static void DrawSection(Bitmap sectionImage, Graphics g)
        {
            Bitmap temp = new Bitmap(sectionImage);
            temp.RotateFlip(GetRotation());
            g.DrawImage(temp, _drawPosition.X, _drawPosition.Y, SectionSize.X, SectionSize.Y);
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
            Direction.West => RotateFlipType.Rotate270FlipNone
        };

        public static Bitmap GetParticipantImage(TeamColors color) => color switch
        {
            TeamColors.Blue => EditImage.GetBitmap(_participantBlue),
            TeamColors.Green => EditImage.GetBitmap(_participantGreen),
            TeamColors.Grey => EditImage.GetBitmap(_participantGrey),
            TeamColors.Yellow => EditImage.GetBitmap(_participantYellow),
            TeamColors.Red => EditImage.GetBitmap(_participantRed)
        };
    }
}