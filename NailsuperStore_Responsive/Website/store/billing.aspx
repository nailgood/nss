<%@ Page Language="VB" AutoEventWireup="false" CodeFile="billing.aspx.vb" MasterPageFile="~/includes/masterpage/checkout.master"
    Inherits="billing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
     <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <div id="billing-page">
        <div class="header">
            <div class="title">
                Billing & Shipping Information - USA</div>
          <%--  <div class="link">
                <asp:LinkButton ID="lbtnInternational" runat="server" Text="Click here if your billing address is International"></asp:LinkButton>
            </div>--%>
        </div>

        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" id="hidBillingZipCode" ClientIDMode="Static" />
                <asp:HiddenField runat="server" id="hidShippingZipCode" ClientIDMode="Static" />
                <div class="form">
                    <div class="panel-content" id="secBilling" clientidmode="Static" runat="server">
                        <div class="title">Billing Address - USA</div>
                        <div class="content" style="border-bottom:1px solid #dadada;margin-top:0px;">
                            <div class="col-sm-6 col-left-pd10">
                                <label class="required" for="drpBillingCountry">
                                    Country</label>
                                <div class="nf-dropdown">
                                    <asp:DropDownList ID="drpBillingCountry" ClientIDMode="Static" Enabled="true" CssClass="form-control"
                                        runat="server" AutoPostBack="false">
                                        <asp:ListItem Text="United States" Value="US" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
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
                                        ErrorMessage="Email is invalid" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate"
                                        runat="server" ValidationGroup="valRegister" />
                                </div>
                                <div id="BillingPhone" class="form-group">
                                    <label class="required" for="txtBillingPhone">
                                        Billing, Home, or Work Phone</label>
                                    <asp:TextBox ID="txtBillingPhone" runat="server" ClientIDMode="Static" type="tel" maxlength="17"
                                        CssClass="form-control" placeholder="Billing, Home, or Work Phone"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtBillingPhone" runat="server" ControlToValidate="txtBillingPhone"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cusvPhoneBillingUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                        CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic"
                                        ErrorMessage="Phone number is invalid." OnServerValidate="ServerCheckPhoneUS"
                                        ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                <%--<div class="form-group">
                                    <label for="txtBillingDaytimePhone">
                                        Daytime / Contact Phone
                                    </label>
                                    <asp:TextBox ID="txtBillingDaytimePhone" ClientIDMode="Static" runat="server" type="tel" maxlength="17"
                                        CssClass="form-control" placeholder="Daytime / Contact Phone "></asp:TextBox>
                                    <asp:CustomValidator ID="cusvDayPhoneBillingUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                        CssClass="text-danger" ControlToValidate="txtBillingDaytimePhone" Display="Dynamic"
                                        ErrorMessage="Daytime phone is invalid." OnServerValidate="ServerCheckPhoneUS"
                                        ValidateEmptyText="False" ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                
                                <div class="form-group" style="display: none;">
                                    <label for="txtBillingFax">
                                        Fax</label>
                                    <asp:TextBox ID="txtBillingFax" ClientIDMode="Static" runat="server" type="tel" CssClass="form-control" maxlength="17"
                                        placeholder="Fax"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ClientCheckPhoneUS" Enabled="false"
                                        CssClass="text-danger" ControlToValidate="txtBillingFax" Display="Dynamic" ErrorMessage="Fax is invalid."
                                        OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="False" ValidationGroup="valRegister"></asp:CustomValidator>
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
                                        ControlToValidate="txtBillingAddress1" Display="Dynamic" ErrorMessage="Street Address is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group" id="divBillingCity">
                                    <label class="required" for="txtBillingCity">
                                        City</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtBillingCity" placeholder="City" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingCity" runat="server" ControlToValidate="txtBillingCity"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="City is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-row">
                                    <div class=" form-group col-sm-6 col-left" id="divBillingState">
                                        <label class="required" for="drpBillingState">
                                            State</label>
                                        <div class="nf-dropdown">
                                            <asp:DropDownList ID="drpBillingState" ClientIDMode="Static" CssClass="form-control"
                                                runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqdrpBillingState" runat="server" ControlToValidate="drpBillingState"
                                                CssClass="text-danger" Display="Dynamic" ErrorMessage="State is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class=" form-group col-sm-6 col-right">
                                        <label id="lbzip" class="required" for="txtBillingZip">
                                            Zip Code <span class="require"></span>
                                        </label>
                                        <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                            ID="txtBillingZip" placeholder="Zip Code" MaxLength="5" />
                                            <asp:RequiredFieldValidator ID="rqtxtBillingZip" runat="server" ControlToValidate="txtBillingZip"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Zip Code is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator id="rqextxtBillingZip" Display="Dynamic" ControlToValidate="txtBillingZip" ValidationExpression="^[\s\S]{5,}$" runat="server" ErrorMessage="Zip Code must be at least 5 digit" CssClass="text-danger" ValidationGroup="valRegister"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="form-group" id="rowDiffAddress" runat="server" clientidmode="Static">
                                    <div class="checkbox address">
                                        <label for="chkDiffAddress">
                                            <asp:CheckBox runat="server" ClientIDMode="Static" ID="chkDiffAddress" Checked="true"
                                                AutoPostBack="true" />
                                            <i class="fa fa-check checkbox-font" ></i>Check here if your shipping address is different
                                            from the billing address
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel-content" id="secShipping" clientidmode="Static" runat="server">
                        <div class="title">
                            Shipping Address - USA</div>
                        <ul class="address-type" id="secShippingListAddress" runat="server" visible="false"
                            clientidmode="Static">
                            <li>
                                <label>
                                    Ship To</label>
                            </li>
                            <li>
                                <div class="nf-dropdown">
                                    <asp:DropDownList CssClass="form-control" ClientIDMode="Static" ID="ddlShipTo" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </li>
                        </ul>
                        <div id="divShippingCountry" class="content" style="border-bottom:1px solid #dadada;margin-top:0px;">
                                <div class="col-sm-6 col-left-pd10">
                                    <label class="required" for="drpShippingCountry">
                                            Country</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpShippingCountry" ClientIDMode="Static" Enabled="true" CssClass="form-control"
                                            runat="server" AutoPostBack="false">
                                            <asp:ListItem Text="United States" Value="US" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                            
                                    </div>
                                </div>
                            </div>
                        <div class="content">
                            <div class="col-sm-6 col-left-pd10">
                                <div class=" form-group">
                                    <label class="required" for="txtShippingFirstName">
                                        First Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtShippingFirstName" placeholder="First Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingFirstName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtShippingFirstName" Display="Dynamic" ErrorMessage="First name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtShippingLastName">
                                        Last Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtShippingLastName" placeholder="Last Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingLastName" runat="server" ControlToValidate="txtShippingLastName"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Last name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                
                                <div id="ShippingPhone" class="form-group">
                                    <label class="required" for="txtShippingPhone">
                                        Shipping, Home, or Work Phone</label>
                                    <asp:TextBox ID="txtShippingPhone" ClientIDMode="Static" runat="server" type="tel"
                                        CssClass="form-control" placeholder="Shipping, Home, or Work Phone"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtShippingPhone" runat="server" ControlToValidate="txtShippingPhone"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cusvShippingPhone" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                        CssClass="text-danger" ControlToValidate="txtShippingPhone" Display="Dynamic"
                                        ErrorMessage="Phone number is invalid." OnServerValidate="ServerCheckPhoneUS"
                                        ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                <div class="form-group">
                                    <label for="txtShippingFax">
                                        Fax</label>
                                    <asp:TextBox ID="txtShippingFax" ClientIDMode="Static" runat="server" type="tel"
                                        CssClass="form-control" placeholder="Fax"></asp:TextBox>
                                    <asp:CustomValidator ID="cusvShippingFax" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                        CssClass="text-danger" ControlToValidate="txtShippingFax" Display="Dynamic" ErrorMessage="Fax is invalid."
                                        OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="False" ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                                
                            </div>
                            <%--Column 2 --%>
                            <div class="col-sm-6 col-right-pd10">
                                
                                <div class="form-group">
                                    <label for="txtShippingCompany">
                                        Company/Salon Name</label>
                                    <asp:TextBox ID="txtShippingCompany" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Company/Salon Name"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtShippingAddress1">
                                        Street Address</label>
                                        <span class="small">Street address and apt/suite number</span>
                                    <asp:TextBox ID="txtShippingAddress1" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Street Address"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtShippingAddress1" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtShippingAddress1" Display="Dynamic" ErrorMessage="Address 1 is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="txtShippingAddress2">
                                        Address 2</label>
                                    <asp:TextBox ID="txtShippingAddress2" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Address Line 2"></asp:TextBox>
                                </div>
                                <div class="form-group" id="divShippingCity">
                                        <label class="required" for="txtShippingCity">
                                            City</label>
                                        <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                            ID="txtShippingCity" placeholder="City" />
                                        <asp:RequiredFieldValidator ID="rqtxtShippingCity" runat="server" ControlToValidate="txtShippingCity"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="City is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    </div>
                                <div class="form-row">
                                    <div class="form-group col-sm-6 col-left" id="divShippingState">
                                        <label class="required" for="drpShippingState">
                                            State</label>
                                        <div class="nf-dropdown">
                                            <asp:DropDownList ID="drpShippingState" ClientIDMode="Static" CssClass="form-control"
                                                runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqdrpShippingState" runat="server" ControlToValidate="drpShippingState"
                                                CssClass="text-danger" Display="Dynamic" ErrorMessage="State is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group col-sm-6 col-right">
                                        <label class="required" for="txtShippingZip">
                                            Zip Code</label>
                                        <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server" ID="txtShippingZip" placeholder="Zip Code" MaxLength="5" />
                                        <asp:RequiredFieldValidator ID="rqtxtShippingZip" runat="server" ControlToValidate="txtShippingZip"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Zip Code is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator id="rqextxtShippingZip" Display="Dynamic" ControlToValidate="txtShippingZip" ValidationExpression="^[\s\S]{5,}$" runat="server" ErrorMessage="Zip Code must be at least 5 digit" CssClass="text-danger" ValidationGroup="valRegister"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-content text-right" style="border-top:1px solid #dadada;margin-top:0px;padding-top:10px">
                        <asp:LinkButton ID="lbtnBack" CssClass="back-link" ClientIDMode="Static" runat="server">Back</asp:LinkButton>
                        <asp:Button ID="btnContinue" ClientIDMode="Static" runat="server" CausesValidation="true"
                            ValidationGroup="valRegister" Text="Continue" data-btn="submit" CssClass="btn btn-submit" />
                    </div>
                </div>
                <div class="popbilling" style="display: none;" id="popConfirm">
                    <div class="header">
                        <asp:Literal ID="litMsg" runat="server" Text="According to US Postal Office database, your shipping address is not correct.<br /> Please choose one of the following options:<br />"></asp:Literal>
                    </div>
                    <ul class="list-addess">
                        <asp:Repeater ID="rptListConfirmAddress" runat="server">
                            <ItemTemplate>
                                <li>
                                    <ul>
                                        <asp:Literal ID="ltrAddress" runat="server">
                                        </asp:Literal>
                                        <li>
                                            <asp:LinkButton ID="lkbYes" runat="server" Text="Yes, this is my shipping address"
                                                CommandName="Select" CommandArgument="<%#Container.DataItem.All%>"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <ul class="bottom-row">
                        <li><a onclick="ClosePopupListConfirm()" href="javascript:void(0);">Edit your shipping
                            address again</a> </li>
                        <li>
                            <asp:LinkButton ID="lkbCandidateCheckout" runat="server" Text="Continue to check-out"></asp:LinkButton>
                        </li>
                    </ul>
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
        $("#txtBillingZip").on('blur keyup change click', function () {
            var zipCode = $("#txtBillingZip").val();
            SetZipCode('Billing');
        });

        $("#txtShippingZip").on('blur keyup change click', function () {
            var zipcode = $("#txtShippingZip").val();
            SetZipCode('Shipping');
        });

        function SetZipTextBox() {
            $("#txtBillingZip").on('blur keyup change click', function () {
                var zipcode = $("#txtBillingZip").val();
                SetZipCode('Billing');
            });

            $("#txtShippingZip").on('blur keyup change click', function () {
                var zipcode = $("#txtShippingZip").val();
                SetZipCode('Shipping');
            });
        }
        function SetZipCode(AddressType) {
            var hid = $("#hid" + AddressType + "ZipCode").val();
            var city = $("#txt" + AddressType + "City").val();
            var zipcode = $("#txt" + AddressType + "Zip").val();
            var state = $("#drp" + AddressType + "State").val();

            if (zipcode.length < 5) {
                return;
            }

            if (hid == zipcode) {
                return;
            }

            $("#hid" + AddressType + "ZipCode").val(zipcode);
            if (!$.isNumeric(zipcode)) {
                ShowError(AddressType + " zip code is invalid. Please check again.");
                return;
            }

            $("#txt" + AddressType + "City").focus();
            var url = '<%=Request.Path %>/SetZipCode';
            $.ajax({
                type: "POST",
                url: url,
                data: "{city:'" + city + "', zipcode:'" + zipcode + "',state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var s = '';
                    var lst = JSON.parse(response.d);
                    if (lst != null) {
                        if (lst == '') {
                            ShowError(AddressType + " zip code is invalid. Please check again.");
                        }
                        else if (lst == 'VI' || lst == 'PR') {
                            window.location.href = 'billingint.aspx?type=Billing&ctr=' + lst + '&act=<%=act%>';
                        }
                        else {
                            $("#div" + AddressType + "City").show();
                            $("#div" + AddressType + "State").show();
                            if (lst.length == 1) {
                                $("#drp" + AddressType + "State").val(lst[0].StateCode);
                                $("#txt" + AddressType + "City").val(lst[0].CityName);
                            }
                            else {
                                var msg = '';
                                var i = 0;
                                $.each(lst, function (idx, obj) {
                                    msg += '<input type="radio" id="rbtn' + obj.StateCode + i + '" class="radio-node" name="rbtn' + AddressType + zipcode + '" onclick="SelectAddress(\'' + obj.CityName + '\',\'' + obj.StateCode + '\',\'' + AddressType + '\');" /><label class="radio-label" for="rbtn' + obj.StateCode + i + '">&nbsp;</label><label class="city" for="rbtn' + obj.StateCode + i + '">' + obj.CityName + ', ' + obj.StateCode + '</label><br>';
                                    i++;
                                });
                                showQtip('qtip-error', msg, 'Revise your ' + AddressType + ' Address');
                            }
                        }
                    }
                },
                failure: function (response) {

                },
                error: function (response) {

                }
            });

        }

        $("#drpBillingCountry, #drpShippingCountry").change(function () {
            fnSelectCountryCode(this.id);
        });

        function SelectAddress(city, state, type) {
            $('#txt' + type + 'City').val(city);
            $('#drp' + type + 'State').val(state);

            $('#qtip-blanket').hide();
            $(".qtip").remove();
        }

        function showListAddress() {
            var html = $('#popConfirm').html();
            html = html.replace('display:none;', 'display:block;');
            showQtipID('qtip-address', html, 'Confirm address', 'tip-confirm-address');
        }
        function ClosePopupListConfirm() {
            $('.qtip-address .qtip-button').click();
        }

        //        function CheckProfessionalStatus(sender, args) {
        //            if (document.getElementById("<=chkProfessionalStatus.ClientID >").checked == true) {
        //                args.IsValid = true;
        //            } else {
        //                args.IsValid = false;
        //            }
        //        }

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

        $(window).load(function () {
            ShowPageLoadError();
            // '#txtBillingPhone, #txtBillingDaytimePhone, #txtBillingFax, #txtShippingPhone, #txtShippingFax
            fnFormatPhoneUS('#txtBillingPhone','','');
            fnFormatPhoneUS('#txtBillingDaytimePhone', '', '');
            fnFormatPhoneUS('#txtBillingFax', '', '');
            fnFormatPhoneUS('#txtShippingPhone', '', '');
            fnFormatPhoneUS('#txtShippingFax', '', '');
        });
        $('#txtBillingPhone, #txtBillingDaytimePhone, #txtBillingFax, #txtShippingPhone, #txtShippingFax').keyup(function (e) {

            fnFormatPhoneUS(this, e, this.id);
        });
     </script>

    <asp:Literal ID="ltrLoadMsg" runat="server"></asp:Literal>
</asp:Content>
