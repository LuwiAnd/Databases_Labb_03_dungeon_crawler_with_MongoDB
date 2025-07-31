
namespace Labb_02_dungeon_crawler
{
    // Om jag inte hade skrivit att denna klass skulle vara public, så 
    // hade den blivit internal, vilket betyder att alla klasser i samma 
    // assembly (projekt som man gjort build på).
    public abstract class LevelElement
    {
        public string Type { get; set; }

        public bool IsVisible { get; set; }

        // abstract om jag vill att jag måste override:a
        // virtual om jag vill att det ska gå att override:a
        //public abstract int[] Position { 
        //public abstract Position Position { 
        //    get; 
        //    set;
        //}

        public virtual Position Position { get; protected set; }
        public virtual char Appearance { get; set; }
        public virtual ConsoleColor Color { get; set; }

        public abstract void Draw();

    }
}