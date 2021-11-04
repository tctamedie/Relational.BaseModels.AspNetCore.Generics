
using Relational.BaseModels.AspNetCore.Generics.Models;
using Relational.BaseModels.AspNetCore.Generics.Models.Security;

namespace Relational.BaseModels.AspNetCore.Generics.Services.Security
{
    public interface IUserProfileService<TContext> : IStandardModifierCheckerService<UserProfile, UserProfileDto, string, TContext, StandardStatusFilter>
        where TContext: SecurityContext
    {
    }

    public class UserProfileService<TContext> : StandardModifierCheckerService<UserProfile, UserProfileDto, string, TContext, StandardStatusFilter>, IUserProfileService<TContext>
        where TContext : SecurityContext
    {
        public UserProfileService(TContext db) : base(db)
        {
        }
        

    }
}
