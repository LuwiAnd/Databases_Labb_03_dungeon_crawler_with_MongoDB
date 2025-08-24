using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces
{
    internal interface IUserRepository
    {
        public Task<bool> HasElements();
        public Task<User> CreateUserAsync(User user);
        public Task<User?> GetByIdAsync(string id);
        public Task<List<User>> GetByNameAsync(string name);
        public Task<List<User>> GetAllAsync();
        public Task<SaveResult> UpdateAsync(User user);
        public Task<int> DeleteAsync(User user);
        public Task<bool> ExistsByNameAsync(string name);
    }
}
