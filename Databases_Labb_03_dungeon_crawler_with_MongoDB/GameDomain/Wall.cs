
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    class Wall : LevelElement
    {
        // Man ska i princip aldrig ha public fields som denna.
        //public new string type = "wall";
        //public string type;
        //private bool isVisible = false;

        // Utan override, så skapas det en extra Position-property här, så att det 
        // hade funnits en Position i föräldraklassen och en i denna klass.
        //public override int[] Position { 
        //    get; 
        //    // set; 
        //}
        public char Appearance
        {
            get;
            set;
        }
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

        //public Wall(int[] position)
        public Wall(int positionX, int positionY)
        {
            Position = new Position(x: positionX, y: positionY);
            IsVisible = false;

            Appearance = '#';
            Color = ConsoleColor.White;
            Type = "wall";
        }

        public Wall(WallState state)
        {
            Position = state.Position;
            IsVisible = state.IsVisible;

            Appearance = '#';
            Color = ConsoleColor.White;
            Type = "wall";
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
