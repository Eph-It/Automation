using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNet.OData;
using OMyEF.Server;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EphIt.BL.ODataExtensions
{
    public class ODataScripts : OMyEFControllerExtensions<EphIt.Db.Models.Script>
    {
        private readonly EphItContext _dbContext;
        private readonly IEphItUser _ephItUser;
        public ODataScripts(EphItContext context, IEphItUser ephItUser)
        {
            _dbContext = context;
            DbContext = _dbContext;
            _ephItUser = ephItUser;
        }
        public override async Task<EphIt.Db.Models.Script> PostAsync(EphIt.Db.Models.Script scriptObject)
        {
            var currentUser = _ephItUser.Register();
            scriptObject.CreatedByUserId = currentUser.UserId;
            scriptObject.Created = DateTime.UtcNow;
            scriptObject.Modified = DateTime.UtcNow;
            scriptObject.ModifiedByUserId = currentUser.UserId;
            return await base.PostAsync(scriptObject);
        }
        public override async Task<Db.Models.Script> PatchAsync(Delta<Db.Models.Script> obj, object key)
        {
            var entity = await _dbContext.Script.FindAsync(key);
            if (entity == null) { return null; }

            obj.Patch(entity);
            entity.Modified = DateTime.UtcNow;
            entity.ModifiedByUserId = _ephItUser.Register().UserId;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public override Task<Db.Models.Script> PutAsync(Db.Models.Script obj)
        {
            obj.Modified = DateTime.UtcNow;
            obj.ModifiedByUserId = _ephItUser.Register().UserId;
            return base.PutAsync(obj);
        }
        public override async Task DeleteAsync(object key)
        {
            var entity = await _dbContext.Script.FindAsync(key);
            entity.IsDeleted = true;
            entity.ModifiedByUserId = _ephItUser.Register().UserId;
            entity.Modified = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
}
