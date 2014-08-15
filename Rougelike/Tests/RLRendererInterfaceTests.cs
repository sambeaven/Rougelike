using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Rougelike.Tests
{
    /// <summary>
    /// These tests are intended to test the interface between the main GameLogic class and the renderer itself.
    /// So the 'renderer' itself isn't really being tested, despite the name of the class.
    /// Instead, we're mocking a renderer and verifying it works as intended.
    /// </summary>
    [TestFixture]
    public class RLRendererInterfaceTests
    {
        /// <summary>
        /// Tests that the correct renderer methods are called during initial setup only
        /// TODO: Fill in more detailed values, times for DrawAgent to be called, etc.
        /// </summary>
        [Test]
        public void RLGameLogic_when_rendering_during_setup_the_correct_methods_are_called()
        {
            var levelGenerator = RLMapHelpers.GetMockLevelGenerator();

            var mockMap = levelGenerator.Object.GenerateMap();

            var mockRenderer = new Mock<GameLogic.Interfaces.IRLRenderer>();

            mockRenderer.Setup(r => r.DrawMap(mockMap)).Verifiable();
            mockRenderer.Setup(r => r.DrawAgent(mockMap, It.IsAny<GameLogic.RLAgent>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();

            var game = new GameLogic.RLGame(renderer: mockRenderer.Object, levelGenerator: levelGenerator.Object, jsonService: new IOLogic.JsonGameIOService());

            game.SetUp();

            mockRenderer.Verify();
        }


        [Ignore]
        [Test]
        public void RLGameLogic_when_moving_hero_correct_renderer_methods_are_called()
        {
            //TODO: fill in this method
        }

        [Ignore]
        [Test]
        public void RLGaemLogic_when_moving_hero_and_monster_is_present_correct_renderer_methods_are_called()
        {
            //TODO: fill in this method
        }
    }
}
