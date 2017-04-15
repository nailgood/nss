using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
//using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace FaceBook
{
    public class FacebookFeed
    {
        public string Id { get; set; }
    }
    public class OpenGraph
    {
        #region Properties and Instance members
        private string baseUrl = "https://graph.facebook.com/";
        public const string GET = "GET";
        public const string POST = "POST";
        #endregion

        #region Methods
        /// <summary>
        /// Base method to call the open graph api via the speciifed url. The object
        /// returned must be specified by the calling method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private T Call<T>(string url, string methodType) where T : class
        {
            T result=null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = methodType;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    string jsonData = reader.ReadToEnd();
                    result = (T)jsSerializer.Deserialize<T>(jsonData);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Status);
                if (ex.Response != null)
                {
                    // can use ex.Response.Status, .StatusDescription
                    if (ex.Response.ContentLength != 0)
                    {                        
                        using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                            string jsonData = reader.ReadToEnd();
                            result = (T)jsSerializer.Deserialize<T>(jsonData);
                        }
                    }
                }    
            }

            return result;
        }
        public FacebookFeed PostMessageToCurrentUsersWall(string message, string link, string picture, string desc, string accessToken)
        {
            string parameters = "me/feed?message=" + message + "&link=" + link + "&picture=" + picture + "&description=" + desc + "&access_token=" + accessToken;
            string url = baseUrl + parameters;

            FacebookFeed feed = Call<FacebookFeed>(url, POST);

            return feed;
        }
        #endregion
    }
}
