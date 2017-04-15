<%@ Control Language="VB" AutoEventWireup="false" CodeFile="review-order.ascx.vb" Inherits="controls_product_revieworder" %>
<%If IsFirstLoad Then%><div class="order"><div class="content<%=itemindex %>"><%End If%>
 <div class="orderreview">
                            <div>
                                <span class="text">Customer Rating</span><span class="hide-sm"> (1 to 5 Stars)</span>: 
                  <%--              <div class="star" style="background:url('<asp:Literal id=litStar runat=server></asp:Literal>') no-repeat center -1px;">
                                    <img src="/includes/theme/images/spacer.gif" width="90px" height="22px" />
                                </div> --%>
                                <asp:Literal id=litStar runat=server></asp:Literal>
                                    <span class="hide-sm"><asp:Literal id="litStar2" runat="server"></asp:Literal></span>
                            </div>
                            <div class="name">by <asp:Label id="lblName" runat="server" CssClass="text"></asp:Label></div>
                            <div><span class="text">Comment</span>: <asp:label ID="lblContent" runat="server"></asp:label></div>
                            <div style="display:table" class="adminReply" id="adminreply" runat="server">
                                <div class="top">&#160;&nbsp;</div>
                                    <div class="content">
                                        <div class="text">The Nail Superstore replied:</div>
                                        <div class="data">{0} </div>
                                    </div>
                            </div>
 </div>
 <%If IsFirstLoad Then%></div></div><%End If%>