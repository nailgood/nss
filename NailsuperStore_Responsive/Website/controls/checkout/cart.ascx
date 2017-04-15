<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cart.ascx.vb" Inherits="controls_checkout_cart" %>

<section id="secCartList" class="cart has-free-gift">

<div id='dvcon' class="continue plull-left">
     <a href="/deals-center">Continue Shopping </a>
</div>
<div id="h-btncart">
    <div class="checkout-button" onclick="CheckOutNow();">
        <ul class="content-checkout">
            <li class="icon">
               <img src="/includes/theme/images/secure.png" />
            </li>
            <li class="text">Secure Checkout Now </li>
        </ul>
     </div>
</div>
<div class="empty alert alert-danger fade in alert-dismissable" id="divEmpty" style="display:none" runat="server">
    There are no items in your shopping cart.
    <br/><a href='<%=DataLayer.ViewedItemRow.getLastUrlByPageType("List") %>'>Click here to continue shopping</a>
</div>

    <table id="tabCart">
        <asp:Repeater runat="server" ID="rptCartItems">
            <HeaderTemplate>
                <tr class="header">
                    <td class="item">
                        Item
                    </td>
                    <td class="name">
                        <div class="hidden-xs">
                            Qty</div>
                    </td>
                    <td class="price hidden-xs">
                        Price
                    </td>
                    <td class="total">
                        Total
                    </td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="row-item" id="trRow" runat="server">
                    <td class="item">
                        <div class="image">
                            <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                        </div>
                        <div class="subsku" id="divSubSKU" runat="server"></div>
                    </td>
                    <td class="name">
                        <div class="name-container1">
                            <div class="name-container2">
                                <div class="des">
                                    <div class="item-name" id="divItemName" runat="server">
                                    </div>
                                    <div class="error" id="divItemError" runat="server" visible="false">
                                    </div>
                                    <div class="sku" id="divSKU" runat="server">
                                        Item#
                                    </div>
                                    <div class="smallprice" id="divSmallPrice" runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="qty" id="divQty" runat="server">
                            <div class="user-input">
                                <asp:Literal ID="ltrArrow" runat="server"></asp:Literal>
                            </div>
                            <asp:Literal ID="ltrArrowUpdate" runat="server"></asp:Literal>
                        </div>
                        <div class="promotion" id="divPromotion" runat="server"></div>
                        <div class="coupon" id="divCoupon" runat="server"></div>
                        <div style="clear:both"></div>
                        <div class="remove">
                            <asp:HyperLink ID="hplRemove" runat="server" Text="Delete"></asp:HyperLink>
                            <asp:HyperLink ID="hplSave" runat="server" Text="Save for later" CssClass="saveforlater"></asp:HyperLink>
                        </div>
                    </td>
                    <td class="price hidden-xs" id="tdPrice" runat="server">
                    </td>
                    <td class="total" id="tdTotal" runat="server">
                    </td>
                </tr>
                <asp:PlaceHolder runat="server" ID="phdFreeItems">
                    <tr class="free-item" id="trFreeItem" runat="server">
                        <td class="left">
                        </td>
                        <td colspan="3" class="right">
                            <table>
                                <tr class="free-header">
                                    <td class="name" colspan="2">
                                        <span class="free-header-text" id="spTitle" runat="server">FREE Items</span>
                                        <asp:Literal ID="ltrLinkChangeFreeItem" runat="server"></asp:Literal>
                                    </td>
                                    <td class="qty hidden-xs">
                                        Qty
                                    </td>
                                    <td class="price hidden-xs">
                                        Price
                                    </td>
                                    <td class="total">
                                        Total
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptFreeItem" runat="server">
                                    <ItemTemplate>
                                        <tr class="free-row-item last" id="trFreeItem" runat="server">
                                            <td class="item">
                                                <div class="image">
                                                    <asp:Literal ID="ltrFreeImage" runat="server"></asp:Literal>
                                                </div>
                                            </td>
                                            <td class="name">
                                                <div class="des">
                                                    <div class="item-name" id="divFreeItemName" runat="server">
                                                    </div>
                                                    <div class="sku" id="divFreeItemSKU" runat="server">
                                                        Item #
                                                    </div>
                                                    <div class="error" id="divFreeError" runat="server" visible="false">
                                                    </div>
                                                    <div class="smallprice" id="divFreeItemSmallPrice" runat="server">
                                                    </div>
                                                    <div class="smallqty" id="divFreeItemSmallQty" runat="server">
                                                    </div>
                                                    <div class="promotion" id="divFreeItemPromotion" runat="server">
                                                    </div>
                                                </div>
                                                <div class="remove">
                                                    <asp:HyperLink ID="hplRemove" runat="server" Text="Delete"></asp:HyperLink>
                                                </div>
                                            </td>
                                            <td class="qty hidden-xs" id="tdFreeItemQty" runat="server">
                                            </td>
                                            <td class="price hidden-xs" id="tdFreeItemPrice" runat="server">
                                            </td>
                                            <td class="total" id="tdFreeItemTotal" runat="server">
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <%--<input type="hidden" value="" id="hidCartItemId" runat="server" />
    <input type="hidden" value="" id="hidHasPriceItem" clientidmode="Static" runat="server" />
    <input type="hidden" value="" id="hidListMixMatchId" runat="server" />--%>
    <input type="hidden" value="" id="hidFirstItemErrorId" runat="server" />
    
</section>
