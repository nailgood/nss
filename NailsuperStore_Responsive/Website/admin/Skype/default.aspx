<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Email" CodeFile="default.aspx.vb" Inherits="admin_skype_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Contact Skype Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">

<tr>
		<td class="field">Name:</td>
		<td class="field"><asp:textbox id="F_Name" runat="server" maxlength="50" columns="50" style="width: 319px;" ValidationGroup="valSave"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="F_Name" ErrorMessage="Field 'Name' is blank" ValidationGroup="valSave"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="field">Skype:</td>
		<td class="field"><asp:textbox id="F_Skype" runat="server" maxlength="50" columns="50" style="width: 319px;" ValidationGroup="valSave"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvSkype" runat="server" Display="Dynamic" ControlToValidate="F_Skype" ErrorMessage="Field 'Skype' is blank" ValidationGroup="valSave"></asp:RequiredFieldValidator></td>
	</tr>
	
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" Visible ="false" />

</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="Save" Runat="server" Text="Save" cssClass="btn" ValidationGroup="valSave"></CC:OneClickButton>
<CC:OneClickButton id="ClearCache" Runat="server" Text="Clear Cache" cssClass="btn"></CC:OneClickButton>

<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?SkypeId=" & DataBinder.Eval(Container.DataItem, "SkypeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Contact Skype?" runat="server" NavigateUrl= '<%# "delete.aspx?SkypeId=" & DataBinder.Eval(Container.DataItem, "SkypeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>		
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
		<asp:BoundField SortExpression="Skype" DataField="Skype" HeaderText="Skype"></asp:BoundField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=UP&SkypeId=" & DataBinder.Eval(Container.DataItem, "SkypeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/moveup.gif" ID="lnkMoveUp">Move Up</asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=DOWN&SkypeId=" & DataBinder.Eval(Container.DataItem, "SkypeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/movedown.gif" ID="lnkMoveDown">Move Down</asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateField>
		</Columns>
</CC:GridView>

</asp:content>

