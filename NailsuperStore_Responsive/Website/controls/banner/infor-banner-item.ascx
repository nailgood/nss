<%@ Control Language="VB" AutoEventWireup="false" CodeFile="infor-banner-item.ascx.vb" Inherits="controls_banner_infor_banner_item" %>
<div class="<%=GetItemClass() %>">
    <div class="hidden-left">
        &nbsp;</div>
    <div class="hidden-top">
        &nbsp;</div>
    <div class="image">
        <a href="<%= BannerItem.Link %>">
           <%-- <img src="<%=image %>" />--%>
           <asp:Literal ID="ltimg" runat="server"></asp:Literal>
        </a>
    </div>
    <div class="clearfix visible-sm visible-xs">
    </div>
    <div class="name">
        <a href="<%= BannerItem.Link %>">
            <%= BannerItem.Name%>
        </a>
    </div>
    <hr />
    <div class="clearfix visible-sm visible-xs">
    </div>
    <div class="desc">
        <%= BannerItem.Description%>
    </div>
    <div class="hidden-bottom">
        &nbsp;</div>
    <div class="hidden-right">
        &nbsp;</div>
</div>
