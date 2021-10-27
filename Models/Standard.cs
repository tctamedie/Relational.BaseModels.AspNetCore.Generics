using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class Standard<T>: Record<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
        
    }
    public class StandardDto<T>: RecordDto<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
    }
}
