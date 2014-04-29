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
            var game1 = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object);
            var game2 = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object);

            game1.SetUp();
            game2.SetUp();

            Assert.AreEqual(game1.map, game2.map);
        }

        [Test]
        public void TwoUnequalMapsAreUnEqual()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();
            var map1 = levelGenerator.Object.GenerateMap();
            var map2 = levelGenerator.Object.GenerateMap();

            map2.First().Passable = !map2.First().Passable;

            //Assert.AreNotEqual(game1.map, game2.map);
            Assert.IsFalse(map1.Equals(map2));
        }

    }
}
