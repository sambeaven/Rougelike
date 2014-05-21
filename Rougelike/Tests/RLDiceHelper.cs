using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.Tests
{
    public class RLDiceHelper
    {
        public static Mock<GameLogic.Interfaces.IRLDice> GetMockDice()
        {
            return GetMockDice(50, 2);
        }

        public static Mock<GameLogic.Interfaces.IRLDice> GetMockDice(int d100result, int d4result)
        {
            var dice = new Mock<GameLogic.Interfaces.IRLDice>();

            dice.Setup(d => d.RollD100())
                .Returns(d100result);

            dice.Setup(d => d.RollD4())
                .Returns(d4result);

            return dice;
        }
    }
}
