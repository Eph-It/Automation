using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EphIt.Db.Models
{
    public class Audit
    {
        [Key]
        public long Audit_Id { get; set; }

        public short RbacActionId { get; set; }
        public short RbacObjectId { get; set; }

        public DateTime Created { get; set; }

        public int UserId { get; set; }

        public int ObjectId { get; set; }

        public virtual RbacAction RbacAction { get; set; }
        public virtual RbacObject RbacObject { get; set; }

        public virtual User User { get; set; }
    }
    
}
