using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wpfSyntezator
{
    class ClientResive
    {
        public string sData { get; private set; }
        public EndPoint IpEndPoint { get; private set; }
        public bool IsReaded { get; set; }

        public ClientResive(string data, EndPoint endPoint)
        {
            this.sData = data;
            IpEndPoint = endPoint;
            IsReaded = false;
        }
    }
}
