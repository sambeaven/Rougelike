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
            Assert.IsNotNull(RLGame.monsters);
        }

        [Test]
        public void SetUpCreatesAgentsAndMap()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var mockRenderer = Mock.Of<GameLogic.Interfaces.IRLRenderer>();

            var game = new GameLogic.RLGame(mockRenderer, levelGenerator.Object, new IOLogic.JsonGameIOService());

            game.SetUp();


            Assert.IsNotNull(game.map);
            Assert.IsNotNull(game.hero);
        }


        [TestCase(1, 0, GameLogic.RLPlayerAction.MoveUp)]
        [TestCase(1, 2, GameLogic.RLPlayerAction.MoveDown)]
        [TestCase(0, 1, GameLogic.RLPlayerAction.MoveLeft)]
        [TestCase(2, 1, GameLogic.RLPlayerAction.MoveRight)]
        [TestCase(1, 1, GameLogic.RLPlayerAction.Wait)]
        public void TakeTurnHandlesMove(int expectedX, int expectedY, GameLogic.RLPlayerAction actionToPass)
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());

            game.SetUp();

            game.ProcessInput(actionToPass);

            Assert.AreEqual(expectedX, game.hero.locationX);
            Assert.AreEqual(expectedY, game.hero.locationY);

            if (actionToPass == GameLogic.RLPlayerAction.Wait)
            {
                Assert.IsTrue(game.messages.First().Item2 == "You wait");

            }

        }

        [Test]
        public void WhenMovingIntoAWallAMessageIsDisplayed()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            levelGenerator.Setup(l => l.GenerateMap())
                .Returns(new GameLogic.RLMap(GameLogic.RLMap.MapType.emptyMap, mapHeight: 3, mapWidth: 3, cells: new List<GameLogic.RLCell>(){
                    new GameLogic.RLCell() { X = 0, Y = 0, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 0, Passable=false, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 0, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 1, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 1, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 1, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 2, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 2, Passable=true,  Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 2, Passable=true,  Unoccupied=true}
                }));
            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());
            game.SetUp();

            game.ProcessInput(GameLogic.RLPlayerAction.MoveUp);

            //Because 0,1 is marked as not passable, we shouldn't have moved
            Assert.AreEqual(1, game.hero.locationX);
            Assert.AreEqual(1, game.hero.locationY);

            //There should also be a message
            Assert.IsTrue(game.messages.Where(m => m.Item2 == GameLogic.RLGame.DESTINATION_IMPASSABLE)
                                       .Count() > 0);

            Assert.IsTrue(game.messages.Where(m => m.Item2 == GameLogic.RLGame.DESTINATION_IMPASSABLE)
                                       .Select(m => m.Item1)
                                       .FirstOrDefault() == ConsoleColor.Red);
        }

        [Test]
        public void LoadingFromASavedGameReturnsTheSameGame()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var savingGame = new GameLogic.RLGame(levelGenerator: levelGenerator.Object);

            savingGame.SetUp();


            savingGame.ProcessInput(GameLogic.RLPlayerAction.EmptyAction);

            bool testResult = savingGame.SaveGame();

            Assert.IsTrue(testResult);

            var savingMap = savingGame.map;
            var savingAgents = savingGame.monsters;
            var savingHero = savingGame.hero;

            savingGame.LoadGame();

            //Map is the same
            Assert.AreEqual(savingMap.Cells.Count, savingGame.map.Cells.Count); 

            //Agents are the same
            Assert.AreEqual(savingAgents.Count, savingGame.monsters.Count);
            Assert.IsTrue(savingHero.Equals(savingGame.hero));
        }





    }
}
