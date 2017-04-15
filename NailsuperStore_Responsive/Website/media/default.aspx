<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"
    Inherits="MediaPress_Default" %>

<%@ Register Src="~/controls/resource-center/media/media-list.ascx" TagName="MediaPress" TagPrefix="ucMediaPress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server"> 
    <div id="page">
        <h1><asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>
            <div id="lstMedia">
                <asp:Repeater ID="rptlstMedia" runat="server">
                    <ItemTemplate>
                        <asp:Literal ID="ltrCategory"  runat="server"></asp:Literal>                    
                        <ucMediaPress:MediaPress ID="media" runat="server" />
                    </ItemTemplate>
               </asp:Repeater>
            </div>
            <input type="hidden" id="hidCategoryId" value="<%=CategoryId %>" />
            <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
<input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="text-align:center" id="imgloadingVideo">
        <img id="loader" alt="" src="<%= Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif" style="display:none;" />
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
    }
    $(window).scroll(function () {
        var url = document.location.href;
        var i = url.indexOf("media-topic");
        var arr = url.substring(i, url.length).split('/');
        if (arr.length == 1) {
            $("#loadmoreVideo").hide();
        }
        else {
            ScrollDataResourceCenter();
        }
    });
    $(window).load(function () {
        DelayResetHeightListVideo(false, 'media');
    });
    $(window).resize(function () {
        DelayResetHeightListVideo(true, 'media');
    });
</script>
    
</div>  
</asp:Content>
