<%@ Control Language="VB" AutoEventWireup="false" CodeFile="review-list.ascx.vb" Inherits="controls_product_review_list" %>
<%@ Register Src="review.ascx" TagName="product" TagPrefix="uc1" %>

<%--<link href="/includes/Theme/css/pager.css" rel="stylesheet" type="text/css" />--%>
<asp:Button ID="btnSortReview" CssClass="hiddenButton" runat="server" />
<asp:Button ID="btnGetMoreItem" CssClass="hiddenButton" runat="server" />
<input type="hidden" id="hidUrl" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<ul class="sumary" id="countReview" runat="server">
    <li>
        <asp:Literal ID="ltrReview" runat="server"></asp:Literal>
    </li>
    <li class="countreview"><a href="javascript:void(0)">
        <asp:Literal ID="ltrReviewCount" runat="server"></asp:Literal>
    </a></li>
    <li class="border"><span>&nbsp;</span> </li>
    <li class="writereview"><a href="/store/review/product-write.aspx?ItemId=<%=hidItemId.value %>">Write
        a review</a> </li>
</ul>
<div class="review-list">
    <input type="hidden" id="hidItemId" runat="server" />
    <ul class="sort" id="divSort" runat="server">
        <li class="text">Sort by: </li>
        <li class="">
            <div class="nf-dropdown">
                <asp:DropDownList ID="drlSort" onChange="return ChangeSort(this.value)" runat="server" CssClass="form-control"
                    AutoPostBack="False">
                    <asp:ListItem Text="Newest" Value="DateAdded"></asp:ListItem>
                    <asp:ListItem Text="Top Reviews" Value="NumStars"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </li>
    </ul>
    <div class="content-review-list" id="content-review-list">
        <asp:Literal ID="ltrReviewList" runat="server"></asp:Literal>
    </div>
    <div id="divMoreReview" runat="server" onclick="return GetMoreReview();" class="see-more-items"
        style="display: inline">
        See More Review >>
    </div>
    <input type="hidden" value="" id="hidPageIndex" runat="server" />
    
</div>
<script type="text/javascript">
    var isSort = 1;
    function ChangeSort(sortField) {
        isSort = 1;
        if (document.getElementById("loading")) {
            document.getElementById("loading").style.display = 'block';
        }
        GetReviewRecordsAjax(sortField);
    }
    function GetMoreReview() {
        isSort = 0;
        var sortField = $('#drlSort').val();
        $('#divMoreReview').html('<img alt="Waiting" src="<%= Utility.ConfigData.CDNmediapath %>/includes/theme/images/loader.gif">');
        GetReviewRecordsAjax(sortField);
    }
    function GetReviewRecordsAjax(sortField) {
       
        var pageIndex = $('#hidPageIndex').val();
        var itemId = $('#hidItemId').val();
        var url = document.getElementById('hidUrl').value,
        arrurl = url.split("?");
        if (arrurl.length > 1)
            url = arrurl[0] + "/GetMoreData?" + arrurl[1];
        else
            url = arrurl[0] + "/GetMoreData";
          $.ajax({
            type: "POST",
            url: url,
            data: '{pageIndex:' + pageIndex + ',itemId:' + itemId + ",sortField:'" + sortField + "',isSort:" + isSort + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: BindMoreReviewData,
            failure: function (response) {
               // alert(response.d);
            },
            error: function (response) {
                //alert("error");
                //                           alert(response.d);
            }
        });
    }
    function BindMoreReviewData(response) {
        var reviewHTML = response.d[0];
        var allowViewMore = response.d[1];
        if (reviewHTML != '') {
            var html = '';
            if (isSort != 1) {               
                html = $('#content-review-list').html() + reviewHTML;
            }
            else
                html = reviewHTML;           
            $('#content-review-list').html(html);
            if (allowViewMore=='False') {
                $('#divMoreReview').css("display", "none");
            }
            $('#hidPageIndex').val(response.d[2]);
        }
        if (isSort != 1) {
            $('#divMoreReview').html('See More Review >>');
        }
        else {
            if (document.getElementById("loading")) {
                document.getElementById("loading").style.display = 'none';
            }
        }
        
    }

    
  
</script>
