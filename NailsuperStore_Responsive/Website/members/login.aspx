<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Members_Login" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="canonical" href="<%=Utility.ConfigData.GlobalRefererName %>/members/login.aspx" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="form-register" class="form col-sm-6">
        <div class="panel-content">
            <div class="title"><asp:Literal ID="Literal1" runat="server" Text="New Customer?"></asp:Literal></div>
            <div id="ct-right" class="content ct-register">
                <p>
                    Your Nail Superstore account is free, private, secure and convenient (See our Privacy
                    Policy). You don’t need a credit card to create an account. Payment isn’t required
                    until you make a purchase.
                </p>
                <ul>
                    <li>Save products in your shopping cart</li>
                    <li>Receive exclusive sales & promotions</li>
                    <li>Access your web order history</li>
                    <li>Track your order shipping status</li>
                </ul>
                <p id="pCreateAccount" class="textlink" runat="server" visible="false"><a href="/members/register.aspx">Create an account</a></p>
            </div>
            <div class="content text-center pn-btn bd-top">
                <asp:Button runat="server" ID="btnRegister" Text="Create Account" CssClass="btn btn-submit" />
            </div>
        </div>
    </div>
    <div id="form-login" class="form col-sm-6" role="form">
        <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin" CssClass="panel-content">
            <h1 class="formtitle">
                <asp:Literal ID="litTitle1" runat="server" Text="Sign in to your account"></asp:Literal>
            </h1>
            <div id="ct-left" class="content ct-login">
                <div class="form-group">
                    <asp:TextBox runat="server" ID="txtUsername" type="text" CssClass="form-control"
                        placeholder="Username or Email Address"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rqtxtUsername" ControlToValidate="txtUsername" 
                        ValidationGroup="valRegister" Display="Dynamic" ErrorMessage="* Username/Email is required."
                        CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:TextBox runat="server" ID="txtPassword" type="password" CssClass="form-control"
                        placeholder="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rqtxtPassword" ControlToValidate="txtPassword"
                        Display="Dynamic" ErrorMessage="* Password is required." ValidationGroup="valRegister"
                        CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
                <div class="lnk-forgot">
                    <a href="/members/forgotpassword.aspx">Forgot your password?</a><br />
                    <a href="/members/activedaccount.aspx">Request account activation code?</a>
                </div>  
            </div>
            <div class="content text-center pn-btn bd-top">
                <asp:Button runat="server" ID="btnLogin" data-btn="submit" OnClientClick="return IsCookieEnable();" Text="Login & Continue" CssClass="btn btn-submit" />
            </div>
        </asp:Panel>
    </div>
       
    <script src="/includes/scripts/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        function setHeightFormAccount() {
            if ($("#pCreateAccount").is(":visible")) {
                //$("#ct-left").height("260px")
                //$("#ct-right").height("260px")
                $("#ct-right").css("height", "auto");
                var hof = $("#ct-right").height() + 40;
                $("#ct-left").height(hof);
            }
        }
        $(document).ready(function () {
            setHeightFormAccount();
        });
        $(window).resize(function () {
            setHeightFormAccount();
        });
    </script>
</asp:Content>
