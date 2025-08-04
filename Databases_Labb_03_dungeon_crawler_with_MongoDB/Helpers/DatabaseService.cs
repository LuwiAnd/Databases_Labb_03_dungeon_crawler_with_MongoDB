using Databases_Labb_03_dungeon_crawler_with_MongoDB.SaveModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal class DatabaseService
    {
        public static (IMongoCollection<User>, IMongoCollection<Level>, IMongoCollection<Game>) InitializeDatabase(IMongoDatabase db)
        {
            var users  = db.GetCollection<User>("users");
            var levels = db.GetCollection<Level>("levels");
            var games  = db.GetCollection<Game>("games");

            return (users, levels, games);
        }

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

        public static void CreateUser(IMongoCollection<User> users)
        {
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
                    Console.WriteLine($"Username \"{username}\" was either not 1 - 10 characters or ");
                }
            }

        }


        //public static User LoadUser(IMongoCollection<User> users)
        public static int? LoadUser(IMongoCollection<User> users)
        {
            Console.Clear();

            var allUsers = users.Find<User>(FilterDefinition<User>.Empty).ToList();
            var usernames = allUsers.Select(u => u.Name).ToList();

            List<string?> options = usernames;

            string search = "";
            int selected = 0;
            int numberOfOptions = usernames.Count;


            if(numberOfOptions == 0)
            {
                Console.WriteLine("There are no users in the database to load.");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return null;
            }


            void DrawDeselect(int option)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, option);
                Console.Write(options[option]);
            }

            void DrawSelect(int option)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(0, option);
                Console.Write(options[option]);
            }

            void DrawBigList()
            {
                Console.Clear();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"{((selected - 2 + numberOfOptions) % numberOfOptions)}. {options[((selected - 2 + numberOfOptions) % numberOfOptions)]}");
                Console.WriteLine($"{((selected - 1 + numberOfOptions) % numberOfOptions)}. {options[((selected - 1 + numberOfOptions) % numberOfOptions)]}");
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write($"{selected}. {options[selected]}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("");
                Console.WriteLine($"{((selected + 1 + numberOfOptions) % numberOfOptions)}. {options[((selected + 1 + numberOfOptions) % numberOfOptions)]}");
                Console.WriteLine($"{((selected + 2 + numberOfOptions) % numberOfOptions)}. {options[((selected + 2 + numberOfOptions) % numberOfOptions)]}");
            }

            Console.WriteLine("Choose a user by using the up and down arrow keys.");
            Console.WriteLine("Start typing a username to filter the list.");
            Console.WriteLine("Press Enter key to select highlighted user.");
            Console.WriteLine("Press escape to exit without loading a user.");
            Console.WriteLine();

            var (left, top) = Console.GetCursorPosition();

            ConsoleKeyInfo cki;
            int intervalTime = 50;

            MenuViewer.View(row: top, options: options, selected: 0);

            while (true)
            {
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(intervalTime);
                }

                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey();



                    //if (numberOfOptions <= 5)
                    if (false)
                    {
                        if (cki.Key == ConsoleKey.UpArrow)
                        {
                            DrawDeselect(selected);
                            selected--;
                            selected = (selected + numberOfOptions) % numberOfOptions;
                            DrawSelect(selected);
                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {
                            DrawDeselect(selected);
                            selected++;
                            selected = selected % numberOfOptions;
                            DrawSelect(selected);
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            return selected;
                        }
                    }
                    else
                    {
                        if (cki.Key == ConsoleKey.UpArrow)
                        {
                            selected--;
                            selected = (selected + numberOfOptions) % numberOfOptions;
                            //DrawBigList();
                            MenuViewer.View(row: top, options: options, selected: selected);
                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {
                            selected++;
                            selected = selected % numberOfOptions;
                            //DrawBigList();
                            MenuViewer.View(row: top, options: options, selected: selected);
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            return selected;
                        }else if(cki.Key == ConsoleKey.Escape)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            return null;
                        }
                    }

                }
            }

            //return new User { Name = "test" };
            return 5; // Jag ska returnera en användare när jag är klar med denna funktion.
        }

        public string? LoadLevel(IMongoCollection<Level> levels)
        {
            Console.Clear();

            var allLevels = levels.Find<Level>(FilterDefinition<Level>.Empty).ToList();
            var levelNames = allLevels.Select(l => l.Name).ToList();

            List<string?> options = levelNames;

            MenuViewer.View()
        }
    }
}
