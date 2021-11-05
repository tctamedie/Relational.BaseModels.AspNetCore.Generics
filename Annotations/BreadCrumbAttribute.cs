using Relational.BaseModels.AspNetCore.Generics.Annotations;
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class BreadCrumbAttribute: Attribute
{
    public BreadCrumbAttribute(string controller, string area, string header, string foreignKey="", string action="Index")
    {
        Controller = controller;
        Action = action;
        Area = area;
        Header = header;
        ForeignKey = string.IsNullOrEmpty(foreignKey)? foreignKey: foreignKey.FirstLetterToLower();
    }
    public string Controller { get;  }
    public string Action { get;  }
    public string Area { get;  }
    public string Header { get;  }
    public string ForeignKey { get; }

}
