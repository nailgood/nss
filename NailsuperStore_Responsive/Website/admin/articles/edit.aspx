<%@ Page ValidateRequest="false" Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_articles_edit"  Title="Articles"%>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ArticleId = 0 Then %>Add<% Else %>Edit<% End If %> Article</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class="red">red color</span> - denotes required fields</td></tr>
	<tr>
		<td class="required">Title:</td>
		<td class="field"><asp:textbox id="txtTitle" runat="server" maxlength="255" columns="50" style="width: 519px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Other Title:</td>
		<td class="field"><asp:textbox id="txtOtherTitle" runat="server" maxlength="255" columns="50" style="width: 519px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Home Page Abstract:</td>
		<td class="field"><asp:TextBox id="txtShortAbstract" style="width: 519px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
	<tr>
		<td class="required">Post Date:</td>
		<td class="field"><CC:DatePicker ID="dtPostDate" runat="server"></CC:DatePicker></td>
		<td>
		<CC:RequiredDateValidator Display="Dynamic" runat="server" id="rdtvPostDate" ControlToValidate="dtPostDate" CssClass="msgError" ErrorMessage="Date Field 'PostDate' is required" />
		<CC:DateValidator Display="Dynamic" runat="server" id="dtvPostDate" ControlToValidate="dtPostDate" CssClass="msgError" ErrorMessage="Date Field 'PostDate' is invalid" />
		</td>
	</tr>
	<tr>
		<td class="optional">Expiration Date:</td>
		<td class="field"><CC:DatePicker ID="dtPastNewsDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvPastNewsDate" ControlToValidate="dtPastNewsDate" CssClass="msgError" ErrorMessage="Date Field 'PastNewsDate' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Image:</td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="/assets/" ImageDisplayFolder="/assets/" runat="server" style="width:300px;" /></td>
		<td ><CC:RequiredFileUploadValidator ControlToValidate="fuImage" ID="rvImage" runat="server" CssClass="msgError" ErrorMessage="Image is blank"></CC:RequiredFileUploadValidator></td>
	</tr>
	<tr>
		<td class="required">Link:</td>
		<td class="field"><asp:textbox id="txtLink" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox><br/><span class="smaller">http:// or https:// are required</span></td>
		<td><asp:RequiredFieldValidator ID="rfvLink" runat="server" Display="Dynamic" ControlToValidate="txtLink" CssClass="msgError" ErrorMessage="Field 'Link' is blank"></asp:RequiredFieldValidator><CC:URLValidator Display="Dynamic" runat="server" id="lnkvLink" ControlToValidate="txtLink" CssClass="msgError" ErrorMessage="Link 'Link' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional"><b>Has Full Version?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkHasFullVersion" /></td></tr>
	<tr>
		<td class="optional">Short Version:</td>
		<td class="field"><asp:TextBox id="txtShortVersion" runat="server" Width="600" Height="300" TextMode="MultiLine" /></td>
	</tr>
	<tr>
		<td class="optional">Full Version:</td>
		<td class="field"><asp:TextBox id="txtFullVersion" runat="server" Width="600" Height="300" TextMode="MultiLine" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Article?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<asp:button id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>

</asp:content>

