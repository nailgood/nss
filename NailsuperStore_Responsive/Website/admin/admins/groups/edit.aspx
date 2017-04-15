<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_admins_groups_edit" title="Add/Edit Admin Group" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">

<h4>Admin Groups - Add / Edit Group</h4>


<table cellSpacing="2" cellPadding="3" border="0">
<tr>
	<td colSpan="2"><span class="red">red color</span> - required fields</td>
</tr>
<tr>
	<td class="required"><b>Group Name:</b></td>
	<td class="field"><asp:textbox id="DESCRIPTION" runat="server" maxlength="50" columns="50" Width="339px"></asp:textbox></td>
	<td><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Group Name is blank" ControlToValidate="DESCRIPTION"
			Display="Dynamic"></asp:requiredfieldvalidator></td>
</tr>
<tr>
	<td class="optional" height="36" valign="top"><b>Privileges</b></td>
	<td class="field">
		<table>
			<tr>
				<td width="142"><STRONG>Doesn't have access to:</STRONG></td>
				<td width="44">&nbsp;</td>
				<td><STRONG>Has Access to:</STRONG></td>
			</tr>
			<tr>
				<td width="142"><asp:ListBox id="lbLeft" Rows="10" Runat="server" SelectionMode="Multiple"></asp:ListBox></td>
				<td align="center" width="44">
					<asp:Button Runat="server" cssClass="btn" id="btnRight" Text="==>" CausesValidation="False" /><br>
					<asp:Button Runat="server" cssClass="btn" id="btnLeft" Text="<==" CausesValidation="False" />
				</td>
				<td><asp:ListBox id="lbRight" Rows="10" Runat="server" SelectionMode="Multiple"></asp:ListBox></td>
			</tr>
		</table>
	</td>
</tr>
</table>

<p>
	<asp:button id="Save" runat="server" Text="Save" cssClass="btn"></asp:button>
	<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this Group?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
	<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>
</p>

</asp:Content>

