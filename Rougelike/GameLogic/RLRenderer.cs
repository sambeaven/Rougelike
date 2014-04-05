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

        public List<RLAgent> agents = new List<RLAgent>();

        public RLRenderer()
        {
            messages = new Stack<Tuple<ConsoleColor, string>>();
        }

        public void TakeTurn(GameLogic.RLMap map, GameLogic.RLAgent hero, ConsoleKeyInfo cki)
        {
            DrawMap();

            //movement
            foreach (var agent in agents)
            {
                if (agent.GetType() == typeof(RLHero))
                {
                    PlacePlayer(map, agent, cki);
                }
                else
                {
                    PlaceAgent(map, agent);
                }
            }

            //remove dead agents
            foreach (var agent in agents)
            {
                if (agent.HitPoints <= 0)
                {
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.DarkYellow, agent.Name + " dies."));
                    map.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Passable = true;
                }
            }

            agents = agents.Where(a => a.HitPoints > 0).ToList();

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

        private void PlaceAgent(RLMap map, RLAgent agent)
        {
            Console.SetCursorPosition(agent.locationX, agent.locationY);
            Console.Write(agent.DisplayChar);
            map.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Passable = false;
        }

        private void PlacePlayer(GameLogic.RLMap map, GameLogic.RLAgent hero, ConsoleKeyInfo cki)
        {
            int playerDestinationX, playerDestinationY;

            playerDestinationX = hero.locationX;
            playerDestinationY = hero.locationY;

            var messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.White, "");


            switch (cki.Key)
            {
                case ConsoleKey.UpArrow:
                    playerDestinationY = hero.locationY - 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step north");
                    break;
                case ConsoleKey.DownArrow:
                    playerDestinationY = hero.locationY + 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step south");
                    break;
                case ConsoleKey.LeftArrow:
                    playerDestinationX = hero.locationX - 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step west");
                    break;
                case ConsoleKey.RightArrow:
                    playerDestinationX = hero.locationX + 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step east");
                    break;
                case ConsoleKey.Spacebar:
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Cyan, "You wait");
                    break;
                default:
                    break;
            }

            var destinationAgent = agents.Where(a => a.locationX == playerDestinationX && a.locationY == playerDestinationY).FirstOrDefault();

            if (map.isLocationPassable(playerDestinationX, playerDestinationY))
            {

                hero.locationX = playerDestinationX;
                hero.locationY = playerDestinationY;
            }
            else if(destinationAgent != null)
            {
                var attackResults = destinationAgent.attackedBy(hero);
                foreach (var attackMessage in attackResults)
                {
                    messages.Push(new Tuple<ConsoleColor,string>(ConsoleColor.Red, attackMessage));
                }
            }
            else
            {
                messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Red, "Ouch! You walk into a wall.");
            }

            messages.Push(messageToAdd);

            Console.SetCursorPosition(hero.locationX, hero.locationY);
            Console.Write(hero.DisplayChar);

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
