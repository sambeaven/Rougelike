using Rougelike.GameLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rougelike.GameLogic
{
    public class RLAIService
    {
        private IRLDice _dice;

        public RLAIService(IRLDice dice = null)
        {
            _dice = dice != null ? dice : new RLDice();
        }

        public Tuple<int, int> moveRandom(RLAgent agent)
        {
            Random randomDirection = new Random();

            int direction = randomDirection.Next(1, 5);
            int x = agent.locationX, y = agent.locationY;

            switch (direction)
            {
                case 1:
                    //north
                    y = agent.locationY - 1;
                    break;
                case 2:
                    //south
                    y = agent.locationY + 1;
                    break;
                case 3:
                    //east
                    x = agent.locationX - 1;
                    break;
                case 4:
                    //west
                    x = agent.locationX + 1;
                    break;
                default:
                    break;
            }

            return new Tuple<int, int>(x, y);
        }

        public Tuple<int, int> moveAway(RLMonster monster, RLHero hero)
        {
            //work out differences to find the axis with the greatest difference. We'll move along that axis.

            int x = monster.locationX, y = monster.locationY;

            int xDiff = monster.locationX - hero.locationX;
            int yDiff = monster.locationY - hero.locationY;

            if (xDiff < 0)
            {
                xDiff = xDiff * -1;
            }

            if (yDiff < 0)
            {
                yDiff = yDiff * -1;
            }

            if (xDiff >= yDiff)
            {
                if (hero.locationX > monster.locationX)
                {
                    x--;
                }
                else
                {
                    x++;
                }
            }
            else
            {
                if (hero.locationY > monster.locationY)
                {
                    y--;
                }
                else
                {
                    y++;
                }
            }

            return new Tuple<int, int>(x, y);
        }

        public Tuple<int, int> moveTowards(RLMonster monster, RLHero hero)
        {
            //work out differences to find the axis with the greatest difference. We'll move along that axis.

            int x = monster.locationX, y = monster.locationY;

            int xDiff = monster.locationX - hero.locationX;
            int yDiff = monster.locationY - hero.locationY;

            if (xDiff < 0)
            {
                xDiff = xDiff * -1;
            }

            if (yDiff < 0)
            {
                yDiff = yDiff * -1;
            }

            if (xDiff >= yDiff)
            {
                if (hero.locationX > monster.locationX)
                {
                    x++;
                }
                else
                {
                    x--;
                }
            }
            else
            {
                if (hero.locationY > monster.locationY)
                {
                    y++;
                }
                else
                {
                    y--;
                }
            }

            return new Tuple<int, int>(x, y);
        }

        public bool CanSeeEachOther(RLAgent first, RLAgent second, RLMap map)
        {
            Func<int, int> moveTowardsX = null;
            Func<int, int> moveTowardsY = null;

            if (first.locationX > second.locationX)
            {
                moveTowardsX = x => x - 1;
            }
            else
            {
                moveTowardsX = x => x + 1;
            }

            if (first.locationY > second.locationY)
            {
                moveTowardsY = y => y - 1;
            }
            else
            {
                moveTowardsY = y => y + 1;
            }

            int checkX = first.locationX;
            int checkY = first.locationY;

            do
            {
                checkX = (checkX != second.locationX) ? moveTowardsX(checkX) : checkX;
                checkY = (checkY != second.locationY) ? moveTowardsY(checkY) : checkY;

                var cellToCheck = map.Cells.Where(c => c.X == checkX && c.Y == checkY).FirstOrDefault();

                if (!cellToCheck.Transparent)
                {
                    return false;
                }
            } while (second.locationX != checkX || second.locationY != checkY);

            return true;
        }

    }
}
