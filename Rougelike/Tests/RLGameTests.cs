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

        [Test]
        public void TakeTurnDoesNotMoveHeroWhenSpaceBarIsPassed()
        {
            var game = new GameLogic.RLGame();
            game.SetUp();

            var hero = game.agents.Where(a => a.GetType() == typeof(GameLogic.RLHero)).FirstOrDefault();

            int initialX = hero.locationX;
            int initialY = hero.locationY;

            Assert.IsNotNull(hero);

            game.TakeTurn(new ConsoleKeyInfo(keyChar:' ', key:ConsoleKey.Spacebar, shift:false, alt:false, control:false));

            hero = game.agents.Where(a => a.GetType() == typeof(GameLogic.RLHero)).FirstOrDefault();

            Assert.AreEqual(initialX, hero.locationX);
            Assert.AreEqual(initialY, hero.locationY);
        }


    }
}
