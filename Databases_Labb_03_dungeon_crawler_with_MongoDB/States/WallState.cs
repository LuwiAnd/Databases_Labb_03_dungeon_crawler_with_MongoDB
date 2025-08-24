using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator("WallState")] // testar att skriva klassnamnet här.
    internal class WallState : LevelElementState
    {
        //public Position Position { get; set; } = new Position();
        public bool IsVisible { get; set; }

        public WallState() { }

        public WallState(Wall wall)
        {
            Type = "wall";
            Position = wall.Position;
            IsVisible = wall.IsVisible;
        }

        
    }
}
