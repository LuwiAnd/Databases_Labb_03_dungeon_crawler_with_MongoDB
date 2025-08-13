using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces
{
    internal interface ILevelRepository
    {
        public Task<string> CreateLevelAsync(Level level);
        public Task<Level?> GetByIdAsync(string id);
        public Task<List<Level>> GetByNameAsync(string name);
        public Task<List<Level>> GetAllAsync();
        public Task<SaveResult> UpdateAsync(Level level);
        public Task<int> DeleteAsync(string id);
        public Task<bool> ExistsByNameAsync(string name);
    }
}
