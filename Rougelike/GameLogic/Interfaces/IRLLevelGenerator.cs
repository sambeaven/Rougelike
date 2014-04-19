using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic.Interfaces
{
    public interface IRLLevelGenerator
    {
        List<RLAgent> GenerateAgents(Rougelike.GameLogic.RLLevelGenerator.agentGeneratorBehaviour generatorBehaviour);
        RLMap GenerateMap();
    }
}
