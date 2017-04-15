<%@ Page Language="VB" AutoEventWireup="false" CodeFile="submission.aspx.vb" Inherits="members_submission"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="nailarttrend_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                <asp:Label id="lbTitle" runat="server">Nail Art Trends</asp:Label></h1>
            <div class="content">
                <div id="trlogin" runat="server" class="form-group">
                    <div class="col-sm-3">
                    </div>
                    <div class="col-sm-5">
                        <a style="text-decoration: underline; color: #3b76ba;" href="/members/login.aspx">Login to your account</a>
                    </div>
                </div>
                <div id="trline" runat="server" class="form-group">
                    <div class="col-sm-3">
                    </div>
                    <div class="col-sm-9">
                        <div style="width: 125px; float: left">
                            <hr />
                        </div>
                        <div style="float: left; padding: 10px">
                            Or</div>
                        <div style="width: 125px; float: left">
                            <hr />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span id="labeltxtName" runat="server">Name</span>
                    </div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" CssClass="form-control" placeholder="Name"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                             Display="Dynamic" ValidationGroup="valSubmission" ErrorMessage="Name is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span id="labeltxtEmailAddress" runat="server">Email</span>
                    </div>
                    <div class="col-sm-5 txt-required">
                        <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="50" CssClass="form-control" placeholder="username@hostname.com"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmailAddress"  CssClass="text-danger"
                            ErrorMessage="Email is required" ValidationGroup="valSubmission" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
                            CssClass="text-danger" ControlToValidate="txtEmailAddress" Display="Dynamic" ErrorMessage="Email is invalid."
                            OnServerValidate="ServerCheckValidEmail" ValidateEmptyText="True" ></asp:CustomValidator>
                    </div>
                </div>
                <div class="form-group <%=csshide %>">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span id="labeltxtDaytimePhone3" runat="server">Country</span>
                    </div>
                    <div class="n-dropdown col-sm-5">
                        <asp:DropDownList ID="drCountry" runat="server" AutoPostBack="false" CssClass="form-control" />
                        <span class="drp-required"></span>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rqdrpBillingCountry" runat="server" ControlToValidate="drCountry" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="Country is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>                        
                    </div>
                </div>
                <div class="form-group <%=csshide %>">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span id="labeltxtYourArt" runat="server">Name of Your Nail Art Design</span>
                    </div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtYourArt" runat="server" MaxLength="100" CssClass="form-control" placeholder="Name of Your Nail Art Design"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvYourArt" ControlToValidate="txtYourArt"
                             Display="Dynamic" ValidationGroup="valSubmission" ErrorMessage="Name of Your Nail Art Design is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span>Your Salon Name</span>
                    </div>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtYourSalon" runat="server" MaxLength="30" CssClass="form-control" placeholder="Your Salon Name"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span><asp:Label id="lbIntructions" runat="server">Additional Info, Instructions</asp:Label></span>
                    </div>
                    <div class="col-sm-5 txt-required">
                        <asp:TextBox ID="txtIntructions" runat="server" rows="3" TextMode="Multiline" CssClass="form-control"></asp:TextBox>
                    </div>
                      <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfIntructions" ControlToValidate="txtIntructions"
                             Display="Dynamic" ValidationGroup="valSubmission" ErrorMessage="Additional Info, Instructions is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Attach picture<br />
                        <span class="smaller">minimum 500px</span>
                    </div>
                     <div class="col-sm-3 hidden-sm hidden-md hidden-lg text-left">
                        Attach picture<br />
                        <span class="smaller">minimum 500px</span>
                    </div>
                    <div class="col-sm-5">
                        <CC:FileUpload runat="server" ID="fuImage" Folder="/upload/nail-art-trends" DisplayImage="true"
                            ImageDisplayFolder="/upload/nail-art-trends" />
                        <CC:FileUpload runat="server" ID="fuImage1" Folder="/upload/nail-art-trends" DisplayImage="true"
                            ImageDisplayFolder="/upload/nail-art-trends" />
                        <CC:FileUpload runat="server" ID="fuImage2" Folder="/upload/nail-art-trends" DisplayImage="true"
                            ImageDisplayFolder="/upload/nail-art-trends" />
                        <span class="smaller">Only format file image</span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        <span id="labeltxtCaptcha" runat="server">Type the code shown</span>
                    </div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="142" AutoCompleteType="Disabled"  placeholder="Type the code shown"
                            autocomplete="off" CssClass="form-control" />                        
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvCaptcha" ControlToValidate="txtCaptcha"
                             Display="Dynamic" ValidationGroup="valSubmission" ErrorMessage="The code shown required" CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:Literal ID="ltCapcha" runat="server" ></asp:Literal>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 text-right">
                    </div>
                    <div class="col-sm-5">
                        <img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" />
                    </div>
                    <div class="col-sm-4 rules <%=cssShow %>" ><a href="<%=linkRules %>" target="_blank">See the Official Rules</a></div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 text-right">
                    </div>
                    <div class="col-sm-8 content">
                         <asp:Button runat="server" ID="btnSave" data-btn="submit" ValidationGroup="valSubmission" Text="Submit" CssClass="btn btn-submit" /><br />
                        <asp:Label ID="lbMsg" runat="server" Style="color: Green;" Visible="false">Your file has been submitted and is pending review. Thank you.</asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CheckValidEmail(sender, args) {
            var email = document.getElementById('txtEmailAddress').value;
            if (email != '') {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (!re.test(email)) {
                    args.IsValid = false;
                    document.getElementById('cusEmail').innerHTML = 'Email is invalid';
                    return;
                }
            }
        }
    </script>
</asp:Content>
