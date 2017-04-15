<%@ Control Language="VB"  AutoEventWireup="false" CodeFile="review.ascx.vb" Inherits="controls_Review" EnableViewState="true" %>
<%@ Register Src="~/controls/layout/pager.ascx" TagName="pager" TagPrefix="uc1" %>
<script type="text/javascript" src="/includes/scripts/popup.js" ></script>

<asp:UpdatePanel ID="upComment" runat="server">
<ContentTemplate>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upComment">
    <ProgressTemplate>
        <center>
        Please wait...<br /><img src="~/includes/theme/images/loader.gif" alt="Waiting" />
        </center>
    </ProgressTemplate>
</asp:UpdateProgress>

<ul class="sumary" id="countReview" runat="server">
    <li class="countreview"><asp:Literal ID="ltrReviewCount" runat="server"></asp:Literal></li>
    <li class="border"><span>&nbsp;</span> </li>
    <li class="writereview"><asp:Literal ID="litWriteReview" runat="server"></asp:Literal></li>
</ul>
<div id="dvReviewComment">
 
    <div id="dvListComment">
        <asp:Repeater EnableViewState="true" runat="server" ID="rptCommnents" OnItemDataBound="rptCommnents_ItemDataBound">
            <ItemTemplate>
                <div class="dvComment">
                    <div class="dvReviewInfo">
                         <asp:Label runat="server" ID="lbCustName" CssClass="name"></asp:Label>
                         <asp:Label runat="server" ID="lbPostDate" CssClass="date"></asp:Label>
                    </div>
                    <asp:Literal runat="server" ID="ltrComment"></asp:Literal>
                    <div class="dvReplyComment">
                        <asp:Literal ID="litReply" runat="server"></asp:Literal>                        
                        <div><asp:ImageButton runat="server" ImageUrl="~/includes/theme/images/cleardot.gif" CssClass="rl" ID="imgLike" CommandName="like" /></div>
                        <span class="count"><asp:Literal runat="server" ID="ltrCountLike"></asp:Literal></span>                        
                        <div><asp:ImageButton runat="server" ImageUrl="~/includes/theme/images/cleardot.gif" CssClass="rul" ID="imgUnLike" CommandName="unlike" /></div>
                        <span class="count"><asp:Literal runat="server" ID="ltrCountUnLike"></asp:Literal></span>
                    </div>                    
                    <div class="dvListReplyComment">
                        <asp:Repeater EnableViewState="true" runat="server" ID="rptReplyCommnents" OnItemCommand="rptReplyCommnents_ItemCommand">
                            <ItemTemplate>
                                <div class="dvComment">
                                    <div class="dvReviewInfo">
                                         <asp:Label runat="server" ID="lbReplyCustName" CssClass="name"></asp:Label>
                                         <asp:Label runat="server" ID="lbReplyPostDate" CssClass="date"></asp:Label>
                                    </div>
                                    <asp:Literal runat="server" ID="ltrReplyComment"></asp:Literal>
                                    <div class="dvReplyComment">
                                        <asp:Literal ID="litReplyComment" runat="server"></asp:Literal>                                        
                                        <div><asp:ImageButton runat="server" ImageUrl="~/includes/theme/images/cleardot.gif" CssClass="rl" ID="imgLike" CommandName="like" /></div>
                                        <span class="count"><asp:Literal runat="server" ID="ltrReplyCountLike"></asp:Literal></span>                                        
                                        <div><asp:ImageButton runat="server" ImageUrl="~/includes/theme/images/cleardot.gif" CssClass="rul" ID="imgUnLike" CommandName="unlike" /></div>
                                        <span class="count"><asp:Literal runat="server" ID="ltrReplyCountUnLike"></asp:Literal></span>
                                    </div>
                                </div>                                
                            </ItemTemplate>                                         
                        </asp:Repeater> 
                       <%-- <asp:Panel Visible="false" runat="server" id="postReply" class="postReplyComment" >
                            <asp:Literal runat="server" ID="ltrReplyLogin"></asp:Literal>
                            <div class="dvClose"><asp:ImageButton runat="server" CommandName="CloseReply" ImageUrl="~/includes/theme/images/close.gif" /></div>
                            <asp:TextBox runat="server" ID="txtReply" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            <div class="btnSumit"> 
                                <asp:Button CommandName="Reply" runat="server" id="btnReply" UseSubmitBehavior="false" class="bg-bt-submit" Text="Reply" />
                            </div>
                        </asp:Panel>--%>
                    </div>                    
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
</div>
<div id="divPaging" runat="server" class="paging" style="margin: 10px 10px 0px 0px; float: right">
   <uc1:pager ID="pagerBottom" runat="server" ViewMode="1" OnPageSizeChanging="pagerBottom_PageSizeChanging" OnPageIndexChanging="pagerBottom_PageIndexChanging" />
</div>


 <div id="divOverlay" style="display:none;" class="clipOverlay" >&nbsp;</div>
    <div style="display: none" class="lightbox" id="divBackground">&nbsp;</div>       
    <div id="dSuccess" style="display:none;">
    <div id="divSuccess">       
        <div class="title"><img src="~/includes/theme/images/icon_success.gif" /> <div class="txt">Success!</div></div>
        <div class="content" id="dSuccessContent"></div>           
        <div class="bot"><div class="closeLink"><a href="javascript:void(0);" onclick="hideMessage()" class="center noul">Close Window</a></div></div>
    </div>       
    </div>
  
<script type="text/javascript">
    function ShowMessage(message) {        
        var dError = document.getElementById('dSuccess');
        var divBackground = document.getElementById('divBackground');
        f_putScreen(dError, divBackground, true);
        document.getElementById('dSuccessContent').innerHTML = message;
    }
    function hideMessage() {
        var dError = document.getElementById('dSuccess');
        var divBackground = document.getElementById('divBackground');
        f_putScreen(dError, divBackground, false);
    }
</script>
</ContentTemplate>
</asp:UpdatePanel>


