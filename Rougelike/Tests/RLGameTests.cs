using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rougelike.Tests
{
    [TestFixture]
    public class RLGameTests
    {
        [Test]
        public void ConstructorCreatesExpectedValues()
        {
            var RLGame = new GameLogic.RLGame();

            Assert.IsNotNull(RLGame.messages);
            Assert.IsNotNull(RLGame.agents);
        }

        [Test]
        public void SetUpCreatesAgentsAndMap()
        {
            var RLGame = new GameLogic.RLGame();
            RLGame.SetUp();

            Assert.IsNotNull(RLGame.map);
            Assert.IsTrue(RLGame.agents.Count() > 0);
        }
    }
}
