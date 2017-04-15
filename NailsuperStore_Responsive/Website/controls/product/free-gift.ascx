<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-gift.ascx.vb" Inherits="controls_product_free_gift" %>
<asp:Repeater ID="rptFreeGift" runat="server">
    <ItemTemplate>
        <article class='freegiftitem <%=IIf(m_isAllowAddCart, "", "noneaddcart") %>'>
            <div class="image">
                <asp:Literal ID="litImage" runat="server"></asp:Literal>
            </div>
            <div class="name">
                <asp:Literal ID="litName" runat="server"></asp:Literal>
            </div>
            <asp:Literal ID="litAddCart" runat="server"></asp:Literal>
        </article>
    </ItemTemplate>
</asp:Repeater>