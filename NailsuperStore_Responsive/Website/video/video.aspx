<%@ Page Language="VB" AutoEventWireup="false" CodeFile="video.aspx.vb" Inherits="Video_List"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<%@ Register Src="~/controls/resource-center/video/video-list.ascx" TagName="video" TagPrefix="uclstvideo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">  
        <div class="content-news">
            <div id="news-top">
                <article>
                    <asp:Literal ID="ltrDivImageTop" runat="server"></asp:Literal>
                    <asp:Literal ID="hlTitleTop" runat="server"></asp:Literal>
                    <div class="dvDate">
                        <div class="date">
                            <asp:Literal ID="ltrDate" runat="server"></asp:Literal>
                        </div>
                        <div class="viewTotal">                        
                           <asp:Literal ID="ltrComment" runat="server"></asp:Literal>
                        </div>
                         <div class="viewTotal">
                            <asp:Literal ID="ltrVote" runat="server"></asp:Literal>
                        </div>
                        <div class="viewTotal">
                            <asp:Literal ID="ltrView" runat="server"></asp:Literal>                            
                        </div>      
                    </div>
                    <div class="dvShortDesc"><asp:Literal ID="ltrShortDesc" runat="server"></asp:Literal></div>
                </article>
            </div>
        </div>    
    <div id="lstvideo">
        <asp:Repeater ID="rptVideo" runat="server">
            <ItemTemplate>
                <asp:Literal ID="ltrmCategory" runat="server"></asp:Literal>
               <uclstvideo:video ID="ucVideo" runat="server" />
                <asp:Literal ID="ltrViewMore" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <input type="hidden" id="hidCategoryId" value="<%=CategoryId %>" />
    <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
<input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center; clear:both;" id="imgloadingVideo">
        <img id="loader" alt="" src="/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmoreVideo" onclick="GetRecordsResourceCenter();" class="see-more-items"  style="display:none">
      <%=Resources.Msg.SeeMoreData%>
</div>
    <script type="text/javascript">
        var pgsize = document.getElementById('hidPageSizeVideo').value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdataVideo').value,
        isLoading = 0;

        if (pageCount - pageIndex * pgsize > 0) {
            $("#loadmoreVideo").show();
            //  alert('1');
        }
        $(window).scroll(function () {
            var url = document.location.href;
            var i = url.indexOf("video-topic");
            var arr = url.substring(i, url.length).split('/');
            if (arr.length == 1) {
                $("#loadmoreVideo").hide();
            }
            else {
                ScrollDataResourceCenter();               
            }
        });
        $(window).load(function () {
            DelayResetHeightListVideo(false, 'video');
        });
        $(window).resize(function () {
            ResetHeightListVideo(true, 'video');
        });
</script>
</asp:Content>
