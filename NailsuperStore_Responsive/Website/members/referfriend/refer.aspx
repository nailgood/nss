<%@ Page Language="VB" AutoEventWireup="false" CodeFile="refer.aspx.vb" Inherits="members_ReferFriend_Refer" MasterPageFile="~/includes/masterpage/interior.master" %>
<%@ Register src="~/controls/layout/barstatus.ascx" tagname="barstatus" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<center>
<style>

</style>
<div id="content-page" preventDefaultButton="ctl02_btnSearch">
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>
   <%-- <script type="text/javascript" src="/includes/scripts/popup.js" ></script>
       <script type="text/javascript" src="/includes/scripts/ZeroClipboard/ZeroClipboard.js"></script> Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
<div id="divOverlay" style="display:none;" class="clipOverlay" >&nbsp;</div>
<div style="display: none" class="lightbox" id="divBackground">&nbsp;</div>  
<div id="content-notice" style="display:none">
    <div id="dNotice">
        <div class="title"><span class="pull-left"><img src="/includes/theme/images/icon_notice.png" />&nbsp;</span><div class="txt">Notice<a href="javascript:void(0);" onclick="hideError()" class="qtip-button" style="float: right; position: relative; cursor: pointer; display: block;"></a></div></div>
        <div class="content" id="dErrorContent"></div>       
    </div>
</div>
<div id="content-success" style="display:none">
<div id="dSuccess">
    <div id="divSuccess">       
        <div class="title"><span class="glyphicon glyphicon-ok pull-left"></span> <div class="txt"> Success!<a href="javascript:void(0);" onclick="hideError()"  class="qtip-button" style="float: right; position: relative; cursor: pointer; display: block;"></a></div></div>
        <div class="content" id="dSuccessContent"></div>   
        <div id="divErr" style="display:none;">
            <div class="errTitle"><span class="pull-left"><img src="/includes/theme/images/icon_notice.png" />&nbsp;</span><div class="txt">Notice</div></div>
            <div class="content" id="dErrorContent2"></div>
        </div>
     </div>       
</div>
</div>

<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
        <ProgressTemplate>
            <center>
                <div class="bg-loading">
                     Please wait...<br />
                     <img src="/includes/theme/images/loader.gif" alt="" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>

   
<div id="refermanager">
   <uc2:barstatus ID="barstatus1" runat="server" />
    <ul class="title">
        <li><div>Send By Email</div></li>
    </ul>
    <div>
        <p>      
        If your friend receives your referral email to sign up for an account and make to place an order, you will receive rewards points. 
        <a href="/services/refer-friend-program.aspx">Learn&nbsp;more</a> 
        </p>
         <div class="ctr">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="mailinput" TextMode="MultiLine" placeholder="Invite friends by email address..." spellcheck="false" >
                </asp:TextBox>
                <br /><asp:Button Runat="server" cssClass="btn btn-submit" id="btnSend"  onClientClick="return SendContact();"  Text="Send" CausesValidation="False" />   
         </div>
         <p><span>*</span> <span class="note">Send more email addresses by adding new lines.</span></p>
     </div>
    <ul class="title">
        <li><div>Share On Social Media</div></li>
    </ul>
  
    <div class="input-group input-group-lg">
      <div id="btnCopy" class="input-group-addon" ngclipboard data-clipboard-target="#txtLink"><span class="copy"></span>Copy this link:</div>
          <asp:TextBox ID="txtLink"  CssClass="form-control" runat="server" readonly="true"  placeholder="http://www.nailsuperstore.com"  Text="http://www.nailsuperstore.com"></asp:TextBox>
    </div>
    <div>
        <div class="btnfb" onclick="return ShareFB();">
            <i class="fa fa-facebook-official fb"></i><span class="hide-sm">Share on Facebook</span>
        </div>
        <div class="btntwt" onclick="return postTW();">
            <i class="fa fa-twitter tw" style="color: #4fb4e6; font-size: 20px;"></i></span><span class="hide-sm">Tweet on Twitter</span>
        </div>
        <asp:HiddenField Runat="server" ID="hdLinkFacebook"></asp:HiddenField>
        <asp:HiddenField Runat="server" ID="hdLinkTwitter"></asp:HiddenField>
    </div>
    <div id="copytext"></div>
</div>
        
    <script type="text/javascript">
        //AngularJS for copy link
        var myApp = angular.module('app', ['ngclipboard']);
        //
        //$(function () {
        //    if (!window.navigator.standalone && navigator.appVersion.indexOf('iPad') < 0 && navigator.appVersion.indexOf('iPhone') < 0) {
        //        $("#btnCopy").each(function () {
        //            var clip = new ZeroClipboard.Client();
        //            var btncopy = $(this);
        //            clip.glue(btncopy[0]);
        //            var txt = $("#txtLink").val();
        //            clip.setText(txt);
        //            clip.addEventListener('complete', function (client, text) {
        //                $("#txtLink").select();
        //            });
        //        });
        //    }
        //    else {
        //        $('#txtLink').attr('readonly', false);
        //        document.getElementById('btnCopy').style.display = "none";
        //        document.getElementById('dvCopy').style.display = "block";
        //    }
        //});

        var width = 730;
        var height = 480;

        function ShareFB() {
            var link = encodeURIComponent(document.getElementById("hdLinkFacebook").value);
            var img = encodeURIComponent('https://www.nailsuperstore.com/images/ShareLogoFB.png');
            var title = 'The Nail Superstore';
            var Desc = 'Register for an account with The Nail Superstore and get <%=totalPoint %> free points! ';
            var st = '?s=100&p[url]=' + link + '&p[images][0]=' + img + '&p[title]=' + title + '&p[summary]=' + Desc;

            var sharer = "https://www.facebook.com/sharer/sharer.php";

            window.open(sharer + st, "_blank", "width=730,height=480");
            return false;
        }
        function postTW() {
            var text = 'Register for an account with The Nail Superstore and get <%=totalPoint %> free points!  ';
            var url = encodeURIComponent(document.getElementById("hdLinkTwitter").value);
            window.open('http://twitter.com/share?url=' + url + '&text=' + text, '_blank', 'width=' + width + ',height=' + height);
            return false;
        }
        function ShowError2(msg) {
            $('#dErrorContent').html(msg);
            var dcontent = $("#content-notice").html();
            showQtip('qtip-notice', dcontent, 'Notice');
        }
        function ShowError1(message) {
            var dError = document.getElementById('dNotice');
            var divBackground = document.getElementById('divBackground');
            f_putScreen(dError, divBackground, true);
            document.getElementById('dErrorContent').innerHTML = message;
        }
        function hideError() {
            $(".qtip-active").css("display", "none");
            $("#qtip-blanket").css("display", "none");
//            var dError = document.getElementById('dNotice');
//            var divBackground = document.getElementById('divBackground');
//            f_putScreen(dError, divBackground, false);
        }
//        function ShowSuccess(message) {
//             var dSuccess = document.getElementById('dSuccess');
//            var divBackground = document.getElementById('divBackground');
//            f_putScreen(dSuccess, divBackground, true);
//            document.getElementById('dSuccessContent').innerHTML = message;
        //        }
        function ShowSuccess(msg) {
            $('#dSuccessContent').html(msg);
            var dcontent = $("#content-success").html();
            showQtip('qtip-success', dcontent, 'Success');
        }
        function ShowSuccessError(message, msgerror) {
            var divErr = document.getElementById('divErr');
            divErr.style.display = '';
            document.getElementById('dErrorContent2').innerHTML = msgerror;
            ShowSuccess(message);
        }
        function hideSuccess() {
            var dSuccess = document.getElementById('dSuccess');
            var divBackground = document.getElementById('divBackground');
            f_putScreen(dSuccess, divBackground, false);
        }
        var txtEmail = document.getElementById("txtEmail");
        if (txtEmail.value != '') {
            if (txtEmail.className.indexOf('bg') > 0)
                txtEmail.className = txtEmail.className.replace(' bg', ' nobg');
        }

        document.getElementById('txtLink').onclick = function () {
            this.select();
        };

        function SendContact() {
            var txtEmail = document.getElementById("txtEmail");
            if (txtEmail.value == '') {
                ShowError2("Please enter at least one email.<br/>");
                return false;
            }
            return true;
        }
        function FocusText() {
            var txtEmail = document.getElementById("txtEmail");
            if (txtEmail.className.indexOf('bg') > 0)
                txtEmail.className = txtEmail.className.replace(' bg', ' nobg');
        }
        function BlurText() {
            var txtEmail = document.getElementById("txtEmail");
            if (txtEmail.value == '') {
                if (txtEmail.className.indexOf('nobg') > 0)
                    txtEmail.className = txtEmail.className.replace(' nobg', ' bg');
            }
        }

        </script>
  </ContentTemplate>
</asp:UpdatePanel>   
</div>



</center>
</asp:Content>