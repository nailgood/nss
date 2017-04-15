<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_itemCategory_Edit"  Title="Navision Item Category"%>
<%@ Register TagPrefix="Ctrl" TagName="DepartmentTree" Src="~/controls/DepartmentTree.ascx" %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If id = 0 Then %>Add<% Else %>Edit<% End If %> Navision Item Category</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Category:</td>
		<td class="field"><asp:Label runat="server" ID="lblCategory" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Name:</td>
		<td class="field"><asp:Label runat="server" ID="lblName" /></td>
		<td></td>
	</tr>
	<tr>
		<td valign="top" class="required"><b>Departments:</b></td>
		<td class="field" width="485">
			Items in this category should be added to these departments during import<br>
			<Ctrl:DepartmentTree id="treeDepartments" runat="server" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

