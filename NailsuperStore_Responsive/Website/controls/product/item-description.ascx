<%@ Control Language="VB" AutoEventWireup="false" CodeFile="item-description.ascx.vb"
    Inherits="controls_product_item_description" %>


<section class="description">
    <div class="label">
        Description <span class="sku">
            <asp:Literal ID="ltrSKU" runat="server"></asp:Literal>
        </span>
    </div>
    <div class="content" itemprop="description">
        <asp:Literal ID="ltrDescription" runat="server"></asp:Literal>
    </div>
</section>
