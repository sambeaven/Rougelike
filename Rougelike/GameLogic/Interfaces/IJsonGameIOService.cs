using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rougelike.Interfaces
{
    public interface IJsonGameIOService
    {
        bool SaveGame(GameLogic.RLGame game);

        GameLogic.RLGame LoadGame();
    }
}
