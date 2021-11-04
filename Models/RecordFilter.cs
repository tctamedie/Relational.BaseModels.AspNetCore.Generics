namespace Relational.BaseModels.AspNetCore.Generics
{
    using Annotations;
    public abstract class RecordFilter
    {
        [TableFilter(Order: 1)]
        public virtual string Search { get; set; }
    }

}
