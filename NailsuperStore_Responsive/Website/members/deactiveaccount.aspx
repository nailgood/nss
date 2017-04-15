<%@ Page Language="VB" AutoEventWireup="false" CodeFile="deactiveaccount.aspx.vb" Inherits="members_deactiveaccount" MasterPageFile="~/includes/masterpage/main.master" %>
<%@ Register TagPrefix="CT" Namespace="MasterPages" Assembly="Common"%>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<center>

<div id="header-page">
   
   
   
</div>

<div id="page">

<div id="left-page">


</div>

<div id="content-page" preventDefaultButton="ctl02_btnSearch">
<CT:ContentRegion runat="server" id="CT_Top" width="744"/>
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>

  <div class="bc"><a class="bdcrmblnk" href="/">Home</a> <img src="/includes/theme/images/icon_stepmenu.gif" /> <a class='bdcrmblnk' href='/members/'>My Account</a><img src="/includes/theme/images/icon_stepmenu.gif" /><b> Deactivate Account</b>
     
    </div>
<div class="form">
 <div class="title">Deactivate Account</div>
  <div class="border" style="padding:10px">

      <div id="dvDeactive" runat=server>
       <div>
          Please note, once you deactivate your account, you will no longer be able to log in your account, add items to shopping cart, check out, receive our promotion offers, coupons, discounts and many other benefits from The Nail Superstore via email.
       </div>
       <div class="lnpadTop5">
           <span id="labeltxtCaptcha" class="blkbold">Type the code shown</span> <span class="red">*</span>
           <span> <asp:TextBox id="txtCaptcha" runat="server" maxlength="142" size="20" class="sbox" style="width:200px;" AutoCompleteType="Disabled" autocomplete="off" /></span>
       </div>
       <div class="deactive">
        <asp:Literal id="ltCapcha" runat="server"></asp:Literal>
    	    <asp:RequiredFieldValidator ValidationGroup="forgot" runat="server" ID="reqTxtCaptcha" ControlToValidate="txtCaptcha" Display="Dynamic" ErrorMessage="The code shown required." CssClass="error" />
	         
	        <p><img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" /></p>
	        <p><asp:Button id="btnSubmit" runat="server" CssClass="btn150" Text="Submit" />         </p>
	         
       </div>  
       </div>
        <asp:Literal id="ltlMsg" runat="server" ></asp:Literal>
  </div>
 
     
</div>

<CT:ContentRegion runat="server" id="CT_Bottom" width="744"/>
</div>

</div>

<div><CT:NavigationRegion runat="server" id="NavigationRegion"/></div>

</center>
</asp:Content>