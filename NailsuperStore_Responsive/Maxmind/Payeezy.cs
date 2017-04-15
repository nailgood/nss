using DataLayer;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Payeezy
{
    public static class Payeezy
    {
        public static bool Authorize(string payload, ref StoreOrderPayflowRow storeOrder)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(payload);
                var apiKey = "3x6HP3vph5EVoWd3HwGo9tiIdOGrVp8M";
                var timeStamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(CultureInfo.InvariantCulture);
                var nonce = (10000000000000000000 * new Random(DateTime.Now.Millisecond).NextDouble()).ToString("0000000000000000000");
                var merchantToken = "fdoa-73d604d675d975a15d80cb8601fbdf2f73d604d675d975a1";
                var secretKey = "69c17fbbedd2c558b4b18b81686f1d1e3d00e27a62e6bc99c805417fe542244a";

                var post = (HttpWebRequest)HttpWebRequest.Create("https://api-cert.payeezy.com/v1/transactions");
                post.Method = "POST";
                post.KeepAlive = true;
                post.Accept = "*/*";
                post.Headers.Add("Accept-Encoding", "gzip");
                post.Headers.Add("Accept-Language", "en-US");
                post.Headers.Add("apikey", apiKey);
                post.Headers.Add("nonce", nonce);
                post.Headers.Add("timestamp", timeStamp);
                var authorize = CreateHMAC(apiKey, secretKey, merchantToken, payload, nonce, timeStamp);
                post.Headers.Add("Authorization", authorize);
                post.ContentType = "application/json";
                post.Headers.Add("token", merchantToken);
                post.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7";
                post.GetRequestStream().Write(byteArray, 0, byteArray.Length);
                var response = post.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseFromServer = reader.ReadToEnd();
                dynamic payeezyObj = JsonConvert.DeserializeObject<dynamic>(responseFromServer);

                storeOrder.PnRef = payeezyObj.transaction_id;
                storeOrder.Result = payeezyObj.transaction_status == "approved" ? 0 : 1;
                storeOrder.RespMsg = payeezyObj.transaction_status;
                storeOrder.RespMsg = payeezyObj.gateway_message;
                //storeOrder.av
                //op.AvsAddr = IIf(Trans.Response.TransactionResponse.AVSAddr = Nothing, "", Trans.Response.TransactionResponse.AVSAddr)
                //op.AvsZip = IIf(Trans.Response.TransactionResponse.AVSZip = Nothing, "", Trans.Response.TransactionResponse.AVSZip)
                //op.Cvv2Match = IIf(Trans.Response.TransactionResponse.CVV2Match = Nothing, "", Trans.Response.TransactionResponse.CVV2Match)
                return true;
            }
            catch (WebException ex) {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);
                var messageFromServer = obj.Error.messages[0].description;
                storeOrder.Result = 2;
                storeOrder.RespMsg = messageFromServer;
                Microsoft.Practices.EnterpriseLibrary.Data.Database defaultDB = DatabaseFactory.CreateDatabase();
                using (DbCommand sqlCmd = defaultDB.GetSqlStringCommand(@"
                    insert into StoreOrderLog values(@OrderId, getdate(), 'Payeezy', 'WriteRawQr', @Object)"))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("OrderId", storeOrder.OrderId));
                    sqlCmd.Parameters.Add(new SqlParameter("Object", payload));
                    defaultDB.ExecuteNonQuery(sqlCmd);
                }
                return false;
            }
        }

        public static string CreateHMAC(string apiKey, string apiSecret, string token,
        string payload, string nonce, string timeStamp)
        {
            var hmacData = apiKey + nonce + timeStamp + token + payload;
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            var encBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hmacData));
            var encStr = ByteArrayToHexString(encBytes);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes((encStr)));
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
