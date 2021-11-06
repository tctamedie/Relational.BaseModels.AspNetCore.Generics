using Relational.BaseModels.AspNetCore.Generics.Annotations;
using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardMaker<T>: Maker<T>
        where T: IEquatable<T>
    {
        [Column(order:2)]
        public virtual string Name { get; set; }
        
    }
    public class StandardMakerDto<T>: MakerDto<T>
        where T: IEquatable<T>
    {
        [Field(1,2)]
        public virtual string Name { get; set; }
    }
}
