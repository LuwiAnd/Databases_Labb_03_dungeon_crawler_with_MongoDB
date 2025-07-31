
// Om jag inte hade klickat i "Do not use top level statement" när jag skapade detta projekt, så hade jag inte behövt skriva namespace runt varje klass.
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers;

namespace Labb_02_dungeon_crawler
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
        public char Appearance { 
            get; 
            set; 
        }
        public ConsoleColor Color { get; set; }

        public override void Draw()
        {
            (int left, int top) = Console.GetCursorPosition();
            //Console.SetCursorPosition(this.Position[0], this.Position[1]);
            //Console.SetCursorPosition(this.Position[0] + GeneralDungeonFunctions.mapDisplacementX, this.Position[1] + GeneralDungeonFunctions.mapDisplacementY);
            Console.SetCursorPosition(this.Position.X + GeneralDungeonFunctions.mapDisplacementX, this.Position.Y + GeneralDungeonFunctions.mapDisplacementY);
            Console.ForegroundColor = this.Color;
            Console.Write(this.Appearance.ToString());

            // Resetting color and cursor, for debugging output to be shown under the dungeon.
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        //public Wall(int[] position)
        public Wall(int positionX, int positionY)
        {
            Appearance = '#';
            this.Color = ConsoleColor.White;
            //this.Position = position;
            this.Position = new Position(x: positionX, y: positionY);
            this.Type = "wall";
            //this.isVisible = false;
            this.IsVisible = false;
        }

        public void Update(Hero hero)
        {
            if(GeneralDungeonFunctions.IsVisible(hero.Position, this.Position))
            {
                //this.isVisible = true;
                this.IsVisible = true;
                this.Draw();
            }
        }
    }
}
