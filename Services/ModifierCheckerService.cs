using Microsoft.EntityFrameworkCore;
using System;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IModifierCheckerService<TEntity, TMap, T, TDbContext> : IModifierService<TEntity, TMap, T, TDbContext>
        where TEntity : ModifierChecker<T>
        where TMap : ModifierCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
    }
    public abstract class  ModifierCheckerService<TEntity, TMap, T, TDbContext> : ModifierService<TEntity, TMap, T, TDbContext>, 
        IModifierCheckerService<TEntity, TMap, T, TDbContext>
        where TEntity : ModifierChecker<T>
        where TMap : ModifierCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public ModifierCheckerService(TDbContext context
            //, IAuditTrailService auditTrailService
            ):base(context)
        {
            
        }
        protected override bool ValidateAuthoriseOnModifier(T id, string user)
        {
            return !Any(s => s.Id.Equals(id) && s.ModifiedBy.ToUpper() == user.ToUpper());
        }
        
        protected override void AppendAuthoriser(TEntity row, string createdBy)
        {
            row.AuthorisedBy = createdBy.ToUpper();
            row.DateAuthorised = DateTime.UtcNow.AddHours(2);
            row.AuthStatus = "A";
        }
        
    }
}
