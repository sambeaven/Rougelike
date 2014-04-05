using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLHero : RLAgent
    {
        public RLHero(int locationX, int locationY, char displayChar, int hitPoints, int strength, int dexterity, string name, int constitution)
            :base(locationX, locationY, displayChar, hitPoints, strength, dexterity, name, constitution)
        {

        }
    }
}
