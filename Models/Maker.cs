using System;
using System.ComponentModel.DataAnnotations;

namespace Relational.BaseModels.AspNetCore.Generics
{

    public class Maker<T>: Record<T>
        where T: IEquatable<T>
    {
        public DateTime DateCreated { get; set; }
        [StringLength(60)]
        public string CreatedBy { get; set; }
        
    }

    public class MakerDto<T>: RecordDto<T>
        where T: IEquatable<T>
    {
    }
}
