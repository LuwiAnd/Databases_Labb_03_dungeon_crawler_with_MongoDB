using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_02_dungeon_crawler
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
            int sum = this.modifier;
            Random random = new Random();
            for(int i = 0; i < this.numberOfDice; i++)
            {
                sum += random.Next(1, this.sidesPerDice + 1);
            }
            return sum;
        }

        public override string ToString()
        {
            return this.numberOfDice + "d" + this.sidesPerDice + "+" + this.modifier;
        }
    }
}
