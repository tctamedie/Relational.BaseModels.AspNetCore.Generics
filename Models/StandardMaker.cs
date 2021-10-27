using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardMaker<T>: Maker<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
        
    }
    public class StandardMakerDto<T>: MakerDto<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
    }
}
