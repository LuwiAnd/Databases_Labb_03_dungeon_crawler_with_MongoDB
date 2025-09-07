using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel
{
    // Id har typen string och inte ObjectId för att det ska gå att byta från MongoDB till SQL.
    internal class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("name")]
        public string? Name { get; set; } = "";

        // Jag tror att jag endast använder CreatedAt för Game-objekt, men kanske intressant att spara här också.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User()
        {

        }

    }
}
