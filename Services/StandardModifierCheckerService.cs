using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IStandardModifierCheckerService<TEntity, TMap, T, TDbContext> : IModifierCheckerService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardModifierChecker<T>
        where TMap : StandardModifierCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {

    }
    public class StandardModifierCheckerService<TEntity, TMap, T, TDbContext> : ModifierCheckerService<TEntity, TMap, T, TDbContext>, IStandardModifierCheckerService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardModifierChecker<T>
        where TMap : StandardModifierCheckerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public StandardModifierCheckerService(TDbContext context
            //, IAuditTrailService auditTrailService
            ) : base(context)
        {
            
            //_auditTrailService = auditTrailService;
        }

        protected override OutputModel Validate(TMap row, [CallerMemberName] string caller = "")
        {
            var validation = base.Validate(row, caller);
            if (validation.Error)
                return validation;

            if (Any(s => s.Name.ToUpper() == row.Name.ToUpper() && !s.Id.Equals(row.Id)))
            {
                return new OutputModel(true)
                {
                    
                    Message = $" Name {row.Name}for {_modelHeader} already exist"
                };
            }
            return new OutputModel();

        }

        
    }
}
