<%@ Page Language="vb" AutoEventWireup="false" Inherits="members_register" CodeFile="register.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
    <meta property="og:image" content="<%=Utility.ConfigData.GlobalRefererName() %>/includes/theme/images/small-logo.png">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="register_form" class="form-horizontal" role="form" runat="server">
        <asp:Panel ID="panRegister" runat="server" DefaultButton="btnSubmit">
        <div class="panel-content">
            <h1 class="formtitle">
                Create an account
            </h1>
            <div class="content">
                <div class="form-group">
                    <div class="n-dropdown col-sm-6">
                        <asp:DropDownList ID="drpCountry" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                        <span class="drp-required"></span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtEmail" type="text" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtEmail" ControlToValidate="txtEmail"
                            ValidationGroup="valRegister" Display="Dynamic" ErrorMessage="Email is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvEmail" ControlToValidate="txtEmail" Display="Dynamic"
                            ValidationGroup="valRegister" ErrorMessage="" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate"
                            runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtPassword" type="password" CssClass="form-control"
                            placeholder="Password"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtPassword" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Password is required" CssClass="text-danger" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvPassword" runat="server" ControlToValidate="txtPassword"
                            CssClass="text-danger" OnServerValidate="ServerCheckPasswordValid" ClientValidationFunction="CheckPasswordValid"
                            Display="Dynamic" ValidationGroup="valRegister" ErrorMessage="Password must be between 8 and 20 characters.">
                        </asp:CustomValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtConfirmpassword" type="password" CssClass="form-control"
                            placeholder="Confirm Password"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtConfirmPassword" ControlToValidate="txtConfirmpassword"
                            ValidationGroup="valRegister" Display="Dynamic" ErrorMessage="Confirm password is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvtxtPassword" runat="server" ControlToCompare="txtPassword"
                            ControlToValidate="txtConfirmPassword" Display="Dynamic" ValidationGroup="valRegister"
                            CssClass="text-danger" ErrorMessage="The passwords you entered do not match"
                            Operator="Equal" Type="String"></asp:CompareValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="checkbox f-chk-required">
                        <label for="chkProfessionalStatus">
                            <asp:CheckBox runat="server" ID="chkProfessionalStatus" />
                            <i class="fa fa-check checkbox-font" ></i>
                            I certify that all of the purchases are for Professional
                            Use Only. Any false statement will void all my rights for return or exchange or
                            claims or product warranty. I accept full responsibilities including misuse of products,
                            accident with no fault against The Nail Superstore.
                        </label>
                    </div>
                    <div class="div-error">
                        <asp:CustomValidator id="cusvProfessionalStatus" CssClass="text-danger"  
                        Display="Dynamic" ValidationGroup="valRegister"
                        ErrorMessage="You must agree to these terms and conditions before register" 
                        ClientValidationFunction="CheckProfessionalStatus"  runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <%--<div class="panel-content">
            <div class="title">
                Refer A Friend
            </div>
            <div class="content">
                <div class="text">
                    You will get 50 pts from Refer Friends program. Read <a href="/services/refer-friend-program.aspx"
                        style="text-decoration: underline; color: #3b76ba; padding-left: 5px;">Refer Friends
                        policy </a>
                </div>
                <div class="form-group">
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="txtReferCode" type="text" CssClass="form-control"
                            placeholder="Referral code"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="panel-content reglast">
            <div class="col-sm-9">
                By creating an account, you agree to NailSuperstore.com's <a href="/services/register-privacy-notice.aspx"
                    style="text-decoration: underline; color: #3b76ba; padding-left: 4px;">Privacy Notice</a></div>
            <div class="col-sm-3 text-right">
                <asp:Button runat="server" ID="btnSubmit" data-btn="submit" Text="Submit" CssClass="btn btn-submit" ValidationGroup="valRegister" />
            </div>
        </div>
        </asp:Panel>
    </div>

    <script type="text/javascript">

        function CheckEmailValidate(source, arguments) {

            var str = ''; //1: Email is not valid, 2: Email is exists, 3 User name is exists

            $.ajax({

                contentType: "application/json; charset=utf-8",
                url: "/members/register_check.ashx?act=email&key=" + arguments.Value,
                dataType: "json",
                async: false,
                success: function (result) {
                    str = result;
                }
            });

            if (str == "1") {

                arguments.IsValid = false;
                source.textContent = 'Email is invalid.';
            }
            else if (str == "2") {
                arguments.IsValid = false;
                source.textContent = "The email is already in use!";
            }
            else if (str == "3") {
                arguments.IsValid = false;
                source.textContent = "The username is already in use!";
            }
            else if (str == "4") {
                arguments.IsValid = true;
            }
            else {
                arguments.IsValid = true;
            }
        }

        function is_int(value) {
            for (i = 0; i < value.length; i++) {
                if ((value.charAt(i) < '0') || (value.charAt(i) > '9')) return false
            }
            return true;
        }


        function CheckPasswordValid(sender, args) {

            var str = '';

            $.ajax({

                contentType: "application/json; charset=utf-8",
                url: "/members/register_check.ashx?act=pass&key=" + args.Value,
                dataType: "json",
                async: false,
                success: function (result) {
                    str = result;
                }
            });

            if (str == "1") {
                args.IsValid = true;

            }
            else {
                args.IsValid = false;
            }

        }
   
    </script> 
    <asp:Panel id="pnScript"  runat="server">
    <script type="text/javascript">
        function CheckProfessionalStatus(sender, args) {
            if (document.getElementById("<%=chkProfessionalStatus.ClientID %>").checked == true) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }
    </script>
</asp:Panel>
<asp:Literal ID="litJS" runat="server"></asp:Literal>  
</asp:Content>
