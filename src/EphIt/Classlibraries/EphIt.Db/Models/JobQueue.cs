using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EphIt.Db.Models
{
    public class JobQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public int ScriptVersionId { get; set; }
        public short ScriptLanguage { get; set; }
        public DateTime Created { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Job Job { get; set; }
    }
    public class JobQueueConfiguration : IEntityTypeConfiguration<JobQueue>
    {
        public void Configure(EntityTypeBuilder<JobQueue> builder)
        {
            builder.HasOne(p => p.Job)
                .WithOne(d => d.JobQueue)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
