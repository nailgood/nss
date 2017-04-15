<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cartScript.ascx.vb" Inherits="controls_checkout_cartScript" %>
<script type="text/javascript">
    function UpdateCartSummaryBox(htmlSumaryBox) {//popup select free item se goi xuong ham nay
       
        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
        }
    }
    function ShowBonusOffer(id) {

        var link = '/includes/popup/special-offer.aspx?mmid=' + id;

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
        return false;
    }
    function showDiscountItem( mId,orderId) {

        var link = '/includes/popup/free-item.aspx?orderId=' + orderId + '&mmId=' + mId + '&discount=1';
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
            titleFromIframe: false,
            endRemove: function () {
                mainScreen.ExecuteCommand('RefeshCart', 'methodHandlers.RefeshCartCallBack', []);
            }   
        });
        return false;

    }
    function InputCartQty(cartItemId, strName, event) {
        if (event.keyCode == 13) {
            event.preventDefault();
        }

        if (numbersonly(strName, event)) {
            OnGotFocus(cartItemId, strName);
        }
    }
    function OnGotFocus(cartItemId, strName) {
        if (document.getElementById('pnUpdate_' + cartItemId)) {
            $('#pnUpdate_' + cartItemId).css('display', 'block');
        }

        /*   var lnkSave = document.getElementById('lnkSave_' + strName);
        lnkSave.style.display = "inline";
        var lnkCancel = document.getElementById('lnkCancel_' + strName);
        lnkCancel.style.display = "inline";*/

    }
    function LostGotFocus(cartItemId) {
        if (document.getElementById('pnUpdate_' + cartItemId)) {
            $('#pnUpdate_' + cartItemId).css('display', 'none');
        }
    }
    function IncreaseCart(cartItemId, strName) {
        var ctrl = document.getElementById(strName);
        var value = parseInt(ctrl.value, 10);
        if (value < 9999) ctrl.value = value + 1;
        OnGotFocus(cartItemId, strName);

    }

    function DecreaseCart(cartItemId, strName) {
        var ctrl = document.getElementById(strName);
        var value = parseInt(ctrl.value, 10);
        if (value > 0) {
            ctrl.value = value - 1;
            OnGotFocus(cartItemId, strName);
        }
    }

    function CancelUpdate(cartItemId, strName, qty) {
        var ctrl = document.getElementById(strName);
        ctrl.value = qty;
        LostGotFocus(cartItemId);
    }

    function OnChangeQty(cartItemId) {

        var arr = [cartItemId];
        var lstIDUpdate = '';
        var lstQty = '';
        var hasUpdate = false;
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                var id = arr[i].toString();
                if (document.getElementById('pnUpdate_' + id)) {
                    var display = $("#pnUpdate_" + id).css('display');
                    if (display == 'block') {
                        var qty = $("#txtQty_" + id).val();
                        if (!isInteger(qty)) {
                            ShowCheckOutCartError('Please input a valid quantity', id)
                            return;
                        }
                        hasUpdate = true;
                        lstIDUpdate = lstIDUpdate + id + ';'
                        lstQty = lstQty + qty + ';'
                    }
                }
            }
        }

        if (hasUpdate) {
            var cartItemUpdateId = cartItemId;
            $("#pnUpdate_" + cartItemUpdateId).hide();
            $('.remove').find('a').addClass('active');
            mainScreen.ExecuteCommand('UpdateCartQty', 'methodHandlers.UpdateCartResultCallBack', [cartItemUpdateId, lstIDUpdate, lstQty]);
        }
        return false;
    }

    function ShowError(msg) {
        showQtip('qtip-error', msg, 'Ooops');
    }

//    function ShowFlammableTip() {
//        showQtip('qtip-msg', 'In compliance with U.S Department of Transportation, IATA laws & regulations, some of your items are required an extra handling fee of $62.5 per shipment (not per item) for international or $40 for US express shipments. <a style="color: #3b76ba; text-decoration: underline;" href="/services/order-shipping-restrictions.aspx">Click here for more information.</a>', 'Flammable & Hazardous Material Item');
//    }

    function OnSaveCartLoginFirst(cartItemId) {
        window.location.href= '/members/login.aspx?url=/store/cart.aspx';
    }

    function OnRemoveCartItem(cartItemId) {
        $('.remove').find('a').addClass('active');
        $("#trRow_" + cartItemId).toggle(300, function () {
            if ($(this).attr('class').indexOf('has-free') >= 0) {
                $(this).closest('tr').next().remove();
            }

            $(this).remove();
            CheckEmptyCart();
        });
        mainScreen.ExecuteCommand('DeleteCartItem', 'methodHandlers.DeleteCartResultCallBack', [cartItemId]);
    }

    function OnRemoveCartFreeItem(cartItemId) {
        $('.remove').find('a').addClass('active');
        $("#trRow_" + cartItemId).toggle(300, function () {
            $(this).remove();
        });
        mainScreen.ExecuteCommand('DeleteCartItem', 'methodHandlers.DeleteCartResultCallBack', [cartItemId]);
    }

    function OnRemoveSaveCartItem(itemId, isCase) {
        $('.remove').find('a').addClass('active');
        $("#trSaveRow_" + itemId).toggle(300, function () {
            $(this).remove();
        });
        mainScreen.ExecuteCommand('DeleteSaveCartItem', 'methodHandlers.DeleteSaveCartResultCallBack', [itemId, isCase]);
    }

    function OnMoveSaveCartItem(itemId, qty, isCase) {
        $('.remove').find('a').addClass('active');
        mainScreen.ExecuteCommand('MoveSaveCartItem', 'methodHandlers.MoveSaveCartResultCallBack', [itemId, qty, isCase]);
    }

    function CheckEmptyCart() {
        if ($('#tabCart tr').length <= 1) {
            $("#ctl00_cphContent_uCart_divEmpty").show();
            $("#tabCart").remove();
            $("#ctl00_cphContent_divSendCart").remove();
            $("#h-btncart").remove();
            $("#dvcon").remove();
            $("#divEstimateShippingResult").remove();
        }
    }
    function OnSaveCartItem(cartItemId) {
        $('.remove').find('a').addClass('active');
        $("#trRow_" + cartItemId).toggle(300, function () {
            if ($(this).attr('class').indexOf('has-free') >= 0) {
                $(this).closest('tr').next().remove();
            }

            $(this).remove();
            CheckEmptyCart();
        });
        mainScreen.ExecuteCommand('SaveCartItem', 'methodHandlers.SaveCartResultCallBack', [cartItemId]);
    }

    function OnRemoveCartFreeGift(cartItemId) {
        $('.remove').find('a').addClass('active');
        $("#trRow_" + cartItemId).toggle(300, function () {
            if ($(this).attr('class').indexOf('has-free') >= 0) {
                $(this).closest('tr').next().remove();
            }

            $(this).remove();
            CheckEmptyCart();
        });

        mainScreen.ExecuteCommand('DeleteCartFreeGiftItem', 'methodHandlers.DeleteCartFreeGiftResultCallBack', [cartItemId]);
    }

    methodHandlers = {};
    methodHandlers.SaveCartResultCallBack = function (html, cartItemCount, isOK, linkredirect) {

        if (linkredirect != '') {
            window.location.href = linkredirect;
        }

        var htmlCart = '';
        var htmlEstimatePopup = '';
        var htmlSumaryBox = '';
        var htmlFreeGift = ''
        var htmlFreeSamples = ''
        var htmlSaveCart = '';
        var htmlRewardsPoints = '';
        var linkRedirect = '';
        var htmlError = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlFreeGift = html[3];
            htmlFreeSamples = html[4];
            htmlRewardsPoints = html[5];
            htmlError = html[6];
            htmlSaveCart = html[7];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                $('.remove').find('a').removeClass('active');
                return;
            }
        }
        if (document.getElementById('secFreeGift')) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }
        if (document.getElementById('secFreeSamples')) {
            $('#secFreeSamples').replaceWith(htmlFreeSamples);
        }
        if (document.getElementById('secRewardsPointsCart')) {
            $('#secRewardsPointsCart').replaceWith(htmlRewardsPoints);
        }
        if (document.getElementById('secSaveCart')) {
            $('#secSaveCart').replaceWith(htmlSaveCart);
        }
        if (document.getElementById('secCartList')) {
            if (htmlCart.length > 0) $('#secCartList').replaceWith(htmlCart);
        }
        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
            if (htmlSumaryBox.length <= 0) {
                $('#secEstimateShipping').replaceWith('');
                $('#ctl00_divEmpty').css('display', 'block');
            }
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }
        
        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');
                $('#secOrderSummary').addClass('summaryestimate');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }
        $('.remove').find('a').removeClass('active');
    }

//    function RecalculatePointEarn()
//    {
//        var total = 0;
//        ($("#ctl00_lblSubTotal").html().replace('$', '').replace(',', ''));
//        total = Math.round(total).toFixed(0);
//        var mep = $("#hidMoneyEachPoint").val();
//        var totalearn = parseFloat(total * mep);
//        $("#orderpointearn").html('$' + totalearn + ' (' + total + ' in points)');
//    }

    methodHandlers.MoveSaveCartResultCallBack = function (html, strMaxQty, cartItemCount, SaveCartCount, isOK, linkRedirect, itemId) {
        if (linkRedirect != '') {
            window.location.href = linkredirect;
            return;
        }
        if (!document.getElementById('secFreeGift')) {
            location.reload();
            return;
        }
        var htmlCart = '';
        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var htmlFreeGift = '';
        var htmlFreeSamples = '';
        var htmlRewardsPoints = '';
        var htmlError = '';
        var htmlSaveCart = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlFreeGift = html[3];
            htmlFreeSamples = html[4];
            htmlRewardsPoints = html[5];
            htmlError = html[6];
            htmlSaveCart = html[7];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                $('.remove').find('a').removeClass('active');
                return;
            }
        }

        if (document.getElementById('secFreeGift')) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }
        if (document.getElementById('secFreeSamples')) {
            $('#secFreeSamples').replaceWith(htmlFreeSamples);
        }
        if (document.getElementById('secRewardsPointsCart')) {
            $('#secRewardsPointsCart').replaceWith(htmlRewardsPoints);
        }
        if (document.getElementById('secCartList')) {
            $('#secCartList').html(htmlCart);
        }
        if (document.getElementById('secSaveCart')) {
            $('#secSaveCart').replaceWith(htmlSaveCart);
        }
        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
        }
        else {
            location.reload();
            return;
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }

        if (document.getElementById('savecart-count')) {
            if (SaveCartCount < 1) {
                $('#secSaveCart').css('display', 'none');
            }
            else {
                var s = SaveCartCount + ' item';
                if (SaveCartCount > 1)
                    s += 's';
                $("#savecart-count").html(s);
            }
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');
                $('#secOrderSummary').addClass('summaryestimate');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        //ResetEstimateShippingDefault();
        $('.remove').find('a').removeClass('active');

        //$("#trSaveRow_" + itemId).toggle(300, function () {
        //    $(this).remove();
        //});
    }

    methodHandlers.DeleteSaveCartResultCallBack = function (html, SaveCartCount) {
        var htmlError = '';
        if (html) {
            htmlError = html[0];
        }

        if (htmlError != '') {
            ShowError(htmlError);
        }

        if (document.getElementById('savecart-count')) {
            if (SaveCartCount == 0) {
                $('#secSaveCart').css('display', 'none');
            }
            else {
                var s = SaveCartCount + ' item';
                if (SaveCartCount > 1)
                    s += 's';
                $("#savecart-count").html(s);
            }
        }

        $('.remove').find('a').removeClass('active');
    }

    methodHandlers.AddFreeGiftCallBack = function (html, linkredirect, cartItemCount, isOK) {

        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var htmlError = '';
        var htmlCart = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlError = html[3];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                return;
            }
        }

        if (linkredirect != '') {
            window.location.href = linkredirect;
            return;
        }

        if (document.getElementById('secCartList')) {
            $('#secCartList').html(htmlCart);
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
            if (htmlSumaryBox.length <= 0) {
                $('#secEstimateShipping').replaceWith('');
                $('#ctl00_divEmpty').css('display', 'block');
            }
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }

        $('#secFreeGift #icon').removeClass('fopen');
        $('#secFreeGift #icon').addClass('fclose');

        $('#divListGift').css('display', 'none');
        $('#divListGift').html('');
    }

    methodHandlers.AddFreeSamplesCallBack = function (html, linkredirect, isOK, ItemId) {

        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var htmlError = '';
        var htmlCart = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlError = html[3];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                return;
            }
        }

        if (linkredirect != '') {
            window.location.href = linkredirect;
            return;
        }

        if (document.getElementById('secCartList')) {
            $('#secCartList').html(htmlCart);
        }

        if (document.getElementById('secOrderSummary')) {
            if (htmlSumaryBox.length > 0) {
                $('#secOrderSummary').replaceWith(htmlSumaryBox);
            }
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        var i = 0;
        if ($("#cart-count").length > 0 && !isNaN($("#cart-count").text())) {
            i = $("#cart-count").text();
            i = parseInt(i) + 1;
        }
        else if ($("#small-cart-count").length > 0 && !isNaN($("#small-cart-count").text()) ) {
            i = $("#small-cart-count").text();
            i = parseInt(i) + 1;
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(i);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(i);
        }

        if (isOK == 1) {
            $('#btnAddcart' + ItemId).replaceWith('<input type="button" id="btnAdded' + ItemId + '" value="Added" class="selected" /> <input type="button" id="btnAddcart' + ItemId + '" value="Delete" class="delete" onclick="DeleteFreeSamples(' + ItemId + ');" />');
            //$('#btnAddcart' + ItemId).prop('value', 'Added');
            //$('#btnAddcart' + ItemId).attr('onclick', 'DeleteFreeSamples(' + ItemId + ');');
        }
    }

    methodHandlers.DeleteFreeSamplesCallBack = function (html, isOK, CartItemId, ItemId) {

        var htmlError = '';
        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        if (html) {
            htmlError = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                return;
            }
        }

        if (document.getElementById('secCartList')) {
            $("#trRow_" + CartItemId).toggle(300, function () {
                $(this).remove();
            });
        }

        if (document.getElementById('secOrderSummary')) {
            if (htmlSumaryBox.length > 0) {
                $('#secOrderSummary').replaceWith(htmlSumaryBox);
            }
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        var i = 0;
        if ($("#cart-count").length > 0 && !isNaN($("#cart-count").text())) {
            i = $("#cart-count").text();
            i = parseInt(i) - 1;
        }
        else if ($("#small-cart-count").length > 0 && !isNaN($("#small-cart-count").text())) {
            i = $("#small-cart-count").text();
            i = parseInt(i) - 1;
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(i);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(i);
        }

        if (isOK == 1) {
            $('#btnAdded' + ItemId).remove();
            $('#btnAddcart' + ItemId).removeClass('delete');
            $('#btnAddcart' + ItemId).prop('value', 'Add');
            $('#btnAddcart' + ItemId).attr('onclick', 'return AddFreeSamples(' + ItemId + ');');
        }
    }

    methodHandlers.DeleteCartFreeGiftResultCallBack = function (html, isOK, linkredirect) {
        if (linkredirect != '') {
            window.location.href = linkredirect; // '/store/free-sample.aspx';   
            return;
        }

        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var linkRedirect = '';
        var htmlError = '';
        var htmlFreeGift = '';
        if (html) {
            htmlEstimatePopup = html[0];
            htmlSumaryBox = html[1];
            htmlFreeGift = html[2];
            htmlError = html[3];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                $('.remove').find('a').removeClass('active');
                return;
            }
            else {
                ShowError(htmlError);
            }
        }

        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
            if (htmlSumaryBox.length <= 0) {
                $('#secEstimateShipping').replaceWith('');
                $('#ctl00_divEmpty').css('display', 'block');
            }
        }

        if (document.getElementById('secFreeGift') && ((typeof htmlFreeGift != 'undefined') && (htmlFreeGift != ''))) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }

        var i = 0;
        if ($("#cart-count").length > 0 && !isNaN($("#cart-count").text())) {
            i = $("#cart-count").text();
            i = parseInt(i) - 1;
        }
        else if ($("#small-cart-count").length > 0 && !isNaN($("#small-cart-count").text())) {
            i = $("#small-cart-count").text();
            i = parseInt(i) - 1;
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(i);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(i);
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        if ($('#secFreeGift i').hasClass('fa-angle-double-up')) {
            $('#secFreeGift i').removeClass('fa-angle-double-up');
            $('#secFreeGift i').addClass('fa-angle-double-down');
            $('#divListGift').hide();
            $('#divListGift').html('');
        }
       
        $('.remove').find('a').removeClass('active');
    }

    methodHandlers.DeleteCartResultCallBack = function (html, cartItemCount, isOK, linkredirect) {
        if (linkredirect != '') {
            window.location.href = linkredirect; // '/store/free-sample.aspx';   
            return;
        }

        var htmlCart = '';
        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var htmlFreeGift = '';
        var htmlFreeSamples = '';
        var htmlRewardsPoints = '';
        var linkRedirect = '';
        var htmlError = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlFreeGift = html[3];
            htmlFreeSamples = html[4];
            htmlRewardsPoints = html[5];
            htmlError = html[6];
        }

        if ((typeof htmlError != 'undefined') && (htmlError != '')) {
            ShowError(htmlError);
            if (isOK == 0) {
                $('.remove').find('a').removeClass('active');
                return;
            }
        }
        if (document.getElementById('secFreeGift')) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }
        if (document.getElementById('secFreeSamples')) {
            $('#secFreeSamples').replaceWith(htmlFreeSamples);
        }
        if (document.getElementById('secRewardsPointsCart')) {
            $('#secRewardsPointsCart').replaceWith(htmlRewardsPoints);
        }

        if (document.getElementById('secCartList')) {
            if (htmlCart.length > 0) {
                $('#secCartList').replaceWith(htmlCart);

            }
        }

        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
            if (htmlSumaryBox.length <= 0) {
                $('#secEstimateShipping').replaceWith('');
                $('#ctl00_divEmpty').css('display', 'block');
            }
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');
                $('#secOrderSummary').addClass('summaryestimate');
                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }
        $('.remove').find('a').removeClass('active');
    }
    methodHandlers.UpdateCartResultCallBack = function (html, cartItemUpdateId, validqty, cartItemCount, cartIdError, isOK, linkredirect) {
        if (linkredirect != '') {
            window.location.href = linkredirect; // '/store/free-sample.aspx';  
            return;
        }
        var htmlCart = '';
        var htmlSumaryBox = '';
        var htmlEstimatePopup = '';
        var htmlFreeGift = '';
        var htmlFreeSamples = '';
        var htmlRewardsPoints = '';
        var htmlError = '';
        if (html) {
            htmlCart = html[0];
            htmlSumaryBox = html[1];
            htmlEstimatePopup = html[2];
            htmlFreeGift = html[3];
            htmlFreeSamples = html[4];
            htmlRewardsPoints = html[5];
            htmlError = html[6];
        }

        if (htmlError != '') {

            if (isOK == 0) {
                ShowCheckOutCartError(htmlError, cartIdError)
                validqty = parseInt(validqty);

                if (validqty > 0) {
                    if (document.getElementById('txtQty_' + cartItemUpdateId)) {
                        $('#txtQty_' + cartItemUpdateId).val(validqty);
                        $('.remove').find('a').removeClass('active');
                        $("#pnUpdate_" + cartItemUpdateId).show();
                    }
                }

                $('.remove').find('a').removeClass('active');
                return;
            }
            else {
                ShowError(htmlError);
            }
        }

        if (document.getElementById('secFreeGift')) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }
        if (document.getElementById('secFreeSamples')) {
            $('#secFreeSamples').replaceWith(htmlFreeSamples);
        }
        if (document.getElementById('secRewardsPointsCart')) {
            $('#secRewardsPointsCart').replaceWith(htmlRewardsPoints);
        }
        if (document.getElementById('secCartList')) {
            if (htmlCart.indexOf("<table") >= 0) {
                $('#secCartList').html(htmlCart);
            }
            else {
                var res = htmlCart.split("|")
                if (res.length > 0) {
                    $('#trRow_' + cartItemUpdateId + ' td.price').html(res[0]);
                    $('#trRow_' + cartItemUpdateId + ' td.total').html(res[1]);
                }
            }
        }
        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');

                poppover();
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }

        $('.remove').find('a').removeClass('active');
        if (isOK == 1) {
            $("#trRow_" + cartItemUpdateId).find("div.error").remove();

            var qtyupdated = $('#txtQty_' + cartItemUpdateId).val();
            //if (qtyupdated > 1) {
            //    $("#trRow_" + cartItemUpdateId).find("ul.cases li").html('Cases');
            //}
            //else {
            //    $("#trRow_" + cartItemUpdateId).find("ul.cases li").html('Case');
            //}
        }
    }

    methodHandlers.RefeshCartCallBack = function (html, cartItemCount, linkredirect, isOK) {
        if (linkredirect != '') {
            window.location.href = linkredirect; // '/store/free-sample.aspx';  
            return;
        }
        var htmlCart = '';
        var htmlEstimatePopup = '';
        var htmlSumaryBox = '';
        var htmlError = '';
        if (html) {
            htmlCart = html[0];
            htmlEstimatePopup = html[1];
            htmlSumaryBox = html[2];
            htmlError = html[3];
        }

        if (htmlError != '') {

            if (isOK == 0) {
                ShowCheckOutCartError(htmlError, cartIdError)
                return;
            }
            else {
                ShowError(htmlError);
            }
        }

        if (document.getElementById('secCartList')) {
            $('#secCartList').html(htmlCart);        
        }

        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }

        if (document.getElementById('divEstimateShippingResult')) {
            if ((typeof htmlEstimatePopup != 'undefined') && (htmlEstimatePopup != '')) {
                $('#divEstimateShippingResult').html(htmlEstimatePopup);
                $('#divEstimateShippingResult').css('display', 'block');
            }
            else {
                $('#divEstimateShippingResult').css('display', 'none');
            }
        }
    }

    function ChangeFreeItem(OrderId, MixMatchId, CartItemId) {
        var link = '/includes/popup/free-item.aspx?mmid=' + MixMatchId + '&orderId=' + OrderId + '&cartItemId=' + CartItemId;
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
            titleFromIframe: false,
            endRemove: function () {
                mainScreen.ExecuteCommand('RefeshCart', 'methodHandlers.RefeshCartCallBack', []);
            }
        });
            var isIphoneIpad = navigator.userAgent.match(/(iphone|ipod|ipad)/i) != null;
            if (isIphoneIpad == true) {
                $('#nyroModalContent').css({ 'overflow-y': 'scroll', '-webkit-overflow-scrolling': 'touch' });
            }
    }

    function SelectFreeGift(itemId) {
        mainScreen.ExecuteCommand('SelectFreeGift', 'methodHandlers.SelectFreeGiftCallBack', [itemId]);

    }
    methodHandlers.SelectFreeGiftCallBack = function (html) {

        var htmlCart = '';
        var htmlError = '';
        if (html) {
            htmlCart = html[0];
            htmlError = html[1];
        }

        if (htmlError != '') {
            ShowError(htmlError);
            return;
        }

        if (document.getElementById('secCartList')) {
            $('#secCartList').replaceWith(htmlCart);
        }

    }
    function RefreshDataUpdateFreeItemDiscountPercent(htmlFreeGift, htmlCart, htmlSumaryBox, cartItemCount) {

        if (document.getElementById('secFreeGift')) {
            $('#secFreeGift').replaceWith(htmlFreeGift);
        }
        if (document.getElementById('secCartList')) {
            $('#secCartList').replaceWith(htmlCart);
        }

        if (document.getElementById('secOrderSummary')) {
            $('#secOrderSummary').replaceWith(htmlSumaryBox);
        }

        if ($("#cart-count").length > 0) {
            $("#cart-count").html(cartItemCount);
        }

        if ($("#small-cart-count").length > 0) {
            $("#small-cart-count").html(cartItemCount);
        }
    }
    function openpopup(link, closebutton, widthpopup, heightpopup, backgroudcolor, indexstart, vmodal, windowresize, titlefrom) {
        //var link = '/includes/popup/review.aspx?ItemReviewId=' + ItemReviewId + '&ParentId=' + ParentId + '&Type=' + Type
        $('#nyroModal').attr('href', link);
        $('#nyroModal').nyroModalManual({
            showCloseButton: closebutton,
            bgColor: backgroudcolor,
            zIndexStart: indexstart,
            modal: vmodal,
            width: widthpopup,
            height: heightpopup,
            windowResize: windowresize,
            titleFromIframe: titlefrom
        });
    }

</script>
