﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EphIt.Db.Models
{
    public class GroupActiveDirectory
    {
        [Key]
        public int GroupActiveDirectoryId { get; set; }
        public int GroupId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public string DistinguisedName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Domain { get; set; }
        [MaxLength(250)]
        public string DisplayName { get; set; }
        [Required]
        [MaxLength(100)]
        public string SID { get; set; }

        public virtual Group Group { get; set; }
    }
    
}
