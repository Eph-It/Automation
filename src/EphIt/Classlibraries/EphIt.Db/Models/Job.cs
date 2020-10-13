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
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public int ScriptId { get; set; }
        public short JobStatusId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual JobStatus JobStatus { get; set; }
        public virtual Script Script { get; set; }
        public virtual ICollection<JobLog> JobLog { get; set; }
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
            builder.HasOne(d => d.Script)
                .WithMany(p => p.Job)
                .HasForeignKey(key => key.ScriptId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
