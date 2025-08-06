using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.States
{
    internal class LevelDataState
    {
        public List<LevelElementState> Elements { get; set; } = new();
        public HeroState Hero { get; set; } = new();

        public LevelDataState(LevelData levelData)
        {
            Hero = new HeroState(levelData.Hero);
            Elements = levelData.Elements
                .Where(e => e.Type != "hero") // Om jag skulle vilja lägga hero i Elements i framtiden.
                .Select(LevelElementStateFactory.CreateFrom)
                .ToList();
        }
    }
}
