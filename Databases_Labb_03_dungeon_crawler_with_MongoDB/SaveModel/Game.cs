using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel
{
    internal class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string LevelId { get; set; }
        public LevelDataState LevelDataState { get; set; } = new();

        //public bool IsCompleted { get; set; }
        public GameStatus GameStatus { get; set; } = GameStatus.Ongoing;


        // Man kan använda DateTime.UtcNow istället, men detta ska
        // bara köras lokalt, så jag väljer att använda lokal tid
        // för att inte bli förvirrad om tiden inte stämmer med min
        // dators klocka.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
