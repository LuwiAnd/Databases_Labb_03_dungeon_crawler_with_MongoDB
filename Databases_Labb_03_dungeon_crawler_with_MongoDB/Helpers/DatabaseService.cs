using Databases_Labb_03_dungeon_crawler_with_MongoDB.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal class DatabaseService
    {
        public static (IMongoCollection<User>, IMongoCollection<Level>, IMongoCollection<Game>) InitializeDatabase(IMongoDatabase db)
        {
            var users  = db.GetCollection<User>("users");
            var levels = db.GetCollection<Level>("levels");
            var games  = db.GetCollection<Game>("games");

            return (users, levels, games);
        }
    }
}
