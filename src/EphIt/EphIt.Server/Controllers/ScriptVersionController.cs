using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EphIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ScriptsRead")]
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
        [HttpGet]
        [EnableQuery]
        [Route("odata/[controller]")]
        public IQueryable<ScriptVersion> Get()
        {
            return _dbContext.ScriptVersion;
        }
        [EnableQuery]
        [HttpGet]
        [Route("odata/[controller]")]
        public SingleResult<ScriptVersion> Get([FromODataUri] int ScriptVersionId)
        {
            return SingleResult.Create(_dbContext.ScriptVersion.Where(p => p.ScriptVersionId == ScriptVersionId));
        }
        [HttpPost]
        [Route("odata/[controller]")]
        [Authorize("ScriptsModify")]
        public async Task<IActionResult> Post([FromBody] ScriptVersion scriptVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _ephItUser.RegisterCurrent();

            var script = _dbContext.Script.Where(p => p.ScriptId == scriptVersion.ScriptId).FirstOrDefault();
            if(script == null)
            {
                return BadRequest();
            }
            
            script.ModifiedByUserId = user.UserId;
            script.Modified = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            scriptVersion.CreatedByUserId = user.UserId;
            scriptVersion.Version = 1;
            scriptVersion.Created = DateTime.UtcNow;

            int? maxScriptVersion = _dbContext.ScriptVersion
                .Where(p => p.ScriptId.Equals(scriptVersion.ScriptId))
                .OrderByDescending(p => p.Version)
                .Select(p => p.Version)
                .FirstOrDefault();
            if (maxScriptVersion.HasValue) { scriptVersion.Version = maxScriptVersion.Value; }

            _dbContext.ScriptVersion.Add(scriptVersion);
            await _dbContext.SaveChangesAsync();
            return Ok(scriptVersion);
        }
        [HttpPost]
        [Route("Script/{scriptId}/Version")]
        [Authorize("ScriptsModify")]
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
