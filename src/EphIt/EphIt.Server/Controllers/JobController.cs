using EphIt.BL.Authorization;
using EphIt.BL.Automation;
using EphIt.BL.JobManager;
using EphIt.BL.Script;
using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EphIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("JobsRead")]
    public class JobController : Controller
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        private IUserAuthorization _userAuth;
        private IJobManager _jobManager;
        private IScriptManager _scriptManager;
        public JobController(EphItContext dbContext, IEphItUser ephItUser, IUserAuthorization userAuth, IJobManager jobManager, IScriptManager scriptManager)
        {
            _dbContext = dbContext;
            _ephItUser = ephItUser;
            _userAuth = userAuth;
            _jobManager = jobManager;
            _scriptManager = scriptManager;
        }
        [HttpPost]
        [Route("/api/[controller]")]
        [Authorize("JobsModify")]
        public Guid? New([FromBody] JobPostParameters postParams)
        {
            //this DB call should be in a BL eventually.
            ScriptVersion ver = _dbContext.ScriptVersion.Where(v => v.ScriptVersionId == postParams.ScriptVersionID).FirstOrDefault();
            if(ver != null)
            {
                return _jobManager.QueueJob(ver, postParams.Parameters, _ephItUser.Register().UserId, postParams.ScheduleID, postParams.AutomationID);
            }
            return null;
        }
        [HttpGet]
        [EnableQuery]
        [Route("/odata/[controller]")]
        public IEnumerable<Job> Get()
        {
            return _dbContext.Job;
        }
        [HttpGet]
        [Route("/api/[controller]/Queued")]
        [Authorize("JobsExecute")]
        public VMScriptJob GetQueuedJob(ScriptLanguageEnum language)
        {
            //is this a runbook worker or something else authorized? TODO

            return _jobManager.GetQueuedScriptedJob(language);

        }

        [HttpPost]
        [Route("/api/[controller]/{jobID}/Start")]
        [Authorize("JobsExecute")]
        public void StartJob(Guid jobId)
        {
            //is this a runbook worker or something else authorized? TODO
            _jobManager.Start(jobId);
        }

        [HttpPost]
        [Route("/api/[controller]/{jobID}/Finish")]
        [Authorize("JobsExecute")]
        public void FinishJob(Guid jobId, bool errored)
        {
            //is this a runbook worker or something else authorized? TODO
            _jobManager.Finish(jobId, errored);
        }

        [HttpPost]
        [Route("/api/[controller]/{jobID}/Output")]
        [Authorize("JobsExecute")]
        public async Task<ActionResult<Guid>> NewOutputAsync(JobOutputPostParameters jobOutputPostParameters, [FromBody]byte[] byteArrayValue)
        {
            JobOutput output = new JobOutput();
            output.ByteArrayValue = byteArrayValue;
            output.JobUid = jobOutputPostParameters.JobUid;
            output.JsonValue = jobOutputPostParameters.JsonValue;
            output.OutputTime = DateTime.UtcNow;
            output.JobOutputId = Guid.NewGuid();
            output.Type = jobOutputPostParameters.Type;
            await _dbContext.JobOutput.AddAsync(output);
            return output.JobOutputId;
        }
    }
}
