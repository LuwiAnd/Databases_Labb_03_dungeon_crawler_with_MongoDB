using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Implementations
{
    internal class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        private static readonly Collation _collation =
            new Collation(locale: "sv", strength: CollationStrength.Primary);



        public UserRepository(IMongoDatabase db)
        {
            _users = db.GetCollection<User>("users");
        }

        public async Task<bool> HasElements()
        {
            //var users = await _users
            //    .Find<User>(FilterDefinition<User>.Empty)
            //    .ToListAsync();
            //
            //return users.Count > 0;

            // Detta gör samma sak som ovanstående, fast utan att hämta hela listan och sen räkna:
            return await _users.CountDocumentsAsync(FilterDefinition<User>.Empty) > 0;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ArgumentException($"User.name måste ha ett värde, inte vara {user.Name}", nameof(user));
            if (await ExistsByNameAsync(user.Name))
                throw new InvalidOperationException($"Användarnamnet '{user.Name}' finns redan.");

            user.Id ??= ObjectId.GenerateNewId().ToString();
            await _users.InsertOneAsync(user);

            return user;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await _users
                    .Find<User>(FilterDefinition<User>.Empty)
                    .SortBy(u => u.Name)
                    .ToListAsync();

            name = name.Trim();

            // Denna kodrad lägger till escape characters framför alla regex-tecken så
            // att användarnamn som har regex-tecken i sig kommer med.
            var pattern = Regex.Escape(name);
            var regex = new BsonRegularExpression(pattern, "i");

            var filter = Builders<User>.Filter.Regex(u => u.Name, regex);

            return await _users
                .Find(filter)
                .SortBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users
                .Find<User>(FilterDefinition<User>.Empty)
                .SortBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<SaveResult> UpdateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Id))
                throw new ArgumentException("UserId måste vara satt för Update.", nameof(user));

            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var options = new ReplaceOptions { IsUpsert = true };

            var result = await _users.ReplaceOneAsync(filter, user, options);

            bool created = result.UpsertedId != null || result.MatchedCount == 0;

            return new SaveResult(
                Id: user.Id,
                Created: created,
                ModifiedCount: result.ModifiedCount
            );

            
        }

        public async Task<int> DeleteAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Id))
                throw new ArgumentException("User.Id måste vara satt för att kunna ta bort user.", nameof(user));

            var res = await _users.DeleteOneAsync(u => u.Id == user.Id);
            return (int)res.DeletedCount; // 0 eller 1, men jag kanske uppdaterar så att man även kan ta bort eventuella dubbletter.
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Du har inte angivit ett värde på name.", nameof(name));

            var filter = Builders<User>.Filter.Eq(u => u.Name, name);
            var options = new CountOptions
            {
                //Collation = new Collation("sv", strength: CollationStrength.Primary),
                Collation = _collation,
                Limit = 1
            };

            var count = await _users.CountDocumentsAsync(filter, options);
            return count > 0;
        }
    }
}
