<%@ Control Language="VB" AutoEventWireup="false" CodeFile="video-item-detail.ascx.vb"
    Inherits="controls_product_video_item_detail" %>
<asp:Repeater ID="rptVideo" runat="server">
    <HeaderTemplate>
        <ul class="video-list">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="ltrVideo" runat="server"></asp:Literal>
    </ItemTemplate>
    <FooterTemplate>
        </ul></FooterTemplate>
</asp:Repeater>
