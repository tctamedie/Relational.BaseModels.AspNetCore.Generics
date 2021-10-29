using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    using Annotations;
    public class Record<T>
        where T: IEquatable<T>
    {
        [Column(order:1)]
        public virtual T Id { get; set; }
    }
    public class RecordDto<T>
        where T : IEquatable<T>
    {
        [Field(1,1, isKey:true)]
        public virtual T Id { get; set; }
    }
}
