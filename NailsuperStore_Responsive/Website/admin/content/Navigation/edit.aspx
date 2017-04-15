<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_content_navigation_Edit"  Title="Navigation"%>
	
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If NavigationId = 0 Then %>Add<% Else %>Edit<% End If %> Navigation</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class="red">red color</span> - denotes required fields</td></tr>
	<tr>
		<td class="required" style="width:150px;">Title:</td>
		<td class="field" style="width:550px;" ><asp:TextBox id="txtTitle" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:TextBox>
		<asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="required"></asp:RequiredFieldValidator></td>
	</tr>
    <tr>
		<td class="optional">ParentID:</td>
		<td class="field">
		<asp:DropDownList id="drpParentID" runat="server"></asp:DropDownList>
		</td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Target:</td>
		<td class="field">
		<asp:DropDownList id="drpTarget" runat="server">
		<asp:ListItem Value="" Text="Not Set (default)" />
		<asp:ListItem Value="_blank" Text="New Window" />
		<asp:ListItem Value="_top" Text="Topmost Window" />
		<asp:ListItem Value="_self" Text="Self Window" />
		<asp:ListItem Value="_parent" Text="Parent Window" />
		</asp:DropDownList>
		</td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Is Hidden</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsHidden" /></td>
    </tr>
	<tr>
		<td class="optional">URL:</td>
		<td class="field" ><asp:textbox id="txtURL" runat="server" maxlength="255" columns="50" style="width: 319px;"></asp:textbox></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Navigation?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

