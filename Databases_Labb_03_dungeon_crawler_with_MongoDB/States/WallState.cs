using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    internal class WallState
    {
        public Position Position { get; set; } = new Position();
        public bool IsVisible { get; set; }

        public WallState() { }

        public WallState(Wall wall)
        {
            Position = wall.Position;
            IsVisible = wall.IsVisible;
        }
    }
}
