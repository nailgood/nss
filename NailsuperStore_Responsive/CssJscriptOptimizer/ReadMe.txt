This library features controls designed to automatically "C4" (compact (minify), compress, combine, and cache)
javascript and css files in an ASP.Net web project.  All of this is done without requiring any changes to 
the web server or the build process.  

Steps to use:

In the web.config file:

1. Within the configSections element, add:

		<section name="optimizerSection" type="CssJscriptOptimizer.ConfigurationSections.OptimizerSection, 
			CssJscriptOptimizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>

2. Within the configuration element, add the optimizerSection.  For example:

		<optimizerSection
		  enable="true"
		  enableScriptCompression="true"
		  enableSheetCompression="true"
		  enableScriptMinification="true"
		  enableSheetMinification="true"
		  fullHandlerPath="~/CnScriptResource.ashx"
		  >
			<add key="1"  path="~/Scripts/Script01.js" />
			<add key="2"  path="~/Scripts/Script02.js" />
			<add key="3"  path="~/Scripts/Script03.js" />
			<add key="4" path="~/Styles/test1.css" />
			<add key="5" path="~/Styles/test2.css" />
			<add key="6" path="~/Styles/test3.css" />
		</optimizerSection>

	where the fullHandlerPath is the path to the name of your httphandler that derives from CssJscriptOptimizerHandler
	and where the key is any arbitrary and url-friendly unique key value, and where the path is the path to
	the script or css file in your project.  Note that all your script and css files that you desire to "C4" in your 
	project should be listed here.  Also note that all paths should start with the "~".
	
	Note that setting the attribute enable="false" will turn off the optimizer.
	
3. Create a new handler file for your web project.  Have it derive from CssJscriptOptimizerHandler.  The path
	to this file should be stated in the fullHandlerPath attribute of the optimizerSection.
	
4.  Register the controls in your .aspx, .ascx, and/or .master file via:
	
		<%@ Register Assembly="CssJscriptOptimizer" Namespace="CssJscriptOptimizer.Controls"
		TagPrefix="cc1" %>
	
4.  In your aspx file, place the StyleSheetCombiner and ScriptCombiner control tags around your css and 
	script declarations.  For example:  

		<cc1:StyleSheetCombiner ID="sheetCombiner" runat="server">
		
			<link rel="stylesheet" href="Styles/test1.css" />
			<link rel="stylesheet" href='<%# sheetCombiner.ResolveUrl("~/Styles/test2.css")%>' />
			<link rel="stylesheet" href='<%# sheetCombiner.ResolveUrl("~/Styles/test3.css")%>' />

		</cc1:StyleSheetCombiner>
		
		<cc1:ScriptCombiner ID="scriptCombiner" runat="server">

			<script src='<%# scriptCombiner.ResolveUrl("~/Scripts/Script01.js")%>' type="text/javascript" />
			<script src='<%# scriptCombiner.ResolveUrl("~/Scripts/Script02.js")%>' type="text/javascript"></script>
			<script src="Scripts/Script03.js" type="text/javascript"></script>

		</cc1:ScriptCombiner>
		
	Note:  do not use <%= controlName.ResolveUrl("~/someUrl")%>, but the data binding version 
	<%# controlName.ResolveUrl("~/someUrl")%> instead.  controlName.DataBind() is called internally by the control.
		
		
	If any of your stylesheet links makes use of the media or title attribute, then you may not want to
	include those within the StyleSheetCombiner control because these attributes will be lost, resulting
	in improper css rendering.


