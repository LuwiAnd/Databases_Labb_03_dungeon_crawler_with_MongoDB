using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Repositories.Interfaces;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal class DatabaseService
    {
        /* Ej asynkron.
        public static (IMongoCollection<User>, IMongoCollection<Level>, IMongoCollection<Game>) InitializeDatabase(IMongoDatabase db)
        {
            var users  = db.GetCollection<User>("users");
            var levels = db.GetCollection<Level>("levels");
            var games  = db.GetCollection<Game>("games");

            return (users, levels, games);
        }
        */

        /* Ej asynkron.
        public static void SaveUserToDb(IMongoCollection<User> users, User user)
        {
            try
            {
                users.InsertOne(user);
                Console.WriteLine($"User inserted with ID: {user.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving user:");
                Console.WriteLine(ex.Message);
            }
        }
        */

        /* Ej asynkron.
        public static void SaveToDb<T>(IMongoCollection<T> collection, T document)
        {
            object? GetId<T>(T doc)
            {
                var idProp = typeof(T).GetProperty("Id");
                return idProp?.GetValue(doc);
            }

            try
            {
                collection.InsertOne(document);
                Console.WriteLine($"Document was inserted with ID: {GetId(document)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving:");
                Console.WriteLine(ex.Message);
            }
        }
        */

        /* Ej asynkron.
        // Den här versionen ska jag inte använda längre, eftersom jag använder repository pattern.
        public static void CreateUser(IMongoCollection<User> users)
        {
            Console.BackgroundColor = ConsoleColor.Black; // Jag tror inte att denna behövs, men använder för säkerhets skull.
            Console.Clear();

            var allUsers = users.Find<User>(FilterDefinition<User>.Empty).ToList();
            //List<string> usernames = allUsers.Select(u => u.Name).ToList();
            var usernames = allUsers.Select(u => u.Name).ToList();

            while (true)
            {
                Console.WriteLine("Enter username:");
                string? username = Console.ReadLine();

                if (usernames.Contains(username))
                {
                    Console.WriteLine($"Username \"{username}\" already exists.");
                    continue;
                }

                
                if(username != null && username.Length <= 10)
                {
                    SaveToDb<User>(users, new User { Name = username });
                    break;
                }
                else
                {
                    Console.WriteLine($"Username \"{username}\" was either not 1 - 10 characters or missing.");
                }
            }

        }
        */


        //public static async Task<User> CreateUserAsync(IUserRepository users)
        public static async Task<User?> CreateUserAsync(IUserRepository users)
        {
            Console.BackgroundColor = ConsoleColor.Black; // Jag tror inte att denna behövs, men använder för säkerhets skull.
            Console.Clear();

            var usernames = (await users.GetAllAsync())
                .Where(u => !string.IsNullOrWhiteSpace(u.Name))
                .Select(u => u.Name)
                .ToList();

            User? createdUser = null;
            while (true)
            {
                Console.WriteLine("Ange ett användarnamn (1–10 tecken, endast a–ö/A–Ö/0–9):");
                Console.WriteLine("Tryck på Escape för att avbryta.");

                (int left, int top) = Console.GetCursorPosition();

                string? username = "";
                ConsoleKeyInfo cki;

                while (true)
                {
                    cki = Console.ReadKey(intercept: true);

                    if (cki.Key == ConsoleKey.Escape)
                    {
                        return null;
                    } 
                    else if(cki.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if(cki.Key == ConsoleKey.Backspace)
                    {
                        if(username.Length > 0)
                        {
                            username = username[..^1];
                            //Console.Write(username);
                            Console.SetCursorPosition(left, top);
                            Console.Write(username.PadRight(10, ' '));
                            Console.SetCursorPosition(left + username.Length, top);
                        }
                    }
                    else if(Regex.IsMatch(cki.KeyChar.ToString(), @"^[0-9A-Za-zÅÄÖåäö]{1,10}$") && username.Length < 10)
                    {
                        username += cki.KeyChar.ToString();
                        Console.Write(cki.KeyChar);
                    }
                }
                
                
                username = username.Trim();


                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Användarnamnet får inte vara tomt.");
                    continue;
                }


                //var valid = Regex.IsMatch(username, @"^[0-9A-Za-zÅÄÖåäö]{1,10}$");
                //if (!valid)
                //{
                //    Console.WriteLine("Ogiltigt namn. Tillåtna tecken: a–ö, A–Ö och 0–9. Max 10 tecken.");
                //    continue;
                //}

                if (usernames.Contains(username))
                {
                    Console.WriteLine($"Användarnamnet \"{username}\" finns redan.[k1]");
                    continue;
                }


                if (await users.ExistsByNameAsync(username))
                {
                    Console.WriteLine($"Användarnamnet \"{username}\" finns redan.[k2]");
                    continue;
                }


                createdUser = await users.CreateUserAsync(new User { Name = username });
                Console.WriteLine($"Användare \"{username}\" skapades!");
                break;
            }

            return createdUser;
        }


        //public static User LoadUser(IMongoCollection<User> users)
        //public static int? LoadUser(IMongoCollection<User> users)
        //{
        //    Console.Clear();

        //    var allUsers = users.Find<User>(FilterDefinition<User>.Empty).ToList();
        //    var usernames = allUsers.Select(u => u.Name).ToList();

        //    List<string?> options = usernames;

        //    string search = "";
        //    int selected = 0;
        //    int numberOfOptions = usernames.Count;


        //    if(numberOfOptions == 0)
        //    {
        //        Console.WriteLine("There are no users in the database to load.");
        //        Console.WriteLine("Press any key to continue");
        //        Console.ReadLine();
        //        return null;
        //    }


        //    void DrawDeselect(int option)
        //    {
        //        Console.BackgroundColor = ConsoleColor.Black;
        //        Console.SetCursorPosition(0, option);
        //        Console.Write(options[option]);
        //    }

        //    void DrawSelect(int option)
        //    {
        //        Console.BackgroundColor = ConsoleColor.DarkGray;
        //        Console.SetCursorPosition(0, option);
        //        Console.Write(options[option]);
        //    }

        //    void DrawBigList()
        //    {
        //        Console.Clear();

        //        Console.BackgroundColor = ConsoleColor.Black;
        //        Console.WriteLine($"{((selected - 2 + numberOfOptions) % numberOfOptions)}. {options[((selected - 2 + numberOfOptions) % numberOfOptions)]}");
        //        Console.WriteLine($"{((selected - 1 + numberOfOptions) % numberOfOptions)}. {options[((selected - 1 + numberOfOptions) % numberOfOptions)]}");
        //        Console.BackgroundColor = ConsoleColor.DarkGray;
        //        Console.Write($"{selected}. {options[selected]}");
        //        Console.BackgroundColor = ConsoleColor.Black;
        //        Console.WriteLine("");
        //        Console.WriteLine($"{((selected + 1 + numberOfOptions) % numberOfOptions)}. {options[((selected + 1 + numberOfOptions) % numberOfOptions)]}");
        //        Console.WriteLine($"{((selected + 2 + numberOfOptions) % numberOfOptions)}. {options[((selected + 2 + numberOfOptions) % numberOfOptions)]}");
        //    }

        //    Console.WriteLine("Choose a user by using the up and down arrow keys.");
        //    Console.WriteLine("Start typing a username to filter the list.");
        //    Console.WriteLine("Press Enter key to select highlighted user.");
        //    Console.WriteLine("Press escape to exit without loading a user.");
        //    Console.WriteLine();

        //    var (left, top) = Console.GetCursorPosition();

        //    ConsoleKeyInfo cki;
        //    int intervalTime = 50;

        //    MenuViewer.View(row: top, options: options, selected: 0);

        //    while (true)
        //    {
        //        while (Console.KeyAvailable == false)
        //        {
        //            Thread.Sleep(intervalTime);
        //        }

        //        if (Console.KeyAvailable)
        //        {
        //            cki = Console.ReadKey();



        //            //if (numberOfOptions <= 5)
        //            if (false)
        //            {
        //                if (cki.Key == ConsoleKey.UpArrow)
        //                {
        //                    DrawDeselect(selected);
        //                    selected--;
        //                    selected = (selected + numberOfOptions) % numberOfOptions;
        //                    DrawSelect(selected);
        //                }
        //                else if (cki.Key == ConsoleKey.DownArrow)
        //                {
        //                    DrawDeselect(selected);
        //                    selected++;
        //                    selected = selected % numberOfOptions;
        //                    DrawSelect(selected);
        //                }
        //                else if (cki.Key == ConsoleKey.Enter)
        //                {
        //                    Console.BackgroundColor = ConsoleColor.Black;
        //                    Console.Clear();
        //                    return selected;
        //                }
        //            }
        //            else
        //            {
        //                if (cki.Key == ConsoleKey.UpArrow)
        //                {
        //                    selected--;
        //                    selected = (selected + numberOfOptions) % numberOfOptions;
        //                    //DrawBigList();
        //                    MenuViewer.View(row: top, options: options, selected: selected);
        //                }
        //                else if (cki.Key == ConsoleKey.DownArrow)
        //                {
        //                    selected++;
        //                    selected = selected % numberOfOptions;
        //                    //DrawBigList();
        //                    MenuViewer.View(row: top, options: options, selected: selected);
        //                }
        //                else if (cki.Key == ConsoleKey.Enter)
        //                {
        //                    Console.BackgroundColor = ConsoleColor.Black;
        //                    Console.Clear();
        //                    return selected;
        //                }else if(cki.Key == ConsoleKey.Escape)
        //                {
        //                    Console.BackgroundColor = ConsoleColor.Black;
        //                    Console.Clear();
        //                    return null;
        //                }
        //            }

        //        }
        //    }

        //    //return new User { Name = "test" };
        //    return 5; // Jag ska returnera en användare när jag är klar med denna funktion.
        //}

        public static async Task<User?> LoadUserAsync(IUserRepository users)
        {
            Console.BackgroundColor = ConsoleColor.Black; // Jag tror inte att denna behövs, men använder för säkerhets skull.
            Console.Clear();

            var filteredUsers = await users.GetAllAsync();
            var usernames = filteredUsers.Select(u => u.Name).ToList();

            List<string?> options = usernames;

            


            int numberOfOptions = usernames.Count;
            if (numberOfOptions == 0)
            {
                Console.WriteLine("There are no users in the database to load.");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return null;
            }

            string search = "";
            int selected = 0;


            

            Console.WriteLine("Välj en användare med hjälp av piltangenterna.");
            Console.WriteLine("Skriv en söksträng för att hitta en användare.");
            Console.WriteLine("Tryck på Enter för att välja den markerad användaren.");
            Console.WriteLine("Tryck på Esc för att gå till föregående meny utan att ladda en användare.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            int inputRow = Console.CursorTop - 2;
            int listRow = Console.CursorTop;

            MenuViewer.View(row: listRow, options: options, selected: 0);

            ConsoleKeyInfo cki;
            int intervalTime = 50;


            async Task RefreshAsync()
            {
                filteredUsers = await users.GetByNameAsync(search);
                options = filteredUsers.Select(u => u!.Name).ToList();

                Console.SetCursorPosition(0, inputRow);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                string prompt = "Sök: " + search;
                Console.Write(prompt.PadRight(Console.WindowWidth));
                Console.ForegroundColor = ConsoleColor.White;

                if (options.Count == 0) selected = 0;
                else
                {
                    if (selected >= options.Count) selected = options.Count - 1;
                    if (selected < 0) selected = 0;
                }

                MenuViewer.View(row: listRow, options: options, selected: selected);

                Console.SetCursorPosition(Math.Min(prompt.Length, Console.WindowWidth - 1), inputRow);
            }

            await RefreshAsync();

            while (true)
            {
                string prompt = "Sök: " + search;
                Console.SetCursorPosition(Math.Min(prompt.Length, Console.WindowWidth - 1), inputRow);

                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (options.Count > 0) { selected = (selected - 1 + options.Count) % options.Count; }
                    MenuViewer.View(row: listRow, options: options, selected: selected);
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (options.Count > 0) { selected = (selected + 1) % options.Count; }
                    MenuViewer.View(row: listRow, options: options, selected: selected);
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return (options.Count == 0) ? null : filteredUsers[selected];
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return null;
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (search.Length > 0)
                    {
                        search = search[..^1];
                        await RefreshAsync();
                    }
                }
                else
                {
                    char ch = cki.KeyChar;
                    search += ch;
                    await RefreshAsync();
                }
            }

            //return null;
        }

        //public string? LoadLevel(IMongoCollection<Level> levels)
        //{
        //    Console.Clear();

        //    var allLevels = levels.Find<Level>(FilterDefinition<Level>.Empty).ToList();
        //    var levelNames = allLevels.Select(l => l.Name).ToList();

        //    List<string?> options = levelNames;

        //    MenuViewer.View()
        //}

        public static async Task<Level?> LoadLevelAsync(ILevelRepository levels)
        {
            var allLevels = (await levels.GetAllAsync()).ToList();
            var levelNames = allLevels.Select(l => l.Name).ToList();

            int? selected = null;
            //while(selected == null)
            //{
            //    selected = MenuViewer.View(options: levelNames);
            //}
            selected = MenuViewer.View(options: levelNames);
            

            if(selected == null)
            {
                //Console.SetCursorPosition(0, Console.CursorTop);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.WriteLine("Återgår till föregående meny.");
                Thread.Sleep(1000);
                return null;
            }

            Console.WriteLine($"Du valde bana: {levelNames[selected.Value]}");
            Thread.Sleep(2000);

            return allLevels[selected.Value];
        }

        public static async Task<Game?> LoadGameAsync(IGameRepository games, User? user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var allGames = await games.GetAllAsync();
            var game = allGames
                .Where(g => g.UserId == user.Id && g.GameStatus == GameStatus.Ongoing)
                .OrderByDescending(g => g.CreatedAt)
                .FirstOrDefault();

            return game;
        }


        public static async Task DisplayHighScore(
            IGameRepository games, 
            ILevelRepository levels, 
            IUserRepository users,
            User? user = null,
            int numberToShow = 3
            )
        {
            var allGames = await games.GetAllAsync();
            var finishedGames = allGames
                .Where(
                    g => g.GameStatus != GameStatus.Ongoing
                    &&
                    g.Score.HasValue
                );


            if(user != null)
            {
                finishedGames = finishedGames
                    .Where(g => g.UserId == user.Id);
            }

            if (!finishedGames.Any())
            {
                Console.WriteLine("Inga avslutade spel att visa.");
                Console.WriteLine();
                Console.WriteLine("Tryck på valfri tangent för att gå tillbaka.");
                Console.ReadKey(true);
                return;
            }


            //var playedLevels = allGames.Select(g => g.LevelId).ToList().Distinct();
            //foreach(string levelId in playedLevels)
            //{
            //    var level = await levels.GetByIdAsync(levelId);
            //    var levelName = level?.Name ?? "Okänt namn";
            //    Console.WriteLine($"Bana: {levelName}");

            //    var highScores = allGames
            //        .Where(g => g.LevelId == levelId)
            //        .OrderBy(g => g.score)...
            //}


            // Jag vill att highscore-listan ska sorteras efter level.Name, inte level.Id.
            var playedLevels = finishedGames.Select(g => g.LevelId).ToList().Distinct();
            var allLevels = await levels.GetAllAsync();
            var namedLevels = allLevels
                .Where(l => playedLevels.Contains(l.Id))
                .Distinct()
                .OrderBy(l => l.Name);

            var levelMap = new Dictionary<string, string>();
            foreach(var nl in namedLevels)
            {
                levelMap[nl.Id] = nl.Name ?? $"Gammal bana med id {nl.Id}";
            }

            //var gamesByLevel = finishedGames.GroupBy(g => g.LevelId);
            var gamesByLevel = finishedGames
                .OrderBy(g => levelMap[g.LevelId])
                .GroupBy(g => levelMap[g.LevelId]);

            var uniqueUserIds = finishedGames
                .Select(g => g.UserId)
                .Distinct()
                .ToList();

            var userMap = new Dictionary<string, string>();
            foreach(var userId in uniqueUserIds)
            {
                var u = await users.GetByIdAsync(userId);
                userMap[userId] = u?.Name ?? "Okänd spelare";
            }

            foreach(var group in gamesByLevel)
            {
                //var level = await levels.GetByIdAsync(group.Key);
                //var levelName = level?.Name ?? "Okänd bana";
                var levelName = group.Key;




                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Bana: {levelName}");
                Console.ForegroundColor = ConsoleColor.White;

                var topScores = group
                    .OrderByDescending(g => g.Score!.Value)
                    .ThenBy(g => g.CompletedAt ?? DateOnly.MaxValue)
                    .Take(numberToShow)
                    .ToList();

                if (!topScores.Any())
                {
                    Console.WriteLine("Inga resultat att visa än.");
                    continue;
                }

                for(int i = 0; i < numberToShow; i++)
                {
                    if(i >= topScores.Count)
                    {
                        Console.WriteLine($"{i + 1}. För få avslutade spel.");
                        continue;
                    }

                    var topGame = topScores[i];
                    //var userName = userMap[topGame.UserId];
                    var userName = userMap.TryGetValue(topGame.UserId, out var name) ? name : "Okänd spelare";
                    var score = topGame.Score;
                    var date = topGame.CompletedAt?.ToString("yyyy-MM-dd") ?? "Okänt datum";

                    Console.WriteLine($"{i + 1}. {userName} - {score} - {date}");
                }

                Console.WriteLine();
                
            }

            Console.WriteLine("Tryck på valfri tangent för att gå tillbaka.");
            //Console.ReadLine();
            Console.ReadKey(true);

        }

        public static async Task DeleteUserAndTheirGames(IUserRepository users, IGameRepository games)
        {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.WriteLine("Välj en användare att radera helt (inkl. alla dess spel).");
            Console.WriteLine("Tryck Escape i användarlistan för att avbryta.");
            Console.WriteLine();

            var user = await LoadUserAsync(users);
            if (user == null)
            {
                Console.WriteLine("Ingen användare vald. Återgår till huvudmenyn.");
                Thread.Sleep(2000);
                return;
            }

            Console.WriteLine($"Du valde: {user.Name} med id {user.Id}.");
            Console.WriteLine("Skriv lösenordet för att bekräfta borttagning (eller lämna tomt/tryck Esc för att avbryta).");

            (int left, int passwordRow) = Console.GetCursorPosition();
            string prompt = "Lösenord: ";
            Console.Write(prompt);

            string password = "";

            void Refresh()
            {
                string censoredPassword = new string('*', password.Length);
                Console.SetCursorPosition(0, passwordRow);
                string line = prompt + censoredPassword;
                //Console.WriteLine($"Lösenord: {censoredPassword}");
                Console.Write(line.PadRight(Console.WindowWidth));
                Console.SetCursorPosition(prompt.Length + password.Length, passwordRow);
            }

            ConsoleKeyInfo cki;
            while (true)
            {
                cki = Console.ReadKey(intercept: true);

                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nAvbrutet.");
                    Thread.Sleep(2000);
                    return;
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                        password = password[..^1];
                    Refresh();
                }
                else
                {
                    if (!char.IsControl(cki.KeyChar) && password.Length < 20)
                    {
                        password += cki.KeyChar;
                        Refresh();
                    }
                }
            }


            if (!string.Equals(password, "schack", StringComparison.Ordinal))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Fel lösenord. Inget togs bort.");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                return;
            }

            await DeleteUserAndTheirGames(users, games, user);
            Console.WriteLine("Klart. Tryck valfri tangent för att återgå.");
            Console.ReadKey(true);

        }

        public static async Task DeleteUserAndTheirGames(IUserRepository users, IGameRepository games, User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            Console.ForegroundColor = ConsoleColor.Magenta;
            var removedGames = await games.DeleteByUserAsync(user);
            Console.WriteLine($"Spelaren hade {removedGames} spel som nu togs bort.");
            var removedUsers = await users.DeleteAsync(user);
            Console.WriteLine($"{removedUsers} stycken spelare med id {user.Id} \noch namn {user.Name} togs bort.");
            Console.ForegroundColor = ConsoleColor.White;
        }



        public static async Task AddDemoData(
            IUserRepository users,
            IGameRepository games,
            ILevelRepository levels
        )
        {
            var hasUsers = await users.HasElements();
            var hasGames = (await games.GetAllAsync()).Any();
            if (hasUsers || hasGames) return;


            var allLevels = await levels.GetAllAsync();
            if (allLevels.Count < 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ingen Demodata genereras för att det finns färre än tre banor i databasen.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            // Banor.
            Level? level1 = FindLevelByName(allLevels, "1", "Level1", "Level 1");
            Level? level2 = FindLevelByName(allLevels, "2", "Level2", "Level 2");
            Level? level3 = FindLevelByName(allLevels, "3", "Level3", "Level 3");

            if(level1 == null || level2 == null || level3 == null)
            {
                var sorted = allLevels.OrderBy(l => l.Name).ToList();
                level1 = sorted.ElementAtOrDefault(0);
                level2 = sorted.ElementAtOrDefault(1);
                level3 = sorted.ElementAtOrDefault(2);
            }


            if (level1 is null || level2 is null || level3 is null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Demodata hoppar över: kunde inte hitta tre banor.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }


            // Användare
            var anna = await users.CreateUserAsync(new User { Name = "Anna" });
            var bosse = await users.CreateUserAsync(new User { Name = "Bosse" });

            
            // Spel
            static LevelDataState BuildState(Level level, GameStatus status)
            {
                var ld = new LevelData();
                ld.LoadFromLayout(level.Layout);
                ld.GameStatus = status;
                
                return new LevelDataState(ld);
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // --------
            // ANNA: bana 1 (100p, Completed), bana 3 (300p, Completed)
            await games.CreateAsync(new Game
            {
                UserId = anna.Id,
                LevelId = level1.Id,
                LevelDataState = BuildState(level1, GameStatus.Completed),
                GameStatus = GameStatus.Completed,
                Score = 100,
                CompletedAt = today,
                CreatedAt = DateTime.Now
            });

            await games.CreateAsync(new Game
            {
                UserId = anna.Id,
                LevelId = level3.Id,
                LevelDataState = BuildState(level3, GameStatus.Completed),
                GameStatus = GameStatus.Completed,
                Score = 300,
                CompletedAt = today,
                CreatedAt = DateTime.Now
            });

            // --------
            // BOSSE: bana 2 (20p, HeroDead), bana 3 (30p, HeroDead), och ett pågående spel på bana 3
            await games.CreateAsync(new Game
            {
                UserId = bosse.Id,
                LevelId = level2.Id,
                LevelDataState = BuildState(level2, GameStatus.HeroDead),
                GameStatus = GameStatus.HeroDead,
                Score = 20,
                CompletedAt = today,
                CreatedAt = DateTime.Now
            });

            await games.CreateAsync(new Game
            {
                UserId = bosse.Id,
                LevelId = level3.Id,
                LevelDataState = BuildState(level3, GameStatus.HeroDead),
                GameStatus = GameStatus.HeroDead,
                Score = 30,
                CompletedAt = today,
                CreatedAt = DateTime.Now
            });

            // Pågående
            //await games.CreateAsync(new Game
            //{
            //    UserId = bosse.Id,
            //    LevelId = level3.Id,
            //    LevelDataState = BuildState(level3, GameStatus.Ongoing),
            //    GameStatus = GameStatus.Ongoing,
            //    Score = null,
            //    CompletedAt = null,
            //    CreatedAt = DateTime.Now
            //});
            //
            // Jag byter ut ovanstående mot nedanstående för att kunna 
            // uppdatera hjälten till att vara nära målet.
            var bossesSpel = new Game
            {
                UserId = bosse.Id,
                LevelId = level3.Id,
                LevelDataState = BuildState(level3, GameStatus.Ongoing),
                GameStatus = GameStatus.Ongoing,
                Score = null,
                CompletedAt = null,
                CreatedAt = DateTime.Now
            };
            bossesSpel.LevelDataState.Hero.Position = new Position(50, 5);
            await games.CreateAsync(bossesSpel);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Demodata skapad.");
            Console.ForegroundColor = ConsoleColor.White;

            static Level? FindLevelByName(IEnumerable<Level> levels, params string[] candidateNames)
            {
                return levels.FirstOrDefault(l =>
                    !string.IsNullOrWhiteSpace(l.Name) &&
                    candidateNames.Any(c => string.Equals(l.Name.Trim(), c, StringComparison.OrdinalIgnoreCase))
                );
            }
                
        }


    }
}
