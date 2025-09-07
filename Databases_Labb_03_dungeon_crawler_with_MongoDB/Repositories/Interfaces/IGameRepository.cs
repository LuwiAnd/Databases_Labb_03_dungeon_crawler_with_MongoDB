using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces
{
    internal interface IGameRepository
    {
        public Task<bool> HasElements(User user, Level level);
        Task CreateAsync(Game game);
        //Task<string> SaveAsync(Game game);
        Task<Game?> GetByIdAsync(string id);
        Task<List<Game>> GetAllAsync();
        Task<bool> DeleteAsync(string id);
        Task<long> DeleteByUserAsync(User user);
        //Task<bool> UpdateAsync(Game game);
        Task<SaveResult> UpdateAsync(Game game);
    }
}
