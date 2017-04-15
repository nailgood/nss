<%@ Control Language="VB" AutoEventWireup="false" CodeFile="testimonial-item.ascx.vb" Inherits="controls_product_testimonial_item" %>

<div id="testimonial">
<asp:Repeater ID="rptTestimonial" runat="server">
    <ItemTemplate>  
            <article class="testimonial-item">
        <ul>
            <li class="title"><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></li>
            <li class="start">
                <asp:Literal ID="ltrStar" runat="server"></asp:Literal>
                <%--<img src="/includes/theme/images/smallstar50.png" style="border-style: none" alt="Red Dragon Sauna Spa Complete System - Chocolate">--%>
            </li>
            <li class="comment"><asp:Literal ID="ltrComment" runat="server"></asp:Literal></li>
            <li class="author"><asp:Literal ID="ltrAuthor" runat="server"></asp:Literal></li>
            <li class="address"><asp:Literal ID="ltrAddress" runat="server"></asp:Literal></li>
        </ul>
    </article>
    </ItemTemplate>
</asp:Repeater>    
</div>