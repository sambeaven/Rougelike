using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike
{
    //Wider Architecture:

    //Class Entity (anything interactable) - has an Interact() method and a DisplayChar() property.
    //              Interact takes a player? Or just an entity? Player makes more sense.
    //Class Player inherits from Entity. Interact() causes the player to lose health? Maybe?
    //Class Monster : Entity - Interact() method causes player to lose health
    //Monster and Player both have HP and Attack properties, and a List of Items. 
    //              On death, monsters (and players?) drop treasure piles

    //Item has a Use(Entity) method




    class Program
    {

        static void Main(string[] args)
        {
            //draw dungeon
            var map = DrawMap();

            var hero = new GameLogic.RLAgent() { locationX = 5, locationY = 10, DisplayChar = '@' };

            int? playerDestinationX = null, playerDestinationY = null;
            //place player
            PlacePlayer(map, hero, hero.locationX, hero.locationY);


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
                    default:
                        break;
                }

                PlacePlayer(map, hero, playerDestinationX, playerDestinationY);

            } while (keyInfo.Key != ConsoleKey.Escape);

            //end game
            //Console.ReadLine();
        }

        private static void PlacePlayer(GameLogic.RLMap map, GameLogic.RLAgent hero, int? x, int? y)
        {
            if ((x.HasValue && y.HasValue) && map.isLocationPassable(x.Value, y.Value))
            {
                DrawMap();
                hero.locationY = y.Value;
                hero.locationX = x.Value;


                Console.SetCursorPosition(x.Value, y.Value);
                Console.Write(hero.DisplayChar);

                Console.SetCursorPosition(0, 20);
            }
        }

        private static GameLogic.RLMap DrawMap()
        {
            GameLogic.RLMap map = new GameLogic.RLMap();

            for (int x = 0; x < map.MaxWidth; x++)
            {
                for (int y = 0; y < map.MaxHeight; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(map.Where(c => c.X == x && c.Y == y).FirstOrDefault().DisplayCharacter);
                }
            }
            return map;
        }

    }
}
