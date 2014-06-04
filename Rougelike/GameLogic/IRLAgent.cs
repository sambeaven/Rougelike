using System;
namespace Rougelike.GameLogic
{
    interface IRLAgent
    {
        System.Collections.Generic.List<string> attackedBy(RLAgent attacker, Rougelike.GameLogic.Interfaces.IRLDice dice);
        int Constitution { get; set; }
        int Dexterity { get; set; }
        char DisplayChar { get; set; }
        ConsoleColor DisplayColor { get; set; }
        bool Equals(RLAgent other);
        int HitPoints { get; set; }
        int locationX { get; set; }
        int locationY { get; set; }
        string Name { get; set; }
        int Strength { get; set; }
    }
}
