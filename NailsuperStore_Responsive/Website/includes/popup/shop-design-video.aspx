<%@ Page Language="VB" AutoEventWireup="false" CodeFile="shop-design-video.aspx.vb" Inherits="Popup_ShopDesignVideo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href='//fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' />
    <%--<link href="/includes/theme/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/default.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/videopopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/includes/scripts/jquery-1.11.1.min.js"></script> Edit css.xml--%>
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
</head>
<body id="bodyVideo">
    <div class="itemVideoPopup">
        <div id="videoPopup" runat="server">            
                <%If Not (String.IsNullOrEmpty(html5Video)) Then%>
                <%=html5Video%>
                <%Else%>
                <asp:Literal ID="ltVideo" runat="server"></asp:Literal>
                <%If (isLocalVideo) Then%>
                <div id="FlabellComponent">
                </div>
                <%End If%>
                <%End If%>
        </div>
        <div class="title" id="divTitle" runat="server">
        </div>
        <table width="500" height="220" align="center" cellpadding="10" id="tblError" runat="server" border="0" style="border: 1px solid #666666;">
            <tr>
                <td>
                    <p align="center">
                        <b>Sorry, an error has occurred on the page you are trying to access.</b></p>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">              

            function ResetHeight() {

                if (parent.document.getElementById('nyroModalContent')) {
                    var $el = parent.document.getElementById('nyroModalContent')
                    var w = $($el).width();
            
                   var h = (w / 3) * 2
                    $('#videoPopup').height(h);
                }
               
            }
            
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            ResetHeight();
        });
        $(window).resize(function () {
            ResetHeight();
        });
        if ((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i)) || (navigator.userAgent.match(/iPad/i)) || (navigator.userAgent.match(/Android/i)) || (navigator.userAgent.match(/IEMobile/i))) {
            var v = function (selector, context) { return (context || document).querySelector(selector) };
            var video = v("video"),
                iframe = v("iframe"),
				domPrefixes = 'Webkit Moz O ms Khtml'.split(' ');

            var fullscreen = function (elem) {
                var prefix;

                // Mozilla and webkit intialise fullscreen slightly differently
                for (var i = -1, len = domPrefixes.length; ++i < len; ) {
                    prefix = domPrefixes[i].toLowerCase();
                    if (elem[prefix + 'EnterFullScreen']) {
                        // Webkit uses EnterFullScreen for video
                        return prefix + 'EnterFullScreen';
                        break;
                    } else if (elem[prefix + 'RequestFullScreen']) {
                        // Mozilla uses RequestFullScreen for all elements and webkit uses it for non video elements
                        return prefix + 'RequestFullScreen';
                        break;
                    }
                }
                return false;
            };
            // Will return fullscreen method as a string if supported e.g. "mozRequestFullScreen" || false;
            var fullscreenvideo = fullscreen(document.createElement("video"));
            v("video").addEventListener("click", function () {
                video.play();
                video[fullscreenvideo]();
            }, false);
            v("video").addEventListener("canplay", function () {
                video.play();
                video[fullscreenvideo]();
            }, false);
        }
    </script>
</body>
</html>
