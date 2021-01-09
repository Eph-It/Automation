using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public partial class Script
    {
        public Script()
        {
            RoleObjectScopeScript = new HashSet<RoleObjectScopeScript>();
            ScriptVersion = new HashSet<ScriptVersion>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScriptId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
        public int? PublishedVersion { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<RoleObjectScopeScript> RoleObjectScopeScript { get; set; }
        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
    }
    
}
