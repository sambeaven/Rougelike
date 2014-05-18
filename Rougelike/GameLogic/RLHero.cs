using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLHero : RLAgent, IEquatable<RLHero>
    {
        public RLHero(int locationX, int locationY, char displayChar, int hitPoints, int strength, int dexterity, string name, int constitution, ConsoleColor color)
            :base(locationX, locationY, displayChar, hitPoints, strength, dexterity, name, constitution, color)
        {

        }


        public bool Equals(RLHero other)
        {
            return (this.Constitution == other.Constitution &&
                    this.Dexterity == other.Dexterity &&
                    this.DisplayChar == other.DisplayChar &&
                    this.DisplayColor == other.DisplayColor &&
                    this.HitPoints == other.HitPoints &&
                    this.Name == other.Name);
        }
    }
}
