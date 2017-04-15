<%@ Page Title="Test Email" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="testemail.aspx.vb" Inherits="admin_testemail" %>
    
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
<h4>Test Email</h4>

<table cellpadding="2" cellspacing="2">
    <tr>
        <td style="width:150px">From Email: </td>
        <td><asp:TextBox ID="txtFromEmail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>From Name: </td>
        <td><asp:TextBox ID="txtFromName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>To Email: </td>
        <td><asp:TextBox ID="txtToEmail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>To Name: </td>
        <td><asp:TextBox ID="txtToName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>BCC Name: </td>
        <td><asp:TextBox ID="txtBcc" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Subject: </td>
        <td><asp:TextBox ID="txtSubject" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Body: </td>
        <td><asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Username: </td>
        <td><asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Password: </td>
        <td><asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Smtp: </td>
        <td><asp:TextBox ID="txtSmtp" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Port: </td>
        <td><asp:TextBox ID="txtPort" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>SSL: </td>
        <td><asp:DropDownList ID="drpSSL" runat="server">
            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
            <asp:ListItem Text="No" Value="0"></asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send Authentication" />
            <asp:Button ID="btnSend2" runat="server" OnClick="btnSend2_Click" Text="Send Without Authentication " />
            
        </td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Literal ID="litMsg" runat="server"></asp:Literal></td>
    </tr>
</table>

    <div style="display:none">
        <asp:TextBox ID="txtItemList" runat="server"></asp:TextBox>
        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear Cache" /><br />
    </div>
</asp:Content>