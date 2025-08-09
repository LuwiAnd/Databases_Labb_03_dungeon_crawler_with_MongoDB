using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    internal class Rat : Enemy
    {
        private static readonly Random random = new Random();

        //public override void Update(Hero hero, List<LevelElement> elements)
        public override void Update(Hero hero, LevelData levelData)
        {
            Move(hero: hero, levelData: levelData);
            IsVisible = GeneralDungeonFunctions.IsVisible(hero.Position, Position);
            if (!IsVisible) { GeneralDungeonFunctions.Erase(Position.X, Position.Y); }
        }

        //public Rat(int[] position)
        public Rat(int positionX, int positionY)
        {
            Position = new Position(x: positionX, y: positionY);
            Color = ConsoleColor.Red;
            HP = 10;
            Type = "rat";
            AttackDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 3);
            DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 1);
            //IsVisible = false; // Jag behöver inte sätta denna variabel i konstruktorn för att den bara används när jag ritar upp spelets "grafik".
        }

        public Rat(RatState state)
        {
            Position = state.Position;
            HP = state.HP;
            //IsVisible = false;
            Color = ConsoleColor.Red;
            Type = "rat";
            AttackDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 3);
            DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 1);
        }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(
                Position.X + GeneralDungeonFunctions.mapDisplacementX,
                Position.Y + GeneralDungeonFunctions.mapDisplacementY
            );
            Console.ForegroundColor = Color;
            Console.Write(GeneralDungeonFunctions.ratChar.ToString());

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        public void Move(Hero hero, LevelData levelData)
        {
            // I changed from using "List<LevelElement> elements" as a parameter to using "LevelData levelData", but 
            // I don't have time to fix all such things this close to deadline, otherwise I would use "levelData.Elements" 
            // instead of "elements" everywhere in this function and done many other such changes thoughout this project.

            List<LevelElement> elements = levelData.Elements;

            string[] relativePositions = new string[]
            {
                "above",
                "below",
                "left",
                "right"
            };
            //Random random = new Random();
            string nextRelativePosition = relativePositions[random.Next(relativePositions.Length)];

            Position adjacentPosition;
            adjacentPosition = GeneralDungeonFunctions.GetAdjacentPosition(Position, nextRelativePosition);
            if (adjacentPosition.X == hero.Position.X && adjacentPosition.Y == hero.Position.Y)
            {
                //AttackHero(hero);
                AttackHero(hero, levelData);
                if (hero.HP > 0) { hero.Attack(levelData, this); }
                return;
            }


            bool possibleToMove = GeneralDungeonFunctions.isPositionEmpty(
                adjacentPosition,
                elements
            );
            if (possibleToMove)
            {
                GeneralDungeonFunctions.Erase(Position.X, Position.Y);
                Position = adjacentPosition;
                Draw();
            }
        }
    }
}
