<%@ Control Language="VB" AutoEventWireup="false" CodeFile="payment-infor.ascx.vb"
    Inherits="controls_checkout_payment_infor" %>
<div class="payment-infor">
    <ul id="ulCreditCard" runat="server">
        <li id="liMethod" runat="server">Payment Method:<span id="lblMethod" runat="server"></span>
        </li>
        <li id="liNamOnCard" visible="false" runat="server">Name on Card:<span id="lblNameOnCard"
            runat="server"></span> </li>
        <li id="liCardType" visible="false" runat="server">Card Type:<span id="lblCardType"
            runat="server"></span> </li>
        <li id="liCardNumber" visible="false" runat="server">Card Number:<span id="lblCardNumber"
            runat="server"></span> </li>
        <li id="liExpireDate" visible="false" runat="server">Expiration Date:<span id="lblExpireDate" runat="server"></span>
        </li>
    </ul>
</div>
