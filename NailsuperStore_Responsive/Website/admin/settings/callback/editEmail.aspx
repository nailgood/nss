<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editEmail.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_requestcallbacklanguage_editEmail"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If LanguageId = 0 Then%>Add<% Else %>Edit<% End If %> Language</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="optional">Language Name:</td>
		<td class="field"><asp:DropDownList id="drpLanguageId" Enabled ="false" runat="server"  /></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">LanguageCode:</td>
		<td class="field"><asp:textbox id="txtLanguageCode" runat="server" maxlength="30" columns="30" style="width: 319px;" Enabled ="true"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtLanguageCode" ErrorMessage="Field 'LanguageCode' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
	
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Language?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

