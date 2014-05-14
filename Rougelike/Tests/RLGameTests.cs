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
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());

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
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());

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


        [Test]
        public void LoadingFromASavedGameReturnsTheSameGame()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var savingGame = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());

            savingGame.SetUp();

            var ConsoleKeyInfo = new ConsoleKeyInfo(keyChar: ' ', key: ConsoleKey.UpArrow, shift: false, alt: false, control: false);

            savingGame.TakeTurn(ConsoleKeyInfo);

            var hero = GetHero(savingGame);

            savingGame.SaveGame();

            var loadingGame = GameLogic.RLGame.LoadGame(new IOLogic.JsonGameIOService());

            //Map is the same
            Assert.AreEqual(savingGame.map.Cells.Count, loadingGame.map.Cells.Count);

            //Agents are the same
            Assert.AreEqual(savingGame.agents.Count, loadingGame.agents.Count);
            foreach (var savedAgent in savingGame.agents)
            {
                Assert.IsTrue(loadingGame.agents.Contains(savedAgent), "loaded game does not contain the agent " + savedAgent.Name);
            }
        }




        private static GameLogic.RLAgent GetHero(GameLogic.RLGame game)
        {
            return game.agents.Where(a => a.GetType() == typeof(GameLogic.RLHero))
                            .FirstOrDefault();
        }
    }
}
