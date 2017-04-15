<%@ Control Language="VB" AutoEventWireup="false" CodeFile="purchased.ascx.vb" Inherits="controls_product_purchased" %>
<%If IsFirstLoad Then%><div class="order"><div class="content<%=itemindex %>"><%End If%>

 <div>
   <h3 class="review-title"><asp:Literal ID="lit" runat="server"></asp:Literal></h3>
 </div>
<div class="tbl-order" id="trCartItems" runat="server">

            <div class="cart">
              <div class="header-row">
                    <div class="header h-group-name" style="width:88px">
                        Item
                    </div>
                    <div class="header h-item-name">&nbsp;</div>
                     <%If isProductReview = False Then%>
                    <div class="header h-group-name hidden-xs">Unit</div>
                    <%End If %>
                    <%If isProductReview Then%>
                   <div class="header h-group-name hidden-xs">
                            Qty
                    </div>
                    <div class="header h-group-name ship hidden-xs">
                            Ship Via
                    </div>
                    <%End If%>
                    <div class="header h-group-name lg-price">
                         <%If isProductReview then%>Total<%else%>Price<%end if %>
                    </div>
                    <div class="header h-w-addcart">&nbsp;</div>
              
              </div>
              <asp:Literal ID="ltContent" runat="server"></asp:Literal>
              <asp:Repeater runat="server" ID="rptCartItems" OnItemDataBound="rptCartItems_ItemDataBound">
                    <ItemTemplate>
                        <div id="trCart" runat="server" class="row-item">
                          <div class="img"><asp:Literal runat="server" ID="lnkImg" /></div>
                          <div class="item-name">
                                <asp:Literal runat="server" ID="litDetails" />
                                <div class="mag">
                                    <asp:Literal runat="server" ID="litCoupon" /></div>
                                    <span class="xs-price"><%If isProductReview Then%><asp:Literal runat="server" ID="ltrTotal1" /><%Else%><asp:Literal runat="server" ID="ltPrice1" /><%End If%></span>
                          </div>
                           <%If isProductReview = False Then%>
                          <div class="group-name unit hidden-xs">
                                 <%#	IIf(Container.DataItem.PriceDesc = Nothing, "&nbsp;", Container.DataItem.PriceDesc)%>
                            </div>
                            <%End If %>
                            <%If isProductReview Then%>
                           <div class="group-name hidden-xs">
                                   <%#Container.DataItem.Quantity%>
                            </div>
                            <div class="group-name ship hidden-xs">
                                <ul class="list-inline">
                                    <asp:Literal runat="server" ID="litImageShipping" />
                                    <li>
                                        <asp:Literal runat="server" ID="litSelected" />
                                    </li>
                                </ul>
                            </div>
                            <%End If%>
                           <div class="group-name lg-price">
                               <asp:Literal runat="server" ID="ltPrice" />
                               <asp:Literal runat="server" ID="ltrTotal" />
                            </div>
                            <div class="group-name w-addcart"> 
                                <asp:Literal ID="ltaddCart" runat="server"></asp:Literal>
                                <%If isProductReview Then%><asp:Literal ID="ltCartReview" runat="server"></asp:Literal><%End If %>
                            </div>
                          </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
</div>
<%If IsFirstLoad Then%></div></div><%End If%>