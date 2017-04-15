<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="edit.aspx.vb" Inherits="admin_store_ordersremove_order_view"  %>
<%@ Register TagName="OrderDetail" TagPrefix="CC" Src="~/controls/product/order-detail.ascx" %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<h4>Store Order Administration - View Order#
		<asp:Literal runat="server" ID="litOrderNo"></asp:Literal>
	</h4>

<CC:OrderDetail runat="server" ID="dtl" />

	<p><asp:Button ID="btnCancel" Runat="server" Text="Return to Order List" CssClass="btn" CausesValidation="False" /> &nbsp;&nbsp; </p>
</asp:content>