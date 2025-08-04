using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    internal class Snake : Enemy
    {
        

        //public override void Update(Hero hero, List<LevelElement> elements)
        public override void Update(Hero hero, LevelData levelData)
        {
            Move(hero, levelData);
            IsVisible = GeneralDungeonFunctions.IsVisible(hero.Position, Position);
            if (!IsVisible) { GeneralDungeonFunctions.Erase(Position.X, Position.Y); }
        }

        //public Snake(int[] position)
        public Snake(int positionX, int positionY)
        {
            //this._position = position;
            Position = new Position(x: positionX, y: positionY);
            HP = 25;
            Color = ConsoleColor.Green;
            Type = "snake";
            AttackDice = new Dice(numberOfDice: 3, sidesPerDice: 4, modifier: 2);
            DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 8, modifier: 5);
        }

        public Snake(SnakeState state)
        {
            Position = state.Position;
            HP = state.HP;
            Color = ConsoleColor.Green;
            Type = "snake";
            AttackDice = new Dice(numberOfDice: 3, sidesPerDice: 4, modifier: 2);
            DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 8, modifier: 5);
        }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(
                Position.X + GeneralDungeonFunctions.mapDisplacementX,
                Position.Y + GeneralDungeonFunctions.mapDisplacementY
            );
            Console.ForegroundColor = Color;
            Console.Write(GeneralDungeonFunctions.snakeChar.ToString());

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        
        //public void Move(Hero hero, List<LevelElement> elements)
        public void Move(Hero hero, LevelData levelData)
        {

            List<LevelElement> elements = levelData.Elements;

            //bool snakeIsNextToHero = false;
            string herosRelativePosition = "";
            string[] relativePositions = new string[]
            {
                "above",
                "below",
                "left",
                "right"
            };
            Position adjacentPosition;
            foreach (string relativePosition in relativePositions)
            {
                adjacentPosition = GeneralDungeonFunctions.GetAdjacentPosition(Position, relativePosition);
                if (adjacentPosition.X == hero.Position.X && adjacentPosition.Y == hero.Position.Y)
                {
                    //snakeIsNextToHero = true;
                    herosRelativePosition = relativePosition;
                }
            }

            Position fleePosition = GeneralDungeonFunctions.GetAdjacentPosition(
                    Position,
                    ReverseRelativePosition(herosRelativePosition)
                );
            bool possibleToFlee = GeneralDungeonFunctions.isPositionEmpty(
                fleePosition,
                elements
            );
            if (possibleToFlee)
            {
                GeneralDungeonFunctions.Erase(Position.X, Position.Y);
                Position = fleePosition;
                Draw();
            }
            else
            {
                Draw();
            }
        }

        private string ReverseRelativePosition(string relativePosition)
        {
            switch (relativePosition)
            {
                case "above":
                    return "below";
                //break; // Eftersom koden ovan är ett return-statement, så behövs ej break.
                case "below":
                    return "above";
                //break;
                case "left":
                    return "right";
                //break;
                case "right":
                    return "left";
                //break;
                default:
                    return "";
                    //break;
            }
        }
    }
}
