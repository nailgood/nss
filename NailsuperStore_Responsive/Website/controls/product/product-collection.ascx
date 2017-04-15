<%@ Control Language="VB" AutoEventWireup="false" CodeFile="product-collection.ascx.vb" Inherits="controls_product_product_collection" %>
<div class="item c-item"><div id="wrap-<%=CountDiv %>">
<div id="bd-<%=CountDiv %>" style="display:none"></div>
            <article>
            <div id="cinfo-<%=CountDiv %>" style="height: 270px">
                <asp:Literal ID="lticItem" runat="server"></asp:Literal>
                <div class="image c-img"><img id="img-<%=CountDiv %>" src="/assets/items/medium/<%=IIf(Image = Nothing, "na.jpg", Image)%>" alt="<%=ItemName%>"  style="width:167px;"/></div>                
                    <div class="title"><%=ItemName %></div>
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
            <div id="ccart-<%=CountDiv %>" class="add-cart" style="text-align:center">
                <center>
                    <div class="pad-qty"><asp:Literal ID="ltrQty" runat="server"></asp:Literal></div>
                    <asp:Literal ID="divCart" runat="server"></asp:Literal>
                    <%--<div runat="server" id="divFlammable" class="red" visible="false">
                    This item is not available for customer outside of 48 states within continental
                    USA.</div>--%>
                   <div name="lblInCart<%=ItemID %>" id="lblInCart<%=ItemID %>" class="incart"><%If isIncart Then%>Added to your cart<%End If%></div> 
                </center>    
             </div>
           
           </article>
</div></div>

     