<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Americaneagle.com" CodeFile="edit.aspx.vb" Inherits="admin_admins_edit" %>
<%@ Import namespace="Controls" %>

<asp:content ID="Content" runat="server" contentplaceholderid="ph">

<h4>Admin Users - Add / Edit Users</h4>

<table cellspacing="2" cellpadding="3" border="0">
<tr>
    <td colSpan="2"><span class="red">red color</span> - required fields</td>
</tr><tr>
	<td class="required"><b>First Name:</b></td>
	<td class="field" width="300"><asp:textbox id="FIRSTNAME" runat="server" maxlength="50" columns="50"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="msgError" ErrorMessage="First Name is blank" ControlToValidate="FIRSTNAME" Display="Dynamic"></asp:requiredfieldvalidator></td>
</tr><tr>
	<td class="required"><b>Last Name:</b></td>
	<td class="field"><asp:textbox id="LASTNAME" runat="server" maxlength="50" columns="50"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" CssClass="msgError" ErrorMessage="Last Name is blank" ControlToValidate="LASTNAME" Display="Dynamic"></asp:requiredfieldvalidator></td>
</tr><tr>
	<td class="required"><b>E-mail:</b></td>
	<td class="field"><asp:textbox id="EMAIL" runat="server" maxlength="100" columns="50"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="msgError" ErrorMessage="E-mail is blank" ControlToValidate="EMAIL" Display="Dynamic"></asp:requiredfieldvalidator><br />
		<CC:EmailValidator id="emailvalidatoremail" runat="server" CssClass="msgError" ErrorMessage="E-mail is not valid" ControlToValidate="EMAIL" Display="Dynamic"></CC:EmailValidator>
	</td>
</tr><tr>
	<td class="required"><b>User Name:</b></td>
	<td class="field"><asp:textbox id="Username" runat="server" maxlength="15" columns="15"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="msgError" ErrorMessage="Username is required" ControlToValidate="Username" Display="Dynamic"></asp:requiredfieldvalidator>
		<CC:UserNameValidator id="usernamevalidator1" runat="server" CssClass="msgError" ErrorMessage="Username is not valid" ControlToValidate="Username" Display="Dynamic"></CC:UserNameValidator>
    </td>
</tr><tr>
	<td class="required" height="36" valign="top"><b>Groups</b></td>
	<td class="field">
		<table>
			<tr>
				<td width="120"><STRONG>Doesn't belong to:</STRONG></td>
				<td width="44">&nbsp;</td>
				<td><STRONG>Belongs to:</STRONG></td>
			</tr>
			<tr>
				<td width="120"><asp:ListBox id="lbLeft" Rows="10" Runat="server" SelectionMode="Multiple"></asp:ListBox></td>
				<td align="center" width="44">
					<asp:Button Runat="server" cssClass="btn" id="btnRight" Text="==>" CausesValidation="False" /><br>
					<asp:Button Runat="server" cssClass="btn" id="btnLeft" Text="<==" CausesValidation="False" />
				</td>
				<td>
				    <asp:ListBox id="lbRight" Rows="10" Runat="server" SelectionMode="Multiple"></asp:ListBox>
				</td>
			</tr>
		</table>
	</td>
</tr>
<tr id="trPassword" runat="server">
    <td colspan="2" runat="server" width=450>
    If you want to change the password for the user, please enter data below, otherwise please leave password fields blank.
    </td>
</tr><tr id="trPassword1" runat="server">
	<td class="required" height="36"><b>Password:</b></td>
	<td class="field" height="36"><asp:textbox id="PASSWORD1" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
	<td height="36">
	    <asp:requiredfieldvalidator id="PASSWORDVALIDATOR1" runat="server" CssClass="msgError" ErrorMessage="Password is required" ControlToValidate="PASSWORD1" Display="Dynamic"></asp:requiredfieldvalidator>
		<asp:RegularExpressionValidator id="PasswordLenghtValidator" Runat="server" ControlToValidate="PASSWORD1" Display="Dynamic" ValidationExpression="[A-Za-z0-9]{6,}" CssClass="msgError" ErrorMessage="Password must contain minimum 6 alphanumeric characters"></asp:RegularExpressionValidator>
	</td>
</tr><tr id="trPassword2" runat="server">
	<td class="required"><b>Re-Type Password:</b></td>
	<td class="field"><asp:textbox id="PASSWORD2" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="PASSWORDVALIDATOR2" runat="server" CssClass="msgError" ErrorMessage="Password confirmation is required" ControlToValidate="PASSWORD2" Display="Dynamic"></asp:requiredfieldvalidator>
			<asp:CompareValidator ID="PASSWORDCOMPAREVALIDATOR" Runat="server" ControlToCompare="PASSWORD1" ControlToValidate="PASSWORD2" Operator="Equal" Display="Dynamic" CssClass="msgError" ErrorMessage="Password and Re-typed passwod don't match" />
	</td>
</tr>
<tr>
            <td class="optional">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
            </td>
            <td></td>
        </tr>

</table>

<p></p>

<CC:OneClickButton id="Save" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this user?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>

</asp:content>