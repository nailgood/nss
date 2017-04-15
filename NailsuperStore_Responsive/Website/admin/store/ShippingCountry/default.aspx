<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Us" CodeFile="default.aspx.vb" Inherits="admin_store_ShippingCountry_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Country Shipping</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Country:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_CountryId" runat="server" /></td>
</tr>
<tr>
<th valign="top">Shipping Code:</th>
<td valign="top" class="field"><asp:textbox id="F_ShippingCode" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Is Shipping Active:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsShippingActive" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Contact Us" cssClass="btn" Visible="false"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?CountryId=" & DataBinder.Eval(Container.DataItem, "CountryId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField visible="false">
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Contact Us?" runat="server" NavigateUrl= '<%# "delete.aspx?CountryId=" & DataBinder.Eval(Container.DataItem, "CountryId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="CountryId" DataField="CountryId" HeaderText="Country Id"></asp:BoundField>
		<asp:BoundField SortExpression="CountryName" DataField="CountryName" HeaderText="Country Name"></asp:BoundField>
		<asp:BoundField SortExpression="ShippingCode" DataField="ShippingCode" HeaderText="Shipping Code"></asp:BoundField>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsShippingActive" DataField="IsShippingActive" HeaderText="Is Shipping Active"/>
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
</asp:content>

