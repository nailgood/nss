using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public enum RequestedType {
        standard, premium
    }
    public class Miscellaneous
    {
        public RequestedType? requested_type { set; get; }
    }
}
