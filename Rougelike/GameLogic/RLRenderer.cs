using System;
using System.Collections.Generic;
using System.Linq;

namespace Rougelike.GameLogic
{
    public class RLRenderer : Interfaces.IRLRenderer
    {
        public void PostMessages(Stack<Tuple<ConsoleColor, string>> messages)
        {
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

        public void DrawAgent(RLMap map, RLAgent agent, int x, int y)
        {
            map.Cells.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Unoccupied = true;
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = agent.DisplayColor;
            agent.locationX = x;
            agent.locationY = y;
            Console.Write(agent.DisplayChar);
            Console.ForegroundColor = ConsoleColor.White;
            map.Cells.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Unoccupied = false;
        }

        public void DrawMap(RLMap map)
        {
            Console.ForegroundColor = ConsoleColor.White;

            for (int x = 0; x < map.MaxWidth; x++)
            {
                for (int y = 0; y < map.MaxHeight; y++)
                {
                    Console.SetCursorPosition(x, y);
                    char characterToDisplay = new char();
                    ConsoleColor color;
                    RLCell cell = map.Cells.Where(c => c.X == x && c.Y == y).FirstOrDefault();
                    if (cell.Items.Count == 0)
                    {
                        characterToDisplay = cell.DisplayCharacter;
                        color = ConsoleColor.White;
                    }
                    else
                    {
                        characterToDisplay = cell.Items.First().DisplayChar;
                        color = cell.Items.First().DisplayColor;
                    }
                    Console.ForegroundColor = color;
                    Console.Write(characterToDisplay);
                }
            }
        }

    }
}
