using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson.Serialization.Attributes;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator]
    internal class HeroState : LevelElementState
    {
        //public Position Position { get; set; } = new Position(); // Finns redan i LevelElementState.
        public double HP { get; set; }

        public HeroState() { }

        public HeroState(Hero hero)
        {
            Type = "hero";
            Position = hero.Position;
            HP = hero.HP;
        }
    }
}
