using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public int NumberOfLaps { get; set; }

        public Driver(string name, IEquipment equipment, TeamColors teamColor)
        {
            Name = name;
            Points = 0;
            Equipment = equipment;
            TeamColor = teamColor;
            NumberOfLaps = -1;
        }
    }
}