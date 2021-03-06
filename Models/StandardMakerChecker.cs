using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardMakerChecker<T>: MakerChecker<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
        
    }
    public class StandardMakerCheckerDto<T>: MakerCheckerDto<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
    }
}
