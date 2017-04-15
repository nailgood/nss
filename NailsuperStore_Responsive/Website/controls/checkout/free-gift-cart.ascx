<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-gift-cart.ascx.vb" Inherits="controls_checkout_free_gift_cart" %>
    <section id="secFreeGift">
        <div style="width: 90%; float:left; overflow: hidden;">
            <span class="lblsub">FREE Gift</span><asp:Literal ID="litFreeGiftMsg" runat="server"></asp:Literal>
       <%-- <div id="icon" class="fclose" onclick="OpenFreeGift();"></div>--%>
        </div>  
         <i class="fa fa-angle-double-down" onclick="OpenFreeGift();"></i>
        <div id="divListGift"></div>
        <asp:HiddenField ID="hidFreeGiftLevelId" runat="server" />
    </section>
    