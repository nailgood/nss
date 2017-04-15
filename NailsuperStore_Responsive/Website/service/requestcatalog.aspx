<%@ Page Language="VB" AutoEventWireup="false" CodeFile="requestcatalog.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" Inherits="service_requestcatalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Request Catalog</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Country</div>
                    <div class="n-dropdown col-sm-5">
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" CssClass="form-control" />
                        <span class="drp-required"></span>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rqdrpBillingCountry" runat="server" ControlToValidate="ddlCountry"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Country is required" ValidationGroup="valCatalog"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        First Name</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="First Name"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvFName" ControlToValidate="txtFirstName"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="First Name is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Last Name</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Last Name"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvLName" ControlToValidate="txtLastName"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="Last Name is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <%--<div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Company</div>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtCompany" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Company"></asp:TextBox>
                    </div>
                </div>--%>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Email Address</div>
                    <div class="col-sm-5 txt-required">
                        <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="50" CssClass="form-control"
                            placeholder="username@hostname.com"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rfEmailAddress" runat="server" ControlToValidate="txtEmailAddress"
                            CssClass="text-danger" ErrorMessage="Email Address is required" ValidationGroup="valCatalog"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
                            CssClass="text-danger" ControlToValidate="txtEmailAddress" Display="Dynamic"
                            ErrorMessage="Email Address is invalid." OnServerValidate="ServerCheckValidEmail"
                            ValidateEmptyText="True" ValidationGroup="valCatalog"></asp:CustomValidator>
                    </div>
                </div>
                <div id="BillingPhone" runat="server" class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Phone</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtBillingPhone" runat="server" MaxLength="30" CssClass="form-control"
                            type="tel" placeholder="Phone"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfPhone" ControlToValidate="txtBillingPhone"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="Phone is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <CC:InternationalPhoneValidator ID="regvPhone" runat="server" ControlToValidate="txtBillingPhone"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is invalid"
                            FrontValidator="False" ValidationGroup="valCatalog"></CC:InternationalPhoneValidator>
                        <asp:CustomValidator ID="cusvPhoneInt" runat="server" ClientValidationFunction="CheckPhoneInt"
                            CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic" ErrorMessage="Phone must be at least 10 digit."
                            OnServerValidate="ServerCheckPhoneInt" ValidateEmptyText="True" ValidationGroup="valCatalog"></asp:CustomValidator>
                        <asp:CustomValidator ID="cusvPhoneUS" runat="server" ClientValidationFunction="CheckPhoneUS"
                            CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic" ErrorMessage="Phone is required."
                            OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="True" ValidationGroup="valCatalog"></asp:CustomValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Street Address</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtAddress1" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Street Address"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfAddress1" ControlToValidate="txtAddress1"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="Street Address is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>                    
                </div>
                <%--<div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right"></div>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtAddress2" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Street Address 2"></asp:TextBox>
                    </div>
                </div>--%>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        City</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="City"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="City is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="trRegion" runat="server" class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Province/Region</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtState" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Province/Region"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfRegion" ControlToValidate="txtState"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="Province/Region is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="trState" runat="server" class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        State</div>
                    <div class="n-dropdown col-sm-5">
                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="false" CssClass="form-control" />
                        <span class="drp-required"></span>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator ID="rqdrpState" runat="server" ControlToValidate="ddlState"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="State is required" ValidationGroup="valCatalog"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="trZipCode" runat="server" class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Zip Code</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtZip" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="Zip Code"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfZipCode" ControlToValidate="txtZip"
                            Display="Dynamic" ValidationGroup="valCatalog" ErrorMessage="Zip Code is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 text-right">
                    </div>
                    <div class="col-sm-8 content">
                        <asp:Button runat="server" ID="btnSubmit" data-btn="submit" Text="Submit" CssClass="btn btn-submit" ValidationGroup="valCatalog" /><br />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="phConfirm" Visible="false">
        <div>Thank you, your request has been received.</div>
        <div style="padding-top: 15px;">Don't miss our <a href="https://www.nailsuperstore.com/deals-center" style="color: #3b76ba;">promotions</a>!</div>
    </asp:PlaceHolder>
           <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
<script type="text/javascript">
    function pageLoad() {
        if ($("#ddlCountry").val() == "US") {
            // $("#txtBillingPhone").mask("?999-999-9999 9999");
            fnFormatPhoneUS('#txtBillingPhone', '');
        }
    }
    $('#txtBillingPhone').keyup(function (e) {
        if ($("#ddlCountry").val() == "US") {
            fnFormatPhoneUS(this, e, this.id);
        }

    });
    function CheckValidEmail(sender, args) {
        var email = document.getElementById('txtEmailAddress').value;
        if (email != '') {
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!re.test(email)) {
                args.IsValid = false;
                document.getElementById('cusEmail').innerHTML = 'Email Address is invalid';
                return;
            }
        }
    }
    function CheckPhoneInt(sender, args) {
        if (document.getElementById("regvPhone").style.display == 'none') {
            var phone = document.getElementById('txtBillingPhone').value;
            if (phone != '') {
                phone = phone.replace(/[^0-9]/g, '');
                if (is_int(phone)) {
                    if (phone.length < 10) {
                        args.IsValid = false;
                        document.getElementById('cusvPhoneInt').innerHTML = 'Phone must be at least 10 digit.';
                        return;
                    }
                }
            }
        }
    }
    function CheckPhoneUS(sender, args) {
        var phone = fnPhoneUS(document.getElementById('txtBillingPhone').value, 'BillingPhone');
        if (phone == '') {
            return;
        }
        var phone1;
        var phone2;
        var phone3;
        var arr;
        try {
            if (phone != '') {
                arr = phone.split(' ');
                if (arr[0] != '') {
                    var lstPhone = arr[0].split('-');
                    phone1 = lstPhone[0];
                    phone2 = lstPhone[1];
                    phone3 = lstPhone[2];
                }
            }
            if (phone1.length != 3 || phone2.length != 3 || phone3.length != 4) {
                args.IsValid = false;
                document.getElementById('cusvPhoneUS').innerHTML = 'Phone number is invalid';
                return;
            }
            if (!is_int(phone1) || !is_int(phone2) || !is_int(phone3)) {
                args.IsValid = false;
            }
            else {
                if (arr.length > 1) {
                    //args.IsValid = true
                    //check Ext Phone
                    var ext = arr[1];
                    if (!CheckPhoneExt(ext)) {
                        args.IsValid = false;
                    }
                    else
                        args.IsValid = true;
                }
            }
            if (!args.IsValid) {
                document.getElementById('cusvPhoneUS').innerHTML = 'Phone number is invalid';
            }
        }
        catch (e) {
            args.IsValid = false;
            document.getElementById('cusvPhoneUS').innerHTML = 'Phone number is invalid';
            return;
        }

    }
    function is_int(value) {
        for (i = 0; i < value.length; i++) {
            if ((value.charAt(i) < '0') || (value.charAt(i) > '9')) return false
        }
        return true;
    }
    function CheckPhoneExt(value) {

        if (value == '') {
            return true;
        }
        var phone = value;
        if (phone.length > 4) {
            return false;
        }
        if (!is_int(phone)) {
            return false;
        }
        return true;
    }
</script>
</asp:Content>
