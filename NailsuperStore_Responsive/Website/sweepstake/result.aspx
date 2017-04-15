<%@ Page Title="Sweepstake Result" Language="VB" MasterPageFile="~/includes/masterpage/main.master" AutoEventWireup="false" CodeFile="result.aspx.vb" Inherits="Sweepstake_Result" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="fb-root"></div>
    <script>    (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=182923348452071";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));
        function Reload() {
            location.reload();
        }
    </script>
    <script src="/includes/scripts/soon/soon.min.js"></script>
    <h1><%=Title %></h1>
    <div id="Sweepstake">
    <div id="Countdown" runat="server">
     <div class="starttext">Start with in</div>
    <div id="sscountdown">
        <div id="countboard">
            <div class="soon" id="my-soon-counter"
                 data-due="<%=DrawingDate %>"
                 data-now="<%=CountDownEndDate %>"
                 data-layout="group spacey label-smaller"
                 data-scale-max="fill"
                 data-format="d,h,m,s"
                 data-separator=":"
                 data-event-complete="Reload"
                 data-face="flip color-dark corners-sharp">
            </div>
    
        </div>
    </div>
    </div>
    <%If isShowVideo Then%>
            <div id="lbName" runat="server"></div>
            <div id="video-detail" itemprop="video" itemscope itemtype="http://schema.org/VideoObject">
                <%--<meta itemprop="url" href="<%#Request.RawUrl %>"  />--%>
                <meta content="Flash" itemprop="playerType" />
                <meta content="770" itemprop="width" />
                <meta content="300" itemprop="height" />
                <asp:Literal ID="ltEmbedUrl" runat="server"></asp:Literal>
                <!--Embeded Video-->
                <div id="videoPopup">
                    <asp:Panel ID="pnFlash" runat="server">
                        <asp:Literal ID="ltVideo" runat="server"></asp:Literal>
                    </asp:Panel>
                </div>
            </div>
           
    <% End If%>
   <div id="fbcomment" class="fb-comments" data-href="https://www.nailsuperstore.com/sweepstake/<%=Id %>" data-width="100%" data-numposts="5"></div>
   </div>

<style type="text/css">
#my-soon-counter {background-color:#ffffff;}
#my-soon-counter .soon-reflection {background-color:#ffffff;background-image:linear-gradient(#ffffff 25%,rgba(255,255,255,0));}
#my-soon-counter {background-position:top;}
#my-soon-counter {color:#929292; height:100px; width:100%; }


</style>
</asp:Content>
 