using Relational.BaseModels.AspNetCore.Generics.Annotations;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardFilter : RecordFilter
    {
        [TableFilter(Order: 1)]
        public virtual string Name { get; set; }

    }
}
