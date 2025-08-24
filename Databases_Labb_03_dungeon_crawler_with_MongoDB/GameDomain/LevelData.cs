
// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
using System.Runtime.InteropServices;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Factories;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{

    class LevelData
    {
        //private List<LevelElement> _elements = new List<LevelElement>();
        //public List<LevelElement> Elements
        //{
        //    get { return _elements; }
        //    //set; // Denna property ska vara readonly.
        //    set { _elements = value; }  // För databaskursen ska jag ha en set, vilket jag inte hade i grundkursen.
        //}

        public List<LevelElement> Elements { get; set; } = new();


        //public Hero hero;
        public Hero Hero { get; set; } //= new();

        public int TurnsUntilClearingMessages { get; set; } = GeneralDungeonFunctions.TurnsUntilClearingMessages;

        // Nya properties för databaskursen.

        public List<string> Messages { get; set; } = new();

        // Denna ska användas för att beräkna score.
        public int TurnCount { get; set; }

        public GameStatus GameStatus { get; set; } = GameStatus.Ongoing;
        public int TotalDamageDealt { get; set; } = 0;
        public int KillBonus { get; set; } = 0;

        public LevelData() { }

        public LevelData(LevelDataState state)
        {
            Hero = new Hero(state.Hero);
            Elements = state.Elements
                .Select(LevelElementFactory.FromState)
                .ToList();

            // Jag tror inte att en LevelDataState kan ha en Messages
            // som är null, men det här är för säkerhets skull.
            Messages = state.Messages?.ToList() ?? new();
        }

        public void Load(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    int y = 0;
                    //int[] currentPosition;
                    //LevelElement le;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        for (int x = 0; x < line.Length; x++)
                        {
                            //currentPosition = new int[] { x, y };
                            if (line[x] == GeneralDungeonFunctions.wallChar)
                            {
                                //_elements.Add(new Wall(x, y));
                                Elements.Add(new Wall(x, y));
                            }
                            if (line[x] == GeneralDungeonFunctions.goalChar)
                            {
                                //_elements.Add(new Wall(x, y));
                                Elements.Add(new Goal(x, y));
                            }
                            if (line[x] == GeneralDungeonFunctions.ratChar)
                            {
                                //_elements.Add(new Rat(x, y));
                                Elements.Add(new Rat(x, y));
                            }
                            if (line[x] == GeneralDungeonFunctions.snakeChar)
                            {
                                Elements.Add(new Snake(x, y));
                            }
                            if (line[x] == GeneralDungeonFunctions.playerChar)
                            {
                                //hero = new Hero(x, y);
                                Hero = new Hero(x, y);
                            }
                        }
                        y++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            //hero.Draw();
            Hero.Draw();
        }

        public void RemoveElements()
        {
            //foreach(var element in this.Elements)
            foreach (var element in Elements.ToList())
            {
                if (element.Type == "rat" || element.Type == "snake")
                {
                    Enemy enemy = (Enemy)element;
                    if (enemy.HP <= 0) { Elements.Remove(element); }
                }
            }
        }

        public void UpdateGoal()
        {
            foreach(LevelElement element in Elements)
            {
                if(element.Type == "goal")
                {
                    Goal goal = (Goal)element;
                    goal.Update(Hero);
                }
            }
        }

        public void UpdateWalls()
        {
            foreach (LevelElement element in Elements)
            {
                if (element.Type == "wall")
                {
                    //Console.WriteLine("Moving a snake!");
                    Wall wall = (Wall)element;
                    //wall.Update(hero);
                    wall.Update(Hero);
                }
            }
        }

        public void UpdateSnakes()
        {
            foreach (LevelElement element in Elements)
            {
                if (element.Type == "snake")
                {
                    Snake snake = (Snake)element;
                    //snake.Update(this.hero, this.Elements);
                    //snake.Update(hero, this);
                    snake.Update(Hero, this);
                }
            }
        }

        public void UpdateRats()
        {
            foreach (LevelElement element in Elements)
            {
                if (element.Type == "rat")
                {
                    Rat rat = (Rat)element;
                    //rat.Update(this.hero, this.Elements);
                    //rat.Update(hero, this);
                    rat.Update(Hero, this);
                }
            }
        }

        public void EraseDeadElements()
        {
            foreach (var element in Elements)
            {
                if (element is Enemy enemy && enemy.HP <= 0)
                {
                    GeneralDungeonFunctions.Erase(enemy.Position.X, enemy.Position.Y);
                }
            }
        }

        public void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            Messages.Add(message);
        }


        public int ComputeScore()
        {
            int score = (int)Math.Round(Hero.HP) + TotalDamageDealt + KillBonus - TurnCount;
            return Math.Max(0, score);
        }


        // Hjälpfunktion.
        public void LoadFromLayout(string layout)
        {
            if (string.IsNullOrEmpty(layout))
                throw new ArgumentException("Layout är tom.", nameof(layout));

            using var sr = new StringReader(layout);

            Elements.Clear();
            Hero = null;

            string? line;
            int y = 0;

            while ((line = sr.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    var ch = line[x];

                    if (ch == GeneralDungeonFunctions.wallChar) Elements.Add(new Wall(x, y));
                    else if (ch == GeneralDungeonFunctions.ratChar) Elements.Add(new Rat(x, y));
                    else if (ch == GeneralDungeonFunctions.snakeChar) Elements.Add(new Snake(x, y));
                    else if (ch == GeneralDungeonFunctions.playerChar) Hero = new Hero(x, y);
                    else if (ch == GeneralDungeonFunctions.goalChar) Elements.Add(new Goal(x, y));
                }
                y++;
            }

            Hero?.Draw();
        }


        public void RenderInitialFrame(bool drawAllElements = false)
        {
            Console.Clear();

            foreach (var e in Elements)
            {
                if (drawAllElements || e.IsVisible || GeneralDungeonFunctions.IsVisible(Hero.Position, e.Position))
                {
                    e.Draw();
                }
            }

            Hero?.Draw();
        }

    }
}