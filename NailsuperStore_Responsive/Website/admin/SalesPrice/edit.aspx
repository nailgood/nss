<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="Edit"  Title="Homepage" ValidateRequest="false" %>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If SalesPriceId = 0 Then%>Add<% Else %>Edit<% End If %> SalesPrice Image
    </h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">SKU:</td>
		<td class="field">
            <asp:textbox id="txtSKU" runat="server" maxlength="50" 
                columns="50" style="width: 319px;" Enabled="False"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvSKU" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtSKU" ErrorMessage="Field 'SKU' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Image File:<br /><span class="smaller">475 x 205</span></td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="/assets/SalesPrice" ImageDisplayFolder="/assets/SalesPrice" DisplayImage="False" runat="server" style="width: 370px;" /><div runat="server" id="divImg">
			<b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
			<div><asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" Width="475px" Height="205px" /></div>
		</div></td>
		<td><CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
</table>

<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Homepage?" Text="Delete" cssClass="btn" CausesValidation="False" Visible="false"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

