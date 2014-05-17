using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public enum RLPlayerAction
    {
        EmptyAction,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Wait,
        Save,
        Load
    }

    public class RLPlayerActionsService
    {
        public RLPlayerAction GetActionFromInput(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.UpArrow:
                    return RLPlayerAction.MoveUp;
                case ConsoleKey.DownArrow:
                    return RLPlayerAction.MoveDown;
                case ConsoleKey.LeftArrow:
                    return RLPlayerAction.MoveLeft;
                case ConsoleKey.RightArrow:
                    return RLPlayerAction.MoveRight;
                case ConsoleKey.Spacebar:
                    return RLPlayerAction.Wait;
                case ConsoleKey.L:
                    return RLPlayerAction.Load;
                case ConsoleKey.S:
                    return RLPlayerAction.Save;
                default:
                    return RLPlayerAction.EmptyAction;
            }
        }
    }
}
