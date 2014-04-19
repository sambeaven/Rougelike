using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

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

        [Test]
        public void MoqTest()
        {
            var levelGenerator = GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object);

            game.SetUp();

        }

        private static Mock<GameLogic.Interfaces.IRLLevelGenerator> GetMockLevelGenerator()
        {
            var levelGenerator = new Mock<GameLogic.Interfaces.IRLLevelGenerator>();

            levelGenerator.Setup(l => l.GenerateMap())
                .Returns(new GameLogic.RLMap(GameLogic.RLMap.MapType.emptyMap, mapHeight: 3, mapWidth: 3){
                    new GameLogic.RLCell() { X = 0, Y = 0},
                    new GameLogic.RLCell() { X = 1, Y = 0},
                    new GameLogic.RLCell() { X = 2, Y = 0},
                    new GameLogic.RLCell() { X = 0, Y = 1},
                    new GameLogic.RLCell() { X = 1, Y = 1},
                    new GameLogic.RLCell() { X = 2, Y = 1},
                    new GameLogic.RLCell() { X = 0, Y = 2},
                    new GameLogic.RLCell() { X = 1, Y = 2},
                    new GameLogic.RLCell() { X = 2, Y = 2}
                });

            levelGenerator.Setup(l => l.GenerateAgents(GameLogic.RLLevelGenerator.agentGeneratorBehaviour.IncludeHero))
                .Returns(new List<GameLogic.RLAgent>(){
                    new GameLogic.RLHero(
                            locationX: 1,
                            locationY: 1,
                            displayChar: '@',
                            hitPoints: 100,
                            strength: 100,
                            dexterity: 100,
                            constitution: 100,
                            name: "You",
                            color: ConsoleColor.Gray
                        )
                });
            return levelGenerator;
        }

    }
}
