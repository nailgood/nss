<%@ Control Language="VB" AutoEventWireup="false" CodeFile="video-list.ascx.vb" Inherits="controls_resource_center_video_video_list" %>
<%@ Register Src="~/controls/resource-center/video/video.ascx" TagName="video" TagPrefix="ucVideo" %>
<asp:Repeater ID="rptlstVideo" runat="server">
    <ItemTemplate>
        <ucVideo:video ID="ucVideo" runat="server" />
    </ItemTemplate>
</asp:Repeater>
<input type="hidden" id="hidVideoIndex" clientidmode="Static" value="" runat="server" />

