<%@ Control Language="VB" AutoEventWireup="false" CodeFile="address-box.ascx.vb"
    Inherits="controls_layout_address_box" %>
<div class="form">
    <div id="up">
        <div class="panel-content">
            <div class="title">
                <%=AddressType %>
                Address</div>
            <div class="content">
                <div class="col-sm-6 col-left-pd10">
                    <div class="form-row billingname">
                        <div class="form-group col-sm-6 col-left">
                            <label class="required" for="txtFirstName">
                                First Name</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtFirstName" placeholder="First Name" />
                        </div>
                        <div class="form-group col-sm-6 col-right">
                            <label class="required" for="txtLastName">
                                Last Name</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtLastName" placeholder="Last Name" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="required" for="txtCompany">
                            Salon Name</label>
                        <asp:TextBox ID="txtCompany" runat="server" type="text" CssClass="form-control" placeholder="Salon Name"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="required" for="txtAddress1">
                            Address Line 1</label>
                        <asp:TextBox ID="txtAddress1" runat="server" type="text" CssClass="form-control"
                            placeholder="Address Line 1"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtAddress2">
                            Address Line 2</label>
                        <asp:TextBox ID="txtAddress2" runat="server" type="text" CssClass="form-control"
                            placeholder="Address Line 2"></asp:TextBox>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-6 col-left">
                            <label class="required" for="txtCity">
                                City</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtCity" placeholder="City" />
                        </div>
                        <div class=" form-group col-sm-6 col-right" runat="server" id="dvState">
                            <label class="required" for="drpState">
                                State</label>
                            <div class="nf-dropdown">
                                <asp:DropDownList ID="drpState" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class=" form-group col-sm-6 col-right" runat="server" id="dvRegion">
                            <label class="required" for="txtRegion">
                                Province/Region</label>
                            <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class=" form-group col-sm-6 col-left">
                            <label class="required" for="txtZip">
                                Zip Code
                                <%If drpCountry.SelectedValue = "US" Then%><span class="require"></span><%End If%></label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtZip" placeholder="Zip Code" />
                        </div>
                        <div class=" form-group col-sm-6 col-right">
                            <label class="required" for="drpCountry">
                                Country</label>
                            <div class="nf-dropdown">
                                <asp:DropDownList ID="drpCountry" CssClass="form-control" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Column 2 --%>
                <div class="col-sm-6 col-right-pd10">
                    <div class="form-group">
                        <label class="multi-line" for="txtPhone" id="lblPhoneLabel" runat="server" clientidmode="Static">
                            <span class="required">Billing, Home, or Work Phone</span><br />
                            <span class="small">Must match billing address</span></label>
                        <asp:TextBox ID="txtPhone" runat="server" type="tel" CssClass="form-control" placeholder="Billing, Home, or Work Phone"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divDaytimePhone" runat="server" clientidmode="Static" visible="false">
                        <label for="txtDayPhone">
                            Daytime / Contact Phone
                        </label>
                        <asp:TextBox ID="txtDayPhone" runat="server" type="tel" CssClass="form-control" placeholder="Daytime / Contact Phone "></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtFax">
                            Fax</label>
                        <asp:TextBox ID="txtFax" runat="server" type="tel" CssClass="form-control" placeholder="Fax"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divEmail" runat="server" clientidmode="Static" visible="false">
                        <label for="txtEmail">
                            <span class="required">Email Address </span>
                        </label>
                        <asp:TextBox ID="txtEmail" runat="server" type="tel" CssClass="form-control" placeholder="Email Address"></asp:TextBox>
                        <label>
                            <span class="small">Example: shop@nss.com</span></label>
                    </div>
                    <div class="form-group" id="divSameAddress" runat="server" clientidmode="Static" visible="false">
                        <div class="checkbox address">
                            <label for="chkSameAsBilling">
                                <asp:CheckBox runat="server" ID="chkSameAsBilling" Checked="true" AutoPostBack="true" />
                                <i class="fa fa-check checkbox-font" ></i>Check here if your shipping address is same as
                                the billing address
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
