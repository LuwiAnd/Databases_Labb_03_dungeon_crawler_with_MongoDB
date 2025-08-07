using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel
{
    internal class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string LevelId { get; set; }

        public bool IsCompleted { get; set; }

        public LevelDataState LevelData { get; set; } = new();


        
    }
}
