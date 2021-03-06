using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class ColumnAttribute: EntityAttribute
    {
        public ColumnAttribute([CallerMemberName] string id="", string displayName="", int order=1, bool isKey=false):base(id,displayName,order, isKey)
        {  
        }
    }
}
