using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.Configuration;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.Handlers;
using System.Web.Caching;
using System.Net;

using CssJscriptOptimizer.ConfigurationSections;
using CssJscriptOptimizer.Minifiers;
using System.Text.RegularExpressions;

namespace CssJscriptOptimizer
{
    public class CssJscriptOptimizer
    {
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromDays(30);
        private List<string> _files = new List<string>();
        List<string> _localFiles = new List<string>();
        private bool _isCss;



        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the 
        /// <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to
        /// the intrinsic server objects (for example, Request, Response, Session, and Server) 
        /// used to service HTTP requests.</param>
        ///
        private string GetKeyCSS(HttpContext context)
        {
            string keys = context.Server.UrlDecode(context.Request.Params["keys"]);
           // keys = "page.product";
            return keys;
        }
        public void RunProcessRequest(HttpContext context)
        {
            bool shouldProcessRequest = true;
            string[] scriptKeys = null;
            string keys = GetKeyCSS(context);          
            string scriptResourcePath = String.Empty;
            ScriptManager objScriptManager = new ScriptManager();
            StringBuilder scriptBuilder = new StringBuilder();
            IHttpHandler handler = new ScriptResourceHandler();
            string cacheKey = context.Request.Url.PathAndQuery;

            if (String.IsNullOrEmpty(keys) || keys.Equals("-1"))
            {
                shouldProcessRequest = false;
            }

            if (shouldProcessRequest)
            {
                _files = GetFilesList(context);
                foreach (string file in _files)
                {
                    if (!file.ToLower().Contains("http://") || !file.ToLower().Contains("https://"))
                    {
                        _localFiles.Add(file);
                    }

                }

                if (_files.Count > 0 && _files[0].ToLower().EndsWith(".css"))
                {
                    _isCss = true;
                }
                else
                {
                    _isCss = false;
                }

                string compressionType = GetCompressionType(context, _isCss);

                if (context.Cache[GetCacheKey(cacheKey, compressionType)] != null)
                {//write the cached bytes to the response stream
                    WriteFromCache(context, compressionType, cacheKey);
                }
                else
                {
                    scriptKeys = keys.Split('.'); //splitting querystring "keys"

                    foreach (string key in scriptKeys)
                    {
                        ScriptElement element = OptimizerConfig.GetScriptByKey(key);
                        if (element == null)
                        {
                            continue;
                        }

                        #region Combining script files into a StringBuilder

                        if (element != null)
                        {
                            string absolutePath = OptimizerHelper.GetAbsolutePath(element);

                            if (OptimizerHelper.IsAbsolutePathExists(absolutePath))
                            {
                                using (StreamReader objJsReader = new StreamReader(absolutePath, true))
                                {

                                    string file = objJsReader.ReadToEnd();
                                    string fixedUrlFile = file;

                                    if (element.Path.EndsWith(".css"))
                                    {
                                        fixedUrlFile = FixUrlPaths(file, element.Path);
                                    }

                                    scriptBuilder.Append(fixedUrlFile);

                                    scriptBuilder.AppendLine();
                                }

                            }
                            else
                            {// check if path is a url
                                if (absolutePath.ToLower().Contains("http://") || absolutePath.ToLower().Contains("https://"))
                                {
                                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(absolutePath);
                                    req.KeepAlive = false;
                                    req.UseDefaultCredentials = true;

                                    //to do:  determine content type

                                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                                    Encoding enc = System.Text.Encoding.GetEncoding(1252);
                                    StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);

                                    string responseString = loResponseStream.ReadToEnd();

                                    scriptBuilder.Append(responseString);
                                    scriptBuilder.AppendLine();

                                    loResponseStream.Close();
                                    response.Close();


                                }

                            }
                        }
                        #endregion
                    }

                    //combine files if there's no entry for the combined files in the cache
                    WriteCombinedScriptsToOutputStream(context, scriptBuilder, cacheKey, compressionType);
                }

            }

        }

        protected List<string> GetFilesList(HttpContext context)
        {
            List<string> files = new List<string>();
            string keys = context.Server.UrlDecode(GetKeyCSS(context));
            string[] scriptKeys = keys.Split('.'); //splitting querystring "keys"

            foreach (string key in scriptKeys)
            {
                ScriptElement element = OptimizerConfig.GetScriptByKey(key);

                if (element != null)
                {
                    string absolutePath = OptimizerHelper.GetAbsolutePath(element);

                    if (OptimizerHelper.IsAbsolutePathExists(absolutePath))
                    {
                        files.Add(absolutePath);
                    }
                }
            }

            return files;
        }
        /*
        protected string FixUrlPaths(string cssFile, string cssFilePath)
        {
            Regex dirsToGoBackRegex = new Regex(@"\.\."); //matching ".."

            Regex urlRegex = new Regex(@"url\(.*\)"); //matchin any url entry

            MatchCollection urlMatches = urlRegex.Matches(cssFile);

            var uniqueMatches = (from Match m in urlMatches
                                 select m.Value).Distinct();

            foreach (string urlMatch in uniqueMatches)
            {
                if (urlMatch.ToLower().Contains("http://") || urlMatch.ToLower().Contains("https://"))
                {//skip any fully qualified url paths
                    continue;
                }

                //making sure to get url(...) only and not any additional parentheses after it
                string url = urlMatch.Substring(0, urlMatch.IndexOf(")") + 1);

                url = url.Replace("url(", "");
                url = url.Replace(")", "");
                url = url.Replace("\"", "");

                //everytime we see a .. in the path, go back a directory on the 
                //referenced path cssFilePath, most likely from the web.config
                MatchCollection dirsToGoBack = dirsToGoBackRegex.Matches(url);
                int numDirsToGoBack = dirsToGoBack.Count;

                if (cssFilePath.LastIndexOf("/") != -1)
                {
                    //go back a directory so file name is not included in path calculation
                    cssFilePath = cssFilePath.Substring(0, cssFilePath.LastIndexOf("/"));
                }

                for (int i = 0; i < numDirsToGoBack; i++)
                {
                    //go back a directory on the cssFilePath
                    if (cssFilePath.LastIndexOf("/") != -1)
                    {
                        cssFilePath = cssFilePath.Substring(0, cssFilePath.LastIndexOf("/"));
                    }

                    if (url.LastIndexOf("..") != -1)
                    {//remove the .. from the url to prepare it for concatenation with an absolute path
                        url = url.Substring(url.LastIndexOf("..") + 2);
                    }
                }

                if (!cssFilePath.EndsWith("/") && !url.StartsWith("/"))
                {
                    cssFilePath = cssFilePath + "/";
                }

                string correctUrl = VirtualPathUtility.ToAbsolute(cssFilePath + url);

                string finalUrl = "url(" + correctUrl + ")";

                cssFile = cssFile.Replace(urlMatch, finalUrl);

            }

            return cssFile;

        }*/

        protected string FixUrlPaths(string cssFile, string filePath)
        {
            Regex dirsToGoBackRegex = new Regex(@"\.\."); //matching ".."

            Regex urlRegex = new Regex(@"url\(.*\)"); //matchin any url entry

            MatchCollection urlMatches = urlRegex.Matches(cssFile);

            var uniqueMatches = (from Match m in urlMatches
                                 select m.Value).Distinct();
            string cssFilePath = filePath;

            if (cssFilePath.LastIndexOf("/") != -1)
            {
                //go back a directory so file name is not included in path calculation
                cssFilePath = cssFilePath.Substring(0, cssFilePath.LastIndexOf("/"));
            }

            foreach (string urlMatch in uniqueMatches)
            {
                
                if (urlMatch.ToLower().Contains("http://") || urlMatch.ToLower().Contains("https://"))
                {//skip any fully qualified url paths
                    continue;
                }

                //making sure to get url(...) only and not any additional parentheses after it
                string url = urlMatch.Substring(0, urlMatch.IndexOf(")") + 1);

                url = url.Replace("url(", "");
                url = url.Replace(")", "");
                url = url.Replace("\"", "");
                url = url.Replace("'", "");
                //everytime we see a .. in the path, go back a directory on the 
                //referenced path cssFilePath, most likely from the web.config
                MatchCollection dirsToGoBack = dirsToGoBackRegex.Matches(url);
                int numDirsToGoBack = dirsToGoBack.Count;

                for (int i = 0; i < numDirsToGoBack; i++)
                {
                    //go back a directory on the cssFilePath
                    if (cssFilePath.LastIndexOf("/") != -1)
                    {
                        cssFilePath = cssFilePath.Substring(0, cssFilePath.LastIndexOf("/"));
                    }

                    if (url.LastIndexOf("..") != -1)
                    {//remove the .. from the url to prepare it for concatenation with an absolute path
                        url = url.Substring(url.LastIndexOf("..") + 2);
                    }
                }

                if (!cssFilePath.EndsWith("/") && !url.StartsWith("/"))
                {
                    cssFilePath = cssFilePath + "/";
                }

                string correctUrl = VirtualPathUtility.ToAbsolute(cssFilePath + url);

                string finalUrl = "url(" + correctUrl + ")";

                cssFile = cssFile.Replace(urlMatch, finalUrl);

            }

            return cssFile;

        }


        protected void WriteCombinedScriptsToOutputStream(HttpContext context, StringBuilder scriptBuilder,
            string cacheKey, string compressionType)
        {
            context.Response.Clear();

            scriptBuilder.AppendLine();

            string combinedScripts = scriptBuilder.ToString();

            if (_isCss)
            {//dealing with css files

                if (OptimizerConfig.EnableSheetMinification)
                {
                    combinedScripts = combinedScripts.CssMinify();
                }

                context.Response.ContentType = "text/css";

            }
            else
            {// dealing with javascript files
                if (OptimizerConfig.EnableScriptMinification)
                {
                    combinedScripts = JsMinifier.GetMinifiedCode(combinedScripts);
                }

                context.Response.ContentType = "application/x-javascript";

            }

            //Creating dependency on the server on any referenced script files
            //Cache will reset when any changes to these files are detected
            CacheDependency dependency = new CacheDependency(_localFiles.ToArray());

            if (compressionType == "gzip")
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(new GZipStream(stream, CompressionMode.Compress), Encoding.UTF8))
                    {
                        writer.Write(combinedScripts);
                    }
                    byte[] buffer = stream.ToArray();

                    // Cache the combined response so that it can be directly written
                    // in subsequent calls 
                    context.Cache.Insert(GetCacheKey(cacheKey, compressionType),
                        buffer, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                        _cacheDuration);

                    WriteBytes(buffer, context, compressionType, cacheKey);

                }
            }
            else if (compressionType == "deflate")
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(new DeflateStream(stream, CompressionMode.Compress), Encoding.UTF8))
                    {
                        writer.Write(combinedScripts);
                    }
                    byte[] buffer = stream.ToArray();

                    // Cache the combined response so that it can be directly written
                    // in subsequent calls 
                    context.Cache.Insert(GetCacheKey(cacheKey, compressionType),
                        buffer, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                        _cacheDuration);

                    WriteBytes(buffer, context, compressionType, cacheKey);
                }
            }
            else
            {
                // Cache the combined response so that it can be directly written
                // in subsequent calls 
                context.Cache.Insert(GetCacheKey(cacheKey, compressionType),
                    combinedScripts, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                    _cacheDuration);

                //no compression plain text...
                context.Response.AddHeader("Content-Length", combinedScripts.Length.ToString());
                context.Response.Write(combinedScripts);
            }

            scriptBuilder = null;
        }

        /// <summary>
        /// Gets the compression type that the browser supports
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isCss">is Css or Jscript file to compress</param>
        /// <returns>none, deflate, or gzip</returns>
        protected string GetCompressionType(HttpContext context, bool isCss)
        {
            string compressionType = "none";
            string encodingTypes = string.Empty;
            if ((!isCss && OptimizerConfig.EnableScriptCompression) || (isCss && OptimizerConfig.EnableSheetCompression))
            {
                encodingTypes = context.Request.Headers["Accept-Encoding"];

                if (!string.IsNullOrEmpty(encodingTypes))
                {
                    encodingTypes = encodingTypes.ToLower();
                    if (context.Request.Browser.Browser == "IE")
                    {
                        if (context.Request.Browser.MajorVersion < 6)
                        {
                            compressionType = "none";
                        }
                        else if (context.Request.Browser.MajorVersion == 6 &&
                            !string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_USER_AGENT"]) &&
                            context.Request.ServerVariables["HTTP_USER_AGENT"].Contains("EV1"))
                        {
                            compressionType = "none";
                        }
                    }
                    if (encodingTypes.Contains("gzip") || encodingTypes.Contains("x-gzip") || encodingTypes.Contains("*"))
                    {
                        compressionType = "gzip";
                    }
                    else if (encodingTypes.Contains("deflate"))
                    {
                        compressionType = "deflate";
                    }
                }
            }
            else
            {
                compressionType = "none";
            }

            return compressionType;
        }

        protected void SetResponseCache(HttpResponse response)
        {
            HttpCachePolicy cache = response.Cache;
            DateTime _now = DateTime.Now;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams["keys"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(_now + TimeSpan.FromDays(7));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(_now);

            //Creating dependency on the browser's cache on any referenced script files
            //Browser cache will reset when any changes to these files are detected
            response.AddFileDependencies(_localFiles.ToArray());
            response.Cache.SetETagFromFileDependencies();
            response.Cache.SetLastModifiedFromFileDependencies();

        }

        protected bool WriteFromCache(HttpContext context, string compressionType, string cacheKey)
        {
            if (!_isCss)
            {
                context.Response.ContentType = "application/x-javascript";
            }
            else
            {
                context.Response.ContentType = "text/css";
            }

            byte[] responseBytes = context.Cache[GetCacheKey(cacheKey, compressionType)] as byte[];

            if (null == responseBytes || 0 == responseBytes.Length)
            {
                string responseString = context.Cache[GetCacheKey(cacheKey, compressionType)] as string;
                if (string.IsNullOrEmpty(responseString))
                {
                    return false;
                }
            }
            this.WriteBytes(responseBytes, context, compressionType, cacheKey);

            return true;
        }

        protected void WriteBytes(byte[] bytes, HttpContext context, string compressionType, string cacheKey)
        {
            HttpResponse response = context.Response;

            SetResponseCache(response);

            if (compressionType != "none")
            {
                response.AddHeader("Content-encoding", compressionType);
                response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                string uncompressedScript = context.Cache[GetCacheKey(cacheKey, compressionType)].ToString();
                response.AddHeader("Content-Length", uncompressedScript.Length.ToString());
                response.Write(uncompressedScript);
            }
            response.Flush();
        }

        protected string GetCacheKey(string key, string compressionType)
        {
            return "HttpCombiner." + key + "." + compressionType;
        }



    }
}
