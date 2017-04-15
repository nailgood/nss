<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="password.aspx.vb" Inherits="admin_admins_password" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>Admin Users - Change Password for <%=sUsername%></h4>

<table cellspacing="2" cellpadding="3" border="0">
<tr>
	<td class="required"><b>Password:</b></td>
	<td class="field" ><asp:textbox id="PASSWORD1" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
	<td >
	    <asp:requiredfieldvalidator id="PASSWORDVALIDATOR1" runat="server" ErrorMessage="Password is required" ControlToValidate="PASSWORD1" Display="Dynamic"></asp:requiredfieldvalidator>
		<asp:RegularExpressionValidator id="PasswordLenghtValidator" Runat="server" ControlToValidate="PASSWORD1" Display="Dynamic" errormessage="<br>Password must be between 8 and 32 characters long,<br> Password must contain an uppercase letter,<br> Password must contain a number,<br> Password must contain a symbol (!@#$%^&*)" ValidationExpression="(?=^.{8,32}$)(?=.*\d)(?![.\n])(?=.*[!@#$%^&*)(?=.*[A-Z]).*$"></asp:RegularExpressionValidator>
		
	</td>
</tr><tr>
	<td class="required"><b>Re-Type Password:</b></td>
	<td class="field"><asp:textbox id="PASSWORD2" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="PASSWORDVALIDATOR2" runat="server" ErrorMessage="Password confirmation is required" ControlToValidate="PASSWORD2" Display="Dynamic"></asp:requiredfieldvalidator>
			<asp:CompareValidator ID="PASSWORDCOMPAREVALIDATOR" Runat="server" ControlToCompare="PASSWORD1" ControlToValidate="PASSWORD2" Operator="Equal" Display="Dynamic" ErrorMessage="Password and Re-typed passwod don't match" />
	</td>
</tr>
</table>

<p></p>

<CC:OneClickButton id="Save" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>

</asp:Content>

