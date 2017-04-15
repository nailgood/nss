<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_contacts_Edit"  Title="Customer Contact"%>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ContactId = 0 Then %>Add<% Else %>Edit<% End If %> Customer Contact</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Contact No:</td>
		<td class="field"><asp:textbox id="txtContactNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvContactNo" runat="server" Display="Dynamic" ControlToValidate="txtContactNo" CssClass="msgError" ErrorMessage="Field 'Contact No' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Contact Name:</td>
		<td class="field"><asp:textbox id="txtContactName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvContactName" runat="server" Display="Dynamic" ControlToValidate="txtContactName" CssClass="msgError" ErrorMessage="Field 'Contact Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Contact Name 2:</td>
		<td class="field"><asp:textbox id="txtContactName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Address:</td>
		<td class="field"><asp:textbox id="txtContactAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Address 2:</td>
		<td class="field"><asp:textbox id="txtContactAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact City:</td>
		<td class="field"><asp:textbox id="txtContactCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Zipcode:</td>
		<td class="field"><asp:textbox id="txtContactZipcode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact County:</td>
		<td class="field"><asp:textbox id="txtContactCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Phone:</td>
		<td class="field"><asp:textbox id="txtContactPhone" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Email:</td>
		<td class="field"><asp:textbox id="txtContactEmail" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Website:</td>
		<td class="field"><asp:textbox id="txtContactWebsite" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Last Export:</td>
		<td class="field"><asp:Label ID="dtLastExport" runat="server" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Login:</td>
		<td class="field"><asp:textbox id="txtLogin" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr id="trPassword" runat="server">
		<td id="Td1" colspan="2" runat="server" width=450>
		If you want to change the password for the user, please enter data below, otherwise please leave password fields blank.
		</td>
	</tr>
	<tr>
		<td class="required" height="36"><b>Password:</b></td>
		<td class="field" height="36"><asp:textbox id="PASSWORD1" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
		<td height="36">
			<asp:requiredfieldvalidator id="PASSWORDVALIDATOR1" runat="server" CssClass="msgError" ErrorMessage="Password is required" ControlToValidate="PASSWORD1" Display="Dynamic"></asp:requiredfieldvalidator>
			<asp:RegularExpressionValidator id="PasswordLenghtValidator" Runat="server" ControlToValidate="PASSWORD1" Display="Dynamic" ValidationExpression="[A-Za-z0-9]{6,}" CssClass="msgError" ErrorMessage="Password must contain minimum 6 alphanumeric characters"></asp:RegularExpressionValidator>
		</td>
	</tr>
	<tr>
		<td class="required"><b>Re-Type Password:</b></td>
		<td class="field"><asp:textbox id="PASSWORD2" runat="server" maxlength="15" columns="15" TextMode="Password"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="PASSWORDVALIDATOR2" runat="server" CssClass="msgError" ErrorMessage="Password confirmation is required" ControlToValidate="PASSWORD2" Display="Dynamic"></asp:requiredfieldvalidator>
				<asp:CompareValidator ID="PASSWORDCOMPAREVALIDATOR" Runat="server" ControlToCompare="PASSWORD1" ControlToValidate="PASSWORD2" Operator="Equal" Display="Dynamic" CssClass="msgError" ErrorMessage="Password and Re-typed passwod don't match" />
		</td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Customer Contact?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

