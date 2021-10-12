using System;

namespace Model
{
    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }

    public interface IParticipant
    {
        string Name { get; set; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors TeamColor { get; set; }
        int NumberOfLaps { get; set; }
        public DateTime StartTime { get; set; }
    }
}

