using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphIt.Db.Models;
using EphIt.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EphIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Script")]
    public class ScriptVersionController : ControllerBase
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        public ScriptVersionController(EphItContext dbContext, IEphItUser ephItUser)
        {
            _dbContext = dbContext;
            _ephItUser = ephItUser;
        }
        [HttpGet]
        [Route("Script/{scriptId}/Version/Latest")]
        public ScriptVersion GetLatest(int scriptId)
        {
            return _dbContext.ScriptVersion.Where(p => 
                p.ScriptId.Equals(scriptId) 
                && p.Script.PublishedVersion.Equals(p.ScriptVersionId)
            ).FirstOrDefault();
        }
        [HttpGet]
        [Route("Script/{scriptId}/Version")]
        public List<ScriptVersion> GetVersions(int scriptId)
        {
            return _dbContext.ScriptVersion.Where(p =>
                p.ScriptId.Equals(scriptId)
            ).ToList();
        }
        [HttpGet]
        [Route("Script/{scriptId}/Version/{VersionId}")]
        public List<ScriptVersion> GetVersions(int scriptId, int VersionId)
        {
            return _dbContext.ScriptVersion.Where(p =>
                p.ScriptId.Equals(scriptId)
                && p.ScriptVersionId.Equals(VersionId)
            ).ToList();
        }
        [HttpPost]
        [Route("Script/{scriptId}/Version")]
        [Authorize("ScriptEdit")]
        public ScriptVersion New(int scriptId, [FromBody]string body)
        {
            int? maxScriptVersion = _dbContext.ScriptVersion
                        .Where(p => p.ScriptId.Equals(scriptId))
                        .OrderByDescending(p => p.Version)
                        .Select(p => p.Version)
                        .FirstOrDefault();

            var userId = _ephItUser.Register().UserId;
            var scriptVersion = new ScriptVersion
            {
                CreatedByUserId = userId,
                Created = DateTime.UtcNow,
                Body = body,
                ScriptId = scriptId,
                Version = 1
            };
            if (maxScriptVersion.HasValue)
            {
                scriptVersion.Version = maxScriptVersion.Value + 1;
            }
            _dbContext.Add(scriptVersion);
            _dbContext.SaveChanges();
            return scriptVersion;
        }
    }
}
