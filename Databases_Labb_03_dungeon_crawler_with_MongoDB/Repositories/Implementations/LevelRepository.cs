using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Implementations
{
    internal class LevelRepository : ILevelRepository
    {
        private readonly IMongoCollection<Level> _levels;

        private static readonly Collation _collation =
            new Collation(locale: "sv", strength: CollationStrength.Primary);

        public LevelRepository(IMongoDatabase db)
        {
            _levels = db.GetCollection<Level>("levels");
        }

        public async Task<string> CreateLevelAsync(Level level)
        {
            if (level is null) throw new ArgumentNullException(nameof(level));
            if (string.IsNullOrWhiteSpace(level.Name))
                throw new ArgumentException("Level.Name måste vara ifyllt.", nameof(level));
            if (string.IsNullOrWhiteSpace(level.Layout))
                throw new ArgumentException("Level.Layout måste vara ifyllt.", nameof(level));

            if (await ExistsByNameAsync(level.Name))
                throw new InvalidOperationException($"Level '{level.Name}' finns redan.");

            level.Id ??= ObjectId.GenerateNewId().ToString();
            await _levels.InsertOneAsync(level);
            return level.Id;
        }



        public async Task<Level?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            // Borde jag inte använda FindAsync här? 
            return await _levels.Find(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Level>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await _levels
                    .Find(FilterDefinition<Level>.Empty)
                    .SortBy(l => l.Name)
                    .ToListAsync();
            }

            var pattern = Regex.Escape(name.Trim());
            var regex = new BsonRegularExpression(pattern, "i");
            var filter = Builders<Level>.Filter.Regex(l => l.Name, regex);

            return await _levels
                .Find(filter)
                .SortBy(l => l.Name)
                .ToListAsync();
        }

        public async Task<List<Level>> GetAllAsync()
        {
            return await _levels
                    .Find(FilterDefinition<Level>.Empty)
                    .SortBy(l => l.Name)
                    .ToListAsync();
        }





        public async Task<SaveResult> UpdateAsync(Level level)
        {
            if (level is null) throw new ArgumentNullException(nameof(level));
            if (string.IsNullOrWhiteSpace(level.Id))
                throw new ArgumentException("Level.Id måste vara satt för Update.", nameof(level));
            if (string.IsNullOrWhiteSpace(level.Name))
                throw new ArgumentException("Level.Name måste vara ifyllt.", nameof(level));
            if (string.IsNullOrWhiteSpace(level.Layout))
                throw new ArgumentException("Level.Layout måste vara ifyllt.", nameof(level));

            var filter = Builders<Level>.Filter.Eq(l => l.Id, level.Id);
            var options = new ReplaceOptions { IsUpsert = true };

            var result = await _levels.ReplaceOneAsync(filter, level, options);
            bool created = result.UpsertedId != null || result.ModifiedCount == 0;

            return new SaveResult(
                Id: level.Id,
                Created: created,
                ModifiedCount: result.ModifiedCount
            );
        }

        public async Task<int> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return 0;

            var result = await _levels.DeleteOneAsync(l => l.Id == id);
            return (int)result.DeletedCount;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var trimmed = Regex.Escape(name.Trim());
            var regex = new BsonRegularExpression($"^{trimmed}$", "i");
            var filter = Builders<Level>.Filter.Regex(l => l.Name, regex);
            var options = new CountOptions { Limit = 1 };


            var count = await _levels
                .CountDocumentsAsync(filter, options);

            return count > 0;
        }





        // Hjälpfunktioner.

        public async Task ImportLevelsFromFolderAsync()
        {
            string levelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels");

            if (!Directory.Exists(levelsFolder))
                throw new DirectoryNotFoundException($"Hittar inte Levels-mappen: {levelsFolder}");

            foreach (var file in Directory.EnumerateFiles(levelsFolder, "*.txt", SearchOption.TopDirectoryOnly))
            {
                var name = Path.GetFileNameWithoutExtension(file);

                // hoppa över om den redan finns (case-insensitive exakt match)
                if (await ExistsByNameAsync(name))
                    continue;

                var layout = await File.ReadAllTextAsync(file);

                await CreateLevelAsync(new Level
                {
                    Name = name,
                    Layout = layout
                });
            }
        }


    }
}
