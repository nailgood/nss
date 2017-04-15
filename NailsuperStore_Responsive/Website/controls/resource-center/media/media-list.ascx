<%@ Control Language="VB" AutoEventWireup="false" CodeFile="media-list.ascx.vb" Inherits="controls_resource_center_media_media_list" %>
<%@ Register Src="~/controls/resource-center/media/media.ascx" TagName="media" TagPrefix="ucMedia" %>
<asp:Repeater ID="dvMedia" runat="server">
    <ItemTemplate>
        <ucMedia:media ID="ucMedia" runat="server"></ucMedia:media>
    </ItemTemplate>
</asp:Repeater>
<input type="hidden" id="hidVideoIndex" clientidmode="Static" value="" runat="server" />