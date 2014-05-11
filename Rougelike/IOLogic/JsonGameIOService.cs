using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Rougelike.IOLogic
{
    public class JsonGameIOService : Interfaces.IJsonGameIOService
    {
        /// <summary>
        /// Takes a game and serializes it and all its children into a JSON text file
        /// </summary>
        /// <param name="game">The game to save</param>
        /// <returns>A bool indicating success or failure</returns>
        public bool SaveGame(GameLogic.RLGame game)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            
            string sPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                using (StreamWriter sw = new StreamWriter(sPath + "/save.txt"))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(sw, game);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads game from a JSON text file. The text file is stored in this object (there is only ever 1 savegame).
        /// </summary>
        /// <returns>A game based on the contents of the savegame file</returns>
        public GameLogic.RLGame LoadGame()
        {
            throw new NotImplementedException();
        }
    }
}
