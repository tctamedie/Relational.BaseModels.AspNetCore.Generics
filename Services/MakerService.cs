using Microsoft.EntityFrameworkCore;
using System;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    public interface IMakerService<TEntity, TMap, T, TDb> : IRecordService<TEntity, TMap, T, TDb>
        where TEntity : Maker<T>
        where TMap : MakerDto<T>
        where T : IEquatable<T>
        where TDb: DbContext
    {
        
    }
    public abstract class  MakerService<TEntity, TMap, T, TDb> : RecordService<TEntity, TMap, T, TDb>, IRecordService<TEntity, TMap, T, TDb>
        where TEntity : Maker<T>
        where TMap : MakerDto<T>
        where T : IEquatable<T>
        where TDb: DbContext
    {
        
        public MakerService(TDb context
            //, IAuditTrailService auditTrailService
            ):base(context)
        {
            
        }
        protected override bool ValidateDeleteOnCreator(T id, string user)
        {
            return !Any(s => s.Id.Equals(id) && s.CreatedBy.ToUpper() == user.ToUpper());
        }
        protected override void AppendCreator(TEntity row, string createdBy)
        {
            row.CreatedBy = createdBy.ToUpper();
            row.DateCreated = DateTime.UtcNow.AddHours(2);
        }
        
    }
}
