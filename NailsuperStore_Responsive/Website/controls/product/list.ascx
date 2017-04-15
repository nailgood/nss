<%@ Control Language="VB" AutoEventWireup="false" CodeFile="list.ascx.vb" Inherits="Controls_ProductList" %>

<%=IIf(Request.RawUrl.Contains("/recently-viewed.aspx"), "<style>#list-item .add-cart {height:32px;}</style>", "")%>

<script type="text/javascript">
    var arrParam = '<asp:Literal ID="litParam" runat="server"></asp:Literal>';
    var arrProduct = [];
    var arrArrange = [];
    var oldwidth = 0;
    var checkWorking = 0;
    var vscreen = 0;
    var log = '';

    $(document).ready(function () {
       <% If Not String.IsNullOrEmpty(queryString) AndAlso Not VideoId > 0 Then %>
        window.scrollTo(0, 0);
        ChangeParam('pageIndex', '1', '');
        oldwidth = $(window).width();
        checkWorking = 1;
        $('#loading').css('display', 'block');
        loadproduct(0);

       <% End If %>
    });

    $(window).load(function(){
        <% If VideoId > 0 Then %>
        oldwidth = $(window).width();
        checkWorking = 2;
        loadproduct(0);
        <% End If %>
    });

    $(window).scroll(function () {
        var wt = $(window).scrollTop(); //wt = window scroll top

        if (wt < 500) {
            $("#imgTop").css("display", "none");
        }
        else {
            $("#imgTop").css("display", "block");
        }

        if (checkWorking < 2) {
            return false;
        }

        var lh = $("#<%=lstName %>list-item").height(); //lh = list height
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
        $('#notest').html(body.scrollHeight + ' | ' + body.offsetHeight + ' | ' +  html.clientHeight + ' | ' + wt + ' + ' + wh + ' + 600 > ' + lh + ' && ' + wt + ' + 600 > ' + olh + ' && ' + lh + ' > 0 | ');
        
         if (wt + wh + 1500 > lh && wt + 1500 > olh && lh > 0) {
            <%If VideoId > 0 Then %>
                //alert('video list item');
                checkWorking = 1;
                ChangeParam('pageIndex', '', 'next');
                loadproduct(1);
                ChangeParam('listheight', lh, '');
            <% End If %>
         }

        if (wt + wh + 600 > lh && wt + 600 > olh && lh > 0) {
            checkWorking = 1;
            ChangeParam('pageIndex', '', 'next');
            loadproduct(1);
            ChangeParam('listheight', lh, '');
        }

        else
        {
            var p = GetParam("pageIndex");
            if(p == 1 && (lh + wt > $(window).height()))
            {
                vscreen = $(window).height();  
            }
        }
       
        var pi = GetParam("pageIndex");
        $('#notest').append(' | p=' + pi + ' | ' + log);
//        $('#imgTop').html(wt + ' + ' + wh + ' + 600 > ' + lh + ' && ' + wt + ' + 600 > ' + olh + ' && ' + lh + ' > 0' + ' | ' + log);
//        $('#imgTop').width(900);
    });

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

    var t ='';

    $(window).resize(function () {
        waitForFinalEvent(function () {
            if ($(window).width() != oldwidth) {
                if ((window.fullScreen) || (window.innerWidth == screen.width && window.innerHeight == screen.height))
                {
                    //stop reload page after clicked fullscreen button on video HTML5 ; 
                    //alert('fullscreen event');
                    return;
                }
                else if (checkWorking < 2) {
                    location.reload();
                    return false;
                }
                //console.log('resize >> oldwidth=' + oldwidth +  ' | newwidth=' + $(window).width());
                oldwidth = $(window).width();

                $('#loading').css('display', 'block');
                $('#loadmore').css('display', 'none');
                $('#<%=lstName %>list-item').css('width', 'auto');
                window.scrollTo(0, 0);
                arrArrange = [];
                arrProduct = [];
                ChangeParam('pageIndex', '1', '');
                ChangeParam('listheight', 0, '');

                if (location.href.indexOf('recently-viewed.aspx') > 0)
                    $("#<%=lstName %>list-item").html('<div style="margin-bottom:-8px;" class="ver-line-group">&nbsp;</div>');
                else
                    $("#<%=lstName %>list-item").html('<div class="ver-line-group">&nbsp;</div>');
                
                loadproduct(0);
            }
        }, 500, "some unique string");
    });

    function loadproduct(act) {
        var iHot = 0;
        var iNew = 0;
        var iBestSeller = 0;

        if (act == 0) {
            var ww = $(window).width(); //window width
            <% If lstName.Length > 0 Then %>
                var hw = $('#h-product').width(); //h1 width
            <% Else %>
                var hw = $('.c-h1').width(); //h1 width
            <% End If %>
            

            var ps = parseInt(hw / 220) * 3;
            <% If lstName.Length > 0 Then %>
            if (ww <= 767) {
                ps = 6;
            }
            else if (ww <= 1199) {
                ps = 16;
            }
            <%  Else %>
            if (ww <= 375) {
                ps = 12;
            }
            else if (ww <= 767) {
                ps = 6;
            }
            else if (ww <= 1199) {
                ps = 9;
            }
            else if (ww <= 1397) {
                ps = 12;
            }       
            <% End If %>
            
            <% If Not (RelatedItemId > 0 Or VideoId > 0 Or OrderId > 0 Or MemberId > 0 Or Not String.IsNullOrEmpty(SessionId)) Then %>
                ChangeParam('pageSize', ps, '');
            <% End If %>
        }

        //console.log(arrParam);
        $.ajax({
            url: '<%=strPath %>',
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

                var isRecentlyView = location.href.indexOf('recently-viewed.aspx') > 0;

                var lst = JSON.parse(response.d);
                if (lst != null) {
                    $.each(lst, function (idx, obj) {

                        if ($.inArray(obj.Index, arrProduct) > -1) {
                            return true;
                        }

                        <% If lstName.Length > 0 Then %>
                        s += '<div class="citem" id="wrap-' + obj.Index + '"><article'
                        
                        s += '>';
                        <% Else %>
                        s += '<div class="item ';
                        if (!(obj.ItemId > 0))
                            s += ' osearch '

                        s += ' left" id="wrap-' + obj.Index + '" data-itemid="'+ obj.ItemId +'" data-itemname="'+ obj.Title + '"><div id="bd-' + obj.Index + '" class="top"></div><article style="display:table-cell;position: relative;">';
                        <% End If %>

                        if (obj.ItemId > 0) { //not recently view keyword
                            //div info
                            if (isRecentlyView)
                                s += '<div class="recently-view-close" onclick="deleteRecentlyItem(this);return false;"><span class="page">Close</span></div>'
                            s += '<div id="info-' + obj.Index + '">';

                            //icon
                            if (obj.Icon != null)
                                s += '<div class="ic-item">' + obj.Icon + '</div>';

                            //image
                            s += '<div class="image"><a href="' + obj.Url + '" class="s-image"><img id="img-' + obj.Index + '" class="lazy"  src="' + obj.Image + '" alt="' + obj.Title + '" /></a></div>';

                            <% If String.IsNullOrEmpty(lstName) Then %>
                            //review
                            if (obj.Review != null)
                                s += obj.Review;
                            <% End If %>

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
                        }
                        else { //recently view
                            s += '<div class="recently-view-close" onclick="deleteRecentlyItem(this);return false;"><span class="page">Close</span></div>'
                            s += '<div id="info-' + obj.Index + '" class="otextsearch"><a href="' + obj.Url + '"><span style="margin-top:10px;display:block">You searched for  </span><b style="font-size: 16px; line-height: 20px;">"' + obj.Title + '"</b></a></div>';
                        }
                        

                        <% If lstName.Length > 0 Then %>
                        s += '</article></div></div></div>';
                        <% Else %>
                        s += '</article></div></div></div><div class="ver-line"></div>';
                        <% End If %>
                        
                        arrProduct.push(obj.Index);

                        //Hot, New, BestSeller
                        if (obj.IsHot) {
                            iHot += 1;
                        }

                        if (obj.IsNew) {
                            iNew += 1;
                        }

                        if (obj.IsBestSeller) {
                            iBestSeller += 1;
                        }
                    });

                    $('#<%=lstName %>list-item').width($('#uplist').width());

                    
                    var pagesize = GetParam('pageSize');
                    $("#<%=lstName %>list-item").append(s);
                    <% If VideoId > 0 Then %>
                    if (ww > 767) {
                        checkWorking = equalheighta('#<%=lstName %>list-item .<%=lstName %>item');
                    }
                    <% Else %>
                    checkWorking = equalheighta('#<%=lstName %>list-item .<%=lstName %>item');
                    <% End If %>

                    $('#loading').css('display', 'none');

                    if (lst.length < parseInt(pagesize)) {
                        $('#loadmore').css('display', 'none');
                        <% If RelatedItemId > 0 Or VideoId > 0 Or OrderId > 0 Or MemberId > 0 Or Not String.IsNullOrEmpty(SessionId) Then %>
                        ChangeParam('listheight', $("body").height(), '');
                        <% Else %>
                        ChangeParam('listheight', $("#<%=lstName %>list-item").height(), '');
                        <% End If %>
                    }
                    else {
                        $('#loadmore').css('display', 'block');
                    }

                    if (act == 0) {
                        setsortby(iHot > 0, iNew > 0, iBestSeller > 0);
                    }
                    

                }
                else {
                    $('#loadmore').css('display', 'none');
                    $('#loading').css('display', 'none');

                    if (act == 0) {
                        <% If RelatedItemId > 0 Then %>
                        $('#divRelated').css('display', 'none');
                        $('#customer-also-bought').css('display', 'none');
                        <% Else If VideoId > 0 Then %>
                        $('#tabnav_item').css('display', 'none');
                        $('#tab_item').css('display', 'none');

                        if($('#tabnav_video').css('display') == 'none')
                        {
                            if (oldwidth > 767) {
                                $("#tab_comment").css('display', 'block');
                                $('#tabnav_comment').addClass('selected');
                            }
                        }
                        else{
                            $('#tabnav_video').addClass('selected');
                            $("#tab_video").css('display','block');
                        }
                        <% Else %>
                        $('#line-fix').css('display', 'none');
                        $("#<%=lstName %>list-item").html('We are sorry! No items were found that match your selection criteria.');
                        <% End If %>
                        
                    }
                }
                
                if (checkWorking == 3) {
                    var olh = GetParam('listheight');
                    vscreen = $(window).height();
                    if (olh == 0) {
                        checkWorking = 1;
                        ChangeParam('pageIndex', '', 'next');
                        loadproduct(1);

                        var lh = $("#<%=lstName %>list-item").height();
                        ChangeParam('listheight', lh, '');
                    }
                }
            }
        });
    }

    equalheighta = function (container) {
        if (window.innerWidth <= 375) {
            return 2;
        }
        
        var currentTallest = 0, currentRowStart = 0, rowDivs = new Array(), $el, topPosition = 0;
        var isRecentlyView = location.href.indexOf('recently-viewed.aspx') > 0;

        <% If lstName.Length > 0 Then %>
            var hc = 65; //height add cart
            if (window.innerWidth <= 768) {
                hc = 73;
            }
        <% Else %>
            var hc = 73; //height add cart
            if (window.innerWidth <= 768) {
                hc = 108;
            }
        <% End If %>

        $(container).each(function () {
            $el = $(this);
            topPostion = $el.position().top;
            if ($.inArray($el.attr("id"), arrArrange) > -1) {
                return true;
            }
            else {
                arrArrange.push($el.attr("id"));
            }
            if (currentRowStart != topPostion) {
                // we just came to a new row.  Set all the heights on the completed row
                for (currentDiv = 0; currentDiv < rowDivs.length; currentDiv++) {
                    if (window.innerWidth > 375 && window.innerWidth <= 768) {
                        
                        if ($('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).length) {
                            $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                            //console.log(rowDivs[currentDiv].attr("id"));
                        }
                        else if (isRecentlyView) {
                            if (currentTallest < 200) {
                                currentTallest = 200;
                            }

                            //console.log(rowDivs[currentDiv].attr("id"));
                            $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest);
                        }
                        else {
                            //console.log(rowDivs[currentDiv].attr("id"));
                            $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest);
                        }
                    }
                    /*Dung cho truong hop 1 column tren mobile, khong can set height cứng
                    else if (window.matchMedia('(max-width: 375px)').matches) {
                        $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height('auto');
                    }*/
                    else {
                        if ($('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).length) {
                            $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                            //console.log(rowDivs[currentDiv].attr("id"));
                        }
                       
                        else {
                            //alert(rowDivs[currentDiv].attr("id") + 'a');
                            if (currentTallest < 200) {
                                currentTallest = 200;
                            }

                            if (window.innerWidth <= 1199) {
                                if (isRecentlyView) {
                                    $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest + 26);
                                }
                                else {
                                    $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest + 46);
                                }
                            }
                            else {
                                $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest);
                            }
                        }

                        
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
                if (window.innerWidth > 375 && window.innerWidth <= 768) {
                    if (isRecentlyView) {
                        if (currentTallest < 200) {
                            currentTallest = 200;
                        }

                        //console.log(rowDivs[currentDiv].attr("id"));
                        $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest);
                    }
                    else {
                        if (hc < $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).height())
                            hc = $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).height();

                        $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                        //console.log(rowDivs[currentDiv].attr("id"));
                    }
                }
                //else if (window.matchMedia('(max-width: 375px)').matches) {
                //    $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height('auto');
                //}
                else {
                    if ($('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).length) {
                        if (hc < $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).height())
                            hc = $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'cart-')).height();

                        $('#' + rowDivs[currentDiv].attr("id").replace('wrap-', 'info-')).height(currentTallest - hc);
                        //console.log(rowDivs[currentDiv].attr("id"));
                    }
                    else {
                        if (currentTallest < 200) {
                            currentTallest = 200;
                        }

                        if (window.innerWidth <= 1199) {
                            if (isRecentlyView) {
                                $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest + 26);
                            }
                            else {
                                $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest + 46);
                            }
                        }
                        else {
                            $('#' + rowDivs[currentDiv].attr("id")).height(currentTallest);
                        }
                    }
                }
            }
        });

        <% If String.IsNullOrEmpty(lstName) Then %>
        $("#line-fix").height($("#<%=lstName %>list-item").height());
        var wh = $(window).height(); //window height
        var lh = $("#<%=lstName %>list-item").height();

        if (lh + 300 < wh) {
            //alert(lh + ' + 300 < ' + wh);
            var olh = GetParam('listheight'); //olh = old list height
            if (olh == 0) {
                return 3;
            }
        }
        <% End If %>
        return 2;
    }

    function AddCartFromList(itemId)
    {
        $('#btnAddCart' + itemId).addClass('btn-active');
        $('#btnAddCart' + itemId).closest('.add-cart').closest('article').addClass('active');
        mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', [itemId, GetQty(itemId),false]);
    }
	
    </script>
<div id="uplist">
    <% If String.IsNullOrEmpty(lstName) Then %>
        <div id="line-fix" class="ver-line">&nbsp;</div>
    <% End If %>
    <div id="<%=lstName %>list-item">
        <%If (Request.RawUrl.Contains("recently-viewed.aspx")) Then %>
                 <div Class="ver-line-group" style="margin-bottom:-8px;">&nbsp;</div>
        <%Else %>
                <div Class="ver-line-group">&nbsp;</div>
        <% End If %>
       
    </div>
    <div id="loadmore" class="bg-loading2" style="display:none;border:solid 1px #dadada;width:100%;margin-top:10px;padding-top:20px">
        <img src="/includes/theme/images/loader.gif" alt="Waiting" />Please wait to see more items...<br />
        <div id="notest"></div>
    </div>
</div>
