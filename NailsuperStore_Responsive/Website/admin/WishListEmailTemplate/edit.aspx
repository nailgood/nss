<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="edit.aspx.vb" Inherits="admin_WishListEmailTemplate_edit" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
	<h4>WishList Email templates Edit Template</h4>
	
	<table cellSpacing="2" cellPadding="3" border="0">
		<tr>
			<td colSpan="2"><span class="red">red color</span> - required fields</td>
		</tr>
		<tr>
			<td class="optional"><b>Email Subject:</b></td>
			<td class="field" width=300><asp:textbox id="txtSubject" runat="server" maxlength="50" columns="50" /></td>
			<td></td>
		</tr>
		<tr>
			<td class="required"><b>Email Purpose:</b></td>
			<td class="field"><asp:Label id="lblPurpose" runat="server" /></td>
			<td></td>					
		</tr>
		<tr>
			<td class="optional"><b>Email Body Text:</b></td>
			<td class="field"><asp:textbox ID="txtEmailBodyText" Runat="server" columns="50" TextMode="MultiLine" /></td>
			<td>&nbsp;</td>
		</tr>
	</table>
	<p>
		<asp:button id="Save" runat="server" Text="Save" cssClass="btn" />
		<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False" /></p>
</asp:Content>

