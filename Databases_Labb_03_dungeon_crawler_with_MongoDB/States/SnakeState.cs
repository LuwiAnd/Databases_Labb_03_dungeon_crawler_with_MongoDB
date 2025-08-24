using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator] // Testar att inte skriva klassnamnet (jämför med WallState).
    internal class SnakeState : EnemyState
    {
        //public Position Position { get; set; } = new Position();
        //public double HP { get; set; } // Denna ska bara finnas i EnemyState för att inte orsaka en krock.

        public SnakeState() { }

        public SnakeState(Snake snake)
        {
            Type = "snake";
            Position = snake.Position;
            HP = snake.HP;
        }
    }
}
