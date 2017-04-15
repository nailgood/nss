<%@ Page Language="vb" AutoEventWireup="false" Inherits="members_account" CodeFile="account.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="form-horizontal" role="form">
        <div class="panel-content">
            <div class="title">
                Edit My Account Details</div>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtEmail" type="" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtEmail" ControlToValidate="txtEmail"
                            ValidationGroup="valRegister" Display="Dynamic" ErrorMessage="Email is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvEmail" ControlToValidate="txtEmail" Display="Dynamic"
                            ValidationGroup="valRegister" ErrorMessage="Email is invalid" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate"
                            runat="server" />
                    </div>
                </div>
            </div>
            <div class="content-btn">
                <asp:Button runat="server" ID="btnSubmit1" Text="Submit" CssClass="btn btn-submit" ValidationGroup="valRegister" />
            </div>
            <div class="content">
                <div class="text small">
                    <p>If you would like to create a new password, please enter it below. Otherwise, please leave these fields blank.</p>
                    <p>Password must be between 8 and 20 characters.</p>
                </div>
                <%--<div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtOldPassword" columns="38" maxlength="100" type="password" CssClass="form-control"
                            placeholder="Old Password"></asp:TextBox>
                    </div>
                </div>--%>
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtNewPassword" columns="38" maxlength="100" type="password" CssClass="form-control"
                            placeholder="New Password"></asp:TextBox>
                    </div>
                    <div class="div-error">
                    <asp:RequiredFieldValidator runat="server" ID="rqtxtPassword" ControlToValidate="txtNewPassword"
                            ValidationGroup="valPassword" Display="Dynamic" ErrorMessage="Confirm password is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvPassword" runat="server" ControlToValidate="txtNewPassword"
                            CssClass="text-danger" OnServerValidate="ServerCheckPasswordValid" ClientValidationFunction="CheckPasswordValid"
                            Display="Dynamic" ValidationGroup="valPassword" ErrorMessage="Password must be between 8 and 20 characters.">
                        </asp:CustomValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-6 txt-required">
                        <asp:TextBox runat="server" ID="txtConfirmPassword" columns="38" maxlength="100" type="password" CssClass="form-control"
                            placeholder="Confirm New Password"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtConfirmPassword" ControlToValidate="txtConfirmpassword"
                            ValidationGroup="valPassword" Display="Dynamic" ErrorMessage="Confirm password is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvtxtPassword" runat="server" ControlToCompare="txtNewPassword"
                            ControlToValidate="txtConfirmPassword" Display="Dynamic" ValidationGroup="valPassword"
                            CssClass="text-danger" ErrorMessage="The passwords you entered do not match"
                            Operator="Equal" Type="String"></asp:CompareValidator>
                    </div>
                </div>
                
            </div>
            <div class="content-btn">
                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="btn btn-submit" ValidationGroup="valPassword" />
            </div>
        </div>
    </div>
    <script>
        function CheckEmailValidate(source, arguments) {
            var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            arguments.IsValid = re.test(arguments.Value);
        }
    </script>
</asp:Content>