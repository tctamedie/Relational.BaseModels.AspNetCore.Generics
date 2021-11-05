using Microsoft.EntityFrameworkCore;
using Relational.BaseModels.AspNetCore.Generics.Models.Security;

namespace Relational.BaseModels.AspNetCore.Generics.Models
{
    public partial class RecordContext: DbContext
    {
        public RecordContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuthorLog> AuthorLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasKey(s => new { s.RecordKey, s.Username, s.AuditDate, s.Table, s.Action });
            modelBuilder.Entity<AuthorLog>().HasKey(s => new { s.RecordKey, s.Username, s.AuthorDate, s.Table, s.AuthorCount });
            base.OnModelCreating(modelBuilder);
        }
    }
}
