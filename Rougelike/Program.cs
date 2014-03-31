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
            DrawMap();

            int playerLocationX = 5;
            int playerLocationY = 10;
            //place player
            PlacePlayer(playerLocationX, playerLocationY);


            ConsoleKeyInfo keyInfo;
            //listen for input and handle moves
            do
            {
                keyInfo = Console.ReadKey(true);
                

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        playerLocationY--;
                        break;
                    case ConsoleKey.DownArrow:
                        playerLocationY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        playerLocationX--;
                        break;
                    case ConsoleKey.RightArrow:
                        playerLocationX++;
                        break;
                    default:
                        break;
                }

                PlacePlayer(playerLocationX, playerLocationY);

            } while (keyInfo.Key != ConsoleKey.Escape);

            //end game
            //Console.ReadLine();
        }

        private static void PlacePlayer(int x, int y)
        {
            DrawMap();
            Console.SetCursorPosition(x, y);
            Console.Write("@");
            Console.SetCursorPosition(0, 20);
        }

        private static void DrawMap(int mapWidth = 10, int mapHeight = 20)
        {
            //outer boundraries are walls
            //inner boundraries are floors
            const string MAP_WALL = "#";
            const string MAP_FLOOR = ".";
            string tileToPrint;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x == 0 || x == mapWidth -1 || y == 0 || y == mapHeight - 1)
                    {
                        tileToPrint = MAP_WALL;
                    }
                    else
                    {
                        tileToPrint = MAP_FLOOR;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.Write(tileToPrint);
                }
            }

        }
    }
}
