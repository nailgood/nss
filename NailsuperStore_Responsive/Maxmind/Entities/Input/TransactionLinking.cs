using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public class TransactionLinking
    {
        /// <summary>
        /// The User-Agent HTTP header.
        /// </summary>
        public string user_agent { set; get; }

        /// <summary>
        /// The Accept-Language HTTP header.
        /// </summary>
        public string accept_language { set; get; }
    }
}
