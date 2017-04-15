using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities.Input
{
    public class BillingAddress
    {
        public string City { set; get; }
        public string Region { set; get; }
        /// <summary>
        /// zip code
        /// </summary>
        public string Postal { set; get; }

        public string Country { set; get; }
    }
}
