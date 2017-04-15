<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="edit.aspx.vb" Inherits="admin_store_shippingint_edit"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<h4>Store State Tax Rate - Add/Edit Rate</h4>
	<table cellSpacing="2" cellPadding="3" border="0">
		<tr>
			<td colSpan="2"><span class="red">red color</span> - required fields</td>
		</tr>
		<tr>
			<td class="required"><b>Name:</b></td>
			<td class="field" width=300><asp:Label id="lblStateName" Runat="server" /></td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="required"><b>Code:</b></td>
			<td class="field" width=300><asp:Label id="lblStateCode" Runat="server" /></td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="optional"><b>Rate:</b></td>
			<td class="field" width=300><asp:textbox id="txtRATE" runat="server" maxlength="10" columns="10"></asp:textbox>%</td>
			<td><CC:FloatValidator id="fvValidator" CssClass="msgError" runat="server" ErrorMessage="Valid tax rate value required" ControlToValidate="txtRATE" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="optional"><b>Include Delivery?:</b></td>
			<td class="field" width=300><asp:CheckBox ID="chkIncludeDelivery" Runat="server" /> Yes</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="optional"><b>Include Gift Wrap?:</b></td>
			<td class="field" width=300><asp:CheckBox ID="chkIncludeGiftWrap" Runat="server" /> Yes</td>
			<td>&nbsp;</td>
		</tr>
	</table>
	<p>
		<asp:button id="Save" runat="server" Text="Save" cssClass="btn"></asp:button>
		<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this swatch?" Text="Delete"
			cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
		<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>
</asp:content>