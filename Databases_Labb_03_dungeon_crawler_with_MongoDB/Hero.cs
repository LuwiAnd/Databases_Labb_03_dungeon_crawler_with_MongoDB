using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;

namespace Labb_02_dungeon_crawler
{
    internal class Hero : LevelElement
    {
        //private Position _position;
        //public Position Position { get; private set; }


        //private int[] _position;
        //public override int[] Position
        //{
        //    get { return this._position; }
        //    // positionen får endast sättas av konstruktorn, eller av Update-funktionen.
        //}

        //public override Position Position { get => base.Position; protected set => base.Position = value; }
        private void SetPosition(Position p)
        {
            this.Position = p;
        }

        public new string Type { get; set; }
        public double HP { get; set; }

        public Dice AttackDice = new Dice(numberOfDice: 2, sidesPerDice: 6, modifier: 2);
        public Dice DefenceDice = new Dice(numberOfDice: 2, sidesPerDice: 6, modifier: 0);

        

        //public Hero(int[] position)
        public Hero(int positionX, int positionY)
        {
            //this._position = position;
            this.Position = new Position(x: positionX, y: positionY);
            this.HP = 100;
            this.Type = "hero";

            this.Color = ConsoleColor.Green;
            this.Appearance = GeneralDungeonFunctions.playerChar;
        }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            //Console.SetCursorPosition(this._position[0] + GeneralDungeonFunctions.mapDisplacementX, this._position[1] + GeneralDungeonFunctions.mapDisplacementY);
            Console.SetCursorPosition(this.Position.X + GeneralDungeonFunctions.mapDisplacementX, this.Position.Y + GeneralDungeonFunctions.mapDisplacementY);
            Console.ForegroundColor = this.Color;
            Console.Write(this.Appearance.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);

        }

        //public void Update(List<LevelElement> elements)
        public void Update(LevelData levelData)
        {

            List<LevelElement> elements = levelData.Elements;

            ConsoleKeyInfo cki;
            int intervalTime = 50;
            bool okDirection = false;
            //bool enemyInTheWay = false;
            //bool enemyMovedAway = false;

            //while (!okDirection && currentWaitTime <= maxWaitTime)
            while (!okDirection)
            {
                //Console.WriteLine("BBBBBB");
                //while (Console.KeyAvailable == false && currentWaitTime <= maxWaitTime)
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(intervalTime);
                    //currentWaitTime += intervalTime;
                }

                //Console.WriteLine("CCCCC");
                if (Console.KeyAvailable)
                {
                    //Console.WriteLine("DDDDD");
                    cki = Console.ReadKey();
                    //Console.WriteLine("You pressed the '{0}' key", cki.Key);
                    okDirection = true;
                    //enemyInTheWay = false;
                    //enemyMovedAway = false;

                    if (cki.Key == ConsoleKey.UpArrow)
                    {
                        /*
                        //okDirection = true;
                        //bool heroHasElementAbove = false;
                        foreach (var element in elements)
                        {
                            //if (element.Position[0] == this.Position[0] && element.Position[1] == (this.Position[1] - 1))
                            if (element.Position.X == this.Position.X && element.Position.Y == (this.Position.Y - 1))
                            {
                                enemyInTheWay = true;
                                if (element.Type != "wall")
                                {
                                    Enemy enemy = (Enemy)element;
                                    // Attack the enemy.
                                    Attack(levelData, enemy);
                                    if (enemy.HP > 0) { enemy.AttackHero(this); }
                                }
                                else
                                {
                                    okDirection = false;
                                }
                                break;
                            }

                        }
                        if (okDirection && !enemyInTheWay)
                        {
                            //this.Position.Y--;
                            //this.SetPosition(new Position(x: this.Position.X, y: this.Position.Y - 1));
                            this.Move("up");
                        }
                        */
                        okDirection = this.HandleArrowKeys(direction: "up", levelData: levelData);
                    }else if (cki.Key == ConsoleKey.LeftArrow)
                    {
                        /*
                        //okDirection = true;
                        //bool heroHasElementAbove = false;
                        foreach (var element in elements)
                        {
                            if (element.Position.X == (this.Position.X - 1) && element.Position.Y == this.Position.Y)
                            {
                                enemyInTheWay = true;
                                if (element.Type != "wall")
                                {
                                    Enemy enemy = (Enemy)element;
                                    Attack(levelData, enemy);
                                    if (enemy.HP > 0) { enemy.AttackHero(this); }
                                }
                                else
                                {
                                    okDirection = false;
                                }
                            }

                        }
                        if (okDirection && !enemyInTheWay)
                        {
                            //this.Position[0]--;
                            //this.SetPosition(new Position(x: this.Position.X - 1, y: this.Position.Y));
                            this.Move("left");
                        }
                        */
                        okDirection = this.HandleArrowKeys(direction: "left", levelData: levelData);
                    }else if (cki.Key == ConsoleKey.DownArrow)
                    {
                        /*
                        foreach (var element in elements)
                        {
                            //if (element.Position[0] == this.Position[0] && element.Position[1] == (this.Position[1] + 1))
                            if (element.Position.X == this.Position.X && element.Position.Y == (this.Position.Y + 1))
                            {
                                enemyInTheWay = true;
                                if (element.Type != "wall")
                                {
                                    Enemy enemy = (Enemy)element;
                                    // Attack the enemy.
                                    Attack(levelData, enemy);
                                    if (enemy.HP > 0) { enemy.AttackHero(this); }
                                }
                                else
                                {
                                    okDirection = false;
                                }
                            }

                        }
                        if (okDirection && !enemyInTheWay)
                        {
                            //this.Position[1]++;
                            //this.SetPosition(new Position(x: this.Position.X, y: this.Position.Y + 1));
                            this.Move("down");
                        }
                        */

                        okDirection = this.HandleArrowKeys(direction: "down", levelData: levelData);
                    }else if (cki.Key == ConsoleKey.RightArrow)
                    {
                        /* 
                        //okDirection = true;
                        //bool heroHasElementAbove = false;
                        //int debugIndex = 0;
                        foreach (var element in elements)
                        {
                            //if (element.Position[0] == (this.Position[0] + 1) && element.Position[1] == this.Position[1])
                            if (element.Position.X == (this.Position.X + 1) && element.Position.Y == this.Position.Y)
                            {
                                enemyInTheWay = true;
                                if (element.Type != "wall")
                                {
                                    Enemy enemy = (Enemy)element;
                                    // Jag hade tolkat ormens rörelser fel. Nedanstående bortkommenterade kodrad behövs inte för att ormarna
                                    // kan uppdateras i game-loopen. Hero ska aldrig stå bredvid ormarna om ormarna inte har något ivägen.
                                    //if(element.Type == "snake") { Snake snake = (Snake)enemy; enemyInTheWay = !snake.Move("right", elements); }
                                    Attack(levelData, enemy);
                                    if (enemy.HP > 0) { enemy.AttackHero(this); }
                                }
                                else
                                {
                                    okDirection = false;
                                }
                            }

                        }
                        if (okDirection && !enemyInTheWay)
                        {
                            //this.Position[0]++;
                            //this.SetPosition(new Position(x: this.Position.X + 1, y: this.Position.Y));
                            this.Move("right");
                        }
                        */
                        okDirection = this.HandleArrowKeys(direction: "right", levelData: levelData);
                    }
                    else
                    {
                        // The user stands still.
                        okDirection = true;
                    }
                }
            }
        }

        private bool HandleArrowKeys(string direction, LevelData levelData)
        {
            Position positionToMoveTo = this.Position;
            switch(direction){
                case "up":
                    positionToMoveTo.Y -= 1;
                    break;
                case "down":
                    positionToMoveTo.Y += 1;
                    break;
                case "left":
                    positionToMoveTo.X -= 1;
                    break;
                case "right":
                    positionToMoveTo.X += 1;
                    break;
            }

            List<LevelElement> elements = levelData.Elements;
            bool enemyInTheWay = false;
            bool okDirection = true;
            foreach (var element in elements)
            {
                if (element.Position.X == positionToMoveTo.X && element.Position.Y == positionToMoveTo.Y)
                {
                    enemyInTheWay = true;
                    if (element.Type != "wall")
                    {
                        Enemy enemy = (Enemy)element;
                        Attack(levelData, enemy);
                        if (enemy.HP > 0) { enemy.AttackHero(this); }
                    }
                    else
                    {
                        okDirection = false;
                    }
                }

            }
            if (okDirection && !enemyInTheWay)
            {
                //this.Position[0]++;
                //this.SetPosition(new Position(x: this.Position.X + 1, y: this.Position.Y));
                //this.Move("right");
                this.Move(direction);
            }

            return okDirection;
        }

        public void Attack(LevelData levelData, Enemy enemy)
        {
            levelData.TurnsUntilClearingMessages = 3;
            int heroAttack = AttackDice.Throw();
            int enemyDefence = enemy.DefenceDice.Throw();
            int enemyDamage = heroAttack - enemyDefence;
            if (enemyDamage < 0) { enemyDamage = 0; }
            enemy.HP -= enemyDamage;

            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Player (HP: {this.HP}) throw dices: {this.AttackDice.ToString()} => {heroAttack}. {enemy.Type} (HP: {enemy.HP}) throw: {enemy.DefenceDice.ToString()} => {enemyDefence}. Damage = {enemyDamage}.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
            
        }

        public void Move(string direction)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(this.Position.X + GeneralDungeonFunctions.mapDisplacementX, this.Position.Y + GeneralDungeonFunctions.mapDisplacementY);
            Console.Write(' ');
            if (direction == "up")
            {
                this.SetPosition(new Position(x: this.Position.X, y: this.Position.Y - 1));
                this.Draw();
            }
            else if(direction == "down")
            {
                this.SetPosition(new Position(x: this.Position.X, y: this.Position.Y + 1));
                this.Draw();
            }
            else if (direction == "left")
            {
                this.SetPosition(new Position(x: this.Position.X - 1, y: this.Position.Y));
                this.Draw();
            }
            else if (direction == "right")
            {
                this.SetPosition(new Position(x: this.Position.X + 1, y: this.Position.Y));
                this.Draw();
            }
            Console.SetCursorPosition(left, top);
        }

        
    }
}
