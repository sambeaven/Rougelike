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

        public List<RLMonster> monsters = new List<RLMonster>();

        private IRLRenderer renderer;

        public RLMap map;

        private IRLLevelGenerator levelGenerator;

        private IJsonGameIOService ioService;

        private RLPlayerActionsService playerActionsService;

        private RLAIService aiService;

        public RLHero hero;
        internal const string DESTINATION_IMPASSABLE = "Ouch! You walk into a wall.";

        public RLDice dice;

        [JsonConstructor]
        public RLGame(Interfaces.IRLRenderer renderer = null, Interfaces.IRLLevelGenerator levelGenerator = null,
            IJsonGameIOService jsonService = null, RLMap map = null, List<RLMonster> monsters = null,
            Stack<Tuple<ConsoleColor, string>> messages = null, RLPlayerActionsService playerActionsService = null,
            RLAIService aiService = null, RLHero hero = null, RLDice injectedDice = null)
        {

            this.renderer = renderer != null ? renderer : new RLRenderer();
            this.levelGenerator = levelGenerator != null ? levelGenerator : new RLLevelGenerator();
            this.ioService = jsonService != null ? jsonService : new JsonGameIOService();
            this.map = map;
            this.monsters = monsters != null ? monsters : new List<RLMonster>();
            this.messages = messages != null ? messages : new Stack<Tuple<ConsoleColor, string>>();
            this.playerActionsService = playerActionsService != null ? playerActionsService : new RLPlayerActionsService();
            this.dice = injectedDice != null ? injectedDice : new RLDice(); 
            this.aiService = aiService != null ? aiService : new RLAIService(dice);
            this.hero = hero != null ? hero : this.levelGenerator.GetDefaultHero();
        }

        /// <summary>
        /// Evaluates a turn, handling movement and combat, and removes any dead agents.
        /// </summary>
        /// <param name="cki">A consolekeyinfo object representing the last button press</param>
        /// <returns>a bool representing whether or not the game has ended. True means gameOver, false means the game will continue.</returns>
        public bool ProcessInput(RLPlayerAction action)
        {
            //movement
            PlacePlayer(map, action);
            foreach (var agent in monsters)
            {
                PlaceMonster(map, agent);
            }

            //remove dead agents
            foreach (var agent in monsters)
            {
                if (agent.HitPoints <= 0)
                {
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.DarkYellow, agent.Name + " died."));
                    map.Cells.Where(c => c.X == agent.locationX && c.Y == agent.locationY).FirstOrDefault().Unoccupied = true;
                }
            }

            monsters = monsters.Where(a => a.HitPoints > 0).ToList();


            renderer.PostMessages(messages);

            if (hero.HitPoints == 0)
            {
                return true;
            }
            return false;
        }

        public bool SaveGame()
        {
            return ioService.SaveGame(this);
        }

        public void LoadGame()
        {
            RLGame loadedGame = ioService.LoadGame();
            this.map = loadedGame.map;
            this.levelGenerator = loadedGame.levelGenerator;
            this.messages = loadedGame.messages;
            this.renderer = loadedGame.renderer;
            this.ioService = loadedGame.ioService;
            this.monsters = loadedGame.monsters;
            this.messages = loadedGame.messages;
            this.playerActionsService = loadedGame.playerActionsService;
            this.aiService = loadedGame.aiService;
            this.hero = loadedGame.hero;
            this.dice = loadedGame.dice;
        }

        public RLPlayerAction GetActionFromInput(ConsoleKeyInfo keyinfo)
        {
            return playerActionsService.GetActionFromInput(keyinfo);
        }

        private void PlaceMonster(RLMap map, RLMonster monster)
        {
            Tuple<int, int> destination = null;

            //if monster can see hero, move according to monster behaviour
            if (aiService.CanSeeEachOther(monster, hero, map))
            {
                switch (monster.monsterBehaviour)
                {
                    case RLMonster.MonsterBehaviour.aggressive:
                        destination = aiService.moveTowards(monster, hero);
                        break;
                    case RLMonster.MonsterBehaviour.passive:
                        destination = aiService.moveRandom(monster);
                        break;
                    case RLMonster.MonsterBehaviour.cowardly:
                        destination = aiService.moveAway(monster, hero);
                        break;
                    default:
                        break;
                }
            }
            else //otherwise, move at random
            {
                destination = aiService.moveRandom(monster);
            }

            if (destination != null)
            {
                if (destination.Item1 == hero.locationX && destination.Item2 == hero.locationY)
                {
                    //redraw in the same location
                    renderer.DrawAgent(map, monster, monster.locationX, monster.locationY);
                    //attack hero
                    var attackResults = hero.attackedBy(monster, dice);
                    foreach (var attackMessage in attackResults)
                    {
                        messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, attackMessage));
                    }
                }
                else if (map.isLocationPassable(destination.Item1, destination.Item2))
                {
                    renderer.DrawAgent(map, monster, destination.Item1, destination.Item2);
                    monster.locationX = destination.Item1;
                    monster.locationY = destination.Item2;
                }
                else
                {
                    //redraw in the same location
                    renderer.DrawAgent(map, monster, monster.locationX, monster.locationY);
                }
            }
        }

        private void PlacePlayer(GameLogic.RLMap map, RLPlayerAction action)
        {
            int playerDestinationX, playerDestinationY;

            playerDestinationX = hero.locationX;
            playerDestinationY = hero.locationY;

            var messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.White, "");

            switch (action)
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
                    LoadGame();
                    playerDestinationX = hero.locationX;
                    playerDestinationY = hero.locationY;
                    renderer.DrawAgent(map, hero, hero.locationX, hero.locationY);
                    break;
                default:
                    break;
            }

            var destinationAgent = monsters
                                    .Where(a => a.locationX == playerDestinationX && a.locationY == playerDestinationY)
                                    .Where(a => a.GetType() != typeof(RLHero))
                                    .FirstOrDefault();


            if (map.isLocationPassable(playerDestinationX, playerDestinationY)
                || (hero.locationY == playerDestinationY && hero.locationX == playerDestinationX))
            {

                renderer.DrawAgent(map, hero, playerDestinationX, playerDestinationY);
                hero.locationX = playerDestinationX;
                hero.locationY = playerDestinationY;
            }
            else if (destinationAgent != null)
            {
                var attackResults = destinationAgent.attackedBy(hero, dice);
                foreach (var attackMessage in attackResults)
                {
                    messages.Push(new Tuple<ConsoleColor, string>(ConsoleColor.Red, attackMessage));
                }
                renderer.DrawAgent(map, hero, hero.locationX, hero.locationY);
            }
            else
            {
                messageToAdd = new Tuple<ConsoleColor, string>(ConsoleColor.Red, DESTINATION_IMPASSABLE);
                renderer.DrawAgent(map, hero, hero.locationX, hero.locationY);
            }
            messages.Push(messageToAdd);
        }

        internal void SetUp()
        {
            //map = levelGenerator.DrawMap
            //agents = levelGenerator.GenerateAgents(agentGenerationBehaviour.IncludeHero)
            //  Once we get on to additional levels, we can call GenerateAgents with ExcludeHero set


            if (map == null || map.Cells.Count == 0)
            {
                map = levelGenerator.GenerateMap();
            }

            //draw dungeon
            renderer.DrawMap(map);

            if (monsters == null || monsters.Count == 0)
            {
                monsters = levelGenerator.GenerateMonsters();
            }

            ProcessInput(RLPlayerAction.EmptyAction);
        }
    }
}
