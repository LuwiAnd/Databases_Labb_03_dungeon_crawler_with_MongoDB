using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal static class LogViewer
    {
        public static void Show(IReadOnlyList<string> messages)
        {
            if (messages == null) return;

            int headerRows = 4;
            int visibleRows = Math.Max(1, Console.WindowHeight - headerRows);
            int top = Math.Max(0, messages.Count - visibleRows);

            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black; // Jag tror inte att denna behövs, men använder för säkerhets skull.
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Meddelandelogg");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Använd upp-/nedåtpil, PgUp/PgDn, Home/End (Esc för att avsluta)");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(new string('─', Math.Min(Console.WindowWidth, 80)));

                for (int i = 0; i < visibleRows; i++)
                {
                    int idx = top + i;
                    string line = (idx >= 0 && idx < messages.Count) ? messages[idx] : string.Empty;
                    if (line != null && line.Length > Console.WindowWidth) line = line.Substring(0, Console.WindowWidth);

                    //if(!string.IsNullOrEmpty(line) && line.Substring(0, 5).ToUpper() == "PLAYER")
                    if(!string.IsNullOrEmpty(line) && line.StartsWith("player", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    if(line != null) Console.WriteLine(line.PadRight(Console.WindowWidth));
                }
                Console.ForegroundColor = ConsoleColor.White;

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape) break;
                else if (key.Key == ConsoleKey.UpArrow) top = Math.Max(top - 1, 0);
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    top = Math.Min(
                        top + 1,
                        Math.Max(messages.Count - visibleRows, 0)
                    );
                }
                else if (key.Key == ConsoleKey.PageUp) top = Math.Max(top - visibleRows, 0);
                else if (key.Key == ConsoleKey.PageDown)
                {
                    top = Math.Min(
                        top + visibleRows,
                        Math.Max(messages.Count - visibleRows, 0)
                    );
                }
                else if (key.Key == ConsoleKey.Home) top = 0;
                else if (key.Key == ConsoleKey.End) top = Math.Max(messages.Count - visibleRows, 0);
            }
        }
    }
}
