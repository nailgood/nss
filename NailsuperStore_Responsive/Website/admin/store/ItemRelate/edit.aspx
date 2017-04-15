<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="edit.aspx.vb" Inherits="admin_store_itemrelate_edit" ValidateRequest="false" %>
<%@ Register TagPrefix="Ctrl" TagName="ImageDropDown" Src="~/controls/ImageDropDown.ascx" %>
<%@ Register TagPrefix="Ctrl" TagName="DepartmentTree" Src="~/controls/DepartmentTree.ascx" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

	<h4>Item Relation- Add / Edit Item Relation</h4>
	<table cellSpacing="1" cellPadding="3" border="0">
		<TBODY>
			<tr>
				<td colSpan="2" width="595"><span class="red">red color</span>
					- required fields</td>
			</tr>
			<tr>
				<td class="required"><b>Item :</b></td>
				<td class="field" width="595"><asp:DropDownList ID="drItem" runat="server" /></td>
				
			
			</tr>
			<tr>
				<td class="required"><b>Item Relatetion:</b></td>
				<td class="field" width="595">&nbsp;<asp:DropDownList ID="drItemRelate" runat="server" /></td>
				
			</tr>
			<tr>
				<td class="required"><b>Desciption:</b></td>
				<td class="field" width="595">&nbsp;<asp:TextBox runat="server" ID="txtDesc" 
                        Columns="50" Rows="20" Height="79px"></asp:TextBox> </td>
				
			</tr>
			
			<tr>
				<td class="optional"><b>Is Active?</b></td>
				<td class="field" width="595">
					<asp:CheckBox ID="IsActive" Text="Yes" Runat="server"></asp:CheckBox></td>
			</tr>
		</TBODY>
	</table>
	
<p>


	<p>
		<asp:button id="Save" runat="server" Text="Save" cssClass="btn"></asp:button>
		<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this item?" Text="Delete"
			cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
		<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>

<script language="javascript">
<!--
//updateItemType(document.getElementById('drpItemType'));
//-->
</script>
</asp:content>