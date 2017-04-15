using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter
{
   public class OAuthData
    {
        public string ConsumerKey { get; private set; }

        public string ConsumerSecret { get; private set; }

        public int ID { get; private set; }

        public string ScreenName { get; set; }

        public string Token { get; set; }

        public string TokenSecret { get; set; }

        public int? UserID { get; set; }
        public OAuthData()
        { 
        }
        public OAuthData(string _ConsumerKey, string _ConsumerSecret, int _ID, string _ScreenName, string _Token, string _TokenSecret)
        {
            ConsumerKey = _ConsumerKey;
            ConsumerSecret = _ConsumerSecret;
            ID = _ID;
            Token = _Token;
            TokenSecret = _TokenSecret;
            ScreenName = _ScreenName;
        }
    }
}
