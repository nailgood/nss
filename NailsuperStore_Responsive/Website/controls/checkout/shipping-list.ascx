<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shipping-list.ascx.vb"
    Inherits="controls_checkout_shipping_list" %>
<input type="hidden" id="hidShippingSelect" runat="server" value="" />
<ul class="list-shiping" id="rdoRadShipVia" clientidmode="Static" runat="server">
    <asp:Literal ID="ltrListShippingMethod" runat="server"></asp:Literal>
    <script>
        $(window).load(function () {
            poppover();
        });
    </script>
</ul>

