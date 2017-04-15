<%@ Control Language="VB" AutoEventWireup="false" CodeFile="review.ascx.vb" Inherits="controls_product_review" %>
<article class="review-item <%=itemfirstClass %>">

    <div class="item-data" id="divImage"  runat="server" visible="false">
        <div class="item-image">
            <asp:Literal ID="ltrItemImage" runat="server">
            </asp:Literal>            
        </div>
       
    </div>
    <div class="review-data" id="divReviewData" runat="server">
     <asp:Literal ID="ltrLinkItem" runat="server"></asp:Literal>
     <ul class="title hidden-xs">
        <li class="star" id="liStar" runat="server">           
        </li>
        <li class="name" id="liName" runat="server" ></li>
        <li class="author">by <span id="spAuthor" runat="server"></span></li>
     </ul>
     <ul id="grSku" runat="server" class="title grsku" visible="false">
        <li class="sku" id="lsku" runat="server"></li>
        <li class="text" id="lnamecollection" runat="server"></li>
     </ul>
     <ul class="title small hidden-md hidden-sm hidden-lg">
        <li class="name" id="liNameSmall" runat="server"></li>
        <li class="star" id="liStarSmall" runat="server"></li>
        <li class="author">by <span id="spAuthorSmall" runat="server"></span></li>
     </ul>
     <asp:Literal ID="ltrGroup" runat="server"></asp:Literal>
     <div class="commment" id="divComment" runat="server">
            <span class="text">Product Comment:</span> 
            <asp:Literal id="ltrComment" runat="server"></asp:Literal>

     </div>
      <div class="commment" id="divSuggestion" runat="server">
            <span class="text">Product Suggestions:</span> 
            <asp:Literal id="ltrSuggestion" runat="server"></asp:Literal>

     </div>
      <div class="commment" id="divBottomLine" runat="server">
            <span class="text">Bottom Line:</span> 
            Yes, I would recommend this to a friend

     </div>
      <div class="adminReply" id="divAdminReply" runat="server" style="display:block">
           <div class="top">&nbsp;</div>
           <div class="content">
                    <div class="title">The Nail Superstore replied:</div>
                    <div class="data"><asp:Literal ID="ltrAdminReply" runat="server"></asp:Literal> </div>
            </div>
       </div>
    </div>
</article>
