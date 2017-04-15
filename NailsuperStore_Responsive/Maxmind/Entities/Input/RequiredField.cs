using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public class RequiredField
    {
        /// <summary>
        /// ip address customers
        /// </summary>
        public string ipAddress { set; get; }

        internal string licenseKey = Utility.ConfigData.MaxMindLicense();

    }
}
