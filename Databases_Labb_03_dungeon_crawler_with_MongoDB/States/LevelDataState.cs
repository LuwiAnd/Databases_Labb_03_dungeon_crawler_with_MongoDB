using Databases_Labb_03_dungeon_crawler_with_MongoDB.Factories;
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
        public List<string> Messages { get; set; }
        public int TurnCount { get; set; }

        public LevelDataState(LevelData levelData)
        {
            Hero = new HeroState(levelData.Hero);
            TurnCount = levelData.TurnCount;
            Elements = levelData.Elements
                .Where(e => e.Type != "hero") // Om jag skulle vilja/råka lägga hero i Elements i framtiden.
                //.Select(LevelElementStateFactory.CreateFrom)
                .Select(LevelElementFactory.ToState)
                .ToList();

            //Messages = levelData.Messages; // Detta verkar göra att jag har två refernser till samma lista istället för att ha en kopia i objekt av denna klass.
            Messages = new List<string>(levelData.Messages);
        }

        public LevelDataState()
        {
        }
    }
}
