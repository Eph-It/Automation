using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphIt.Authorization;
using EphIt.Db.Models;
using EphIt.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EphIt.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize("Script")]
    public class ScriptController : ControllerBase
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        private IUserAuthorization _userAuth;
        public ScriptController(EphItContext dbContext, IEphItUser ephItUser, IUserAuthorization userAuth)
        {
            _dbContext = dbContext;
            _ephItUser = ephItUser;
            _userAuth = userAuth;
        }
        [HttpGet]
        [Route("/api/[controller]/{scriptId}")]
        [Authorize("ScriptRead")]
        public Script Get(int scriptId)
        {
            return _dbContext.Script.Find(scriptId);
        }
        [HttpGet]
        [Route("/api/[controller]")]
        public IEnumerable<Script> Get(string Name)
        {
            return _dbContext.Script.Where(p => p.Name.Contains(Name)).ToList();
        }
        [HttpPost]
        [Route("/api/[controller]")]
        [Authorize("ScriptEdit")]
        public Script New([FromBody] ScriptPostParameters postParams)
        {
            var newScript = new EphIt.Db.Models.Script
            {
                Created = DateTime.UtcNow,
                CreatedByUserId = _ephItUser.Register().UserId,
                Description = postParams.Description,
                Name = postParams.Name,
                Modified = DateTime.UtcNow
            };
            newScript.ModifiedByUserId = newScript.CreatedByUserId;
            _dbContext.Add(newScript);
            _dbContext.SaveChanges();

            if (_userAuth.AuthenticatedWith().HasValue
                //&& _userAuth.AuthenticatedWith() != 1
            )
            {
                var newScriptRBAC = new EphIt.Db.Models.RoleObjectScopeScript();
                newScriptRBAC.RoleId = _userAuth.AuthenticatedWith().Value;
                newScriptRBAC.ScriptId = newScript.ScriptId;
                _dbContext.Add(newScriptRBAC);
                _dbContext.SaveChangesAsync();
            }
            
            return _dbContext.Script.Find(newScript.ScriptId);
        }
        [HttpPut]
        [Route("[controller]/{scriptId}")]
        [Authorize("ScriptEdit")]
        public Script Update(int scriptId, [FromBody] ScriptPostParameters postParams)
        {
            var script = _dbContext.Script.Find(scriptId);
            if (!string.IsNullOrEmpty(postParams.Name))
            {
                script.Name = postParams.Name;
                script.Modified = DateTime.UtcNow;
                script.ModifiedByUserId = _ephItUser.Register().UserId;
            }
            if (!string.IsNullOrEmpty(postParams.Description))
            {
                script.Description = postParams.Description;
                script.Modified = DateTime.UtcNow;
                script.ModifiedByUserId = _ephItUser.Register().UserId;
            }
            _dbContext.SaveChanges();
            return script;
        }
        [HttpDelete]
        [Route("[controller]/{scriptId}")]
        [Authorize("ScriptDelete")]
        public void Delete(int scriptId)
        {
            var script = _dbContext.Script.Find(scriptId);
            _dbContext.Remove(script);
            _dbContext.SaveChanges();
        }
    }
    public class ScriptPostParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Published_Version { get; set; }
    }
}
