using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CssJscriptOptimizer
{
    public class CssJscriptOptimizerHandler: IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            CssJscriptOptimizer cssjs = new CssJscriptOptimizer();
            cssjs.RunProcessRequest(context);
        }

    }
}
