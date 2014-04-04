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

            var hero = new GameLogic.RLAgent(locationX: 5, locationY: 10, displayChar: '@', hitPoints: 200, strength: 50, dexterity: 50);

            
            //place player
            Console.SetCursorPosition(hero.locationX, hero.locationY);
            Console.Write(hero.DisplayChar);


            ConsoleKeyInfo keyInfo;
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
