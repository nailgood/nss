<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="admin_Login" title="" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
<script language="javascript">
    if (parent.location != document.location) parent.location = document.location;
</script>

<table border="0" align="center">
<tr>
<td colspan=3>
    <asp:Label id="Msg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />
    </td>
<tr>
	<td>Username:</td>
	<td><input id="Username" type="text" runat="server"></td>
	<td><ASP:RequiredFieldValidator ControlToValidate="Username" Display="Static" ErrorMessage="Please enter username" runat="server" id="RequiredFieldValidator1" /></td>
</tr>
<tr>
	<td>Password:</td>
	<td><input id="UserPass" type="password" runat="server"></td>
	<td><ASP:RequiredFieldValidator ControlToValidate="UserPass" Display="Static" ErrorMessage="Please enter password" runat="server" id="RequiredFieldValidator2" /></td>
</tr>
<tr>
	<td>Persistent Cookie:</td>
	<td><ASP:CheckBox id="PersistCookie" runat="server" />
	</td>
</tr>
<tr>
<td colspan=3>
<asp:Button CssClass="btn" text="Sign in" runat="server" id="btnLogin" />
</td>
</tr>
</table>



</asp:Content>

