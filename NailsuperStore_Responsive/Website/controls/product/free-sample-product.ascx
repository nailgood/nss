<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-sample-product.ascx.vb"
    Inherits="controls_product_free_sample_product" %>
<article class="sample-item">
    <div class="top"></div>
    <div class="image">
        <asp:Literal ID="ltrImage" runat="server">
        </asp:Literal>
    </div>
    <div class="name">
        <asp:Literal ID="ltrName" runat="server">
        </asp:Literal>
    </div>
    <div class="desc">
        <asp:Literal ID="ltrDes" runat="server">
        </asp:Literal>
    </div>
    <div class="price" id="divPrice" runat="server">
        <span class="strike bold">$29.99</span>&nbsp;&nbsp;<span class="red bold">FREE</span>
    </div>
    <div class="error" id="divError" runat="server" visible="true">
        <asp:Literal ID="ltrError" runat="server"></asp:Literal>
    </div>
     <div class="select" id="divSampleSelect" runat="server">
        <div class="checkbox">
            <label for="chkSelect">
                <asp:CheckBox runat="server" ID="chkSelect" AutoPostBack="False" />
                <asp:Literal ID="ltrSpanSelect" runat="server"></asp:Literal>
                <span id="spLabel" runat="server" clientidmode="Static">I want this free sample!</span>
            </label>
        </div>
    </div>
    <div class="select selectGift" id="divFreeGiftSelect" runat="server">
        <asp:Button ID="btnAddcart" ClientIDMode="Static" runat="server" />
    </div>
</article>
<div class="ver-line"></div>