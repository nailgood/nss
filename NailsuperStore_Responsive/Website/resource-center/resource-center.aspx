<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="resource-center.aspx.vb" Inherits="resource_center_resource_center" %>
<%@ Register Src="~/controls/resource-center/news/top3-news-list.ascx" TagName="news" TagPrefix="uclstTop3News" %>
<%@ Register Src="~/controls/resource-center/video/top3-video-list.ascx" TagName="video" TagPrefix="uclstTop3Video" %>
<%@ Register Src="~/controls/resource-center/media/top3-media-list.ascx" TagName="media" TagPrefix="uclstTop3Media" %>
<%@ Register Src="~/controls/resource-center/top4-gallery-list.ascx" TagName="gallery" TagPrefix="uclstTop4Gallery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="lstsummary">
        <uclstTop3News:news ID="ucNews" runat="server" />
        <uclstTop3Video:video ID="ucVideo" runat="server" />
        <uclstTop3Media:media  ID="ucMedia" runat="server" />
        <uclstTop4Gallery:gallery  ID="ucGallery" runat="server" />
    </div>
</asp:Content>

