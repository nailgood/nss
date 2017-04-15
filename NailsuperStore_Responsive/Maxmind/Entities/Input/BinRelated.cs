using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities.Input
{
    public class BinRelated
    {
        /// <summary>
        /// The credit card BIN number. This is the first 6 digits of the credit card number. It identifies the issuing bank.
        /// </summary>
        public string bin { set; get; }

        public string binName { set; get; }

        public string binPhone { set; get; }
    }
}
