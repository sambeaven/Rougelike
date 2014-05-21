using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLDice : Rougelike.GameLogic.Interfaces.IRLDice
    {
        public int RollD100()
        {
            return _random.Next(1, 100);
        }

        public int RollD4()
        {
            return _random.Next(1, 5);
        }

        private Random _random;

        public RLDice(Random random)
        {
            _random = random;
        }

        public RLDice()
        {
            _random = new Random();
        }

    }
}
