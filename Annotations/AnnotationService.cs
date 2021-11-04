
using System;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public interface IAnnotationService<TEntity, TMap, T>: IViewAnnotationService
        where T : IEquatable<T>
        where TEntity : Record<T>
        where TMap : RecordDto<T>
    {
        TableModel GetTableModel(string foreignKey = "");
        FormModel GetFormModel(string foreignKey="");
    }
        public class AnnotationService<TEntity, TMap, T>: ViewAnnotationService, IAnnotationService<TEntity, TMap, T>
        where T: IEquatable<T>
        where TEntity: Record<T>
        where TMap: RecordDto<T>
    {
        
        public TableModel GetTableModel(string foreignKey="")
        {
            return GetTableModel<TEntity, T>(foreignKey);
        }
        public FormModel GetFormModel(string foreignKey="")
        {
            return GetFormModel<TMap, T>(foreignKey);            
        }
        
    }
}
