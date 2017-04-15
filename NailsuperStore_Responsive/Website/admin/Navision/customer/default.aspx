<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Customer" CodeFile="default.aspx.vb" Inherits="admin_navision_customer_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Customer Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Customer No:</th>
<td valign="top" class="field"><asp:textbox id="F_CustomerNo" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Name:</th>
<td valign="top" class="field"><asp:textbox id="F_Name" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Name 2:</th>
<td valign="top" class="field"><asp:textbox id="F_Name2" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">City:</th>
<td valign="top" class="field"><asp:textbox id="F_City" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Zipcode:</th>
<td valign="top" class="field"><asp:textbox id="F_Zipcode" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">County:</th>
<td valign="top" class="field"><asp:textbox id="F_County" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Email:</th>
<td valign="top" class="field"><asp:textbox id="F_Email" runat="server" Columns="50" MaxLength="80"></asp:textbox></td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Customer" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?CustomerId=" & DataBinder.Eval(Container.DataItem, "CustomerId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Customer?" runat="server" NavigateUrl= '<%# "delete.aspx?CustomerId=" & DataBinder.Eval(Container.DataItem, "CustomerId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="CustomerNo" DataField="CustomerNo" HeaderText="Customer No"></asp:BoundField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
		<asp:BoundField SortExpression="Name2" DataField="Name2" HeaderText="Name 2"></asp:BoundField>
		<asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>
		<asp:BoundField DataField="Zipcode" HeaderText="Zipcode"></asp:BoundField>
		<asp:BoundField DataField="County" HeaderText="County"></asp:BoundField>
		<asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
	</Columns>
</CC:GridView>

</asp:content>

