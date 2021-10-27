using System;
using System.ComponentModel.DataAnnotations;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class MakerChecker<T>: Maker<T>
        where T: IEquatable<T>
    {
        public DateTime? DateAuthorised { get; set; }
        [StringLength(60)]
        public string AuthorisedBy { get; set; }
        [StringLength(2)]
        public string AuthStatus { get; set; }
        public int AuthCount { get; set; }
    }
    public class MakerCheckerDto<T> : MakerDto<T>
        where T : IEquatable<T>
    {
    }
}
