<%@ Control Language="VB" AutoEventWireup="false" CodeFile="top3-video-list.ascx.vb" Inherits="controls_resource_center_video_top3_video_list" %>
<asp:Literal ID="ltrmCategory" runat="server"></asp:Literal>
<asp:Repeater ID="rptVideo" runat="server">
    <ItemTemplate>
        <asp:Literal ID="ltrdvVideo" runat="server"></asp:Literal>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="ltrViewMore" runat="server"></asp:Literal>