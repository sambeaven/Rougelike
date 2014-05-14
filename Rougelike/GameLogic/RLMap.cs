using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Rougelike.GameLogic
{
    public class RLMap : IEquatable<RLMap>
    {
        public enum MapType
        {
            boxMap,
            emptyMap
        }


        public const char MAP_WALL = '#';
        public const char MAP_FLOOR = '.';

        [JsonConstructor]
        public RLMap(MapType mapType, int mapWidth = 50, int mapHeight = 20, List<RLCell> cells = null)
        {
            this.MaxHeight = mapHeight;
            this.MaxWidth = mapWidth;
            if (cells != null)
            {
                this.Cells = cells;
            }
            else
            {
                this.Cells = new List<RLCell>();
            }

            if (mapType == MapType.boxMap)
            {
                GenerateBoxMap(mapWidth, mapHeight);
            }

        }

        public RLMap(List<RLCell> cells)
        {
            this.Cells = cells;
        }

        public List<RLCell> Cells { get; set; }

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

                    this.Cells.Add(cell);
                }
            }
        }

        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }

        public bool isLocationPassable(int x, int y)
        {
            var destCell = this.Cells.Where(c => c.X == x && c.Y == y).FirstOrDefault();

            if (destCell != null)
            {
                return destCell.Passable && destCell.Unoccupied;
            }
            return false;
        }

        public bool Equals(RLMap other)
        {
            foreach (RLCell otherCell in other.Cells)
            {
                if (this.Cells.Where(c => c.Equals(otherCell)).Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
