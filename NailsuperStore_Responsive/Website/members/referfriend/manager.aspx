<%@ Page Language="VB" AutoEventWireup="false" CodeFile="manager.aspx.vb" Inherits="members_referfriend_manager" MasterPageFile="~/includes/masterpage/interior.master" %>
<%@ Register src="~/controls/layout/barstatus.ascx" tagname="barstatus" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<center>
<div id="refermanager" preventDefaultButton="ctl02_btnSearch">
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>
<ul class="title">
    <li><div>Refer-A-Friend Referral Manager</div> </li>
</ul>	
<div class="intro">
   <div class="head bold">
    Share our site and earn $5 per referral!
   </div>
    <div class="content">
    Our Refer-A-Friend Program is a great way to share nailsuperstore.com with family and friends! Each new customer you refer will get a discount on their first order, and you can earn $5 worth of  big bucks reward points for use towards future purchases on our site! <a href="/services/refer-friend-program.aspx"> more</a>
    
    </div>
</div>
<ul class="title">
    <li><div>Referral Code</div> </li>
</ul>
<div class="code">
    <div class="ucode">
        Your Unique Referral Code: <strong><asp:Literal id="ltrReferCode" Runat="server"></asp:Literal> </strong>
    </div>
    <div class="point">
        Total Referral Reward Points: <strong><asp:Literal id="ltrReferPoint" Runat="server"></asp:Literal> points</strong>
    </div>
</div>
<ul class="title">
    <li><div>Get Rewards by Inviting Your Friends</div> </li>
</ul>
<uc2:barstatus ID="barstatus1" runat="server" />
<ul class="title" id="ulHistory" runat="server">
    <li><div>Your Referral History</div> </li>
</ul>
   
<asp:Repeater ID="rptReferHistory" runat="server">
    <HeaderTemplate>
        <div id="history">
            <div class="header-row hide-sm">
                <div class="header h-group-name" style="width:15%">Date</div>
                <div class="header h-group-name">Email</div>
                <div class="header h-group-name" style="width:25%">Status</div>      
            </div>
    </HeaderTemplate>
    <ItemTemplate>
       <div class="header-row">
        <div class="group-name text-right"> <%#DataBinder.Eval(Container.DataItem, "CreatedDate", "{0:MM/dd/yyyy}")%>  </div>
        <div class="group-name text-left"><asp:Literal id="ltrEmail" runat="server" ></asp:Literal></div>
        <div class="group-name text-left"><asp:Literal id="ltrStatus" runat="server" ></asp:Literal>
        
        </div>         
    </div>
    </ItemTemplate>
   
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>


</div>
</center>
</asp:Content>