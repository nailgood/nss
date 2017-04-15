<%@ Page Language="VB" AutoEventWireup="false" CodeFile="forgotpassword.aspx.vb" Inherits="SSPForgotpassword" MasterPageFile="~/includes/masterpage/interior.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="form-horizontal col-sm-6" role="form">
        <div class="panel-content">
            <h1 class="formtitle">
                Forgot your password?</h1>
            <asp:Panel ID="pnlSearch" DefaultButton="btnRetrieve" runat="server">
                <div class="content">
                    <p>
                        Please enter your email address below. We will send you an email message containing
                        your log in information.</p>
                    <div class="form-group">
                        <div class="col-sm-12 txt-required">
                            <asp:TextBox runat="server" ID="txtEmail" type="text" CssClass="form-control" placeholder="Your Email"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="reqTxtEmail" ControlToValidate="txtEmail"
                                ValidationGroup="forgot" Display="Dynamic" ErrorMessage="Email is required."
                                CssClass="text-danger"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ValidationGroup="forgot" runat="server" ID="regTxtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is invalid." CssClass="text-danger" />
                        </div>   
                    </div>
                    <div class="form-group">
                        <div class="col-sm-12 txt-required">
                            <asp:TextBox runat="server" ID="txtCaptcha" type="text" CssClass="form-control" placeholder="Type the code shown"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="forgot" runat="server" ID="reqTxtCaptcha"
                                ControlToValidate="txtCaptcha" Display="Dynamic" ErrorMessage="The code shown required."
                                CssClass="text-danger" />
                            <asp:Literal ID="ltCapcha" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="capcha">
                        <img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" />
                    </div>
                </div>
                <div class="content text-center pn-btn bd-top">
                    <asp:Button runat="server" ID="btnRetrieve" data-btn="submit" Text="Submit" CssClass="btn btn-submit" ValidationGroup="forgot" />
                </div>
            </asp:Panel>
            <asp:Panel ID="PChooseUser" runat="server" DefaultButton="btnRetrieve2" Visible="false">
                <div class="content">
                    <p>
                        Choose your Username below and we will send you an email message containing your
                        login information.</p>
                    <div class="form-group">
                        <label for="ddlUser">
                            Choose User</label>
                        <div class="nf-dropdown">
                            <asp:DropDownList runat="server" ID="ddlUser" CssClass="form-control" AutoPostBack="false" />
                        </div>
                    </div>
                </div>
                <div class="content text-center pn-btn bd-top">
                    <asp:Button ID="btnRetrieve2" runat="server" CssClass="btn150" Text="Submit" />
                </div>
            </asp:Panel>
            <asp:Literal ID="ltlResult" runat="server" />
        </div>
    </div>
    <script type="text/javascript">

	    if (window.addEventListener) {
	        window.addEventListener('load', SetFocusOnUsername, false);
	    } else if (window.attachEvent) {
	        window.attachEvent('onload', SetFocusOnUsername);
	    }

	    function SetFocusOnUsername() {
	        if (document.getElementById('txtEmail') != undefined)
	            document.getElementById('txtEmail').focus();
	    }

	</script>

</asp:Content>