using System;
using System.Collections.Generic;

namespace EphIt.Db.Models
{
    public partial class Script
    {
        public Script()
        {
            Job = new HashSet<Job>();
            RoleObjectScopeJob = new HashSet<RoleObjectScopeJob>();
            RoleObjectScopeScript = new HashSet<RoleObjectScopeScript>();
            ScriptVersion = new HashSet<ScriptVersion>();
        }

        public int ScriptId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
        public int? PublishedVersion { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<Job> Job { get; set; }
        public virtual ICollection<RoleObjectScopeJob> RoleObjectScopeJob { get; set; }
        public virtual ICollection<RoleObjectScopeScript> RoleObjectScopeScript { get; set; }
        public virtual ICollection<ScriptVersion> ScriptVersion { get; set; }
    }
}
