using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLLevelGenerator : Interfaces.IRLLevelGenerator
    {
        public List<RLMonster> GenerateMonsters()
        {
            var monsters = new List<RLMonster>();


            var monster1 = new GameLogic.RLMonster(
                locationX: 10,
                locationY: 10,
                displayChar: 'g',
                hitPoints: 200,
                strength: 25,
                dexterity: 25,
                name: "Goblin",
                constitution: 25,
                color: ConsoleColor.DarkGreen
                );

            var monster2 = new GameLogic.RLMonster(
                locationX: 10,
                locationY: 15,
                displayChar: 'X',
                hitPoints: 200,
                strength: 75,
                dexterity: 75,
                name: "Balrog",
                constitution: 75,
                color: ConsoleColor.Red
                );

            monster2.monsterBehaviour = RLMonster.MonsterBehaviour.cowardly;

            monsters.Add(monster1);
            monsters.Add(monster2);

            return monsters;

        }

        public RLHero GetDefaultHero()
        {
            return new GameLogic.RLHero(
                locationX: 5,
                locationY: 10,
                displayChar: '@',
                hitPoints: 200,
                strength: 50,
                dexterity: 50,
                name: "You",
                constitution: 50,
                color: ConsoleColor.Green
                );
        }


        public RLMap GenerateMap()
        {
            return new GameLogic.RLMap(GameLogic.RLMap.MapType.boxMap);
        }
    }
}
