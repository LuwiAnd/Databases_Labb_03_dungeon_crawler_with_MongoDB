
// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
//using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameObjects;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Implementations;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Driver;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Labb_02_dungeon_crawler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            

            //var smallChoice = MenuViewer.View(new List<string> { "aaa", "bbbb", "ccccc" });
            //Console.WriteLine("You choose:");
            //Console.WriteLine(smallChoice);
            //Thread.Sleep(1000);

            //var bigChoice = MenuViewer.View(new List<string> { 
            //    "aaa", 
            //    "bbbb", 
            //    "ccccc",
            //    "dddddd",
            //    "eeeeeee",
            //    "ffffffff",
            //    "ggggggggg",
            //    "hhhhhhhhhh",
            //});
            //Console.WriteLine("You choose:");
            //Console.WriteLine(bigChoice);


            // Först ska jag kolla om det finns en databas.
            // Om det inte finns ska jag skapa en.
            // - sen ska användaren skriva in sitt namn.
            // - sen välja bana
            // - sen börjar spelet
            // - efter varje drag man gör ska spelet sparas
            // Om det finns en databas:
            // - Låt användaren välja att fortsätta det senaste spelet
            // - eller ladda in ett annat pågående spel
            // - eller ta bort ett pågående spel 
            // - eller starta ett nytt spel:
            // - - sen skriva in sitt namn
            // - - sen välja bana
            // - - sen börjar spelet

            //var dbClient = new MongoClient("localhost:27017");
            var dbClient = new MongoClient("mongodb://localhost:27017");
            string dbName = "LudwigAndersson";
            var db = dbClient.GetDatabase(dbName);

            IUserRepository userRepo = new UserRepository(db);
            ILevelRepository levelRepo = new LevelRepository(db);
            IGameRepository gameRepo = new GameRepository(db);

            ////(IMongoCollection<User> users, IMongoCollection<Level> levels, IMongoCollection<Game> games) = DatabaseService.InitializeDatabase(db);
            //var (users, levels, games) = DatabaseService.InitializeDatabase(db);

            //string levelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels");
            await levelRepo.ImportLevelsFromFolderAsync();

            await DatabaseService.AddDemoData(userRepo, gameRepo, levelRepo);



            User? selectedUser = null;
            Level? selectedLevel = null;
            Game? selectedGame = null;
            LevelData? currentLevelData = null;

            // Loop för hela mitt program som kommer att köras tills programmet avslutas.
            while (true)
            {
                if(selectedUser == null)
                {
                    selectedLevel = null;
                    selectedGame = null;
                    currentLevelData = null;
                }
                else if(selectedLevel == null)
                {
                    selectedGame = null;
                    currentLevelData = null;
                }else if(selectedGame == null)
                {
                    currentLevelData = null;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.WriteLine("Dungeon Crawler (MongoDB) — Huvudmeny");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine();

                ////bool userCollectionExists = db.ListCollectionNames().ToList().Contains("users");
                ////bool levelCollectionExists = db.ListCollectionNames().ToList().Contains("levels");
                ////bool gameCollectionExists = db.ListCollectionNames().ToList().Contains("games");

                //bool userCollectionHasData  = db.GetCollection< User>( "users").Find(FilterDefinition< User>.Empty).Any();
                //bool levelCollectionHasData = db.GetCollection<Level>("levels").Find(FilterDefinition<Level>.Empty).Any();
                //bool gameCollectionHasData  = db.GetCollection< Game>( "games").Find(FilterDefinition< Game>.Empty).Any();

                ////bool levelCollectionHasData = db.ListCollectionNames().ToList().Contains("levels");
                if(selectedUser == null)
                {
                    bool userCollectionHasData = await userRepo.HasElements();
                    if (userCollectionHasData)
                    {
                        int? optionTmp = null;
                        List<string> userNewOldOptions = new List<string>
                        {
                            "Skapa en ny användare.",
                            "Välj en befintlig användare.",
                            "Visa Highscore-listan.",
                            "Radera en användare och alla dess spel!"
                        };
                        while (optionTmp == null)
                        {
                            //Console.WriteLine("Steg 1. Vad vill du göra nu?");
                            //optionTmp = MenuViewer.View(1, userNewOldOptions, 0);
                            optionTmp = MenuViewer.View(userNewOldOptions, "Steg 1. Vad vill du göra nu?");

                            if (optionTmp != null && optionTmp.Value == 2)
                            {
                                optionTmp = null;

                                await DatabaseService.DisplayHighScore(
                                    games: gameRepo,
                                    levels: levelRepo,
                                    users: userRepo,
                                    user: null
                                );
                            }
                        }
                    

                        Console.WriteLine("Du valde:");
                        Console.WriteLine(userNewOldOptions[optionTmp.Value]);

                        if(optionTmp.Value == 0)
                        {
                            //Console.WriteLine("Skriv in ditt användarnamn (högst 10 tecken)");
                            selectedUser = await DatabaseService.CreateUserAsync(userRepo);
                        }
                        else if(optionTmp.Value == 1)
                        {
                            selectedUser = await DatabaseService.LoadUserAsync(userRepo);
                        }
                        else if(optionTmp.Value == 3)
                        {
                            await DatabaseService.DeleteUserAndTheirGames(userRepo, gameRepo);
                        }
                    }
                    else
                    {
                        selectedUser = await DatabaseService.CreateUserAsync(userRepo);
                    }
                }
                

                //// Jag behöver inte kolla att det finns levels här, för det kollar jag när
                //// jag anropar ImportLevelsFromFolderAsync.
                //bool levelCollectionHasData = await levelRepo.HasElements();
                //if (!levelCollectionHasData)
                //{
                //}

                //while (selectedLevel == null)
                while (currentLevelData == null && selectedUser != null)
                {
                    Console.WriteLine("Vad vill du göra nu?");
                    List<string> tmpOptions = new List<string>
                    {
                        "Starta ett nytt spel.",
                        "Ladda ditt pågående spel (om det inte finns startas ett nytt spel).",
                        "Visa mina rekord."
                    };

                    var tmpSelected = MenuViewer.View(tmpOptions);
                    if (!tmpSelected.HasValue)
                    {
                        // Om man tryckt på Escape, så ska man gå tillbaka
                        // till föregående meny och välja användare på nytt.
                        selectedUser = null; 
                        break;
                    }
                    


                    

                    if (tmpSelected.Value == 0)
                    {
                        //selectedLevel = await DatabaseService.LoadLevelAsync(levelRepo);
                        //if (selectedLevel == null)
                        //{
                        //    Console.WriteLine("Ingen level vald.");
                        //    continue;
                        //}

                        //currentLevelData = new LevelData();
                        //currentLevelData.LoadFromLayout(selectedLevel.Layout);

                        //selectedGame = await GameCreationHelper.CreateAndSaveNewGameAsync(
                        //    gameRepository: gameRepo,
                        //    userId: selectedUser!.Id,
                        //    levelId: selectedLevel.Id,
                        //    levelDataState: new Databases_Labb_03_dungeon_crawler_with_MongoDB.States.LevelDataState(currentLevelData)
                        //);

                        var result = await GameCreationHelper.StartNewGameAsync(levelRepo, gameRepo, selectedUser!);
                        selectedGame = result.Game;
                        currentLevelData = result.LevelData;

                        if (selectedGame == null || currentLevelData == null) continue;
                    }
                    else if (tmpSelected.Value == 1)
                    {
                        selectedGame = await DatabaseService.LoadGameAsync(gameRepo, selectedUser!);

                        if (selectedGame == null)
                        {
                            Console.WriteLine("Inget pågående spel hittades. Startar nytt istället.");
                            Thread.Sleep(3000);
                            var result = await GameCreationHelper.StartNewGameAsync(
                                levelRepo, 
                                gameRepo, 
                                selectedUser!
                            );
                            selectedGame = result.Game;
                            currentLevelData = result.LevelData;

                            if (selectedGame == null || currentLevelData == null) continue;
                        }

                        currentLevelData = new LevelData(selectedGame.LevelDataState);
                    }
                    else if(tmpSelected.Value == 2)
                    {
                        await DatabaseService.DisplayHighScore(
                            games: gameRepo,
                            levels: levelRepo,
                            users: userRepo,
                            user: selectedUser
                        );
                    }
                    else
                    {
                        // Testkod som kontrollerar att jag inte lagt till något val och glömt att uppdatera min kod.
                        Console.WriteLine("Ogiltigt val. Uppdatera koden!");
                    }
                }

                if (currentLevelData == null) continue;

                currentLevelData.RenderInitialFrame();
                selectedGame!.LevelDataState = new LevelDataState(currentLevelData);
                selectedGame.GameStatus = currentLevelData.GameStatus;
                await gameRepo.UpdateAsync(selectedGame);

                bool continueToPlay = await GameLoop.PlayGameAsync(
                    currentLevelData,
                    async ld =>
                    {
                        selectedGame!.LevelDataState = new LevelDataState(ld);
                        selectedGame.GameStatus = ld.GameStatus;
                        
                        if(ld.GameStatus != GameStatus.Ongoing)
                        {
                            selectedGame.Score = ld.ComputeScore();
                            selectedGame.CompletedAt = DateOnly.FromDateTime(DateTime.Today);
                        }

                        await gameRepo.UpdateAsync(selectedGame);
                    }
                );

                if (continueToPlay)
                {
                    Console.BackgroundColor = ConsoleColor.Black; // Jag tror inte att denna behövs, men använder för säkerhets skull.
                    Console.Clear();
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine($"Status: {currentLevelData.GameStatus}");
                    Console.WriteLine($"Poäng:  {currentLevelData.ComputeScore()}");
                    Console.WriteLine();
                    Console.WriteLine("Tryck valfri tangent för att återgå till menyn...");
                    Console.ReadKey(true);
                }
                

                selectedUser = null;
                selectedLevel = null;
                selectedGame = null;
                currentLevelData = null;


            }



            //Console.WriteLine("Player: Luwi");
            //Console.WriteLine("Enemy: ????");

            //LevelData levelData = new LevelData();
            //levelData.Load("C:\\Users\\ludwi\\source\\repos\\Labb_02_dungeon_crawler\\LevelElement\\bin\\Debug\\net8.0\\Level1.txt");
            //levelData.TurnsUntilClearingMessages = 3;

            
        }

        
    }
}