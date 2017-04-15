<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-samples.ascx.vb" Inherits="controls_product_free_samples" %>
<asp:Repeater ID="rptFreeSamples" runat="server">
    <ItemTemplate>
        <article class='freesamplesitem <%=IIf(m_isAllowAddCart, "", "noneaddcart") %>'>
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