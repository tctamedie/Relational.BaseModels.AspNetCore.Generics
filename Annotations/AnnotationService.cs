
using System;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public interface IAnnotationService<TEntity, TMap, T, TFilter> : IViewAnnotationService
        where T : IEquatable<T>
        where TEntity : Record<T>
        where TMap : RecordDto<T>
        where TFilter : RecordFilter
    {
        TableModel GetTableModel(string foreignKey = "");
        FormModel GetFormModel(string foreignKey = "");
    }
    public class AnnotationService<TEntity, TMap, T, TFilter> : ViewAnnotationService, IAnnotationService<TEntity, TMap, T, TFilter>
    where T : IEquatable<T>
    where TEntity : Record<T>
    where TMap : RecordDto<T>
        where TFilter : RecordFilter
    {

        public TableModel GetTableModel(string foreignKey = "")
        {
            return GetTableModel<TEntity,TFilter, T>(foreignKey);
        }
        public FormModel GetFormModel(string foreignKey = "")
        {
            return GetFormModel<TMap, T>(foreignKey);
        }

    }
}
