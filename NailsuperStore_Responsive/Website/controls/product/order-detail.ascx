<%@ Control Language="VB" AutoEventWireup="false" CodeFile="order-detail.ascx.vb"
    Inherits="controls_product_order_detail" %>
<div class="title" id="divTitle" runat="server">
</div>
<ul class="address" id="ulAddress" runat="server">
    <li class="billing">
        <asp:Literal ID="ltrBillingAddress" runat="server">
        </asp:Literal>
        <div id="IPLocation" runat="server"></div>
        <div class="customer" id="divCustomer" runat="server" visible="false">
        </div>
        <ul id="ulAddTracking" runat="server" visible="false" class="order-tracking">
            <li class="title">Tracking number</li>
            <%If linkTracking <> String.Empty Then%>
            <li>
                <%= linkTracking%></li>
            <%End If%>
            <%If linkEdit <> String.Empty Then%>
            <li>
                <%= linkEdit%></li>
            <%End If%>
        </ul>
    </li>
    <li class="shipping">
        <asp:Literal ID="ltrShippingAddress" runat="server">
        </asp:Literal>
    </li>
</ul>
<div class="cart-item-list">
    <div class="header">
        <div class="image">
            Item</div>
        <div class="name">
        </div>
        <div class="qty">
            Qty</div>
            <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
        <div class="qtyship">
            Qty Shipped</div>
            <% End If%>
        <div class="unit">
            Unit</div>
        <div class="shipment">
            Ship Via / Options</div>
        <div class="total">
            Total</div>
    </div>
    <asp:Repeater runat="server" ID="rptCartItems">
        <ItemTemplate>
            <div class="cart-row" id="divRow" runat="server">
                <div class="image cart-cell">
                    <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                    <div class="sku">
                        #<asp:Literal ID="ltrSKU" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="name cart-cell">
                    <div class="item-name">
                        <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                    </div>
                    
                    <div class="error" id="divWarning" runat="server" visible="false">
                        
                    </div>
                    <div id="divPromotion" runat="server" class="promotion">
                    </div>
                    <div class="smallqty">
                        Qty:
                        <asp:Literal ID="ltrSmallQty" runat="server"></asp:Literal>
                    </div>
                    <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                    <div class="smallqtyship">
                        Qty Shipped:
                        <asp:Literal ID="ltrSmallQtyShip" runat="server"></asp:Literal></div>
                    <div class="smallshipment ">
                        <asp:Literal ID="ltrSmallShipment" runat="server"></asp:Literal>
                    </div>
                    <% End If%>
                </div>
                <div class="qty cart-cell ">
                    <asp:Literal ID="ltrQty" runat="server"></asp:Literal></div>
                
                    <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                <div class="qtyship cart-cell">
                    <asp:Literal ID="ltrQtyShip" runat="server"></asp:Literal></div>
                     <% End If%>
                <div class="unit cart-cell">
                    <asp:Literal ID="ltrUnit" runat="server">
                    </asp:Literal>
                </div>
                <div class="shipment cart-cell">
                    <div class="tbl-shipping" id="tdShipping" runat="server">
                        <div class="shipping-row">
                            <div class="icon">
                                <asp:Literal ID="ltrImageShipping" runat="server"></asp:Literal>
                            </div>
                            <div class="text">
                                <asp:Literal ID="ltrShippingName" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="shipping-row" id="divFreeShip" visible="false" runat="server">
                            <div class="icon">
                            </div>
                            <div class="text free">
                                FREE Shipping
                            </div>
                        </div>
                    </div>
                </div>
                <div class="total cart-cell">
                    <asp:Literal ID="ltrTotal" runat="server"></asp:Literal>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="order-note" id="divNote" runat="server" visible="false">
    <div class="order-note-title">
        Order Notes
    </div>
    <div class="content" id="divNoteContent" runat="server">
        This is NOT residential!! I should not have a residential charge.
    </div>
</div>
