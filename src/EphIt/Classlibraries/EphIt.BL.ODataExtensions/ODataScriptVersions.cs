using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using OMyEF.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EphIt.BL.ODataExtensions
{
    public class ODataScriptVersions : OMyEFControllerExtensions<EphIt.Db.Models.ScriptVersion>
    {
        private readonly EphItContext _dbContext;
        private readonly IEphItUser _ephItUser;
        public ODataScriptVersions(EphItContext context, IEphItUser ephItUser)
        {
            _dbContext = context;
            DbContext = _dbContext;
            _ephItUser = ephItUser;
        }
        private void ThrowIfScriptNotExist(int scriptId)
        {
            if (!_dbContext.Script.Where(p => p.ScriptId == scriptId && !p.IsDeleted).Any())
            {
                throw new KeyNotFoundException("Script not found");
            }
        }
        public override Task<ScriptVersion> PostAsync(ScriptVersion obj)
        {
            ThrowIfScriptNotExist(obj.ScriptId);
            int? maxScriptVersion = _dbContext.ScriptVersion
                            .Where(p => p.ScriptId == obj.ScriptId)
                            .OrderByDescending(desc => desc.Version)
                            .Select(p => p.Version)
                            .FirstOrDefault();
            if (!maxScriptVersion.HasValue)
            {
                obj.Version = 1;
            }
            else
            {
                obj.Version = maxScriptVersion.Value + 1;
            }
            obj.Created = DateTime.UtcNow;
            obj.CreatedByUserId = _ephItUser.Register().UserId;
            return base.PostAsync(obj);
        }
        public override async Task<ScriptVersion> PatchAsync(Delta<ScriptVersion> obj, object key)
        {
            var entity = await _dbContext.ScriptVersion
                            .Where(p => p.ScriptVersionId.Equals((int)key))
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            if (entity == null) { return null; }
            
            obj.Patch(entity);
            
            // shouldnt be able to edit Versions - so create a new one if an edit command comes in
            return await PostAsync(entity);
        }
        public override async Task<ScriptVersion> PutAsync(ScriptVersion obj)
        {
            return await PostAsync(obj);
        }
        public override async Task DeleteAsync(object key)
        {
            var version = await _dbContext.ScriptVersion.FindAsync(key);
            version.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
