using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    internal class Goal : LevelElement
    {
        public char Appearance { get; set; } = GeneralDungeonFunctions.goalChar;
        public ConsoleColor Color { get; set; }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(Position.X + GeneralDungeonFunctions.mapDisplacementX, Position.Y + GeneralDungeonFunctions.mapDisplacementY);
            Console.ForegroundColor = Color;
            Console.Write(Appearance.ToString());

            // Resetting color and cursor, for debugging output to be shown under the dungeon.
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        public Goal(int positionX, int positionY)
        {
            Position = new Position(x: positionX, y: positionY);
            IsVisible = false;

            Appearance = 'G';
            Color = GeneralDungeonFunctions.goalColor;
            Type = "goal";
        }

        public Goal(GoalState goalState)
        {
            Position = goalState.Position;
            IsVisible = goalState.IsVisible;

            Appearance = GeneralDungeonFunctions.goalChar;
            Color = GeneralDungeonFunctions.goalColor;
            Type = "goal";
        }

        public void Update(Hero hero)
        {
            if (GeneralDungeonFunctions.IsVisible(hero.Position, Position))
            {
                //this.isVisible = true;
                IsVisible = true;
                Draw();
            }
        }
    }
}
