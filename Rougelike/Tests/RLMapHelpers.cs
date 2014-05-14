using Moq;
using System;
using System.Collections.Generic;

namespace Rougelike.Tests
{
    public static class RLMapHelpers
    {
        public static Mock<GameLogic.Interfaces.IRLLevelGenerator> GetMockLevelGenerator()
        {
            var levelGenerator = new Mock<GameLogic.Interfaces.IRLLevelGenerator>();

            levelGenerator.Setup(l => l.GenerateMap())
                .Returns(new GameLogic.RLMap(GameLogic.RLMap.MapType.emptyMap, mapHeight: 3, mapWidth: 3, cells: new List<GameLogic.RLCell>(){
                    new GameLogic.RLCell() { X = 0, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 0, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 1, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 0, Y = 2, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 1, Y = 2, Passable=true, Unoccupied=true},
                    new GameLogic.RLCell() { X = 2, Y = 2, Passable=true, Unoccupied=true}
                }));

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
