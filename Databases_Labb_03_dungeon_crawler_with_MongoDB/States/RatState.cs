using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    internal class RatState : EnemyState
    {
        //public Position Position { get; set; } = new Position();
        public double HP { get; set; }

        public RatState() { }

        public RatState(Rat rat)
        {
            Position = rat.Position;
            HP = rat.HP;
        }
    }
}
