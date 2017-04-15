<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_cusioncolor_Edit"  Title="Cusion Color"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% 	If CusionColorId = 0 Then%>Add<% Else %>Edit<% End If %> Cusion Color</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Cusion Color:</td>
		<td class="field"><asp:textbox id="txtCusionColor" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCusionColor" runat="server" Display="Dynamic" ControlToValidate="txtCusionColor" CssClass="msgError" ErrorMessage="Field 'Cusion Color' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Swatch:<br />
		<span class="smaller">40 x 40</span></td>
		<td class="field"><CC:FileUpload ID="fuSwatch" Folder="/assets/CusionColor" ImageDisplayFolder="/assets/CusionColor" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feSwatch" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="fuSwatch" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Cusion Color?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

