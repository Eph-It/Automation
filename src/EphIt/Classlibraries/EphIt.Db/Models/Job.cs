using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Job
    {
        public Job()
        {
            JobLog = new HashSet<JobLog>();
            JobOutput = new HashSet<JobOutput>();
            JobObjectIds = new HashSet<VRBACJobToObjectId>();
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
        public virtual ICollection<VRBACJobToObjectId> JobObjectIds { get; set; }
    }
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(many => many.Job)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.JobStatus)
                .WithMany(p => p.Job)
                .HasForeignKey(key => key.JobStatusId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.ScriptVersion)
                .WithMany(p => p.Jobs)
                .HasForeignKey(key => key.ScriptVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(filter => filter.JobObjectIds.Count > 0);
        }
    }
}
