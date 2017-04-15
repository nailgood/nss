<%@ Control Language="VB" AutoEventWireup="false" CodeFile="news.ascx.vb" Inherits="controls_resource_center_news_news" %>

<div class="dvNewsItem">
    <asp:Literal ID="ltrCategory" runat="server"></asp:Literal>
    <article>
        <asp:Literal ID="ltrDivImage" runat="server"></asp:Literal>
        <div class="boxshort">
            <div class="title">
                <asp:HyperLink ID="hlTitle" runat="server"></asp:HyperLink></div>
            <div class="date">
                <asp:Literal ID="ltrDate" runat="server"></asp:Literal>
            </div>
            <div class="desc">
                <asp:Literal ID="ltrDesc" runat="server"></asp:Literal></div>
        </div>
    </article>
</div>
