<%@ Page Language="VB" AutoEventWireup="false" CodeFile="order-list.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="ReviewOrderList" %>
<%@ Register src="~/controls/product/review-order.ascx" tagname="revieworder" tagprefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<h1>Customer Reviews</h1>
             <div id="order-detail">
                    <asp:Repeater runat="server" ID="rptOrderReview">
                        <itemtemplate>
                            <uc:revieworder ID="ucReview" runat="server" />
                        </itemtemplate>
                    </asp:Repeater>
                    <asp:Label id="lblNoRecords" visible="false" runat="server" text="<%=Resources.Msg.review_no_record%>"></asp:Label>
            </div>
 <input type="hidden" id="hiCountdata" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSize" value="<%=PageSize %>" />
<input type="hidden" id="hidUrl" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center" id="imgloading">
        <img id="loader" alt="" src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmore" onclick="GetOrderRecords(0,pgsize);" class="see-more-items" style="display: none">
      <%=Resources.Msg.SeeMoreData%>
</div>
<script type="text/javascript" language="javascript">
    var pgsize = document.getElementById('hidPageSize').value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdata').value,
        isLoading = 0;
    $(window).scroll(function () {
        ScrollDataOrder();
    });
    if (pageCount - pageIndex * pgsize > 0 || pageIndex * pgsize - pageCount < pgsize) {
        $("#loadmore").show();
    }
    $("#narrowsearch").addClass("hide");
    $("#resource-center .content-rating").replaceWith("<div id='order-review' class='hidden-sm hidden-xs'>" + $("#dvRating .cate-filter-content").html() + "</div>");
  </script>         
 </asp:Content>