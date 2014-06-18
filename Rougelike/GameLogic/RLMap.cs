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
            emptyMap,
            towerFloor
        }


        public const char MAP_WALL = '#';
        public const char MAP_FLOOR = '.';

        public RLDice dice { get; set; }

        [JsonConstructor]
        public RLMap(MapType mapType, int mapWidth = 79, int mapHeight = 20, List<RLCell> cells = null, RLDice injectedDice = null)
        {
            this.MaxHeight = mapHeight;
            this.MaxWidth = mapWidth;
            this.dice = injectedDice == null ? new RLDice() : injectedDice;
            

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
            else if (mapType == MapType.towerFloor)
            {
                GenerateTowerFloor(mapWidth, mapHeight);
            }

        }

        private void GenerateTowerFloor(int mapWidth, int mapHeight)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    RLCell cell = new RLCell();
                    cell.SetFloor();
                    cell.Unoccupied = true;
                    cell.X = x;
                    cell.Y = y;
                    this.Cells.Add(cell);
                }
            }

            //generate cells


            //outer edges are all walls
            var outerWalls = this.Cells.Where(c => c.X == 0 || c.X == (mapWidth - 1)
                || c.Y == 0 || c.Y == (mapHeight - 1));
            foreach (var cell in outerWalls)
            {

                cell.SetWall();
            }

            //find corridor hub
            int hubCellX = dice.GenerateBetweenTwoNumbers(minimumValue:(mapWidth / 2) - 5, maximumValue: (mapWidth / 2) - 5);
            int hubCellY = dice.GenerateBetweenTwoNumbers(minimumValue:(mapHeight / 2) - 5, maximumValue: (mapHeight / 2) - 5);

            var hubCell = this.Cells.Where(c => c.X == hubCellX && c.Y == hubCellY).FirstOrDefault();

            hubCell.SetFloor();


            //make a corridor going north/south, with walls at the edges.
            var corridorWalls = this.Cells
                                    .Where(c => c.X == hubCell.X - 1 || c.X == hubCell.X + 1)
                                    .Where(c => c.Y != hubCell.Y)
                                    .ToList();

            //make another corridor going east/west
            corridorWalls.AddRange(this.Cells
                                       .Where(c => c.Y == hubCell.Y - 1 || c.Y == hubCell.Y + 1)
                                       .Where(c => c.X != hubCell.X));
            
            foreach (var cell in corridorWalls)
            {
                cell.SetWall();
            }

            //punch holes in the corridor walls
            
            //how do I do this? Actually punching the holes through feels messy.
            //What I want to do is skip over a couple of the cells at random.

            //divide up into rooms and punch holes in those as well

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
