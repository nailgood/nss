<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cart-free-gift.ascx.vb" Inherits="controls_checkout_cart_free_gift" %>
<div id="secFreeGift" class="free-gift">
    <ul>
        <asp:Repeater runat="server" ID="rptFreeGift">
            <ItemTemplate>
                <asp:Literal ID="ltrData" runat="server">
                </asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

</div>

