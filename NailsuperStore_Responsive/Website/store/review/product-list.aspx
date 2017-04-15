<%@ Page Language="VB" AutoEventWireup="false" CodeFile="product-list.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="store_productreviews" %>
<%@ Register Src="~/controls/product/review.ascx" TagName="product" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="item-detail">
<div id="content-left" style="display:none"></div> 
    <section class="review-section" id="review-section">
    <div class="content">
    <h1>Product Reviews</h1>
    <div class="review-list">
             <div class="content-review-list" id="content-review-list">
                <asp:Literal ID="ltrReviewList" runat="server"></asp:Literal>
            </div>
            <div id="divMoreReview" runat="server" onclick="return GetMoreReview();" class="see-more-items" style="display: inline">
                <%=Resources.Msg.SeeMoreData%>
            </div>
        <input type="hidden" id="hidPageIndex" value="<%=pgIndex %>" /> 
        <input type="hidden" id="hidpageSize" value="<%=PageSize %>" /> 
        <input type="hidden" id="hidUrl" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
         <input type="hidden" id="hiCountdata" value="<%=TotalRecords %>" />
        <div>
        <asp:Label ID="lblNoRecords" runat="server" text="" visible="false"></asp:Label>
        </div>
    </div>
    </div>
    </section>
</div>
<script type="text/javascript">
    var pgsize = $('#hidpageSize').val(),
        pageIndex = $('#hidPageIndex').val(),
        pageCount = $('#hiCountdata').val(),
        isLoading = 0;
    $(window).scroll(function () {
        ScrollReview();
    });
    function ScrollReview() {
        //alert("aa");
        var val1 = ",100,200,300,400,500";
        var hscroll = $(window).scrollTop(),
        hdoc = $("#content-page").height() + $("header").height(); // -$("footer").height(); //$(document).height() - $(window).height() - $("footer").height();
        if(hdoc.toString().indexOf(".") != -1) {
            hdoc = hdoc.toString().substr(0, hdoc.toString().indexOf("."));
        }
        var hscroll = hscroll + $("footer").height() + $("header").height() + 200,
        val = 00,
        lsroll2 = hscroll.toString().substr(hscroll.toString().length - 2, 2),
        ldoc2 = hdoc.toString().substr(hdoc.toString().length - 2, 2),
        eqhscroll = hscroll - lsroll2 + 100,
        eqhdoc = hdoc - ldoc2;
        var browser = navigator.userAgent.toLowerCase();
        //alert(hdoc + "--" + topmore + "--" + lsroll2 + "--" + eqhscroll + "--" + eqhdoc + "--" + ltopmore + "--" + etopmore + "--" + lmore + "--" + eqmore);
        if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {
           if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll && isLoading == 0) {
                isExistScroll = eqhscroll;
                isLoading = 1;
                if ((pgsize * pageIndex <= pageCount)) {
                    GetMoreReview()
                }
            }
        }
        else {
            //alert(eqhscroll - eqhdoc);
           if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll && isLoading == 0) {
                isExistScroll = eqhscroll;
                isLoading = 1;
                if ((pgsize * pageIndex <= pageCount)) {
                    GetMoreReview()
                }
            }
        }

    }
    function GetMoreReview() {

        $('#divMoreReview').html('<img src="<%= Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif">');
        GetReviewRecordsAjax();
    }
    function GetReviewRecordsAjax() {
        var url = document.getElementById('hidUrl').value,
        arrurl = url.split("?");
        if (arrurl.length > 1)
            url = arrurl[0] + "/GetMoreData?" + arrurl[1];
        else
            url = arrurl[0] + "/GetMoreData";
        pageIndex++;
               
        if ((pgsize * pageIndex <= pageCount || pgsize * pageIndex - 12 <= pageCount)) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + '}',
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
        
    }
    function BindMoreReviewData(response) {
        var reviewHTML = response.d[0];
        var allowViewMore = response.d[1];
        if (reviewHTML != '') {
            var html = '';
            //if (isSort != 1) {
                html = $('#content-review-list').html() + reviewHTML;
           // }
            //else
               // html = reviewHTML;
            $('#content-review-list').html(html);
            if (allowViewMore == 'False') {
                $('#divMoreReview').css("display", "none");
            }
            else {
                $('#divMoreReview').html('See More Review >>');
             }
            //$('#hidPageIndex').val(response.d[2]);
        }
        isLoading = 0;
    }
</script>
</asp:Content>
