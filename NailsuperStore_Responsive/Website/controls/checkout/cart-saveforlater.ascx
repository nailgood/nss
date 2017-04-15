<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cart-saveforlater.ascx.vb" Inherits="controls_checkout_cartsave" %>

<section id="secSaveCart" class="cart"> 
  
    <asp:Repeater runat="server" ID="rptCartSave">
        <HeaderTemplate>
        <div class="title"><%=SaveCartCount %></div>
        <table id="tabSaveCart">
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
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="row-item" id="trRow" runat="server">
                <td class="item">
                    <div class="image">
                        <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                    </div>
                </td>
                <td class="name">
                    <div class="name-container1 cart-saveforlater-container1">
                        <div class="name-container2 cart-saveforlater-container2">
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
                    <div class="qty qty-disable cart-saveforlater" id="divQty" runat="server">
                    </div>
                    <div class="remove">
                        <asp:HyperLink ID="hplRemove" runat="server" Text="Delete"></asp:HyperLink> |
                        <asp:HyperLink ID="hplMove" runat="server" Text="Move to Cart"></asp:HyperLink>
                    </div>
                </td>
                <td class="price hidden-xs" id="tdPrice" runat="server">
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</section>
