using System.Drawing;
using System.Numerics;
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

        private static readonly string _straight = "..\\..\\..\\Resources\\road_straight.png";
        private static readonly string _cornerRight = "..\\..\\..\\Resources\\road_cornerRight.png";
        private static readonly string _cornerLeft = "..\\..\\..\\Resources\\road_cornerLeft.png";
        private static readonly string _finish = "..\\..\\..\\Resources\\road_finish.png";
        private static readonly string _startGrid = "..\\..\\..\\Resources\\road_startgrid.png";

        #endregion

        #region participants

        private static readonly string _participantBlue = "..\\..\\..\\Resources\\motorcycle_blue.png";
        private static readonly string _participantYellow = "..\\..\\..\\Resources\\motorcycle_yellow.png";
        private static readonly string _participantRed = "..\\..\\..\\Resources\\motorcycle_red.png";
        private static readonly string _participantGreen = "..\\..\\..\\Resources\\motorcycle_green.png";
        private static readonly string _participantGrey = "..\\..\\..\\Resources\\motorcycle_black.png";

        private static readonly string _broken = "..\\..\\..\\Resources\\broken.png";

        #endregion

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _trackSize = CalculateTrackSize(race);
        }

        private static Vector2 CalculateTrackSize(Race race)
        {
            var temp = new Vector2(2, 2);
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
                UpdateDirection(section.SectionType);
                DrawParticipants(_currentRace.GetSectionData(section), g);
                UpdateDrawPosition();
            }

            return EditImage.CreateBitmapSourceFromGdiBitmap(background);
        }

        public static float GetPositionX(bool left) => _currentDirection switch
        {
            Direction.North => left ? _drawPosition.X + SectionSize.X / 1.5f : _drawPosition.X + SectionSize.X / 3,
            Direction.East => left ? _drawPosition.X + SectionSize.X / 1.5f : _drawPosition.X + SectionSize.X / 3.5f,
            Direction.South => left ? _drawPosition.X + SectionSize.X / 2.5f : _drawPosition.X + SectionSize.X / 5,
            Direction.West => left ? _drawPosition.X + SectionSize.X / 4 : _drawPosition.X + SectionSize.X / 2f
        };

        public static float GetPositionY(bool left) => _currentDirection switch
        {
            Direction.North => left ? _drawPosition.Y + SectionSize.Y / 6 : _drawPosition.Y + SectionSize.Y / 3,
            Direction.East => left ? _drawPosition.Y + SectionSize.Y / 2 : _drawPosition.Y + SectionSize.Y / 4,
            Direction.South => left ? _drawPosition.Y + SectionSize.Y / 3 : _drawPosition.Y + SectionSize.Y / 1.5f,
            Direction.West => left ? _drawPosition.Y + SectionSize.Y / 2 : _drawPosition.Y + SectionSize.Y / 4
        };

        public static void DrawParticipants(SectionData sectionData, Graphics g)
        {
            float participantWidth = ((int)_currentDirection % 2 == 0) ? ParticipantSize.Y : ParticipantSize.X;
            float participantHeight = ((int)_currentDirection % 2 != 0) ? ParticipantSize.Y : ParticipantSize.X;
            float brokenWidth = SectionSize.X / 4;
            float brokenHeight = SectionSize.Y / 4;

            if (sectionData.Left != null)
            {
                float imagePositionX = GetPositionX(true);
                float imagePositionY = GetPositionY(true);

                Bitmap temp = new Bitmap(GetParticipantImage(sectionData.Left.TeamColor));
                temp.RotateFlip(GetRotation());

                //teken de participant
                g.DrawImage(temp, imagePositionX, imagePositionY, participantWidth, participantHeight);

                //teken broken equipment
                if (sectionData.Left.Equipment.IsBroken)
                    g.DrawImage(new Bitmap(_broken), imagePositionX, imagePositionY, brokenWidth, brokenHeight);
            }

            if (sectionData.Right != null)
            {
                float imagePositionX = GetPositionX(false);
                float imagePositionY = GetPositionY(false);

                Bitmap temp = new Bitmap(GetParticipantImage(sectionData.Right.TeamColor));
                temp.RotateFlip(GetRotation());

                //teken de participant
                g.DrawImage(temp, imagePositionX, imagePositionY, participantWidth, participantHeight);

                //teken broken equipment
                if (sectionData.Right.Equipment.IsBroken)
                    g.DrawImage(new Bitmap(_broken), imagePositionX, imagePositionY, brokenWidth, brokenHeight);
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