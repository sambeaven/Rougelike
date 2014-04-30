using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rougelike.IOLogic
{
    interface IJsonGameIOService
    {
        void SaveGame(GameLogic.RLGame game);

        GameLogic.RLGame LoadGame();
    }
}
