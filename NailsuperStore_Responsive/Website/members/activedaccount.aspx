<%@ Page Language="vb" AutoEventWireup="false" CodeFile="ActivedAccount.aspx.vb" Inherits="Members_ActivedAccount" MasterPageFile="~/includes/masterpage/interior.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="DivMsg" class="active-acc" runat="server">
        <asp:Literal ID="ltlMsg" runat="server" />
    </div>
    <div id="DivActiveCode" runat="server" class="form-horizontal col-sm-6">
        <div class="panel-content">
            <h1 class="formtitle" style="margin-bottom:0px">Request account activation code</h1>
            <asp:Panel ID="pnlSearch" DefaultButton="btnRetrieve" runat="server">
                <div class="content">
                    <p>
                        Enter your email address below and we will send you an email message containing
                        your account activation code.</p>
                    <div class="form-group">
                        <div class="col-sm-12 txt-required">
                            <asp:TextBox runat="server" ID="txtEmail" type="email" CssClass="form-control" placeholder="Your Email"></asp:TextBox>
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
                    <asp:Button runat="server" ID="btnRetrieve" data-btn="submit"
                        AlternateText="Active Account" ValidationGroup="active" Text="Submit" CssClass="btn btn-submit" />
                </div>
            </asp:Panel>
            <asp:Literal ID="ltlResult" runat="server" />
        </div>
    </div>
	<div>
        <asp:Literal ID="ltlCompleteMsg" runat="server" />
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