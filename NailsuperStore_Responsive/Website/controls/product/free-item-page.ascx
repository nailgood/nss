<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-item-page.ascx.vb"
    Inherits="controls_product_free_item_page" %>
<%@ Register Src="free-sample-product.ascx" TagName="free" TagPrefix="uc1" %>
<%@ Register Src="~/controls/checkout/free-gift-level.ascx" TagName="freeGiftLevel"
    TagPrefix="uc2" %>
<%@ Register Src="~/controls/product/free-item-list.ascx" TagName="freeItemList"
    TagPrefix="uc2" %>
<div class="topbar">
    <uc2:freeGiftLevel ID="ucFreeGiftLevel" runat="server" />
    <div id="cat-desc" clientidmode="Static" class="dept-desc">
        <%= Description%>
    </div>
</div>
<div class="continue" id="divCartTop" runat="server" visible="false">
    <asp:Button ID="btnAddCartTop" CssClass="button" runat="server" Text="Continue" />
</div>
<uc2:freeItemList ID="freeItemList" runat="server" />
<div class="continue continue-bottom" id="divCartBottom" runat="server" visible="false">
    <asp:Button ID="btnAddCartBottom" CssClass="button" runat="server" Text="Continue" />
</div>
<input type="hidden" runat="server" id="hidContinueLink" name="hidContinueLink" clientidmode="Static" />
