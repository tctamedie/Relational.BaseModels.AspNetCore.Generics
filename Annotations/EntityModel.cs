namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class EntityModel
    {
        public EntityModel(int order, int width, string id, bool isKey, string displayName)
        {
            Id = id;
            Width = width;
            Order = order;
            IsKey = isKey;
            DisplayName = displayName;
            
        }
        public int Order { get; }
        public int Width { get; }
        public string Id { get; }
        public bool IsKey { get; }
        public string DisplayName { get; }
        public string DataType { get; set; }
        
    }
}
