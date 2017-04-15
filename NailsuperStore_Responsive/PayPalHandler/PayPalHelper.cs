using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using DataLayer;

namespace PayPalHandler
{
    public class PayPalHelper
    {
        private string m_strLogoUrl = "";
        private string m_strAccountEmail = "";
        private string m_strSuccessUrl = "";
        private string m_strCancelUrl = "";
        private string m_strItemName = "";
        private decimal m_decAmount = 0.00M;
        private string m_strInvoiceNo = "";
        private string m_strNotifyUrl = "";
        private decimal m_taxAmount = 0.0M;
        private decimal m_shippingTotal = 0.0M;
        private decimal m_HandlingTotal = 0.0M;
        private Decimal m_ItemTotal = 0.0M;
        private decimal m_Insurance = 0.0M;
        private string m_currencyCode = "USD";
        private decimal m_OrderTotal = 0.0M;
        private string m_ShipToName = "";
        private string m_ShipToStreet = "";
        private string m_ShipToCity = "";
        private string m_ShipToState = "";
        private string m_ShipToCountry = "";
        private string m_ShipToZipCode = "";

        private string m_strPayPalBaseUrl = "https://www.paypal.com/cgi-bin/webscr?";

        public PayPalHelper()
        {            
            m_strLogoUrl = ConfigurationSettings.AppSettings["LogoUrl"];
            m_strSuccessUrl = ConfigurationSettings.AppSettings["SuccessUrl"];
            m_strCancelUrl = ConfigurationSettings.AppSettings["CancelUrl"];
            m_strNotifyUrl = ConfigurationSettings.AppSettings["NotifyUrl"];
            m_strPayPalBaseUrl = ConfigurationSettings.AppSettings["PayPalUrl"];
            m_strAccountEmail = ConfigurationSettings.AppSettings["PayPalEmail"];

            m_strItemName = String.Empty;
            m_decAmount = 0.00M;
            m_strInvoiceNo = String.Empty;
        }

        #region Public Properties

        // PaypalUrl	
        public string PaypalUrl
        {
            get
            {
                return m_strPayPalBaseUrl;
            }
            set
            {
                m_strPayPalBaseUrl = value;
            }
        }

        // AccountEmail
        public string AccountEmail
        {
            get
            {
                return m_strAccountEmail;
            }
            set
            {
                m_strAccountEmail = value;
            }
        }

        // LogoUrl
        public string LogoUrl
        {
            get
            {
                return m_strLogoUrl;
            }
            set
            {
                m_strLogoUrl = value;
            }
        }

        // SuccessUrl
        public string SuccessUrl
        {
            get
            {
                return m_strSuccessUrl;
            }
            set
            {
                m_strSuccessUrl = value;
            }
        }

        // CancelUrl
        public string CancelUrl
        {
            get
            {
                return m_strCancelUrl;
            }
            set
            {
                m_strCancelUrl = value;
            }
        }

        // NotifyUrl
        public string NotifyUrl
        {
            get
            {
                return m_strNotifyUrl;
            }
            set
            {
                m_strNotifyUrl = value;
            }
        }

        // ItemName
        public string ItemName
        {
            get
            {
                return m_strItemName;
            }
            set
            {
                m_strItemName = value;
            }
        }

        // Amount
        public decimal Amount
        {
            get
            {
                return m_decAmount;
            }
            set
            {
                m_decAmount = value;
            }
        }

        // InvoiceNo
        public string InvoiceNo
        {
            get
            {
                return m_strInvoiceNo;
            }
            set
            {
                m_strInvoiceNo = value;
            }
        }

        public Decimal TaxAmount
        {
            set { m_taxAmount = value; }
            get { return m_taxAmount; }
        }
        
        public Decimal ShippingTotal
        {
            set { m_shippingTotal = value; }
            get { return m_shippingTotal; }
        }

        public Decimal ItemTotal
        {
            set { m_ItemTotal = value; }
            get { return m_ItemTotal; }
        }

        public Decimal HandlingTotal
        {
            set { m_HandlingTotal = value; }
            get { return m_HandlingTotal; }
        }

        public Decimal Insurance
        {
            set { m_Insurance = value; }
            get { return m_Insurance; }
        }

        public string CurrencyCode
        {
            set { m_currencyCode = value; }
            get { return m_currencyCode; }
        }

        public Decimal OrderTotal
        {
            set { m_OrderTotal = value; }
            get { return m_OrderTotal; }
        }

        public String ShipToName
        {
            set { m_ShipToName = value; }
            get { return m_ShipToName; }
        }

        public String ShipToStreet
        {
            set { m_ShipToStreet = value; }
            get { return m_ShipToStreet; }
        }

        public String ShipToCity
        {
            set { m_ShipToCity = value; }
            get { return m_ShipToCity; }
        }

        public String ShipToState
        {
            set { m_ShipToState = value; }
            get { return m_ShipToState; }
        }

        public String ShipToCountry
        {
            set { m_ShipToCountry = value; }
            get { return m_ShipToCountry; }
        }

        public String ShipToZipcode
        {
            set { m_ShipToZipCode = value; }
            get { return m_ShipToZipCode; }
        }

        #endregion

        /// <summary>
        /// Get an URL to submit it to the paypay.
        /// </summary>
        public string GetSubmitUrl()
        {
            StringBuilder url = new StringBuilder();

            url.Append(this.PaypalUrl + "cmd=_xclick&business=" +
              vtHttpUtils.UrlEncode(AccountEmail));

            if (TaxAmount != 0.0M)
                url.AppendFormat("&TAXAMT={0:f2}", TaxAmount);

            if (ShippingTotal != 0.0M)
                url.AppendFormat("&shipping={0:f2}", ShippingTotal);

            if (Amount != 0.00M)
                url.AppendFormat("&amount={0:f2}", Amount);

            if (HandlingTotal != 0.00M)
                url.AppendFormat("&handling={0:f2}", HandlingTotal);

            if (TaxAmount != 0.00M)
                url.AppendFormat("&tax_cart={0:f2}", TaxAmount);

            if (LogoUrl != null && LogoUrl != "")
                url.AppendFormat("&image_url={0}", vtHttpUtils.UrlEncode(LogoUrl));

            if (ItemName != null && ItemName != "")
                url.AppendFormat("&item_name={0}", vtHttpUtils.UrlEncode(ItemName));
                      
            if (InvoiceNo != null && InvoiceNo != "")
                url.AppendFormat("&invoice={0}", vtHttpUtils.UrlEncode(InvoiceNo));

            if (SuccessUrl != null && SuccessUrl != "")
                url.AppendFormat("&return={0}", vtHttpUtils.UrlEncode(SuccessUrl));

            if (CancelUrl != null && CancelUrl != "")
                url.AppendFormat("&cancel_return={0}", vtHttpUtils.UrlEncode(CancelUrl));

            if (NotifyUrl != null && NotifyUrl != "")
                url.AppendFormat("&notify_url={0}", vtHttpUtils.UrlEncode(NotifyUrl));

            return url.ToString();
        }

        public static string PaypalItemList(StoreCartItemCollection list, double handling, double tax_cart, double discount, string PP_email)
        {            
            StringBuilder payPalItems = new StringBuilder();

            string str = "";

            str += "<input type=\"hidden\" name=\"cmd\" value=\"_cart\" />\n";
            str += "<input type=\"hidden\" name=\"upload\" value=\"0\" />\n";
            str += "<input type=\"hidden\" name=\"business\" value=\"" + PP_email + "\" />\n";
            //str += "<input type=\"hidden\" name=\"business\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["PayPalEmail"] + "\" />\n";
            str += "<input type=\"hidden\" name=\"discount_amount_cart\" value=\"" + string.Format("{0:f2}", discount) + "\" />\n";
            //str += "<input type=\"hidden\" name=\"shipping_cart\" value=\"" + string.Format("{0:f2}", handling) + "\" />\n";
            //str += "<input type=\"hidden\" name=\"shipping2_cart\" value=\"" + string.Format("{0:f2}", handling) + "\" />\n";
            str += "<input type=\"hidden\" name=\"handling_cart\" value=\""+ string.Format("{0:f2}", handling) + "\" />\n";
            str += "<input type=\"hidden\" name=\"tax_cart\" value=\""+ string.Format("{0:f2}", tax_cart) + "\" />\n";
            str += "<input type=\"hidden\" name=\"image_url\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["LogoUrl"] + "\"/>\n";
            str += "<input type=\"hidden\" name=\"return\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["SuccessUrl"] + "\" />\n";
            str += "<input type=\"hidden\" name=\"cancel_return\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["CancelUrl"] + "\" />\n";
            str += "<input type=\"hidden\" name=\"no_note\" value=\"1\" />\n";
            payPalItems.Append(str);
            string strGetLowestPrice = "";
            int counter = 0;
            foreach (StoreCartItemRow x in list)
            {
                //vuphuong add: 30/11/2009
                
                //////////////////////////
                counter++;

                string itemNameTemplate = "<input type=\"hidden\" name=\"item_name_$count$\" value=\"$itemName$\" />\n";
                string amountTemplate = "<input type=\"hidden\" name=\"amount_$count$\" value=\"$amount$\" />\n";
                string qtyTemplate = "<input type=\"hidden\" name=\"quantity_$count$\" value=\"$quantity$\" />\n";

                strGetLowestPrice = string.Format("{0:#,##0.00}", GetLowestPrice(x));
                //strGetLowestPrice = strGetLowestPrice.Replace("$count$", counter.ToString());
                string name = x.ItemName;
                if (x.IsRewardPoints)
                    name = "Redeem Rewards Point-" + name;
                itemNameTemplate = itemNameTemplate.Replace("$itemName$", name).Replace("$count$", counter.ToString());
                //amountTemplate = amountTemplate.Replace("$amount$", (x.CustomerPrice > 0 ? x.CustomerPrice : x.Price).ToString("#.00")).Replace("$count$", counter.ToString());
                amountTemplate = amountTemplate.Replace("$amount$", strGetLowestPrice).Replace("$count$", counter.ToString());
                qtyTemplate = qtyTemplate.Replace("$quantity$", x.Quantity.ToString()).Replace("$count$", counter.ToString());

                payPalItems.Append(itemNameTemplate).Append(amountTemplate).Append(qtyTemplate);
            }

            return payPalItems.ToString();
        }

        public static string PaypalItemListNew(StoreCartItemCollection list, double handling, double tax_cart, double discount, string PP_email, string OrderId, string MemberId)
        {
            return PaypalItemListNew(list, handling, tax_cart, discount, PP_email, OrderId, MemberId, false);
        }

        public static string PaypalItemListNew(StoreCartItemCollection list, double handling, double tax_cart, double discount, string PP_email, string OrderId, string MemberId, bool Mobile)
        {
            StringBuilder payPalItems = new StringBuilder();

            try
            {
                payPalItems.Append("<input type=\"hidden\" name=\"cmd\" value=\"_cart\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"upload\" value=\"0\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"business\" value=\"" + PP_email + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"discount_amount_cart\" value=\"" + string.Format("{0:f2}", discount) + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"handling_cart\" value=\"" + string.Format("{0:f2}", handling) + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"tax_cart\" value=\"" + string.Format("{0:f2}", tax_cart) + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"image_url\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["LogoUrl"] + "\"/>");
                payPalItems.Append("<input type=\"hidden\" name=\"return\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["SuccessUrl"] + "&Orderid=" + OrderId.ToString() + "&Memberid=" + MemberId.ToString() + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"cancel_return\" value=\"" + System.Configuration.ConfigurationSettings.AppSettings["CancelUrl"] + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"no_note\" value=\"1\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"custom\" value=\"" + OrderId + "-" + MemberId + (Mobile ? "-M" : "") + "\" />");
                payPalItems.Append("<input type=\"hidden\" name=\"invoice\" value=\"" + OrderId + "\" />");
                payPalItems.Append("Please wait...");
                int counter = 0;

                bool bLimit = list.Count > 90;

                //PayPal limit 91 items
                string arrName = string.Empty;
                string arrSKU = string.Empty;
                double arrAmount = 0;

                foreach (StoreCartItemRow x in list)
                {
                    if (x.IsFreeItem && bLimit)
                        continue;

                    counter++;

                    if (counter > 90)
                    {
                        if (string.IsNullOrEmpty(arrName))
                        {
                            arrName = x.ItemName;
                            if (x.IsRewardPoints)
                                arrName = "Redeem Rewards Point-" + arrName;
                        }

                        arrSKU += "," + x.SKU;

                        //Amount sum all
                        arrAmount += GetLowestPrice(x) * x.Quantity;
                    }
                    else
                    {
                        string name = x.ItemName;
                        if (x.IsRewardPoints)
                            name = "Redeem Rewards Point-" + name;

                        string itemName = string.Format(@"<input type=""hidden"" name=""item_name_{0}"" value=""{1}"" />", counter.ToString(), name);
                        payPalItems.Append(itemName);

                        string itemNumber = string.Format(@"<input type=""hidden"" name=""item_number_{0}"" value=""{1}"" />", counter.ToString(), x.SKU);
                        payPalItems.Append(itemNumber);

                        // string unitPriceString = GetLowestPrice(x).ToString();
                        string amountTemplate = string.Format(@"<input type=""hidden"" name=""amount_{0}"" value=""{1:#,##0.00}"" />", counter.ToString(), GetLowestPrice(x));
                        payPalItems.Append(amountTemplate);

                        string qtyTemplate = string.Format(@"<input type=""hidden"" name=""quantity_{0}"" value=""{1}"" />", counter.ToString(), x.Quantity);
                        payPalItems.Append(qtyTemplate);
                    }
                }

                if (counter > 90)
                {
                    if (counter - 91 > 0)
                    {
                        arrName += string.Format(" and {0} items", counter - 91);
                    }

                    string itemName = string.Format(@"<input type=""hidden"" name=""item_name_91"" value=""{0}"" />", arrName);
                    payPalItems.Append(itemName);

                    string itemNumber = string.Format(@"<input type=""hidden"" name=""item_number_91"" value=""{0}"" />", arrSKU.Substring(1));
                    payPalItems.Append(itemNumber);

                    string amountTemplate = string.Format(@"<input type=""hidden"" name=""amount_91"" value=""{0:#,##0.00}"" />", arrAmount);
                    payPalItems.Append(amountTemplate);

                    string qtyTemplate = string.Format(@"<input type=""hidden"" name=""quantity_91"" value=""1"" />");
                    payPalItems.Append(qtyTemplate);
                }
            }
            catch (Exception ex)
            {
                Components.Email.SendError("ToError500", "PaypalItemListNew", payPalItems.ToString().Replace("\n", "<br>") + "<br><br>-----<br><br>Exception:<br>" + ex.ToString());
            }

            return payPalItems.ToString();
        }
        
        public static double GetLowestPrice(StoreCartItemRow ci)
        {
            if (ci.IsRewardPoints)
                return 0;
            double result = ci.Total;
            if (ci.Quantity < 2)
                return result;
            result = ci.Total / ci.Quantity;
            return result;
        }
    }
}
