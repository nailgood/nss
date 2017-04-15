<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/main.master"
    AutoEventWireup="false" CodeFile="home.aspx.vb" Inherits="home" %>

<%@ Register Src="~/controls/banner/main.ascx" TagName="MainBanner" TagPrefix="uc" %>

<%@ Register Src="~/controls/banner/top-banner.ascx" TagName="topBanner" TagPrefix="uc" %>
<%@ Register Src="~/controls/banner/block-banner.ascx" TagName="blockBanner"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/resource-center/menu.ascx" TagName="ResourceCenter"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/product/shop-save.ascx" TagName="ShopSave" TagPrefix="uc" %>
<%@ Register Src="controls/layout/about-us.ascx" TagName="AboutUs" TagPrefix="uc1" %>
<%--<%@ Register Src="controls/layout/social-share.ascx" TagName="social" TagPrefix="uc2" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<%--<script type="application/ld+json">
    [{ "@context" : "http://schema.org",
                "@type" : "Organization",
                "name" : "Nailsuperstore",
                "logo" : "https://www.nailsuperstore.com/includes/theme/images/logo.png",
                "url" : "https://www.nailsuperstore.com",
                "sameAs" : [ "http://www.facebook.com/nailsuperstore",
                "http://twitter.com/nailsuperstore", "http://pinterest.com/nailsuperstore", " https://plus.google.com/b/102316218849562253438/+nailsuperstore/posts", "http://www.youtube.com/nailsuperstore"]
                },{
                "@context": "http://schema.org",
                "@type": "WebSite",
                "name" : "Nailsuperstore",
                "alternateName" : "Nailsuperstore",
                "url": "https://www.nailsuperstore.com/",
                "potentialAction": {
                "@type": "SearchAction",
                "target": "https://cse.google.com/cse/publicurl?cx=002746936636853198978:slkc9cw3p6y&q={search_term_string}",
                "query-input": "required name=search_term_string"
                }
            }]
</script>--%>
    <script type="application/ld+json">
    { "@context" : "http://schema.org",
                "@type" : "Organization",
                "name" : "Nailsuperstore",
                "logo" : "https://www.nailsuperstore.com/includes/theme/images/logo.png",
                "url" : "https://www.nailsuperstore.com",
                "sameAs" : [ "http://www.facebook.com/nailsuperstore",
                "http://twitter.com/nailsuperstore", "http://pinterest.com/nailsuperstore", " https://plus.google.com/b/102316218849562253438/+nailsuperstore/posts", "http://www.youtube.com/nailsuperstore"]
                }
            }
</script>
<%--    <link rel="stylesheet" href="includes/scripts/bxslider/jquery.bxslider.css" type="text/css" />
    <script type="text/javascript" src="includes/scripts/bxslider/jquery.bxslider.js" defer="defer"></script> Edit css.xml--%>
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <uc:topBanner ID="ucTopBanner" runat="server" />
    <div class="clearfix">
    </div>
    <uc:MainBanner ID="ucMainBanner" runat="server" />
    <uc:blockBanner ID="ucBlockBanner" runat="server" />
    <div class="clearfix">
    </div>
   
    <uc:ShopSave ID="ShopYourWay" Type="4" runat="server" />
    <uc:ShopSave ID="TopCategpry" Type="5" runat="server" />    
    <uc:ResourceCenter ID="ResourceCenter" runat="server" />
    <uc1:AboutUs ID="ucAboutUs" runat="server" />
<%--   <uc2:social ID="social1" runat="server" />--%>
    <script type="text/javascript">
        // $(document).ready(function () {
        var isInitSlide = false;
        $(window).load(function () {
            //ResetHeightTopBanner();
            //ResetHeightInforBanner();
            if (ViewPortVidth() <= 991) {
                ResetHeightShopSave();
            }
            else {
                ResetHeightShopSaveSlider();
                InitSlide();
            }
            
        });
        $(document).ready(function () {
          //  fnClickReadmoreAboutUs();
        });
        function InitSlide() {
            isInitSlide = true;
            $('.bxslider').bxSlider({
                minSlides: 4,
                maxSlides: 4,
                slideWidth: 360,
                slideMargin: 0,
                type: 2
            });
        }
        $(window).resize(function () {
            //ResetHeightTopBanner();
            //ResetHeightInforBanner();
            if (ViewPortVidth() <= 991) {
                ResetHeightShopSave();
            }
            else {
                if (!isInitSlide) {
                    ResetHeightShopSaveSlider();
                    InitSlide();
                }
            }
        });

        function ResetHeightInforBanner() {
            if (!document.getElementById('secInforBanner')) {
                return;
            }
            if (ViewPortVidth() <= 991) {
                var lstchild = $(".infor-banner").children('.infor-banner-item');
                ResetHeightList(lstchild, 'infor-banner');
                return;
            }
            //return;
            var lstReset = new Array();
            var defaultTop = -1;
            var maxRowHeight = 0;
            var maxNameHeight = 0;
            var maxDescHeight = 0;
            var $el;
            var index = 0;
            $(".infor-banner .infor-banner-item").each(function () {
                $el = $(this);
                $(this).children(".name").height('auto');
                $(this).children(".desc").height('auto');
                $($el).height('auto');
                $($el).removeClass("noneborderright")
                if (index < 2) {
                    $($el).css("border-top", "none");
                }
                index = index + 1;
                var position = $el.position();
                if (position.top != defaultTop) {

                    defaultTop = position.top;
                    if (lstReset.length > 0) {

                        ResetHeightInforBannerRow(lstReset, maxNameHeight, maxDescHeight);
                        lstReset.length = 0;
                        defaultTop = $el.position().top;
                    }
                    //else AddShopSaveBorderLeft($el)
                    maxNameHeight = 0;
                    maxDescHeight = 0;
                    if ($(this).children(".name").height() > maxNameHeight) {
                        maxNameHeight = $(this).children(".name").height()
                    }
                    if ($(this).children(".desc").height() > maxDescHeight) {
                        maxDescHeight = $(this).children(".desc").height()
                    }
                    lstReset.push($el);
                }
                else {

                    if ($(this).children(".name").height() > maxNameHeight) {
                        maxNameHeight = $(this).children(".name").height()
                    }
                    if ($(this).children(".desc").height() > maxDescHeight) {
                        maxDescHeight = $(this).children(".desc").height()
                    }
                    lstReset.push($el);
                }
            })
            if (lstReset.length > 0) {
                ResetHeightInforBannerRow(lstReset, maxNameHeight, maxDescHeight);
            }

        }
        function ResetHeightInforBannerRow(lstReset, maxNameHeight, maxDesHeight, imageHeight) {

            for (var j = 0; j < lstReset.length; j++) {
                lstReset[j].children(".name").height(maxNameHeight);
                lstReset[j].children(".desc").height(maxDesHeight);
            }
            lstReset[lstReset.length - 1].addClass('noneborderright');
        }
        function ResetHeightTopBanner() {
            var lstchild = $('.top-banner ul').children('.top-banner-item');

            ResetHeightList(lstchild, 'top-banner');

        }
    </script>
</asp:Content>
