<%@ Control Language="VB" AutoEventWireup="false" CodeFile="top3-media-list.ascx.vb" Inherits="controls_resource_center_media_top3_media_list" %>
<asp:Literal ID="ltrmCategory" runat="server"></asp:Literal>
<asp:Repeater ID="rptMedia" runat="server">
    <ItemTemplate>
        <asp:Literal ID="ltrdvMedia" runat="server"></asp:Literal>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="ltrViewMore" runat="server"></asp:Literal>
