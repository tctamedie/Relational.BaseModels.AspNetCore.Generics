using System.Collections.Generic;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class TableModel
    {
        public string KeyField { get; set; }
        public string ForeignKey { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Header { get; set; }
        public List<ColumnModel> Columns { get; set; }
    }
}
