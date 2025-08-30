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

            var allGames = await gameRepository.GetAllAsync();

            var ongoingGames = allGames
                .Where(g => g.UserId == userId && g.GameStatus == GameStatus.Ongoing)
                .ToList();

            foreach (var ongoingGame in ongoingGames)
            {
                await gameRepository.DeleteAsync(ongoingGame.Id);
            }


            // För testning:
            ongoingGames = allGames
                .Where(g => g.UserId == userId && g.GameStatus == GameStatus.Ongoing)
                .ToList();
            Console.WriteLine("Nu ska det finnas noll sparade pågående spel för användaren.");
            Console.WriteLine($"Användaren har {ongoingGames.Count} sparade spel!");
            Thread.Sleep(5000);


            var newGame = new Game
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                LevelId = levelId,
                LevelDataState = levelDataState,
                CreatedAt = DateTime.Now,
                GameStatus = GameStatus.Ongoing
            };

            //await gameRepository.SaveAsync(newGame);
            await gameRepository.CreateAsync(newGame);

            return newGame;
        }

        // Från början hade jag endast CreateAndSaveNewGameAsync i denna klass, men 
        // jag märkte att jag gjorde koden i denna funktion två gånger i Program.cs, 
        // så jag skapade denna funktion och flyttade den hit efter ett tag.
        public static async Task<(Game? Game, LevelData? LevelData)> StartNewGameAsync(
            ILevelRepository levelRepo,
            IGameRepository gameRepo,
            User selectedUser
        )
        {
            // Ladda en level
            var selectedLevel = await DatabaseService.LoadLevelAsync(levelRepo);
            if (selectedLevel == null)
            {
                Console.WriteLine("Ingen level vald.");
                return (null, null);
            }

            // Skapa nytt LevelData
            var currentLevelData = new LevelData();
            currentLevelData.LoadFromLayout(selectedLevel.Layout);

            // Skapa nytt Game-objekt
            var createdGame = await CreateAndSaveNewGameAsync(
                gameRepository: gameRepo,
                userId: selectedUser.Id,
                levelId: selectedLevel.Id,
                levelDataState: new Databases_Labb_03_dungeon_crawler_with_MongoDB.States.LevelDataState(currentLevelData)
            );

            return (createdGame, currentLevelData);
        }

    }
}
