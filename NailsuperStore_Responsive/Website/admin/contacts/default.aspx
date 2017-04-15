<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Customer Contact" CodeFile="default.aspx.vb" Inherits="admin_contacts_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Customer Contact</h4>

<span class="smaller">Please provide search criteria below</span>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Contact No:</th>
<td valign="top" class="field"><asp:textbox id="F_ContactNo" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Contact Name:</th>
<td valign="top" class="field"><asp:textbox id="F_ContactName" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Contact Name 2:</th>
<td valign="top" class="field"><asp:textbox id="F_ContactName2" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Login:</th>
<td valign="top" class="field"><asp:textbox id="F_Login" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Customer Contact" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Customer Contact?" runat="server" NavigateUrl= '<%# "delete.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="ContactNo" DataField="ContactNo" HeaderText="Contact No"></asp:BoundField>
		<asp:BoundField SortExpression="ContactName" DataField="ContactName" HeaderText="Contact Name"></asp:BoundField>
		<asp:BoundField SortExpression="ContactName2" DataField="ContactName2" HeaderText="Contact Name 2"></asp:BoundField>
		<asp:BoundField DataField="Login" HeaderText="Login"></asp:BoundField>
		<asp:BoundField SortExpression="LastExport" DataField="LastExport" HeaderText="Last Export" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
	</Columns>
</CC:GridView>

</asp:content>
