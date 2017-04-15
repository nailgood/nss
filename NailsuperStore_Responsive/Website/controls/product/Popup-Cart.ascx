<%@ Control Language="VB" AutoEventWireup="false" CodeFile="popup-cart.ascx.vb" Inherits="controls_PopupCart" %>

                   <%If (cartItemCount > 0) Then%>
                    <div class="c-header">
                        You have <span class="c-cart"><%=cartItemCount%></span> items in your cart
                    </div>
                    <div class="cart-scroll">
                        <asp:Repeater runat="server" ID="rptCartItems">
                                <ItemTemplate>
                                    <div class="cart-wrapper" id="PopCart_<asp:Literal runat='server' ID='litCartItem' />">
                                        <div class="w-img">
                                            <div class="c-img" summary="image"><asp:Literal runat="server" ID="lnkImg" /></div>
                                                <asp:Literal ID="ltrRemove" runat="server"></asp:Literal>
                                        </div>
                                        <div class="desc" style="">
                                            <asp:Literal ID="ltrFree" runat="server"></asp:Literal>
                                            <div class="prod-name"><asp:Literal ID="ltrName" runat="server"></asp:Literal></div>
                                            <div class="sku-qty">Item# <asp:Literal ID="ltrSKU" runat="server"></asp:Literal> &nbsp;|&nbsp; Qty: <asp:Literal ID="ltrQty" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="price-desc"><asp:Literal ID="ltrPrice" runat="server"></asp:Literal></div>
                                     </div>
                                </ItemTemplate>
                         </asp:Repeater>
                    </div>
                    <div class="sumary">
                        <div class="row-sumary">
                            <div class="c-label">Merchandise Subtotal</div>
                            <div class="c-value" id="PopMerchandise"><%=FormatCurrency(merchandiseSubTotal, 2)%>
                            </div>
                        </div>
                        <div class="row-sumary" id="divsave" runat="server">
                            <div class="c-label">You Saved</div>
                            <div class="c-value price2" id="PopYouSave"><asp:Label runat="server" ID="lblPromotionalDiscount" /></div>
                        </div>
                        <div class="row-sumary" id="divDiscount" runat="server">
                            <div class="c-label">Coupon Discount</div>
                            <div class="c-value"><asp:Label runat="server" ID="lblCouponDiscount" /></div>
                        </div>
                        <div class="row-sumary" id="divPoint" runat="server">
                            <div class="c-label">Cash Rewards Points</div> 
                            <div class="c-value"><asp:Label runat="server" ID="lblPurchasePoint" /></div>
                        </div>
                       
                        <div class="row-sumary">
                            <div class="c-label">Order Subtotal</div>
                            <div class="c-value" id="PopOrderSubTotal"><%=FormatCurrency(subTotal)%></div>
                        </div>
                    </div>
                    <div class="pull-right">
                       <div class="c-button" onclick="window.location='/store/cart.aspx'"><span class="ic-check-out pull-left"></span>Secure Checkout Now</div>
                    </div>
                    <ul id="cart-notice">
                        <li>See your samples, gifts and rewards in <a href="/store/cart.aspx">Shopping Cart</a>.</li>
                        <%=textFreeShipping %>
                    </ul>
                    <%Else%>
                    <div class="empty">
                        <div class="text-center">
                            Your shopping cart is empty.</div>
                    </div>
                    <%End If%>