<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/interior.master" Inherits="Components.SitePage" %>

<%--<%@ Register TagPrefix="CT" Namespace="MasterPages" Assembly="Common" %>
<%@ Register Src="../modules/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc2" %>
<%@ Register Src="../modules/Menu.ascx" TagName="Menu" TagPrefix="uc3" %>
<%@ Register Src="../modules/CustomerServiceMenu.ascx" TagName="Menu" TagPrefix="uc4" %>
<%@ Register Src="../modules/NeedAssistance.ascx" TagName="Menu" TagPrefix="uc5" %>
<%@ Register Src="../modules/EmailSignup.ascx" TagName="Menu" TagPrefix="uc6" %>
<%@ Register Src="~/controls/layout/bread-crumb.ascx" TagName="Menu" TagPrefix="uc7" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <title id="PageTitle" enableviewstate="False" runat="server"></title>
    
    <center>
        <div id="header-page">
           <%-- <uc2:SearchBar ID="SearchBar1" runat="server" />
            <uc3:Menu ID="Menu1" runat="server" />--%>
        </div>
        <div id="page">
            <div id="left-page">
                <%--<uc4:Menu ID="CustomerServiceMenu1" runat="server" />
                <uc5:Menu ID="NeedAssistance1" runat="server" />
                <uc6:Menu ID="EmailSignup1" runat="server" />--%>
            </div>
            <div id="content-page">
               <%-- <uc7:Menu ID="BreadCrumb1" runat="server" />--%>
                <div id="dContent">
                    <div class="dsubContent">
                        <p>
                            <a href="javascript:void(0);/*1215549546714*/">
                                <img src="../../App_Themes/Default/images/Customer Service.jpg" alt="" width="746"
                                    height="175" /></a></p>
                        <p class="sTitle">
                            Ordering &amp; Shipping<br />
                            <br />
                            <br />
                        </p>
                        <table border="0" cellspacing="1" cellpadding="10" width="100%">
                            <tbody>
                                <tr width="100%">
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-catalog-quick-order.aspx">Catalog Quick Order</a><br />
                                        </strong>We call this Catalog Quick Order. Using this feature, you can order the
                                        products you want by using the item number shown in... <a href="../../services/order-catalog-quick-order.aspx">
                                            Read more</a>.
                                    </td>
                                    <td width="50%">
                                        <strong><a href="../../services/order-changing-cancelling.aspx">Changing &amp; Cancelling</a><br />
                                        </strong>If you need to change or cancel an order, please contact us by phone as
                                        soon as possible. You can call us toll-free... <a href="../../services/order-changing-cancelling.aspx">
                                            Read more</a>.
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" height="50%" valign="top">
                                        <a href="../../services/order-damaged-shipment.aspx"><strong>Damaged Shipment</strong></a><br />
                                        We carefully pack and check all boxes before they are sent. However damages can
                                        occur during transit... <a href="../../services/order-damaged-shipment.aspx">Read more</a>.<span
                                            style="text-decoration: none;"><br />
                                        </span>
                                    </td>
                                    <td valign="top">
                                        <strong><a href="../../services/order-delivery-time.aspx">Delivery Time</a><br />
                                        </strong>Orders received Monday through Thursday by 1:00 p.m. CST are usually processed
                                        the same day...<em> </em><a href="../../services/order-delivery-time.aspx">Read more.</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <strong><a href="../../services/order-international-shipment.aspx">International Shipment</a><br />
                                        </strong>Shipping time and rate will depend of the products, country of delivery
                                        and freight carrier. You can request our Customer Service...<em> </em><a href="../../services/order-international-shipment.aspx">
                                            Read more</a>.
                                    </td>
                                    <td width="50%" height="25%" valign="top">
                                        <a href="../../services/order-status.aspx"><strong>Order Status</strong></a><br />
                                        Enter your order number and email address to view order status. When can I track
                                        my order? Packages can be tracked...<em> </em><a href="../../services/order-status.aspx">
                                            Read more</a>.
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" height="25%" valign="top">
                                        <strong><a href="../../services/order-payment.aspx">Payment</a></strong>
                                        <br />
                                        All our prices are in US dollar.<br />
                                        We accept payments by Visa, MasterCard or Discover cards...<em> </em><a href="../../services/order-payment.aspx">
                                            Read more</a>.
                                    </td>
                                    <td width="50%" height="92" valign="top">
                                        <strong><a href="../../services/order-price-match.aspx">Price Match</a><br />
                                        </strong>When you shop at NailSuperstore.com you&rsquo;re always getting the Best
                                        Price and we&rsquo;re willing to back that up with our 110% Price Match... <a href="../../services/order-price-match.aspx">
                                            Read more</a>.
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" height="92" valign="top">
                                        <strong><a href="../../services/order-sales-tax.aspx">Sales Tax</a><br />
                                        </strong><strong style="font-weight: 400;">Taxes that appear on your online order are
                                            approximate.</strong> The actual taxes charged to your credit card will reflect...
                                        <a href="../../services/order-sales-tax.aspx">Read more</a>.<span style="text-decoration: none;"><br />
                                        </span>
                                    </td>
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-shipping-policies.aspx">Shipping Policies</a><br />
                                        </strong>Your shipping/handling fee is determined by the parcel's weight and destination.
                                        Value is also considered for...<em> </em><a href="../../services/order-shipping-policies.aspx">
                                            Read more</a>.
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-shipping-restrictions.aspx">Shipping Restrictions</a><br />
                                        </strong>Certain destinations are only serviced by select shipping methods. Depending
                                        on the shipping address you specify, you will only be... <a href="../../services/order-shipping-restrictions.aspx">
                                            Read more</a>.
                                    </td>
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-truck-delivery.aspx">Truck Delivery</a><br />
                                        </strong>Due to size and/or weight, salon furniture, pedicure spa and facial equipment
                                        require special shipping &amp; handling that are assessed...<em> </em><a href="../../services/order-truck-delivery.aspx">
                                            Read more</a>.
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-warranty.aspx">Warranty</a><br />
                                        </strong>All equipment when sold is new and carries a limited manufacturer&rsquo;s
                                        warranty. Once you purchase the equipment it will be your... <a href="../../services/order-warranty.aspx">
                                            Read more</a>.<span style="text-decoration: none;"><br />
                                            </span>
                                    </td>
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/free-shipping-policies.aspx">Free Shipping Info</a><br />
                                        </strong>Free Shipping with purchase of $<%=Utility.ConfigData.FreeShippingOrderAmount() %>. No coupon required... <a href="../../services/free-shipping-policies.aspx">
                                            Read more</a>.<span style="text-decoration: none;"><br />
                                            </span>
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td width="50%" valign="top">
                                        <br />
                                    </td>
                                    <td width="50%" valign="top">
                                        <strong><a href="../../services/order-samples-promotional-items.aspx">Samples &amp;
                                            promotional items</a><br />
                                        </strong>Please note that excessive abuse or misuse of promotions and codes may
                                        result in order or item cancellation... <a href="../../services/free-shipping-policies.aspx">
                                            Read more</a>.<span style="text-decoration: none;"><br />
                                            </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <p class="sTitle">
                            <strong>
                                <br />
                                <br />
                            </strong>
                        </p>
                        <input id="gwProxy" type="hidden" /><!--Session data--><input id="jsProxy" onclick="jsCall();"
                            type="hidden" />
                        <div id="refHTML">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        
        <div>
            <CT:NavigationRegion runat="server" ID="NavigationRegion" />
        </div>
    </center>
</asp:Content>
