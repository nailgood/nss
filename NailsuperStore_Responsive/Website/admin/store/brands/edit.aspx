<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_brands_Edit"  Title="Brands"%>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If BrandId = 0 Then %>Add<% Else %>Edit<% End If %> Brand</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Brand Name:</td>
		<td class="field"><asp:textbox id="txtBrandName" runat="server" maxlength="200" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvBrandName" runat="server" Display="Dynamic" ControlToValidate="txtBrandName" CssClass="msgError" ErrorMessage="Field 'Brand Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Brand Name RewriteURL:</td>
		<td class="field"><asp:textbox id="txtBrandNameUrl" runat="server" maxlength="200" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtBrandName" ErrorMessage="Field 'Brand Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Description:</td>
		<td class="field"><asp:TextBox id="txtDescription" style="width: 319px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Header Image:</td>
		<td class="field"><CC:FileUpload ID="fuHeaderImage" Folder="/assets/storebrand" ImageDisplayFolder="/assets/storebrand" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feHeaderImage"  CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="fuHeaderImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">IsTop?</td>
		<td class="field"><asp:CheckBox id="chkIsTop" runat="server" Text="display as top brands"></asp:CheckBox></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Brand?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

