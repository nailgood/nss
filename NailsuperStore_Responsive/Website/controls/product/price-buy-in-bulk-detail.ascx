<%@ Control Language="VB" AutoEventWireup="false" CodeFile="price-buy-in-bulk-detail.ascx.vb" Inherits="controls_product_price_buy_in_bulk_detail" %>

<%--<div id="secIcon" class="icon none-att ">
    <ul class="buy-bulk">
        <li>
            <span class="ico-buybulk"></span>
        </li>
    </ul>
</div>--%>
<div class="icon">
    <span class="ico-buybulk">Buy In  Bulk</span></div>
<div class="boxCase">
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
    <div class="clearfix ">
    </div>
    <div class="cart-wrapper" id="cartWrapper" runat="server">
        <div class="text">
            Quantity
        </div>
        <div class="qty">
            <div class="plusCase">
                <a href="javascript:void(ChangeQty('txtQtyCase','up','<%=hidUnitPoint %>','<%=hidPricePoint %>'));">+</a>
            </div>
            <div class="minCase">
                <a href="javascript:void(ChangeQty('txtQtyCase','down','<%=hidUnitPoint %>','<%=hidPricePoint %>'))">&ndash;</a>
            </div>
            <asp:TextBox ID="txtQtyCase" ClientIDMode="Static" onkeypress="return numbersonly(txtQtyCase,event);"
                runat="server" CssClass="txt-qty" MaxLength="4" Text="1"></asp:TextBox>
            <ul>
                <li class="up" onclick="ChangeQty('txtQtyCase','up','<%=hidUnitPoint %>','<%=hidPricePoint %>')"><b></b></li>
                <li class="middleCase">&nbsp;</li>
                <li class="down" onclick="ChangeQty('txtQtyCase','down','<%=hidUnitPoint %>','<%=hidPricePoint %>')"><b></b>
                </li>
            </ul>
        </div>
        <input type="button" id="btnAddCartCase" runat="server" class="add-cart" value="Add To Cart" />
        <div class="incart" id="divInCart" runat="server" style="text-align:center">Case is in your cart</div>
    </div>
    
    <div class="textCase" id="divCaseDetail" runat="server" style="text-align: center"></div>
</div>
