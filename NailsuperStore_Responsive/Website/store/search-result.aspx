<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false"
    CodeFile="search-result.aspx.vb" Inherits="store_searchresult" %>

<%@ Register TagName="Filter" TagPrefix="CC" Src="~/controls/product/Filter.ascx" %>
<%@ Register Src="~/controls/resource-center/video/video-list.ascx" TagName="video"
    TagPrefix="uclstvideo" %>
<%@ Register Src="~/controls/product/product-list.ascx" TagName="product" TagPrefix="uc1" %>
<%@ Register Src="~/controls/layout/tab-search.ascx" TagName="tabSearch" TagPrefix="ucTabSearch" %>
<%@ Register Src="~/controls/product/store-browser.ascx" TagName="storeBrowser" TagPrefix="ucstoreBrowser" %>
<%@ Import Namespace="Utility" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<script type="text/javascript">
    function AddCartFromList(itemId)
    {
        $('#btnAddCart' + itemId).addClass('btn-active');
        $('#btnAddCart' + itemId).closest('.add-cart').closest('article').addClass('active');

        mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', [itemId, GetQty(itemId),false]);
    }
    </script>
    <div id="nct-fltr" class="pull-right">
    </div>
    <h1 class="c-h1" id='<%=IIf(Not String.IsNullOrEmpty(replaceKeywordName), "box-search-replace", "") %>'>
        <p>
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
        </p>
    </h1>
    <div class="line" style="clear: both">
    </div>
    <div id="content-left" class="hidden-md hidden-lg"></div> 
    <div id="fct-fltr">
        <CC:Filter ID="fltr" runat="server"></CC:Filter>
    </div>
    <div class="tabs-content">
        <ucTabSearch:tabSearch ID="ucTabSearch" runat="server" />
    </div>
    <div id="content-tab">
        <div id="pnProduct" runat="server">
            <div id="uplist">
                <div id="line-fix" class="ver-line">
                    &nbsp;</div>
                <div id="list-item">
                    <div class="ver-line-group">
                        &nbsp;</div>
                </div>
                <div id="loadmore" class="bg-loading2" style="display:none;border:solid 1px #dadada;width:100%;margin-top:10px;padding-top:20px">
                    <img src="/includes/theme/images/loader.gif" alt="" />  Please wait to see more items...<br />
                    <div id="notest"></div>
                </div>
            </div>
            <div id="divNoresult" class="noresult" visible="false" runat="server">
                <div class="mean" id="divSuggest" runat="server">
                    Did you mean:<asp:Literal ID="ltrMean" runat="server"></asp:Literal>
                    ?
                </div>
                <div class="msg">
                    No items were found that match your search criteria.
                </div>
                <ul class="help">
                    <li class="help-content">
                        <div class="need-help">
                            <img src="../includes/theme/images/NeedHelp.gif" />
                            <div class="title">
                                Need Help?</div>
                            <div class="available">
                                We're available Mon-Fri, 9:00 AM to 5:30 PM CST
                            </div>
                        </div>
                        <div class="phone">
                            <span class="icon">&nbsp;</span> <span class="text">Phone</span>
                        </div>
                        <div class="phone-us">
                            1-800-669-9430 Inside US
                        </div>
                        <div class="phone-int">
                            001-847-260-4000 Outside US
                        </div>
                    </li>
                    <li class="tip">
                        <div class="title">
                            Search Tips
                        </div>
                        <ul>
                            <li><span>Try alternative selections or spellings</span></li>
                            <li><span>Broaden your search by entering fewer keywords. (You can always refine your
                                search later)</span></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div id="pnArticle" style="display: none;" runat="server">
            <div id="pnArticleContent">
                <div class="group-article" id="dvTip" runat="server">
                    <div class="name">
                        Expert Tips & Advice</div>
                    <asp:Literal ID="ltrTipData" runat="server"></asp:Literal>
                </div>
                <div class="group-article" id="dvNew" runat="server">
                    <div class="name">
                        News & Events</div>
                    <asp:Literal ID="ltrNewData" runat="server"></asp:Literal>
                </div>
                <div class="group-article" id="dvMediaPress" runat="server">
                    <div class="name">
                        Media Press</div>
                    <asp:Literal ID="ltrMediaPressData" runat="server"></asp:Literal>
                </div>
            </div>
            <div style="display: block; text-align: center; clear: both; display: none;" id="imgloadingArticle">
                <img id="loaderArticle" alt="" src="/includes/theme/images/loader.gif" />
            </div>
            <div id="loadmoreArticle" onclick="GetMoreArticleSearch();" class="see-more-items"
                style="display: none">
                <%=Resources.Msg.SeeMoreData%>
            </div>
        </div>
        <div id="pnVideo" style="display: none;" runat="server">
            <div id="lstvideo">
                <uclstvideo:video ID="ucListVideo" runat="server" />
            </div>
            <div style="display: block; text-align: center; clear: both; display: none;" id="imgloadingVideo">
                <img id="loaderVideo" alt="" src="/includes/theme/images/loader.gif" />
            </div>
            <div id="loadmoreVideo" onclick="GetMoreVideoSearch();" class="see-more-items" style="display: none">
                <%=Resources.Msg.SeeMoreData%>
            </div>
        </div>
    </div>
    <input type="hidden" value="" id="hidKeyword" runat="server" />
    <input type="hidden" id="hidAllowSrollProduct" value="0" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidTotalVideo" value="" />
    <input type="hidden" id="hidPageSizeVideo" value="<%=Utility.ConfigData.PageSizeScroll.ToString() %>" />
    <input type="hidden" id="hidPageIndexVideo" value="1" />
    <input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
    <input type="hidden" id="hiCountdataVideo" value="" />
    <input type="hidden" id="hidPageSizeArticle" value="<%=Utility.ConfigData.PageSizeScroll.ToString() %>" />
    <input type="hidden" id="hidPageIndexArticle" value="1" />
    <input type="hidden" id="hidArticleType" value="0" />
    <script type="text/javascript">
        $(window).load(function () {
            if (window.ViewPortVidth() <= 991) {
                $('li > .left-nav').css('display', 'none');
                if($('#filter').text() == 'null')
                    $('#content-left').css('display', 'none')
            }
            else
                $('li > .left-nav').css('display', 'block');

        });
        $(window).resize(function () {
            if (window.ViewPortVidth() <= 991) {
                $('li > .left-nav').css('display', 'none');
                if ($('#filter').text() == 'null')
                    $('#content-left').css('display', 'none')
            }
            else
                $('li > .left-nav').css('display', 'block');
        });

        var arrParam = '<asp:Literal ID="litParam" runat="server"></asp:Literal>';
        var arrProduct = [];
        var arrArrange = [];
        var oldwidth = 0;
        var checkWorking = 0; //0=initial, 1=working, 2=finish, 3=continue scroll page
        var vscreen = 0;
        var log = '';

        $(document).ready(function () {
            //check noresult
            if (document.getElementById('divNoresult')) {
                return;
            }
            window.scrollTo(0, 0);
            ChangeParam('pageIndex', '1', '');

            oldwidth = $(window).width();
            checkWorking = 1;

            $('#loading').css('display', 'block');
            loadproduct(0);
        });
        function loadproduct(act) {
            var isHot = false;
            var isNew = false;
            var isBestSeller = false;

            if (act == 0) {
                var ww = $(window).width(); //window width
                var hw = $('.c-h1').width(); //h1 width
                var ps = 6;
                if (ww > 767) {
                    ps = parseInt(hw / 220) * 3;
                }

                ChangeParam('pageSize', ps, '');
            }
            if(arrParam !== undefined)
            $.ajax({
                url: '/store/search-result.aspx/GetData',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{' + arrParam + '}',
                failure: function (response) {
                    log += response;
                    //console.log(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //console.log(xhr.status + ' - ' + thrownError);
                    log += xhr.status + ' - ' + thrownError;
                },
                success: function (response) {
                    var s = '';
                    var allowMore = 0;
                    var objReturn = JSON.parse(response.d);

                    if (objReturn != null) {
                        var lst = objReturn.ItemList;
                        allowMore = objReturn.AllowMore;
                        // $('#hidAllowSrollProduct').val(allowMore);
                        if (lst != null) {
                            //  $('.tabs-content').html(htmlTag);
                            $.each(lst, function (idx, obj) {

                                if ($.inArray(obj.Index, arrProduct) > -1) {
                                    return true;
                                }

                                s += '<div class="item left" id="wrap-' + obj.Index + '"><div id="bd-' + obj.Index + '" class="top"></div><article style="display:table-cell">';
                                //div info
                                s += '<div id="info-' + obj.Index + '">';

                                //icon
                                if (obj.Icon != null)
                                    s += '<div class="ic-item">' + obj.Icon + '</div>';

                                //image
                                s += '<div class="image"><a href="' + obj.Url + '" class="s-image"><img id="img-' + obj.Index + '" class="lazy" src="' + obj.Image + '" alt="' + obj.Title + '" /></a></div>';

                                //review
                                if (obj.Review != null)
                                    s += obj.Review;

                                //title
                                if (obj.Title != null)
                                    s += '<div class="title"><a href="' + obj.Url + '">' + obj.Title + '</a></div>';

                                //sku
                                if (obj.SKU != null)
                                    s += '<div class="sku">Item #' + obj.SKU + '</div>';

                                //price
                                if (obj.Price != null) {
                                    s += '<div class="g-price"><ul><li class="price">' + obj.Price + '</li>';
                                    if (obj.YouSave != null)
                                        s += obj.YouSave;
                                    s += '</ul></div>';
                                }

                                //promotion
                                if (obj.Promotion != null)
                                    s += '<div class="promotion">' + obj.Promotion + '</div>';

                                s += '</div>';

                                //cart
                                s += '<div id="cart-' + obj.Index + '" class="add-cart">';
                                if (obj.AddCart != null) {
                                    s += obj.AddCart;
                                }

                                //in cart
                                s += '<div id="lblInCart' + obj.ItemId + '" name="lblInCart' + obj.ItemId + '" class="incart">';
                                if (obj.InCart == 'True') {
                                    s += 'Added to your cart';
                                }
                                s += '</div>';

                                s += '</div>';
                                s += '</article></div></div></div><div class="ver-line"></div>';
                                arrProduct.push(obj.Index);

                                //Hot, New, BestSeller
                                if (isHot == false && obj.IsHot) {
                                    isHot = true;
                                }

                                if (isNew == false && obj.IsNew) {
                                    isNew = true;
                                }

                                if (isBestSeller == false && obj.IsBestSeller) {
                                    isBestSeller = true;
                                }
                            });
                            // alert(allowMore);
                            $('#list-item').width($('#uplist').width());
                            var pagesize = GetParam('pageSize');
                            $("#list-item").append(s);
                            checkWorking = equalheighta('#list-item .item');
                            $('#loading').css('display', 'none');

                            if (allowMore == 0) {
                                //alert('allowMore');
                                $('#loadmore').css('display', 'none');


                                ChangeParam('listheight', $("#list-item").height(), '');
                            }
                            else {
                                $('#loadmore').css('display', 'block');
                            }


                        }
                        else {
                            $('#loadmore').css('display', 'none');
                            $('#loading').css('display', 'none');
                        }
                    }
                    else {

                        $('#loadmore').css('display', 'none');
                        $('#loading').css('display', 'none');

                    }
                    $('#hidAllowSrollProduct').val(allowMore);

                    if (checkWorking == 3) {
                        var olh = GetParam('listheight');
                        vscreen = $(window).height();
                        if (olh == 0) {
                            checkWorking = 1;
                            ChangeParam('pageIndex', '', 'next');
                            loadproduct(1);

                            var lh = $("#list-item").height();
                            ChangeParam('listheight', lh, '');
                        }
                    }
                }
            });

        }
        equalheighta = function (container) {
            var currentTallest = 0, currentRowStart = 0, rowDivs = new Array(), $el, topPosition = 0;
            var hc = 73; //height add cart
            if (window.matchMedia('(max-width: 767px)').matches) {
                hc = 138;
            }
            ///alert(hc);
            $(container).each(function () {

                $el = $(this);
                topPostion = $el.position().top;
                if ($.inArray($el.attr("id"), arrArrange) > -1) {
                    //$el.css('background-color', '#333' + iColor);
                    return true;
                }
                else {
                    arrArrange.push($el.attr("id"));
                }

                if (currentRowStart != topPostion) {
                    // we just came to a new row.  Set all the heights on the completed row
                    for (currentDiv = 0; currentDiv < rowDivs.length; currentDiv++) {
                        if (window.matchMedia('(max-width: 767px)').matches) {
                            rowDivs[currentDiv].height(currentTallest);
                        }
                        else {
                            $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                        }
                    }

                    // set the variables for the new row
                    rowDivs.length = 0; // empty the array
                    currentRowStart = topPostion;
                    currentTallest = $el.height();
                    rowDivs.push($el);

                } else {
                    // another div on the current row.  Add it to the list and check if it's taller
                    rowDivs.push($el);
                    currentTallest = (currentTallest < $el.height()) ? ($el.height()) : (currentTallest);
                }

                for (currentDiv = 0; currentDiv < rowDivs.length; currentDiv++) {
                    // do the last row
                    if (window.matchMedia('(max-width: 767px)').matches) {
                        rowDivs[currentDiv].height(currentTallest);
                    }
                    else {
                        $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                    }
                }
            });

            $("#line-fix").height($("#list-item").height());
            var wh = $(window).height(); //window height
            var lh = $("#list-item").height();

            if (lh + 300 < wh) {
                //alert(lh + ' + 300 < ' + wh);
                var olh = GetParam('listheight'); //olh = old list height
                if (olh == 0) {
                    return 3;
                }
            }

            return 2;
        }

        var waitForFinalEvent = (function () {
            var timers = {};
            return function (callback, ms, uniqueId) {
                if (!uniqueId) {
                    uniqueId = "Don't call this twice without a uniqueId";
                }
                if (timers[uniqueId]) {
                    clearTimeout(timers[uniqueId]);
                }
                timers[uniqueId] = setTimeout(callback, ms);
            };
        })();

        var idResizeSearch;
        $(window).resize(function () {
            var isVideoActive = isShowPanel('pnVideo');
            var isArticleActive = isShowPanel('pnArticle');
            if (isVideoActive || isArticleActive) {
                clearTimeout(idResizeSearch);
                idResizeSearch = setTimeout(ChangeResize, 500);
            }
            else {
                var ww = $(window).width();
                waitForFinalEvent(function () {
                    if ($(window).width() != oldwidth) {
                        if (checkWorking < 2) {
                            //alert(arrArrange.join(',') + ' | ' + arrProduct.join(',') + ' | ' + arrParam + ' | ' + checkWorking);
                            location.reload();
                            return false;
                        }

                        oldwidth = $(window).width();
                        $('#loading').css('display', 'block');
                        $('#loadmore').css('display', 'none');
                        $('#list-item').css('width', 'auto');
                        window.scrollTo(0, 0);
                        arrArrange = [];
                        arrProduct = [];
                        ChangeParam('pageIndex', '1', '');
                        ChangeParam('listheight', 0, '');
                        $("#list-item").html('<div class="ver-line-group">&nbsp;</div>');
                        loadproduct(0);
                        //$("#imgTop").html(arrParam);
                    }
                }, 500, "some unique string");
            }
        });
        var isResize = false;
        function ChangeResize() {
            isLoadFullWidthVideo = true;
            isResize = true;
            SetHeightPanelArticle();
            SetHeightPanelVideo();
        }

        $(window).scroll(function () {
            var wt = $(window).scrollTop();

            if (wt < 500) {
                $("#imgTop").css("display", "none");
            }
            else {
                $("#imgTop").css("display", "block");
            }

            var isVideoActive = isShowPanel('pnVideo');
            if (isVideoActive) {
                ScrollDataVideo();
            }
            else {
                var isArticleActive = isShowPanel('pnArticle');
                if (isArticleActive) {
                    ScrollDataArticle();
                }
                else {
                    if (checkWorking < 2) {
                        return false;
                    }

                    var hidAllowSrollProduct = $('#hidAllowSrollProduct').val();
                    if (hidAllowSrollProduct == '1') {
                        var lh = $("#list-item").height(); //lh = list height
                        var wh = $(window).height() - wt; //lh = list height
                        if (vscreen > 0) {
                            if (wh < 800)
                                wh = 800;
                        }
                        else {
                            wh = 0;
                        }

                        var body = document.body, html = document.documentElement;
                        var olh = GetParam('listheight'); //olh = old list height
                        $('#notest').html(body.scrollHeight + ' | ' + body.offsetHeight + ' | ' + html.clientHeight + ' | ' + html.scrollHeight + ' | ' + html.offsetHeight + ' | ' + wt + ' + ' + wh + ' + 600 > ' + lh + ' && ' + wt + ' + 600 > ' + olh + ' && ' + lh + ' > 0');

                        if (wt + wh + 600 > lh && wt + 600 > olh && lh > 0) {
                            checkWorking = 1;
                            ChangeParam('pageIndex', '', 'next');
                            loadproduct(1);
                            ChangeParam('listheight', lh, '');
                        }
                        else {
                            var p = GetParam("pageIndex");
                            if (p == 1 && (lh + wt > $(window).height())) {
                                vscreen = $(window).height();
                            }
                        }

                        var pi = GetParam("pageIndex");
                        $('#notest').append(' | p=' + pi + ' | ' + log);
                    }
                }
            }
        });
        function isShowPanel(divPanel) {
            if (document.getElementById(divPanel)) {
                var display = $('#' + divPanel).css('display');
                if (display == 'none') {
                    return false;
                }
                return true;
            }
            return false;
        }
        function SetHeightPanelVideo() {
            if (isShowPanel('pnVideo')) {
                ResetHeightListVideo(true, 'video');
            }
        }
        function SetHeightPanelArticle() {
            if (isShowPanel('pnArticle')) {
                var currentArticleType = $('#hidArticleType').val();

                if (currentArticleType == '0') {//all
                    var lstchild = $('.searchresult #pnArticle  #pnArticleContent').children('.group-article');
                    ResetHeightList(lstchild, 'search-article');
                }


            }
        }
        function ViewMoreResultArticle(type) {

            var hidKeyword = $('#hidKeyword').val();
            var pgsize = $('#hidPageSizeArticle').val();
            var pageIndex = $('#hidPageIndexArticle').val();
            $('#hidArticleType').val(type);
            mainScreen.ExecuteCommand('ViewMoreResultArticleByType', 'methodHandlers.ViewMoreResultArticleByTypeCallBack', [hidKeyword, type, pageIndex, pgsize]);
            return false;
        }


        methodHandlers.ViewMoreResultArticleByTypeCallBack = function (html, type, allowMore) {

            var htmlArticle = ''
            var div = '';
            if (html) {
                htmlArticle = html[0];

            }
            var category = ''
            if (type == 1) { //news
                div = 'dvNew';
                category = 'News & Event'
            }
            else if (type == 2) //tips
            {
                category = 'Expert Tips & Advice'
                div = 'dvTip'
            }
            else //media press
            {
                category = 'Media Press'
                div = 'dvMediaPress'
            }
            $('#dvNew').hide();
            $('#dvTip').hide();
            $('#dvMediaPress').hide();
            $('#' + div + '> ul').replaceWith(htmlArticle);
            $('#' + div + '> div.viewmore').hide();
            $('#' + div + '[style]').removeAttr('style');
            $('#' + div).height('auto');
            $('#' + div).width('100%');
            $('#' + div).show();
            $('#lnkBackArticle').show();

            $('#' + div).addClass("more-result");
            var location = window.location.href;
            var titleHTML = $('.c-h1').html();
            titleHTML = titleHTML + ' on "<span class=kw>' + category + '</span>"';
            // titleHTML = titleHTML + "<a href='" + location + "' class='back'>Back</a>"
            $('.c-h1').html(titleHTML);
            $('#nct-fltr').hide();

            if (allowMore == '1') {

                $("#loadmoreArticle").show();
            }
            $('#hidPageIndexArticle').val('2');
            isLoadingArticle = 0;

        }
        function SearchArticle(countProduct, countVideo) {
            var rawURL = window.location.href;
            //  $('#hidAllowSroll').val('0');
            var hidKeyword = $('#hidKeyword').val();
            $('#hidPageIndexArticle').val('1');
            //if ($('#box-search-replace').length > 0) { //replace keyword
            //    $('#box-search-replace').removeAttr('id');
            //}
            mainScreen.ExecuteCommand('SearchArticle', 'methodHandlers.SearchArticleCallBack', [hidKeyword, countProduct, countVideo, rawURL]);
            return false;
        }
        methodHandlers.SearchArticleCallBack = function (html) {

            var htmlArticle = '';
            var htmlTag = '';
            var title = '';
            if (html) {
                htmlArticle = html[1];
                htmlTag = html[0];
                title = html[2];
            }

            $('#pnArticleContent').html(htmlArticle);
            $('.tabs-content').html(htmlTag);
            $('#pnArticle').show();
            $('#loadmoreArticle').hide();
            $('#pnProduct').hide();
            $('#pnVideo').hide();
            //$('.c-h1').html(title);
            $('#hidArticleType').val('0');
            //SetHeightPanelArticle();
        }
        function SearchVideo(countProduct, countArticle, pageIndex, pageSize) {
            var rawURL = window.location.href;
            var panelVideoWidth = $("#content-tab").innerWidth();
            var countItemInRow = 0; // Math.floor(panelVideoWidth / 278);   //278 la width cua 1 video     
            var hidKeyword = $('#hidKeyword').val();
            if (document.getElementById("loading")) {
                document.getElementById("loading").style.display = 'block';
            }
            var url = document.getElementById('hidUrlVideo').value;
            var arrurl = url.split("?");
            url = arrurl[0] + "/GetSearchVideo";
            $.ajax({
                type: "POST",
                url: url,
                data: "{pageIndex:" + pageIndex + ", pageSize:" + pageSize + ",keyword:'" + hidKeyword + "',countItemInRow:" + countItemInRow + ",countProduct:" + countProduct + ",countArticle:" + countArticle + ",rawURL:'" + rawURL + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSearchVideoSuccess,
                failure: function (response) {

                },
                error: function (response) {

                }
            });

        }
        var isLoadFullWidthVideo = false;
        function OnSearchVideoSuccess(response) {
            var allowMore = 0;
            var addContent = response.d[0];
            $("#lstvideo").html(addContent);
            $('#hidTotalVideo').val(response.d[1]);
            $('.tabs-content').html(response.d[2]);
            $('#hidPageSizeVideo').val(response.d[3]);
            $('#hiCountdataVideo').val(response.d[1]);
            $('#hidPageIndexVideo').val(1);
            allowMore = response.d[4]
            $('#pnArticle').hide();
            $('#pnProduct').hide();
            $('#pnVideo').show();
            $('#nct-fltr').hide();
            if (document.getElementById("loading")) {

                document.getElementById("loading").style.display = 'none';
            }
            if (allowMore == '1') {
                $("#loadmoreVideo").show();
            }
            isLoadingVideo = 0;
            isLoadFullWidthVideo = true;
            DelayResetHeightListVideo(true, 'video');
        }
        function LoadFullWidthVideo(count, maxItemInRow, maxIndex) {
            //count: so luong item phai load them cho du dong
            //maxItemInRow: so luong item lon nhat tren moi dong
            //maxIndex: index cua item cuoi cung
            //   alert(count + '-' + maxItemInRow + '-' + maxIndex);
            //thay vi load them cho du thi remove nhung item bi du di ( cach cu lam load them item nen ham moi co ten la LoadFullWidthVideo)
            if (isResize) {
                LoadFullWidthVideoResize(count, maxItemInRow, maxIndex);
            }
            else {
                if (count > 0) {//neu co item du moi chay ham nay
                    LoadFullWidthVideoLoad(count, maxItemInRow, maxIndex);
                }
            }
            isResize = false;
        }
        function LoadFullWidthVideoResize(count, maxItemInRow, maxIndex) {
           
           
            //co the load lai trg 1 nhu K lam ben list product
           //load them item sao cho so page phai chan
            var countLastRowItem = 0; //so luong item bi du
            var newPageSize = maxItemInRow * 3; //1 page load 3 dong item
            //remove
            var countModRow = maxIndex % newPageSize; //tinh xem bi du bao nhieu dong thi thi remove nhung dong du nay di
            var tmpTotal = maxIndex;
            if (countModRow > 0) {

                tmpTotal = maxIndex - countModRow;
                var countPageIndex = tmpTotal / newPageSize;
                isLoadFullWidthVideo = false;               
                $('#imgloadingVideo').show();
                $('#loadmoreVideo').hide();
                var hidKeyword = $('#hidKeyword').val();
                var url = document.getElementById('hidUrlVideo').value;
                var arrurl = url.split("?");
                url = arrurl[0] + "/LoadFullWidthVideoResize";
                $.ajax({
                    type: "POST",
                    url: url,
                    data: "{countModRow:" + countModRow + ",pageIndex:" + countPageIndex + ", pageSize:" + newPageSize + ",keyword:'" + hidKeyword + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnGetMoreVideoSearchSuccess,
                    failure: function (response) {

                    },
                    error: function (response) {

                    }
                });

            }
            else {
                var countPageIndex = maxIndex / newPageSize;
                $('#hidPageIndexVideo').val(countPageIndex);
                $('#hidPageSizeVideo').val(newPageSize);
            }
            
        }

        function LoadFullWidthVideoLoad(count, maxItemInRow, maxIndex) {

            var countLastRowItem = maxItemInRow - count; //so luong item bi du
            $("#loadmoreVideo").show();
            $("#lstvideo").height('auto');
            var i = maxIndex;
            var indexRemove = 0;
            while (indexRemove < countLastRowItem) {//remove item bi du
                $("#videoItem_" + maxIndex).replaceWith('');
                maxIndex = maxIndex - 1;
                indexRemove = indexRemove + 1;
            }
            //update lai page size
            var currentPageSize = $('#hidPageSizeVideo').val();
            currentPageSize = parseInt(currentPageSize);
            currentPageSize = currentPageSize - countLastRowItem;
            $('#hidPageSizeVideo').val(currentPageSize);
        }

        function GetMoreVideoSearch() {
            isLoadFullWidthVideo = false;
            var pageSize = $('#hidPageSizeVideo').val();
            var pageIndex = $('#hidPageIndexVideo').val();
            pageSize = parseInt(pageSize);
            pageIndex = parseInt(pageIndex);
            $('#imgloadingVideo').show();
            $('#loadmoreVideo').hide();
            var hidKeyword = $('#hidKeyword').val();
            var url = document.getElementById('hidUrlVideo').value;
            var arrurl = url.split("?");
            url = arrurl[0] + "/GetMoreVideo";
            $.ajax({
                type: "POST",
                url: url,
                data: "{pageIndex:" + pageIndex + ", pageSize:" + pageSize + ",keyword:'" + hidKeyword + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnGetMoreVideoSearchSuccess,
                failure: function (response) {

                },
                error: function (response) {

                }
            });
        }
        function OnGetMoreVideoSearchSuccess(response) {
            var lastItem = $('#lstvideo .dvVideoItem').last();
            var addContent = response.d[0];
            var allowMore = response.d[1];
            $('#hidPageIndexVideo').val(response.d[2]);
            $('#hidPageSizeVideo').val(response.d[3]);
            lastItem.after(addContent);
            $('#imgloadingVideo').hide();
            if (allowMore == '1') {
                $("#loadmoreVideo").show();
            }
            else {
                $("#loadmoreVideo").hide();
            }
            isLoadingVideo = 0;
            ResetHeightListVideo(false, 'video');
        }
        var isExistScrollVideo;
        var isLoadingVideo = 0;
        function ScrollDataVideo() {

           
            var val1 = ",100,200,300,400,500,600";
            var hdoc = $("#lstvideo").height();
            var hlstShopDesign = $("#shop-designlst").height();

            var hscroll = $(window).scrollTop();
            hdoc = hdoc + $("header").height();
            hscroll = hscroll + $("footer").height() + $("header").height() + 200,
		 val = 00,
        lsroll2 = hscroll.toString().substr(hscroll.toString().length - 2, 2),
        ldoc2 = hdoc.toString().substr(hdoc.toString().length - 2, 2),
        eqhscroll = hscroll - lsroll2,
        eqhdoc = hdoc - ldoc2,
        topmore = $("#loadmoreVideo").position().top,
        ltopmore = topmore.toString().substr(2, 2),
        etopmore = topmore.toString().replace(ltopmore, "00");
          
            var browser = navigator.userAgent.toLowerCase();
            if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {

                if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScrollVideo != eqhscroll && isLoadingVideo == 0) {
                    isExistScrollVideo = eqhscroll;
                    isLoadingVideo = 1;
                    if (isAllowMoreVideo()) {
                        GetMoreVideoSearch();
                    }
                }
            }
            else {
                if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScrollVideo != eqhscroll && isLoadingVideo == 0) {
                    // alert(eqhscroll - eqhdoc);
                    isExistScrollVideo = eqhscroll;
                    isLoadingVideo = 1;
                    if (isAllowMoreVideo()) {
                        GetMoreVideoSearch();
                    }
                }
            }
        }
        var isExistScrollArticle;
        var isLoadingArticle = 0;
        function ScrollDataArticle() {

            var val1 = ",100,200,300,400,500,600";
            var hdoc = $("#dvTip").height();
            var hlstShopDesign = $("#shop-designlst").height();
            var hscroll = $(window).scrollTop();
            hdoc = hdoc + $("header").height();
            hscroll = hscroll + $("footer").height() + $("header").height() + 200,
		 val = 00,
        lsroll2 = hscroll.toString().substr(hscroll.toString().length - 2, 2),
        ldoc2 = hdoc.toString().substr(hdoc.toString().length - 2, 2),
        eqhscroll = hscroll - lsroll2,
        eqhdoc = hdoc - ldoc2,
        topmore = $("#loadmoreArticle").position().top,
        ltopmore = topmore.toString().substr(2, 2),
        etopmore = topmore.toString().replace(ltopmore, "00");

            var browser = navigator.userAgent.toLowerCase();
            if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {

                if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScrollArticle != eqhscroll && isLoadingArticle == 0) {
                    isExistScrollArticle = eqhscroll;
                    isLoadingArticle = 1;
                    if (isAllowMoreArticle()) {

                        GetMoreArticleSearch();
                    }
                }
            }
            else {
                if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScrollArticle != eqhscroll && isLoadingArticle == 0) {
                    // alert(eqhscroll - eqhdoc);
                    isExistScrollArticle = eqhscroll;
                    isLoadingArticle = 1;
                    if (isAllowMoreArticle()) {
                        GetMoreArticleSearch();
                    }
                }
            }
        }
        function isAllowMoreVideo() {
            var display = $('#loadmoreVideo').css('display');
            if (display == 'block') {
                return true;
            }
            return false;
        }
        function isAllowMoreArticle() {
            var display = $('#loadmoreArticle').css('display');
            if (display == 'block') {
                return true;
            }
            return false;
        }
        function GetMoreArticleSearch() {

            var ps = $('#hidPageSizeArticle').val();
            var pi = $('#hidPageIndexArticle').val();
            pi = parseInt(pi);
            ps = parseInt(ps);
            $('#imgloadingArticle').show();
            $('#loadmoreArticle').hide();
            var type = $('#hidArticleType').val();
            var hidKeyword = $('#hidKeyword').val();
            var newPageIndex = pi + 1;
            $('#hidPageIndexArticle').val(newPageIndex.toString());
            mainScreen.ExecuteCommand('ViewMoreResultArticleByType', 'methodHandlers.ScrollViewMoreResultArticleByTypeCallBack', [hidKeyword, type, pi, ps]);
            return false;
        }
        methodHandlers.ScrollViewMoreResultArticleByTypeCallBack = function (html, type, allowMore) {
            var htmlArticle = ''
            var div = '';
            if (html) {
                htmlArticle = html[0];

            }

            if (type == 1) { //news
                div = 'dvNew';

            }
            else if (type == 2) //tips
            {

                div = 'dvTip'
            }
            else //media press
            {

                div = 'dvMediaPress'
            }
            var oldContent = $('#' + div + '> ul').html();
            oldContent = oldContent + htmlArticle;
            $('#' + div + '> ul').html(oldContent);
            $('#imgloadingArticle').hide();
            if (allowMore == '1') {

                $("#loadmoreArticle").show();
            }
            else {
                $("#loadmoreArticle").hide();
            }

            isLoadingArticle = 0;

        }
    </script>
</asp:Content>
