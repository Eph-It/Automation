using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class Variable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariableId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }
    public class VariableConfiguration : IEntityTypeConfiguration<Variable>
    {
        public void Configure(EntityTypeBuilder<Variable> builder)
        {
            builder.HasKey(d => d.VariableId);
        }
    }

}
