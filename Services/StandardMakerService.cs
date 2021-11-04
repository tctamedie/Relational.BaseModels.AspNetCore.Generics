using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IStandardMakerService<TEntity, TMap, T, TDbContext, TFilter> : IMakerService<TEntity, TMap, T, TDbContext, TFilter>
        where TEntity : StandardMaker<T>
        where TMap : StandardMakerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
        where TFilter: StandardFilter
    {

    }
    public class StandardMakerService<TEntity, TMap, T, TDbContext, TFilter> : MakerService<TEntity, TMap, T, TDbContext, TFilter>, IStandardMakerService<TEntity, TMap, T, TDbContext, TFilter>
        where TEntity : StandardMaker<T>
        where TMap : StandardMakerDto<T>
        where T : IEquatable<T>
        where TDbContext: DbContext
        where TFilter: StandardFilter
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
        public override IQueryable<TEntity> SearchByFilterModel(TFilter model, IQueryable<TEntity> data = null)
        {
            string name = string.IsNullOrEmpty(model.Name) ? "" : model.Name.ToLower();
            return _context.Set<TEntity>().Where(s => s.Name.ToLower().Contains(name));
        }

    }
}
