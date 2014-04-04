using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    /// <summary>
    /// I guess I should probably make this a singleton one day? Maybe?
    /// </summary>
    public class RLRenderer
    {

        public Stack<Tuple<ConsoleColor, string>> messages { get; set; }

        public RLRenderer()
        {
            messages = new Stack<Tuple<ConsoleColor, string>>();
        }

        public void TakeTurn(GameLogic.RLMap map, GameLogic.RLAgent hero, int? x, int? y)
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

        public Stack<Tuple<ConsoleColor, string>> PlacePlayer(GameLogic.RLMap map, GameLogic.RLAgent hero, int? x, int? y, Stack<Tuple<ConsoleColor, string>> messages)
        {
            if ((x.HasValue && y.HasValue) && map.isLocationPassable(x.Value, y.Value))
            {
                if (y < hero.locationY)
                {
                    //y - 1 == north
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step north"));
                }
                else if (y > hero.locationY)
                {
                    //y + 1 == south
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step south"));
                }
                else if (x < hero.locationX)
                {
                    //x - 1 == west
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step west"));
                }
                else if (x > hero.locationX)
                {
                    //x + 1 == east
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step east"));
                }
                else
                {
                    //no change == wait
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Cyan, "You wait"));
                }

                hero.locationY = y.Value;
                hero.locationX = x.Value;
            }
            else
            {

                messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, "Ouch! You walk into a wall."));
            }
            Console.SetCursorPosition(hero.locationX, hero.locationY);
            Console.Write(hero.DisplayChar);
            return messages;
        }

        public GameLogic.RLMap DrawMap()
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
