<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_splashpage_Edit"  Title="Splash Page" ValidateRequest="false" %>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If SplashPageImageId = 0 Then %>Add<% Else %>Edit<% End If %> Splash Page Image</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field"><asp:textbox id="txtImageName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvImageName" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtImageName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Image File:<br /><span class="smaller">500 x 390</span></td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="/assets/splashpage" ImageDisplayFolder="/assets/splashpage" DisplayImage="False" runat="server" style="width: 319px;" /><div runat="server" id="divImg">
			<b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
			<div><asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
		</div></td>
		<td><CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Image Map:</td>
		<td class="field"><asp:TextBox id="txtImageMap" style="width: 319px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Is Active?</td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
		<td></td>
	</tr>
</table>

<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Splash Page?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
