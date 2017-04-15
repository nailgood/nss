<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detail.aspx.vb" Inherits="videos_Detail" MasterPageFile="~/includes/masterpage/interior.master" EnableViewState="true" %>

<%@ Register Src="~/controls/resource-center/review.ascx" TagName="Review" TagPrefix="rv" %>
<%@ Register Src="~/controls/product/related-item.ascx" TagName="RelatedItem" TagPrefix="uc" %>
<%@ Register Src="~/controls/resource-center/video/video-list.ascx" TagName="video" TagPrefix="uclstvideo" %>
<%@ Register Src="~/controls/layout/addthis.ascx" TagName="share" TagPrefix="usSocialShare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">  
   <%--<script type="text/javascript" src="/includes/scripts/jquery-1.7.1.min.js?v=20150929035834"></script>--%>
   <%-- <div id="contentnews-left">
        <div id="smcategoryNews" style="display:none;">
            <div id="smtitleNews"></div>
            <div id="smcategory"></div>
        </div>
    </div>--%>
            <div id="video-detail" itemprop="video" itemscope itemtype="http://schema.org/VideoObject">
                <%--<meta itemprop="url" href="<%#Request.RawUrl %>"  />--%>
                <meta content="Flash" itemprop="playerType" />
                <meta content="770" itemprop="width" />
                <meta content="433" itemprop="height" />
                <asp:Literal ID="ltEmbedUrl" runat="server"></asp:Literal>
                <!--Embeded Video-->
                <div id="videoPopup">
                    <asp:Panel ID="pnFlash" runat="server">
                        <asp:Literal ID="ltVideo" runat="server"></asp:Literal>
                        <%If Not (String.IsNullOrEmpty(videoURLImg)) Then%>
                            <meta itemprop="thumbnailURL" content="<%= Utility.ConfigData.CDNmediapath & videoURLImg %>" />
                        <%End If%>
                        <%If Not (String.IsNullOrEmpty(flashVideoURL)) Then%><meta itemprop="contentURL" content="<%=Utility.ConfigData.CDNmediapath & flashVideoURL %>" /><%End If%>
                    </asp:Panel>
                </div>
                <div class="title">
                    <h1 itemprop="name">
                        <asp:Literal ID="ltTitle" runat="server"></asp:Literal></h1>
                </div>
                <div class="dvDate">
                    <div class="date">
                        <asp:Literal ID="ltDate" runat="server"></asp:Literal>
                    </div>
                     <div class="viewTotal">
                        <span id="icomment"></span><%=TotalComment%>                        
                    </div>
                    <div class="viewTotal">
                        <span id="ivote"></span>
                        <%=TotalLike %>
                    </div>
                     <div class="viewTotal">
                       <span id="iconview"></span>
                        <asp:Literal ID="ltrViewCount" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="video-sdes">
                    <div runat="server" id="dvFullDesc" itemprop="description">
                        <asp:Literal ID="ltFullDesc" runat="server"></asp:Literal></div>
                </div>
                <div class="shareHeader">
                     <div id="share_social">
                        <span>Share:</span>    
                        <usSocialShare:share ID="ushare" runat="server" />
                        <a id="sh_embed" onclick="changePackageTab('embed')"><img src="<%=Utility.ConfigData.CDNmediapath %>/includes/theme/images/embed.png" alt="" /></a>
                    </div>
                    <div id="likesocial">
                        <div class="facebook">
                            <div class="fb-like" data-href="<%= GlobalSecureName &  Request.RawUrl %>" data-width="82" data-layout="button_count"
                                data-action="like" data-show-faces="true" data-share="false">
                            </div>
                        </div>
                        <div class="twitter">
                            <a href="https://twitter.com/share" class="twitter-share-button" data-via="nailsplash">
                                Tweet</a>
                            <script>                                !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>
                        </div>
                        <div class="google">
                            <script type="text/javascript" src="https://apis.google.com/js/plusone.js">                                { lang: 'en' } </script>
                            <g:plusone size="medium"></g:plusone>
                        </div>
                    </div>                   
                </div>
            </div>
        <%--<div id="contentframe" style="display:none"></div>--%>
<%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBottom" runat="Server">--%>
    <%--<script type="text/javascript" src="/includes/video/swfobject.js"></script>
    <script type="text/javascript" src="/includes/scripts/ZeroClipboard/ZeroClipboard.js"></script>--%>
    <style>
        /*Css form only video page*/
        @media screen and (max-width: 375px){
            #main-page{margin-bottom: 40px;}
            #list-item .review a{float: none}
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var totalcomment = <%=TotalComment %>;
            if (totalcomment<1 & ($("#tabnav_item").css('display') == 'block'))
            {
                $("#tab_comment").css('display','none');
                $('#tabnav_item').addClass('selected');
                $("#tab_item").css('display','block');
                if($(window).width() <= 767){
                    $("#tab_item").css('display','none');
                }
            }
            else if(totalcomment<1 & ($("#tabnav_video").css('display') == 'block'))
            {
                $("#tab_comment").css('display','none');
                $('#tabnav_video').addClass('selected');
                $("#tab_video").css('display','block');
            }
        });

        function changePackageTab(tabname) {
            document.getElementById('tab_comment').style.display = 'none';
            document.getElementById('tabnav_comment').className = '';
            document.getElementById('tab_item').style.display = 'none';
            document.getElementById('tabnav_item').className = '';
            document.getElementById('tab_video').style.display = 'none';
            document.getElementById('tabnav_video').className = '';
            document.getElementById('tab_embed').style.display = 'none';
            document.getElementById('tabnav_embed').className = '';
            if (tabname != '') {
         
                document.getElementById('tabnav_' + tabname).className = 'selected';
                document.getElementById('tab_' + tabname).style.display = 'block';
                if (id = "embed") {
                    fnSetInputVal(0,'', 0, 0);
                   // SetValueButtonCopy();                  
                }                 
            }
        }

        function EnterCustomW(e, ctr) {
            var value = ctr.value;
            var id = ctr.id;
            var key;
            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox
            if (key == 13) {
                return;
            }
            else {
                var w = ctr.value;
                w = parseInt(w);
                var percent = 1.33333;
                var h = w / percent;
                h = Math.round(h);
                h = parseInt(h)
                if (w > 0 && h > 0) {
                    document.getElementById('txtCustomsizeH').value = h;
                    fnSetInputVal(2,ctr, w, h);
                    //SetValueButtonCopy();
                }
            }
            return false;
        }
        function EnterCustomH(e, ctr) {

            var value = ctr.value;
            var id = ctr.id;
            var key;
            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox
            if (key == 13) {
                return;
            }
            else {
                var h = ctr.value;
                var percent = 1.33333;
                var w = h * percent;
                w = Math.round(w);
                w = parseInt(w)
                h = parseInt(h)
                if (w > 0 && h > 0) {
                    fnSetInputVal(2,ctr, w, h);
                    document.getElementById('txtCustomsizeW').value = w;
                   // SetValueButtonCopy();
                }
            }
            return false;
        }

        function ChangeSize(ctr) {
            if (ctr.value == 'custom') {
                document.getElementById('tdCustomerSize').style.display = '';
            }
            else {
                document.getElementById('tdCustomerSize').style.display = 'none';
                fnSetInputVal(1,ctr, 0, 0);
               // SetValueButtonCopy();
            }
        }
        $(window).load(function () {
            ShowTabOfVideo();
           //DelayResetHeightListVideo(true,'tabvideo');
           fnGetContentEmbed();
        });
        $(window).ready(function() {
            fnGetContentEmbed();
        });
        $(window).resize(function () { 
            ShowTabOfVideo();
           //DelayResetHeightListVideo(true,'tabvideo');
            fnGetContentEmbed();
       });
        fnGetContentEmbed = function () {
        //    $("#contentframe").load("/video/getembed.aspx iframe");
           fnSetInputVal(0,'', 0, 0);
        }
        fnSetInputVal = function (isSize, ctr, width, height) {
            var valInput = '',
            w = 480,
            h = 360;
            url='https://www.nailsuperstore.com';
            var videoUrl= window.location.href;
            var splitUrl = videoUrl.lastIndexOf('/');
            var videoId = videoUrl.substring(splitUrl +1);
            //valInput = $("#contentframe").html();
            if (isSize == 1) {
                var arr = new Array();
                arr = ctr.value.split('-'); //$("#drlSize")val().split('-');
                w = arr[0].toString();
                h = arr[1].toString();
            }
            if (isSize == 2) {
                w = width;
                h = height;
            }
            valInput = '<iframe width="{0}" height="{1}" scrolling="no"  src="' + url  + '/embed/how-to-video/'+ videoId + '?autoplay=false" frameborder="0"></iframe>';
            valInput = valInput.replace('{0}', w);
            valInput = valInput.replace('{1}', h);
            $("#txtEmbed").val(valInput);
        }
        
        </script>
    <div id="comment" runat="server">
        <div class="tabbao" id="tabbox">
            <nav>
               <a id="tabnav_comment" onclick="changePackageTab('comment')" runat="server">All Reviews (<%=TotalComment%>)<span class="arrow"></span></a>
                <div id="stab_comment" style="display:none;"></div>

                <a id="tabnav_item" onclick="changePackageTab('item')" runat="server">Related Items<span class="arrow"></span></a>
                <div id="stab_item" style="display:block;"></div>

                <a id="tabnav_video" onclick="changePackageTab('video')" runat="server">Related Videos<span class="arrow"></span></a>
                <div id="stab_video" style="display:none;"></div>                 

                <a id="tabnav_embed" onclick="changePackageTab('embed')">Embed<span class="arrow"></span></a>
                <div id="stab_embed" style="display:none;"></div>
            </nav>
        </div>
       <div id="tab_comment" class="boxtab">
            <rv:Review runat="server" ID="rvComment" />
        </div>

        <div id="tab_item" class="boxtab" style="display: none;">
            <uc:RelatedItem ID="ucRelatedItem" runat="server" />
        </div>
        
       <div id="tab_video" class="boxtab" style="display: none;">
            <uclstvideo:video ID="rlVideo" runat="server" />
       </div>

        <div id="tab_embed" class="boxtab" style="display: none;">
            <input type="hidden" id="hidVideoEmbedLink" name="hidVideoEmbedLink" runat="server" />
            <div class="input-group input-group-lg">
                <div id="lbtnCopy" class="input-group-addon" ngclipboard data-clipboard-target="#txtEmbed"><span class="copy"></span>Copy Embed:</div>
                 <asp:TextBox ID="txtEmbed"  CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                </div>
            <div class="rowembed n-dropdown">  
                <span> Video size:</span>
                <asp:DropDownList ID="drlSize" onChange="ChangeSize(this);" runat="server"  CssClass="form-control">
                            <asp:ListItem Value="480-360">480 x 360</asp:ListItem>
                            <asp:ListItem Value="640-480">640 x 480</asp:ListItem>
                            <asp:ListItem Value="960-720">960 x 720</asp:ListItem>
                            <asp:ListItem Value="custom">Custom size</asp:ListItem>
                        </asp:DropDownList>
                <div id="tdCustomerSize" style="display: none;">
                            <asp:TextBox ID="txtCustomsizeW" onblur="EnterCustomW(event,this);" MaxLength="4" CssClass="form-control"
                                runat="server"></asp:TextBox>
                                <span>x</span>
                            <asp:TextBox ID="txtCustomsizeH" MaxLength="4" onblur="EnterCustomH(event,this);" CssClass="form-control"
                                runat="server"></asp:TextBox></div>
            </div> 
               <div id="copytext"></div>        
        </div>
        <script type="text/javascript">
            var myApp = angular.module('app', ['ngclipboard']);
        </script>
    </div>
</asp:Content>
