using System;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityConfiguration: Attribute
    {
        public EntityConfiguration(string controller, string area, string header="")
        {
            Controller = controller;
            Area = area;
            if (string.IsNullOrEmpty(header))
            {
                Header = controller;
            }
            else
                Header = header;
            
        }
        public string Controller { get; }
        public string Area { get; }        
        public string Header { get; }
    }
}
