using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    public class Dice
    {
        private int numberOfDice;
        private int sidesPerDice;
        private int modifier;

        public Dice(int numberOfDice, int sidesPerDice, int modifier)
        {
            this.numberOfDice = numberOfDice;
            this.sidesPerDice = sidesPerDice;
            this.modifier = modifier;
        }

        public int Throw()
        {
            int sum = modifier;
            Random random = new Random();
            for (int i = 0; i < numberOfDice; i++)
            {
                sum += random.Next(1, sidesPerDice + 1);
            }
            return sum;
        }

        public override string ToString()
        {
            return numberOfDice + "d" + sidesPerDice + "+" + modifier;
        }
    }
}
