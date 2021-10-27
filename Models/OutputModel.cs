using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relational.BaseModels.AspNetCore.Generics
{
    public class OutputModel
    {
        public OutputModel(bool errorOccured=false)
        {
            Error = errorOccured;
            Message = "";
            Data = null;
        }
        public bool Error { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
