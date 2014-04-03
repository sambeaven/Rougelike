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
            Stack<Tuple<ConsoleColor, string>> messages = new Stack<Tuple<ConsoleColor, string>>();
            var map = DrawMap();

            var hero = new GameLogic.RLAgent(locationX: 5, locationY: 10, displayChar: '@', hitPoints: 200, strength: 50, dexterity: 50);

            int? playerDestinationX = null, playerDestinationY = null;
            //place player
            PlacePlayer(map, hero, hero.locationX, hero.locationY, messages);


            ConsoleKeyInfo keyInfo;
            //listen for input and handle moves
            do
            {
                playerDestinationX = hero.locationX;
                playerDestinationY = hero.locationY;

                keyInfo = Console.ReadKey(true);

                //TODO: re-architect this: move message logic down into the TakeTurn method. Figure out direction after running through PlacePlayer()
                //(maybe we should re-architect all of this into a class?)
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        playerDestinationY = hero.locationY - 1;
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step north"));
                        break;
                    case ConsoleKey.DownArrow:
                        playerDestinationY = hero.locationY + 1;
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step south"));
                        break;
                    case ConsoleKey.LeftArrow:
                        playerDestinationX = hero.locationX - 1;
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step west"));
                        break;
                    case ConsoleKey.RightArrow:
                        playerDestinationX = hero.locationX + 1;
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step east"));
                        break;
                    case ConsoleKey.Spacebar:
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You wait"));
                        break;
                    default:
                        break;
                }

                TakeTurn(map, hero, playerDestinationX, playerDestinationY, messages);


            } while (keyInfo.Key != ConsoleKey.Escape);

            //end game
            //Console.ReadLine();
        }

        private static void TakeTurn(GameLogic.RLMap map, GameLogic.RLAgent hero, int? x, int? y, Stack<Tuple<ConsoleColor, string>> messages)
        {
            DrawMap();
            messages = PlacePlayer(map, hero, x, y, messages);


            //post messages
            Console.SetCursorPosition(0, 20);
            var messagesToDisplay = messages.Take(5).ToList();
            int i = 0;
            foreach (var message in messages)
            {


                Console.SetCursorPosition(0, 20 + i);

                Console.ForegroundColor = message.Item1;
                Console.WriteLine(message.Item2.Trim() + new String(' ', Console.BufferWidth));
                if (i >= 4) { break; } else { i++; }

            }
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

        }

        private static Stack<Tuple<ConsoleColor, string>> PlacePlayer(GameLogic.RLMap map, GameLogic.RLAgent hero, int? x, int? y, Stack<Tuple<ConsoleColor, string>> messages)
        {
            if ((x.HasValue && y.HasValue) && map.isLocationPassable(x.Value, y.Value))
            {

                hero.locationY = y.Value;
                hero.locationX = x.Value;


                Console.SetCursorPosition(x.Value, y.Value);
                Console.Write(hero.DisplayChar);

                
            }
            else
            {
                Console.SetCursorPosition(hero.locationX, hero.locationY);
                Console.Write(hero.DisplayChar);
                messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, "Ouch! You walk into a wall."));
            }

            return messages;
        }

        private static GameLogic.RLMap DrawMap()
        {
            //Console.Clear();
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
