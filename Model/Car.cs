using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public Car(int speed, int performance)
        {
            Quality = 0; 
            Performance = performance;
            Speed = speed;
            IsBroken = false;
        }
    }
}
