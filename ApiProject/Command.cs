using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiProject
{
    public class Command
    {
        public string CommandName { get; set; }
        public List<byte> HexValues { get; set; }
        public string ApiDescription { get; set; }
    }
}
