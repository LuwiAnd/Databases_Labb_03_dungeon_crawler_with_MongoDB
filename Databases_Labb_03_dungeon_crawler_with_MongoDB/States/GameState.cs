using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    internal class GameState
    {
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.Now;

        public LevelDataState LevelData { get; set; } = new();
    }
}
