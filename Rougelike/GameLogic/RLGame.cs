using System;
using System.Collections.Generic;
using System.Linq;

namespace Rougelike.GameLogic
{
    /// <summary>
    /// I guess I should probably make this a singleton one day? Maybe?
    /// </summary>
    public class RLGame
    {

        public Stack<Tuple<ConsoleColor, string>> messages { get; set; }

        public List<RLAgent> agents = new List<RLAgent>();

        private RLRenderer renderer;

        private RLMap map;

        public RLGame()
        {
            messages = new Stack<Tuple<ConsoleColor, string>>();
            renderer = new RLRenderer();
        }

        public void TakeTurn(ConsoleKeyInfo cki)
        {
            renderer.DrawMap();

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
                    map.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Unoccupied = true;
                }
            }


            agents.Where(a => a.HitPoints <= 0)
                .ToList()
                .ForEach(a => messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.DarkRed, a.Name + " died.")));


            agents = agents.Where(a => a.HitPoints > 0).ToList();


            renderer.PostMessages(messages);

        }

        private void PlaceAgent(RLMap map, RLAgent agent)
        {
            if (agent.GetType() == typeof(RLMonster)) //always true at the moment, but might not be if I introduce NPCs.
            {
                var monster = (RLMonster)agent;
                var hero = (RLHero)agents.Where(a => a.GetType() == typeof(RLHero)).First();

                Tuple<int, int> destination = null;

                //if monster can see hero, move according to monster behaviour
                if (CanSeeEachOther(agent, hero, map))
                {


                    //messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, agent.Name + " can see " + hero.Name + "!"));
                    switch (monster.monsterBehaviour)
                    {
                        case RLMonster.MonsterBehaviour.aggressive:
                            destination = moveTowards(monster, hero);
                            break;
                        case RLMonster.MonsterBehaviour.passive:
                            destination = moveRandom(monster);
                            break;
                        case RLMonster.MonsterBehaviour.cowardly:
                            destination = moveAway(monster, hero);
                            break;
                        default:
                            break;
                    }
                }
                else //otherwise, move at random
                {
                    destination = moveRandom(monster);

                }

                if (destination != null)
                {
                    if (destination.Item1 == hero.locationX && destination.Item2 == hero.locationY)
                    {
                        //redraw in the same location
                        renderer.DrawAgent(map, monster, monster.locationX, monster.locationY);
                        //attack hero
                        var attackResults = hero.attackedBy(monster);
                        foreach (var attackMessage in attackResults)
                        {
                            messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, attackMessage));
                        }
                    }
                    else if (map.isLocationPassable(destination.Item1, destination.Item2))
                    {
                        renderer.DrawAgent(map, monster, destination.Item1, destination.Item2);
                    }
                    else
                    {
                        //redraw in the same location
                        renderer.DrawAgent(map, monster, monster.locationX, monster.locationY);
                    }
                }
            }
        }

        private Tuple<int, int> moveRandom(RLAgent agent)
        {
            Random randomDirection = new Random();

            int direction = randomDirection.Next(1, 4);
            int x = agent.locationX, y = agent.locationY;

            switch (direction)
            {
                case 1:
                    //north
                    y = agent.locationY - 1;
                    break;
                case 2:
                    //south
                    y = agent.locationY + 1;
                    break;
                case 3:
                    //east
                    x = agent.locationX - 1;
                    break;
                case 4:
                    //west
                    x = agent.locationX + 1;
                    break;
                default:
                    break;
            }

            return new Tuple<int, int>(x, y);
        }

        private Tuple<int, int> moveAway(RLMonster monster, RLHero hero)
        {
            //work out differences to find the axis with the greatest difference. We'll move along that axis.

            int x = monster.locationX, y = monster.locationY;

            int xDiff = monster.locationX - hero.locationX;
            int yDiff = monster.locationY - hero.locationY;

            if (xDiff < 0)
            {
                xDiff = xDiff * -1;
            }

            if (yDiff < 0)
            {
                yDiff = yDiff * -1;
            }

            if (xDiff >= yDiff)
            {
                if (hero.locationX > monster.locationX)
                {
                    x--;
                }
                else
                {
                    x++;
                }
            }
            else
            {
                if (hero.locationY > monster.locationY)
                {
                    y--;
                }
                else
                {
                    y++;
                }
            }

            return new Tuple<int, int>(x, y);
        }

        private Tuple<int, int> moveTowards(RLMonster monster, RLHero hero)
        {
            //work out differences to find the axis with the greatest difference. We'll move along that axis.

            int x = monster.locationX, y = monster.locationY;

            int xDiff = monster.locationX - hero.locationX;
            int yDiff = monster.locationY - hero.locationY;

            if (xDiff < 0)
            {
                xDiff = xDiff * -1;
            }

            if (yDiff < 0)
            {
                yDiff = yDiff * -1;
            }

            if (xDiff >= yDiff)
            {
                if (hero.locationX > monster.locationX)
                {
                    x++;
                }
                else
                {
                    x--;
                }
            }
            else
            {
                if (hero.locationY > monster.locationY)
                {
                    y++;
                }
                else
                {
                    y--;
                }
            }

            return new Tuple<int, int>(x, y);
        }

        private bool CanSeeEachOther(RLAgent first, RLAgent second, RLMap map)
        {
            Func<int, int> moveTowardsX = null;
            Func<int, int> moveTowardsY = null;

            if (first.locationX > second.locationX)
            {
                moveTowardsX = x => x - 1;
            }
            else
            {
                moveTowardsX = x => x + 1;
            }

            if (first.locationY > second.locationY)
            {
                moveTowardsY = y => y - 1;
            }
            else
            {
                moveTowardsY = y => y + 1;
            }

            int checkX = first.locationX;
            int checkY = first.locationY;

            do
            {
                checkX = (checkX != second.locationX) ? moveTowardsX(checkX) : checkX;
                checkY = (checkY != second.locationY) ? moveTowardsY(checkY) : checkY;

                var cellToCheck = map.Where(c => c.X == checkX && c.Y == checkY).FirstOrDefault();

                if (!cellToCheck.Transparent)
                {
                    return false;
                }
            } while (second.locationX != checkX || second.locationY != checkY);

            return true;
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

            if (cki.Key != ConsoleKey.Spacebar)
            {
                if (map.isLocationPassable(playerDestinationX, playerDestinationY))
                {

                    renderer.DrawAgent(map, hero, playerDestinationX, playerDestinationY);
                }
                else if (destinationAgent != null)
                {
                    var attackResults = destinationAgent.attackedBy(hero);
                    foreach (var attackMessage in attackResults)
                    {
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, attackMessage));
                    }
                    renderer.DrawAgent(map, hero, hero.locationX, hero.locationY);
                }
                else
                {
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Red, "Ouch! You walk into a wall.");
                    renderer.DrawAgent(map, hero, hero.locationX, hero.locationY);
                }
            } messages.Push(messageToAdd);
        }

        internal void SetUp()
        {
            //draw dungeon
            map = renderer.DrawMap();

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

            agents.Add(hero);
            agents.Add(monster1);
            agents.Add(monster2);
            TakeTurn(new ConsoleKeyInfo());
        }
    }
}
