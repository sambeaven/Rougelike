using Newtonsoft.Json;
using Rougelike.GameLogic.Interfaces;
using Rougelike.Interfaces;
using Rougelike.IOLogic;
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

        public RLMap map;

        private IRLLevelGenerator levelGenerator;

        private IJsonGameIOService ioService;

        private RLPlayerActionsService playerActionsService;

        public RLGame()
        {
            messages = new Stack<Tuple<ConsoleColor, string>>();
            renderer = new RLRenderer();
            levelGenerator = new RLLevelGenerator();
            playerActionsService = new RLPlayerActionsService();
        }

        [JsonConstructor]
        public RLGame(RLRenderer renderer = null, Interfaces.IRLLevelGenerator levelGenerator = null,
            IJsonGameIOService jsonService = null, RLMap map = null, List<RLAgent> agents = null,
            Stack<Tuple<ConsoleColor, string>> messages = null, RLPlayerActionsService playerActionsService = null)
        {

            this.renderer = renderer != null ? renderer : new RLRenderer();
            this.levelGenerator = levelGenerator != null ? levelGenerator : new RLLevelGenerator();
            this.ioService = jsonService != null ? jsonService : new JsonGameIOService();
            this.map = map;
            this.agents = agents != null ? agents : new List<RLAgent>();
            this.messages = messages != null ? messages : new Stack<Tuple<ConsoleColor, string>>();
            this.playerActionsService = playerActionsService != null ? playerActionsService : new RLPlayerActionsService();
        }

        /// <summary>
        /// Evaluates a turn, handling movement and combat, and removes any dead agents.
        /// </summary>
        /// <param name="cki">A consolekeyinfo object representing the last button press</param>
        /// <returns>a bool representing whether or not the game has ended. True means gameOver, false means the game will continue.</returns>
        public bool ProcessInput(ConsoleKeyInfo cki)
        {
            renderer.DrawMap(map);

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
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.DarkYellow, agent.Name + " died."));
                    map.Cells.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Unoccupied = true;
                }
            }

            agents = agents.Where(a => a.HitPoints > 0).ToList();


            renderer.PostMessages(messages);

            if (agents.Where(a => a.GetType() == typeof(RLHero)).Count() == 0)
            {
                return true;
            }

            return false;
        }

        public bool SaveGame()
        {
            return ioService.SaveGame(this);
        }

        public static RLGame LoadGame(IJsonGameIOService ioService)
        {
            return ioService.LoadGame();
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

            int direction = randomDirection.Next(1, 5);
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

                var cellToCheck = map.Cells.Where(c => c.X == checkX && c.Y == checkY).FirstOrDefault();

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
            var playerInput = playerActionsService.GetActionFromInput(cki);

            switch (playerInput)
            {
                case RLPlayerAction.EmptyAction:
                    break;
                case RLPlayerAction.MoveUp:
                    playerDestinationY = hero.locationY - 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step north");
                    break;
                case RLPlayerAction.MoveDown:
                    playerDestinationY = hero.locationY + 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step south");
                    break;
                case RLPlayerAction.MoveLeft:
                    playerDestinationX = hero.locationX - 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step west");
                    break;
                case RLPlayerAction.MoveRight:
                    playerDestinationX = hero.locationX + 1;
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Green, "You step east");
                    break;
                case RLPlayerAction.Wait:
                    messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Cyan, "You wait");
                    break;
                case RLPlayerAction.Save:
                    SaveGame();
                    break;
                case RLPlayerAction.Load:
                    //TODO: Figure out how to implement this!
                    //  Is there a wrapper object around game that can replace this?
                    //  Does Game run inside another object?
                    break;
                default:
                    break;
            }

            var destinationAgent = agents
                                    .Where(a => a.locationX == playerDestinationX && a.locationY == playerDestinationY)
                                    .Where(a => a.GetType() != typeof(RLHero))
                                    .FirstOrDefault();


            if (map.isLocationPassable(playerDestinationX, playerDestinationY)
                || (hero.locationY == playerDestinationY && hero.locationX == playerDestinationX))
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
            messages.Push(messageToAdd);
        }

        internal void SetUp()
        {
            //map = levelGenerator.DrawMap
            //agents = levelGenerator.GenerateAgents(agentGenerationBehaviour.IncludeHero)
            //  Once we get on to additional levels, we can call GenerateAgents with ExcludeHero set


            map = levelGenerator.GenerateMap();

            //draw dungeon
            renderer.DrawMap(map);

            agents = levelGenerator.GenerateAgents(RLLevelGenerator.agentGeneratorBehaviour.IncludeHero);

            ProcessInput(new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false));
        }
    }
}
