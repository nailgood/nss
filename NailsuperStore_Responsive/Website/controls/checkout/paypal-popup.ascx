<%@ Control Language="VB" AutoEventWireup="false" CodeFile="paypal-popup.ascx.vb"
    Inherits="controls_checkout_paypal_popup" %>
<span class="msg">Your order will be shipped to the address you entered in PayPal. All
    Paypal orders will be charged at the time the order is placed.</span>
<div class="checkbox">
    <label for="chkPaypal" onclick="CheckPaypal();">
        <input type="checkbox" name="ctl00$cphContent$chkPaypal" id="chkPaypal">
        <i class="fa fa-check checkbox-font" ></i>I understand that I will be charged at the time
        I place my order.
    </label>
</div>
<span class="error-msg">You must agree to be charged at the time you place you order
    before<br />
    continuing to Paypal </span>
<div class="qtip-footer">
     <div class="cancelLink">
            <a href="javascript:void(0)" onclick="CancelPaypal();">Cancel</a>
        </div>
        <input type="button" class="btn btn-submit btn-cancel" id="btnCancelPaypal" onclick="CancelPaypal();"
            value="Cancel" name="btnCancelPaypal">
        <input type="button" class="btn btn-submit" id="btnContinuePaypal" name="btnContinuePaypal"
            onclick="ContinuePaypal();" value="Continue To Paypal">
      
    
</div>
