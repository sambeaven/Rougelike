using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLMap : List<RLCell>
    {

        public const char MAP_WALL = '#';
        public const char MAP_FLOOR = '.';

        public RLMap(int mapWidth = 50, int mapHeight = 20)
        {
            this.MaxHeight = mapHeight;
            this.MaxWidth = mapWidth;
            GenerateBoxMap(mapWidth, mapHeight);
        }


        private void GenerateBoxMap(int mapWidth, int mapHeight)
        {
            //outer boundraries are walls
            //inner boundraries are floors
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    RLCell cell = new RLCell();

                    if (x == 0 || x == mapWidth - 1 || x == 20 && y < 15 || y == 0 || y == mapHeight - 1)
                    {
                        cell.DisplayCharacter = MAP_WALL;
                        cell.Passable = false;
                        cell.Transparent = false;
                        cell.Unoccupied = true;
                    }
                    else
                    {
                        cell.DisplayCharacter = MAP_FLOOR;
                        cell.Passable = true;
                        cell.Transparent = true;
                        cell.Unoccupied = true;
                    }

                    cell.X = x;
                    cell.Y = y;

                    if (x == 25 && y == 10)
                    {
                        cell.Items.Add(new RLItem() { DisplayChar = '%', DisplayColor = ConsoleColor.Yellow, Name = "Candlestick" });
                    }

                    this.Add(cell);
                }
            }
        }

        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }

        public bool isLocationPassable(int x, int y)
        {
            var destCell = this.Where(c => c.X == x && c.Y == y).FirstOrDefault();

            if (destCell != null)
            {
                return destCell.Passable && destCell.Unoccupied;
            }
            return false;
        }
    }
}
