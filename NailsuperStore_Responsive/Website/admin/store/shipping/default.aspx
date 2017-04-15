<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Shipping Range" CodeFile="default.aspx.vb" Inherits="admin_store_shipping_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Shipping Range</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Shipment Method:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_MethodId" runat="server" /></td>
</tr>
<tr>
<th valign="top"><b>Zipcode:</b></th>
<td valign="top" class="field"><asp:TextBox ID="F_Zipcode" runat="server" MaxLength="5" /></td>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Shipping Range" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?ShippingRangeId=" & DataBinder.Eval(Container.DataItem, "ShippingRangeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Shipping Range?" runat="server" NavigateUrl= '<%# "delete.aspx?ShippingRangeId=" & DataBinder.Eval(Container.DataItem, "ShippingRangeId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Shipment Method"></asp:BoundField>
		<asp:BoundField SortExpression="LowValue" DataField="LowValue" HeaderText="Lower Zipcode" ItemStyle-HorizontalAlign="right"></asp:BoundField>
		<asp:TemplateField><HeaderTemplate>&nbsp;</HeaderTemplate><ItemTemplate>-</ItemTemplate></asp:TemplateField>
		<asp:BoundField SortExpression="HighValue" DataField="HighValue" HeaderText="Upper Zipcode"></asp:BoundField>
		<asp:BoundField SortExpression="OverUnderValue" DataField="OverUnderValue" HeaderText="Order Threshold ($)"></asp:BoundField>
		<asp:BoundField SortExpression="FirstPoundOver" DataField="FirstPoundOver" HeaderText="First Over"></asp:BoundField>
		<asp:BoundField SortExpression="FirstPoundUnder" DataField="FirstPoundUnder" HeaderText="First Under"></asp:BoundField>
		<asp:BoundField SortExpression="AdditionalPound" DataField="AdditionalPound" HeaderText="Additional Pound"></asp:BoundField>
		<asp:BoundField SortExpression="AdditionalThreshold" DataField="AdditionalThreshold" HeaderText="Additional Threshold"></asp:BoundField>
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
</asp:content>

