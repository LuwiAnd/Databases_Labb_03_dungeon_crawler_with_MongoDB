using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;

namespace Labb_02_dungeon_crawler
{
    internal class Rat : Enemy
    {
        //private int[] _position;

        //public override int[] Position
        //{
        //    get { return this._position; }
        //    // positionen får endast sättas av konstruktorn, eller av Update-funktionen.
        //}
        //public string Type { get; set; }
        //public double HP { get; set; }

        //Dice AttackDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 3);
        //Dice DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 1);

        //public override void Update(Hero hero, List<LevelElement> elements)
        public override void Update(Hero hero, LevelData levelData)
        {
            this.Move(hero: hero, levelData: levelData);
            this.IsVisible = GeneralDungeonFunctions.IsVisible(hero.Position, this.Position);
            if (!this.IsVisible) { GeneralDungeonFunctions.Erase(this.Position.X, this.Position.Y); }
        }

        //public Rat(int[] position)
        public Rat(int positionX, int positionY)
        {
            //this._position = position;
            this.Position = new Position(x: positionX, y: positionY);
            this.Color = ConsoleColor.Red;
            this.HP = 10;
            this.Type = "rat";
            this.AttackDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 3);
            this.DefenceDice = new Dice(numberOfDice: 1, sidesPerDice: 6, modifier: 1);
            this.IsVisible = false;
        }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(
                this.Position.X + GeneralDungeonFunctions.mapDisplacementX,
                this.Position.Y + GeneralDungeonFunctions.mapDisplacementY
            );
            Console.ForegroundColor = this.Color;
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
            Random random = new Random();
            string nextRelativePosition = relativePositions[random.Next(relativePositions.Length)];

            Position adjacentPosition;
            adjacentPosition = GeneralDungeonFunctions.GetAdjacentPosition(this.Position, nextRelativePosition);
            if (adjacentPosition.X == hero.Position.X && adjacentPosition.Y == hero.Position.Y)
            {
                this.AttackHero(hero);
                if(hero.HP > 0) { hero.Attack(levelData, this); }
                return;
            }

            
            bool possibleToMove = GeneralDungeonFunctions.isPositionEmpty(
                adjacentPosition,
                elements
            );
            if (possibleToMove)
            {
                GeneralDungeonFunctions.Erase(this.Position.X, this.Position.Y);
                this.Position = adjacentPosition;
                this.Draw();
            }
        }
    }
}
