
using System;
using System.Collections.Generic;
using System.Linq;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public interface IViewAnnotationService
    {
        /// <summary>
        /// Retrievies and transforms Entity Configuration attribute on database entity to come up with a table model
        /// </summary>
        /// <typeparam name="TEntity">The database entity from which the entity configurations are retrieved</typeparam>
        /// <typeparam name="TFilter">The filter which will provide filter criterion</typeparam>
        /// <typeparam name="T">the datatype of the key of the entity</typeparam>
        /// <param name="foreignKey">the optional foreign key column in database entity</param>
        /// <returns>the table model from which to build the presentation html table and filters</returns>
        TableModel GetTableModel<TEntity, TFilter, T>(string foreignKey = "")
            where T : IEquatable<T>
        where TEntity : Record<T>
        where TFilter : RecordFilter;
        /// <summary>
        /// Retrievies and transforms Form Configuration attribute on database entity dto to come up with a user interface
        /// </summary>
        /// <typeparam name="TMap">The database entity dto</typeparam>
        /// <typeparam name="T"> the database entity primary key data type</typeparam>
        /// <param name="foreignKey">the optional foreign key column in database entity</param>
        /// <returns>the form model from which to build the user interface</returns>
        FormModel GetFormModel<TMap, T>(string foreignKey = "")
            where T : IEquatable<T>
        where TMap : RecordDto<T>;
    }
    public class ViewAnnotationService : IViewAnnotationService
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
        List<BreadCrumb> GetBreadCrumbs<TEntity, T>()
            where T: IEquatable<T>
            where TEntity: Record<T>
        {
            var data = GetClassAttributes<BreadCrumbAttribute, TEntity>();
            List<BreadCrumb> breadcrumbs = new();
            foreach(var row in data)
            {
                breadcrumbs.Add(new BreadCrumb( row.Order,row.Controller, row.Area, row.ForeignKey, row.Action, row.Header));
            }

            return breadcrumbs.OrderBy(s=>s.Order).ToList();
        }
        public TableModel GetTableModel<TEntity,TFilter, T>(string foreignKey = "")
        where T : IEquatable<T>
        where TEntity : Record<T>
            where TFilter: RecordFilter

        {
            var config = GetClassAttributes<EntityConfiguration, TEntity>().FirstOrDefault();
            
            var filters = GetFilterModels<TFilter>(config);
            var columns = GetColumnModels<TEntity, T>().OrderBy(s=>s.Order).ToList();
            var keyField = columns.Where(s => s.IsKey).Select(s => s.Id).FirstOrDefault();
            return new TableModel
            {
                ForeignKey = foreignKey,
                KeyField = keyField,
                Area = config.Area,
                Controller = config.Controller,
                Header = config.Header,
                Columns = columns,
                Filters = filters,
                BreadCrumbs = GetBreadCrumbs<TEntity, T>(),
                NavigationLinks = GetLinkModels<TEntity, T>(),
                Buttons = GetButtonModels<TEntity, T>()
            };
        }
        public List<Link> GetLinkModels<TEntity, T>()
            where TEntity : Record<T>
            where T : IEquatable<T>
        {
            List<Link> models = new List<Link>();
            var properties = typeof(TEntity).GetProperties().Where(t => t.GetCustomAttributes(typeof(LinkAttribute), true).Any()).Select(s => new { Name = s.Name, DataType = s.PropertyType, Attribute = ((LinkAttribute)s.GetCustomAttributes(typeof(LinkAttribute), true).First()) }).ToList();
            foreach (var record in properties)
            {
                var atrribute = record.Attribute;
                var model = new Link(atrribute.Controller, atrribute.LinkButtonTitle, atrribute.LinkButtonTitle, atrribute.LinkButtonIcon, atrribute.LinkButtonClass, atrribute.Action, atrribute.ID,atrribute.Area)
                ;
                models.Add(model);
            }
            return models;
        }
        public List<ButtonModel> GetButtonModels<TEntity, T>()
            where TEntity : Record<T>
            where T : IEquatable<T>
        {
            List<ButtonModel> models = new List<ButtonModel>();
            var data = GetClassAttributes<ButtonAttribute, TEntity>();
            foreach (var atrribute in data)
            {
                var model = new ButtonModel(atrribute.Icon, atrribute.Action, atrribute.Title, atrribute.Text, atrribute.Class, atrribute.ButtonType);
                models.Add(model);
            }
            return models.OrderBy(s=>s.ButtonType).ToList();
        }

        public List<ColumnModel> GetColumnModels<TEntity, T>()
            where TEntity : Record<T>
            where T : IEquatable<T>
        {
            List<ColumnModel> models = new List<ColumnModel>();
            var properties = typeof(TEntity).GetProperties().Where(t => t.GetCustomAttributes(typeof(ColumnAttribute), true).Any()).Select(s => new { Name = s.Name, DataType = s.PropertyType, Attribute = ((ColumnAttribute)s.GetCustomAttributes(typeof(ColumnAttribute), true).First()) }).ToList();
            foreach (var record in properties)
            {
                string dataType = "string";
                var atrribute = record.Attribute;
                if (record.DataType == typeof(decimal) || record.DataType == typeof(decimal?))
                {
                    dataType = "number";
                }
                else if (record.DataType == typeof(int) || record.DataType == typeof(int?))
                    dataType = "int";
                else if (record.DataType == typeof(DateTime) || record.DataType == typeof(DateTime?))
                    dataType = "date";
                var model = new ColumnModel(atrribute.Order, atrribute.Width, atrribute.Id, atrribute.IsKey, atrribute.DisplayName, atrribute.EntityId)
                {
                    DataType = dataType
                };
                models.Add(model);
            }
            return models;
        }
        public List<TableFilterModel> GetFilterModels<TFilter>(EntityConfiguration configuration)            
        where TFilter : RecordFilter
        {
            List<TableFilterModel> models = new List<TableFilterModel>();
            var properties = typeof(TFilter).GetProperties();
            foreach (var property in properties)
            {
                var attr = (TableFilterAttribute)property.GetCustomAttributes(typeof(TableFilterAttribute), true).FirstOrDefault();
                var lsAttr = (ListAttribute)property.GetCustomAttributes(typeof(ListAttribute), true).FirstOrDefault();
                string dataType = "string";
                var record = property.PropertyType;
                if (record == typeof(decimal) || record == typeof(decimal?))
                {
                    dataType = "number";
                }
                else if (record == typeof(int) || record == typeof(int?))
                    dataType = "int";
                else if (record == typeof(DateTime) || record == typeof(DateTime?))
                    dataType = "date";
                var model = new TableFilterModel(attr.Row, attr.Order, attr.Width, attr.Id, attr.DisplayName, attr.DefaultValue, attr.ControlType, attr.OnChangeAction, attr.EntityId);
                model.DataType = dataType;
               
                if (lsAttr != null)
                {
                    string controller = lsAttr.Controller;
                    if (string.IsNullOrEmpty(lsAttr.Controller))
                        controller = configuration.Controller;
                    string area = lsAttr.Area;
                    if (string.IsNullOrEmpty(lsAttr.Area))
                        area = configuration.Area;
                    model.List = new ListModel(
                        controller,
                        lsAttr.Action,
                        lsAttr.ValueField,
                        lsAttr.TextField,
                        lsAttr.ID,
                        area,
                        lsAttr.MultipleSelect,
                        lsAttr.OnSelectedChange,
                        lsAttr.OnField,
                        lsAttr.FilterColumn,
                        lsAttr.FilterValue,
                        lsAttr.SortField);
                }
                models.Add(model);
            }
            return models;
        }
        public List<FieldModel> GetFieldModels<TMap, T>(FormConfiguration configuration)
            where T : IEquatable<T>
        where TMap : RecordDto<T>
        {
            List<FieldModel> models = new List<FieldModel>();
            var properties = typeof(TMap).GetProperties();
            foreach (var property in properties)
            {
                var attr = (FieldAttribute)property.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault();
                var lsAttr = (ListAttribute)property.GetCustomAttributes(typeof(ListAttribute), true).FirstOrDefault();
                string dataType = "string";
                var record = property.PropertyType;
                if (record == typeof(decimal) || record == typeof(decimal?))
                {
                    dataType = "number";
                }
                else if (record == typeof(int) || record == typeof(int?))
                    dataType = "int";
                else if (record == typeof(DateTime) || record == typeof(DateTime?))
                    dataType = "date";
                var model = new FieldModel(attr.Row, attr.Order, attr.Width, attr.Id, attr.IsKey, attr.DisplayName, attr.Autogenerated, attr.Type, attr.TabId, attr.EntityId);
                model.DataType = dataType;
                if (lsAttr!=null)
                {
                    string controller = lsAttr.Controller;
                    if (string.IsNullOrEmpty(lsAttr.Controller))
                        controller = configuration.Controller;
                    string area = lsAttr.Area;
                    if (string.IsNullOrEmpty(lsAttr.Area))
                        area = configuration.Area;
                    model.List = new ListModel(
                        controller,
                        lsAttr.Action,
                        lsAttr.ValueField,
                        lsAttr.TextField,
                        lsAttr.ID,
                        area,
                        lsAttr.MultipleSelect,
                        lsAttr.OnSelectedChange,
                        lsAttr.OnField,
                        lsAttr.FilterColumn,
                        lsAttr.FilterValue,
                        lsAttr.SortField);
                    
                }
                models.Add(model);
            }
            return models;
        }
        public FormModel GetFormModel<TMap, T>(string foreignKey = "")
        where T : IEquatable<T>
        where TMap : RecordDto<T>
        {
            var config = GetClassAttributes<FormConfiguration, TMap>().FirstOrDefault();
            var tabs = GetFormTabs<TMap, T>(config);
            return new FormModel
            {
                ForegnKey = foreignKey,
                KeyField = tabs.KeyField,
                Area = config.Area,
                Controller = config.Controller,
                Header = config.Header,
                Tabs = tabs.Tabs
            };
        }
        public virtual (List<TabModel> Tabs, string KeyField) GetFormTabs<TMap, T>(FormConfiguration configuration)
            where T : IEquatable<T>
            where TMap : RecordDto<T>
        {
            var data = GetClassAttributes<TabAttribute, TMap>(true);
            List<TabModel> models = new List<TabModel>();
            foreach (var row in data)
            {
                models.Add(new TabModel(row.Order, row.ID, row.Name, row.IsActiveTab, row.Field, row.IsHidden));
            }
            if (models.Count == 0)
            {
                models.Add(new TabModel(1, "", "NotApplicable", true, ""));
            }
            var fieldModels = GetFieldModels<TMap, T>(configuration);
            string keyField = fieldModels.Where(s => s.IsKey).Select(s => s.Id).FirstOrDefault();
            models.ForEach(tab =>
            {
                var fields = fieldModels.Where(s => s.TabId == tab.ID).ToList();
                var rows = fields.Select(s => s.Row).Distinct().ToList();
                foreach (var row in rows)
                {
                    var rowFields = fields.Where(s => s.Row == row).ToList();
                    tab.Rows.Add(new TabRow
                    {
                        Order = row,
                        Fields = rowFields
                    });
                }

            });
            return (models, keyField);
        }
    }
}
