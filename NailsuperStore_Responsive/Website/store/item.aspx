<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item.aspx.vb" MasterPageFile="~/includes/masterpage/main.master" Inherits="class_item" %>

<%@ Register Src="~/controls/product/review-list.ascx" TagName="product" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/related-item.ascx" TagName="related" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/item-description.ascx" TagName="item" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/share-item-detail.ascx" TagName="share" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/image-item-detail.ascx" TagName="itemImage" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/video-item-detail.ascx" TagName="itemVideo" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/price-box-item-detail.ascx" TagName="priceCart" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <asp:Literal runat="server" ID="ltrImageSrc" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="item-detail" itemscope itemtype="http://schema.org/Product">
        <div id="up">
            <div class="header">
				<div itemprop="image">
                    <asp:Literal ID="divThumbImage" runat="server"></asp:Literal>
                </div>
                <h1 itemprop="name">
                    <asp:Literal runat="server" ID="litItemName"></asp:Literal>
                </h1>
               
                <div <%=AggregateRating %>>
                     <nav>
                        <ul>
                            <li class='reviewicon'>
                                <asp:Literal ID="ltrHeaderReview" runat="server"></asp:Literal>
                            </li>
                            <li class="countreview"><a href="#" onclick="return ScrollToSection('#item-detail .data .review-section');">
                                <asp:Literal ID="ltrHeaderReviewCount" runat="server"></asp:Literal></a> </li>
                            <li><a href="#" onclick="return ScrollToSection('#item-detail .data .description');">
                                Description</a> </li>
                            <li id="customer-also-bought">
                                <asp:Literal ID="ltrHeaderRelated" runat="server"></asp:Literal>
                            </li>
                            <li>
                                <asp:Literal ID="ltrInstructionHeader" runat="server"></asp:Literal>
                            </li>
                            <li><div class="sku"><asp:Literal runat="server" ID="litSKU"></asp:Literal></div></li>
                        </ul>
                    </nav>
                </div>
              
            </div>
              <span itemprop="sku" class="show-snippet"><%=Item.SKU %></span>
            <div class="header-fix">
            </div>
            <div class="item-main">
                <section class="overview">
                    <uc:itemImage ID="ucItemImage" runat="server" />
                </section>
                <div class="clearfix visible-sm">
                </div>
                <section class="cart" id="cart-section">
                    <uc:priceCart ID="ucPriceCart" runat="server" />
                </section>
            </div>
            <div class="data">
                <uc:share ID="ucShare" runat="server" />
                <uc:item ID="ucDescription" runat="server" />
                <section class="video" id="divVideo" runat="server">
                    <div class="label" id="lblHowToVideoTitle" runat="server">
                        How-To Video</div>
                    <div class="content">
                        <uc:itemVideo ID="ucItemVideo" runat="server" />
                    </div>
                </section>
                <section class="related" id="divRelated" runat="server">
                    <div class="label">
                        Customers Also Bought
                    </div>
                    <div class="content">
                        <uc:related ID="ucRelatedItem" runat="server" />
                    </div>
                </section>
                <section class="instruction" id="divInstruction" runat="server">
                    <div class="label">
                        Instructions
                    </div>
                    <div class="content">
                        <asp:Literal ID="ltrInstruction" runat="server"></asp:Literal>
                    </div>
                </section>
                <section class="review-section" id="review-section">
                    <div class="label">
                        Customer Reviews
                    </div>
                    <div class="content">
                        <uc:product ID="ucProductReview" runat="server" />
                    </div>
                </section>
            </div>
            <input type="hidden" id="hidItemGroupId" runat="server" value="" />
        </div>
    </div>
    <script type="text/javascript">
        var isFocusInput = false;
        $(document).ready(function () {
            $('input').bind('focus', function () {
                isFocusInput = true;
            });
            $('input').bind('blur', function () {
                isFocusInput = false;
            });
        });

        function AddCartDetail(itemId, type) {

            var qty = 0;
            if (type == 'case') {
                qty = $("#txtQtyCase").val();
            }
            else {
                qty = $("#txtQty").val();
            }

            if (!isInteger(qty) || parseInt(qty) < 1 || isNaN(parseInt(qty))) {
                ShowError('Please input a valid quantity');
                return;
            }

            if (type == 'case') {
                mainScreen.ExecuteCommand('AddCartCase', 'methodHandlers.AddCartCaseDetailCallBack', [itemId, qty, true]);
            }
            else {
                mainScreen.ExecuteCommand('AddCart', 'methodHandlers.AddCartDetailCallBack', [itemId, qty, true]);
            }
            //GAAddToCart(qty);
        }

        methodHandlers.AddCartDetailCallBack = function (htmlReturn, maxQty) {
            var error = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    error = htmlReturn[0];
                }
            }
            if (error.length > 0) {
                ShowError(error);
                maxQty = parseInt(maxQty);
                if (maxQty > 0) {
                    $('#txtQty').val(maxQty);
                }
                return;
            }

            window.location.href = '/store/cart.aspx';
        }

        methodHandlers.AddCartCaseDetailCallBack = function (htmlReturn, maxQty) {
            var error = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    error = htmlReturn[0];
                }
            }
            if (error.length > 0) {
                ShowError(error);
                maxQty = parseInt(maxQty);
                if (maxQty > 0) {
                    $('#txtQtyCase').val(maxQty);
                }
                return;
            }

            window.location.href = '/store/cart.aspx';
        }

        // Called when a product is added to a shopping cart.
//        function GAAddToCart(qty) {
//            
//            ga('ec:addProduct', {
//                'id': '<=Item.SKU >',
//                'name': '<=Item.ItemName >',
//                'category': '',
//                'brand': '',
//                'variant': '',
//                'price': '<=IIf(Item.LowSalePrice > 0, Item.LowSalePrice, Item.Price) >',
//                'quantity': qty
//            });
//            ga('ec:setAction', 'add', { 'step': 1 });
//            ga('send', 'event', 'UX', 'click', 'add to cart');     // Send data using an event.

//            
//        }
        function CalculatePointCase(unitPoint, lstPrice) {

            var qty = 0;
            if (document.getElementById('txtQtyCase')) {
                qty = $('#txtQtyCase').val();
            }
            else
                qty = 1;


            var indexSep = lstPrice.indexOf(',');
            var result = 0;
            if (indexSep < 1) {
                result = parseFloat(qty) * parseFloat(unitPoint) * parseFloat(lstPrice);

            }
            else {
                var arr = new Array();
                arr = lstPrice.split(',');
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var groupPrice = arr[i].toString();
                        if (groupPrice != '') {
                            var arr1 = new Array();
                            arr1 = groupPrice.split('-');
                            var minQty = parseInt(arr1[0].toString());
                            var maxQty = parseInt(arr1[1].toString());
                            var idPrice = 'trPriceCase' + i;
                            $('#' + idPrice).removeClass();
                            if (qty >= minQty && qty < maxQty) {
                                $('#' + idPrice).addClass('line selectCase');
                                var price = arr1[2].toString();
                                result = parseInt(qty) * parseInt(unitPoint) * parseFloat(price);
                                $('#buyInBulkPrice').text(price.toString());
                                $('#buyInBulkUnitPrice').text(arr1[4].toString());
                                $('#buyInBulkSavePc').text(arr1[3].toString());
                            }
                            else {
                                $('#' + idPrice).addClass('line');
                            }

                        }
                    }
                }
            }
            if (result < 1)
                result = 1;
            else {

                result = Math.ceil(result.toFixed(2));

            }
            $('#earnpointCase').text(result.toString());
        }
        function CalculatePoint(unitPoint, lstPrice) {
            
            var qty = 0;
            if (document.getElementById('txtQty')) {
                qty = $('#txtQty').val();
            }
            else
                qty = 1;


            var indexSep = lstPrice.indexOf(',');
            var result = 0;
            if (indexSep < 1) {
                result = parseFloat(qty) * parseFloat(unitPoint) * parseFloat(lstPrice);

            }
            else {
                var arr = new Array();
                arr = lstPrice.split(',');
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var groupPrice = arr[i].toString();
                        if (groupPrice != '') {
                            var arr1 = new Array();
                            arr1 = groupPrice.split('-');
                            var minQty = parseInt(arr1[0].toString());
                            var maxQty = parseInt(arr1[1].toString());
                            var idPrice = 'trPrice' + i;
                            $('#' + idPrice).removeClass();
                            if (qty >= minQty && qty <= maxQty) {
                                $('#' + idPrice).addClass('line select');
                                var price = arr1[2].toString();

                                result = parseInt(qty) * parseInt(unitPoint) * parseFloat(price);

                            }
                            else {
                                $('#' + idPrice).addClass('line');
                            }

                        }
                    }
                }
            }
            if (result < 1)
                result = 1;
            else {
            
                result = Math.ceil(result.toFixed(2));
                
            }
            $('#earnpoint').text(result.toString());
        }

        function ChangeQty(ctr, type, unitPoint, pricePoint) {

            if (type == 'up') {
                Increase(ctr);
            }
            else {
                Decrease(ctr, 1);
            }
            if (ctr == 'txtQty')
                CalculatePoint(unitPoint, pricePoint);
            else
                CalculatePointCase(unitPoint, pricePoint);
        }

        $(window).scroll(function () {
            FixPageItemDetail();
        });
        function FixPageItemDetail() {
          
            // var itemposition = $("#item-detail .data .description").position().top;

            if (isAllowFixPageItemDetail()) {
                $("#item-detail .header-fix").html($("#item-detail .header").html().replace(/itemprop/g, 'el').replace(/itemscope/g, 'is').replace(/itemtype/g, 'it'));
                var itemposition = $("#item-detail .header").position().top;
                if ($(window).scrollTop() >= itemposition) {
                    FixItemHeader();
                }
                else {
                    ClearFixItemHeader();
                }
                var itempositionCart = GetPositionFixCart(); // $("#item-detail #cart-section").position().top;
                if ($(window).scrollTop() >= itempositionCart) {
                    FixCartBox();
                }
                else {
                    ClearFixBoxCart();
                }
            }
            else {
                ClearFixItemHeader();
                ClearFixBoxCart();
            }
        }
        function isAllowFixPageItemDetail() {
           
            if (IsiPad()) {
                if (isFocusInput) {
                    return false;
                }
                return true;
            }
            return true;
        }
        function GetPositionFixCart() {
            var itempositionCart = $("#item-detail #cart-section").position().top;
            if (IsHasAttribute()) {
                var margintop = $("#ulAttribute .first").css("margin-top");
                margintop = margintop.replace('px', '');
                margintop = margintop.replace('-', '');
                itempositionCart = itempositionCart - parseInt(margintop);
            }
            else {
                var panelIconH = GetIconPanelHeight();
                if (panelIconH > 0) {
                    itempositionCart = itempositionCart - parseInt(panelIconH);
                }
                else {
                    itempositionCart = itempositionCart -10;
                }
            }
            return itempositionCart;
        }
        function GetIconPanelHeight() {
            if (document.getElementById('secIcon')) {
                var iConHeight = $('#secIcon').outerHeight();
                return parseInt(iConHeight) + 10;
            }
            return 0;
        }
        function ClearFixItemHeader() {

            $('#item-detail .header-fix').removeAttr('style')
            $("#item-detail .header-fix").css("display", "none");
        }
        function ClearFixBoxCart() {

            $("#item-detail .cart .page-wrapper").removeClass("page-wrapper-fix");
            $("#item-detail .cart .page-wrapper").removeClass("page-wrapper-absolute");
            $('#item-detail .cart .page-wrapper[style]').removeAttr('style')

        }
        function FixCartBox() {
            if (ViewPortVidth() <= 991) {
                ClearFixBoxCart();
                return;
            }
            var cartheight = $("#item-detail .cart .page-wrapper").outerHeight();
            var cartWidth = $("#item-detail .cart .page-wrapper").outerWidth();
            $("#item-detail .cart .page-wrapper").css("width", cartWidth + "px");
            var posFooter = $('#recentlyView').position().top;
            var footerMargin = $('#recentlyView').css("margin-top");
            footerMargin = footerMargin.replace('px', '');
            footerMargin = footerMargin.replace('-', '');
            footerMargin = parseInt(footerMargin);

            var carttotal = posFooter - cartheight;
            carttotal = carttotal - 10
            if ($(window).scrollTop() + cartheight >= (posFooter - 10)) {
                $("#item-detail .cart .page-wrapper").removeClass("page-wrapper-fix");
                $("#item-detail .cart .page-wrapper").addClass("page-wrapper-absolute");
                $("#item-detail .cart .page-wrapper").css("top", carttotal + "px");
            }
            else {
                // $('#divPoint').html('scrollTop():' + $(window).scrollTop() + ',cartheight:' + cartheight + ',<br/>posFooter:' + posFooter + ',fix');
                //$("#cartif").html('else');
                $("#item-detail .cart .page-wrapper").removeClass("page-wrapper-absolute");
                $("#item-detail .cart .page-wrapper").addClass("page-wrapper-fix");
                ResetPositionCartBox();
            }
        }
        function FixItemHeader() {
            if (ViewPortVidth() <= 767) {
                ClearFixItemHeader();
                return;
            }
            if (IsiPad()) {
                var posFooter = $('footer .header').position().top;
                var footerMargin = $('footer .header').css("margin-top");
                footerMargin = footerMargin.replace('px', '');
                footerMargin = footerMargin.replace('-', '');
                footerMargin = parseInt(footerMargin);
                var headerHeight = $("#item-detail .header-fix").outerHeight();
                var currentLeft = $("#item-detail .header-fix").position().left
                var top = posFooter - headerHeight - 10;
                if ($(window).scrollTop() + headerHeight >= (posFooter - 6)) {
                    $("#item-detail .header-fix").css({ "position": "absolute", "top": top + "px" });
                    
                }
                else {
                    top = 0;
                    $("#item-detail .header-fix").css({ "position": "fixed", "top": top + "px" });
                    ResetPositionHeaderItem();
                    $("#item-detail .header-fix").css("display", "block");
                    $("#item-detail .header-fix .thumb-image").css("display", "block");

                }
            }
            else {
                ResetPositionHeaderItem();
                // $("#item-detail .header").addClass("header-fix");
                $("#item-detail .header-fix").css("display", "block");
                $("#item-detail .header-fix .thumb-image").css("display", "block");
            }
            
        }
        function IsHasAttribute() {
            if (document.getElementById('ulAttribute')) {
                return true;
            }
            return false;
        }
        function ResetPositionHeaderItem() {

            if (ViewPortVidth() <= 991) {
                var nameWidth = $("#item-detail ").width();
                $("#item-detail .header-fix").css("width", nameWidth + "px");
                if (isFixPageHeader()) {
                    var headerHeight = $("#top-bar #menu-dept").height();
                    $("#item-detail .header-fix").css("top", headerHeight + "px");
                }
                else
                    $("#item-detail .header-fix").css("top", "0px");
                $("#item-detail .cart .page-wrapper").removeClass("page-wrapper-fix");
                $('#item-detail .cart .page-wrapper[style]').removeAttr('style')
            }
            else {
                var nameWidth = $("#item-detail .header").outerWidth();
                $("#item-detail .header-fix").css("width", nameWidth + "px");
                $("#item-detail .header-fix").css("top", "0px");


            }
        }
        function ResetPositionCartBox() {
            var top = 0;
            var paddingtop = 10;
            var iConHeight = GetIconPanelHeight();
            if (iConHeight > 0) {
                paddingtop = iConHeight;
            }
            var cartWidth = $("#item-detail .cart .page-wrapper").outerWidth();
            var cartLeft = $("#item-detail .cart .page-wrapper").position.left;
            $("#item-detail .cart .page-wrapper").css("width", cartWidth + "px");
            $("#item-detail .cart .page-wrapper").css("top", top.toString() + "px");
            $("#item-detail .cart .page-wrapper").css("left", cartLeft + "px");
            if (IsHasAttribute()) {               
                var margintop = $("#ulAttribute .first").css("margin-top");
                margintop = margintop.replace('px', '');
                margintop = margintop.replace('-', '');
                paddingtop = margintop;

            }
            if (paddingtop > 0) {
                $("#item-detail .cart .page-wrapper").css("padding-top", paddingtop + "px");
            }


        }

        function ResetCartSubmitPosition() {

            var nameWidth = $("#item-detail .header-fix").width();
            var cartWidth = $("#item-detail .cart .page-wrapper").outerWidth();
            var cartLeft = $("#item-detail .header-fix").position().left + nameWidth - cartWidth;
            $("#item-detail .cart .page-wrapper").css("left", cartLeft + 'px');
        }
        function isFixPageHeader() {
            return $("#top-bar").hasClass("fixed-top");
        }
        function isFixItemHeader() {
            //return $("#item-detail .header-fix").hasClass("header-fix");
            var display = $("#item-detail .header-fix").css('display');
            if (display == 'none')
                return false;
            return true;

        }
        function isFixCart() {
            return $("#item-detail .cart .page-wrapper").hasClass("page-wrapper-fix");


        }
        $(window).resize(function () {
            ChangeResize();
        });
        function ChangeResize() {
            ResetMainImageWidth();
            ClearFixBoxCart();
            FixPageItemDetail();
            CheckZoomImageResize();
            ResetHeightListHowToVideo();
            if (ViewPortVidth() < 767) {
                $("#item-detail #cart-section").css("height", "auto");
               
                return;
            }
        }
        function CheckZoomImageResize() {

            if (ViewPortVidth() <= 991) {//not resize
                $(".image-detail #imgSource").css("display", "block");
                $(".image-detail #wrap-zoom-image").css("display", "none");
            }
            else {
                var allowZoom = $(".image-detail #imgSource").attr('allowzoom');
                if (allowZoom == '1') {
                    $(".image-detail #imgSource").css("display", "none");
                    $(".image-detail #wrap-zoom-image").css("display", "block");
                    // $('.cloud-zoom, .cloud-zoom-gallery').CloudZoom();
                    BeginZoom();
                }
                else {
                    $(".image-detail #imgSource").css("display", "block");
                    $(".image-detail #wrap-zoom-image").css("display", "none");
                }


            }
        }
        function ScrollToSection(section) {
            var pos = $(section).offset().top;
            var headerHeight = $("#item-detail .header-fix").outerHeight();
            // alert(headerHeight);
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
        function PlayVideo(vid) {
            var link = '/includes/popup/product-video.aspx?vid=' + vid
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
        $(window).load(function () {
            CheckZoomImagePageLoad();
            ResetMainImageWidth();
           // $("#item-detail .header-fix").html($("#item-detail .header").html())
            ResetSizeBoxCart();
            // ResetRelatedHeight();           
            ResetHeightListHowToVideo();
          //  alert(ViewPortVidth());
        });
        function ResetMainImageWidth() {
//            var w = ViewPortVidth();
//            if (w < 640) {
//                w = w - 50;
//                $('#item-detail .overview .image').css('width', w + 'px');
//            }
//            else {
//                $('#item-detail .overview .image').removeAttr('style')
//            }
        }
        function ResetSizeBoxCart() {
            if (isFixCart())
                return;
            if (ViewPortVidth() < 767) {
                $("#item-detail #cart-section").css("height", "auto");
                return;
            }
            var cartHeight = $("#item-detail #cart-section").height();
            $("#item-detail #cart-section").css("height", cartHeight + "px");
        }
        function CheckZoomImagePageLoad() {
            var allowZoom = $(".image-detail #wrap-zoom-image").css('display');

            if (allowZoom == 'none') {
                $(".image-detail #imgSource").css("display", "block");
                $(".image-detail #wrap-zoom-image").css("display", "none");
                return;
            }
            if (ViewPortVidth() <= 991) {
                $(".image-detail #imgSource").css("display", "block");
                $(".image-detail #wrap-zoom-image").css("display", "none");
            }
            else {
                $(".image-detail #imgSource").css("display", "none");
                $(".image-detail #wrap-zoom-image").css("display", "block");
                BeginZoom();
            }
        }

        function ShowAddCardRelatedError(msg) {
            showQtip('qtip-error', msg, 'Error');
        }

        function ShowHandlingTip() {
            showQtip('qtip-msg', 'Special Handling Fee covers items that are heavy, oversized or requires special packing and handling.', 'Special Handling Fee');
        }
        function ShowFlammableTip() {
            showQtip('qtip-msg', '<%=Resources.Msg.ItemFlammable %>', 'Flammable & Hazardous Material Item');
                }
        function ShowCashPointTip(content) {
            showQtip('qtip-msg', content, 'Cash Reward Points Program');
        }

        function AddCart() {
            return IsCookieEnable();
        }
        function SelectItemGroup(groupID, optionId, choiceId, hidListOptionId, lstChoiceId) {
            var arr = new Array();
            arr = lstChoiceId.split(';');
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].toString() != '') {
                        var n = arr[i].toString().indexOf(optionId + ",");
                        if (n > -1) {
                            lstChoiceId = lstChoiceId.replace(arr[i].toString(), optionId + "," + choiceId);

                            mainScreen.ExecuteCommand('ChangeItemGroup', 'methodHandlers.ChangeItemGroupCallBack', [groupID, hidListOptionId, lstChoiceId, choiceId]);
                        }
                    }
                }
            }

        }
        methodHandlers.ChangeItemGroupCallBack = function (htmlReturn) {
            var error = '';
            var itemName = '';
            var htmlImageDetail = '';
            var htmlVideo = '';
            var htmlPriceBox = '';
            var htmlDescription = '';
            var sku = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    error = htmlReturn[0];
                    if (error != '') {
                        ShowError(error);
                        return;
                    }
                    itemName = htmlReturn[1];
                    htmlImageDetail = htmlReturn[2];
                    htmlVideo = htmlReturn[3];
                    htmlPriceBox = htmlReturn[4];
                    htmlDescription = htmlReturn[5];
                    sku = htmlReturn[6];
                    $('.header h1').html(itemName);
                    $('.sku').html("Item #" + sku);
                    $("#item-detail .header-fix").html($("#item-detail .header").html().replace(/itemprop/g, 'el').replace(/itemscope/g, 'is').replace(/itemtype/g, 'it'));
                    $('.item-main .overview').html(htmlImageDetail);
                    $('#cart-section').html(htmlPriceBox);
                    $('.data .description').html(htmlDescription);

                    if (htmlVideo != '') {
                        $('#divVideo').css('display', 'block');
                        $('#divVideo .content').html(htmlVideo);
                    }
                    else {
                        $('#divVideo').css('display', 'none');
                    }
                    // FixPageItemDetail();
                    CheckZoomImagePageLoad();
                    ResetHeightListHowToVideo();
                    ClearFixBoxCart()
                    FixPageItemDetail();
                }
            }
        }

        function ShowBonusOffer(id) {
            var link = '/includes/popup/special-offer.aspx?mmid=' + id

            var heightPopup = 490;
            var popupW = 850;

            $('#nyroModal').attr('href', link);
            $('#nyroModal').nyroModalManual({
                showCloseButton: true,
                bgColor: 'gray',
                zIndexStart: '9999999',
                modal: false,
                width: popupW,
                height: heightPopup,
                windowResize: true,
                titleFromIframe: false
            });

        }
        function ResetRelatedHeight() {

            if (document.getElementById('divRelated')) {

                equalheight('#list-item .item', 0);
            }
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
       function ShowPolicyPopup(policyid, title) {
            var content = $('#hidPolicyContent' + policyid)[0].value;
            showQtip('qtip-msg', content, title);
            $('#qtip-blanket').click(function () {
                $('#qtip-blanket').css('display', 'none');
                $('.qtip-active').css('display', 'none');
                $('.qtip-wrapper .qtip-content').removeClass('qtip-content');
            });
       }
    </script>
</asp:Content>
