using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CssJscriptOptimizer.ConfigurationSections
{
	/// <summary>
	/// OptimizerSection
	/// </summary>
	public class OptimizerSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public ScriptCollection Scripts
		{
			get { return (ScriptCollection)this[""]; }
			set { this[""] = value; }
		}

		[ConfigurationProperty("enable", IsRequired = true)]
		public bool Enable
		{
			get { return (bool)this["enable"]; }
			set { this["enable"] = value; }
		}
		[ConfigurationProperty("enableScriptCompression", IsRequired = true)]
		public bool EnableScriptCompression
		{
			get { return (bool)this["enableScriptCompression"]; }
			set { this["enableScriptCompression"] = value; }
		}

		[ConfigurationProperty("enableSheetCompression", IsRequired = true)]
		public bool EnableSheetCompression
		{
			get { return (bool)this["enableSheetCompression"]; }
			set { this["enableSheetCompression"] = value; }
		}

		[ConfigurationProperty("enableScriptMinification", IsRequired = true)]
		public bool EnableScriptMinification
		{
			get { return (bool)this["enableScriptMinification"]; }
			set { this["enableScriptMinification"] = value; }
		}

		[ConfigurationProperty("enableSheetMinification", IsRequired = true)]
		public bool EnableSheetMinification
		{
			get { return (bool)this["enableSheetMinification"]; }
			set { this["enableSheetMinification"] = value; }
		}

		[ConfigurationProperty("fullHandlerPath", IsRequired = true)]
		public string FullHandlerPath
		{
			get { return (string)this["fullHandlerPath"]; }
			set { this["fullHandlerPath"] = value; }
		}

        [ConfigurationProperty("cacheDate", IsRequired = true)]
        public string cacheDate
        {
            get { return (string)this["cacheDate"]; }
            set { this["cacheDate"] = value; }
        }
	}

	/// <summary>
	/// Section int the config file that has all the scripts
	/// </summary>
	public class ScriptCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ScriptElement();
		}
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ScriptElement)element).Key;
		}
	}

	/// <summary>
	/// A script in the config file
	/// </summary>
	public class ScriptElement : ConfigurationElement
	{
		[ConfigurationProperty("key", IsKey = true, IsRequired = true)]
		public string Key
		{
			get { return (string)base["key"]; }
			set { base["key"] = value; }
		}

		[ConfigurationProperty("path", IsRequired = true)]
		public string Path
		{
			get { return (string)base["path"]; }
			set { base["path"] = value; }
		}
	}

	/// <summary>
	/// Section in config file
	/// </summary>
	public class OptimizerConfig
	{
		protected static Dictionary<string, ScriptElement> _scripts;
		protected static bool _enable;
		protected static bool _enableScriptCompression;
		protected static bool _enableSheetCompression;
		protected static bool _enableScriptMinification;
		protected static bool _enableSheetMinification;
		protected static string _fullScriptHandlerPath;
        protected static string _cacheDate;

		/// <summary>
		/// constructor
		/// </summary>
		static OptimizerConfig()
		{
			_scripts = new Dictionary<string, ScriptElement>();

			OptimizerSection sec = null;
			sec = (OptimizerSection)System.Configuration.ConfigurationManager.GetSection("optimizerSection");

			foreach (ScriptElement i in sec.Scripts)
			{
				_scripts.Add(i.Key, i);
			}
			_enable = sec.Enable;
			_enableScriptCompression = sec.EnableScriptCompression;
			_enableSheetCompression = sec.EnableSheetCompression;
			_enableScriptMinification = sec.EnableScriptMinification;
			_enableSheetMinification = sec.EnableSheetMinification;
			_fullScriptHandlerPath = sec.FullHandlerPath;
            _cacheDate = sec.cacheDate;

		}

		/// <summary>
		/// Gets the script associated with the given key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static ScriptElement GetScriptByKey(string key)
		{
			ScriptElement objElement = null;
			try
			{
				objElement = _scripts[key];
			}
			catch 
			{ 
			}

			return objElement;
		}

		/// <summary>
		/// Gets the script associated with the given path
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static ScriptElement GetScriptByPath(string path)
		{
			ScriptElement objElement = null;
			foreach (KeyValuePair<string, ScriptElement> element in _scripts)
			{
				string fullElementPath = element.Value.Path.Replace("~", HttpRuntime.AppDomainAppVirtualPath);
				fullElementPath = fullElementPath.Replace("//", "/");

				if (fullElementPath == path)
				{
					objElement = element.Value;
					break;
				}
			}
			return objElement;
		}

		/// <summary>
		/// Gets value that determines whether the ScriptCombiner or SheetCombiner control are enabled or not
		/// </summary>
		public static bool Enable
		{
			get { return _enable; }
		}

		/// <summary>
		/// Gets value that determines whether the script files will be compressed or not
		/// </summary>
		public static bool EnableScriptCompression
		{
			get { return _enableScriptCompression; }
		}

		/// <summary>
		/// Gets the value that determines whether the stylesheet files will be compressed or not
		/// </summary>
		public static bool EnableSheetCompression
		{
			get { return _enableSheetCompression; }
		}

		/// <summary>
		/// Gets the value that determines whether the script files will be minified or not
		/// </summary>
		public static bool EnableScriptMinification
		{
			get { return _enableScriptMinification; }
		}

		/// <summary>
		/// Gets the value that determines whether the stylesheet files will be minified or not
		/// </summary>
		public static bool EnableSheetMinification
		{
			get { return _enableSheetMinification; }
		}

		/// <summary>
		/// Path of the handler that will handle the processing of the scripts and stylesheets
		/// </summary>
		public static string FullHandlerPath
		{
			get { return _fullScriptHandlerPath; }
		}

        public static string cacheDate
        {
            get { return _cacheDate; }
        }


	}

	/// <summary>
	/// Helper class for the Optimizer
	/// </summary>
	public class OptimizerHelper
	{
		/// <summary>
		/// Gets the file system path of the element
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static string GetAbsolutePath(ScriptElement element)
		{
			string _absolutePath = string.Empty;

			if (null != element)
			{
				if (!element.Path.StartsWith("~", StringComparison.OrdinalIgnoreCase) && !element.Path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
				{
					_absolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + element.Path);
				}
				else
				{
					_absolutePath = System.Web.HttpContext.Current.Server.MapPath(element.Path);
				}
			}

			return _absolutePath;
		}

		/// <summary>
		/// Gets whether the path is to an existing file
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool IsAbsolutePathExists(ScriptElement element)
		{
			return System.IO.File.Exists(GetAbsolutePath(element));
		}

		/// <summary>
		/// Gets whether the path is to an existing file
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsAbsolutePathExists(string path)
		{
			return System.IO.File.Exists(path);
		}
	}
}