using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal class MenuViewer
    {
        public static int? View(List<string> options)
        {
            if(options == null || options.Count == 0)
            { 
                return null;
            }

            int selected = 0;
            int numberOfOptions = options.Count;

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

            

            Console.Clear();
            if(numberOfOptions <= 5)
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    if(i == selected) 
                    { 
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(options[i]);
                    Console.WriteLine("");
                }
            }
            else
            {
                DrawBigList();
            }
            


            ConsoleKeyInfo cki;
            int intervalTime = 50;

            while (true)
            {
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(intervalTime);
                }

                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey();

                    if(numberOfOptions <= 5)
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
                            DrawBigList();
                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {
                            selected++;
                            selected = selected % numberOfOptions;
                            DrawBigList();
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            return selected;
                        }
                    }
                    
                }
            }
        }
    }
}
