using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class TableFilterAttribute : EntityAttribute
    {
        public TableFilterAttribute(int Order,int Row=1, [CallerMemberName] string ID = "", [CallerMemberName] string Name = "", int Width = 6,  ControlType ControlType = Annotations.ControlType.Text,  string DefaultValue="", string OnChangeAction="Search") : base(ID, Name, Order, false, Width)
        {
            this.ControlType = ControlType;
            this.DefaultValue = DefaultValue;
            this.OnChangeAction = OnChangeAction;
            this.Row = Row;
        }
       
        public ControlType ControlType { get; }
        public string DefaultValue { get;  }
        public string OnChangeAction { get; set; }
        public int Row { get; }

    }
}
