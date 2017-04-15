using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    /// <summary>
    /// The lead, survey, and sitereg types are used for non-purchase transactions.
    /// </summary>
    public enum TxnYype
	{
        creditcard, debitcard, paypal, google, lead, survey, sitereg
	}
    public class TransactionInfomation
    {
        public string txnID { set; get; }
        public decimal? order_amount {set;get;}
        public string order_currency { set; get; }
        public string shopID { get; set; }
        public TxnYype? txn_type { set; get; }
    }
}
