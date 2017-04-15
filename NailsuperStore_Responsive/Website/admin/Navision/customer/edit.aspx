<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_customer_Edit"  Title="Customer"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If CustomerId = 0 Then %>Add<% Else %>Edit<% End If %> Customer</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Customer No:</td>
		<td class="field"><asp:textbox id="txtCustomerNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCustomerNo" runat="server" Display="Dynamic" ControlToValidate="txtCustomerNo" CssClass="msgError" ErrorMessage="Field 'Customer No' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Name:</td>
		<td class="field"><asp:textbox id="txtName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Name 2:</td>
		<td class="field"><asp:textbox id="txtName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Address:</td>
		<td class="field"><asp:textbox id="txtAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Address 2:</td>
		<td class="field"><asp:textbox id="txtAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">City:</td>
		<td class="field"><asp:textbox id="txtCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Zipcode:</td>
		<td class="field"><asp:textbox id="txtZipcode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">County:</td>
		<td class="field"><asp:textbox id="txtCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Phone:</td>
		<td class="field"><asp:textbox id="txtPhone" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact:</td>
		<td class="field"><asp:textbox id="txtContact" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Email:</td>
		<td class="field"><asp:textbox id="txtEmail" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Website:</td>
		<td class="field"><asp:textbox id="txtWebsite" runat="server" maxlength="80" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sales Tax Exemption Number:</td>
		<td class="field"><asp:textbox id="txtSalesTaxExemptionNumber" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Currency Code:</td>
		<td class="field"><asp:textbox id="txtCurrencyCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Customer Price Group Id:</td>
		<td class="field"><asp:DropDownList id="drpCustomerPriceGroupId" runat="server" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Customer Discount Group:</td>
		<td class="field"><asp:textbox id="txtCustomerDiscountGroup" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Language Code:</td>
		<td class="field"><asp:textbox id="txtLanguageCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Payment Terms Code:</td>
		<td class="field"><asp:textbox id="txtPaymentTermsCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Last Date Modified:</td>
		<td class="field"><CC:DatePicker ID="dtLastDateModified" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvLastDateModified" ControlToValidate="dtLastDateModified" CssClass="msgError" ErrorMessage="Date Field 'Last Date Modified' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Last Import:</td>
		<td class="field"><CC:DatePicker ID="dtLastImport" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvLastImport" ControlToValidate="dtLastImport" CssClass="msgError" ErrorMessage="Date Field 'Last Import' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Last Export:</td>
		<td class="field"><CC:DatePicker ID="dtLastExport" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvLastExport" ControlToValidate="dtLastExport" CssClass="msgError" ErrorMessage="Date Field 'Last Export' is invalid" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Customer?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

