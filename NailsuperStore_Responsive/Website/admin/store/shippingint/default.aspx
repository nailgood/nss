<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="International Shipping Range" CodeFile="default.aspx.vb" Inherits="admin_store_shippingint_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>International Shipping Range Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Method:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_MethodId" runat="server" /></td>
</tr>
<tr>
<th valign="top"><b>Country Code:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_CountryCode" runat="server" /></td>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New International Shipping Ranges" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?ShippingRangeIntId=" & DataBinder.Eval(Container.DataItem, "ShippingRangeIntId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this International Shipping Ranges?" runat="server" NavigateUrl= '<%# "delete.aspx?ShippingRangeIntId=" & DataBinder.Eval(Container.DataItem, "ShippingRangeIntId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="MethodId" DataField="MethodId" HeaderText="Method"></asp:BoundField>
		<asp:BoundField SortExpression="CountryCode" DataField="CountryCode" HeaderText="Country Code"></asp:BoundField>
		<asp:BoundField SortExpression="AdditionalThreshold" DataField="AdditionalThreshold" HeaderText="Additional Threshold"></asp:BoundField>
		<asp:BoundField SortExpression="OverUnderValue" DataField="OverUnderValue" HeaderText="Over Under Value" DataFormatString="{0:c}" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="FirstPoundOver" DataField="FirstPoundOver" HeaderText="First Pound Over" DataFormatString="{0:c}" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="FirstPoundUnder" DataField="FirstPoundUnder" HeaderText="First Pound Under" DataFormatString="{0:c}" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="AdditionalPound" DataField="AdditionalPound" HeaderText="Additional Pound" DataFormatString="{0:c}" HTMLEncode="False"></asp:BoundField>
	</Columns>
</CC:GridView>

</asp:content>

