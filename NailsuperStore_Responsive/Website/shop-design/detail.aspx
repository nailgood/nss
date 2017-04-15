<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detail.aspx.vb" MasterPageFile="~/includes/masterpage/main.master" Inherits="shop_design_detail" %>

<%@ Register src="~/controls/product/item-description.ascx" tagname="item" tagprefix="uc" %>
<%@ Register src="~/controls/resource-center/review.ascx" tagname="review" tagprefix="uc" %>
<%@ Register Src="~/controls/layout/addthis.ascx" TagName="share" TagPrefix="uc1" %>
<%@ Register Src="~/controls/layout/menu/shop-by-design.ascx" TagName="shopbydesign" TagPrefix="uchideShopByDesign" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <%--<link rel="stylesheet" type="text/css" href="/includes/Theme/css/shopdesign.css" />
    <link rel="stylesheet" type="text/css" href="/includes/Theme/css/resource-center.css" />Edit css.xml--%>
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
<%--    <link rel="stylesheet" type="text/css" href="/includes/Theme/css/pager.css" />--%>
   
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="categoryhide" style="display:none;">
        <uchideShopByDesign:shopbydesign ID="shopbydesign" runat="server" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            CheckShowBreadCrumbMenuPopup('categoryhide', 'Shop by Design');
        });
        $(window).resize(function () {
            CheckShowBreadCrumbMenuPopup('categoryhide', 'Shop by Design');
        });
</script>
    <div id="shop-design">
        <div class="header">
            <h1><asp:Literal runat="server" ID="litShopDesignName"></asp:Literal></h1>
            <nav>
                <ul>
                    <li class='reviewicon'>
                        <asp:Literal ID="ltrHeaderReview" runat="server"></asp:Literal>
                    </li>
                    <li class="countreview">
                        <a href="#" onclick="return ScrollToSection('#shop-design .review-section');">
                        <asp:Literal ID="ltrHeaderReviewCount" runat="server"></asp:Literal></a>
                    </li>
                        <li>
                        <a href="#" onclick="return ScrollToSection('#shop-design .description');">Description</a>
                    </li>
                        <li>
                        <asp:Literal ID="ltrHeaderRelated" runat="server"></asp:Literal>
                    </li>
                        <li>
                        <asp:Literal ID="ltrInstructionHeader" runat="server"></asp:Literal>
                    </li>
                </ul>                    
            </nav>
        </div>
        <div class="header-fix"></div>
        <div class="clearboth"></div>

        <div class="item-main">
            <section class="overview">
                <div class="image">
                    <div class="image-detail" runat="server" id="divImage" enableviewstate="true"></div> 
                </div>
                <div class="img-panel">
                    <asp:Literal ID="ltrImageList" runat="server"></asp:Literal>
                </div>
            </section>
            <section class="cart" id="cart-backup"></section>
            <%--<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-520b2d5b4eb4b59a"></script>--%>
            <section class="share">  
                <uc1:share ID="ucShare" runat="server" />
                <%--<div  class="addthis_toolbox addthis_default_style addthis_20x20_style"  addthis:url='<%=AddthisAssociatedUrl%>' addthis:title='<%=AddthisAssociatedName%>' >
                     <a class="addthis_button_facebook"></a>
                    <a class="addthis_button_twitter"></a>          
                    <a class="addthis_button_pinterest_share"></a>
                    <a class="addthis_button_google_plusone_share"></a>
                </div>--%>
            </section>            
            <section class="description">
                <div class="label">Description</div>
                <div class="content">
                    <asp:Literal ID="ltrDescription" runat="server"></asp:Literal>
                </div>
            </section>
            <section class="video" id="divVideo" runat="server">
                <div class="label">How-To Video</div>
                <div class="content">
                        <asp:Repeater ID="rptVideo" runat="server" >
                        <HeaderTemplate><ul class="video-list"></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="ltrVideo" runat="server"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate></ul></FooterTemplate>
                    </asp:Repeater>
                </div>
            </section>
            <section class="instruction" id="divInstruction" runat="server">
                <div class="label">Instructions</div>
                <div class="content"><asp:Literal ID="ltrInstruction" runat="server"></asp:Literal></div>
            </section>
            <section class="review-section" id="review-section">
                <div class="label">Customer Reviews</div>
                <div class="content">
                    <uc:review runat="server" ID="ucReview" Type="ShopDesign" />
                </div>
            </section>
            <div class="clearfix visible-sm"></div>
        </div>

        <section class="cart" id="cart-section">
            <div class="page-wrapper" id="cart-item">
            
            <asp:Repeater id="rptListItem" runat="server">
                <ItemTemplate>
            <div class="boxitem">
                <div class="title"><asp:Literal ID="litItemName" runat="server"></asp:Literal></div>
                <div class="iteminfo">
                    <div id="divAddCart" class="addcart" runat="server">
                        <div class="labelqty">Qty</div>
                        <div class="qty"><asp:Literal ID="litQtyPlusMin" runat="server"></asp:Literal><div class="pull-left"><asp:Literal ID="litQtyInput" runat="server"></asp:Literal></div><div id="arrow-qty" class="bao-arrow"><ul><asp:Literal ID="litQtyArrow" runat="server"></asp:Literal></ul></div></div>
                        <asp:Literal ID="litAction" runat="server"></asp:Literal>
                    </div>
                    <asp:Literal ID="divCart" runat="server"></asp:Literal>
                    <div class="image"><asp:Literal ID="litItemImage" runat="server"></asp:Literal></div>
                    <div class="g-price">
                        <ul>
                            <li class="price">
                                 <asp:Literal runat="server" ID="lblPrice"></asp:Literal>
                            </li>
                            <asp:Literal ID="ltsave" runat="server"></asp:Literal>
                        </ul>
                    </div>
                    <div class="promotion" id="divPromo">
                        <asp:Literal runat="server" ID="litPromotionText"></asp:Literal>
                    </div>
                    <div runat="server" id="divFlammable" class="red" visible="false">This item is not available for customer outside of 48 states within continental USA.</div>
                </div>
            </div>
                </ItemTemplate>
            </asp:Repeater>
            <%--<div id="cartmsg"></div>--%>
            </div>
        </section>
    </div>
    
    <script type="text/javascript">
        $(window).resize(function () {
            ChangeResize();
        });
        $(window).scroll(function () {
//            var scrollBottom = $(window).scrollTop() + $(window).height();
//            var footertop = $("footer").position().top;
//            var cartheight = $("#cart-item").height();
//            var carttop = $("#cart-item").position().top;
//            var carttotal = parseInt(footertop) - parseInt(cartheight);
//            var headerfix = $("div.header-fix").position().top;
//            $("#cartmsg").html($(window).height() + ' | scrollBottom=' + scrollBottom + ' | window.top= ' + $(window).scrollTop()  + ' | footertop=' + footertop + ' | cartheight=' + cartheight + ' | headerfix= ' + headerfix + ' | carttop=' + carttop + ' | carttotal=' + carttotal);          

            FixPageItemDetail();
        });
        function ChangeResize() {
            BackupCart();
            ClearFixBoxCart();
            FixPageItemDetail();
            ResetHeightListHowToVideo();
            
            
        }
        function BackupCart() {
            if (ViewPortVidth() <= 991) {
                if ($("#cart-backup").html().length < 2) {
                    $("#cart-backup").html($("#cart-section").html());
                    $("#cart-backup").css("display", "block");

                    $("#cart-section").html('');
                    $("#cart-section").css("display", "none");
                }
            }
            else {
                if ($("#cart-section").html().length < 2) {
                    $("#cart-section").html($("#cart-backup").html());
                    $("#cart-section").css("display", "block");

                    $("#cart-backup").html('');
                    $("#cart-backup").css("display", "none");
                }
            }
        }
        function FixItemHeader() {
            if (ViewPortVidth() <= 767) {
                ClearFixItemHeader();
                return;
            }
            ResetPositionHeaderItem();
            // $("#shop-design .header").addClass("header-fix");
            $("#shop-design .header-fix").css("display", "block");
            if (ViewPortVidth() <= 991) {
                return;
            }
        }
        function ResetPositionCartBox() {

            var cartWidth = $("#cart-section").outerWidth() - 20;
            var cartLeft = $("#shop-design .cart .page-wrapper").position().left;
            $("#shop-design .cart .page-wrapper").css("width", cartWidth + "px");
            $("#shop-design .cart .page-wrapper").css("top", "10px");
            $("#shop-design .cart .page-wrapper").css("left", cartLeft + "px");
        }
        function ResetSizeBoxCart() {
            if (isFixCart())
                return;
            var cartHeight = $("#shop-design #cart-section").height();
            $("#shop-design #cart-section").css("height", cartHeight + "px");
        }
        function isFixCart() {
            return $("#shop-design .cart .page-wrapper").hasClass("page-wrapper-fix");
        }
        function FixPageItemDetail() {
            // var itemposition = $("#shop-design .data .description").position().top;
            var itemposition = $("#shop-design .header").position().top;
           
            if ($(window).scrollTop() >= itemposition) {
                FixItemHeader();
            }
            else {
                ClearFixItemHeader();
            }

            if ($(window).height() > $("#cart-item").height()) {
                //alert('1');
                var itempositionCart = $("#shop-design #cart-section").position().top;
                if ($(window).scrollTop() >= itempositionCart) {
                    FixCartBox();
                }
                else {
                    ClearFixBoxCart();
                }
            }
        }
        function ClearFixItemHeader() {

            $('#shop-design .header-fix').removeAttr('style')
            $("#shop-design .header-fix").css("display", "none");
        }
        function ClearFixBoxCart() {

            $("#shop-design .cart .page-wrapper").removeClass("page-wrapper-fix");
            $('#shop-design .cart .page-wrapper[style]').removeAttr('style')

        }
        function ScrollToSection(section) {
            var pos = $(section).offset().top;
            var headerHeight = $("#shop-design .header-fix").outerHeight();
            if (ViewPortVidth() <= 767) {
                headerHeight = 0;
            }
            pos = (pos - headerHeight);
            if (ViewPortVidth() <= 991) {//not resize
                if (isFixPageHeader()) {
                    pos = (pos - 40);
                }
                else {
                    pos = (pos - 70);
                }
            }
            $('html,body').animate({
                scrollTop: pos
            },
            'slow');
            return false;
        }
        function FixCartBox() {
            if (ViewPortVidth() <= 991) {
                ClearFixBoxCart();                
                return;
            }

            var scrollBottom = $(window).scrollTop() + $(window).height();
            var footertop = $("footer").position().top;
            var cartheight =  $("#cart-item").height();
            var carttop = $("#cart-item").position().top;
            var carttotal = parseInt(footertop) - parseInt(cartheight);

            if ((footertop - $(window).scrollTop() < cartheight)) {
                //$("#cartif").html('if');
                $("#shop-design .cart .page-wrapper").removeClass("page-wrapper-fix");
                $("#shop-design .cart .page-wrapper").addClass("page-wrapper-absolute");
                $("#shop-design .cart .page-wrapper").css("top", carttotal + "px");
            }
            else {
                //$("#cartif").html('else');
                $("#shop-design .cart .page-wrapper").removeClass("page-wrapper-absolute");
                $("#shop-design .cart .page-wrapper").addClass("page-wrapper-fix");
                ResetPositionCartBox();
            }

            
        }
        function ResetPositionHeaderItem() {

            if (ViewPortVidth() <= 991) {
                var nameWidth = $("#shop-design").width();

                $("#shop-design .header-fix").css("width", nameWidth + "px");
                if (isFixPageHeader()) {
                    var headerHeight = $("#top-bar #menu-dept").height();
                    $("#shop-design .header-fix").css("top", headerHeight + "px");
                }
                else
                    $("#shop-design .header-fix").css("top", "0px");
                $("#shop-design .cart .page-wrapper").removeClass("page-wrapper-fix");
                $('#shop-design .cart .page-wrapper[style]').removeAttr('style')
            }
            else {
                var nameWidth = $("#shop-design .header").outerWidth();

                $("#shop-design .header-fix").css("width", nameWidth + "px");
                $("#shop-design .header-fix").css("top", "0px");

            }
        }
        function isFixPageHeader() {
            return $("#top-bar").hasClass("fixed-top");
        }
        function isFixItemHeader() {
            //return $("#shop-design .header-fix").hasClass("header-fix");
            var display = $("#shop-design .header-fix").css('display');
            if (display == 'none')
                return false;
            return true;

        }
        $(window).load(function () {
            $("#shop-design .header-fix").html($("#shop-design .header").html())
            ResetSizeBoxCart();
            ResetHeightListHowToVideo();
            BackupCart();
        });
        function PlayVideo(id) {
            var link = '/includes/popup/shop-design-video.aspx?id=' + id
            var heightPopup = 580;
            $('#nyroModal').attr('href', link);
            $('#nyroModal').nyroModalManual({
                showCloseButton: true,
                bgColor: 'gray',
                zIndexStart: '9999999',
                modal: false,
                width: 760,
                height: heightPopup,
                windowResize: true,
                titleFromIframe: false
            });
        }

        function updateAlternateimg(imgPath, zoomPath, image, allowZoom) {

            $(".image-detail #imgSource img").attr("src", imgPath + '/' + image);
            //update panel zoom
            var rel = $(".image-detail .cloud-zoom").attr('href');
            $(".image-detail .cloud-zoom").attr('href', zoomPath + image);
            $(".image-detail .cloud-zoom #largeImage").attr("src", imgPath + '/' + image);
            if (ViewPortVidth() <= 991) {//not resize
                $(".image-detail #imgSource").css("display", "block");
                $(".image-detail #wrap-zoom-image").css("display", "none");
                return;
            }
            //  $('.cloud-zoom, .cloud-zoom-gallery').CloudZoom();

            if (allowZoom == 1) {
                $(".image-detail #imgSource").css("display", "none");
                $(".image-detail #wrap-zoom-image").css("display", "block");
                BeginZoom();
            }
            else {
                $(".image-detail #imgSource").css("display", "block");
                $(".image-detail #wrap-zoom-image").css("display", "none");
            }
            $(".image-detail #imgSource").attr('allowzoom', allowZoom);
        }
        function ResetHeightListHowToVideo() {
            if (!document.getElementById('divVideo')) {
                return;
            }
            var lstchild = $("#divVideo .video-list").children('.video-item');
            var lstReset = new Array();
            var defaultTop = -1;
            var maxRowHeight = 0;
            var $el;
            $(lstchild).each(function () {
                $el = $(this);
                //alert($el.html());
                $($el).height('auto');
                var position = $el.position();
                if (position.top != defaultTop) {

                    defaultTop = position.top;
                    if (lstReset.length > 0) {
                        ResetHeightVideoRow(lstReset, maxRowHeight);
                        lstReset.length = 0;
                        defaultTop = $el.position().top;
                    }
                    //else AddShopSaveBorderLeft($el)
                    maxRowHeight = 0;
                    if ($el.height() > maxRowHeight)
                        maxRowHeight = $el.height();
                    lstReset.push($el);
                }
                else {
                    if ($el.height() > maxRowHeight)
                        maxRowHeight = $el.height();
                    lstReset.push($el);
                }
            })
            if (lstReset.length > 0) {
                ResetHeightVideoRow(lstReset, maxRowHeight);
            }
        }
        function ResetHeightVideoRow(lstReset, maxRowHeight) {
            for (var j = 0; j < lstReset.length; j++) {
                lstReset[j].height(maxRowHeight);
            }
            //lstReset[lstReset.length - 1].addClass('noneborderright');

        }
    </script>
</asp:Content>
