<%@ Control Language="VB" AutoEventWireup="false" CodeFile="header.ascx.vb" Inherits="Header" %>
<%@ Register Src="~/controls/product/popup-cart.ascx" TagName="PopupCart" TagPrefix="uc1" %>
<%@ Register Src="~/controls/product/search-bar.ascx" TagName="SearchBar" TagPrefix="uc2" %>

<!--Header-->
<div id="wrapper" class="<%=ContainerCss %>">
    <header>
         <div id="h-wrapper">
            <div class="logo">
                <a href="/"> The Nailsuperstore
                </a>
            </div>
            <div id="l-header" class="hidden-xs">
                <div class="pro">
                    <ul class="w-chat " >
                       <li class="pull-right"><a href="/service/callback"><span class="glyphicon ic-req"></span> Schedule a Call</a></li>
                       <% If isShowLiveChat Then%>
                       <li class="leave-message pull-right">
                            <div id="scHwEb"></div>
                            <div id="sdHwEb"></div>
                            <script type="text/javascript">
                                var seHwEb = document.createElement("script");
                                seHwEb.type = "text/javascript";
                                var seHwEbs = "/includes/scripts/live-chat-safe-textlink.js?ps_h=HwEb&ps_t=" + new Date().getTime() + "&online-link-html=Live%20Chat&offline-link-html=Leave%20A%20Message";
                                setTimeout("seHwEb.src=seHwEbs;document.getElementById('sdHwEb').appendChild(seHwEb)", 1000)
                            </script>
                            <noscript>
                                <div style="display: inline">
                                    <a href="http://www.providesupport.com?messenger=nailsuperstore">Live Customer Service</a></div>
                            </noscript>
                            <div id="scPr0e"></div>
                            <div id="sdPr0e"></div>
                            <script type="text/javascript">
                                var sePr0e = document.createElement("script");
                                sePr0e.type = "text/javascript";
                                var sePr0es = "/includes/scripts/live-chat-safe-standard.js?ps_h=Pr0e&ps_t=" + new Date().getTime() + "&online-image=https%3A//www.nailsuperstore.com/images/chat_online.png&offline-image=https%3A//www.nailsuperstore.com/images/chat_offline.png";
                                setTimeout("sePr0e.src=sePr0es;document.getElementById('sdPr0e').appendChild(sePr0e)", 1000)
                            </script>
                            <noscript>
                                <div style="display: inline">
                                    <a href="http://www.providesupport.com?messenger=nailsuperstore">Live Customer Service</a></div>
                            </noscript>
                      </li>
                      <%End If%>
                      <%--<li class="contact pull-right hidden-md hidden-lg" ><a href="/contact/default.aspx"><span class="ic-contact"></span>Contact Us</a></li>--%>
                  </ul>
                    <div class="promotion-text"><span>Free Shipping</span> for orders of $<%=Utility.ConfigData.FreeShippingOrderAmount() %> or more <span class="sep"> |</span> <a href="/services/free-shipping-policies.aspx">Info/Exclusions</a></div>
                   
                <%--  <div id="quotes" class="pull-left">
                        <%  Try
                                For i As Integer = 0 To arrPro.Length - 1
                                    Dim arr As String() = arrPro(i).ToString().Split(";")%>
                        <div class="pro-item"><a href="/promotion-sales-price"><span class="c-pro"><%=arr(0)%></span> <%=arr(1)%></a></div>    
                        <% 
                        Next
                    Catch
                    End Try
                        %>
                   </div>--%>
                </div>
  
                
                <div id="menu">
                    <ul>
                        <li>
                            <span class="drop">FAQ <b class="arrow-down" id="baFA"></b></span>
                            <%--<span class="need-help visible-xs ">Need Help? <b class="arrow-down"></b></span>--%>
                            <div class="popover bottom submenu faq">
                                <span class="no-bg"></span>
                                <div class="arrow"></div>
                                <div class="popover-content">                            
                                    <ul class="faq-sub visible-md visible-lg">
                                        <li class="title">FAQs</li>
                                        <li><a href="/services/returns-policies.aspx">Return Policy</a></li>
                                        <li><a href="/service/order.aspx">Order & Shipping</a></li>
                                        <li><a href="/services/order-warranty.aspx">Warranty</a></li>
                                        <li><a href="/service/default.aspx">Customer Service</a></li>
                                    </ul>                            
                                    <div>
                                        <p><a class="text-underline" href="/contact/default.aspx">Contact Us</a></p> 
                                        <p><strong>Need Help?</strong><br />
                                        We're available Mon - Fri<br />
                                        9:00 AM to 5:30 PM CST</p>

                                        <p><strong>Headquarters</strong><br />
                                        3804 Carnation St<br />
                                        Franklin Park, IL 60131, USA</p>

                                        <p><strong>Phone</strong><br />
                                        800.669.9430 US<br />
                                        +1.847.260.4000 International</p>                                                           
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li class="none-border-sm1"><span>Resource Center <b class="arrow-down" id="baRC"></b></span>
                            <div class="popover bottom submenu">
                                <span class="no-bg"></span>
                                <div class="arrow"></div>
                                <div class="popover-content">
                                    <ul>
                                        <li><a href="/tips">Expert Tips & Advice</a></li>
                                        <li><a href="/gallery/nail-art-trend">Photo Gallery</a></li>
                                        <li><a href="/video-topic">How-To Videos</a></li>
                                        <li><a href="/news-topic">News & Events</a></li>
                                        <li><a href="/blog">Blog</a></li>
                                        <li><a href="/media-topic">As Seen In Media</a></li>
                                        <li><a href="/order-reviews">Customer Reviews</a></li>
                                        <li><a href="/product-reviews">Product Reviews</a></li>
                                        <li><a href="/service/msds.aspx">Material Safety Data Sheet</a></li>
                                        </ul>
                                </div>
                            </div>
                        </li>
                        <li class="visible-md visible-lg"><a href="/store/quickorder.aspx">Order by Item #</a></li>
                        <li><span>Shop by Design <b class="arrow-down" id="baSD"></b></span>
                            <asp:Literal runat="server" ID="ltrCategory"></asp:Literal> 
                        </li>
                        
                        <li><span>Deals Center <b class="arrow-down" id="baDC"></b></span>
                            <div class="popover bottom submenu deals-center">
                            <div class="ver-line">&nbsp;</div>
                                <span class="no-bg"></span>
                                <div class="arrow"></div>
                                <div class="popover-content">
                                    <div class="ver-line-group">&nbsp;</div>
                                    <ul class="shopnow">
                                        <%-- <li class="title">Shop Now</li>--%>
                                        <asp:Repeater runat="server" ID="rptShopNow">
                                            <ItemTemplate>
                                                <li>
                                                    <hr />
                                                    <div>
                                                        <div class="ver-line">&nbsp;</div>
                                                        <asp:Literal runat="server" ID="ltrLink"></asp:Literal>
                                                        <%--<div class="line-hide"></div>--%>
                                                    </div>
                                                </li>
                                                </ItemTemplate>
                                        </asp:Repeater>
                                        <%If TotalRecords > 7 Then%>
                                            <li>
                                            <hr />
                                                <div class="see-more">
                                                    <a href="/deals-center"><image src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/arrow-see-more.gif"></image><br /><p>See All</p></a>
                                                </div>
                                            </li>
                                        <%End If%>
                                    </ul>
                             
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
  </header>
    <!--End header-->
</div>

<!--NAV-->
<nav id="top-bar">
    <div class="<%=ContainerCss %> hidden-xs">
        <div id="menu-dept">
            <div class="menu"></div>
            Shop 
        </div>
            <div id="logged">
                <ul class="text-left">
                    <% If Not (cartItemCount > 0) %>
                    <li class="cart" onmouseout="ppoutcart();" onmouseover="ppovercart();" onclick="ncartclick();">
                        <a id="aCart" role="button" data-toggle="dropdown" href="#">
                            <span id="n-cart-count">0</span>
                            <div class="text">
                                <span>My </span>Cart
                                <span class="no-bg"></span><b class="arrow-down-cart arrow-cart"></b>
                            </div>
                        </a>

                        <div id="n-cart-down" class="popover bottom">
                            <div class="arrow"></div>
                            Your shopping cart is empty
                        </div>
                    </li>
                    <% Else %>
                    <li class="cart" onmouseout="ppoutcart();" onmouseover="ppovercart();" onclick="cartclick();">
                        <a id="aCart" role="button" href="/store/cart.aspx">
                            <span id="cart-count"><%=cartItemCount %></span>
                            <div class="text">
                                <span>My </span>Cart
                                <span class="no-bg"></span><b class="arrow-down-cart arrow-cart"></b>
                            </div>
                        </a>
                        <div id="cart-down" class="popover bottom">
                            <div class="arrow"></div>
                            <section>
                                <uc1:PopupCart ID="PopupCart1" runat="server" />
                                <span class="hidden"><asp:Button id="btnremove" runat="server" /></span>
                                <asp:LinkButton ID="lnkremoveBuyPoint" runat="server"></asp:LinkButton>
                                <input type="hidden" id="hidRemove" name="hidRemove" runat="server" />
                                <input type="hidden" id="reItemId" name="hidItemId" runat="server" />
                            </section>
                        </div>
                    </li>
                    <% End If %>

                    <li class="login <%If Not (m_MemberId) > 0 Then%>guest<%End If%>"><a href="/members/default.aspx">
                        <span class="hello">Hello, <%If m_MemberId > 0 Then%> <%=m_LoggedInName%> <%Else%> Sign in<%End If%></span>
                        <br />
                        <span>Your Account <%If m_MemberId > 0 Then%><b class="arrow-down-cart"></b><%End If%><span class="no-bg"></span></span>
                        </a>
                        <% If m_MemberId > 0 Then%>
                        <div id="acc-down" class="popover bottom">
                            <div class="arrow"></div>
                            <div style="padding-top:5px;">
                                <ul>
                                    <li><a href="/members/default.aspx" class="top">Your Account</a></li>
                                    <li><a href="/members/orderhistory/">Your Orders</a></li>
                                    <li><a href="/members/LeaveReview.aspx">Order / Product Review</a></li>
                                    <li><a href="/members/creditmemo/">Credit Memo</a></li>
                                    <li><a href="/members/addressbook/">Address Book</a></li>
                                    <li><a href="/store/recently-viewed.aspx">Recently Viewed</a></li>
                                    <li class="sign-out"><a href="/members/logout.aspx" class="sign-out">Not <%=m_LoggedInName %>? Sign Out <span class="pull-right glyphicon ic-power-off"></span></a></li>
                                </ul>
                            </div>
                        </div>
                        <%End If%>
                    </li>
                </ul>
            </div>
            
        <%--
            <div id="logged" class="hidden-xs">
                <ul class="text-left">
                    <li class="rw-point pull-right">
                        <span class="hello"><a href="/members/pointbalance.aspx">Rewards Points</a></span>
                        <span><a id="A1" href="/members/pointbalance.aspx"><strong><span class="c-point"><%=m_Point %></span> points</strong></a></span>
                    </li>
                </ul>
            </div>
        --%>


         <div id="divLargeSearch" class="box-search">
            <div class="visible-xs pad-search-sm"></div>
            <uc2:SearchBar ID="SearchBar1" runat="server" />
        </div>
        <%--<div id="ic-menu" class="hidden-md hidden-lg">
            <span class="ic-menu-sm"></span>
        </div>
        <div id="endCol" class="endCol"></div>--%>
    </div>
    <div id="menu-mobile" class="visible-xs">
        <ul class="text-left">
            <li class="w-xs-li"><a href="/"><span class="ic-mainmenu"></span></a></li>
            <li class="w-xs-li"><a onclick="msearchclick();"><span class="ic-search"></span></a></li>
            <li class="w-xs-li"><a href="/store/cart.aspx"><span id="small-cart-count"><%=cartItemCount %></span></a></li>
            <li style="margin-top:0px;float:none"><div id="divSmallSearch" class="hide"></div></li>
        </ul>
        
    </div>
</nav>
<div id="ListDepartment" class="popover bottom submenu list-dept">
    <span class="no-bg"></span>
    <div class="arrow">
    </div>
    <div class="popover-content">
        <asp:Literal runat="server" ID="ltrListDepartment"></asp:Literal>
    </div>
</div>
<!--End NAV-->

<input type="hidden" id="hidAllowShowPopupCart" name="hidAllowShowPopupCart" runat="server" />
<asp:Literal ID="ltscript" runat="server"></asp:Literal>
<a href="#" id="nyroModal" target="_blank"></a>
<input type="hidden" id="hidFixMenu" value="<%=Utility.ConfigData.PageFixMenu %>" />
<input type="hidden" id="hiddomain" value="<%=Utility.ConfigData.GlobalRefererName %>" />
<input type="hidden" id="hidUrl"  value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<script type="text/javascript">
    PageMethods.set_path("<%=strPage %>");

    var methodHandlers = {};
    methodHandlers.ShowCart = function (htmlReturn, maxqty, id, countitem) {
        var popupcart = '';
        var error = '';
        if (htmlReturn != null) {
            if (htmlReturn.length > 0) {
                popupcart = htmlReturn[0];
                error = htmlReturn[1];
            }
        }
        
        if (popupcart.length > 0) {

            $("#logged").removeClass("hidden");
            if (popupcart.indexOf('cart.aspx') >= 0) {
                if ($("#n-cart-down").length != 0) {
                    $("#n-cart-down").attr('id', 'cart-down');
                    $("li.cart").attr('onclick', 'cartclick();');
                }

                $("#cart-down").html(popupcart);
            }
            else {
                var res = popupcart.split("@")
                if (res.length > 0) {
                    if (res[0] == "False") {
                        var pid = $(res[1]).filter('.cart-wrapper').attr('id');
                        if (document.getElementById(pid)) {
                            $('#' + pid).remove();
                        }
                    }
                    if (document.getElementById("PopOrderSubTotal")) {
                        $(".cart-scroll").prepend(res[1]);
                        $("#PopMerchandise").html(res[2]);
                        $("#PopYouSave").html(res[3]);
                        $("#PopOrderSubTotal").html(res[4]);
                        $("span.c-cart").html(countitem);
                        $("#cart-notice li:last").html(res[5]);
                    }
                    else {
                        location.reload();
                    }

                }
            }
        }

        if (countitem.length > 0 && countitem.toString() != 'null') {
            if ($("#cart-count").length != 0) {
                $("#cart-count").html(countitem);
            }

            if ($("#n-cart-count").length != 0) {
                $("#n-cart-count").html(countitem);
            }

            if ($("#small-cart-count").length != 0) {
                $("#small-cart-count").html(countitem);
            }
        }

        /* show error */
        if (error.length > 0) {
            ShowError(error);
        }

        var arrQty = maxqty.split("|");
        var arrItem = id.split("|");
        var pathname = window.location.pathname;
        var bOK = 0;
        for (var i = 0; i < arrQty.length; i++) {
            // alert(arrQty[i]);
            if (arrQty[i] > 0) {
                /* show item with max qty */
                try {
                    var $txt = $('input[name="txtQtyItem' + arrItem[i] + '"]');
                    if ($txt.length > 0) {
                        for (var y = 0; y < $txt.length; y++) {
                            $txt.eq(y).val(arrQty[i]);
                        }
                    }
                } catch (err) { alert(err); }
            }
            else if (arrQty[i] == 0) {
                /* show item add cart success */
                try {
                    var $lbl = $('div[name="lblInCart' + arrItem[i] + '"]');
                    if ($lbl.length > 0) {
                        for (var y = 0; y < $lbl.length; y++) {
                            $lbl.eq(y).html('Added to your cart');
                            $lbl.eq(y).css("padding-top", "3px");
                            bOK = 1;
                        }
                    }

                    if ($('#divQty' + arrItem[i]).length != 0) {
                        $('#btnAddCart' + arrItem[i]).css('display', 'none');
                        $('#btnViewCart' + arrItem[i]).css('display', 'block');
                        
                        $('#divQty' + arrItem[i]).css('display', 'none');

                        if (pathname.indexOf('nail-collection') > -1) {
                            $('#txtQtyItem' + arrItem[i]).val('0');
                        }
                        else {
                            $('#txtQtyItem' + arrItem[i]).val('1');
                        }
                    }
                } catch (err) { alert(err); }
            }
            else if (arrQty[i] == -2) {
                /* item out of stock */
                try {
                    var $txt = $('input[name="txtQtyItem' + arrItem[i] + '"]');
                    if ($txt.length > 0) {
                        for (var y = 0; y < $txt.length; y++) {
                            $txt.eq(y).val(0);
                        }
                    }
                } catch (err) { alert(err); }
            }

            $('#btnAddCart' + arrItem[i]).removeClass('btn-active');
            $('#btnAddCart' + arrItem[i]).closest('.add-cart').closest('article').removeClass('active');
            
            $("#cart-down").removeClass('hide');
            if ($("#top-bar").hasClass('fixed-top') == true) {
                //$("#cart-down").addClass('cart-fixed');
                var w = $("#top-bar").width() - $("#cart-down").width();
                //$("#cart-down").css('left', w + 'px');
            }

            if (bOK == 1) {
                $("#cart-down").show().delay(3000).fadeOut("fast", function () {
                    //$("#cart-down").css('left', '');
                    $("#cart-down").removeClass('cart-fixed');
                    $("#cart-down").hide();
                });
            }
        }
    };

    methodHandlers.ShowPopupCart = function (htmlReturn, id, countitem, isDeleteFreeGift) {
        
        var popupcart = '';
        var error = '';
        if (htmlReturn != null) {
            if (htmlReturn.length > 0) {
                popupcart = htmlReturn[0];
                error = htmlReturn[1];
            }
        }
        if (popupcart.length > 0) {
            try { $("#cart-down").html(popupcart); } catch (err) { }
        }
        if (countitem.length > 0 && countitem.toString() != 'null') {
            if ($("#cart-count").length > 0) {
                $("#cart-count").html(countitem);
            }

            if ($("#n-cart-count").length > 0) {
                $("#n-cart-count").html(countitem);
            }

            if (countitem == 0) {
                //$('#cart-down').attr("id", "n-cart-down");
            }
            //$(".ic-cart").text(countitem);
        }

        if (error.length > 0) {
            ShowError(error);
        }
        else {

            AfterRemoveCartItem(id);
        }
    };

    function AfterRemoveCartItem(id) {
        try {
            var $lbl = $('div[name="lblInCart' + id + '"]');
            if ($lbl.length > 0) {
                for (var i = 0; i < $lbl.length; i++) { $lbl.eq(i).html(''); }
            }

//            var $txt = $('input[name="txtQtyItem' + id + '"]');
//            if ($txt.length > 0) {
//                for (var i = 0; i < $txt.length; i++) {
//                    if (document.URL.indexOf('purchased-product') > 0) {
//                        $txt.eq(i).val(0);
//                    }
//                    else if (document.URL.indexOf('collection') == -1) {
//                        $txt.eq(i).val(1);
//                    }
//                }
            //            }
            if (document.getElementById('divQty' + id)) {
                $('#btnAddCart' + id).css('display', 'block');
                $('#btnViewCart' + id).css('display', 'none');
                $('#divQty' + id).css('display', 'block');
            }
        } catch (err) { }
    }   


</script>
<%--<script src="/includes/scripts/timecountdown.js" type="text/javascript"></script>--%><!--1/9 tat-->
<script type="text/javascript">
    function AllowShowPopupCart() {
        var url = window.location.href;
        if (url.indexOf('/store/cart.aspx') > 0 || url.indexOf('/store/reward-point.aspx') > 0 || url.indexOf('/store/free-sample.aspx?act=checkout') > 0 || url.indexOf('/store/confirmation.aspx') > 0 || url.indexOf('/store/free-gift.aspx?act=checkout') > 0) {
            return false;
        }
        return true;
    }

    function ppoutcart() {
        if (window.ViewPortVidth() > 991) {
            $("#cart-down").removeClass("show");
            $("#cart-down").addClass("hide");
        }
    }

    function ppovercart() {
        if (window.ViewPortVidth() > 991) {
            $("#cart-down").stop();
            $("#cart-down").css('opacity', '1');
            if (AllowShowPopupCart()) {
                $("#cart-down").addClass("show");
                $("#cart-down").removeClass("hide");
            }
        }
    }

    function msearchclick() {
        if ($("#divSmallSearch").hasClass("hide")) {
            $("#divSmallSearch").html($("#divLargeSearch").html());
            $("#divLargeSearch").html('')
            $("#divSmallSearch").addClass("show");
            $("#divSmallSearch").removeClass("hide");
        }
        else {
            $("#divSmallSearch").addClass("hide");
            $("#divSmallSearch").removeClass("show");
            $("#divLargeSearch").html($("#divSmallSearch").html());
            $("#divSmallSearch").html('');
            
        }
    }

    function cartclick() {
        if (AllowShowPopupCart()) {
            if (window.ViewPortVidth() <= 1024 && window.ViewPortVidth() > 767) {
                fnResetAllPopup(4);
                if (($('#cart-down').hasClass("show") == false || $('#cart-down').hasClass("hide") == false) && ClickCart != 2) {
                    $("#cart-down").removeClass("hide");
                    $("#cart-down").addClass("show");
                    ClickCart = 1;
                    $("#logged .cart > a").click(function () {
                        if ($('#cart-down').hasClass("show")) {
                            $("#cart-down").removeClass("show");
                            ClickCart = 2;
                        }
                    });
                }
                else {
                    if (ClickCart != 1) {
                        $("#cart-down").addClass("hide");
                        $("#cart-down").removeClass("show");
                        ClickCart = 0;
                        $("#logged .cart > a").unbind("click");
                    }
                }

                $("#logged .cart > a").removeAttr("href");
            }
            else {
                if ($('#cart-down').hasClass("show") == false) {
                    $("#cart-down").removeClass("hide");
                    $("#cart-down").addClass("show");
                }
                //else {
                //    $("#cart-down").removeClass("show");
                //    $("#cart-down").addClass("hide");
                //}

                //if (window.ViewPortVidth() > 1024) {
                //    window.location.href = "/store/cart.aspx";
                //}
            }
        }
        else {
            window.location.href = "/store/cart.aspx";
        }
    }

    function ncartclick() 
    {
        if (window.ViewPortVidth() <= 991 && window.ViewPortVidth() > 767) {
            fnResetAllPopup(5);
            if ($('#n-cart-down').hasClass("show") == false) {
                $("#n-cart-down").removeClass("hide");
                $("#n-cart-down").addClass("show");
            } else {
                $("#n-cart-down").addClass("hide");
                $("#n-cart-down").removeClass("show");
            }
        }
    }

    var IsAllow = 0;
    var wait;
    $(window).resize(function () {
        clearTimeout(wait);
        wait = setTimeout(function () {
            fnCheckSmartPhone();

        }, 500);
        fnSetheightverlineDealsCenter(1);
        //fnChangeVisibleHeader();
        //fnResizeVisibleHeader();
        //fnOpenLinkAccountCart();
       
    });
    $(window).load(function () {
        //fnCheckSmartPhone();
        fnSetheightverlineDealsCenter(0);
        //fnChangeVisibleHeader();
        //fnResizeVisibleHeader();
        //fnOpenLinkAccountCart();
    });

    $(window).scroll(function () {
        //fnChangeVisibleHeader();
    });

    //Countdown 1/9 tat
//    TargetDate = '2015-08-31 23:59:59.000';
//    BackColor = "palegreen";
//    ForeColor = "navy";
//    CountActive = true;
//    CountStepper = -1;
//    LeadingZero = true;
//    DisplayFormat = "%%D%%d  %%H%%:%%M%%:%%S%%";
//    FinishMessage = "";
//    prefixmessage = "<span>Our 20th Anniversary Sale</span><span class='start-in'>&nbsp;| Ends in</span> &nbsp;";
//    //End Countdown
//    var getSecond = <%=getTimeEnd %>;
//    CountBack(getSecond);
</script>
