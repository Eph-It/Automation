using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class JobStatus
    {
        public JobStatus()
        {
            Job = new HashSet<Job>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short JobStatusId { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }

        public virtual ICollection<Job> Job { get; set; }
    }
    public class JobStatusConfiguration : IEntityTypeConfiguration<JobStatus>
    {
        public void Configure(EntityTypeBuilder<JobStatus> builder)
        {
            // The table is populated by AuthenticationEnum in project EphIt.Db.Models
            List<JobStatus> SeededValues = new List<JobStatus>();
            foreach (JobStatusEnum a in (JobStatusEnum[])Enum.GetValues(typeof(JobStatusEnum)))
            {
                SeededValues.Add(new JobStatus()
                {
                    JobStatusId = (short)a,
                    Status = a.ToString()
                });
            }
            builder.HasData(SeededValues.ToArray());
        }
    }
}
