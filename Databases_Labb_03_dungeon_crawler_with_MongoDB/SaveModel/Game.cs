using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel
{
    internal class Game
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string LevelName { get; set; } = string.Empty;

        public int TurnCount { get; set; }
        public HeroState
    }
}
