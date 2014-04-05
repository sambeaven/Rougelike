using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike
{

    class Program
    {

        static void Main(string[] args)
        {
            var renderer = new GameLogic.RLRenderer();
            //draw dungeon
            var map = renderer.DrawMap();

            var hero = new GameLogic.RLHero(
                locationX: 5, 
                locationY: 10, 
                displayChar: '@', 
                hitPoints: 200, 
                strength: 50, 
                dexterity: 50,
                name: "You",
                constitution: 50
                );

            var monster1 = new GameLogic.RLMonster(
                locationX: 10,
                locationY: 10,
                displayChar: 'g',
                hitPoints: 200,
                strength: 25,
                dexterity: 25,
                name: "Goblin",
                constitution: 25
                );

            var monster2 = new GameLogic.RLMonster(
                locationX: 10,
                locationY: 15,
                displayChar: 'X',
                hitPoints: 200,
                strength: 75,
                dexterity: 75,
                name: "Balrog",
                constitution: 75
                );

            renderer.agents.Add(hero);
            renderer.agents.Add(monster1);
            renderer.agents.Add(monster2);

            //place player

            //turn this into a TakeInitialTurn method? Or Setup?
            //I could put all the above setup code in there as well (although it's not likely to stay there).
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            renderer.TakeTurn(map, hero, keyInfo);

            //listen for input and handle moves
            do
            {
                keyInfo = Console.ReadKey(true);

                renderer.TakeTurn(map, hero, keyInfo);
            } while (keyInfo.Key != ConsoleKey.Escape);

            //end game
            //Console.ReadLine();
        }
    }
}
