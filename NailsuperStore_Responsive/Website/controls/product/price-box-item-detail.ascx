<%@ Control Language="VB" AutoEventWireup="false" CodeFile="price-box-item-detail.ascx.vb" Inherits="controls_product_price_box_item_detail" %>

<%@ Register Src="~/controls/product/policy.ascx" TagPrefix="uc" TagName="Policy"  %>
<%@ Register Src="~/controls/product/price-buy-in-bulk-detail.ascx" TagPrefix="uc" TagName="BuyInBulk" %>


<div class="page-wrapper">
    <asp:Repeater ID="rptAttribute" runat="server">
        <HeaderTemplate>
            <ul class="list-attribute" id="ulAttribute">
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Literal ID="ltrAttribute" runat="server"></asp:Literal>
        </ItemTemplate>
        <FooterTemplate>
            </ul></FooterTemplate>
    </asp:Repeater>
    <div id="divPriceBox" itemprop="offers" itemscope itemtype="http://schema.org/<%=typeOffer %>" runat="server">
        <asp:Literal ID="ltrIcon" runat="server"></asp:Literal>    
        <div class="box">
            <div class="price-wrapper">
                <asp:Literal ID="ltrPrice" runat="server"></asp:Literal>
                <div id="divHandlingFee" class="handling" runat="server">
                    <a href="javascript:void(ShowHandlingTip());">Special Handling Fee:
                        <asp:Literal ID="ltrHandlingFee" runat="server"></asp:Literal>
                    </a>
                </div>
            </div>
            <div class="point" id="divPoint" runat="server">
            </div>
            <div class="promotion" id="divPromotion" runat="server" visible="false">
                <div class="name">
                    Special Offer<span id="spanGoodThru" runat="server"> </span>
                </div>
                <div class="desc">
                    <asp:Literal ID="ltrMM" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="message" id="divMessage" runat="server">
            </div>
            <div class="outstock" id="divOutStock" runat="server">
                <span>Out of Stock</span>
            </div>
            <div id="divNotify" class="notify" runat="server">
                <asp:HyperLink runat="server" NavigateUrl="" ID="lnkNotify" Text="Notify me when in stock" /></div>
            <div class="instock" id="divInStock" runat="server">
               
                <div itemtype="http://schema.org/Offer"><link href="http://schema.org/InStock" itemprop="availability" />
                <span class="warning"
                    id="divStockWarning" runat="server"></span>
                </div>
            </div>
            <div class="clearfix ">
            </div>
            <div class="cart-wrapper" id="cartWrapper" runat="server">
                <div class="text">
                    Quantity
                </div>
                <div class="qty">
                    <div class="plus">
                        <a href="javascript:void(ChangeQty('txtQty','up','<%=hidUnitPoint %>','<%=hidPricePoint %>'));">+</a></div>
                    <div class="min">
                        <a href="javascript:void(ChangeQty('txtQty','down','<%=hidUnitPoint %>','<%=hidPricePoint %>'))">&ndash;</a></div>
                    <asp:TextBox ID="txtQty" ClientIDMode="Static"  onkeypress="return numbersonly(txtQty,event);"
                        runat="server" CssClass="txt-qty" MaxLength="4" Text="1"></asp:TextBox>
                    <ul>
                        <li class="up" onclick="ChangeQty('txtQty','up','<%=hidUnitPoint %>','<%=hidPricePoint %>')"><b></b></li>
                        <li class="middle" >&nbsp;</li>
                        <li class="down" onclick="ChangeQty('txtQty','down','<%=hidUnitPoint %>','<%=hidPricePoint %>')"><b></b>
                        </li>
                    </ul>
                </div>
                <input type="button" id="btnAddCart" runat="server" class="add-cart" value="Add To Cart" />
                <div class="incart" id="divInCart" runat="server" style="text-align:center">Item is in your cart</div>
            </div>
        </div>
        
        <div class="flammable" id="divFlammableWarning" runat="server" visible="false">
            <a href="javascript:void(ShowFlammableTip());"><img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/flammable-warning.png" alt="Flammable Warning" />Flammable & Hazardous Material Item</a>
        </div>
        <div id="divMSDS" class="flammable" runat="server" visible="false">
            <a href="<%=Utility.ConfigData.CDNMediaPath %>/upload/msds/<%=linkMSDS %>" target="_blank"><img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/pdf-icon.png" alt="Material Safety Data Sheet" />Material Safety Data Sheet (MSDS)</a>
        </div>
        <uc:BuyInBulk runat="server" ID="ucBuyInBulk" />
    </div>
    <div id="divLoginViewPrice" runat="server" visible="false">
        <div class="msg">Required registered professional<br />log in to purchase OPI products</div>
        <a href="/members/login.aspx">Log In | Register</a>
    </div>
    <uc:policy ID="ucPolicy" runat="server" />
</div>

