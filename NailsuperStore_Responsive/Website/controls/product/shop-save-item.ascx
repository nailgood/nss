<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shop-save-item.ascx.vb" Inherits="controls_product_shop_save_item" %>
<div class="<%=GetItemClass() %>">
    <div class="hidden-left">
        &nbsp;</div>
    <div class="hidden-top">
        &nbsp;</div>
    <div class="box" title="<%= ShopSaveItem.Name %>">
        <a href="<%= ShopSaveItem.Url %>">
            <%--<img src="<%=image %>" />--%>
            <asp:Literal ID="ltimg" runat="server"></asp:Literal>
        </a>
    </div>
    <div class="name">
        <%= ShopSaveItem.Name%></div>
    <div class="desc">
        <asp:Literal ID="divDes" runat="server"></asp:Literal>
    </div>
   
    <div class="shopnow">
        <div class="c-button"><a href="<%= ShopSaveItem.Url %>">Buy Now</a></div>
    </div>
    <div class="hidden-bottom">
        &nbsp;</div>
    <div class="hidden-right">
        &nbsp;</div>
</div>
