using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public class ShippingAddress
    {
        public string shipAddr { set; get; }
        public string shipCity { set; get; }
        public string shipRegion{set;get;}
        public string shipPostal { set; get; }
        public string shipCountry { set; get; }

    }
}
