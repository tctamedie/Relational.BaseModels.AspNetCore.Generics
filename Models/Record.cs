using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class Record<T>
        where T: IEquatable<T>
    {
        public virtual T Id { get; set; }
    }
    public class RecordDto<T>
        where T : IEquatable<T>
    {
        public virtual T Id { get; set; }
    }
}
