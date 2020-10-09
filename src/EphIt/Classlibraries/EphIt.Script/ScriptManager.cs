using EphIt.Db.Models;
using EphIt.User;
using System;
using System.Linq;

namespace EphIt.Script
{
    public class ScriptManager : IScriptManager
    {
        private EphItContext _dbContext;
        private IEphItUser _ephItUser;
        public ScriptManager(EphItContext dbContext, IEphItUser ephitUser)
        {
            _dbContext = dbContext;
            _ephItUser = ephitUser;
        }
        public Db.Models.Script New(string name, string description)
        {
            var newScript = new EphIt.Db.Models.Script
            {
                Created = DateTime.UtcNow,
                CreatedByUserId = _ephItUser.Register().UserId,
                Description = description,
                Name = name
            };
            newScript.ModifiedByUserId = newScript.CreatedByUserId;
            _dbContext.Add(newScript);
            _dbContext.SaveChanges();
            return newScript;
        }
        public Db.Models.Script Get(string name)
        {
            return _dbContext.Script.Where(p => p.Name.Equals(name)).FirstOrDefault();
        }
        public Db.Models.Script Get(int id)
        {
            return _dbContext.Script.Find(id);
        }
        
    }
}
