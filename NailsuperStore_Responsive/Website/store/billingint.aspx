<%@ Page Language="VB" AutoEventWireup="false" CodeFile="billingint.aspx.vb" Inherits="billingint"
    MasterPageFile="~/includes/masterpage/checkout.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
     <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <div id="billing-page">
        <div class="header">
            <div class="title">
                Billing & Shipping Information - International
            </div>
            <div class="note">
                Note: we can only ship your order to the same address as your billing address
            </div>
           <%-- <div class="link">
                <asp:LinkButton ID="lbtnUS" runat="server" Text="Click here if your billing address is in the US"></asp:LinkButton>
            </div>--%>
        </div>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="form">
                    <div class="panel-content" id="secBilling" clientidmode="Static" runat="server">
                        <div class="title">Billing Address - International</div>
                        <div class="content" style="border-bottom:1px solid #dadada;margin-top:0px;">
                             <div class="col-sm-6 col-left-pd10">
                                <label class="required" for="drpBillingCountry">
                                    Country</label>
                                <div class="nf-dropdown">
                                    <asp:DropDownList ID="drpBillingCountry" ClientIDMode="Static" Enabled="true" CssClass="form-control"
                                        runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rqBillingCountry" runat="server" ControlToValidate="drpBillingCountry"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Country is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="content">
                            <div class="col-sm-6 col-left-pd10">
                                <div style="color: #333333; font-size: 18px;font-weight: 600; padding:0px 20px 7px 0px;">Contact Information</div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingFirstName">
                                        First Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtBillingFirstName" placeholder="First Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingFirstName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtBillingFirstName" Display="Dynamic" ErrorMessage="First name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingLastName">
                                        Last Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtBillingLastName" placeholder="Last Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingLastName" runat="server" ControlToValidate="txtBillingLastName"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Last name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtEmail">
                                        Email Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtEmail" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cusvEmail" ControlToValidate="txtEmail" Display="Dynamic"
                                        ValidationGroup="valRegister" ErrorMessage="Email is invalid" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate"
                                        runat="server" />
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingPhone">Billing, Home, or Work Phone</label>
                                    <asp:TextBox ID="txtBillingPhone" runat="server" ClientIDMode="Static" type="tel"
                                        CssClass="form-control" placeholder="Billing, Home, or Work Phone"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtBillingPhone" runat="server" ControlToValidate="txtBillingPhone"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cusvPhoneBillingInt" runat="server" ClientValidationFunction="ClientCheckPhoneInternational"
                                        OnServerValidate="ServerCheckPhoneInternational" CssClass="text-danger" ErrorMessage="Phone number is invalid"
                                        ControlToValidate="txtBillingPhone" Display="Dynamic" ValidateEmptyText="false"
                                        ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                <%--<div class="form-group">
                                    
                                    <label for="txtBillingDaytimePhone">
                                        Daytime / Contact Phone
                                    </label>
                                    <asp:TextBox ID="txtBillingDaytimePhone" ClientIDMode="Static" runat="server" type="tel"
                                        CssClass="form-control" placeholder="Daytime / Contact Phone "></asp:TextBox>
                                    <asp:CustomValidator ID="cusvDayPhoneBillingInt" runat="server" ClientValidationFunction="ClientCheckPhoneInternational"
                                        OnServerValidate="ServerCheckDayPhoneInternational" CssClass="text-danger" ErrorMessage="Phone number is invalid"
                                        ControlToValidate="txtBillingDaytimePhone" Display="Dynamic" ValidateEmptyText="false"
                                        ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                <div class="form-group">
                                    <label for="txtBillingFax">
                                        Fax</label>
                                    <asp:TextBox ID="txtBillingFax" ClientIDMode="Static" runat="server" type="tel" CssClass="form-control"
                                        placeholder="Fax"></asp:TextBox>
                                    <asp:CustomValidator ID="cusvFaxBillingInt" runat="server" ClientValidationFunction="ClientCheckFaxInternational"
                                        OnServerValidate="ServerCheckFaxInternational" CssClass="text-danger" ErrorMessage="Fax number is invalid"
                                        ControlToValidate="txtBillingFax" Display="Dynamic" ValidateEmptyText="false"
                                        ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>--%>
                                
                            </div>
                            <%--Column 2 --%>
                            <div class="col-sm-6 col-right-pd10">
                                <div style="color: #333333; font-size: 18px;font-weight: 600; padding:0px 20px 7px 0px;">Billing Address</div>
                                <div class="form-group">
                                    <label for="txtBillingCompany">
                                        Company/Salon Name</label>
                                    <asp:TextBox ID="txtBillingCompany" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Company/Salon Name"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingAddress1">
                                        Street Address</label>
                                    <asp:TextBox ID="txtBillingAddress1" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Street Address"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtBillingAddress1" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtBillingAddress1" Display="Dynamic" ErrorMessage="Address 1 is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="txtBillingAddress2">
                                        Address 2</label>
                                    <asp:TextBox ID="txtBillingAddress2" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Address 2"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingCity">
                                        City</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtBillingCity" placeholder="City" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingCity" runat="server" ControlToValidate="txtBillingCity"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="City is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-row">
                                    
                                    <div class=" form-group col-sm-6 col-left" runat="server" id="dvBillingRegion">
                                        <label class="required" for="txtBillingRegion">
                                            Province/Region</label>
                                        <asp:TextBox ID="txtBillingRegion" runat="server" CssClass="form-control" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="rqtxtBillingRegion" runat="server" ControlToValidate="txtBillingRegion"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Region is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class=" form-group col-sm-6 col-right">
                                        <label for="txtBillingZip">
                                            Postal Code <span class="require"></span>
                                        </label>
                                        <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                            ID="txtBillingZip" placeholder="Postal Code" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div id="divProfessionalStatus" class="form-group" runat="server" style="border-top:1px solid #dadada;margin-top:0px;padding-top:10px">
                        <div class="checkbox f-chk-required">
                            <label for="ctl00_cphContent_chkProfessionalStatus">
                                <asp:CheckBox runat="server" ID="chkProfessionalStatus" />
                                <span class="checkbox-icon"></span>
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
                    </div>--%>
                    <div class="panel-content text-right">
                        <asp:LinkButton ID="lbtnBack" CssClass="back-link" ClientIDMode="Static" runat="server">Back</asp:LinkButton>
                        <asp:Button ID="btnContinue" ClientIDMode="Static" runat="server" CausesValidation="true"
                            ValidationGroup="valRegister" Text="Continue" data-btn="submit" CssClass="btn btn-submit" />
                    </div>
                </div>
                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="up">
                    <ProgressTemplate>
                        <center>
                            <div class="ajaxwaiting">
                                Please wait...<br />
                                <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/loader.gif" style="border: 0px" alt="" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
     function CheckEmailValidate(source, arguments) {

        var str = ''; //1: Email is not valid, 2: Email is exists, 3 User name is exists
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (re.test(arguments.Value) == true) {
            arguments.IsValid = true;
        }
        else {
            arguments.IsValid = false;
        }
    }

//    function CheckProfessionalStatus(sender, args) {
//        if (document.getElementById("%=chkProfessionalStatus.ClientID %").checked == true) {
//            args.IsValid = true;
//        } else {
//            args.IsValid = false;
//        }
    //    }

        $(window).load(function () {
            ShowPageLoadError();
        });
    $("#drpBillingCountry").change(function () {
        fnSelectCountryCode(this.id);
    });



    </script>

    <asp:Literal ID="ltrLoadMsg" runat="server"></asp:Literal>
</asp:Content>
