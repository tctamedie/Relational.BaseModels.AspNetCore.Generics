using System;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class StandardModifier<T>: Modifier<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
        
    }
    public class StandardModifierDto<T>: ModifierDto<T>
        where T: IEquatable<T>
    {
        public virtual string Name { get; set; }
    }
}
