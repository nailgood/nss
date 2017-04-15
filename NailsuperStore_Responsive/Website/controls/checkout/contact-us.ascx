<%@ Control Language="VB" AutoEventWireup="false" CodeFile="contact-us.ascx.vb" Inherits="controls_checkout_contact_us" %>
 <section class="checkout-contact-us visible-md visible-lg">
    <div>
        <div class="customer-title">Customer Service</div>
        <div class="customer-content">
            If you're having any difficulty completing your order, feel free to contact us and we'll gladly help. <span>Call 1.800.669.9430 US / 1.847.260.4000 International</span>
        </div>
    </div>
    <div class="contact">
        <a href="/contact/default.aspx">Email Us</a>
    </div>
    <% If isShowLiveChat Then%>
    <div class="leave-message">
        <div id="scPr0e"></div>
        <div id="scHwEb"></div>
        <div id="sdHwEb"></div>
        <script type="text/javascript">
            var seHwEb = document.createElement("script");
            seHwEb.type = "text/javascript";
            var seHwEbs = "/includes/scripts/live-chat-safe-textlink.js?ps_h=HwEb&ps_t=" + new Date().getTime() + "&online-link-html=Live%20Chat&offline-link-html=Leave%20A%20Message";
            setTimeout("seHwEb.src=seHwEbs;document.getElementById('sdHwEb').appendChild(seHwEb)", 1000)
        </script>
        <noscript>
            <div style="display: inline">
                <a href="http://www.providesupport.com?messenger=nailsuperstore">Live Customer Service</a></div>
        </noscript>
        
        <div id="sdPr0e"></div>
        <script type="text/javascript">
            var sePr0e = document.createElement("script");
            sePr0e.type = "text/javascript";
            var sePr0es = "/includes/scripts/live-chat-safe-standard.js?ps_h=Pr0e&ps_t=" + new Date().getTime() + "&online-image=https%3A//www.nailsuperstore.com/images/chat_online.png&offline-image=https%3A//www.nailsuperstore.com/images/chat_offline.png";
            setTimeout("sePr0e.src=sePr0es;document.getElementById('sdPr0e').appendChild(sePr0e)", 1000)
        </script>
        <noscript>
            <div style="display: inline">
                <a href="http://www.providesupport.com?messenger=nailsuperstore">Live Customer Service</a></div>
        </noscript>
    </div>
    <%End If%>
</section>