using System;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class EntityAttribute : Attribute
    {
        public EntityAttribute([CallerMemberName] string id = "", string displayName = "", int order = 1, bool isKey = false, int width=6)
        {
            Id = id.FirstLetterToLower();
            Order = order;
            IsKey = isKey;
            if (string.IsNullOrEmpty(displayName))
            {
                DisplayName = id.CamelSplit();
            }
            else
                DisplayName = displayName;
            Width = width;
            
        }
        public int Order { get; }
        public int Width { get; }
        public string Id { get; }
        public bool IsKey { get; }
        public string DisplayName { get; }
        public string DataType { get; set; }
        


    }
}
