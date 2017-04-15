<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_postingGroup_Edit"  Title="Customer Posting Group"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If Id = 0 Then %>Add<% Else %>Edit<% End If %> Customer Posting Group</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Code:</td>
		<td class="field"><asp:textbox id="txtCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCode" runat="server" Display="Dynamic" ControlToValidate="txtCode" ErrorMessage="Field 'Code' is blank"></asp:RequiredFieldValidator></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Customer Posting Group?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
