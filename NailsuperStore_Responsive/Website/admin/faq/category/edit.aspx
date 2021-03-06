<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_faq_category_Edit"  Title="Faq Category"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If FaqCategoryId = 0 Then %>Add<% Else %>Edit<% End If %> FAQ Category</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required"><b>Is Active?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblIsActive" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="required">Category Name:</td>
		<td class="field"><asp:textbox id="txtCategoryName" runat="server" maxlength="400" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" Display="Dynamic" ControlToValidate="txtCategoryName" CssClass="msgError" ErrorMessage="Field 'Category Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Admin:</td>
		<td class="field"><asp:DropDownList id="drpAdminId" runat="server" /><div class="smaller">If an administrator is selected, users can send questions.</div></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this FAQ Category?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

