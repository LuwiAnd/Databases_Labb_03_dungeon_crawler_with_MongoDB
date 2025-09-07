using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    static internal class GeneralDungeonFunctions
    {
        public const char wallChar = '#';
        public const char ratChar = 'r';
        public const char snakeChar = 's';
        public const char playerChar = '@';
        public const char goalChar = 'G';

        // Jag borde gjort färger för alla LevelElement från början.
        public const ConsoleColor goalColor = ConsoleColor.Yellow;

        public const int mapDisplacementX = 0;
        public const int mapDisplacementY = 3;

        public const int maxConsoleMessageLenght = 120;

        public const double visionRange = 5;

        public const int TurnsUntilClearingMessages = 3;

        public static bool IsVisible(Position p1, Position p2)
        {
            double SquaredXDistance = Math.Pow(p1.X - p2.X, 2.0);
            double SquaredYDistance = Math.Pow(p1.Y - p2.Y, 2.0);
            double SquaredVisionRange = Math.Pow(visionRange, 2.0);

            return SquaredXDistance + SquaredYDistance < SquaredVisionRange;
        }

        public static Position GetAdjacentPosition(Position position, string relativePosition)
        {
            if (relativePosition == "above") { position.Y -= 1; }
            else if (relativePosition == "below") { position.Y += 1; }
            else if (relativePosition == "left") { position.X -= 1; }
            else if (relativePosition == "right") { position.X += 1; }

            return position;
        }

        public static bool isPositionEmpty(Position position, List<LevelElement> elements)
        {
            bool empty = true;
            foreach (var element in elements)
            {
                if (element.Position.X == position.X && element.Position.Y == position.Y) empty = false;
            }
            return empty;
        }

        public static void Erase(int x, int y)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(x + mapDisplacementX, y + mapDisplacementY);
            Console.Write(" ");
            Console.SetCursorPosition(left, top);
        }

        public static void ClearConsoleMessages()
        {
            string clearConsoleString = "   ".PadLeft(maxConsoleMessageLenght, ' ');
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 1);
            Console.Write(clearConsoleString);
            Console.SetCursorPosition(0, 2);
            Console.Write(clearConsoleString);
            Console.SetCursorPosition(left, top);
        }

        
        public static string TranslateWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            string lowerWord = word.ToLower();
            string answer;

            switch(lowerWord)
            {
                case "rat": answer = "råtta"; break;
                case "snake": answer = "orm"; break;
                case "player": answer = "spelare"; break;
                case "hero": answer = "hjälte"; break;
                case "goal": answer = "mål"; break;
                case "wall": answer = "vägg"; break;
                case "damage": answer = "skada"; break;
                case "throw": answer = "kastar"; break;
                case "throws": answer = "kastar"; break;
                case "dices": answer = "tärningar"; break;
                default: answer = word; break;
            }

            if (!string.IsNullOrEmpty(answer) && char.IsUpper(word[0]))
            {
                answer = char.ToUpper(answer[0]) + answer.Substring(1);
            }

            return answer;
        }


        public static string Translate(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                return sentence;

            var words = sentence.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                // Tar bort specialtecken i början/slutet av orden
                string cleanWord = words[i].Trim('!', '.', ',', ';', ':');

                string translatedWord = TranslateWord(cleanWord);

                if (words[i].EndsWith("."))
                    translatedWord += ".";
                else if (words[i].EndsWith(","))
                    translatedWord += ",";
                else if (words[i].EndsWith("!"))
                    translatedWord += "!";
                else if (words[i].EndsWith(":"))
                    translatedWord += ":";

                words[i] = translatedWord;
            }

            return string.Join(" ", words);
        }

    }
}
