using System;
using System.Collections.Generic;
using System.Text;

namespace PayPalHandler
{
    public class PostToRemoteServer
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        private string m_sUrl = "";
        private string m_sMethod = "post";
        private string m_sFormName = "form1";

        public PostToRemoteServer(string sActionURL)
        {
            m_sUrl = sActionURL;
        }

        public PostToRemoteServer()
        {
        }

        #region Properties

        public string ActionURL
        {
            set { m_sUrl = value; }
            get { return m_sUrl; }
        }

        public string Method
        {
            set { m_sMethod = value; }
            get { return m_sMethod; }
        }

        public string FormName
        {
            set { m_sFormName = value; }
            get { return m_sFormName; }
        }

        #endregion // Properties

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, ActionURL));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");
            System.Web.HttpContext.Current.Response.End();
        }

        public void Post(string sPageContent)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, ActionURL));

            System.Web.HttpContext.Current.Response.Write(sPageContent);
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");
            System.Web.HttpContext.Current.Response.End();
        }
    }
}
