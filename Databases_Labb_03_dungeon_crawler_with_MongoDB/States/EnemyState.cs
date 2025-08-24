using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator]
    internal abstract class EnemyState : LevelElementState
    {
        //public Position Position { get; set; } = new Position(); // Finns i LevelElementState.
        public double HP { get; set; }

        protected EnemyState() { }

        protected EnemyState(Position position, double hp)
        {
            // Jag är osäker på om jag här bör skriva:
            // Type = "enemy";
            Position = position;
            HP = hp;
        }
    }
}
