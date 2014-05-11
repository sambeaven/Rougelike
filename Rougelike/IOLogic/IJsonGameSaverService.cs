using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rougelike.IOLogic
{
    interface IJsonGameIOService
    {
        bool SaveGame(GameLogic.RLGame game);

        GameLogic.RLGame LoadGame();
    }
}
