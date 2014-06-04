using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.Tests
{
    [TestFixture]
    public class RLCombatTests
    {
        //TODO: Make this work. We need an interface for RLMonster.
        //[Test]
        //public void WhenHeroMovesOntoMonsterAttackedbyIsCalledOnTheMonsterCorrectly()
        //{
        //    var levelGenerator = RLMapHelpers.GetMockLevelGenerator();
        //    string sMonsterName = Guid.NewGuid().ToString();
        //    string sHeroName = Guid.NewGuid().ToString();

        //    var mockMonster = Mock.Of<GameLogic.IRLAgent>(m => m.locationX == 1 
        //        && m.locationY == 0
        //        && m.Name == sMonsterName);

            

        //    levelGenerator.Setup(l => l.GenerateMonsters())
        //        .Returns(new List<GameLogic.RLMonster>{
        //            mockMonster
        //        });
        //    var hero = new GameLogic.RLHero(1, 1, '@', 100, 100, 100, sHeroName, 100, ConsoleColor.White);
        //    levelGenerator.Setup(l => l.GetDefaultHero()).Returns(
        //        hero);


        //    var game = new GameLogic.RLGame(new GameLogic.RLRenderer(), levelGenerator.Object, new IOLogic.JsonGameIOService());
        //    game.SetUp();

        //    game.ProcessInput(GameLogic.RLPlayerAction.MoveRight);

        //    Mock.Get(mockMonster).Verify(m => m.attackedBy(hero, game.dice), Times.AtLeastOnce());

        //}
    }
}
