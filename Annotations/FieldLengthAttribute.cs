using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class FieldLengthAttribute : StringLengthAttribute
    {
        public FieldLengthAttribute(int maximumLength, [CallerMemberName] string id = "") : base(maximumLength)
        {

            ID = id.FirstLetterToLower();
        }
        public string ID { get; }
    }
}