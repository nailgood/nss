<%@ Control Language="VB" AutoEventWireup="false" CodeFile="top3-news-list.ascx.vb" Inherits="controls_resource_center_news_top3_news_list" %>
<asp:Literal ID="ltrmCategory" runat="server"></asp:Literal>
<asp:Repeater ID="rptNews" runat="server">
    <ItemTemplate>
        <asp:Literal ID="ltrdvNews" runat="server"></asp:Literal>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="ltrViewMore" runat="server"></asp:Literal>