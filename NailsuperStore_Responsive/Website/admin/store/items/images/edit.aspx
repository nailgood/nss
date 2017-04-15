<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_items_images_Edit"  Title="Item Image"%>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ImageId = 0 Then%>Add<% Else %>Edit<% End If %> Alternate Image</h4>

<p>
Images for <b><% =dbStoreItem.ItemName%></b> | <a href="/admin/store/items/default.aspx?<%= GetPageParams(Components.FilterFieldType.All,"F_ItemId") %>">&laquo; Go Back To Store Item List</a>
</p>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Image:</td>
		<td class="field"><CC:FileUpload Required="true" ID="fuImage" Folder="/assets/items/original/" ImageDisplayFolder="/assets/items/" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td>
		<CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
        <CC:RequiredFileUploadValidator ControlToValidate="fuImage" ID="rvImage" CssClass="msgError" runat="server" ErrorMessage="File name is blank"></CC:RequiredFileUploadValidator>
		</td>
	</tr>
	<tr>
		<td class="optional2">Image Alt Tag:</td>
		<td class="field"><asp:textbox id="txtImageAltTag" runat="server" maxlength="100" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Item Image?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>