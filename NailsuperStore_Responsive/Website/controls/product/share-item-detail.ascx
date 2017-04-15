<%@ Control Language="VB" AutoEventWireup="false" CodeFile="share-item-detail.ascx.vb"
    Inherits="controls_product_share_item_detail" %>
<%@ Register Src="../layout/addthis.ascx" TagName="addthis" TagPrefix="uc1" %>

<section class="share">
    <uc1:addthis ID="ucAddthis" runat="server" />
    <ul class="social-nav">
        <li><a href="/store/recently-viewed.aspx">Recently Viewed </a></li>
        <li><a href="/contact/pricematch.aspx">Request Price Match </a></li>
        <%=linkMSDS %>
    </ul>
</section>