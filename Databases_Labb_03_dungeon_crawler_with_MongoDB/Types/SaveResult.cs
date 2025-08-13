using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.Types
{
    internal record SaveResult(string Id, bool Created, long ModifiedCount);
}
