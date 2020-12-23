using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphIt.BL.Authorization;
using EphIt.BL.JobManager;
using EphIt.BL.Script;
using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.AspNet.OData;

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
    }
    public class JobPostParameters
    {
        public int ScriptVersionID { get; set; }
        public int? ScheduleID { get; set; }
        public int? AutomationID { get; set; } //maybe should be guid?
        //maybe add a runbook server param later?
        public string Parameters { get; set; }
    }
}
