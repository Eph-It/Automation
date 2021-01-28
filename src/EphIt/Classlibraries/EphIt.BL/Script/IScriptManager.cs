using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.Script
{
    public interface IScriptManager
    {
        Task<Db.Models.VMScript> FindAsync(int scriptId, bool includePublished = false);
        Task<List<Db.Models.VMScript>> SafeSearchScriptsAsync(string name, bool includePublished = false);
        Task<int> NewAsync(string name, string description);
        Task Update(int scriptId, string name = null, string description = null, int? PublishedVersion = null);
        Task Delete(int scriptId);
        Task<List<VMScriptVersion>> GetVersionAsync(int scriptId, bool IncldueAll);
        Task<int> NewVersionAsync(int scriptId, string scriptBody, short scriptLanguageId);
        Task<VMScript> PublishVersionAsync(int scriptId, int? scriptVersionID);
        int GetLatestVersionID(int scriptId);
    }
}
