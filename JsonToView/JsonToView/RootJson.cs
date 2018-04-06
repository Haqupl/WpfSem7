using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToView
{
    class RootJson
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Data> Data { get; set; }

    }
}
