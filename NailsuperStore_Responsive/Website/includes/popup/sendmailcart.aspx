<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sendmailcart.aspx.vb" Inherits="sendmailcart" %>

<html>
<head>
    <title>Send cart item - <%=SiteName%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href='//fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' />
    <link href="/includes/theme/css/default.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/videopopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/includes/scripts/jquery-1.7.1.min.js"></script>
</head>

<body id="sendcart">
<form method="post" id="main" runat="server">
<h1>Email Your Shopping Cart</h1>
    <asp:Panel ID="pnlFields" runat="Server">
    
        <div class="panel-content"><p>Please enter your email address below and your cart content will be sent to you in a few minutes. Thank you !</p></div>
         <div class="form-group">
         <div class="col-sm-12 txt-required"> 
            <asp:TextBox ID="txtName" CssClass="form-control" runat="server" MaxLength="50" placeholder="Name" />
         </div>
         <div class="div-error">
                <asp:RequiredFieldValidator runat="server" ID="regName" ControlToValidate="txtName" Display="Dynamic" ValidationGroup="emailme" ErrorMessage="Name is required"
            CssClass="text-danger" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </div>
         </div>
        <div class="form-group">
        <div class="col-sm-12 txt-required">
            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="50" placeholder="Email" />
        </div>
        <div class="div-error">
             <asp:RequiredFieldValidator ID="regEmail" runat="server" ValidationGroup="emailme" CssClass="text-danger"
                ControlToValidate="txtEmail" ErrorMessage="Email is required."
                Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regvEmail" runat="server" ValidationGroup="emailme" CssClass="text-danger"
                ErrorMessage="Email is invalid." Display="Dynamic" SetFocusOnError="true"
                ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </div>    
           
        </div>
      <%--  <div class="form-group">
            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="50" class="form-control" placeholder="Type the code shown" AutoCompleteType="Disabled" autocomplete="off" />
            <asp:RequiredFieldValidator ValidationGroup="emailme" runat="server" ID="reqTxtCaptcha" CssClass="text-danger"
                ControlToValidate="txtCaptcha" Display="Dynamic" ErrorMessage="The code shown required.<br>" />
            <asp:Literal ID="ltCapcha" runat="server"></asp:Literal>
            <img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" style="margin-top:10px" />
        </div>--%>
        <div style="padding-top:5px">
            <asp:Button ID="btnSend" runat="server" ValidationGroup="emailme" CssClass="btn btn-submit" data-btn="submit" Text="Send" />
        </div>
    </asp:Panel>
<asp:literal id="ltmsg" runat="server"></asp:literal>
</form>
</body>
      <script type="text/javascript" src="/includes/scripts/layout.js" defer="defer"></script>
</html>
