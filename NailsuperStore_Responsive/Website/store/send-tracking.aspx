<%@ Page Language="VB" AutoEventWireup="false" CodeFile="send-tracking.aspx.vb" Inherits="store_send_tracking" %>
<%@ Import Namespace="DataLayer" %>
<%@ Register TagPrefix="CC" TagName="tracking" Src="~/controls/tracking.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Tracking Email - <%=SysParam.GetValue("CompanyName")%></title>
<style>
body
{	
	font: 12px/18px Arial;
	color: #454545;
}
#ship .titprice {
    border-bottom: 1px dotted #CCCCCC;
    font: bold 12px Arial;
    padding-bottom: 5px;
    padding-top:5px;
    text-align: left;
    vertical-align: bottom;
    width: 240px;
}
#ship .price1 
{
    border-bottom: 1px dotted #CCCCCC;
    font: bold 12px Arial;
    height: 22px;
    padding-bottom: 5px;
    text-align: right;
    vertical-align: bottom;
}
.btn150
{
	background:url("https://www.nailsuperstore.com/includes/theme/images/btn150.gif") no-repeat top left;
	width:150px;
	height:35px;
	margin-right:5px;
	border-style:none;
	cursor:pointer;
	border:none;
	color:White;
	font:bold 12px Verdana;
	padding-bottom:4px;
}
.line-track {
    border-bottom: 1px solid #DADADA;
    color: #BE048D;
    font-weight: bold;
    width: 100%;
}
.border {
    border: 1px solid #DADADA;
}
.h-info {
    background-color: #F5F5F5;
    height:auto; 
    overflow:hidden;
    padding: 15px 0 15px 10px;
}
.ship-status-italic {
    font-size: 11px;
    font-style: italic;
    padding-right: 20px;
    text-align: center;
}
</style>
</head>
<body style="color: #454545;font: 12px/18px Arial;">
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" border="0" style="width: 780px; height: 100%; border:solid 1px #dadada">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;
                       background-color:#f5f5f5; height: 93px; padding-bottom: 0;padding-left: 1px;padding-right: 1px;border-top: solid 1px #CCCCCC;border-bottom:solid 1px #CCCCCC;">
                        <tr>
                            <td style="float: left;padding-left: 10px;padding-top: 5px;text-align: left;width: 300px;">
                                <a href="<%=webRoot %>"><img alt="Logo" style="border: medium none; text-align: left;" src="<%=webRoot %>/includes/theme/images/logo_tracking.png"></a>
                            </td>
                            <td style="vertical-align:top;padding-top:10px;padding-right: 20px;text-align: right;list-style-type: none;font-family: Arial;font-size: 13px;line-height: 1.3;font-weight: bold;">
                                <div style="font-size:16px">
                                   Shipping Confirmation
                                </div>
                                <div>
                                   Order #<asp:Literal ID="ltLinkTracking" runat="server"></asp:Literal>
                                </div>
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <CC:tracking ID="tracking" runat="server"></CC:tracking>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
