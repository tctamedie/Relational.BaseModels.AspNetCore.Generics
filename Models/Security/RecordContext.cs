using Microsoft.EntityFrameworkCore;
using Relational.BaseModels.AspNetCore.Generics.Models.Security;

namespace Relational.BaseModels.AspNetCore.Generics.Models
{
    public partial class SecurityContext: RecordContext
    {
        public SecurityContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ParentMenu> ParentMenus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<SystemUserProfile> SystemUserProfiles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileMenu> UserProfileMenus { get; set; }
        public DbSet<UserProfileReport> UserProfileReports { get; set; }
        
    }
}
