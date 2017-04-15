<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_addressbook_edit"
    MasterPageFile="~/includes/masterpage/checkout.master" CodeFile="edit.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <div id="billing-page">
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" id="hidZipCode" ClientIDMode="Static" />
                <div class="form">
                    <div class="panel-content" style="margin-top: 15px;">
                        <div class="title">
                            <asp:Literal ID="ltrTitle" runat="server">
                            </asp:Literal>
                        </div>
                        <div class="content" style="border-bottom:1px solid #dadada;margin-top:0px;">
                                <div class="col-sm-6 col-left-pd10">
                                    <label class="required" for="drpCountry">
                                        Country</label>
                                    <div class="nf-dropdown">
                                        <asp:DropDownList ID="drpCountry" ClientIDMode="Static" Enabled="true" CssClass="form-control"
                                            runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqdrpCountry" runat="server" ControlToValidate="drpCountry"
                                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Country is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        <div class="content">
                            <div class="col-sm-6 col-left-pd10">
                                <div style="color: #333333; font-size: 18px;font-weight: 600; padding:0px 20px 7px 0px;">Contact Information</div>
                                <div class="form-group">
                                    <label class="required" for="txtLabel">
                                        Label
                                    </label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtLabel" placeholder="Label" />
                                    <asp:RequiredFieldValidator ID="rqtxtLabel" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtLabel" Display="Dynamic" ErrorMessage="Label is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtFirstName">
                                        First Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtFirstName" placeholder="First Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtFirstName" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="First name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtLastName">
                                        Last Name</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server"
                                        ID="txtLastName" placeholder="Last Name" />
                                    <asp:RequiredFieldValidator ID="rqtxtLastName" runat="server" ControlToValidate="txtLastName"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Last name is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group" id="divEmail" runat="server" visible="false">
                                    <label class="required" for="txtEmail">
                                        Email Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" ClientIDMode="Static" type="text"
                                        CssClass="form-control" placeholder="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtEmail" runat="server" CssClass="text-danger" Visible="false"
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cusvEmail" ControlToValidate="txtEmail" Display="Dynamic" Visible="false"
                                        ErrorMessage="Email is invalid" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate"
                                        runat="server" ValidationGroup="valRegister" />
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtBillingPhone">
                                        Home, or Work Phone</label>
                                    <asp:TextBox ID="txtBillingPhone" runat="server" ClientIDMode="Static" type="tel" CssClass="form-control"
                                        placeholder="Phone"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtBillingPhone" runat="server" ControlToValidate="txtBillingPhone"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Phone number is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cusvPhoneUS" runat="server" ClientValidationFunction="ClientCheckPhoneUS"
                                        CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic" ErrorMessage="Phone number is invalid."
                                        OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cusvPhoneInt" runat="server" ClientValidationFunction="ClientCheckPhoneInternational"
                                        OnServerValidate="ServerCheckPhoneInternational" CssClass="text-danger" ErrorMessage="Phone number is invalid"
                                        ControlToValidate="txtBillingPhone" Display="Dynamic" ValidateEmptyText="false" ValidationGroup="valRegister"></asp:CustomValidator>
                                </div>
                            </div>

                            <div class="col-sm-6 col-right-pd10">
                                <div style="color: #333333; font-size: 18px;font-weight: 600; padding:0px 20px 7px 0px;">Address</div>
                                <div class="form-group">
                                    <label for="txtCompany">
                                        Company/Salon Name</label>
                                    <asp:TextBox ID="txtCompany" runat="server" ClientIDMode="Static" type="text" CssClass="form-control"
                                        placeholder="Company/Salon Name"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="required" for="txtAddress1">
                                        Street Address</label>
                                    <span class="small">Street address and apt/suite number</span>
                                    <asp:TextBox ID="txtAddress1" runat="server" ClientIDMode="Static" type="text" CssClass="form-control"
                                        placeholder="Street Address"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtAddress1" runat="server" CssClass="text-danger"
                                        ControlToValidate="txtAddress1" Display="Dynamic" ErrorMessage="Street Address is required"
                                        ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group" id="divCity">
                                    <label class="required" for="txtCity">
                                        City</label>
                                    <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server" MaxLength="20"
                                        ID="txtCity" placeholder="City" />
                                    <asp:RequiredFieldValidator ID="rqtxtCity" runat="server" ControlToValidate="txtCity"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="City is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-row">
                                    <div class=" form-group col-sm-6 col-left" runat="server" id="divState">
                                        <label class="required" for="drpState">
                                            State</label>
                                        <div class="nf-dropdown">
                                            <asp:DropDownList ID="drpState" ClientIDMode="Static" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqdrpState" runat="server" ControlToValidate="drpState"
                                                CssClass="text-danger" Display="Dynamic" ErrorMessage="State is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class=" form-group col-sm-6 col-left" runat="server" id="dvRegion" visible="false">
                                        <label class="required" for="txtRegion">
                                            Province/Region
                                        </label>
                                        <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="rqtxtRegion" runat="server" ControlToValidate="txtRegion"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Province/Region is required"
                                            ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class=" form-group col-sm-6 col-right">
                                        <label id="lbzip" class="<%=css %>" for="txtZip">
                                            Zip Code <span class="require"></span>
                                        </label>
                                        <asp:TextBox type="text" class="form-control" ClientIDMode="Static" runat="server" onkeyup="SetZipCode();" ID="txtZip" placeholder="Zip Code" MaxLength="5" />
                                        <asp:RequiredFieldValidator ID="rqtxtZip" runat="server" ControlToValidate="txtZip"
                                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Zip Code is required" ValidationGroup="valRegister"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator id="rqextxtZip" Display="Dynamic" ControlToValidate="txtZip" ValidationExpression="^[\s\S]{5,}$" runat="server" ErrorMessage="Zip Code must be at least 5 digit" CssClass="text-danger" ValidationGroup="valRegister"></asp:RegularExpressionValidator>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-content text-right">
                        <asp:LinkButton ID="lbtnBack" CssClass="back-link" ClientIDMode="Static" runat="server">Cancel</asp:LinkButton>
                        <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" CausesValidation="true"
                            ValidationGroup="valRegister" Text="Save" data-btn="submit" CssClass="btn btn-submit"/>
                    </div>
                </div>
                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="up">
                    <ProgressTemplate>
                        <center>
                            <div class="ajaxwaiting">
                                Please wait...<br />
                                <img src="/includes/theme/images/loader.gif" style="border: 0px" alt="" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="drpCountry" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        $("#txtZip").on('blur change click', function () {
            SetZipCode();
        });

        $(window).load(function () {
            if ($("#drpCountry").val() == "US") {
                fnFormatPhoneUS('#txtBillingPhone', '', '');

                if ($('#txtCity').val() == '') {
                    $('#divState').hide();
                    $('#divCity').hide();
                }

            }
        });
        $('#txtBillingPhone').keyup(function (e) {
            if ($("#drpCountry").val() == "US") {
                fnFormatPhoneUS(this, e, this.id);
            }
        });
 
        SetZipCode = function () {
            if ($("#drpCountry").val() != "US") {
                return;
            }

            $hid = $("#hidZipCode").val();
            $city = $("#txtCity").val();
            $zipcode = $("#txtZip").val();
            $state = $("#drpState").val();

            if ($zipcode.length < 5) {
                return;
            }

            if ($hid == $zipcode) {
                return;
            }

            $("#hidZipCode").val($zipcode);
            if (!$.isNumeric($zipcode)) {
                ShowError("Zip code is invalid. Please check again.");
                return;
            }
            else
            {
                var url = '<%=Request.Path %>/SetZipCode';
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
                                ShowError("Zip code is invalid. Please check again.");
                            }
                            else if (lst == 'VI' || lst == 'PR') {
                                $('#drpCountry').val(lst);
                                __doPostBack($('#drpCountry').id, 'OtherInformation');
                                $('#divState').hide();
                                $('#dvRegion').show();
                            }
                            else {
                                $("#divCity").show();
                                $("#divState").show();
                                if (lst.length == 1) {
                                    $("#drpState").val(lst[0].StateCode);
                                    $("#txtCity").val(lst[0].CityName);
                                }
                                else {
                                    $msg = '';
                                    var i = 0;
                                    $.each(lst, function (idx, obj) {
                                        $msg += '<input type="radio" id="rbtn' + obj.StateCode + i + '" class="radio-node" name="rbtn' + $zipcode + '" onclick="SelectAddress(\'' + obj.CityName + '\',\'' + obj.StateCode + '\');" /><label class="radio-label" for="rbtn' + obj.StateCode + i + '">&nbsp;</label><label class="city" for="rbtn' + obj.StateCode + i + '">' + obj.CityName + ', ' + obj.StateCode + '</label><br>';
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
         }

        SelectAddress = function(city, state) {
            $('#txtCity').val(city);
            $('#drpState').val(state);
            $('#qtip-blanket').hide();
            $(".qtip").remove();
        }

    </script>
</asp:Content>
