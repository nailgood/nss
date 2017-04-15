<%@ Control Language="VB" AutoEventWireup="false" CodeFile="footer.ascx.vb" Inherits="controls_Footer" %>
<%@ Register Src="~/controls/layout/social-share.ascx" TagName="social" TagPrefix="uc1" %>
<%@ Register Src="~/controls/checkout/secure-icon.ascx" TagName="secure" TagPrefix="uc2" %>

<%  If (ItemCollection IsNot Nothing AndAlso ItemCollection.Count > 0) Then%>
<div id="recentlyView" class="shopway">
    <div class="shopsave-title">
        <a id="recentlyView_link" href="/store/recently-viewed.aspx">Recently View</a>
    </div>
    <div class="slider" id="shopsaveslider_45">
        <ul class="bxslider" id="ulSlideContentRc" runat="server">
            <% For index = 0 To ItemCollection.Count - 1 %>
            <div id='<%= ItemCollection(index).ItemId %>' class="<%=IIf(index Mod 2 <> 0, "shop-save-item smalllast", "shop-save-item smallfirst") %>" >
                <div class="recently-view-close" onclick="deleteRecentlyItemFt('<%= ItemCollection(index).ItemId %>');return false;">
                    <span class="footer">Close</span>
                </div>
                <div class="hidden-left">
                    &nbsp;
                </div>
                <div class="hidden-top">
                    &nbsp;
                </div>
                <div style="height: auto;" class="box" title="<%= ItemCollection(index).Title %>">
                    <a href="<%= ItemCollection(index).Url %>">
                        <picture>
                            <source srcset="<%= ItemCollection(index).Image %>" media="(min-width: 992px)">
                            <source srcset="<%= ItemCollection(index).Image %>" media="(max-width: 480px)">
                            <img src="<%= ItemCollection(index).Image %>" alt="<%= ItemCollection(index).Title %>">
                        </picture>
                    </a>
                </div>
                <div class="name">
                    <a href="<%= ItemCollection(index).Url %>"><%= ItemCollection(index).Title %></a>
                </div>
                <div class="hidden-bottom">
                    &nbsp;
                </div>
                <div class="hidden-right">
                    &nbsp;
                </div>
            </div>
            <%   Next %>
        </ul>
       
    </div>
</div>
<script>
    var bxslider;

    function setMargin(sliderNum) {
        $('#shopsaveslider_45 > .bx-wrapper').css('margin', '0 auto');

        var height = ($("#recentlyView").height() - 145)/2;
        $("#shopsaveslider_45 > .bx-wrapper > div > .bx-controls-direction > .bx-prev").css('top', height + 'px');
        $("#shopsaveslider_45 > .bx-wrapper > div > .bx-controls-direction > .bx-next").css('top', height + 'px');
    }

    function setHeight() {
        var maxHeight = -1;

        $('#ulSlideContentRc > .shop-save-item').each(function () {
            maxHeight = maxHeight > $(this).height() ? maxHeight : $(this).height();
        });

        $('#ulSlideContentRc > .shop-save-item').each(function () {
            $(this).height(maxHeight);
        });

       
    }

    function InitSlide() {
        isInitSlide = true;

        if (bxslider != undefined)
            bxslider.destroySlider();

        var sliderNum;
        var widthRecentlyView = $('#recentlyView').width();
        var sliderWidth = 185, marginSlider = 20;
        var marginSliderLeftRight = 100 * 2;


        sliderNum = Math.floor((widthRecentlyView - marginSlider + marginSlider - 80) / (sliderWidth + marginSlider));
        if (sliderNum < 4) sliderNum = 4;
        
        bxslider = $('.bxslider').bxSlider({
            minSlides: sliderNum,
            maxSlides: sliderNum,
            slideWidth: sliderWidth,
            slideMargin: marginSlider,
            type: 2
        });
        bxslider.reloadSlider();
        setHeight();
        setMargin();

    }
    $(document).ready(function () {
        if ($('#main').length == 1 && $('#recentlyView').length == 1) {
            $('#main').append($('#recentlyView'));
            //$('#main').css('margin-bottom', '-40px');
        }
        $('#recentlyView').css('visibility', 'hidden');
        $('#recentlyView').css('clear', 'both');

    });
    $(window).resize(function () {
        var isInitSlide = false;
        
        InitSlide();
        setHeight();
        setMargin();
        $('#recentlyView').css('visibility', '');
    });
    $(window).on("orientationchange load", function () {
       
        InitSlide();
        setHeight();
        setMargin();
        $('#main').css('margin-bottom', '-40px');
        $('#recentlyView').css('visibility', '');
    });

    function deleteRecentlyItemFt(itemId, itemName) {
        mainScreen.ExecuteCommand('deleteRecentlyViewItem', 'methodHandlers.deleteRecentlyViewItemCallback', [itemId, itemName]);
        return false;
    }
    methodHandlers.deleteRecentlyViewItemCallback = function (temp, itemId) {

        if (itemId.length > 0) {
            $('#' + itemId).remove();
            if ($('#ulSlideContentRc').children().length == 0) {
                $('#recentlyView').remove();
                $('#main').css('margin-bottom', '0');
            }
            else {
                bxslider.reloadSlider();
                setHeight();
                setMargin();
            }
        }

    }
</script>
<%  End If%>

<footer>
    <div class="header">

        <div class="container">
            <div class="leave-message">
                <div id="scHwEb2"></div>
                <div id="scPr0e2"></div>
            </div>
            <span class="phone"></span>
            <div class="phone1">
                <span>800.669.9430 / 847.260.4000 US</span>
            </div>
            <div class="phone2">
                +1.847.260.4000 <span>International</span>
            </div>     
        </div>

    </div>
    <div class="menu">
        <div class="container">
            <div class="content">
                <div class="item hidden-xs hidden-sm visible-md visible-lg">
                    <ul>
                        <li class="itemlabel">Shop by Products</li>
                        <asp:Literal runat="server" ID="ltrAltMainDepartment"></asp:Literal>
                    </ul>
                </div>
                <div class="item company">
                    <ul>
                        <li class="itemlabel">Company Info</li>
                        <li><a href="/services/about-company.aspx">About Us</a></li>
                        <li><a href="/service/default.aspx">Customer Service</a></li>
                        <li><a href="/service/about-directions-to-our-warehouse.aspx">Directions</a></li>
                        <li><a href="/services/legal-user-agreement.aspx">User Agreement</a></li>
                        <li><a href="/services/legal-privacy-policies.aspx">Privacy Policy</a></li>
                        <li><a href="/sitemap/default.aspx">Site Map</a></li>
                    </ul>
                    <ul class="hidden-xs hidden-sm visible-md visible-lg">
                        <li class="itemlabel">My Account</li>
                        <li><a href="/members/login.aspx">Sign In</a></li>
                        <li><a href="/members/default.aspx">Account Details</a></li>
                        <li><a href="/members/address.aspx">Edit Account</a></li>
                        <li><a href="/members/orderhistory/">Order History</a></li>
                        <li><a href="/members/pointbalance.aspx">Cash Reward Points Balance</a></li>
                        <li><a href="/members/unsubscribe.aspx">Unsubscribe Email</a></li>
                    </ul>
                </div>
                <div class="item ">
                    <ul>
                        <li class="itemlabel">Contact Us</li>
                        <li><a href="/contact/generalquestion.aspx">General Question</a></li>
                        <li><a href="/contact/returnauthorizationrequest.aspx">Return Request</a></li>
                        <li><a href="/contact/wholesaleinquiry.aspx">Wholesale Inquiry</a></li>
                    </ul>
                    <ul class="itemsub">
                        <li class="itemlabel">Catalog</li>
                        <li><a href="/store/quickorder.aspx">Catalog Quick Order</a></li>
                        <li><a href="/service/catalog.aspx">Online Catalog</a></li>
                        <li><a href="/service/requestcatalog.aspx">Catalog Request</a></li>
                    </ul>
                    <ul class="hidden-xs hidden-sm visible-md visible-lg">
                        <li class="itemlabel">Helpful Links</li>
                        <li><a href="/tips">Expert Tips & Advice</a></li>
                        <li><a href="/video-topic">How-To Videos</a></li>
                        <li><a href="/news-topic">News & Events</a></li>
                        <li><a href="/media-topic">As Seen In Media</a></li>
                        <li><a href="/order-reviews">Customer Testimonials</a></li>
                        <li><a href="/product-reviews">Product Reviews</a></li>
                    </ul>
                </div>
                <div class="item visible-xs visible-sm hidden-md hidden-lg">
                    <ul>
                        <li class="itemlabel">Helpful Links</li>
                        <li><a href="/tips">Expert Tips & Advices</a></li>
                        <li><a href="/video-topic">How To Videos</a></li>
                        <li><a href="/news-topic">News & Events</a></li>
                        <li><a href="/media-topic">As Seen In Media</a></li>
                        <li><a href="/order-reviews">Customer Feedbacks</a></li>
                        <li><a href="/product-reviews">Product Reviews</a></li>
                        <li><a href="/shop-by-design">Shop by Design</a></li>
                    </ul>
                </div>
                <div class="item payment">
                    <ul>
                        <li class="itemlabel">Shipping & Returns</li>
                        <li><a href="/service/myorder.aspx">Track Order</a></li>
                        <li><a href="/service/order.aspx">Shipping</a></li>
                        <li><a href="/service/return.aspx">Returns</a></li>
                        <li><a href="/services/order-international-shipment.aspx">International Shipping</a></li>
                        <li><a href="/services/order-truck-delivery.aspx">Truck Delivery</a></li>
                        <li><a href="/services/order-residential-delivery.aspx">Residential Delivery</a></li>
                        <li><a href="/services/order-delivery-time.aspx">Delivery Time</a></li>
                    </ul>
                    <ul class="hidden-xs hidden-sm visible-md visible-lg">
                        <li>* Free Shipping for orders of $<%=Utility.ConfigData.FreeShippingOrderAmount() %> or more is available to the continental United States only. Excludes Alaska, Hawaii, and Puerto Rico.</li>
                    </ul>
                </div>
                <div class="item ">
                    <div class="hidden-xs hidden-sm visible-md visible-lg">
                        <ul>
                            <li class="itemlabel">Enter your email for exclusive money-saving offers</li>
                            <li>
                                <div class="signup hidden-xs hidden-sm visible-md visible-lg">
                                    <div class="form ">
                                        <asp:Panel ID="panSubscribe" runat="server" DefaultButton="btnSubscribe">
                                            <asp:TextBox ID="txtEmail" CssClass="form-control email" placeholder="Enter your Email Address" runat="server">
                                            </asp:TextBox>
                                            <asp:Button ID="btnSubscribe" CssClass="btn btn-submit" runat="server" Text="Submit" />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <ul>
                            <li class="itemlabel">Give us a call</li>
                            <li>1.800.669.9430/1.847.260.4000<br />
                                +1.847.260.4000 International<br />
                                Mon - Fri 9am - 5:30pm CST
                            </li>
                        </ul>
                        <ul>
                            <li class="itemlabel">Stay informed and get connected</li>
                            <li>The Nail Superstore<br />
                                3804 Carnation St<br />
                                Franklin Park, IL 60131, USA
                            </li>
                        </ul>
                        <ul>
                            <li>
                                <uc1:social ID="social" runat="server" />
                            </li>
                        </ul>
                        <ul>
                            <li>
                                <uc2:secure ID="ucSecure" runat="server" IsCartPage="false" />
                            </li>
                        </ul>
                    </div>
                    <ul class="visible-xs visible-sm hidden-md hidden-lg">
                        <li>* Free Shipping for orders of $<%=Utility.ConfigData.FreeShippingOrderAmount() %> or more is available to the continental United States only. Excludes Alaska, Hawaii, and Puerto Rico.</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="copyright">
        ©2016 Nail Superstore, All rights Reserved.
    </div>
</footer>
<%--<div id="department-wrapper" class="hidden-xs hidden-sm visible-md visible-lg">
    <div class="container">
        <div class="title">
            Departments
        </div>
        <div class="detail">
            <asp:Literal runat="server" ID="ltrAltMainDepartment"></asp:Literal>
        </div>
    </div>
</div>--%>

<div id="imgTop" class="backtotop" style="display: none"></div>
<script>
    $(window).load(function () {
        fnClicktoTop();
    });
</script>
