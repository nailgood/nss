<%@ Page Language="VB" AutoEventWireup="false" CodeFile="share.aspx.vb" Inherits="video_share"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=videoTitle %></title>
<asp:Literal ID="ltCanonical" runat="server"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
    <div id="videoPopup">
        <asp:Panel ID="pnFlash" runat="server">
            <%If Not (String.IsNullOrEmpty(html5Video)) Then%>
            <%=html5Video%>
            <%Else%>
            <asp:Literal ID="ltVideo" runat="server"></asp:Literal>
            <%If (isLocalVideo) Then%>
            <div id="FlabellComponent">
            </div>
            <%End If%>
            <%End If%>
        </asp:Panel>
    </div>


    <%--<script type="text/javascript" src="/includes/video/swfobject.js"></script> Edit css.xml--%>
        <asp:Literal ID="litScript" runat="server"></asp:Literal>

    <script type="text/javascript">

        function GetFrameSize() {
            var w = 0;
            var h = 0;
            if (!window.innerWidth) {
                if (!(document.documentElement.clientWidth == 0)) {
                    //strict mode
                    w = document.documentElement.clientWidth; h = document.documentElement.clientHeight;
                } else {
                    //quirks mode
                    w = document.body.clientWidth; h = document.body.clientHeight;
                }
            } else {
                //w3c
                w = window.innerWidth; h = window.innerHeight;
            }
            document.getElementById('<%=hidFrameW.ClientID %>').value = w;
            document.getElementById('<%=hidFrameH.ClientID %>').value = h;
            
            document.forms[0].submit();


        }
        function BuildFlashVideo(stageW, stageH) {

            // JAVASCRIPT VARS
            // cache buster
            var cacheBuster = "?t=" + Date.parse(new Date());

          


            // ATTRIBUTES
            var attributes = {};
            attributes.id = 'FlabellComponent';
            attributes.name = attributes.id;

            // PARAMS
            var params = {};
            params.allowfullscreen = "true";
            params.allowScriptAccess = "always";
            params.bgcolor = "#000000";
            //params.wmode = "transparent";


            /* FLASH VARS */
            var flashvars = {};

            /// path to the content folder(where the xml files, images or video are nested)
            /// if you want to use absolute paths(like "http://domain.com/images/....") then leave it empty("")
            flashvars.pathToFiles = "";

            // player dimensions
            flashvars.componentWidth = stageW;
            flashvars.componentHeight = stageH;
            // player settings xml path
            flashvars.xmlPath = "/includes/video/playersettings.xml";
            // video title
            flashvars.videoTitle = "<%=videoTitle %>";
            // video path
            flashvars.videoPath = '<%=flashVideoURL %>';
            flashvars.streamer = "";
            // preview path
            // flashvars.previewPath = '<%=videoURLImg %>';
            flashvars.autoPlay = 'true';
            /** EMBED THE SWF**/
            swfobject.embedSWF("/includes/video/preview.swf" + cacheBuster, attributes.id, stageW, stageH, "9.0.124", "/includes/video/expressInstall.swf", flashvars, params);
        }
    </script>

    <input type="hidden" runat="server" id="hidFrameW" name="hidFrameW" />
    <input type="hidden" runat="server" id="hidFrameH" name="hidFrameH" />
    <asp:Literal ID="ltrScript" runat="server"></asp:Literal>
    </form>
</body>
</html>
