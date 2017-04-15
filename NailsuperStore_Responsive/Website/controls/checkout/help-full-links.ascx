<%@ Control Language="VB" AutoEventWireup="false" CodeFile="help-full-links.ascx.vb" Inherits="controls_checkout_help_full_links" %>
<%@ Register Src="~/controls/checkout/secure-icon.ascx" TagName="secure" TagPrefix="uc" %>

<section class="help-link largelink">
    <ul>
        <li class="title">Helpful Links</li>
        <li class="row-link"><a href="/services/order-shipping-policies.aspx">Shipping Infomation</a>
        </li>
        <li class="row-link"><a href="/services/returns-policies.aspx">Return Policies</a>
        </li>
        <li class="row-link"><a href="/services/order-payment.aspx">Payment Information</a>
        </li>
        <li class="row-link"><a href="/services/order-sales-tax.aspx">Sales Tax</a> </li>
    </ul>
</section>
<uc:secure ID="secure" runat="server" />
