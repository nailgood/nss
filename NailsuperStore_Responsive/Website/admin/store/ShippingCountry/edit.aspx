<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_ShippingCountry_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If CountryId = 0 Then%>Add<% Else %>Edit<% End If %> Country Shipping</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="optional">Country:</td>
		<td class="field"><asp:DropDownList id="drpCountryId" runat="server" Enabled="false" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Shipping Code:</td>
		<td class="field"><asp:textbox id="txtShippingCode" runat="server" maxlength="5" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvShippingCode" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtShippingCode" ErrorMessage="Field 'Shipping Code' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
        <td class="optional">
            <b>Is Shipping Active?</b></td>
        <td class="field">
            <asp:CheckBox ID="chkIsActive" runat="server" /></td>
    </tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Us?" Text="Delete" cssClass="btn" CausesValidation="False" Visible ="false"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

