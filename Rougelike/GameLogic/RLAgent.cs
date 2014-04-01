using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLAgent
    {
        public int locationX { get; set; }
        public int locationY { get; set; }
        public char DisplayChar { get; set; }

        public int HitPoints { get; set; }
        /// <summary>
        /// Used to determine damage. 1-100
        /// </summary>
        public int Strength { get; set; }
        /// <summary>
        /// used to determine initiative and hit rolls. 1-100
        /// </summary>
        public int Dexterity { get; set; }

        public RLAgent(int locationX, int locationY, char displayChar, int hitPoints, int strength, int dexterity)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.DisplayChar = displayChar;
            this.HitPoints = hitPoints;
            this.Strength = strength;
            this.Dexterity = dexterity;
        }

    }
}
