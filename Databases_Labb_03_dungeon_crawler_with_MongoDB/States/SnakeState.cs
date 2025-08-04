using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    internal class SnakeState
    {
        public Position Position { get; set; } = new Position();
        public double HP { get; set; }

        public SnakeState() { }

        public SnakeState(Snake snake)
        {
            Position = snake.Position;
            HP = snake.HP;
        }
    }
}
