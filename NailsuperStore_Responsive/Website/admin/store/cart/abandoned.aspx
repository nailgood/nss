<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Shopping Cart Abandonment Metrics" CodeFile="abandoned.aspx.vb" Inherits="admin_store_cart_abandoned"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Shopping Cart Abandonment Metrics<br /><span style="font-size:.75em;"><%=dStart.ToShortDateString%> - <%=dEnd.ToShortDateString%> <%if not Username = Nothing then%> - Username: <%=Username%><%end if%></span></h4>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" EmptyDataText="No orders were found for your search criteria" runat="server" ShowFooter="true" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="False" BorderWidth="0">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<FooterStyle CssClass="optional" />
	<Columns>
		<asp:TemplateField HeaderText="Last Checkout Page" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right">
			<ItemTemplate><%#IIf(CBool(Container.DataItem("Completed")), "Completed Orders (" & IIf(CBool(Container.DataItem("IsDifferentSession")), "Different Session", "Same Session") & ")", IIf(IsDBNull(Container.DataItem("CheckoutPage")), "(Did Not Start Checkout)", Container.DataItem("CheckoutPage") & " Page"))%></ItemTemplate>
			<FooterTemplate>Total Incomplete Orders:<br />Total Complete Orders:<br />Total Orders:<br />Completion Percentage:</FooterTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Total Orders" ItemStyle-HorizontalAlign="left">
			<ItemTemplate><%#Container.DataItem("iCount")%></ItemTemplate>
			<FooterTemplate><asp:Literal runat="server" id="ltlOrdersTotal" /></FooterTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Average Order" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right">
			<ItemTemplate><%#FormatCurrency(Container.DataItem("average"))%></ItemTemplate>
			<FooterTemplate>Average Incomplete Order: <asp:Literal runat="server" id="ltlAverageIncomplete" /><br />Average Complete Order: <asp:Literal runat="server" id="ltlAverageComplete" /><br />Average Order: <asp:Literal runat="server" id="ltlAverageTotal" /><br /></FooterTemplate>
		</asp:TemplateField>
	</Columns>
</CC:GridView>

<p></p>
<a href="default.aspx">&laquo; Back</a>

</asp:content>

