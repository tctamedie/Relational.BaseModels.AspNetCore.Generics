
using System;
using System.Collections.Generic;
using System.Linq;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public interface IAnnotationService<TEntity, TMap, T>
        where T : IEquatable<T>
        where TEntity : Record<T>
        where TMap : RecordDto<T>
    {
        TableModel GetTableModel();
        FormModel GetFormModel();
    }
        public class AnnotationService<TEntity, TMap, T>: IAnnotationService<TEntity, TMap, T>
        where T: IEquatable<T>
        where TEntity: Record<T>
        where TMap: RecordDto<T>
    {
        public List<TAttribute> GetClassAttributes<TAttribute, TClass>(bool specific = false) where TAttribute : Attribute where TClass : class
        {
            string name = typeof(TClass).Name;
            var data = typeof(TClass).Assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any())
                .Select(t => new { t.Name, Attribute = t.GetCustomAttributes(typeof(TAttribute), true) })
                .ToList();
            if (specific)
                data = data.Where(s => s.Name == name).ToList();
            List<TAttribute> records = new List<TAttribute>();
            if (data.Count > 0)
            {
                foreach (var attr in data)
                {
                    foreach (var record in attr.Attribute)
                    {
                        var row = (TAttribute)record;
                        if (row != null)
                            records.Add(row);
                    }

                }
            }

            return records;
        }
        public TableModel GetTableModel()
        {
            var config = GetClassAttributes<EntityConfiguration, TEntity>().FirstOrDefault();
            return new TableModel
            {
                Area = config.Area,
                Controller = config.Controller,
                Header = config.Header
            };
        }
        public FormModel GetFormModel()
        {
            var config = GetClassAttributes<FormConfiguration, TEntity>().FirstOrDefault();
            return new FormModel
            {
                Area = config.Area,
                Controller = config.Controller,
                Header = config.Header
            };
        }
    }
}
