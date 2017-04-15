<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirmation_send.aspx.vb" Inherits="store_confirmation_send" %>
<%@ Import Namespace="DataLayer" %>


<%@ Register src="../controls/checkout/mail-order-detail.ascx" tagname="mail" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirmation Email - <%=SysParam.GetValue("CompanyName")%></title>
<style>
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
</style>
</head>
<body style="color: #333;font: 14px/18px Open Sans;">
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" border="0" style="width: 780px; height: 100%; border:solid 1px #dadada">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;
                       background-color:#f3f4f5; height: 93px; padding-bottom: 0;padding-left: 1px;padding-right: 1px;border-top: solid 1px #CCCCCC;border-bottom:solid 1px #CCCCCC;">
                        <tr>
                            <td  style="float: left;padding-left: 10px;padding-top: 5px;text-align: left;width: 300px;">
                                <a href="<%=webRoot %>">
                                    <img alt="Logo" style="border: medium none; text-align: left;" src="<%=webRoot %>/includes/theme/images/logo_send.png"></a>
                            </td>
                            <td style="padding-right: 20px;text-align: right;list-style-type: none;font-family: Open Sans;font-size: 14px;line-height: 1.3;font-weight: 600; color: #59595b;">
                                <div>
                                    3804 Carnation St
                                </div>
                                <div>
                                    Franklin Park, IL 60131, USA
                                </div>
                                <div>
                                    <a style="color:#3b76ba;" href="mailto:customerservice@nss.com">customerservice@nss.com</a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                
                    <uc1:mail ID="od" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>

</html>
