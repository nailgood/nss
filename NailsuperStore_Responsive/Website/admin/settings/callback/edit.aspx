<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_requestcallbacklanguage_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If DetailId = 0 Then%>Add<% Else %>Edit<% End If %> Email Language</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="optional">Language:</td>
		<td class="field"><asp:DropDownList id="drpSubjectId" AutoPostBack="true"  runat="server" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Email:</td>
		<td class="field"><asp:DropDownList id="drpEmailId" AutoPostBack="true" runat="server" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field"><asp:textbox id="txtName" runat="server" maxlength="30" columns="30" style="width: 319px;" Enabled ="false"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
	<tr>
		<td class="required">Email Address:</td>
		<td class="field"><asp:textbox id="txtEmailAddress" runat="server" maxlength="50" columns="50" style="width: 319px;" Enabled ="false"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmailAddress" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Us?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

