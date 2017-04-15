<%@ Control Language="VB" AutoEventWireup="false" CodeFile="main.ascx.vb" Inherits="controls_MainBanner" %>
<%--<link href="/includes/scripts/soon/soon.css" rel="stylesheet">
<script src="/includes/scripts/soon/soon.min.js"></script>--%>


<div class="main-banner">
<%--<div id="countdown">
<div id="countboard">
    <div class="soon" id="my-soon-counter"
         data-due="2015-11-25T23:59:58"
         data-now="<%=SecondAnniversary %>"
         data-layout="group spacey label-smaller"
         data-scale-max="fill"
         data-format="d,h,m,s"
         data-separator=":"
         data-face="flip color-dark corners-sharp">
    </div>
    
</div>
</div>--%>

    <script type="text/javascript">
        $(document).ready(function () {

            $('.main-banner-slider').bxSlider({

                hideControlOnEnd: true,
                infiniteLoop: true,
                autoControls: true
            });
        });
    </script>

    <ul class="main-banner-slider" id="ulData" runat="server">
    </ul>

<%--<style type="text/css">
#my-soon-counter {background-color:#ffffff;}
#my-soon-counter .soon-reflection {background-color:#ffffff;background-image:linear-gradient(#ffffff 25%,rgba(255,255,255,0));}
#my-soon-counter {background-position:top;}
#my-soon-counter {color:#929292; height:100px; width:100%; }
</style>--%>
<%--    <link rel="stylesheet" href="/includes/scripts/flipclock/flipclock.css" />
	<script src="/includes/scripts/flipclock/flipclock.js"></script>

    <div id="countdown" class="clock"></div>
	<script type="text/javascript">
	    var clock;

	    $(document).ready(function () {
	        var clock;

	        clock = $('.clock').FlipClock({
	            clockFace: 'DailyCounter',
	            autoStart: false,
	            callbacks: {
	                stop: function () {
	                    $('#countdown').css('display', 'none');
	                }
	            }
	        });

	        clock.setTime(<%=SecondAnniversary %>);
	        clock.setCountdown(true);
	        clock.start();

	    });
	</script>--%>
</div>