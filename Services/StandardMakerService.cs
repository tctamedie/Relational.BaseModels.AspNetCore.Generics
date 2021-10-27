using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IStandardMakerService<TEntity, TMap, T, TDbContext> : IMakerService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardMaker<T>
        where TMap : StandardMakerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {

    }
    public class StandardMakerService<TEntity, TMap, T, TDbContext> : MakerService<TEntity, TMap, T, TDbContext>, IStandardMakerService<TEntity, TMap, T, TDbContext>
        where TEntity : StandardMaker<T>
        where TMap : StandardMakerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
    {
        
        public StandardMakerService(TDbContext context
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
