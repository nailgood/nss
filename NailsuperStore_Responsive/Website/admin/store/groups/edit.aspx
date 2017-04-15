<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_groups_Edit"  Title="Item Group"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ItemGroupId = 0 Then %>Add<% Else %>Edit<% End If %> Item Group</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Group Name:</td>
		<td class="field"><asp:textbox id="txtGroupName" runat="server" maxlength="255" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvGroupName" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtGroupName" ErrorMessage="Field 'Group Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Options:</td>
		<td class="field"><CC:CheckBoxList ID="cblcblOptions" runat="server" RepeatColumns="3"></CC:CheckBoxList><asp:Literal runat="server" ID="litMsg"><div class="red smaller">You cannot change options for a group that currently has items in it</div></asp:Literal></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Item Group?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

