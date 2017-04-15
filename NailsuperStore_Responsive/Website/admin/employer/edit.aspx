<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_employer_Edit"  Title="Employer"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If EmployerId = 0 Then %>Add<% Else %>Edit<% End If %> Employer</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Employer Name:</td>
		<td class="field"><asp:textbox id="txtEmployerName" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmployerName" runat="server" Display="Dynamic" ControlToValidate="txtEmployerName" CssClass="msgError" ErrorMessage="Field 'Employer Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Image:</td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="/assets/application" ImageDisplayFolder="/assets/application" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="required"><b>Is Featured?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsFeatured" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Employer?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

