<%@ Page Language="VB" AutoEventWireup="false" CodeFile="payment.aspx.vb" MasterPageFile="~/includes/masterpage/checkout.master" Inherits="Store_Payment" %>

<%@ Register Src="../controls/checkout/shipping-option.ascx" TagName="shipping" TagPrefix="uc1" %>
<%@ Register Src="../controls/checkout/coupon-list.ascx" TagName="coupon" TagPrefix="uc2" %>
<%@ Register Src="../controls/checkout/shipping-freight-option.ascx" TagName="shipping"
    TagPrefix="uc3" %>
<%@ Register Src="../controls/checkout/shipping-list.ascx" TagName="shipping" TagPrefix="uc4" %>
<%@ Register Src="../controls/checkout/reward-point-summary.ascx" TagName="reward"
    TagPrefix="uc5" %>
<%@ Register Src="../controls/checkout/cart-summary.ascx" TagName="cart" TagPrefix="uc6" %>
<%@ Register Src="../controls/checkout/list-member-address.ascx" TagName="list" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <%--<script type="text/javascript" src="/includes/scripts/jquery.creditCardValidator.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <div id="payment">
        <section class="sec sec-address bill-address">
            <div class="header">
                <div class="text">
                    Billing Address
                </div>
                 <% If (Session("ShippingAddressRedirect") IsNot Nothing AndAlso Session("ShippingAddressRedirect")) Or Session("ShippingAddressRedirect") Is Nothing Then %>
               <%-- <div class="collapse collapse-close">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-down"></i>
                <div class="link">
                    <a href='javascript:void(0)' onclick="ShowListBillingAddress();">Change</a>
                </div>
            </div>
            <div class="content" style="display: none;">
                <% Else %>
                <%-- <div class="collapse collapse-open">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-down"></i>
                <div class="link">
                    <a href='javascript:void(0)' onclick="ShowListBillingAddress();">Change</a>
                </div>
                <script type="text/javascript">
                    $(window).ready(function () {
                        ShowListBillingAddress();
                    });
                </script>
            </div>
            
            <div class="content">
              
            <% End If %>
                <div class="pn-default" id="divBillingAddress" runat="server" clientidmode="Static">
                </div>
                <div class="pn-edit" id="divListBillingAddress" style="display: none;">
                    <uc7:list ID="ucListBillingAddress" runat="server" />
                </div>
                <div class="pn-email"><asp:Literal ID="litOrderEmail" runat="server"></asp:Literal></div>
            </div>
        </section>
        <section class="sec sec-address ship-address">
            <div class="header">
                <div class="text">
                    Shipping Address
                </div>
                <% If Session("ShippingAddressRedirect") IsNot Nothing AndAlso Session("ShippingAddressRedirect") Then %>
            <%--        <div class="collapse collapse-open">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-down"></i>
            </div>
            <div class="content">
            <% Else %>
                <%-- <div class="collapse collapse-close">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-down"></i>
                 </div>
            <div class="content" style="display: none;">
            <% End If %>
                 <% if Session("ShippingAddressRedirect") IsNot Nothing Then
                        Session.Remove("ShippingAddressRedirect")
                    End If
                %>
                <ul class="pn-select">
                    <li class="same">
                        <input type="radio" name="rdoTypeShippingAddress" onclick="ChangeOrderSameAddress('rdoSameAddress');"
                            id="rdoSameAddress" runat="server" clientidmode="Static" class="radio-node" />
                        <label for="rdoSameAddress" class="radio-label rdoTypeShippingAddress">
                        </label>
                        <span class="radio-text" onclick="ChangeOrderSameAddress('rdoSameAddress');">Same as
                            Billing Address</span> </li>
                    <li class="diff" id="liDiffAddress" runat="server" clientidmode="Static">
                        <input type="radio" name="rdoTypeShippingAddress" runat="server" clientidmode="Static"
                            id="rdoDiffAddress" class="radio-node" onclick="ChangeOrderSameAddress('rdoDiffAddress');" />
                        <label for="rdoDiffAddress" class="radio-label rdoTypeShippingAddress">
                        </label>
                        <span class="radio-text" onclick="ChangeOrderSameAddress('rdoDiffAddress');">Ship to
                            a different address</span> </li>
                </ul>
                <div class="pn-edit" id="divListShippingAddress" runat="server" clientidmode="Static">
                    <uc7:list ID="ucListShippingAddress" runat="server" />
                </div>
            </div>
        </section>
        <section class="sec shipping">
            <div class="header">
                <div class="text">
                    Shipping Method
                </div>
                <%--<div class="collapse collapse-open">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-up"></i>
            </div>
            <div class="content">
                <div id="divListShipping" runat="server" clientidmode="Static">
                    <uc4:shipping ID="ucShippingList" runat="server" />
                </div>
                <div class="ship-option" id="divShippingOption" clientidmode="Static" runat="server">
                    <uc1:shipping ID="ucShippingOption" runat="server" />
                </div>
                <div class="freight-option" id="dvFreightOption" visible="false" clientidmode="Static"
                    runat="server">
                    <%--<div>Must come to pick up your order at our Franklin Park warehouse within 2 days.</div>--%>
                    <uc3:shipping ID="ucFreightOption" runat="server" />
                </div>
            </div>
        </section>
        <section class="sec reward-point">
            <div class="header">
                <div class="text">
                    <asp:Literal ID="ltrSecPointTitle" Text="Redeem Coupon & Rewards Points" runat="server"></asp:Literal>
                </div>
              <%--  <div class="collapse collapse-close">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-up"></i>
            </div>
            <div class="content">
                <div class="form coupon">
                    <div class="form-row">
                        <div class="label">
                            Coupon:
                        </div>
                        <div class="control">
                            <asp:TextBox ID="txtCoupon" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                            <input type="button" onclick="AddCoupon();" value="Apply" class="btnGreen">
                        </div>
                    </div>
                    <div class="data" id="divCouponList">
                        <uc2:coupon ID="ucCouponList" runat="server" />
                    </div>
                </div>
                <div class="cashpoint" id="divCashPoint" runat="server" clientidmode="Static">
                    <h4>
                        Cash Points Rewards
                    </h4>
                    <div class="msg">
                        <asp:Literal ID="ltMsgPoint" runat="server"></asp:Literal></div>
                    <div class="msg-min">
                        <asp:Literal ID="ltrMsgPointMin" runat="server"></asp:Literal>
                    </div>
                    <ul id="ulPointReward" runat="server">
                        <li>
                            <ul class="control">
                                <li>
                                    <div class="nf-dropdown cashpoint-data">
                                        <asp:DropDownList ID="drpCashPoint" CssClass="form-control" ClientIDMode="Static"
                                            runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <input type="button" id="btnUpdatePoint" class="btnGreen" value="Update Amount" onclick="UpdatePoint();" />
                                </li>
                            </ul>
                            <ul class="link">
                                <li><a href="/members/pointbalance.aspx">My Reward Account </a></li>
                                <li class="sec">| </li>
                                <li><a href="/services/reward-point-program.aspx">Points Redemption Policy </a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
        <section class="sec place-order">
            <div class="header">
                <div class="text">
                    Payment
                </div>
                <%--<div class="collapse collapse-open">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-up"></i>
            </div>
            <div class="content">
                <ul class="payment-method">
                    <li class="credit-card radio-text">
                        <input type="radio" class="radio-node" id="rdoCreditCard" name="radGroupPaymentMethod"
                            checked="checked" />
                        <label class="radio-label radGroupPaymentMethod" for="rdoCreditCard">
                        </label>
                        <span class="text">Credit Card </span>
                          <span>
                            <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/amex.png" alt="American Express" />
                        </span>
                        <span>
                            <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/visa.png" alt="Visa" />
                        </span><span>
                            <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/mastercard.png" alt="master card" />
                        </span><span>
                            <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/discover.png" alt="discover" />
                        </span></li>
                    <li class="paypal radio-text">
                        <input type="radio" class="radio-node" id="rdoPaypal" onclick="ClickPaypal();" name="radGroupPaymentMethod" />
                        <label class="radio-label radGroupPaymentMethod" for="rdoPaypal">
                        </label>
                        <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/paypal.png" onclick="ClickPaypal();" alt="Paypal" />
                    </li>
                </ul>
                <div class="form">
                    <div class="form-row">
                        <div class="require-text">
                            <span class="required"></span>Required Fields
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="label">
                            <span class="required"></span>Name On Card:
                        </div>
                        <div class="control">
                            <input type="text" maxlength="50" class="form-control" id="txtCardName" value="" />
                        </div>
                    </div>
                   <%-- <div class="form-row">
                        <div class="label">
                            <span class="required"></span>Card Type:
                        </div>
                        <div class="control">
                            <div class="nf-dropdown card-type">
                                <asp:DropDownList ID="drlCardType" ClientIDMode="Static" AutoPostBack="false" CssClass="form-control"
                                    runat="server">
                                    <asp:ListItem Text="-- Select --" Value="" />
                                    <asp:ListItem Text="Visa" Value="V" />
                                    <asp:ListItem Text="MasterCard" Value="M" />
                                    <asp:ListItem Text="Discover" Value="D" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>--%>
                    <div class="form-row">
                        <div class="label">
                            <span class="required"></span>Card Number:
                        </div>
                        <div class="control">
                           <input type="tel" class="form-control pull-left" maxlength="16" id="txtCardNumber" value="" /> 
                        </div>
                        <div class="control log"></div>
                    </div>
                    <div class="form-row">
                        <div class="label">
                            <span class="required"></span>Security Code:
                        </div>
                        <div class="control">
                            <input type="tel" class="form-control" maxlength="4" id="txtCardSecurity" value="" />
                            <div class="help-security">
                                <a href="javascript:void(0);" onclick="ShowTipCardSercurity();">What is my Security Code?</a>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="label">
                            <span class="required"></span>Expiration:
                        </div>
                        <div class="control">
                            <div class="nf-dropdown expire-month">
                                <asp:DropDownList ID="drlCardExpireMonth" ClientIDMode="Static" AutoPostBack="false"
                                    CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="nf-dropdown expire-year">
                                <asp:DropDownList ID="drlCardExpireYear" ClientIDMode="Static" AutoPostBack="false"
                                    CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-row note">
                        Please note that by submitting this order, you are authorizing The Nail Superstore
                        to charge your credit or debit card for the amount indicated.
                    </div>
                    <div class="form-row comment">
                        <span>Comments or requests? Please enter it here:</span>
                        <asp:TextBox ID="txtOrderComment" ClientIDMode="Static" CssClass="txtComment" runat="server"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
        </section>
        <div class="submit">
            <div class="checkout-button">
                <ul class="content-checkout">
                    <li class="icon">
                        <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/secure.png">
                    </li>
                    <li class="text">Place Your Order </li>
                </ul>
            </div>
        </div>
        <section class="sec pay-list-item">
            <div class="header">
                <div class="text">
                    Review Your Order
                </div>
                <%--<div class="collapse collapse-close" id="collapseListCart">
                    &nbsp;
                </div>--%>
                <i class="collapse fa fa-angle-double-down" id="collapseListCart"></i>
            </div>
            <div class="content" style="display: none;" id="divListCartContent">
            </div>
        </section>
    </div>
    <asp:LinkButton ID="ltrPaypalCheckout" ClientIDMode="Static" runat="server"></asp:LinkButton>
    <input type="hidden" id="hidCardType" value="" />
    <script type="text/javascript">//<![CDATA[
        function BindDataShippingList(htmlShippingList) {
            if (htmlShippingList != '') {
                if (document.getElementById('divListShipping')) {
                    try{
                        $('#divListShipping').html(htmlShippingList);
                        }
                    catch(err){
                        alert(err);
                    };

                }
            }
        }
        function BindDataCartList(htmlCartList) {
            if (htmlCartList != '') {
                if (document.getElementById('divListCartContent')) {
                    $('#divListCartContent').html(htmlCartList);
                }
            }
        }
        function BindDataShippingOption(htmlShippingOption) {
            if (document.getElementById('divShippingOption')) {
                $('#divShippingOption').html(htmlShippingOption);
            }
        }
        function BindDataFreightShippingOption(htmlFreightShippingOption) {
            if (document.getElementById('dvFreightOption')) {
                $('#dvFreightOption').html(htmlFreightShippingOption);
            }
        }
        function BindDataCouponList(htmlListCoupon) {
            if (document.getElementById('divCouponList')) {
                $('#divCouponList').html(htmlListCoupon);
            }
        }
        $(document).ready(function ($) {
            $(".sec .header .collapse").click(function () {
                // var parent = $(panel).parent().get(0).tagName;
                CollapsePanel(this);
            });
            
            if (ViewPortVidth() <= 768) {
                $('#txtCardNumber').clone().attr('type', 'tel').attr('pattern', '[0-9]*').insertAfter('#txtCardNumber').prev().remove();
                $('#txtCardSecurity').clone().attr('type', 'tel').attr('pattern', '[0-9]*').insertAfter('#txtCardSecurity').prev().remove();
            }
        });

        $(window).load(function () {
            // alert(ViewPortVidth());

            ShowPageLoadError();

        });

        $(window).scroll(function () {

        });


        $(window).resize(function () {
            ChangeResize();
        });

        function CollapsePanel(panel) {

            var parent = $(panel).parent().get(0);
            var contentCollapse = $(parent).next('.content');
            $(contentCollapse).slideToggle("fast", function () {
                CollapsePanelComplate(panel, contentCollapse);
            });
        }
        function CollapsePanelComplate(panel, contentCollapse) {
            $(panel).removeClass();
            var display = $(contentCollapse).css('display');
            if (display == 'none') {
                //$(panel).addClass("collapse collapse-close");
                $(panel).addClass("collapse fa fa-angle-double-down");

            }
            else {
                //$(panel).addClass("collapse collapse-open");
                $(panel).addClass("collapse fa fa-angle-double-up");
                
                if (panel.id == 'collapseListCart') {
                    mainScreen.ExecuteCommand('GetListCartItem', 'methodHandlers.GetListCartItemCallBack', []);
                }
            }

        }
        methodHandlers.GetListCartItemCallBack = function (htmlReturn, linkRedirect) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }
            var htmlError = '';
            var htmlCartList = ''
            if (htmlReturn) {

                htmlError = htmlReturn[0];
                htmlCartList = htmlReturn[1];
            }
            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            if (htmlCartList != '') {
                BindDataCartList(htmlCartList);
            }


        }
        function ClickPaypal() {
            if (document.getElementById('tip-paypal')) {
                $('#tip-paypal').replaceWith('');
            }
            //  var html = 'Your order will be shipped to the address you entered in PayPal. All Paypal orders will be charged at the time the order is placed.'
            mainScreen.ExecuteCommand('SelectPaypalPayment', 'methodHandlers.SelectPaypalPaymentCallBack', []);
            // showQtip('qtip-msg', html, 'PayPal Checkout');
        }
        methodHandlers.SelectPaypalPaymentCallBack = function (htmlReturn, linkRedirect) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }
            var htmlError = '';
            var paypalPopup = ''
            if (htmlReturn) {

                htmlError = htmlReturn[0];
                paypalPopup = htmlReturn[1];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                CancelPaypal();
                return;

            }
            if (paypalPopup != '')
                showQtipID('qtip-paypal', paypalPopup, 'PayPal Checkout', 'tip-paypal');

        }
        function CheckPaypal() {

            if (document.getElementById('chkPaypal').checked == true) {
                document.getElementById('chkPaypal').checked = false;
                // $('#btnContinuePaypal').prop('disabled', true);
            }
            else {
                document.getElementById('chkPaypal').checked = true;
                //$('#btnContinuePaypal').prop('disabled', false);
            }
        }

        function CancelPaypal() {

            $('.qtip-paypal .qtip-button').click();
            $("#rdoCreditCard").prop("checked", true);

        }

        function ContinuePaypal() {
            if (document.getElementById('chkPaypal').checked == false) {
                $('.qtip-paypal .error-msg').css('display', 'inline');
                return;
            }
            if (document.getElementById('ltrPaypalCheckout')) {

                __doPostBack('ctl00$cphContent$ltrPaypalCheckout', '')

            }

        }

        methodHandlers.PayPalCheckOutCallBack = function (html, linkRedirect) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }
            var htmlError = '';
            if (html) {

                htmlError = html[0];
            }

            if (htmlError != '') {
                ShowError(htmlError);

            }
        }
        function IsRenderListCart() {
            var result = 0;
            var displayListCart = $('#divListCartContent').css('display');
            if (displayListCart == 'block') {
                result = 1;
            }
            return result;
        }


        function ChangeShippingMethod(methodID) {

            var RenderListCart = IsRenderListCart();
            $("#rdoShipping_" + methodID).prop("checked", true);
            mainScreen.ExecuteCommand('ChangeShippingMethod', 'methodHandlers.ChangeShippingMethodCallBack', [methodID, RenderListCart]);
        }
        methodHandlers.ChangeShippingMethodCallBack = function (html, linkRedirect, isPickup) {
           <%-- if (linkRedirect != '' || linkRedirect != undefined) {
                window.location.href = linkRedirect;
            }--%>

            var htmlShippingOption = '';
            var htmlFreightShippingOption = '';
            var htmlSumaryBox = '';
            var htmlCartList = '';
            var htmlError = '';
            if (html) {

                htmlError = html[0];
                htmlShippingOption = html[1];
                htmlFreightShippingOption = html[2];
                htmlSumaryBox = html[3];
                htmlCartList = html[4];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                BindDataCartSummaryBox(htmlSumaryBox)
                return;
            }
            BindDataShippingOption(htmlShippingOption);
            BindDataFreightShippingOption(htmlFreightShippingOption)
            BindDataCartSummaryBox(htmlSumaryBox)
            BindDataCartList(htmlCartList)
            if (isPickup == 1) {
                $('#msgPickup').css('display', 'block');
                $('#rdoRadShipVia').css('width', '100%');

            }
            else {
                $('#msgPickup').css('display', 'none');
                $('#rdoRadShipVia').css('width', 'auto');
            }
            
        }
        function CheckShippingInsurance(value) {
            mainScreen.ExecuteCommand('CheckShippingInsurance', 'methodHandlers.CheckShippingInsuranceCallBack', [value]);
        }
        methodHandlers.CheckShippingInsuranceCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlError = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            BindDataCartSummaryBox(htmlSumaryBox);


        }
        function CheckShippingSignature(value) {
          //  var check = $('#chkSignature').is(':checked');
            mainScreen.ExecuteCommand('CheckShippingSignature', 'methodHandlers.CheckShippingSignatureCallBack', [value]);
        }
        methodHandlers.CheckShippingSignatureCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlError = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            BindDataCartSummaryBox(htmlSumaryBox);


        }
        function AddCoupon() {
            var code = $('#txtCoupon').val();
            if (code == '') {
                ShowError('Please input coupon code.');
                return;
            }

            mainScreen.ExecuteCommand('AddCoupon', 'methodHandlers.AddCouponCallBack', [code, IsRenderListCart()]);
        }
        methodHandlers.AddCouponCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlListCoupon = '';
            var htmlError = '';
            var htmlCartList = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlListCoupon = html[2];
                htmlCartList = html[3];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            BindDataCartSummaryBox(htmlSumaryBox);
            BindDataCouponList(htmlListCoupon);
            BindDataCartList(htmlCartList);
            $('#txtCoupon').val('');
        }
        function DeleteCoupon(code) {
            mainScreen.ExecuteCommand('DeleteCoupon', 'methodHandlers.DeleteCouponCallBack', [code, IsRenderListCart()]);
        }
        methodHandlers.DeleteCouponCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlListCoupon = '';
            var htmlError = '';
            var htmlCartList = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlListCoupon = html[2];
                htmlCartList = html[3];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            BindDataCartSummaryBox(htmlSumaryBox);
            BindDataCouponList(htmlListCoupon);
            BindDataCartList(htmlCartList);
        }
        function ShowTipSignatureConfirm() {
            showQtip('qtip-signature-confirm', '<div class="contentsign">In order to have your shipment delivered safely, signature confirmation is required for shipment to residential delivery. After the order is shipped, you can contact the carrier with your tracking number to arrange for getting your package if you are unable to be home to sign for them.<br>To learn more about signature required options: <a target="_blank" href="http://ask.van.fedex.com/learn/signature-required">http://ask.van.fedex.com/learn/signature-required</a>.</div>', 'Signature Confirmation');
        }
        function ShowTipEmailOrder() {
            showQtip('qtip-signature-confirm', '<div class="contentsign">In order to receive your order confirmation receipt and shipping tracking number, a valid email address is required.</div>', 'Email Order Confirmation');
        }
        function ShowTipCardSercurity() {
            showQtip('qtip-card-security', '<div class="contentsign"><img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/HelpCard.gif" /></div>', 'Visa, MasterCard & Discover Cards');
        }
        function ShowListBillingAddress() {
            $('#divListBillingAddress').css('display', 'block');
            $('#divBillingAddress').css('display', 'none');
            $('.pn-email').css('display', 'none');

            var panelDisplay = $('.bill-address .content').css('display');

            if (panelDisplay == 'none') {
                $('.bill-address .collapse').click();
            }
        }

        function ChangeOrderSameAddress(ctrId) {
            var same = false;
            if (ctrId == 'rdoDiffAddress') {
                same = false
            }
            else {
                same = true
            }
            $("#" + ctrId).prop("checked", true);
            mainScreen.ExecuteCommand('ChangeOrderSameAddress', 'methodHandlers.ChangeOrderSameAddressCallBack', [same, IsRenderListCart(),GetListCouponInValid()]);

        }
        methodHandlers.ChangeOrderSameAddressCallBack = function (html, linkRedirect, isSameAddress) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlShippingList = '';
            var htmlShippingOption = '';
            var htmlFreightShippingOption = '';
            var htmlCartList = '';
            var htmlError = '';
            var htmlListShippingAddress = '';
            var htmlListCoupon = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlShippingList = html[2];
                htmlShippingOption = html[3];
                htmlFreightShippingOption = html[4];
                htmlCartList = html[5];
                htmlListShippingAddress = html[6];
                htmlListCoupon = html[7];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }

            if (isSameAddress == 'true') {

                if (document.getElementById('divListShippingAddress')) {
                    $('#divListShippingAddress').css('display', 'none');
                }
                if (htmlListCoupon != '') {
                    BindDataCouponList(htmlListCoupon);
                }

                BindDataCartSummaryBox(htmlSumaryBox);
                BindDataCartList(htmlCartList);
                BindDataShippingOption(htmlShippingOption);
                BindDataFreightShippingOption(htmlFreightShippingOption);
                BindDataShippingList(htmlShippingList);
            }
            else {

                if (document.getElementById('divListShippingAddress')) {
                    $('#divListShippingAddress').html(htmlListShippingAddress);
                    $('#divListShippingAddress').css('display', '');
                }
                $('#divListShippingAddress .data .select input[type=radio]').each(function () {
                    var ctr = $(this);
                    if (document.getElementById(ctr[0].id)) {
                        var checked = document.getElementById(ctr[0].id).checked;
                        if (checked == true) {
                            document.getElementById(ctr[0].id).checked = false;

                        }
                    }
                });
            }
        }
        function UpdatePoint() {
            var point = $('#drpCashPoint').val();
            var pointMsg = $("select[id=drpCashPoint] option:selected").text()

            mainScreen.ExecuteCommand('PaymentPointForOrder', 'methodHandlers.PaymentPointForOrderCallBack', [point, pointMsg]);
        }
        methodHandlers.PaymentPointForOrderCallBack = function (html, linkRedirect, redeenablePoint) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlRewardsPoints = '';
            var htmlSumaryBox = '';
            var htmlError = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlRewardsPoints = html[2]
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }

            if (document.getElementById('secPointSummary')) {
                $('#secPointSummary').replaceWith(htmlRewardsPoints);
            }

            BindDataCartSummaryBox(htmlSumaryBox);
            $('#divCashPoint .msg').html('You currently have ' + redeenablePoint + ' redeemable reward points')
        }
        function RequestOversizeFee(check, type) {

            mainScreen.ExecuteCommand('RequestOversizeFee', 'methodHandlers.RequestOversizeFeeCallBack', [check, type]);
        }
        methodHandlers.RequestOversizeFeeCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlError = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            BindDataCartSummaryBox(htmlSumaryBox);

        }
        
        var rdoBillingAddressChecked = '';
        function ChangeOrderBillingAddress(addressId) {
            $("#rdoBillingAddress_" + addressId).prop("checked", true);
            rdoBillingAddressChecked = "rowBillingAddress_" + addressId;
            mainScreen.ExecuteCommand('ChangeOrderBillingAddress', 'methodHandlers.ChangeOrderBillingAddressCallBack', [addressId, IsRenderListCart(), GetListCouponInValid()]);

        }
        function GetListCouponInValid() {
            var result = '';
            if (document.getElementById('hidCouponIsNotValid')) {
                result = $('#hidCouponIsNotValid').val();
            }
            return result;
        }
        methodHandlers.ChangeOrderBillingAddressCallBack = function (html, linkRedirect, isSame, billingAddressId, shippingAddressId, isUSAddress) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }
            var htmlListCoupon = '';
            var htmlSumaryBox = '';
            var htmlShippingList = '';
            var htmlShippingOption = '';
            var htmlFreightShippingOption = '';
            var htmlCartList = '';
            var htmlError = '';
            var htmlListShippingAddress = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlShippingList = html[2];
                htmlShippingOption = html[3];
                htmlFreightShippingOption = html[4];
                htmlCartList = html[5];
                htmlListShippingAddress = html[6];
                htmlListCoupon = html[7];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                //return;
            }
            DisableDeleteAddress(billingAddressId, shippingAddressId);
            if (isSame == 'true') {
                if(htmlListCoupon!='')
                {
                BindDataCouponList(htmlListCoupon);}
                BindDataCartSummaryBox(htmlSumaryBox);
                BindDataCartList(htmlCartList);
                BindDataShippingList(htmlShippingList);
                BindDataShippingOption(htmlShippingOption);
                BindDataFreightShippingOption(htmlFreightShippingOption);
                if (document.getElementById('divListShippingAddress')) {
                    $('#divListShippingAddress').css('display', 'none');
                }
                $("#rdoSameAddress").prop("checked", true);

            }
            else {
                if (document.getElementById('divListShippingAddress')) {
                    $('#divListShippingAddress').html(htmlListShippingAddress);
                    $('#divListShippingAddress').css('display', '');
                }
            }
            if (isUSAddress == 'true') {
                $('#liDiffAddress').css('display', 'none');
                $('#divListShippingAddress').css('display', 'none');
                //rdoSameAddress
                $("#rdoSameAddress").prop("checked", true);


            }
            else {
                $('#liDiffAddress').css('display', '');
            }

            fillAddress();
        }
        function DeleteAddress(addressId) {
            var yes = confirm('Are you sure delete this address ?')
            if (yes) {
                mainScreen.ExecuteCommand('DeleteAddress', 'methodHandlers.DeleteAddressCallBack', [addressId]);
            }
        }
        methodHandlers.DeleteAddressCallBack = function (html, linkRedirect, addressId) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }
            var htmlError = '';
            if (html) {

                htmlError = html[0];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            if (document.getElementById('rowShippingAddress_' + addressId)) {
                $('#rowShippingAddress_' + addressId).replaceWith('');
            }
            if (document.getElementById('rowBillingAddress_' + addressId)) {
                $('#rowBillingAddress_' + addressId).replaceWith('');
            }
        }
        function DisableDeleteAddress(billAddressId, shipAddressId) {

            var lstAddressId = $('#hidListAddressBookId').val();
            if (lstAddressId != '') {
                var arr = new Array();
                arr = lstAddressId.split(',');
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var id = arr[i].toString();
                        if (id != '') {
                            //refresh pannel billing
                            if (id == billAddressId || id == shipAddressId) {
                                if (document.getElementById('sepAddressBilling_' + id)) {
                                    $('#sepAddressBilling_' + id).css('display', 'none');
                                }
                                if (document.getElementById('lnkRemoveAddressBilling_' + id)) {
                                    $('#lnkRemoveAddressBilling_' + id).css('display', 'none');
                                }
                            }
                            else {
                                if (document.getElementById('sepAddressBilling_' + id)) {
                                    $('#sepAddressBilling_' + id).css('display', '');
                                }
                                if (document.getElementById('lnkRemoveAddressBilling_' + id)) {
                                    $('#lnkRemoveAddressBilling_' + id).css('display', '');
                                }
                            }
                            //refresh pannel shipping
                            if (id == billAddressId || id == shipAddressId) {
                                if (document.getElementById('sepAddressShipping_' + id)) {
                                    $('#sepAddressShipping_' + id).css('display', 'none');
                                }
                                if (document.getElementById('lnkRemoveAddressShipping_' + id)) {
                                    $('#lnkRemoveAddressShipping_' + id).css('display', 'none');
                                }
                            }
                            else {
                                if (document.getElementById('sepAddressShipping_' + id)) {
                                    $('#sepAddressShipping_' + id).css('display', '');
                                }
                                if (document.getElementById('lnkRemoveAddressShipping_' + id)) {
                                    $('#lnkRemoveAddressShipping_' + id).css('display', '');
                                }
                            }
                        }
                    }
                }
            }


        }

        function ChangeOrderShippingAddress(addressId) {
            $("#rdoShippingAddress_" + addressId).prop("checked", true);
            rdoBillingAddressChecked = "rowShippingAddress_" + addressId;

            mainScreen.ExecuteCommand('ChangeOrderShippingAddress', 'methodHandlers.ChangeOrderShippingAddressCallBack', [addressId, IsRenderListCart(),GetListCouponInValid()]);

        }
        methodHandlers.ChangeOrderShippingAddressCallBack = function (html, linkRedirect, billingAddressId, shippingAddressId, isSameAddress) {

            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlSumaryBox = '';
            var htmlShippingList = '';
            var htmlShippingOption = '';
            var htmlFreightShippingOption = '';
            var htmlCartList = '';
            var htmlError = '';
            var htmlListCoupon = '';
            if (html) {

                htmlError = html[0];
                htmlSumaryBox = html[1];
                htmlShippingList = html[2];
                htmlShippingOption = html[3];
                htmlFreightShippingOption = html[4];
                htmlCartList = html[5];
                htmlListCoupon = html[6];
            }

            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            if (htmlListCoupon != '') {
                BindDataCouponList(htmlListCoupon);
            }
            BindDataCartSummaryBox(htmlSumaryBox);
            BindDataCartList(htmlCartList);
            BindDataShippingOption(htmlShippingOption);
            BindDataFreightShippingOption(htmlFreightShippingOption);
            DisableDeleteAddress(billingAddressId, shippingAddressId);
            BindDataShippingList(htmlShippingList);
            if (isSameAddress == 'true') {
                if (document.getElementById('divListShippingAddress')) {
                    $('#divListShippingAddress').css('display', 'none');
                }
                $("#rdoSameAddress").prop("checked", true);
            }
           
        }
        $("#.submit .checkout-button").click(function () {
            var isSelectShipping = false;
            var isDiffAddress = document.getElementById('rdoDiffAddress').checked;
            if (isDiffAddress) {
                $('#divListShippingAddress .data .select input[type=radio]').each(function () {
                    var ctr = $(this);
                    if (document.getElementById(ctr[0].id)) {
                        var checked = document.getElementById(ctr[0].id).checked;
                        if (checked == true) {
                            isSelectShipping = true;

                        }
                    }
                });
            }
            var errorMsg = '';
            if (!isSelectShipping && isDiffAddress) {
                errorMsg = MergeErrorMessage(errorMsg, 'Please select a shipping address.')
            }
            var txtCardName = $('#txtCardName').val();
            if (txtCardName == '') {
                errorMsg = MergeErrorMessage(errorMsg, 'Name on card is required.')
            }
            //            var drlCardType = $('#drlCardType').val();
            //            if (drlCardType == '') {
            //                errorMsg = MergeErrorMessage(errorMsg, 'Credit card type is required.')
            //            }
            var drlCardType = $("#hidCardType").val();
            if (drlCardType == '') {
                errorMsg = MergeErrorMessage(errorMsg, 'We can only accept Visa, MasterCard, Discover or PayPal payment.');
                //errorMsg = MergeErrorMessage(errorMsg, 'Credit card type is required.');
            }

            var txtCardNumber = $('#txtCardNumber').val();
            if (txtCardNumber == '') {
                errorMsg = MergeErrorMessage(errorMsg, 'Please enter a valid credit card number.')
            }
            var drlCardExpireMonth = $('#drlCardExpireMonth').val();
            var drlCardExpireYear = $('#drlCardExpireYear').val();
            if (drlCardExpireMonth == '' || drlCardExpireYear == '') {
                errorMsg = MergeErrorMessage(errorMsg, 'Please enter a valid expiration date.')

            }
            var txtCardSecurity = $('#txtCardSecurity').val();
            if (txtCardSecurity == '') {
                errorMsg = MergeErrorMessage(errorMsg, 'CID Code is required.')

            }
            if (errorMsg != '') {
                ShowError(errorMsg);
                return;
            }
            var note = $('#txtOrderComment').val();
            mainScreen.ExecuteCommand('PlaceOrder', 'methodHandlers.PlaceOrderCallBack', [txtCardName, drlCardType, txtCardNumber, drlCardExpireMonth, drlCardExpireYear, txtCardSecurity, note]);
        });
       
        methodHandlers.PlaceOrderCallBack = function (html, linkRedirect) {
            if (linkRedirect != '') {
                window.location.href = linkRedirect;
            }

            var htmlError = '';
            if (html) {
                htmlError = html[0];
            }
            
            if (htmlError != '') {
                ShowError(htmlError);
            }
        }

        $(function () {
            $('#txtCardNumber').validateCreditCard(function (result) {
                var imgcard = "<img src='<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/{0}.png' />",
                    str = '';
                try {
                    str = result.card_type.name.toString();
                    if (str == "mastercard")
                        $("#hidCardType").val("2");
                    else if (str == "visa")
                        $("#hidCardType").val("1");
                    else if (str == "discover")
                        $("#hidCardType").val("4");
                    else if (str == "amex")
                        $("#hidCardType").val("3");
                    else {
                        imgcard = '';
                        $("#hidCardType").val("");
                    }
                }
                //alert(str);
                catch (e) { $("#hidCardType").val(""); }

                if (imgcard != '')
                    imgcard = String.format(imgcard, str);

                $('.log').html((result.card_type == null ? '' : imgcard));
            });
        });

    </script>
    <asp:Literal ID="ltrLoadMsg" runat="server"></asp:Literal>
</asp:Content>
