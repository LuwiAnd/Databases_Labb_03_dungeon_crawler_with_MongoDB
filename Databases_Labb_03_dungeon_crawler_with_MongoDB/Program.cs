
// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
//using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameObjects;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Implementations;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
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

            //string? username;
            //string? level;

            User? selectedUser = null;
            Level? selectedLevel = null;
            Game? selectedGame = null;
            LevelData? currentLevelData = null;

            // Loop för hela mitt program som kommer att köras tills programmet avslutas.
            while (true)
            {
                selectedUser = null;
                selectedLevel = null;
                selectedGame = null;
                currentLevelData = null;

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

                bool userCollectionHasData = await userRepo.HasElements();
                if (userCollectionHasData)
                {
                    int? optionTmp = null;
                    List<string> userNewOldOptions = new List<string>
                    {
                        "Skapa en ny användare.",
                        "Välj en befintlig användare."
                    };
                    while (optionTmp == null)
                    {
                        Console.WriteLine("Vill du skapa en ny användare eller välja en befintlig?");
                        optionTmp = MenuViewer.View(userNewOldOptions);
                    }
                    

                    Console.WriteLine("Du valde:");
                    Console.WriteLine(userNewOldOptions[optionTmp.Value]);

                    if(optionTmp.Value == 0)
                    {
                        //Console.WriteLine("Skriv in ditt användarnamn (högst 10 tecken)");
                        selectedUser = await DatabaseService.CreateUserAsync(userRepo);
                    }
                    else
                    {
                        selectedUser = await DatabaseService.LoadUserAsync(userRepo);
                    }
                }
                else
                {
                    selectedUser = await DatabaseService.CreateUserAsync(userRepo);
                }

                //// Jag behöver inte kolla att det finns levels här, för det kollar jag när
                //// jag anropar ImportLevelsFromFolderAsync.
                //bool levelCollectionHasData = await levelRepo.HasElements();
                //if (!levelCollectionHasData)
                //{
                //}

                //while (selectedLevel == null)
                while (currentLevelData == null)
                {
                    Console.WriteLine("Vad vill du göra nu?");
                    List<string> tmpOptions = new List<string>
                    {
                        "Starta ett nytt spel.",
                        "Ladda ditt pågående spel (om det inte finns startas ett nytt spel)"
                    };

                    var tmpSelected = MenuViewer.View(tmpOptions);
                    if (!tmpSelected.HasValue) continue;

                    //switch (tmpSelected)
                    //{
                    //    case 0:
                    //        Console.WriteLine("Du valde att starta ett nytt spel.");
                    //        selectedLevel = await DatabaseService.LoadLevelAsync(levelRepo);
                    //        break;
                    //    case 1:
                    //        Console.WriteLine("Du valde att ladda ett pågående spel.");
                    //        selectedGame = await DatabaseService.LoadGameAsync(gameRepo, selectedUser);
                    //        break;
                    //    default:
                    //        Console.WriteLine("Något gick fel. Du valde ett alternativ som inte finns.");
                    //        break;
                    //}


                    

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
                            Console.WriteLine("Inget pågående spel hittades. Startar nytt i stället.");
                            var result = await GameCreationHelper.StartNewGameAsync(levelRepo, gameRepo, selectedUser!);
                            selectedGame = result.Game;
                            currentLevelData = result.LevelData;

                            if (selectedGame == null || currentLevelData == null) continue;
                        }

                        currentLevelData = new LevelData(selectedGame.LevelDataState);
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val.");
                    }
                }

                currentLevelData.RenderInitialFrame();
                await GameLoop.PlayGameAsync(
                    currentLevelData,
                    async ld =>
                    {
                        selectedGame!.LevelDataState = new Databases_Labb_03_dungeon_crawler_with_MongoDB.States.LevelDataState(ld);
                        selectedGame.GameStatus = ld.GameStatus;
                        
                        await gameRepo.UpdateAsync(selectedGame);
                    }
                );

                Console.Clear();
                Console.SetCursorPosition(0, 3);
                Console.WriteLine($"Status: {currentLevelData.GameStatus}");
                Console.WriteLine($"Poäng:  {currentLevelData.ComputeScore()}");
                Console.WriteLine();
                Console.WriteLine("Tryck valfri tangent för att återgå till menyn...");
                Console.ReadKey(true);

            }



            //Console.WriteLine("Player: Luwi");
            //Console.WriteLine("Enemy: ????");

            //LevelData levelData = new LevelData();
            //levelData.Load("C:\\Users\\ludwi\\source\\repos\\Labb_02_dungeon_crawler\\LevelElement\\bin\\Debug\\net8.0\\Level1.txt");
            //levelData.TurnsUntilClearingMessages = 3;

            
        }

        
    }
}