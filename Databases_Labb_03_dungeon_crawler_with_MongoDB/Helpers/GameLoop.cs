using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Databases_Labb_03_dungeon_crawler_with_MongoDB.Types;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Helpers
{
    internal static class GameLoop
    {
        //public static async Task PlayGameAsync(
        public static async Task<bool> PlayGameAsync(
            LevelData levelData,
            Func<LevelData, Task?>? saveGameStateAsync = null
        )
        {

            if (levelData == null) throw new ArgumentNullException(nameof(levelData));

            saveGameStateAsync ??= (_ => Task.CompletedTask);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Använd piltangenterna för att spela och L för att se händelsehistoriken.");
            Console.WriteLine(""); // rad för spelarens attack
            Console.WriteLine(""); // rad för fiendens attack
            Console.WriteLine("");


            bool gameOver = false;
            bool continueToPlay = true;
            while (!gameOver)
            {
                if (levelData.GameStatus == GameStatus.Completed || levelData.GameStatus == GameStatus.HeroDead) break;

                if (levelData.TurnsUntilClearingMessages > 0) levelData.TurnsUntilClearingMessages--;
                else GeneralDungeonFunctions.ClearConsoleMessages();

                continueToPlay = levelData.Hero.Update(levelData);

                if (!continueToPlay) break;

                if (CheckIsHeroDead(levelData.Hero)) 
                {
                    levelData.GameStatus = GameStatus.HeroDead;
                    await saveGameStateAsync(levelData);
                    break;
                } 

                levelData.UpdateWalls();
                levelData.UpdateGoal();
                levelData.UpdateSnakes();
                if (CheckIsHeroDead(levelData.Hero)) break;
                levelData.UpdateRats();
                if (CheckIsHeroDead(levelData.Hero)) break;

                levelData.Hero.Draw();
                levelData.EraseDeadElements();
                levelData.RemoveElements();

                await saveGameStateAsync(levelData);
            }

            return continueToPlay;
        }

        private static bool CheckIsHeroDead(Hero hero)
        {
            if (hero.HP <= 0)
            {
                Console.SetCursorPosition(10, 22);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Du dog! Spelet är slut!");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                //break;
                return true;
            }
            return false;
        }

    }
}
