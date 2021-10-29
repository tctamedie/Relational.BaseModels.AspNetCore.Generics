namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class ColumnModel : EntityModel
    {
        public ColumnModel(int order, int width, string id, bool isKey, string displayName) : base(order, width, id, isKey, displayName)
        {
        }
    }
}
