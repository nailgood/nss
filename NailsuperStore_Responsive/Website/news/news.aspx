<%@ Page Language="VB" AutoEventWireup="false" CodeFile="news.aspx.vb" Inherits="News_List"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<%@ Register Src="~/controls/resource-center/news/news.ascx" TagName="news" TagPrefix="ucNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">  
    <div id="contentnews-left">      
           <div id="smcategoryNews" style="display:none;">
                <div id="smtitleNews"> 
                    <span style="float: right; " class="glyphicon ic-minus ic-plus"></span>
                </div>
                <div id="smcategory"></div>
           </div>
    </div>
          <%--  <h1> <asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>--%>
            <div class="content-news">
                <div id="news-top">
                    <article>
                         <asp:Literal ID="ltrDivImageTop" runat="server"></asp:Literal>
                         <asp:Literal ID="hlTitleTop" runat="server"></asp:Literal>
                         <asp:Literal ID="ltrDescTop" runat="server"></asp:Literal>
                     </article>
                </div>           
                <div class="news-lst" style="float:left;">
                    <asp:Repeater ID="rptNews" runat="server">
                        <ItemTemplate>
                          <asp:Literal ID="ltrmCategory" runat="server"></asp:Literal>
                            <ucNews:news ID="ucNews" runat="server"></ucNews:news>
                          </ItemTemplate>
                    </asp:Repeater>
                </div>
                <input type="hidden" id="hidCategoryId" value="<%=CategoryId %>" />
                <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
<input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center" id="imgloadingVideo">
        <img id="loader" alt="" src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmoreVideo" onclick="GetRecordsResourceCenter();" class="see-more-items"  style="display:none;">
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
        $(window).load(function () {
            ShowArchive();
        });
        $(window).resize(function () {
            ShowArchive();
        });
        $(window).scroll(function () {
            var url = document.location.href;
            var i = url.indexOf("news-topic");
            var arr = url.substring(i, url.length).split('/');
            if (arr.length == 1 ) {
                $("#loadmoreVideo").hide();
            }
            else {
                ScrollDataResourceCenter();
            }
        });
</script>
  </div>   
</asp:Content>
