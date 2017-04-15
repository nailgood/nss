using Maxmind.Entities.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using Maxmind.Entities.Input;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Maxmind.Entities
{
    public class OutputField
    {
        public static string UriService = Utility.ConfigData.MaxMindUriService();
        public static string getResponseString(string url) {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    var streamReader = new StreamReader(stream);
                    var resultStr = streamReader.ReadToEnd();
                    return resultStr;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #region Fields
        public string maxmindID { set; get; }

        public RiskScore riskScore { set; get; }
        public GeoIPLocationChecks geoIpLocationChecks { set; get; }
        public ProxyDetection proxyDetection { set; get; }
        public EmailsAndLoginChecks emailLoginChecks { set; get; }
        public BankChecks bandChecks { set; get; }
        public AddressAndPhoneNumberChecks addressPhoneChecks { set; get; }
        public AccountInformationFields accountInfoFields { set; get; }
        public ErrorReporting errorReporting { set; get; }
        private InputField _input = null;
        public string resultString = string.Empty;
        public string queryString = string.Empty;

        private string _cardNumber = string.Empty; 
        #endregion

        /// <summary>
        /// in background
        /// </summary>
        /// <param name="orderId"></param>
        public OutputField(int orderId, string cardNumber)
        {
            try
            {
                CacheItemRemovedCallback(cardNumber, orderId, new CacheItemRemovedReason());
            }
            catch (Exception)
            {
                Components.Email.SendError("ToError500", "Maxmind process data. OutputField(int orderId, string cardNumber)", "Init");
            }
            //Components.Email.SendError("ToError500", "Maxmind process data. OutputField(int orderId, string cardNumber)", "Init");
            //CacheItemRemovedCallback(cardNumber, orderId, new CacheItemRemovedReason());
            //add background job
            //HttpContext.Current.Cache.Add(cardNumber, orderId, null, DateTime.MaxValue, TimeSpan.FromSeconds(20)
            //    , System.Web.Caching.CacheItemPriority.Normal, new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }
        public OutputField(InputField input)
        {
            _input = input;
            process();
        }
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            input = input.ToLower();
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void CacheItemRemovedCallback(string cardnumber, object value, CacheItemRemovedReason reason)
        {
            int orderId =  0;
            DataTable dt = new DataTable();
            Microsoft.Practices.EnterpriseLibrary.Data.Database defaultDB = null;
            try
            {
                orderId = (int)value;
                if (orderId <= 0) {
                    Components.Email.SendError("ToError500", "Maxmind process data. OrderId <= 0 : OrderId: " + orderId.ToString(), "OrderId <= 0");
                    return;
                };

                #region GetData
                defaultDB = DatabaseFactory.CreateDatabase();
                string storedProcName = "sp_StoreOrder_getInfoMaxmind";
                using (DbCommand sprocCmd = defaultDB.GetStoredProcCommand(storedProcName))
                {
                    defaultDB.AddInParameter(sprocCmd, "OrderId", DbType.Int32, orderId);
                    using (IDataReader sprocReader = defaultDB.ExecuteReader(sprocCmd))
                    {
                        dt.Load(sprocReader);

                        if (dt.Rows.Count == 0) {
                            Components.Email.SendError("ToError500", "Maxmind process data. OrderId not found: OrderId: " + orderId.ToString(), "dt.Rows.Count == 0");
                            return;
                        };

                        #region Input data.
                        _input = new InputField();

                        DataRow dr = dt.Rows[0];

                        string valueStr = dr["i"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.requireField.ipAddress = valueStr;

                        //billing
                        valueStr = dr["city"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.billingAddress.City = valueStr;

                        valueStr = dr["region"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.billingAddress.Region = valueStr;

                        valueStr = dr["postal"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.billingAddress.Postal = valueStr;

                        valueStr = dr["country"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.billingAddress.Country = valueStr;
                        
                        //shipping
                        valueStr = dr["shipAddr"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.shippingAddress.shipAddr = valueStr;

                        valueStr = dr["shipCity"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.shippingAddress.shipCity = valueStr;

                        valueStr = dr["shipRegion"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.shippingAddress.shipRegion = valueStr;

                        valueStr = dr["shipPostal"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.shippingAddress.shipPostal = valueStr;

                        valueStr = dr["shipCountry"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.shippingAddress.shipCountry = valueStr;

                        //user data
                        valueStr = dr["domain"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.userData.domain = valueStr;

                        valueStr = dr["custPhone"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.userData.custPhone = valueStr;

                        valueStr = dr["emailMD5"].ToString();
                        if (!string.IsNullOrEmpty(valueStr)) {
                            _input.userData.emailMD5 = CalculateMD5Hash(valueStr);
                        }

                        valueStr = dr["usernameMD5"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        {

                            _input.userData.usernameMD5 = CalculateMD5Hash(valueStr);
                        }

                        //bin related
                        //valueStr = dr["bin"].ToString();
                        if (!string.IsNullOrEmpty(cardnumber) && cardnumber.Length > 6)
                            _input.binRelated.bin = cardnumber.Substring(0, 6);

                        valueStr = dr["binName"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.binRelated.binName = valueStr;

                        valueStr = dr["binPhone"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.binRelated.binPhone = valueStr;

                        //transaction info
                        valueStr = dr["txnID"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.transactionInfomation.txnID = valueStr;

                        valueStr = dr["order_amount"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            decimal amount = 0;
                            if(decimal.TryParse(valueStr, out amount))
                                _input.transactionInfomation.order_amount = amount;
                        }

                        valueStr = dr["order_currency"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.transactionInfomation.order_currency = valueStr;

                        valueStr = dr["shopID"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                            _input.transactionInfomation.shopID = valueStr;

                        valueStr = dr["txn_type"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        { 
                            TxnYype txnType = TxnYype.creditcard;
                            if(Enum.TryParse<TxnYype>(valueStr, out txnType))
                                _input.transactionInfomation.txn_type = txnType;
                        }

                        //creditcard check
                        valueStr = dr["avs_result"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            AVSResult avsResult = AVSResult.A;
                            if (Enum.TryParse<AVSResult>(valueStr, out avsResult))
                                _input.creditCartCheck.avs_result = avsResult;
                        }

                        valueStr = dr["cvv_result"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            CVVResult cvvResult = CVVResult.N;
                            if (Enum.TryParse<CVVResult>(valueStr, out cvvResult))
                                _input.creditCartCheck.cvv_result = cvvResult;
                        }

                        //misc
                        valueStr = dr["requested_type"].ToString();
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            RequestedType qType = RequestedType.standard;
                            if (Enum.TryParse<RequestedType>(valueStr, out qType))
                                _input.misc.requested_type = qType;
                        }
                        #endregion
                    }
                }
                #endregion

                process();

                #region update data
                if (this.riskScore != null)
                {
                    string notes = Math.Round(this.riskScore.riskScore + 0.00000001M, 0).ToString() + "% fraud (" + this.maxmindID + ")";
                    using (DbCommand sqlCmd = defaultDB.GetSqlStringCommand(@"
                    update StoreOrder set Comments = 
                        case 
                            when isnull(cast(Comments as nvarchar(max)), '') = '' then @rickScore 
                            else 
                                case when charindex('|', cast(Comments as nvarchar(max)), 12) > 0 
                                    then replace(cast(Comments as nvarchar(max))
                                                    , substring(
                                                        cast(Comments as nvarchar(max))
                                                        , charindex(
                                                                '|'
                                                                , cast(Comments as nvarchar(max))
                                                                , len(cast(Comments as nvarchar(max))) - 22
                                                                    )
                                                        , 22
                                                             )
                                                    , '|' +  @rickScore
                                                )
                                    else cast(Comments as nvarchar(max)) + '|' + @rickScore end 
                            end
                    where OrderId = @OrderId"))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("rickScore", notes));
                        sqlCmd.Parameters.Add(new SqlParameter("OrderId", orderId));
                        defaultDB.ExecuteNonQuery(sqlCmd);

                        string orderNo = string.Empty;
                        if (dt.Rows.Count > 0)
                            orderNo = dt.Rows[0]["txnID"].ToString();
                        // Components.Email.SendError("ToError500", "Maxmind process data. CacheItemRemovedCallback: OrderNo: " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                        Components.Email.SendReport("ToReportPayment", "[MaxMind] OrderNo " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                    }
                }
                else if (this.errorReporting == ErrorReporting.MAX_REQUESTS_REACHED)
                {
                    _input.misc.requested_type = RequestedType.premium;
                    process();

                    if (this.riskScore != null)
                    {
                        string notes = Math.Round(this.riskScore.riskScore + 0.00000001M, 0).ToString() + "% fraud (" + this.maxmindID + ")";
                        using (DbCommand sqlCmd = defaultDB.GetSqlStringCommand(@"
                    update StoreOrder set Comments = 
                        case 
                            when isnull(cast(Comments as nvarchar(max)), '') = '' then @rickScore 
                            else 
                                case when charindex('|', cast(Comments as nvarchar(max)), 12) > 0 
                                    then replace(cast(Comments as nvarchar(max))
                                                    , substring(
                                                        cast(Comments as nvarchar(max))
                                                        , charindex(
                                                                '|'
                                                                , cast(Comments as nvarchar(max))
                                                                , len(cast(Comments as nvarchar(max))) - 22
                                                                    )
                                                        , 22
                                                             )
                                                    , '|' +  @rickScore
                                                )
                                    else cast(Comments as nvarchar(max)) + '|' + @rickScore end 
                            end
                    where OrderId = @OrderId"))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter("rickScore", notes));
                            sqlCmd.Parameters.Add(new SqlParameter("OrderId", orderId));
                            defaultDB.ExecuteNonQuery(sqlCmd);

                            string orderNo = string.Empty;
                            if (dt.Rows.Count > 0)
                                orderNo = dt.Rows[0]["txnID"].ToString();
                            // Components.Email.SendError("ToError500", "Maxmind process data. CacheItemRemovedCallback: OrderNo: " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                            Components.Email.SendReport("ToReportPayment", "[MaxMind] OrderNo " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                        }
                    }
                    else
                    {
                        string notes = this.errorReporting.ToString();
                        using (DbCommand sqlCmd = defaultDB.GetSqlStringCommand(@"
                    update StoreOrder set Comments = 
                        case 
                            when isnull(cast(Comments as nvarchar(max)), '') = '' then @rickScore 
                            else 
                                case when charindex('|', cast(Comments as nvarchar(max)), 12) > 0 
                                    then replace(cast(Comments as nvarchar(max))
                                                    , substring(
                                                        cast(Comments as nvarchar(max))
                                                        , charindex(
                                                                '|'
                                                                , cast(Comments as nvarchar(max))
                                                                , len(cast(Comments as nvarchar(max))) - 22
                                                                    )
                                                        , 22
                                                             )
                                                    , '|' +  @rickScore
                                                )
                                    else cast(Comments as nvarchar(max)) + '|' + @rickScore end 
                            end
                    where OrderId = @OrderId"))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter("rickScore", notes));
                            sqlCmd.Parameters.Add(new SqlParameter("OrderId", orderId));
                            defaultDB.ExecuteNonQuery(sqlCmd);

                            string orderNo = string.Empty;
                            if (dt.Rows.Count > 0)
                                orderNo = dt.Rows[0]["txnID"].ToString();
                            // Components.Email.SendError("ToError500", "Maxmind process data. CacheItemRemovedCallback: OrderNo: " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                            Components.Email.SendReport("ToReportPayment", "[MaxMind] OrderNo " + orderNo, "INPUT: " + queryString.Replace("license_key=" + _input.requireField.licenseKey, "").Replace("&", "<br/>") + "<br/>-----------------<br/>RESULT: " + resultString.Replace(";", "<br />"));
                        }
                    }
                }
                #endregion

                //send imail raw string.
                using (DbCommand sqlCmd = defaultDB.GetSqlStringCommand(@"
                    insert into StoreOrderLog values(@OrderId, getdate(), 'Maxmind.dll', 'WriteRawQr', @QueryString)"))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("OrderId", orderId));
                    sqlCmd.Parameters.Add(new SqlParameter("QueryString", "QueryString" + queryString + "; Result: " + resultString));
                    defaultDB.ExecuteNonQuery(sqlCmd);
                }
            }
            catch (Exception ex) {
                Components.Email.SendError("ToError500", "Maxmind process data. CacheItemRemovedCallback: OrderId: " + orderId.ToString(), ex.ToString());
            }
        }
        private void process()
        {
            queryString = getQueryString();

            if (!string.IsNullOrEmpty(queryString))
            {
                string url = UriService + "?" + queryString;
                resultString = getResponseString(url);

                if (!string.IsNullOrEmpty(resultString))
                    getData();
            }
        }
        private void getData() {
            if (string.IsNullOrEmpty(resultString)) {
                Components.Email.SendError("ToError500", "Maxmind process data. : resultString empty: " + "", "");
                return;
            };

            string[] resultArr = resultString.Split(';');

            if(resultArr.Any(i => i.Contains("riskScore"))){
                string riskScoreResponse = resultArr.First(i => i.Contains("riskScore"));
                var arr = riskScoreResponse.Split('=');
                if (arr.Length > 1) //has riskScore
                {
                    decimal riskScoreVal = 0;
                    if (decimal.TryParse(arr[1], out riskScoreVal))
                    {
                        riskScore = new RiskScore()
                        {
                            riskScore = riskScoreVal
                        };
                    }
                }
            }
            if (resultArr.Any(i => i.Contains("maxmindID")))
            {
                string maxmindIDResponse = resultArr.First(i => i.Contains("maxmindID"));
                var arr = maxmindIDResponse.Split('=');
                if (arr.Length > 1) //has riskScore
                {
                    this.maxmindID = arr[1];
                }
            }
            if (resultArr.Any(i => i.Contains("err")))
            {
                string err = resultArr.First(i => i.Contains("err"));
                var arr = err.Split('=');
                if (arr.Length > 1) //has riskScore
                {
                    string errString = arr[1].Trim();
                    if (errString == "MAX_REQUESTS_REACHED")
                    {
                        this.errorReporting = ErrorReporting.MAX_REQUESTS_REACHED;
                    }
                }
            }
        }
        private string getQueryString()
        {
            StringBuilder queryString = new StringBuilder();
            
            //add license_key
            if (string.IsNullOrEmpty(_input.requireField.licenseKey))
                throw new Exception("licenseKey empty.");
            else
                queryString.AppendFormat("license_key={0}", _input.requireField.licenseKey);
            
            //ip
            if (string.IsNullOrEmpty(_input.requireField.ipAddress))
                throw new Exception("ipAddress empty.");
            else
                queryString.AppendFormat("&i={0}", _input.requireField.ipAddress);

            #region BillingAddr
            if (!string.IsNullOrEmpty(_input.billingAddress.City))
                queryString.AppendFormat("&city={0}", _input.billingAddress.City);
            if (!string.IsNullOrEmpty(_input.billingAddress.Country))
                queryString.AppendFormat("&country={0}", _input.billingAddress.Country);
            if (!string.IsNullOrEmpty(_input.billingAddress.Postal))
                queryString.AppendFormat("&postal={0}", _input.billingAddress.Postal);
            if (!string.IsNullOrEmpty(_input.billingAddress.Region))
                queryString.AppendFormat("&region={0}", _input.billingAddress.Region);
            #endregion

            #region Shipping address
            if (!string.IsNullOrEmpty(_input.shippingAddress.shipAddr))
                queryString.AppendFormat("&shipAddr={0}", _input.shippingAddress.shipAddr);
            if (!string.IsNullOrEmpty(_input.shippingAddress.shipCity))
                queryString.AppendFormat("&shipCity={0}", _input.shippingAddress.shipCity);
            if (!string.IsNullOrEmpty(_input.shippingAddress.shipRegion))
                queryString.AppendFormat("&shipRegion={0}", _input.shippingAddress.shipRegion);
            if (!string.IsNullOrEmpty(_input.shippingAddress.shipPostal))
                queryString.AppendFormat("&shipPostal={0}", _input.shippingAddress.shipPostal);
            if (!string.IsNullOrEmpty(_input.shippingAddress.shipCountry))
                queryString.AppendFormat("&shipCountry={0}", _input.shippingAddress.shipCountry);
            #endregion

            #region user data
            if (!string.IsNullOrEmpty(_input.userData.domain))
                queryString.AppendFormat("&domain={0}", _input.userData.domain);
            if (!string.IsNullOrEmpty(_input.userData.custPhone))
                queryString.AppendFormat("&custPhone={0}", _input.userData.custPhone);
            if (!string.IsNullOrEmpty(_input.userData.emailMD5))
                queryString.AppendFormat("&emailMD5={0}", _input.userData.emailMD5);
            if (!string.IsNullOrEmpty(_input.userData.usernameMD5))
                queryString.AppendFormat("&usernameMD5={0}", _input.userData.usernameMD5);
            #endregion

            #region Bin related
            if (!string.IsNullOrEmpty(_input.binRelated.bin))
                queryString.AppendFormat("&bin={0}", _input.binRelated.bin);
            if (!string.IsNullOrEmpty(_input.binRelated.binName))
                queryString.AppendFormat("&binName={0}", _input.binRelated.binName);
            if (!string.IsNullOrEmpty(_input.binRelated.binPhone))
                queryString.AppendFormat("&binPhone={0}", _input.binRelated.binPhone);
            #endregion

            #region Transaction Linking
            if (!string.IsNullOrEmpty(_input.transactionLinking.user_agent))
                queryString.AppendFormat("&user_agent={0}", _input.transactionLinking.user_agent);
            if (!string.IsNullOrEmpty(_input.transactionLinking.accept_language))
                queryString.AppendFormat("&accept_language={0}", _input.transactionLinking.accept_language);
            #endregion

            #region Transaction Information
            if (!string.IsNullOrEmpty(_input.transactionInfomation.txnID))
                queryString.AppendFormat("&txnID={0}", _input.transactionInfomation.txnID);
            if (_input.transactionInfomation.order_amount != null)
                queryString.AppendFormat("&order_amount={0}", _input.transactionInfomation.order_amount);
            if (!string.IsNullOrEmpty(_input.transactionInfomation.order_currency))
                queryString.AppendFormat("&order_currency={0}", _input.transactionInfomation.order_currency);
            if (!string.IsNullOrEmpty(_input.transactionInfomation.shopID))
                queryString.AppendFormat("&shopID={0}", _input.transactionInfomation.shopID);
            if (_input.transactionInfomation.txn_type != null)
                queryString.AppendFormat("&txn_type={0}", _input.transactionInfomation.txn_type.ToString());
            #endregion

            #region Credit Card Check - Misc
            if (_input.creditCartCheck.avs_result != null)
                queryString.AppendFormat("&avs_result={0}", _input.creditCartCheck.avs_result.ToString());
            if (_input.creditCartCheck.cvv_result != null)
                queryString.AppendFormat("&cvv_result={0}", _input.creditCartCheck.cvv_result.ToString());

            if (_input.misc.requested_type != null)
                queryString.AppendFormat("&requested_type={0}", _input.misc.requested_type.ToString());
            #endregion

            return queryString.ToString();
        }
    }
}
