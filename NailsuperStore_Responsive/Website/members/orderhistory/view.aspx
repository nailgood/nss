<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_orderhistory_view"
    MasterPageFile="~/includes/masterpage/checkout.master" CodeFile="view.aspx.vb" %>

<%@ Register Src="../../controls/checkout/cart-summary.ascx" TagName="cart" TagPrefix="uc1" %>
<%@ Register Src="../../controls/checkout/payment-infor.ascx" TagName="payment" TagPrefix="uc2" %>
<%@ Register Src="../../controls/product/order-detail.ascx" TagName="order" TagPrefix="uc3" %>
<%@ Register Src="../../controls/layout/bread-crumb.ascx" TagName="bread" TagPrefix="uc4" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
  <%--  <uc4:bread ID="bread1" runat="server" />--%>
    <div id="orderconfirm">
        <div class="title" id="divDetail" runat="server">
            Order Details <span class="back"><a href="/members/orderhistory/">Back</a></span> </div>
        <ul class="order-status" id="ulStatus" runat="server">
            <li id="liStatus" runat="server"><span class="label">Order Status:</span><span id="spStatus"
                runat="server"></span></li>
            <li id="liTrackingNo" runat="server"><span class="label">Tracking No:</span><span
                id="spTrackingNo" runat="server"></span></li>
        </ul>
        <uc3:order ID="ucOrderDetail" runat="server" />
    </div>
    <script type="text/javascript">
        $(window).load(function () {




        });
    </script>
</asp:Content>
