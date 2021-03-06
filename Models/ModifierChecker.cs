using Relational.BaseModels.AspNetCore.Generics.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Relational.BaseModels.AspNetCore.Generics
{
    [Button(ButtonType.Approve)]
    public class ModifierChecker<T>: Modifier<T>
        where T: IEquatable<T>
    {
        public DateTime? DateAuthorised { get; set; }
        [StringLength(60)]
        public string AuthorisedBy { get; set; }
        [StringLength(2)]
        public string AuthStatus { get; set; }
        public int AuthCount { get; set; }
    }
    public class ModifierCheckerDto<T> : ModifierDto<T>
        where T : IEquatable<T>
    {
    }
}
