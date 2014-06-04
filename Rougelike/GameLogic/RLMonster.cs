using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLMonster : RLAgent
    {
        public RLMonster(int locationX, int locationY, char displayChar, int hitPoints, int strength, int dexterity, string name, int constitution, ConsoleColor color)
            : base(locationX, locationY, displayChar, hitPoints, strength, dexterity, name, constitution, color)
        {

        }

        public enum MonsterBehaviour
        {
            aggressive,
            passive,
            cowardly
        }

        public RLMonster() { }

        private MonsterBehaviour _monsterBehaviour;

        public MonsterBehaviour monsterBehaviour
        {
            get { return _monsterBehaviour; }
            set { _monsterBehaviour = value; }
        }
        



    }
}
