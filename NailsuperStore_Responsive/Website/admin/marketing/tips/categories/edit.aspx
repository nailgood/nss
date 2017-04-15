<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_tips_categories_Edit"  Title="Tip Category"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If TipCategoryId = 0 Then %>Add<% Else %>Edit<% End If %> Tip Category</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Tip Category:</td>
		<td class="field"><asp:textbox id="txtTipCategory" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvTipCategory" runat="server" Display="Dynamic" ControlToValidate="txtTipCategory" CssClass="msgError" ErrorMessage="Field 'Tip Category' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Description:</td>
		<td class="field"><asp:TextBox id="txtDescription" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="Dynamic" ControlToValidate="txtDescription" CssClass="msgError" ErrorMessage="Field 'Description' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Viet Category:</td>
		<td class="field"><asp:textbox id="txtVietCategory" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
	</tr>
	<tr>
		<td class="optional">Viet Description:</td>
		<td class="field"><asp:TextBox id="txtVietDescription" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
	</tr>
	<tr runat="server" visible="false">
		<td class="optional">Meta Description:</td>
		<td class="field"><asp:TextBox runat="server" ID="txtMetaDescription" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
	</tr>
	<tr runat="server" visible="false">
		<td class="optional">Meta Keywords:</td>
		<td class="field"><asp:TextBox runat="server" ID="txtMetaKeywords" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Tip Category?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

