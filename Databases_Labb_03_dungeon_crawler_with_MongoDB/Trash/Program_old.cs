
//// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
//using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
////using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameObjects;
//using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
//using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
//using MongoDB.Driver;
//using System.Reflection.Metadata;
//using System.Security.Cryptography;

//namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Trash
//{
//    internal class Program_old
//    {
//        private static void Main(string[] args)
//        {


//            //var smallChoice = MenuViewer.View(new List<string> { "aaa", "bbbb", "ccccc" });
//            //Console.WriteLine("You choose:");
//            //Console.WriteLine(smallChoice);
//            //Thread.Sleep(1000);

//            //var bigChoice = MenuViewer.View(new List<string> { 
//            //    "aaa", 
//            //    "bbbb", 
//            //    "ccccc",
//            //    "dddddd",
//            //    "eeeeeee",
//            //    "ffffffff",
//            //    "ggggggggg",
//            //    "hhhhhhhhhh",
//            //});
//            //Console.WriteLine("You choose:");
//            //Console.WriteLine(bigChoice);


//            // Först ska jag kolla om det finns en databas.
//            // Om det inte finns ska jag skapa en.
//            // - sen ska användaren skriva in sitt namn.
//            // - sen välja bana
//            // - sen börjar spelet
//            // - efter varje drag man gör ska spelet sparas
//            // Om det finns en databas:
//            // - Låt användaren välja att fortsätta det senaste spelet
//            // - eller ladda in ett annat pågående spel
//            // - eller ta bort ett pågående spel 
//            // - eller starta ett nytt spel:
//            // - - sen skriva in sitt namn
//            // - - sen välja bana
//            // - - sen börjar spelet

//            //var dbClient = new MongoClient("localhost:27017");
//            var dbClient = new MongoClient("mongodb://localhost:27017");
//            string dbName = "LudwigAndersson";

//            var db = dbClient.GetDatabase(dbName);

//            //(IMongoCollection<User> users, IMongoCollection<Level> levels, IMongoCollection<Game> games) = DatabaseService.InitializeDatabase(db);
//            var (users, levels, games) = DatabaseService.InitializeDatabase(db);

//            //bool userCollectionExists = db.ListCollectionNames().ToList().Contains("users");
//            //bool levelCollectionExists = db.ListCollectionNames().ToList().Contains("levels");
//            //bool gameCollectionExists = db.ListCollectionNames().ToList().Contains("games");

//            bool userCollectionHasData = db.GetCollection<User>("users").Find(FilterDefinition<User>.Empty).Any();
//            bool levelCollectionHasData = db.GetCollection<Level>("levels").Find(FilterDefinition<Level>.Empty).Any();
//            bool gameCollectionHasData = db.GetCollection<Game>("games").Find(FilterDefinition<Game>.Empty).Any();

//            //bool levelCollectionHasData = db.ListCollectionNames().ToList().Contains("levels");


//            string? username;
//            string? level;
//            if (!userCollectionHasData)
//            {
//                while (true)
//                {
//                    Console.WriteLine("Enter your username, max 10 characters:");
//                    username = Console.ReadLine();

//                    if (username != null && username?.Length <= 10)
//                    {
//                        User currentUser = new User { Name = username };

//                        //users.InsertOne(currentUser);

//                        //try
//                        //{
//                        //    users.InsertOne(currentUser);
//                        //    Console.WriteLine($"User inserted with ID: {currentUser.Id}");
//                        //}
//                        //catch (Exception ex)
//                        //{
//                        //    Console.WriteLine("Fel vid insert:");
//                        //    Console.WriteLine(ex.Message);
//                        //}
//                        DatabaseService.SaveToDb(users, currentUser);


//                        break;
//                    }
//                    else
//                    {
//                        Console.Clear();
//                    }
//                }
//            }

//            string levelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels");
//            if (!levelCollectionHasData)
//            {
//                if (Directory.Exists(levelsFolder))
//                {
//                    var txtFiles = Directory.GetFiles(levelsFolder, "*.txt");

//                    foreach (var filePath in txtFiles)
//                    {
//                        var levelData = new LevelData();
//                        levelData.Load(filePath);
//                        levelData.TurnsUntilClearingMessages = 3;

//                        Level tmpLevel = new Level
//                        {
//                            Name = Path.GetFileNameWithoutExtension(filePath)
//                        };

//                        levels.InsertOne(tmpLevel);
//                    }
//                }
//            }


//            while (true)
//            {
//                Console.WriteLine("What do you want to do now?");
//                List<string> tmpOptions = new List<string>
//                {
//                    "Start a new game.",
//                    "Create a new user.",
//                    "Load a user.",
//                };

//                var tmpSelected = MenuViewer.View(tmpOptions);

//                switch (tmpSelected)
//                {
//                    case 0:
//                        Console.WriteLine("You choose to create a new game.");
//                        DatabaseService.LoadLevels();
//                        break;
//                    case 1:
//                        Console.WriteLine("You choose to create a new user.");
//                        DatabaseService.CreateUser(users);
//                        break;
//                    case 2:
//                        Console.WriteLine("You choose to load an existing user.");
//                        DatabaseService.LoadUser(users);
//                        Console.ReadLine();
//                        break;
//                    default:
//                        Console.WriteLine("Something went wrong. Your selected option is not supported.");
//                        break;
//                }

//            }



//            Console.WriteLine("Player: Luwi");
//            Console.WriteLine("Enemy: ????");

//            //LevelData levelData = new LevelData();
//            //levelData.Load("C:\\Users\\ludwi\\source\\repos\\Labb_02_dungeon_crawler\\LevelElement\\bin\\Debug\\net8.0\\Level1.txt");
//            //levelData.TurnsUntilClearingMessages = 3;

//            bool gameOver = false;
//            while (!gameOver)
//            {
//                if (levelData.TurnsUntilClearingMessages > 0) levelData.TurnsUntilClearingMessages--;
//                else GeneralDungeonFunctions.ClearConsoleMessages();

//                //levelData.hero.Update(levelData.Elements);
//                levelData.Hero.Update(levelData);
//                if (CheckIsHeroDead(levelData.Hero)) break;

//                levelData.UpdateWalls();
//                levelData.UpdateSnakes();
//                if (CheckIsHeroDead(levelData.Hero)) break;
//                levelData.UpdateRats();
//                if (CheckIsHeroDead(levelData.Hero)) break;

//                levelData.hero.Draw();
//                levelData.EraseDeadElements();
//                levelData.RemoveElements();
//            }
//        }

//        private static bool CheckIsHeroDead(Hero hero)
//        {
//            if (hero.HP <= 0)
//            {
//                Console.SetCursorPosition(10, 22);
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("You died! Game over!");
//                Console.ForegroundColor = ConsoleColor.White;
//                //break;
//                return true;
//            }
//            return false;
//        }
//    }
//}
