<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emailme.aspx.vb" Inherits="EmailMe" %>

<html>
<head>
    <title>Notify me when in stock - <%=SiteName%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href='//fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' />
    <%--<link href="/includes/theme/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/default.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/videopopup.css" rel="stylesheet" type="text/css" /> Edit css.xml--%>
     <asp:Literal ID="litCSS" runat="server"></asp:Literal>
</head>

<body id="bodyVideo">
<form method="post" id="main" runat="server">
    <h1>Remind Me When Available</h1>
    <asp:Panel ID="pnlResult" runat="Server">
        <asp:Literal ID="litResult" runat="server"></asp:Literal>
    </asp:Panel>
    
    <asp:Panel ID="pnlFields" runat="Server">
        <div class="panel-content"><p>Please enter your email address below and we will notify you when the product: <strong><asp:Literal ID="ltlItemName" runat="server" /></strong> becomes available.</p></div>
        <div class="form-group">
            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="50" placeholder="Your Email Address" />
            <asp:RequiredFieldValidator ID="regEmail" runat="server" ValidationGroup="emailme" CssClass="text-danger"
                ControlToValidate="txtEmail" ErrorMessage="Email Address is required."
                Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regvEmail" runat="server" ValidationGroup="emailme" CssClass="text-danger"
                ErrorMessage="Email Address is invalid." Display="Dynamic" SetFocusOnError="true"
                ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="50" class="form-control" placeholder="Type the code shown" AutoCompleteType="Disabled" autocomplete="off" />
            <asp:RequiredFieldValidator ValidationGroup="emailme" runat="server" ID="reqTxtCaptcha" CssClass="text-danger"
                ControlToValidate="txtCaptcha" Display="Dynamic" ErrorMessage="The code shown required.<br>" />
            <asp:Literal ID="ltCapcha" runat="server"></asp:Literal>
            <img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" style="margin-top:10px" />
        </div>
        <div style="padding-top:10px;border-top:1px solid #cfcfcf;">
            <asp:Button ID="btnSend" runat="server" ValidationGroup="emailme" CssClass="btn btn-submit" Text="Submit" />
        </div>
    </asp:Panel>
                    

</form>
</body>
</html>
