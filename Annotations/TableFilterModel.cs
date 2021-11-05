namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class TableFilterModel : EntityModel
    {
        public TableFilterModel(int row,int order, int width, string id, string displayName, string defaultValue, ControlType controlType, string onChangeAction, string entityId) : base(order, width, id, false, displayName, entityId)
        {
            Row = row;
            ControlType = controlType;
            OnChangeAction = onChangeAction;
            DefaultValue = defaultValue;
        }
        public ControlType ControlType { get; }
        public string DefaultValue { get; }
        public string OnChangeAction { get; }
        public int Row { get; }
        public ListModel List { get; set; }
    }
}
