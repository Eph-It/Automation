using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EphIt.Db.Models
{
    public class JobParameters
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid JobUid { get; set; }
        public string Parameters { get; set; }

        public virtual Job Job { get; set; }
    }
    public class JobParametersConfiguration : IEntityTypeConfiguration<JobParameters>
    {
        public void Configure(EntityTypeBuilder<JobParameters> builder)
        {
            builder.HasOne(p => p.Job)
                .WithOne(d => d.JobParameters)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
