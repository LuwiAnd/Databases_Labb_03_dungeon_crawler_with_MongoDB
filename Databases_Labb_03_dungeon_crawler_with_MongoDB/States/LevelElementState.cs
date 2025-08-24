using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WallState), typeof(GoalState), typeof(EnemyState), typeof(RatState), typeof(SnakeState), typeof(HeroState))]
    internal class LevelElementState
    {
        public string Type { get; set; } = "";
        public Position Position { get; set; } = new Position();
    }
}
