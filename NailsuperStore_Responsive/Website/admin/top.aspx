<%@ Page Language="VB" AutoEventWireup="false" CodeFile="top.aspx.vb" Inherits="admin_TopPage" %>
<%@ Import Namespace="DataLayer" %>
<html>
<head>
    <link href="#" rel="stylesheet" type="text/css" />
    <link href="/includes/theme-admin/css/menu_top.css" rel="stylesheet" type="text/css">
</head>

<body>

<table cellpadding="0" cellspacing="0" border="0" width="100%" bgcolor="#E2EBF7">
    <tr>
        <td class="login" width="50%"><%=SysParam.GetValue("SiteName")%> Content Management System | <a href="/admin/guide.aspx" target="main">Help Center</a></td>
        <td align="right" width="50%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="right" bgcolor="#E2EBF7">
                        <table cellpadding="0" cellspacing="0" border="0" width="700px">
                            <tr>
                                <td class="welcome">Welcome <asp:Literal ID="ltlFullName" runat="server" /></td>
                                <td><asp:Literal ID="ltlChangePassword" runat="server" /></td>
                                <td><asp:Literal ID="ltlSystemParameters" runat="server" /></td>
                                <td><asp:Literal ID="ltlLog" runat="server" /></td>
                                <td><asp:Literal ID="ltlLogout" runat="server" /></td>
                            </tr>
                        </table>
                        <img src="/includes/theme-admin/images/spacer.gif" width="15" height="1">
                    </td>
                </tr>
            </table>

        </td>
    </tr>
</table>

</body>
</html>
