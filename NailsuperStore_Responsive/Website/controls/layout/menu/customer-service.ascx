<%@ Control Language="VB" AutoEventWireup="false" CodeFile="customer-service.ascx.vb"
    Inherits="controls_layout_customer_service_menu" %>
<nav class="left-nav">
    <div id="divCustomerServiceMenu" class="titleroot">Customer Service</div>
    <ul id="customer-service">
    <% If String.IsNullOrEmpty(Args) Then%>
      <%= GenerateGroupMenu("contact", "Contact Us", "/contact/default.aspx")%>
      <%= GenerateGroupMenu("about", "Company Info", "/service/about.aspx")%>
      <%= GenerateGroupMenu("order", "Ordering", "/service/order.aspx")%>
      <%= GenerateGroupMenu("shipping", "Shipping", "/service/shipping.aspx")%>
      <%= GenerateGroupMenu("return", "Returning and Exchange", "/service/return.aspx")%>
      <%= GenerateGroupMenu("", "FAQs", "/service/faq.aspx")%>
      <%= GenerateGroupMenu("", "Reward Points Program", "/services/reward-point-program.aspx")%>
      <%= GenerateGroupMenu("", "Referral Program", "/services/refer-friend-program.aspx")%>     
      <%= GenerateGroupMenu("", "Our Catalog", "/service/catalog.aspx")%>
      <%= GenerateGroupMenu("salon-template", "Salon Template", "/service/salon-template.aspx")%>      
      <%= GenerateGroupMenu("privacy", "Legal Notice and Privacy", "/service/privacy.aspx")%>
      <%= GenerateGroupMenu("", "Site Map", "/sitemap/default.aspx")%>
      
     <%ElseIf Args = "order" Then%>
        <%= GenerateGroupMenu("order", "Ordering", "/service/order.aspx")%>
        <%ElseIf Args = "shipping" Then%>
        <%= GenerateGroupMenu("shipping", "Shipping", "/service/shipping.aspx")%>
     <%ElseIf Args = "return" Then%>
        <%= GenerateGroupMenu("return", "Returning and Exchange", "/service/return.aspx")%>
   
     <%ElseIf Args = "contact" Then%>
        <%= GenerateGroupMenu("contact", "Contact Us", "/contact/default.aspx")%>
     <%ElseIf Args = "about" Then%>
        <%= GenerateGroupMenu("about", "Company Information", "/service/about.aspx")%>
     <%ElseIf Args = "privacy" Then%>
        <%= GenerateGroupMenu("privacy", "Legal Notice and Privacy", "/service/privacy.aspx")%>
     <%ElseIf Args = "salon-template" Then%>
        <%= GenerateGroupMenu("salon-template", "Salon Template", "/service/salon-template.aspx")%>
      <%End If%>  
    
    </ul>
</nav>
<script type="text/javascript">
    $(document).ready(function () {
        CheckShowBreadCrumbMenuPopup('divCustomerServiceMenu', 'Customer Service');
    });
    $(window).resize(function () {
        CheckShowBreadCrumbMenuPopup('divCustomerServiceMenu', 'Customer Service');
    });
</script>
