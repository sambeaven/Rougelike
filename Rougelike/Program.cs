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

            int? playerDestinationX = null, playerDestinationY = null;
            //place player
            Console.SetCursorPosition(hero.locationX, hero.locationY);
            Console.Write(hero.DisplayChar);


            ConsoleKeyInfo keyInfo;
            //listen for input and handle moves
            do
            {
                playerDestinationX = hero.locationX;
                playerDestinationY = hero.locationY;

                keyInfo = Console.ReadKey(true);

                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        playerDestinationY = hero.locationY - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        playerDestinationY = hero.locationY + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        playerDestinationX = hero.locationX - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        playerDestinationX = hero.locationX + 1;
                        break;
                    case ConsoleKey.Spacebar:
                        break;
                    default:
                        break;
                }

                renderer.TakeTurn(map, hero, playerDestinationX, playerDestinationY);


            } while (keyInfo.Key != ConsoleKey.Escape);

            //end game
            //Console.ReadLine();
        }
    }
}
