using EphIt.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EphIt.DB
{
    public class EphItContext : DbContext
    {
        public EphItContext() { }
        public EphItContext(DbContextOptions<EphItContext> options) : base(options) { }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<AuthenticationType> AuthenticationType { get; set; }
        public virtual DbSet<Automation> Automation { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
