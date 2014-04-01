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
            //draw dungeon
            var map = DrawMap();

            var hero = new GameLogic.RLAgent(locationX: 5, locationY: 10, displayChar: '@', hitPoints: 200, strength: 50, dexterity: 50);

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

                List<Tuple<ConsoleColor, string>> messages = new List<Tuple<ConsoleColor, string>>();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        playerDestinationY = hero.locationY - 1;
                        messages.Add(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step north"));
                        break;
                    case ConsoleKey.DownArrow:
                        playerDestinationY = hero.locationY + 1;
                        messages.Add(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step south"));
                        break;
                    case ConsoleKey.LeftArrow:
                        playerDestinationX = hero.locationX - 1;
                        messages.Add(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step west"));
                        break;
                    case ConsoleKey.RightArrow:
                        playerDestinationX = hero.locationX + 1;
                        messages.Add(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step east"));
                        break;
                    case ConsoleKey.Spacebar:
                        messages.Add(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You wait"));
                        break;
                    default:
                        break;
                }

                PlacePlayer(map, hero, playerDestinationX, playerDestinationY);
                foreach (var message in messages)
                {
                    //This doesn't work very well. I think I need a stack of messages (say, the last 5) and to write all of them.
                    Console.ForegroundColor = message.Item1;
                    Console.WriteLine(message.Item2);
                }
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
            Console.ForegroundColor = ConsoleColor.White;
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
