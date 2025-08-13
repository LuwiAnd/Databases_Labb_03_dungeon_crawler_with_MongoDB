using System;
using System.Collections.Generic;
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
        public const int mapDisplacementY = 2;

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
            Console.SetCursorPosition(0, 0);
            Console.Write(clearConsoleString);
            Console.SetCursorPosition(0, 1);
            Console.Write(clearConsoleString);
            Console.SetCursorPosition(left, top);

        }
    }
}
