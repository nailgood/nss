<%@ Control Language="VB" AutoEventWireup="false" CodeFile="google-analytics.ascx.vb" Inherits="controls_GoogleTracking" %>
<asp:literal id="litGoogleTrustedStore" Runat="server"></asp:literal>
<script type="text/javascript">
(function (i, s, o, g, r, a, m) { i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () { (i[r].q = i[r].q || []).push(arguments) }, i[r].l = 1 * new Date(); a = s.createElement(o),
m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m) })(window, document, 'script', '../../includes/scripts/google-analytics.js', 'ga');

    try {
    
        ga('create', '<%=Utility.ConfigData.GAProperty() %>', 'auto');
        <asp:literal id="litGoogleOrderTracking" Runat="server"></asp:literal>    
        <asp:literal id="litlGooglePageView" Runat="server"></asp:literal>
        
    } catch (e) {
        sendError(e.message);
    }
</script>

<asp:Panel ID="pnScript" runat="server">
<script type="text/javascript">
            var trackcmp_email = '';
            var trackcmp = document.createElement("script");
            trackcmp.async = true;
            trackcmp.type = 'text/javascript';
            trackcmp.src = '//trackcmp.net/visit?actid=65698332&e='+trackcmp_email +'&r='+encodeURIComponent(document.referrer)+'&u='+encodeURIComponent(window.location.href);
            var trackcmp_s = document.getElementsByTagName("script");
            if (trackcmp_s.length) {
                        trackcmp_s[0].parentNode.appendChild(trackcmp);
            } else {
                        var trackcmp_h = document.getElementsByTagName("head");
                        trackcmp_h.length && trackcmp_h[0].appendChild(trackcmp);
            }
</script>

    <!-- BEGIN: Google Trusted Stores -->
    <script type="text/javascript">
        var gts = gts || [];

        gts.push(["id", "576381"]);
        gts.push(["badge_position", "BOTTOM_RIGHT"]);
        gts.push(["locale", "en_US"]);
        gts.push(["google_base_offer_id", ""]);
        gts.push(["google_base_subaccount_id", ""]);

        (function() {
            var gts = document.createElement("script");
            gts.type = "text/javascript";
            gts.async = true;
            gts.src = "https://www.googlecommerce.com/trustedstores/api/js";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(gts, s);
        })();
    </script>
    <!-- END: Google Trusted Stores -->


<script type="text/javascript">    (function () {
        var _fbq = window._fbq || (window._fbq = []);
        if (!_fbq.loaded) {
            var fbds = document.createElement('script');
            fbds.async = true;
            fbds.src = '../../includes/scripts/facebook.js';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(fbds, s);
            _fbq.loaded = true;
        }
        _fbq.push(['addPixelId', '1434494766846583']);
    })();
    window._fbq = window._fbq || [];
    window._fbq.push(['track', 'PixelInitialized', {}]);
</script>
<%--<script type='text/javascript'>
window.__lo_site_id = 51652;

	(function() {
		var wa = document.createElement('script'); wa.type = 'text/javascript'; wa.async = true;
		wa.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://cdn') + '.luckyorange.com/w.js';
		var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(wa, s);
	  })();
	</script>--%>



<noscript><img height="1" width="1" alt="Pixel Initialized" style="display:none" src="https://www.facebook.com/tr?id=1434494766846583&amp;ev=PixelInitialized" /></noscript>
</asp:Panel>
<%--//www.google-analytics.com/analytics.js--%>
<%--https://connect.facebook.net/en_US/fbds.js--%>



