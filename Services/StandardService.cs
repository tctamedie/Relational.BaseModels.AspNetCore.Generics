using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IStandardService<TEntity, TMap, T, TDbContext> : IRecordService<TEntity, TMap, T, TDbContext>
        where TEntity : Standard<T>
        where TMap : StandardDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {

    }
    public class StandardService<TEntity, TMap, T, TDbContext> : RecordService<TEntity, TMap, T, TDbContext>, IStandardService<TEntity, TMap, T, TDbContext>
        where TEntity : Standard<T>
        where TMap : StandardDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public StandardService(TDbContext context
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
