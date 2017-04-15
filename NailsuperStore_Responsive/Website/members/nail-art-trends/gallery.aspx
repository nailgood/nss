<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="gallery.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" Inherits="members_nail_art_trends_gallery" %>

<%@ Register Src="~/controls/resource-center/gallery.ascx" TagName="Gallery" TagPrefix="ucGallery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="page">
        <%--<script type="text/javascript" src="/includes/scripts/popup.js"></script>Edit css.xml--%>
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
      <h1>Customer Nail Art</h1>
      <p>Want to see your nail art design here? <a href="/nail-art-trend" style="color: #3b76ba;">submit it now<b class="arrow-gallery-left"></b></a></p>
                <div id="lstGallery" style="float:left;">                    
                    <asp:Repeater runat="server" ID="rptGallery">
                        <ItemTemplate>
                            <ucGallery:Gallery ID="gallery" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <input type="hidden" id="hidCategoryId" value="" />
                <input type="hidden" id="hidVideoIndex" clientidmode="Static" value="<%=hidGalleryIndex %>" />
                 <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
<input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center" id="imgloadingVideo">
        <img id="loader" alt="" src="<%Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmoreVideo" onclick="GetRecordsResourceCenter(0,pgsize);" class="see-more-items"  style="display:none">
      <%=Resources.Msg.SeeMoreData%>
</div>
                <div id="dvSubmitNow">
                    Want to see your nail art design here? <a style="color: #3b76ba;" href="/nail-art-trend">
                        Submit it now!</a>
                </div>
    </div>
    <script type="text/javascript">
        var pgsize = document.getElementById('hidPageSizeVideo').value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdataVideo').value,
        isLoading = 0;
       
        if (pageCount - pageIndex * pgsize > 0) {
            $("#loadmoreVideo").show();
        }
        $(window).scroll(function () {
            ScrollDataResourceCenter();
        });
        $(window).load(function () {
            DelayResetHeightListVideo(false,'gallery');
        });
        $(window).resize(function () {
            ResetHeightListVideo(true, 'gallery');
        });
</script>
</asp:Content>
