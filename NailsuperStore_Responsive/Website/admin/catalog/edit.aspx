<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_catalog_edit" title="" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
	<h4>Catalogs - Add / Edit Catalog</h4>
	
	<table cellSpacing="2" cellPadding="3" border="0">
		<tr>
			<td colSpan="2"><span class="red">red color</span> - required fields</td>
		</tr>
		<tr>
			<td class="optional"><b>Title:</b></td>
			<td class="field" width=300><asp:textbox id="txtTitle" runat="server" maxlength="50" columns="50" /></td>
			<td></td>
		</tr>
		<tr>
			<td class="required"><b>Rich FX Link</b></td>
			<td class="field"><asp:textbox id="txtLink" runat="server" maxlength="255" columns="50" /></td>
			<td>
			<asp:RequiredFieldValidator runat="server" id="rqLink" ControlToValidate="txtLink" CssClass="msgError" ErrorMessage="Link to the online catalog is required!" Display="Dynamic" /></td>					
		</tr>
		<tr>
			<td class="required"><b>Image:</b></td>
			<td class="field" width="485">
					<asp:Image Runat="server" ID="img" ImageUrl="/includes/theme-admin/images/spacer.gif" /><br>
					<input id="Image" type="file" runat="server" NAME="Image"><br>
					<font class="smaller"><asp:label ID="lblSizes" Runat="server" /></font>
				</td>
				<td><asp:RequiredFieldValidator runat="server" id="ImageNameReq" ControlToValidate="Image" CssClass="msgError" ErrorMessage="File is blank" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="optional"><b>Image Alt Tag:</b></td>
			<td class="field"><asp:textbox ID="txtImageAltTag" Runat="server" MaxLength="100" columns="50" /></td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="optional"><b>Is Active?:</b></td>
			<td class="field"><asp:CheckBox ID="IsActive" Runat="server" /> Yes</td>
			<td>&nbsp;</td>
		</tr>
	</table>
	<p>
		<asp:button id="Save" runat="server" Text="Save" cssClass="btn" />
		<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this catalog?" Text="Delete" CssClass="btn" CausesValidation="False" />
		<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False" />
</asp:content>