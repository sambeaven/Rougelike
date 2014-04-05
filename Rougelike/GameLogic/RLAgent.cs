using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLAgent
    {
        public string Name { get; set; }
        public int locationX { get; set; }
        public int locationY { get; set; }
        public char DisplayChar { get; set; }

        public int HitPoints { get; set; }

        /// <summary>
        /// Used to determine damage. 1-100
        /// </summary>
        
        public int Strength { get; set; }
        /// <summary>
        /// used to determine initiative and hit rolls. 1-100
        /// </summary>
        public int Dexterity { get; set; }
        
        /// <summary>
        /// Used to determine defense. 1-100
        /// </summary>
        public int Constitution { get; set; }

        public RLAgent(int locationX, int locationY, char displayChar, int hitPoints, int strength, int dexterity, string name, int constitution)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.DisplayChar = displayChar;
            this.HitPoints = hitPoints;
            this.Strength = strength;
            this.Dexterity = dexterity;
            this.Name = name;
            this.Constitution = constitution;
        }

        /// <summary>
        /// Resolves attacks. Pass attacker into the target.
        /// </summary>
        /// <param name="attacker">the RLAgent that is attacking</param>
        /// <returns>A List of Strings describing what happened in combat</returns>
        public List<string> attackedBy(RLAgent attacker)
        {
            var messages = new List<string>();

            Random d100 = new Random();

            messages.Add(attacker.Name + " attacks " + this.Name + "!");

            //Roll to hit - 1-100 + Dex for each. Highest wins.
            int attackRoll = d100.Next(1, 100) + attacker.Dexterity;
            int defenseRoll = d100.Next(1, 100) + this.Dexterity;

            if (attackRoll > defenseRoll)
            {
                //if hit, roll for damage = 1-100 + Str for attacker vs 1-100 + Con for defender
                int attackWoundRoll = d100.Next(1, 100) + attacker.Strength;
                int defenseWouldRoll = d100.Next(1, 100) + this.Constitution;
                //difference (if positive) is damage to defender's hitpoints

                if (attackWoundRoll > defenseWouldRoll)
                {
                    int damage = attackWoundRoll - defenseWouldRoll;
                    this.HitPoints = this.HitPoints - damage;

                    messages.Add(attacker.Name + " hits " + this.Name + " for " + damage + " points!");
                }
                else
                {
                    messages.Add(this.Name + " shrugs off the blow!");
                }

            }
            else
            {
                //If defender wins, return with 'Parry' message
                messages.Add(this.Name + " parries the attack!");
            }

            return messages;
        }
    }
}
