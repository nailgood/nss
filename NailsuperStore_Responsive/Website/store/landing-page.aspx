<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="true" CodeFile="landing-page.aspx.vb" Inherits="store_landing_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">

  	<%--<script src="/includes/jquery.qtip.min.js" type="text/javascript"></script> <script type="text/javascript">
          <link href="/includes/nyroModal/styles/nyroModal.full.css" rel="stylesheet" type="text/css" />
    <script src="/includes/nyroModal/js/jquery.nyroModal-1.6.2.min.js" type="text/javascript"></script> Edit css.xml--%>
     <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
       var ProductInfo = '';
       var w = 450;
       var arrProductInfo = new Array();
   </script> <%--<div id="dvBanner" style="margin-bottom:10px;"><img alt="" src="/images/LP_Banner.jpg" /> </div>--%><asp:UpdatePanel ID="up" runat="server"><ContentTemplate>
   <asp:UpdateProgress 
    ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up"><ProgressTemplate><center>Please wait...<br /><img 
        alt="" src="/includes/theme/images/loader.gif" /> </center></ProgressTemplate></asp:UpdateProgress><div 
    ID="divOverlay" class="clipOverlay" style="display:none;">&#160;</div><div 
    ID="divBackground" class="lightbox" style="display: none;">&#160;</div><div 
    ID="dError" style="display:none;"><div ID="diverror"><div class="right"></div><div 
            class="left"></div>
                <div class="title"><img src="/includes/theme/images/icon_error.gif" /> <div 
                        class="txt">This form was not processed due to the following reasons:</div></div><div 
            ID="dErrorContent" class="content"></div>
                <div class="closeLink"><a class="center noul" href="javascript:void(0);" 
                        onclick="hideAddCardError()">Close Window</a></div><div 
            class="bot"><div class="rightbot"></div> <div class="leftbot"></div></div>
            </div>       
        </div>  
        <script src="/includes/popup.js" type="text/javascript"></script> 
       <asp:Literal 
    ID="litHTML" runat="server"></asp:Literal><%  If (isShowRelatedItem) Then%> <table 
    ID="tblTabs" runat="server" border="0" cellpadding="0" cellspacing="0" 
    style="width:754px;margin:10px 0 0 0;" summary="tabbed content"><tr><td 
            class="tabbao"><div><div ID="tabnav_relateditems" class="tabon">Related Items</div><div 
                ID="tabnav_relateditems_r" class="tabon_r"></div>
            </div>
        </td>
    </tr>
    <tr><td><%--<div ID="tab_relateditems" class="product-contenttab"><uc2:RelateItemsNew 
            ID="RelateItemsNew1" runat="server" /></div>--%>
	    </td>
    </tr>
    </table>
<%  End If%> <asp:HiddenField ID="hdQuantity" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdCouponCode" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdVideoId" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdItemId" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdItemSKU" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdMultiItem" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdShowPopupCart" runat="server"></asp:HiddenField>

<asp:LinkButton  id="lnkAddCart" runat="server"></asp:LinkButton><asp:LinkButton  id="lnkAdd2Wishlist" runat="server"></asp:LinkButton></ContentTemplate></asp:UpdatePanel><a href="#" id="nyroModal" target="_blank"></a>
    <script type="text/javascript">
    function AddToCart(quantity, CouponCode) {
        if (!isNaN(quantity)) {
                var txtCouponCode = document.getElementById("hdCouponCode");
                var txtQuantity = document.getElementById("hdQuantity");
                txtQuantity.value = quantity;
                txtCouponCode.value = CouponCode;
                var lnkAddCart = 'lnkAddCart';
                __doPostBack(lnkAddCart, '');
        }
        else {
            ShowAddCardError('Please enter a valid quantity.');
        }
    }
    function AddToCartSKU(sku, quantity, CouponCode, isShowPopup) {
        if (!isNaN(quantity)) {
            var txtCouponCode = document.getElementById("hdCouponCode");
            var txtSKU = document.getElementById("hdItemSKU");
            var txtQuantity = document.getElementById("hdQuantity");
            var txtShowPopup = document.getElementById("hdShowPopupCart");
            txtSKU.value = sku;
            txtQuantity.value = quantity;
            txtCouponCode.value = CouponCode;
            if (isShowPopup == true) {
                txtShowPopup.value = 1;
            }
            else {
                txtShowPopup.value = 0;
            }
            var lnkAddCart = 'lnkAddCart';
            __doPostBack(lnkAddCart, '');
        }
        else {
            ShowAddCardError('Please enter a valid quantity.');
        }
    }
    function AddToCart2(id, quantity, CouponCode) {       
        if (!isNaN(quantity)) {
            var txtItemId = document.getElementById("hdItemId");
            var txtCouponCode = document.getElementById("hdCouponCode");
            var txtQuantity = document.getElementById("hdQuantity");
            var txtShowPopup = document.getElementById("hdShowPopupCart");
            txtShowPopup.value = 1;
            txtQuantity.value = quantity;
            txtCouponCode.value = CouponCode;
            txtItemId.value = id;
            var lnkAddCart = 'lnkAddCart';
            __doPostBack(lnkAddCart, '');
        }
        else {
            ShowAddCardError('Please enter a valid quantity.');
        }
    }
    function AddMultiToCart(quantity) {
        var item = document.getElementById("hdMultiItem")
        if (item.value.length > 0 && item.value != ',') {
            if (!isNaN(quantity)) {
                var txtQuantity = document.getElementById("hdQuantity");
                txtQuantity.value = quantity;
                var lnkAddCart = 'lnkAddCart';
                __doPostBack(lnkAddCart, '');
            }
            else {
                ShowAddCardError('Please enter a valid quantity.');
            }
        }
    }
    function AddMultiItem(sku) {
        
        var txtMultiItem = document.getElementById("hdMultiItem");
        var chk = document.getElementById('ckbox_' + sku);
       
        if (txtMultiItem.value.length < 1) {
            txtMultiItem.value = ",";
        }
        if (chk.checked) {
            txtMultiItem.value = txtMultiItem.value + sku + ',';
        }
        else {
            txtMultiItem.value = txtMultiItem.value.replace(',' + sku + ',', ',');
        }
    }
    function AddMultiQtyItemToCart() {
        var txtMultiItem = document.getElementById("hdMultiItem");
        txtMultiItem.value = "";
        var error = true;
        var noQty = true;
        var arr = document.getElementsByTagName("input");
        for (var i = 0; i < arr.length; i++) {            
            var t = arr[i].id.indexOf("_txtQtyItem");
            if (t > 0) {
                var sku = arr[i].id.replace("_txtQtyItem", "");
                var qty = arr[i].value;
                if (!isNaN(qty)) {
                    if (parseInt(qty) > 0) {
                        txtMultiItem.value = txtMultiItem.value + sku + ":" + qty + "|";
                        noQty = false;
                        error = false;
                    }
                }
                else {
                    ShowAddCardError('Please enter a valid quantity.');
                    error = true;
                    return;
                }
            }
        }
        if (noQty == true) {
            ShowAddCardError('Please select at least one item to add to your shopping cart.');
            return;
        }
        else if (error == false) {
            var lnkAddCart = 'lnkAddCart';
            __doPostBack(lnkAddCart, '');
        }
    }
    
    function Add2WishList(sku, quantity) {
        if (!isNaN(quantity)) {
            var txtSKU = document.getElementById("hdItemSKU");
            var txtQuantity = document.getElementById("hdQuantity");
            txtSKU.value = sku;
            txtQuantity.value = quantity;
            var lnkAdd2Wishlist = 'lnkAdd2Wishlist';
            __doPostBack(lnkAdd2Wishlist, '');
        }
        else {
            ShowAddCardError('Please enter a valid quantity.');
        }
    }
    
    function ShowAddCardError(message) {
        var dError = document.getElementById('dError');
        var divBackground = document.getElementById('divBackground');
        f_putScreen(dError, divBackground, true);
        divBackground.style.height = $(document).height();
        document.getElementById('dErrorContent').innerHTML = message;
    }
    function hideAddCardError() {
        var dError = document.getElementById('dError');
        var divBackground = document.getElementById('divBackground');
        f_putScreen(dError, divBackground, false);
    }
    function ShowPopupCart() {
        DisplayPopupCart();
        var timeOut = setTimeout(function() {
            OutCart();
        }, 4000);
        $('#ulMain').hover(function() { clearTimeout(timeOut); });
    }
    function scrollTop() {
        $('html, body').animate({ scrollTop: 0 }, 'fast');
        //$('html, body').scrollTop(0,0);
    }

    $(document).ready(function() {
        var VideoId = document.getElementById('hdVideoId').value;
        if (VideoId != '' && VideoId != 'underfied') {
            if (parseInt(VideoId) > 0) {
                $('#aViewVideo').click(function() {
                    var more = true;
                    var link = '/Popup/ViewVideo.aspx?vid=' + VideoId;
                    var heightPopup = 545;
                    if (more == 'True') {
                        heightPopup = heightPopup + 30;
                        link = link + "&more=1"
                    }
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
                });
            }
        }
        if (ProductInfo != '') {
            $('#aProductInformation').qtip(
            {
                content: {
                    title: {
                        text: 'Product Information',
                        button: 'Close'
                    },
                    text: ProductInfo
                },
                position: {
                    target: $(document.body), // Position it via the document body...
                    corner: 'center' // ...at the center of the viewport
                },
                show: {
                    when: 'click', // Show it on click
                    solo: true // And hide all other tooltips
                },
                hide: false,
                style: {
                    width: { max: w },
                    padding: '14px',
                    border: {
                        width: 9,
                        radius: 9,
                        color: '#666666'
                    },
                    name: 'light'
                },
                api: {
                    beforeShow: function() {
                        // Fade in the modal "blanket" using the defined show speed
                        $('#qtip-blanket').fadeIn(this.options.show.effect.length);
                    },
                    beforeHide: function() {
                        // Fade out the modal "blanket" using the defined hide speed
                        $('#qtip-blanket').fadeOut(this.options.hide.effect.length);
                    }
                }
            });
        }
        else if (arrProductInfo.length > 0) {
            var i = 0;
            $('a[id^=aProductInformation]').each(function() {
                if (i <= arrProductInfo.length - 1 && arrProductInfo[i] != '' && arrProductInfo[i] != 'undefined') {
                    $(this).qtip(
                    {
                        content: {
                            title: {
                                text: 'Product Information',
                                button: 'Close'
                            },
                            text: arrProductInfo[i]
                        },
                        position: {
                            target: $(document.body), // Position it via the document body...
                            corner: 'center' // ...at the center of the viewport
                        },
                        show: {
                            when: 'click', // Show it on click
                            solo: true // And hide all other tooltips
                        },
                        hide: false,
                        style: {
                            width: { max: w },
                            padding: '14px',
                            border: {
                                width: 9,
                                radius: 9,
                                color: '#666666'
                            },
                            name: 'light'
                        },
                        api: {
                            beforeShow: function() {
                                // Fade in the modal "blanket" using the defined show speed
                                $('#qtip-blanket').fadeIn(this.options.show.effect.length);
                            },
                            beforeHide: function() {
                                // Fade out the modal "blanket" using the defined hide speed
                                $('#qtip-blanket').fadeOut(this.options.hide.effect.length);
                            }
                        }
                    });
                }
                i = i + 1;
            });

        }
        // Create the modal backdrop on document load so all modal tooltips can use it
        $('<div id="qtip-blanket">').css({
            position: 'absolute',
            top: $(document).scrollTop(), // Use document scrollTop so it's on-screen even if the window is scrolled
            left: 0,
            height: $(document).height(), // Span the full document height...
            width: '100%', // ...and full width

            opacity: 0.7, // Make it slightly transparent
            backgroundColor: 'black',
            zIndex: 5000  // Make sure the zIndex is below 6000 to keep it below tooltips!
        })
      .appendTo(document.body) // Append to the document body
      .hide(); // Hide it initially

    });

</script>

<asp:Literal id="ltrScript" runat="server"></asp:Literal>

</asp:Content>