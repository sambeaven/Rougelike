using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLCell : IEquatable<RLCell>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Passable { get; set; }

        public bool Unoccupied { get; set; }

        public bool Transparent { get; set; }

        /// <summary>
        /// Refactor this? This feels like display logic that should really be somewhere else.
        /// </summary>
        public char DisplayCharacter { get; set; }

        public List<RLItem> Items { get; set; }

        public RLCell()
        {
            this.Items = new List<RLItem>();
        }


        public bool Equals(RLCell other)
        {
            return (other.X == this.X &&
                    other.Y == this.Y &&
                    other.Passable == this.Passable &&
                    other.Unoccupied == this.Unoccupied &&
                    other.Transparent == this.Transparent &&
                    other.DisplayCharacter == this.DisplayCharacter);
        }

        internal void SetFloor()
        {
            this.DisplayCharacter = RLMap.MAP_FLOOR;
            this.Passable = true;
            this.Transparent = true;
        }

        internal void SetWall()
        {
            this.DisplayCharacter = RLMap.MAP_WALL;
            this.Passable = false;
            this.Transparent = false;
        }
    }
}
