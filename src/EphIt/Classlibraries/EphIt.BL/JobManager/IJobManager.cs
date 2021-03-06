﻿using EphIt.Db.Enums;
using EphIt.Db.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace EphIt.BL.JobManager
{
    public interface IJobManager
    {
        Guid QueueJob(ScriptVersion script, string parameters, int? UserId = null, int? ScheduleId = null, int? AutomationId = null);
        Job GetQueuedJob(ScriptLanguageEnum languages);
        Hashtable GetJobParameters(Job job);
        void Start(Job job);
        void Start(Guid jobId);
        void Finish(Job job, bool Errored = false);
        void Finish(Guid jobId, bool Errored = false);
        string GetScript(Job job);
        Task LogAsync(Guid jobUid, string message, JobLogLevelEnum level, string Exception = null);
        VMScriptJob GetQueuedScriptedJob(ScriptLanguageEnum languages);
    }
}
