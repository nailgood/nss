<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cart-summary.ascx.vb" Inherits="controls_checkout_cart_summary" %>


<div class="summary" id="secOrderSummary">
    <div class="content">
        <div class="box">
            <table class="tbl-summary <%=CssPadding %>">
                <tr class="header" id="trHeader" runat="server">
                      <td class="left">
                    </td>
                    <td colspan="2">
                       Order Summary
                    </td>
                      <td class="right">
                    </td>
                </tr>
                <tr>
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Merchandise Subtotal
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblMerchSubTotal" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr id="trPromotionalDiscount" runat="server">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        You Saved
                    </td>
                    <td class="label-data save">
                        <asp:Label runat="server" ID="lblPromotionalDiscount" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <asp:Literal runat="server" ID="litShippingDetails" />
                <tr runat="server" id="trResidential" visible="false">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Residential Delivery Surcharge
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblResidentil" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trSalesTax">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Sales Tax
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblSalesTax" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trCouponDiscount" visible="false">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Coupon Discount
                    </td>
                    <td class="label-data save">
                        <asp:Label runat="server" ID="lblCouponDiscount" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trPoint">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Cash Reward Points
                    </td>
                    <td class="label-data save">
                        <asp:Label runat="server" ID="lblPurchasePoint" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trLevelPoint">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        <asp:Label ID="lblMLevelPoint" runat="server"></asp:Label>
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblDiscPoint" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trHazardous">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Hazardous Material Fee <a href="javascript:void(ShowFlammableTip());"><span class="question"></span></a>
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblHazardous" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trHandlingFee">
                    <td class="left">
                    </td>
                    <td class="label-text">
                        Special Handling Fee
                    </td>
                    <td class="label-data">
                        <asp:Label runat="server" ID="lblHandlingFee" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr runat="server" id="trSpendFreeShipping" visible="false">
                    <td class="left"></td>
                    <td colspan="2" class="spendfreeshipping"><asp:Label runat="server" ID="lblSpendFreeShipping" CssClass="SpendFreeShipping" /></td>
                </tr>
                <tr>
                    <td class="left">
                    </td>
                    <td class="label-text subtotal">
                        <asp:Literal ID="ltrTitleTotal" runat="server"></asp:Literal>
                    </td>
                    <td class="label-data subtotal">
                        <asp:Label runat="server" ID="lblSubTotal" />
                    </td>
                    <td class="right">
                    </td>
                </tr>
                <tr id="trCheckOut" runat="server">
                    <td class="left">
                    </td>
                    <td colspan="2" class="checkout">
                        <div class="checkout-button" onclick="CheckOutNow();">
                            <ul class="content-checkout">
                                <li class="icon">
                                    <img src="/includes/theme/images/secure.png" />
                                </li>
                                <li class="text">Secure Checkout Now </li>
                            </ul>
                        </div>
                    </td>
                    <td class="right">
                    </td>
                </tr>
            </table>
        </div>
        <div class="continue" id="divContinueShipping" runat="server">
            <a href="/deals-center">Continue Shopping </a>
        </div>
         <div class="revise" id="divReviseCart" runat="server">
            <a href="/store/revise-cart.aspx">Edit Your Order </a>
        </div>
    </div>
</div>