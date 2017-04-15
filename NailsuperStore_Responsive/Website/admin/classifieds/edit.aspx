<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_classifieds_Edit"  Title="Classified"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ClassifiedId = 0 Then %>Add<% Else %>Edit<% End If %> Classified</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Classified Category:</td>
		<td class="field"><asp:DropDownList id="drpClassifiedCategoryId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvClassifiedCategoryId" runat="server" Display="Dynamic" ControlToValidate="drpClassifiedCategoryId" CssClass="msgError" ErrorMessage="Field 'Classified Category' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Title:</td>
		<td class="field"><asp:textbox id="txtTitle" runat="server" maxlength="200" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Description:</td>
		<td class="field"><asp:TextBox id="txtDescription" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="Dynamic" ControlToValidate="txtDescription" CssClass="msgError" ErrorMessage="Field 'Description' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	
	<tr>
		<td class="optional">Photo 1:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto0" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto0" runat="server" Display="Dynamic" ControlToValidate="fuPhoto0" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Photo 2:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto1" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto1" runat="server" Display="Dynamic" ControlToValidate="fuPhoto1" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Photo 3:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto2" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto2" runat="server" Display="Dynamic" ControlToValidate="fuPhoto2" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Photo 4:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto3" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto3" runat="server" Display="Dynamic" ControlToValidate="fuPhoto3" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Photo 5:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto4" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto4" runat="server" Display="Dynamic" ControlToValidate="fuPhoto4" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Photo 6:<br /> <span class="smaller">300 x 225</span></td>
		<td class="field"><CC:FileUpload ID="fuPhoto5" Folder="/assets/classified" ImageDisplayFolder="/assets/classified" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="fePhoto5" runat="server" Display="Dynamic" ControlToValidate="fuPhoto5" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Contact Name:</td>
		<td class="field"><asp:textbox id="txtContactName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Contact Number:</td>
		<td class="field"><asp:textbox id="txtContactNumber" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Email:</td>
		<td class="field"><asp:textbox id="txtEmail" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Field 'Email' is blank"></asp:RequiredFieldValidator><CC:EmailValidator Display="Dynamic" runat="server" id="fvEmail" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Field 'Email' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
	<tr>
		<td class="required">Expiration Date:</td>
		<td class="field"><CC:DatePicker ID="dtExpirationDate" runat="server"></CC:DatePicker></td>
		<td><CC:RequiredDateValidator Display="Dynamic" runat="server" id="rdtvExpirationDate" ControlToValidate="dtExpirationDate" CssClass="msgError" ErrorMessage="Date Field 'Expiration Date' is blank" /><CC:DateValidator Display="Dynamic" runat="server" id="dtvExpirationDate" ControlToValidate="dtExpirationDate" CssClass="msgError" ErrorMessage="Date Field 'Expiration Date' is invalid" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Classified?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

