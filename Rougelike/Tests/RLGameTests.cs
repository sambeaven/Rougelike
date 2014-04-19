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
            var levelGenerator = GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object);

            game.SetUp();


            Assert.IsNotNull(game.map);
            Assert.IsTrue(game.agents.Count() > 0);
        }


        [TestCase(1, 0, ConsoleKey.UpArrow)]
        [TestCase(1, 2, ConsoleKey.DownArrow)]
        [TestCase(0, 1, ConsoleKey.LeftArrow)]
        [TestCase(2, 1, ConsoleKey.RightArrow)]
        [TestCase(1, 1, ConsoleKey.Spacebar)]
        public void TakeTurnHandlesMove(int expectedX, int expectedY, ConsoleKey keyToPass)
        {
            var levelGenerator = GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object);

            game.SetUp();

            var ConsoleKeyInfo = new ConsoleKeyInfo(keyChar: ' ', key: keyToPass, shift: false, alt: false, control: false);

            game.TakeTurn(ConsoleKeyInfo);

            var hero = GetHero(game);

            Assert.AreEqual(expectedX, hero.locationX);
            Assert.AreEqual(expectedY, hero.locationY);

            if (keyToPass == ConsoleKey.Spacebar)
            {
                Assert.IsTrue(game.messages.First().Item2 == "You wait");

            }

        }

        private static GameLogic.RLAgent GetHero(GameLogic.RLGame game)
        {
            return game.agents.Where(a => a.GetType() == typeof(GameLogic.RLHero))
                            .FirstOrDefault();
        }



        private static Mock<GameLogic.Interfaces.IRLLevelGenerator> GetMockLevelGenerator()
        {
            var levelGenerator = new Mock<GameLogic.Interfaces.IRLLevelGenerator>();

            levelGenerator.Setup(l => l.GenerateMap())
                .Returns(new GameLogic.RLMap(GameLogic.RLMap.MapType.emptyMap, mapHeight: 3, mapWidth: 3){
                    new GameLogic.RLCell() { X = 0, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 2, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 2, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 2, Passable=true, Unoccupied=true}
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
