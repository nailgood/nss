<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shipping-charges-international.ascx.vb"
    Inherits="controls_checkout_shipping_charges_international" %>
<article class="shipping-charge">
   <header>
        <p> Shipping Charges - International</p>
    </header>
    <div class="des">        
          <p>
                    Shipment outside the continental United States (including Hawaii and Alaska) are
                    subject to the international carriers shipping rate. An additional hazardous material
                    fee will be incurred as set forth by the international freight lines’ regulations.
                    You may choose to expedite your order by air. All rates are variable and estimates
                    given with each individual order. With the exception of Hawaii and Alaska, shipping
                    costs are billed directly to you by the carrier.</p>
                <p>
                    Duties, taxes, VAT and import costs are not included in the cost of delivery. These
                    fees are set and charged to you by the country of destination, based on the shipment
                    value and class of goods. These costs are billed directly to you by the carrier.</p>
                <p>
                    <%=Resources.Alert.Russia%></p>
    </div>
</article>
 <section class="secure visible-md visible-lg">
    <script language="JavaScript" type="text/javascript">        SiteSeal("<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/networksolutions.png", "NETEV", "none");</script>
</section>