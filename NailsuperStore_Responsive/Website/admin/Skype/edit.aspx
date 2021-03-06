<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_skype_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If SkypeId = 0 Then%>Add<% Else %>Edit<% End If %> Contact Skype</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field"><asp:textbox id="txtName" runat="server" maxlength="30" columns="30" style="width: 319px;" ></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
	<tr>
		<td class="required">Skype:</td>
		<td class="field"><asp:textbox id="txtSkype" runat="server" maxlength="50" columns="50" style="width: 319px;" ></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvSkype" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtSkype" ErrorMessage="Field 'Skype' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
	
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Skype?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

