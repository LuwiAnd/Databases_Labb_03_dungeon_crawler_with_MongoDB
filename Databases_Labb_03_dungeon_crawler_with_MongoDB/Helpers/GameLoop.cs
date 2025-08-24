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
        public static async Task PlayGameAsync(
            LevelData levelData,
            Func<LevelData, Task?> saveGameStateAsync = null,
            CancellationToken ct = default
        )
        {

            if (levelData == null) throw new ArgumentNullException(nameof(levelData));

            saveGameStateAsync ??= (_ => Task.CompletedTask);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(""); // rad för spelarens attack
            Console.WriteLine(""); // rad för fiendens attack

            bool gameOver = false;
            while (!gameOver)
            {
                if (levelData.GameStatus == GameStatus.Completed || levelData.GameStatus == GameStatus.HeroDead) break;

                if (levelData.TurnsUntilClearingMessages > 0) levelData.TurnsUntilClearingMessages--;
                else GeneralDungeonFunctions.ClearConsoleMessages();

                levelData.Hero.Update(levelData);
                if (CheckIsHeroDead(levelData.Hero)) break;

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
        }

        private static bool CheckIsHeroDead(Hero hero)
        {
            if (hero.HP <= 0)
            {
                Console.SetCursorPosition(10, 22);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("You died! Game over!");
                Console.ForegroundColor = ConsoleColor.White;
                //break;
                return true;
            }
            return false;
        }

    }
}
