using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Implementations
{
    internal class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _collection;

        public GameRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Game>("games");
        }

        public async Task<bool> HasElements(User user, Level level)
        {
            // Detta fungerade inte, för det går inte att använda tupler i Filter.Eq.
            //var filter = Builders<Game>.Filter.Eq(game => (game.UserId, game.LevelId), (user.Id, level.Id));

            var filter = Builders<Game>.Filter.And(
                Builders<Game>.Filter.Eq(game => game.UserId, user.Id),
                Builders<Game>.Filter.Eq(game => game.LevelId, level.Id)
            );
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task CreateAsync(Game game)
        {
            game.Id ??= ObjectId.GenerateNewId().ToString();
            await _collection.InsertOneAsync(game);
        }

        

        public async Task<Game?> GetByIdAsync(string id)
        {
            //var filter = Builders<Game>.Filter.Eq(g => g.Id, ObjectId.Parse(id));
            var filter = Builders<Game>.Filter.Eq(g => g.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Game>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            //var result = await _collection.DeleteOneAsync(g => g.Id == ObjectId.Parse(id));
            var result = await _collection.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }

        

        //public async Task<bool> UpdateAsync(Game game)
        //{
        //    var filter = Builders<Game>.Filter.Eq(g => g.Id, game.Id);
        //    var result = await _collection.ReplaceOneAsync(filter, game);
        //    return result.ModifiedCount > 0;
        //}

        //public async Task<string> UpdateAsync(Game game)
        public async Task<SaveResult> UpdateAsync(Game game)
        {
            game.Id ??= ObjectId.GenerateNewId().ToString();

            var filter = Builders<Game>.Filter.Eq(g => g.Id, game.Id);
            var options = new ReplaceOptions { IsUpsert = true };

            var result = await _collection.ReplaceOneAsync(filter, game, options);

            bool created = result.UpsertedId != null || result.MatchedCount == 0;

            return new SaveResult(game.Id, created, result.ModifiedCount);
        }

    }
}
