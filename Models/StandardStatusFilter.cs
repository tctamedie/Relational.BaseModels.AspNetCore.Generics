using Relational.BaseModels.AspNetCore.Generics.Annotations;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardStatusFilter : RecordStatusFilter
    {
        [TableFilter(Order: 3)]
        public virtual string Name { get; set; }

    }
}
