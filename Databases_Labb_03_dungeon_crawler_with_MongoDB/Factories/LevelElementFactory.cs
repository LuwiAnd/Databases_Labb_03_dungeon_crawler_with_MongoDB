using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.States;
using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Factories
{
    internal static class LevelElementFactory
    {
        public static LevelElement FromState(LevelElementState state)
        {
            return state.Type switch
            {
                 "hero" when state is  HeroState  heroState => new  Hero( heroState),
                 "wall" when state is  WallState  wallState => new  Wall( wallState),
                 "goal" when state is  GoalState  goalState => new  Goal( goalState),
                  "rat" when state is   RatState   ratState => new   Rat(  ratState),
                "snake" when state is SnakeState snakeState => new Snake(snakeState),
                _ => throw new ArgumentException($"Unknown LevelElementState type: {state.Type}")
            };
        }

        // ToState() blir lite enklare att koda för att elementens typ är mer felsäkra när de kommer från kod
        // istället för att läsas in från en databas som någon kan ha mixtrat med.
        public static LevelElementState ToState(LevelElement element)
        {
            return element.Type switch
            {
                "hero" => new HeroState((Hero)element),
                "wall" => new WallState((Wall)element),
                "goal" => new GoalState((Goal)element),
                "rat" => new RatState((Rat)element),
                "snake" => new SnakeState((Snake)element),
                _ => throw new ArgumentException($"Unknown LevelElement type: {element.Type}")
            };
        }
    }
}
