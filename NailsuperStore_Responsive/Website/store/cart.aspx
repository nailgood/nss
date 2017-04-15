<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/checkout.master" AutoEventWireup="false" CodeFile="cart.aspx.vb" Inherits="store_cart" %>

<%@ Register Src="~/controls/checkout/free-gift-cart.ascx" TagName="freegift" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/rewards-points-cart.ascx" TagName="rewardspoints" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/free-samples-cart.ascx" TagName="freesamples" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/cart.ascx" TagName="cart" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/cart-saveforlater.ascx" TagName="savecart" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/cartScript.ascx" TagName="cartScript" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/cart-summary.ascx" TagName="summary" TagPrefix="uc" %>
<%@ Register Src="~/controls/checkout/estimate-shipping.ascx" TagName="estimate" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
 <script type="text/javascript">
     //maxmind
     maxmind_user_id = "104402";
     (function () {
         var loadDeviceJs = function () {
             var element = document.createElement('script');
             element.src = ('https:' == document.location.protocol ? 'https:' : 'http:')
               + '//device.maxmind.com/js/device.js';
             document.body.appendChild(element);
         };
         if (window.addEventListener) {
             window.addEventListener('load', loadDeviceJs, false);
         } else if (window.attachEvent) {
             window.attachEvent('onload', loadDeviceJs);
         }
     })();
     //end


     var isLoad = false;
     var msgError = '';
     function LoadError(msg) {
         msgError = msg;
         setInterval('ShowLoadError()', 1000);
     }
     function ShowLoadError() {
         if (isLoad) {
             isLoad = false;
             ShowError(msgError);
         }
     }
     $(document).ready(function () {
         isLoad = true;
         CheckEmptyCart();
//         if (ViewPortVidth() <= 768) {
//             $('#txtZipCode').clone().attr('type', 'tel').attr('pattern', '[0-9]*').insertAfter('#txtZipCode').prev().remove();
//         }
     });

     function AddFreeGift(itemId) {
         var btn = $('#btnAddcart' + itemId);
         var className = $('#btnAddcart' + itemId).attr('class');
         if (className == 'in-cart') {
             return false;
         }
         mainScreen.ExecuteCommand('AddFreeGift', 'methodHandlers.AddFreeGiftCallBack', [itemId]);
         $('#secFreeGift i').removeClass('fa-angle-double-up');
         $('#secFreeGift i').addClass('fa-angle-double-down');
         return false;
     }

     function AddFreeSamples(itemId) {
         var btn = $('#btnAddcart' + itemId);
         var className = $('#btnAddcart' + itemId).attr('class');
         if (className == 'in-cart') {
             return false;
         }
         mainScreen.ExecuteCommand('AddFreeSamples', 'methodHandlers.AddFreeSamplesCallBack', [itemId]);
         return false;
     }

     function DeleteFreeSamples(itemId) {
         mainScreen.ExecuteCommand('DeleteFreeSamples', 'methodHandlers.DeleteFreeSamplesCallBack', [itemId]);
     }

     function OpenFreeGift() {
         $('.qtip-title .qtip-button').click();
         //if ($('#secFreeGift #icon').hasClass('fa-angle-double-down')) {
         if ($('#secFreeGift i').hasClass('fa-angle-double-down')) {
             $('#secFreeGift i').removeClass('fa-angle-double-down');
             $('#secFreeGift i').addClass('fa-angle-double-up');
             var id = $('#ctl00_cphContent_ucFreeGift_hidFreeGiftLevelId').val();
             if (typeof (id) == 'undefined') {
                 id = $('#ctl00_hidFreeGiftLevelId').val();
             }
             mainScreen.ExecuteCommand('OpenFreeGift', 'methodHandlers.OpenFreeGiftCallBack', [id]);
         }
         else {
             //$('#secFreeGift #icon').removeClass('fopen');
             //$('#secFreeGift #icon').addClass('fclose');
             $('#secFreeGift i').removeClass('fa-angle-double-up');
             $('#secFreeGift i').addClass('fa-angle-double-down');
             $('#divListGift').css('display', 'none');
             $('#divListGift').html('');
         }
     }

     function OpenFreeSamples() {
         //if ($('#secFreeSamples #icon').hasClass('fa-angle-double-down')) {
         if ($('#secFreeSamples i').hasClass('fa-angle-double-down')) {
             $('#secFreeSamples i').removeClass('fa-angle-double-down');
             $('#secFreeSamples i').addClass('fa-angle-double-up');
             mainScreen.ExecuteCommand('OpenFreeSamples', 'methodHandlers.OpenFreeSamplesCallBack', []);
         }
         else {
             //$('#secFreeSamples #icon').removeClass('fopen');
             //$('#secFreeSamples #icon').addClass('fclose');
             $('#secFreeSamples i').removeClass('fa-angle-double-up');
             $('#secFreeSamples i').addClass('fa-angle-double-down');

             $('#divListSamples').css('display', 'none');
             $('#divListSamples').html('');
         }
     }
    function showPointBalancePopup(evt) {
                $.get('/members/pointbalance.aspx?isPopUp=true', {}, function (result) {
                    var resultString = $(result).find('div#popup').html();
                    showQtip('qtip-msg', resultString, 'Your Cash Reward Points Balance');
                }
                , 'text');
            }
    </script>

    <uc:rewardspoints ID="ucRewardsPoints" runat="server" />
    <uc:freesamples ID="ucFreeSamples" runat="server" />
    <uc:freegift ID="ucFreeGift" runat="server" />

    <!--cart item-->
    <div id="divListCart">
        <uc:cart ID="uCart" runat="server" />
    </div>
    
    <uc:cartScript ID="cartScript" runat="server" />
    <asp:Literal ID="ltrScript" runat="server" Text=""></asp:Literal>
    <div id="divSendCart" runat="server" class="linksendcart">
        Not ready to buy yet? <a href="#" onclick="openpopup('/includes/popup/sendmailcart.aspx',false, 580, 240, 'gray', '9999999', false, true, false)">Email Shopping Cart</a> and come back later to complete your order.
    </div>
    <a id="aTest"></a>
    <uc:savecart ID="ucSaveCart" runat="server" />
</asp:Content>
