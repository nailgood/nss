<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ipn.aspx.vb" Inherits="store_ipn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NailSuperStore.com</title>
    <meta name="robots" content="noindex,nofollow" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="vertical-align:top;">
                <asp:TextBox ID="txtRequest" runat="server" TextMode="MultiLine" Rows="40" Width="500px"></asp:TextBox>
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" />    
            </td>
            <td style="margin-left:20px;vertical-align:top;">
                <asp:Literal ID="litMsg" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
