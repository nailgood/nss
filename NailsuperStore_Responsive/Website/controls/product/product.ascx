<%@ Control Language="VB" AutoEventWireup="false" CodeFile="product.ascx.vb" Inherits="controls_product_product" %>
<div class="item left"><div id="wrap-<%=CountDiv %>">
<div id="bd-<%=CountDiv %>" class="top"></div>
            <article style="display:table-cell">
            <div id="info-<%=CountDiv %>" style="height: 400px">
            <asp:Literal ID="lticItem" runat="server"></asp:Literal>
                <div class="image"><a href="" runat="server" id="aItem1" clientidmode="AutoID" class="s-image"><img id="img-<%=CountDiv %>" class="lazy" src="<%=Utility.ConfigData.CDNmediapath %>/assets/items/medium/<%=IIf(Image = Nothing, "na.jpg", Image)%>" alt="<%=ItemName%>" /></a></div>
                     
                   
                    <asp:Literal runat="server" ID="litReview"></asp:Literal>
                    <div class="title"><a runat="server" id="aItem2" clientidmode="AutoID"></a></div>
                    <div class="g-price">
                        <ul>
                            <li class="price">
                                 <asp:Literal runat="server" ID="lblPrice"></asp:Literal>
                            </li>
                            <asp:Literal ID="ltsave" runat="server"></asp:Literal>
                        </ul>
                    </div>
                    <div class="promotion">
                        <%=PromotionText %>
                    </div>
             </div>
            <div id="cart-<%=CountDiv %>" class="add-cart">
                  <div id="divCartWrapper" runat="server" clientidmode="Static" >
                    <asp:Literal ID="ltrQty" runat="server"></asp:Literal>
                    <asp:Literal ID="litBtnAddCart" runat="server"></asp:Literal>
                    <asp:Literal ID="divCart" runat="server"></asp:Literal>
                    
                 <%--   <div runat="server" id="divFlammable" class="red">This item is not available for customer outside of 48 states within continental USA.</div>--%>
                   
                </div>  
                <div id="lblInCart<%=ItemID %>" name="lblInCart<%=ItemID %>" class="incart"><%If isIncart Then%>Added to your cart<%End If%></div> 
                <div id="divRemoveCart" runat="server" clientidmode="Static" class="remove-rewardpoint"></div> 
             </div>
           
           </article>
         <%--<span style="color:Red"><%=CountDiv%></span>--%>
</div></div>
