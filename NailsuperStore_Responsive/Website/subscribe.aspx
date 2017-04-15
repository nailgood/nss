<%@ Page Language="vb" AutoEventWireup="false" Inherits="subscribe" CodeFile="subscribe.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="dContent" class="form-horizontal panel-content">
        <h1 class="formtitle">
            Newsletter Registration</h1>
        <div class="content">
            <div id="divMain" runat="server">
             <asp:Panel ID="panSubscribe" runat="server" DefaultButton="btnSignup">
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Email</div>
                    <div class="col-sm-5 txt-required">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control"
                            placeholder="username@hostname.com"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmail"
                            CssClass="text-danger" ErrorMessage="Email is required" ValidationGroup="valNewsletter"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
                            CssClass="text-danger" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is invalid."
                            OnServerValidate="ServerCheckValidEmail" ValidateEmptyText="True" ValidationGroup="valNewsletter"></asp:CustomValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Name</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" CssClass="form-control" placeholder="Name"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                            Display="Dynamic" ValidationGroup="valNewsletter" ErrorMessage="Name is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        E-mail format
                    </div>
                    <div class="col-sm-5">
                        <div id="group-radio">
                            <asp:RadioButtonList ID="radEmailFormat" runat="server" RepeatDirection="Horizontal"
                                CssClass="radio-node">
                                <asp:ListItem Value="1"><i class="ico-radio"></i>HTML</asp:ListItem>
                                <asp:ListItem Value="0"><i class="ico-radio"></i>Plain Text</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        E-mail lists
                    </div>
                    <div class="col-sm-5">
                        <asp:Literal ID="ltrEmailList" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 text-right">
                    </div>
                    <div class="col-sm-8 content">
                        <asp:Button runat="server" ID="btnSignup" data-btn="submit" Text="Sign Up Now!" CssClass="btn btn-submit" ValidationGroup="valNewsletter"
                            CausesValidation="true" />
                        <input class="btn btn-cancel" type="button" value="Cancel" onclick="document.location.href='/'" />
                    </div>
                </div>
            </asp:Panel>
            </div>
            <div id="divConfirm" runat="server" visible="false">
                <p>
                    The subscription for e-mail <b>
                        <asp:Literal runat="Server" ID="ltlEmail" /></b> has been successfully processed.</p>
                <p>
                    Thank you for joining the newsletter. You will be hearing from us shortly!</p>
            </div>
        </div>
    </div>
    <input id="hidList" type="hidden" value="" runat="server" />
    <script type="text/javascript">
        function CheckValidEmail(sender, args) {
            var email = document.getElementById('txtEmail').value;
            if (email != '') {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (!re.test(email)) {
                    args.IsValid = false;
                    document.getElementById('cusEmail').innerHTML = 'Email Address is invalid';
                    return;
                }
            }
        }
    </script>
</asp:Content>
