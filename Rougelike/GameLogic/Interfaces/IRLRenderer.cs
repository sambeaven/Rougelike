using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic.Interfaces
{
    interface IRLRenderer
    {
        void PostMessages(Stack<Tuple<ConsoleColor, string>> messages);
        void DrawAgent(RLMap map, RLAgent agent, int x, int y);
        GameLogic.RLMap DrawMap();
    }
}
