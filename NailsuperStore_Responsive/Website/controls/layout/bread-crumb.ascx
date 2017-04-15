<%@ Control Language="VB" AutoEventWireup="false" CodeFile="bread-crumb.ascx.vb" Inherits="Controls_Breadcrumb" %>

<div id="divBC" class="bread-crumb" runat="server">
    <nav id="bread-crumb-nav" data-toggle="offcanvas" style="display:none" >
        <div class="title">
            <span class="ico"></span>
            <h1>Customer Service</h1>
        </div>
        <div class="arrow"></div>
    </nav>
   <%=breadcrumbContent %>
   
    <ul id="bread-crumb-nav-content"  class="hidden-md hidden-lg">
    
   </ul>
</div>

<div style="clear:both;"></div>
<script type="text/javascript">
//    $(document).ready(function () {
//        var strRequest = window.location.href;
//        if (strRequest.match('members/orderhistory/view.aspx')) {
//            $('h1').text('My Account');
//        }
//    })
    function CheckShowBreadCrumbMenuPopup(menuId, menuname) {
        if (document.getElementById(menuId)) {
            $('#bread-crumb-nav .title h1').text(menuname);
            if (ViewPortVidth() <= 991) {
                var currentPage = window.location.href;
                if (currentPage.indexOf('search-result.aspx') >= 0) {
                    var countItemSearch = $('#hidCountProductSearch').val();
                    countItemSearch = parseInt(countItemSearch);
                    if (countItemSearch < 1) {
                        $('#bread-crumb-nav').css('display', 'none');
                        $(".ic-minus").addClass("hidden");
                        return;
                    }

                }
                <% If Request.Path.Contains("/shop-design/detail.aspx") Then %>
                    $('#bread-crumb-nav').css('display', 'none');
                    $('.shop-by-design li:first-child,.shop-by-design li:nth-child(2)').css('display', 'block'); 
                <%Else %>  
                    $('#bread-crumb-nav').css('display', 'block');    
                <% End If %>
            }
            else {
                $('#bread-crumb-nav').css('display', 'none');
            }
        }
    }
    $(window).load(function () {
        fnClicCusService();
        fnSetContentCusService();
        fnClickOffCusService();
    });
    var wait;
    $(window).resize(function () {
        clearTimeout(wait);
        wait = setTimeout(function () {
            fnClicCusService();
            fnSetContentCusService();
            fnClickOffCusService();
        }, 500);
    });

</script>