using Maxmind.Entities.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maxmind.Entities
{
    public class InputField
    {
        public RequiredField requireField { get; set; }
        public BillingAddress billingAddress { set; get; }
        public ShippingAddress shippingAddress { set; get; }
        public UserData userData { set; get; }
        public BinRelated binRelated { set; get; }
        public TransactionLinking transactionLinking { set; get; }
        public TransactionInfomation transactionInfomation { set; get; }
        public CreditCartCheck creditCartCheck { get;set; }
        public Miscellaneous misc {set;get;}

        public InputField() {
            requireField = new RequiredField();
            billingAddress = new BillingAddress();
            shippingAddress = new ShippingAddress();
            userData = new UserData();
            binRelated = new BinRelated();
            transactionInfomation = new TransactionInfomation();
            transactionLinking = new TransactionLinking();
            creditCartCheck = new CreditCartCheck();
            misc = new Miscellaneous();
        }
    }
}
