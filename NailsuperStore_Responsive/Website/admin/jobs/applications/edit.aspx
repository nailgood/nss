<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_jobs_applications_Edit"  Title="Post Job Application"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ApplicationId = 0 Then %>Add<% Else %>Edit<% End If %> Post Job Application</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Company Name:</td>
		<td class="field"><asp:textbox id="txtCompanyName" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" Display="Dynamic" ControlToValidate="txtCompanyName" CssClass="msgError" ErrorMessage="Field 'Company Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Member:</td>
		<td class="field"><asp:Label id="txtMember" runat="server"></asp:Label></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Website:</td>
		<td class="field"><asp:textbox id="txtWebsite" runat="server" maxlength="255" columns="50" style="width: 319px;"></asp:textbox><br/><span class="smaller">http:// or https:// are required</span></td>
		<td><CC:URLValidator Display="Dynamic" runat="server" id="lnkvWebsite" ControlToValidate="txtWebsite" CssClass="msgError" ErrorMessage="Link 'Website' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Email:</td>
		<td class="field"><asp:textbox id="txtEmail" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Field 'Email' is blank"></asp:RequiredFieldValidator><CC:EmailValidator Display="Dynamic" runat="server" id="fvEmail" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Field 'Email' is invalid" /></td>
	</tr>
	<tr>
		<td class="required"><b>Is Approved?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsApproved" /></td>
	</tr>
	<tr>
		<td class="optional">Image:</td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="/assets/application" ImageDisplayFolder="/assets/application" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Post Job Application?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

