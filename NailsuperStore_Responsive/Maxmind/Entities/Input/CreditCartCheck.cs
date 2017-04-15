using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities.Input
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Address_Verification_System#Address_Verification_Service_.28AVS.29_codes
    /// </summary>
    public enum AVSResult
    {
        A = 1, B, C, D, E, F, G, H, I, J, K, L
    }
    public enum CVVResult
    {
        Y, N
    }
    public class CreditCartCheck
    {
        public AVSResult? avs_result { set; get; }
        public CVVResult? cvv_result { set; get; }
    }
}
