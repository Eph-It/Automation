﻿using EphIt.Db.Enums;
using EphIt.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace EphIt.BL.JobManager
{
    public class JobManager : IJobManager
    {
        private EphItContext _context;
        private ILogger<JobManager> _logger;
        public JobManager(EphItContext context, ILogger<JobManager> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Guid QueueJob(ScriptVersion script, string parameters, int? UserId = null, int? ScheduleId = null, int? AutomationId = null)
        {
            var newJob = new Job();
            newJob.Created = DateTime.UtcNow;
            newJob.CreatedByAutomationId = AutomationId;
            newJob.CreatedByScheduleId = ScheduleId;
            newJob.CreatedByUserId = UserId;
            newJob.JobStatusId = (short)JobStatusEnum.New;
            newJob.JobUid = Guid.NewGuid();
            newJob.ScriptVersionId = script.ScriptVersionId;
            _context.Add(newJob);
            _context.SaveChanges();

            if(!string.IsNullOrEmpty(parameters))
            {
                JobParameters jobParameters = new JobParameters();
                jobParameters.JobUid = newJob.JobUid;
                jobParameters.Parameters = parameters;
                _context.Add(jobParameters);
                _context.SaveChanges();
            }
            var newJobQueue = new JobQueue();
            newJobQueue.JobUid = newJob.JobUid;
            newJobQueue.Created = newJob.Created;
            newJobQueue.ScriptVersionId = script.ScriptVersionId;
            newJobQueue.ScriptLanguage = (short)script.ScriptLanguageId;
            _context.Add(newJobQueue);
            _context.SaveChanges();
            return newJobQueue.JobUid;
        }
        public Job GetQueuedJob(ScriptLanguageEnum languages)
        {
            _logger.LogInformation("Attemping to find a queued job");
            List<short> LanguagesToQuery = new List<short>();
            foreach (ScriptLanguageEnum scriptLanguage in (ScriptLanguageEnum[])Enum.GetValues(typeof(ScriptLanguageEnum)))
            {
                if (languages.HasFlag(scriptLanguage))
                {
                    _logger.LogDebug($"Will search for language {scriptLanguage}");
                    LanguagesToQuery.Add((short)scriptLanguage);
                }
            }
            var job = _context.JobQueue
                .Where(p =>
                    LanguagesToQuery.Contains(p.ScriptLanguage) 
                )
                .OrderBy(p => p.Created)
                .Include(p => p.Job)
                .FirstOrDefault();
            if(job == null)
            {
                _logger.LogInformation("No pending jobs found.");
                return null;
            }
            _logger.LogDebug($"Found job {job.JobUid} - attempting to process it.");
            _context.Remove(job);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Concurrency conflict in getting job - will throw out result and try again. This means another worker picked up the job.");
                var entry = ex.Entries.Single();
                if(entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Detached;
                }
                else
                {
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
                return GetQueuedJob(languages);
            }
            return job.Job;
        }
        public Hashtable GetJobParameters(Job job)
        {
            var returnHash = new Hashtable();
            string parameters = _context.JobParameters
                                    .Where(p => p.JobUid.Equals(job.JobUid))
                                    .Select(p => p.Parameters)
                                    .FirstOrDefault();
            if (!String.IsNullOrEmpty(parameters))
            {
                try
                {
                    returnHash = JsonConvert.DeserializeObject<Hashtable>(parameters);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error converting parameters from json to Dictionary<string, object>. Parameter data is:{parameters}");
                    throw;
                }
            }
            return returnHash;
        }
        public void Start(Job job)
        {
            job.Start = DateTime.UtcNow;
            job.JobStatusId = (short)JobStatusEnum.InProgress;
            _context.SaveChanges();
        }
        public void Start(Guid jobId)
        {
            var job = _context.Job.Where(j => j.JobUid.Equals(jobId)).FirstOrDefault();
            if(job != null)
            {
                Start(job);
            }
        }
        public void Finish(Job job, bool Errored = false)
        {
            job.JobStatusId = (short)JobStatusEnum.Complete;
            if (Errored)
            {
                job.JobStatusId = (short)JobStatusEnum.Error;
            }
            job.Finish = DateTime.UtcNow;
            _context.SaveChanges();
        }
        public void Finish(Guid jobId, bool Errored = false)
        {
            var job = _context.Job.Where(j => j.JobUid.Equals(jobId)).FirstOrDefault();
            if (job != null)
            {
                Finish(job, Errored);
            }
        }


        public string GetScript(Job job)
        {
            return _context.ScriptVersion
                .Where(p => p.ScriptVersionId == job.ScriptVersionId)
                .Select(p => p.Body)
                .FirstOrDefault();
        }
        public Task LogAsync(Guid jobUid, string message, JobLogLevelEnum level, string Exception = null)
        {
            var newJobLog = new JobLog();
            newJobLog.JobLogId = Guid.NewGuid();
            newJobLog.Exception = Exception;
            newJobLog.JobUid = jobUid;
            newJobLog.LevelId = (short)level;
            newJobLog.LogTime = DateTime.UtcNow;
            newJobLog.Message = message;
            _context.Add(newJobLog);
            return _context.SaveChangesAsync();
        }
        public VMScriptJob GetQueuedScriptedJob(ScriptLanguageEnum languages)
        {
            var job = GetQueuedJob(languages);
            if(job == null)
            {
                return null;
            }
            var body = GetScript(job);
            var parameters = GetJobParameters(job);
            VMScriptJob vmScriptJob = new VMScriptJob();
            vmScriptJob.Job = job;
            vmScriptJob.Script = body;
            vmScriptJob.Parameters = parameters;
            return vmScriptJob;
        }

    }
}
