using Microsoft.EntityFrameworkCore;
using Relational.BaseModels.AspNetCore.Generics.Models.Security;

namespace Relational.BaseModels.AspNetCore.Generics.Models
{
    public partial class RecordContext: DbContext
    {
        public RecordContext(DbContextOptions<RecordContext> options):base(options)
        {

        }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuthorLog> AuthorLogs { get; set; }
    }
}
