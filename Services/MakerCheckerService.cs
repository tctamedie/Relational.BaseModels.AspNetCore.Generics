using Microsoft.EntityFrameworkCore;
using System;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IMakerCheckerService<TEntity, TMap, T, TDbContext> : IMakerService<TEntity, TMap, T, TDbContext>
        where TEntity : MakerChecker<T>
        where TMap : MakerCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
    }
    public abstract class  MakerCheckerService<TEntity, TMap, T, TDbContext> : MakerService<TEntity, TMap, T, TDbContext>, IMakerCheckerService<TEntity, TMap, T, TDbContext>
        where TEntity : MakerChecker<T>
        where TMap : MakerCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public MakerCheckerService(TDbContext context
            //, IAuditTrailService auditTrailService
            ):base(context)
        {
            
        }
        protected override bool ValidateAuthoriseOnCreator(T id, string user)
        {
            return !Any(s => s.Id.Equals(id) && s.CreatedBy.ToUpper() == user.ToUpper());
        }
        
        protected override void AppendAuthoriser(TEntity row, string createdBy)
        {
            row.AuthorisedBy = createdBy.ToUpper();
            row.DateAuthorised = DateTime.UtcNow.AddHours(2);
            row.AuthStatus = "A";
        }
        
    }
}
