<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Promotion.aspx.vb" Inherits="store_Promotion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=MixMatch.Description%></title>
    <link rel="stylesheet" href="/includes/style.css" />
    <link rel="stylesheet" href="/includes/theme.css" />
    <script language="javascript">
		function gotoLink(sUrl) {
			window.opener.location = sUrl;
			window.close();
		}
    </script>
    <meta name="robots" content="noindex,nofollow" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="dhtmltooltip"></div>    
    <script src="/includes/ddtooltip.js" type="text/javascript"></script>
    <div runat="server" id="div1">
    <h4 class="mag" style="margin-left:4px;margin-top:6px;margin-bottom:6px;">These items are available for purchase<%if Promotion.Type = PromotionType.LeastExpensive then%>/discount<%end if%> in this promotion</h4>
    <asp:DataList runat="server" ID="dl" RepeatColumns="3" RepeatDirection="vertical" CellSpacing="2" CellPadding="3" Width="100%" ItemStyle-Width="33%" ItemStyle-VerticalAlign="top">
		<ItemTemplate>
			<table border="0" cellspacing="0" cellpadding="5" style="border:solid 1px #e3e3e3;background:#f3f3f3;" width="100%"><tr valign="top">
<td>
<%#IIf(IsDBNull(Container.DataItem("image")), "", "<a href=""javascript:void(0);"" onclick=""gotoLink('/" & ReWriteURL.ReplaceUrl(Container.DataItem("itemname")) & "/" & Container.DataItem("DepartmentId") & "_" & Container.DataItem("itemid") & ".aspx');return false;""><div style=""height: 35px; width:35px; border:solid 1px #999999; background-image: url(" & IIf(IsDBNull(Eval("Image")), "/assets/items/cart/na.jpg", "/assets/items/free/" & Eval("Image")) & "); background-repeat:no-repeat""><img src=""/assets/nobg.gif"" height=""100%"" width=""100%"" style=""border-style:none"" alt=""" & Container.DataItem("itemname") & """ /></div></a><br />")%>
</td>
<td width="100%"><a href="javascript:void(0);" onclick="gotoLink('/<%# ReWriteUrl.ReplaceUrl(Container.DataItem("itemname"))%>/<%#Container.DataItem("DepartmentId")%>_<%#Container.DataItem("itemid")%>.aspx');return false;"><%#Container.DataItem("itemname")%></a><br />#<%#Container.DataItem("SKU")%></td>
			</tr></table>
		</ItemTemplate>
    </asp:DataList>
    </div>

    <div runat="server" id="div2">
    <h4 class="mag" style="margin-left:4px;margin-top:16px;margin-bottom:6px;">These items are available for discount in this promotion</h4>
    <asp:DataList runat="server" ID="dl2" RepeatColumns="3" RepeatDirection="vertical" CellSpacing="2" CellPadding="3" Width="100%" ItemStyle-Width="33%" ItemStyle-VerticalAlign="top">
		<ItemTemplate>
			<table border="0" cellspacing="0" cellpadding="5" style="border:solid 1px #e3e3e3;background:#f3f3f3;" width="100%"><tr valign="top">
			<td>
			<%#IIf(IsDBNull(Container.DataItem("image")), "", "<a href=""javascript:void(0);"" onclick=""gotoLink('/" & ReWriteURL.ReplaceUrl(Container.DataItem("itemname")) & "/" & Container.DataItem("DepartmentId") & "_" & Container.DataItem("itemid") & ".aspx');return false;""><div style=""height: 35px; width:35px; border:solid 1px #999999; background-image: url(" & IIf(IsDBNull(Eval("Image")), "/assets/items/cart/na.jpg", "/assets/items/free/" & Eval("Image")) & "); background-repeat:no-repeat""><img src=""/assets/nobg.gif"" height=""100%"" width=""100%"" style=""border-style:none"" alt=""" & Container.DataItem("itemname") & """ /></div></a><br />")%>
			</td>
			<td width="100%"><a href="javascript:void(0);" onclick="gotoLink('/<%# ReWriteUrl.ReplaceUrl(Container.DataItem("itemname"))%>/<%#Container.DataItem("DepartmentId")%>_<%#Container.DataItem("itemid")%>.aspx');return false;"><%#Container.DataItem("itemname")%></a><br />#<%#Container.DataItem("SKU")%></td>
			</tr></table>
		</ItemTemplate>
    </asp:DataList>
    </div>
    </form>
</body>
</html>
