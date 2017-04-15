<%@ Control Language="VB" AutoEventWireup="false" CodeFile="share-social.ascx.vb" Inherits="controls_resource_center_news_share_social" %>
<%@ Register Src="~/controls/layout/addthis.ascx" TagName="share" TagPrefix="usSocialShare" %>
<section id="multimedia">
        <asp:Literal ID="ltrMultimediaHeader" runat="server"></asp:Literal>
        <asp:Literal ID="ltrImage" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltrDoc" runat="server" Visible="false"></asp:Literal>
  
        <div id="share_social" style="clear:both; padding-top:10px;">
            <usSocialShare:share ID="uShare" runat="server" />
        </div>
        <div id="likesocial">
            <div class="facebook">                    
                <div class="fb-like" data-href="<%=Request.RawUrl %>" data-width="82" data-layout="button_count" data-action="like" data-show-faces="true" data-share="false"></div>                 
            </div>
            <div class="twitter">					    
	            <a href="https://twitter.com/share" class="twitter-share-button"  data-via="nailsplash">Tweet</a>
                <script>            !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>			    
            </div>
            <div class="google"><script type="text/javascript" src="https://apis.google.com/js/plusone.js">                            { lang: 'en' } </script>
                <g:plusone size="medium"></g:plusone>
            </div> 
        </div>
   
</section>

   