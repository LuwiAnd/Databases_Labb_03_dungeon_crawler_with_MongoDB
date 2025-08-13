using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel
{
    internal class Level
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("layout")]
        public string Layout { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
