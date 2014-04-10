using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLCell
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
    }
}
