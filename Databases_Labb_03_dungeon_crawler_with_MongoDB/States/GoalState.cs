using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    [BsonDiscriminator]
    internal class GoalState : LevelElementState
    {
        public bool IsVisible { get; set; }

        public GoalState() { }

        public GoalState(Goal goal)
        {
            Type = "goal";
            Position = goal.Position;
            IsVisible = goal.IsVisible;
        }
    }
}
