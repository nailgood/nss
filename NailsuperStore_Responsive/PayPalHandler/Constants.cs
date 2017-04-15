using System;
using PayPal.Payments.DataObjects;
using System.Configuration;

namespace PayPalHandler
{
	public class Constants
	{
		private Constants()
		{
		}
				
		// Payflow Pro Host Name. This is the host name for the PayPal Payment Gateway.
		// For testing: 	pilot-payflowpro.paypal.com
		// For production: 	payflowpro.paypal.com
		// DO NOT use test-payflow.verisign.com or payflow.verisign.com.
		
		// If needed, set proxy like given example.
		// internal static PayflowConnectionData Connection = new PayflowConnectionData("pilot-payflowpro.paypal.com",443,<ProxyAddress>,<ProxyPort>,<ProxyLogon>,<ProxyPassword>);
        public static PayflowConnectionData GetConnection(bool livePaypal)
        {
            if(livePaypal)
                return new PayflowConnectionData("payflowpro.paypal.com", 443, null, 0, null, null);
            else
                return new PayflowConnectionData("pilot-payflowpro.paypal.com", 443, null, 0, null, null);
        }

        public static string ShowMsg(int ResponseCode)
        {
            string str = string.Empty;
            switch (ResponseCode)
            {
                case 12:
                    str = "Transaction is declined. Please check the credit card number, expiration date and transaction information to make sure they were entered correctly.";
                    break;

                case 24:
                    str = "Invalid expiration date. Please try again";
                    break;

                case 23:
                    str = "Invalid credit card number. Please try again";
                    break;

                case 112:
                    str = "Billing Address and ZIP code do not match. Please try again";
                    break;

                case 114:
                    str = "Invalid CID. Please try again";
                    break;

                case 115:
                    str = "Invalid CID. Please try again";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

		public static PayflowConnectionData Connection = new PayflowConnectionData("payflowpro.paypal.com",443,null,0,null,null);       
		public static String LocalHostName  = ConfigurationSettings.AppSettings["hostName"];
	}
}
