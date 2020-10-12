using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.Script
{
    public interface IScriptManager
    {
        Task<Db.Models.Script> FindAsync(int scriptId, bool includePublished = false);
        Task<IEnumerable<Db.Models.Script>> SafeSearchScriptsAsync(string name, bool includePublished = false);
        Task<int> NewAsync(string name, string description);
        Task Update(int scriptId, string name = null, string description = null, int? PublishedVersion = null);
        Task Delete(int scriptId);
    }
}
