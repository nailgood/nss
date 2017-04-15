using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public class UserData
    {
        /// <summary>
        /// The domain for the user’s email address. This field should not be hashed.
        /// </summary>
        public string domain { set; get; }

        /// <summary>
        /// The customer’s phone number. Most formats are acceptable. We strip out all non-numeric characters from the input.
        /// </summary>
        public string custPhone { set; get; }
        /// <summary>
        /// An MD5 hash of the user’s email address in ASCII encoding.
        /// </summary>
        public string emailMD5 { set; get; }

        /// <summary>
        /// An MD5 hash of the user’s username in ASCII encoding.
        /// </summary>
        public string usernameMD5 { set; get; }
    }
}
