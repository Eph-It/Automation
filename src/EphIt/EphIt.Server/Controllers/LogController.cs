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
using EphIt.BL.Automation;

namespace EphIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("JobsRead")]
    public class LogController : Controller
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        private IUserAuthorization _userAuth;
        private IJobManager _jobManager;
        private IScriptManager _scriptManager;
        public LogController(EphItContext dbContext, IEphItUser ephItUser, IUserAuthorization userAuth, IJobManager jobManager, IScriptManager scriptManager)
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
        public void New([FromBody] LogPostParameters postParams)
        {
            _jobManager.LogAsync(postParams.jobUid, postParams.message, postParams.level, postParams.Exception);
        }
    }
}