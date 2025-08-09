using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;

using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal static class GameCreationHelper
    {
        public static async Task<Game> CreateAndSaveNewGameAsync(
            IGameRepository gameRepository,
            string userId,
            string levelId,
            LevelDataState levelDataState
        )
        {
            var game = new Game
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                LevelId = levelId,
                LevelDataState = levelDataState,
                CreatedAt = DateTime.Now,
                GameStatus = GameStatus.Ongoing
            };

            //await gameRepository.SaveAsync(game);
            await gameRepository.CreateAsync(game);

            return game;
        }
    }
}
