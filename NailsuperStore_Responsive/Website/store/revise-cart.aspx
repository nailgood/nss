<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/checkout.master" AutoEventWireup="false" CodeFile="revise-cart.aspx.vb" Inherits="store_revise_cart" %>
<%@ Register Src="~/controls/checkout/cart.ascx" TagName="cart" TagPrefix="uc2" %>
<%@ Register Src="~/controls/checkout/cartScript.ascx" TagName="cartScript" TagPrefix="uc3" %>
<%@ Register src="~/controls/checkout/cart-summary.ascx" tagname="cart" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="revise-title">Revise Order</div>
   <%-- <uc1:free ID="ucFreeGift" runat="server" />--%>
    <!--cart item-->
    <div id="divListCart">
        <uc2:cart ID="uCart" runat="server" />
    </div>
    <uc3:cartScript ID="cartScript1" runat="server" />
  
</asp:Content>
