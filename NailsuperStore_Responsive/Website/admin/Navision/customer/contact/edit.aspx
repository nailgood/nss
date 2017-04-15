<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_customer_contact_Edit"  Title="Customer Contacts"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ContactId = 0 Then %>Add<% Else %>Edit<% End If %> Contact</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Contact No:</td>
		<td class="field"><asp:textbox id="txtContactNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvContactNo" runat="server" Display="Dynamic" ControlToValidate="txtContactNo" CssClass="msgError" ErrorMessage="Field 'Contact No' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Name:</td>
		<td class="field"><asp:textbox id="txtContactName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Name 2:</td>
		<td class="field"><asp:textbox id="txtContactName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Address:</td>
		<td class="field"><asp:textbox id="txtContactAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Address 2:</td>
		<td class="field"><asp:textbox id="txtContactAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">City:</td>
		<td class="field"><asp:textbox id="txtContactCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Zipcode:</td>
		<td class="field"><asp:textbox id="txtContactZipcode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">County:</td>
		<td class="field"><asp:textbox id="txtContactCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Country:</td>
		<td class="field"><asp:textbox id="txtContactCountry" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Phone:</td>
		<td class="field"><asp:textbox id="txtContactPhone" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Email:</td>
		<td class="field"><asp:textbox id="txtContactEmail" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Website:</td>
		<td class="field"><asp:textbox id="txtContactWebsite" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sales Tax Exemption Number:</td>
		<td class="field"><asp:textbox id="txtSalesTaxExemptionNumber" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Login:</td>
		<td class="field"><asp:textbox id="txtLogin" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Password:</td>
		<td class="field"><asp:textbox id="txtPassword" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Last Export:</td>
		<td class="field"><CC:DatePicker ID="dtLastExport" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvLastExport" ControlToValidate="dtLastExport" CssClass="msgError" ErrorMessage="Date Field 'Last Export' is invalid" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

