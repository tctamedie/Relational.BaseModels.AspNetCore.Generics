using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IStandardModifierService<TEntity, TMap, T, TDbContext> : IModifierService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardModifier<T>
        where TMap : StandardModifierDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {

    }
    public class StandardModifierService<TEntity, TMap, T, TDbContext> : ModifierService<TEntity, TMap, T, TDbContext>, IStandardModifierService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardModifier<T>
        where TMap : StandardModifierDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public StandardModifierService(TDbContext context
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
