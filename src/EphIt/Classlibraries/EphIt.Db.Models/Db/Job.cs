﻿using OMyEF.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    [GenerateODataController(Authorize = true, SetName = "Jobs")]
    public class Job
    {
        public Job()
        {
            JobLog = new HashSet<JobLog>();
            JobOutput = new HashSet<JobOutput>();
            //JobObjectIds = new HashSet<VRBACJobToObjectId>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public int ScriptVersionId { get; set; }
        public short JobStatusId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? CreatedByScheduleId { get; set; }
        public int? CreatedByAutomationId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual JobStatus JobStatus { get; set; }
        public virtual ScriptVersion ScriptVersion { get; set; }
        public virtual ICollection<JobLog> JobLog { get; set; }
        public virtual JobQueue JobQueue { get; set; }
        public virtual JobParameters JobParameters { get; set; }
        public virtual ICollection<JobOutput> JobOutput { get; set; }
        //public virtual ICollection<VRBACJobToObjectId> JobObjectIds { get; set; }
    }
    
}
