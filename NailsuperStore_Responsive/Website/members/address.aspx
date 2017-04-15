<%@ Page Language="VB" AutoEventWireup="false" CodeFile="address.aspx.vb" Inherits="members_address"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <h1>
        Edit Account Details</h1>
    <asp:Panel runat="server" DefaultButton="btnSubmit" CssClass="form" role="form">
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="panel-content" id="divBillingAddress" runat="server">
                    <div class="title">
                        Billing Address</div>
                    <div class="content">
                        <div class="col-sm-6 col-left-pd10 ">
                            <div class="form-row billingname">
                                <div class="form-group col-sm-6 col-left">
                                    <label class="required" for="txtBillingFirstName">
                                        First Name</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtBillingFirstName"
                                        placeholder="First Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingFirstName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtBillingFirstName" Display="Dynamic" ErrorMessage="Billing first name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-sm-6 col-right">
                                    <label class="required" for="txtBillingLastName">
                                        Last Name</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtBillingLastName"
                                        placeholder="Last Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingLastName" runat="server" ControlToValidate="txtBillingLastName"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Billing last name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtBillingCompany"> Salon Name</label>
                                <asp:TextBox ID="txtBillingCompany" runat="server" type="text" CssClass="form-control" placeholder="Salon Name"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="required" for="txtBillingAddress1"> Address Line 1</label>
                                <asp:TextBox ID="txtBillingAddress1" runat="server" type="text" CssClass="form-control" placeholder="Address Line 1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqtxtBillingAddress1" runat="server" CssClass="text-danger"
                                    ControlToValidate="txtBillingAddress1" Display="Dynamic" ErrorMessage="Billing address is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtBillingAddress2">
                                    Address Line 2</label>
                                <asp:TextBox ID="txtBillingAddress2" runat="server" type="text" CssClass="form-control"
                                    placeholder="Address Line 2"></asp:TextBox>
                            </div>
                            <div class="form-row" id="dvStateCity">
                                <div class="form-group col-sm-6 col-left">
                                    <label class="required" for="txtBillingCity">
                                        City</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtBillingCity"
                                        placeholder="City" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingCity" runat="server" ControlToValidate="txtBillingCity"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Billing city is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class=" form-group col-sm-6 col-right" runat="server" id="dvBillingState">
                                    <label class="required" for="drpBillingState">
                                        State</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpBillingState" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rqdrpBillingState" runat="server" ControlToValidate="drpBillingState"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Billing state is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class=" form-group col-sm-6 col-right" runat="server" id="dvBillingRegion">
                                    <label class="required" for="txtBillingRegion">
                                        Province/Region</label>
                                    <asp:TextBox ID="txtBillingRegion" runat="server" CssClass="form-control" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingRegion" runat="server" ControlToValidate="txtBillingRegion"
                                        Display="Dynamic" CssClass="text-danger" Enabled="false" ErrorMessage="Billing region is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class=" form-group col-sm-6 col-left">
                                    <label id="lbzip" class="<%=css %>" for="txtBillingZip">
                                        Zip Code
                                        <%If drpBillingCountry.SelectedValue = "US" Then%><span class="require"></span><span id="zipcode" style="display:none;" runat="server"></span><%End If%></label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtBillingZip" placeholder="Zip Code" />
                                    <asp:RequiredFieldValidator ID="rqtxtBillingZip" runat="server" ControlToValidate="txtBillingZip"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Zip Code is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                         <asp:RegularExpressionValidator id="rqextxtBillingZip" Display="Dynamic" ControlToValidate="txtBillingZip" ValidationExpression="^[\s\S]{5,}$" runat="server" ErrorMessage="Zip Code must be at least 5 digit" CssClass="text-danger" ValidationGroup="valRegister"></asp:RegularExpressionValidator>
                                </div>
                                <div class=" form-group col-sm-6 col-right">
                                    <label class="required" for="drpBillingCountry">
                                        Country</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpBillingCountry" CssClass="form-control" runat="server" AutoPostBack="true" >
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rqdrpBillingCountry" runat="server" ControlToValidate="drpBillingCountry"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Billing country is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <%--Column 2 --%>
                        <div class="col-sm-6 col-right-pd10">
                            <div id="BillingPhone" class="form-group">
                                <label class="multi-line" for="txtBillingPhone">
                                    <div class="required">Billing, Home, or Work Phone</div>
                                    <div class="small">Must match billing address</div></label>
                                <asp:TextBox ID="txtBillingPhone" runat="server" type="tel" CssClass="form-control"
                                    placeholder="Billing, Home, or Work Phone"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqtxtBillingPhone" runat="server" ControlToValidate="txtBillingPhone"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Billing phone number is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cusvPhoneBillingInt" runat="server" ClientValidationFunction="ClientCheckPhoneInternational"
                                    CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic"
                                    ErrorMessage="Phone is invalid." OnServerValidate="ServerCheckPhoneInternational"
                                    ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                <asp:CustomValidator ID="cusvPhoneBillingUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                    CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic"
                                    ErrorMessage="Phone number is invalid." OnServerValidate="ServerCheckPhoneUS"
                                    ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtBillingFax">
                                    Fax</label>
                                <asp:TextBox ID="txtBillingFax" runat="server" type="tel" CssClass="form-control"
                                    placeholder="Fax"></asp:TextBox>
                                <asp:CustomValidator ID="cusvFaxBillingInt" runat="server" ClientValidationFunction="ClientCheckFaxInternational"
                                    OnServerValidate="ServerCheckFaxInternational" CssClass="text-danger" ErrorMessage="Fax number is invalid"
                                    ControlToValidate="txtBillingFax" Display="Dynamic" ValidateEmptyText="false"
                                    ValidationGroup="valRegister"></asp:CustomValidator>
                                <asp:CustomValidator ID="cusvFaxBillingUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                    CssClass="text-danger" ControlToValidate="txtBillingFax" Display="Dynamic" ErrorMessage="Fax is invalid."
                                    OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="False" ValidationGroup="valRegister"></asp:CustomValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtBillingEmail">
                                    <span class="required">Email</span></label>
                                <asp:TextBox ID="txtBillingEmail" runat="server" type="text" CssClass="form-control"
                                    placeholder="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqBillingEmail" runat="server" ControlToValidate="txtBillingEmail"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Email is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="CheckEmailValidate" CssClass="text-danger" ErrorMessage="Email is invalid"
                                    ControlToValidate="txtBillingEmail" Display="Dynamic" ValidateEmptyText="false" OnServerValidate="ServerCheckEmail"
                                    ValidationGroup="valRegister"></asp:CustomValidator>
                                <%--<asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                    CssClass="text-danger" ControlToValidate="txtBillingEmail" Display="Dynamic" ErrorMessage="Email is invalid."
                                    OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="False" ValidationGroup="valRegister"></asp:CustomValidator>--%>
                            </div>
                            <div class="form-group">
                                <div class="checkbox address">
                                    <label for="chkSameAsBilling">
                                        <asp:CheckBox runat="server" ID="chkSameAsBilling" Checked="true" AutoPostBack="true" />
                                        <i class="fa fa-check checkbox-font" ></i>Same As My Billing Address
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
                    <ProgressTemplate>
                        <center>
                            <div class="ajaxwaiting">
                                Please wait...<br />
                                <img src="/includes/theme/images/loader.gif" style="border: 0" alt="" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="panel-content" id="divShippingAddress" runat="server">
                    <div class="title">
                        Shipping Address</div>
                    <div class="content">
                        <div class="col-sm-6 col-left-pd10">
                            <div class="form-row">
                                <div class=" form-group col-sm-6 col-left">
                                    <label class="required" for="txtShippingFirstName">
                                        First Name</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtShippingFirstName"
                                        placeholder="First Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingFirstName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtShippingFirstName" Display="Dynamic" ErrorMessage="Shipping first name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-sm-6  col-right">
                                    <label class="required" for="txtShippingLastName">
                                        Last Name</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtShippingLastName"
                                        placeholder="Last Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingLastName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtShippingLastName" Display="Dynamic" ErrorMessage="Shipping last name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtShippingCompany">
                                    Salon Name</label>
                                <asp:TextBox ID="txtShippingCompany" runat="server" type="text" CssClass="form-control"
                                    placeholder="Salon Name"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="required" for="txtShippingAddress1">
                                    Address Line 1</label>
                                <asp:TextBox ID="txtShippingAddress1" runat="server" type="text" CssClass="form-control"
                                    placeholder="Address Line 1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqtxtShippingAddress1" runat="server" CssClass="text-danger"
                                    ControlToValidate="txtShippingAddress1" Display="Dynamic" ErrorMessage="Shipping address is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtShippingAddress2">
                                    Address Line 2</label>
                                <asp:TextBox ID="txtShippingAddress2" runat="server" type="text" CssClass="form-control"
                                    placeholder="Address Line 2"></asp:TextBox>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-6 col-left">
                                    <label class="required" for="txtShippingCity">
                                        City</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtShippingCity"
                                        placeholder="City" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingCity" runat="server" ControlToValidate="txtShippingCity"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Shipping city is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-sm-6 col-right" runat="server" id="dvShippingState">
                                    <label class="required" for="drpShippingState">
                                        State</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpShippingState" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rqdrpShippingState" runat="server" ControlToValidate="drpShippingState"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Shipping state is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-right" runat="server" id="dvShippingRegion">
                                    <label for="txtShippingRegion">
                                        Province/Region</label>
                                    <asp:TextBox ID="txtShippingRegion" runat="server" CssClass="form-control" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingRegion" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtShippingRegion" Display="Dynamic" Enabled="false" ErrorMessage="Shipping region is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-6 col-left">
                                    <label class="required" for="txtShippingZip">
                                        Zip Code</label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtShippingZip"
                                        placeholder="Zip Code" />
                                    <asp:RequiredFieldValidator ID="rqtxtShippingZip" runat="server" ControlToValidate="txtShippingZip"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Zip Code must be at least 5 digit"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-sm-6 col-right">
                                    <label class="required" for="drpShippingCountry">
                                        Country</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpShippingCountry" CssClass="form-control" runat="server"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rqdrpShippingCountry" runat="server" ControlToValidate="drpShippingCountry"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Shipping country is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <%--Column 2 --%>
                        <div class="col-sm-6 col-right-pd10">
                            <div class="form-group">
                                <label class="required" for="txtShippingPhone">
                                    Shipping, Home, or Work Phone</label>
                                <asp:TextBox ID="txtShippingPhone" runat="server" type="tel" CssClass="form-control"
                                    placeholder="Shipping, Home, or Work Phone"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqtxtShippingPhone" runat="server" ControlToValidate="txtShippingPhone"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cusvShippingPhoneInt" runat="server" ClientValidationFunction="ClientCheckPhoneInternational"
                                    CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic"
                                    ErrorMessage="Phone is invalid." OnServerValidate="ServerCheckPhoneInternational"
                                    ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                <asp:CustomValidator ID="cusvShippingPhoneUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                    CssClass="text-danger" ControlToValidate="txtShippingPhone" Display="Dynamic"
                                    ErrorMessage="Phone number is invalid." OnServerValidate="ServerCheckPhoneUS"
                                    ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div id="divTaxInfo" class="panel-content" runat="server" visible="false">
                    <div class="title">
                        Sales Tax Information</div>
                    <div class="content">
                        <div class="form-group">
                            <div class="checkbox">
                                <label for="chkDORRegistered">
                                    <asp:CheckBox runat="server" ID="chkDORRegistered" />
                                    <i class="fa fa-check checkbox-font" ></i>I am registered as a reseller with the Illinois
                                    Department of Revenue
                                </label>
                            </div>
                        </div>
                        <div id="divSubTaxInfo" runat="server">
                            <div class="text">
                                <strong>Sales and Use Tax Certificate Electronic Signature</strong></div>
                            <div class="form-group txt-required">
                                <asp:TextBox ID="txtTaxNumber" runat="server" class="form-control" MaxLength="30"
                                    placeholder="Sales Tax Exempt #" />
                                <asp:RequiredFieldValidator ID="rqtxtTaxNumber" runat="server" ControlToValidate="txtTaxNumber"
                                    Display="Dynamic" CssClass="text-danger" Enabled="false" ErrorMessage="Tax number is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <div class="checkbox">
                                    <label for="chkResaleAgreement">
                                        <asp:CheckBox runat="server" ID="chkResaleAgreement" />
                                        <i class="fa fa-check checkbox-font" ></i>I certify that all of the purchases that I make
                                        from nailsuperstore.com are for resale.
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="checkbox">
                                    <label for="chkInformationAccuracyAcceptance">
                                        <asp:CheckBox runat="server" ID="chkInformationAccuracyAcceptance" />
                                        <i class="fa fa-check checkbox-font" ></i>Under penalties of perjury, I swear or affirm
                                        that the information on this form is true and correct as to every material matter.
                                    </label>
                                </div>
                            </div>
                            <div class="text">
                                <strong>Authorized Signature</strong>
                                <br />
                                <span class="small">Please type your name in the spaces below to electronically sign
                                    your application</span>
                            </div>
                            <div class="form-group txt-required">
                                <asp:TextBox ID="txtAuthorizedSignatureName1" runat="server" class="form-control"
                                    placeholder="Full Name" />
                                <asp:RequiredFieldValidator ID="rqtxtAuthorizedSignatureName1" runat="server" ControlToValidate="txtAuthorizedSignatureName1"
                                    CssClass="text-danger" Display="Dynamic" Enabled="false" ErrorMessage="Full Name is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                            </div>
                            <div class="text small">
                                Please re-type your name in the spaces below to confirm your electronic application</div>
                            <div class="form-group txt-required">
                                <asp:TextBox ID="txtAuthorizedSignatureName2" runat="server" class="form-control"
                                    placeholder="Confirm to Full Name" />
                                <asp:RequiredFieldValidator ID="rqtxtAuthorizedSignatureName2" runat="server" ControlToValidate="txtAuthorizedSignatureName2"
                                    CssClass="text-danger" Display="Dynamic" Enabled="false" ErrorMessage="Full Name is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                <CC:CompareValidatorFront ID="cvAuthorizedSignatureName" runat="server" ControlToCompare="txtAuthorizedSignatureName2"
                                    CssClass="text-danger" ControlToValidate="txtAuthorizedSignatureName1" Display="Dynamic"
                                    ErrorMessage="Electronic Signature Names must match" Operator="Equal" Type="String"
                                    ValidationGroup="valRegister"></CC:CompareValidatorFront>
                            </div>
                            <div class="form-group txt-required">
                                <asp:TextBox ID="txtTitle" runat="server" class="form-control" placeholder="Title" />
                                <asp:RequiredFieldValidator ID="rqtxtTitle" runat="server" ControlToValidate="txtTitle"
                                    Display="Dynamic" CssClass="text-danger" Enabled="false" ErrorMessage="Title is required"
                                    ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">
                                    Date</label>
                                <div class="col-sm-10">
                                    <p class="form-control-static">
                                        <asp:Label ID="lblDate" runat="server" /></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
               <%-- <div class="panel-content form-horizontal" runat="server" id="divProfessionalStatus">
                    <div class="form-group">
                        <div class="checkbox">
                            <label for="chkProfessionalStatus">
                                <asp:CheckBox runat="server" ID="chkProfessionalStatus" />
                                <i class="fa fa-check checkbox-font" ></i>I certify that all of the purchases are for Professional
                                Use Only. Any false statement will void all my rights for return or exchange or
                                claims or product warranty. I accept full responsibilities including misuse of products,
                                accident with no fault against The Nail Superstore.
                            </label>
                        </div>
                    </div>
                </div>--%>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="drpBillingCountry" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="panel-content text-right">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" data-btn="submit" CssClass="btn btn-submit" CausesValidation="true"/>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        function CheckEmailValidate(source, arguments) {

            var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            arguments.IsValid = re.test(arguments.Value);
        }

        function CheckSameAsBilling() {
            if ($('#chkSameAsBilling').is(':checked')) {
                $('#divShippingAddress').css('display', 'none');
            }
            else {
                $('#divShippingAddress').css('display', 'block');
            }
            //TaxInfoCheck();
        }

        $('#chkSameAsBilling').click(function () {
            CheckSameAsBilling();
        });
        CheckSameAsBilling();
        
        function eventRegistration() {
            $('someSelectoHere').click(WorkFunction);
        }
        function WorkFunction() {
            //Randome code here to do yoru work.
            //If no callback function is used.
            eventRegistration()
            //if there is a call back function from the server put the eventRegistration() in that function.
        }
       
        $(document).ready(function () {
            
            if ($('#error').length > 0)
            {
                $("#txtBillingZip").on('input change click', function (e) {
                    SetZipCode("txtBillingZip");
                });

                $("#txtShippingZip").on('input change click', function (e) {
                    SetZipCode("txtShippingZip");
                });

                $('#drpBillingState').on("change", function () {
                    var currentZipCode = $('#zipcode').text().split("|");
                    $('#zipcode').text(' |' + currentZipCode[1]);
                    SetZipCode("txtBillingZip");
                });

                $('#drpShippingState').on("change", function () {
                    var currentZipCode = $('#zipcode').text().split("|");
                    $('#zipcode').text(currentZipCode[0] + '| ');
                });

                return;
            }

            if ($('#drpBillingCountry').val() == 'US') {
                SetZipCode("txtBillingZip");
            }
            else {
                var currentZipCode = $('#zipcode').text().split("|");
                $('#zipcode').text(' |' + currentZipCode[1]);
            }

            if ($('#drpShippingCountry').val() == 'US') {
                SetZipCode("txtShippingZip");
            }
            else {
                var currentZipCode = $('#zipcode').text().split("|");
                $('#zipcode').text(currentZipCode[0] + '| ');
            }

            $("#txtBillingZip").on('input change click', function (e) {
                SetZipCode("txtBillingZip");
            });

            $("#txtShippingZip").on('input change click', function (e) {
                SetZipCode("txtShippingZip");
            });

            $('#drpBillingState').on("change", function () {
                var currentZipCode = $('#zipcode').text().split("|");
                $('#zipcode').text(' |' + currentZipCode[1]);
                SetZipCode("txtBillingZip");
            });

            $('#drpShippingState').on("change", function () {
                var currentZipCode = $('#zipcode').text().split("|");
                $('#zipcode').text(currentZipCode[0] + '| ');
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            if ($('#drpBillingCountry').val() == 'US') {
                if ($('#chkSameAsBilling').is(':checked')) {
                    if ($('#zipcode').text().indexOf('|chkSameAsBillingChange') < 1) {
                        $('#dvStateCity').css('display', 'none');
                        var currentZipCode = $('#zipcode').text().split("|");
                        $('#zipcode').text(' |' + currentZipCode[1]);
                        SetZipCode("txtBillingZip");
                    }
                }
            }
            
            $("#txtBillingZip").on('input change click', function (e) {
                SetZipCode("txtBillingZip");
            });

            $('#drpBillingState').on("change", function () {
                var currentZipCode = $('#zipcode').text().split("|");
                $('#zipcode').text(' |' + currentZipCode[1]);
            });

            if ($('#txtShippingZip').length == 1) {
                if ($('#drpShippingCountry').val() == 'US') {
                    if (!$('#chkSameAsBilling').is(':checked')) {
                        if ($('#zipcode').text().indexOf('|chkSameAsBillingChange') < 1) {
                            var currentZipCode = $('#zipcode').text().split("|");
                            $('#zipcode').text(currentZipCode[0] + '| ');
                            SetZipCode('txtShippingZip');
                        }
                    }
                }

                $("#txtShippingZip").on('input change click', function (e) {
                    SetZipCode("txtShippingZip");
                });

                $('#drpShippingState').on("change", function () {
                    var currentZipCode = $('#zipcode').text().split("|");
                    $('#zipcode').text(currentZipCode[0] + '| ');
                    SetZipCode('txtShippingZip');
                });
            }
        });

        SetZipCode = function (target) {
            if (target == "txtShippingZip") {
                if ($("#drpShippingCountry").val() != "US")
                    return;

                $city = $("#txtShippingCity").val();
                $zipcode = $("#txtShippingZip").val();
                $state = $("#drpShippingState").val();
                if ($zipcode.length < 5) {
                    return;
                }
                if ($zipcode.length > 5) {
                    $zipcode = $zipcode.substring(0, 5);
                    $("#txtShippingZip").val($zipcode);
                }

                var currentZipCode = $('#zipcode').text().split("|");

                if (currentZipCode[1] == $zipcode) {
                    return;
                }

                $('#zipcode').text(currentZipCode[0] + '|' + $zipcode);

                if (!$.isNumeric($zipcode)) {
                    showQtip('qtip-error', "Zip code is invalid. Please check again.", "Ooops");
                    return;
                }

                $("#txtShippingCity").focus();
                var url = '/members/addressbook/edit.aspx/SetZipCode';
                $.ajax({
                    type: "POST",
                    url: url,
                    data: "{city:'" + $city + "', zipcode:'" + $zipcode + "',state:'" + $state + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var s = '';
                        var lst = JSON.parse(response.d);
                        if (lst != null) {
                            if (lst == '') {
                                showQtip('qtip-error', "Zip code is invalid. Please check again.", "Ooops");
                            }
                            else {

                                if (lst.length == 1) {
                                    $("#drpShippingState").val(lst[0].StateCode);
                                    $("#txtShippingCity").val(lst[0].CityName);
                                }
                                else {
                                    $msg = '';
                                    var i = 0;
                                    $.each(lst, function (idx, obj) {
                                        $msg += '<input type="radio" id="rbtn' + obj.StateCode + i + '" class="radio-node" name="rbtn' + $zipcode + '" onclick="SelectAddress(\'' + obj.CityName + '\',\'' + obj.StateCode + '\', false);" /><label class="radio-label" for="rbtn' + obj.StateCode + i + '">&nbsp;</label><label class="city" for="rbtn' + obj.StateCode + i + '">' + obj.CityName + ', ' + obj.StateCode + '</label><br>';
                                        i++;
                                    });
                                    showQtip('qtip-error', $msg, 'Revise your address');
                                }
                            }
                        }

                    },
                    failure: function (response) {
                    },
                    error: function (response) {
                    }
                });
                return;
            }

            if ($("#drpBillingCountry").val() != "US") {
                return;
            }

            $city = $("#txtBillingCity").val();
            $zipcode = $("#txtBillingZip").val();
            $state = $("#drpBillingState").val();
            if ($zipcode.length < 5) {
                return;
            }
            if ($zipcode.length > 5) {
                $zipcode = $zipcode.substring(0, 5);
                $("#txtBillingZip").val($zipcode);
            }

            var currentZipCode = $('#zipcode').text().split("|");

            if (currentZipCode[0] == $zipcode) {
                return;
            }

            $('#zipcode').text($zipcode + "|" + currentZipCode[1]);

            if (!$.isNumeric($zipcode)) {
                showQtip('qtip-error', "Zip code is invalid. Please check again.", "Ooops");
                return;
            }

            var url = '/members/addressbook/edit.aspx/SetZipCode';
            $.ajax({
                type: "POST",
                url: url,
                data: "{city:'" + $city + "', zipcode:'" + $zipcode + "',state:'" + $state + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var s = '';
                    var lst = JSON.parse(response.d);
                    if (lst != null) {
                        if (lst == '') {
                            showQtip('qtip-error', "Zip code is invalid. Please check again.", "Ooops");
                        }
                        else {
                            
                            if (lst.length == 1) {
                                $("#drpBillingState").val(lst[0].StateCode);
                                $("#txtBillingCity").val(lst[0].CityName);
                                $('#dvStateCity').css('display', 'block');
                            }
                            else {
                                $msg = '';
                                var i = 0;
                                $.each(lst, function (idx, obj) {
                                    $msg += '<input type="radio" id="rbtn' + obj.StateCode + i + '" class="radio-node" name="rbtn' + $zipcode + '" onclick="SelectAddress(\'' + obj.CityName + '\',\'' + obj.StateCode + '\', true);" /><label class="radio-label" for="rbtn' + obj.StateCode + i + '">&nbsp;</label><label class="city" for="rbtn' + obj.StateCode + i + '">' + obj.CityName + ', ' + obj.StateCode + '</label><br>';
                                    i++;
                                });
                                  showQtip('qtip-error', $msg, 'Revise your address');
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

        SelectAddress = function (city, state, isBillingAddress) {
            if (isBillingAddress) {
                $('#txtBillingCity').val(city);
                $('#drpBillingState').val(state);
                $('#qtip-blanket').hide();
                
                $('#dvStateCity').css('display', 'block');
            }
            else {
                $('#txtShippingCity').val(city);
                $('#drpShippingState').val(state);
                $('#qtip-blanket').hide();
            }
            $(".qtip").remove();
        }

        function pageLoad() {
            if ($("#drpBillingCountry").val() == "US") {
               // $("#txtBillingPhone").mask("?999-999-9999 9999");
                // $("#txtBillingFax").mask("?999-999-9999 9999");
                fnFormatPhoneUS('#txtBillingPhone', '', '');
                fnFormatPhoneUS('#txtBillingFax', '', '');
            }
            if (document.getElementById('chkSameAsBilling').checked == false && $("#drpShippingCountry").val() == "US") {
                // $("#txtShippingPhone").mask("?999-999-9999 9999");
                fnFormatPhoneUS('#txtShippingPhone', '');
            }
            $('#txtBillingPhone, #txtBillingFax').keyup(function (e) {
                if ($("#drpBillingCountry").val() == "US") {
                    fnFormatPhoneUS(this, e, this.id);
                }

            });
            $('#txtShippingPhone').keyup(function (e) {
                if (document.getElementById('chkSameAsBilling').checked == false && $("#drpBillingCountry").val() == "US") {
                    fnFormatPhoneUS(this, e);
                }

            });
           
        }
      
    </script>
    <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
</asp:Content>
