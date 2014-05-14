using System;
using System.Linq;
using NUnit.Framework;
using Moq;

namespace Rougelike.Tests
{
    public class RLMapTests
    {
        [Test]
        public void TwoEqualMapsAreEqual()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();
            var game1 = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());
            var game2 = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());

            game1.SetUp();
            game2.SetUp();

            Assert.AreEqual(game1.map, game2.map);
        }

        [Test]
        public void TwoUnequalMapsAreUnEqual()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();
            GameLogic.RLMap map1 = levelGenerator.Object.GenerateMap();
            
            levelGenerator = RLMapHelpers.GetMockLevelGenerator();
            GameLogic.RLMap map2 = levelGenerator.Object.GenerateMap();

            Assert.IsTrue(map1.Cells.First().Passable);

            map2.Cells.First().Passable = false;

            Assert.IsFalse(map1.Equals(map2));
        }

    }
}
