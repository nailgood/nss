<%@ Control Language="VB" AutoEventWireup="false" CodeFile="form-contact.ascx.vb"
    Inherits="controls_layout_form_contact" %>
<%@ Register Src="~/controls/layout/contact.ascx" TagName="contact" TagPrefix="uc1" %>
 <asp:Panel ID="panContact" runat="server" DefaultButton="btnSave">
<div id="trCountry" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Country</div>
    <div class="n-dropdown col-sm-5">
        <asp:DropDownList ID="drCountry" runat="server" AutoPostBack="true" CssClass="form-control" />
        <span class="drp-required"></span>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator ID="rqdrpBillingCountry" runat="server" ControlToValidate="drCountry"
            CssClass="text-danger" Display="Dynamic" ErrorMessage="Country is required" ValidationGroup="valContact"></asp:RequiredFieldValidator>
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
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="First Name is required"
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
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Last Name is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Email Address</div>
    <div class="col-sm-5 txt-required">
        <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="50" CssClass="form-control"
            placeholder="username@hostname.com"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmailAddress"
            CssClass="text-danger" ErrorMessage="Email Address is required" ValidationGroup="valContact"
            Display="Dynamic"></asp:RequiredFieldValidator>
   <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
            CssClass="text-danger" ControlToValidate="txtEmailAddress" Display="Dynamic" ErrorMessage="Email Address is invalid."
            OnServerValidate="ServerCheckValidEmail" ValidateEmptyText="True" ValidationGroup="valContact"></asp:CustomValidator>
    </div>
</div>
<div id="trSalon" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Salon/Company Name</div>
    <div class="col-sm-5">
        <asp:TextBox ID="txtSalonName" runat="server" MaxLength="30" CssClass="form-control"
            placeholder="Salon/Company Name"></asp:TextBox>
    </div>
</div>
<div id="trShippingAddress" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Shipping Address</div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtShipping" runat="server" MaxLength="30" CssClass="form-control"
            placeholder="Shipping Address"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfShippingAddress" ControlToValidate="txtShipping"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Shipping Address is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
    </div>
</div>
<div id="trCity" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        City</div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtCity" runat="server" MaxLength="30" CssClass="form-control" placeholder="City"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="City is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
    </div>
</div>
<div id="trRegion" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Province/Region</div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtRegion" runat="server" MaxLength="30" CssClass="form-control"
            placeholder="Province/Region"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfRegion" ControlToValidate="txtRegion"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Province/Region is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
    </div>
</div>
<div id="trState" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        State</div>
    <div class="n-dropdown col-sm-5">
        <asp:DropDownList ID="drState" runat="server" AutoPostBack="false" CssClass="form-control" />
        <span class="drp-required"></span>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator ID="rqdrpState" runat="server" ControlToValidate="drState"
            CssClass="text-danger" Display="Dynamic" ErrorMessage="State is required" ValidationGroup="valContact"></asp:RequiredFieldValidator>
    </div>
</div>
<div id="trZipCode" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Zip Code</div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtZipCode" runat="server" MaxLength="30" CssClass="form-control"
            placeholder="Zip Code"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfZipCode" ControlToValidate="txtZipCode"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Zip Code is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
    </div>
</div>
<div id="BillingPhone" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Daytime Phone</div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtBillingPhone" runat="server" MaxLength="30" CssClass="form-control"
            type="tel" placeholder="Daytime Phone"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfPhone" ControlToValidate="txtBillingPhone"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Daytime Phone is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <CC:InternationalPhoneValidator ID="regvPhone" runat="server" ControlToValidate="txtBillingPhone"
            CssClass="text-danger" Display="Dynamic" ErrorMessage="Daytime phone number is invalid"
            FrontValidator="False" ValidationGroup="valContact"></CC:InternationalPhoneValidator>
        <asp:CustomValidator ID="cusvPhoneInt" runat="server" ClientValidationFunction="CheckPhoneInt"
            CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic" ErrorMessage="Daytime phone must be at least 10 digit."
            OnServerValidate="ServerCheckPhoneInt" ValidateEmptyText="True" ValidationGroup="valContact"></asp:CustomValidator>
        <asp:CustomValidator ID="cusvPhoneUS" runat="server" ClientValidationFunction="CheckPhoneUS"
            CssClass="text-danger" ControlToValidate="txtBillingPhone" Display="Dynamic" ErrorMessage="Daytime Phone is required."
            OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="True" ValidationGroup="valContact"></asp:CustomValidator>
    </div>
</div>
<div id="trInvoice" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Invoice Number
    </div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtInvoice" runat="server" MaxLength="30" CssClass="form-control"
            placeholder="Invoice Number"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfInvoiceNumber" ControlToValidate="txtInvoice"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Invoice Number is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrInvoice" runat="server"></asp:Literal>
    </div>
</div>
<div id="trDescQty" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs">
    </div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtDescQty" runat="server" MaxLength="30" CssClass="form-control"
            TextMode="Multiline" placeholder="Please describe product & quantity that you are interested"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfDescQty" ControlToValidate="txtDescQty"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Describe product & quantity is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrDescQty" runat="server"></asp:Literal>
    </div>
</div>
<div id="trDescItem" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs">
    </div>
    <div class="col-sm-5  txt-required">
        <asp:TextBox ID="txtDescItem" runat="server" MaxLength="30" CssClass="form-control"
            TextMode="Multiline" placeholder="Item # or Product Description"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfDescItem" ControlToValidate="txtDescItem"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Item # or Product Description is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrDescItem" runat="server"></asp:Literal>
    </div>
</div>
<div id="trItemNotReceive" runat="server">
   <%-- <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5  txt-required">
            <p>
                Once the issue is resolved, would you like your merchandise reshipped or credited?</p>
            <div id="group-radio">
                <asp:RadioButtonList ID="" runat="server" RepeatDirection="Horizontal"
                    CssClass="radio-node">
                    <asp:ListItem Value="1">Reshipped</asp:ListItem>
                    <asp:ListItem Value="0">Credited</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfradCreditedReceived" ControlToValidate="radCreditedReceived"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Your merchandise reshipped or credited is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrCreditedReceived" runat="server"></asp:Literal>
        </div>
    </div>--%>

     <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <p>
                Once the issue is resolved, would you like your merchandise reshipped or credited.</p>
            <div class="group-radio" style="margin-bottom: 10px;">
                <asp:RadioButtonList ID="radCreditedReceived" runat="server" RepeatDirection="Horizontal"
                    CssClass="radio-node">
                    <asp:ListItem Value="1">Reshipped</asp:ListItem>
                    <asp:ListItem Value="0">Credited</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfradCreditedReceived" ControlToValidate="radCreditedReceived"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Your merchandise reshipped or credited is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrCreditedReceived" runat="server"></asp:Literal>
        </div>
    </div>


    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5  txt-required" style="margin-bottom: 10px;">
            <asp:TextBox ID="txtItemNotReceive" runat="server" MaxLength="30" CssClass="form-control"
                TextMode="Multiline" placeholder="Item not received?"></asp:TextBox>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfItemNotReceive" ControlToValidate="txtItemNotReceive"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="'Item not received' is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrItemNotReceive" runat="server"></asp:Literal>
        </div>
    </div>
</div>
<div id="trDamaged" runat="server">
    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <p>
                Is the merchandise Damaged/Broken or Defective?</p>
            <div class="group-radio">
                <asp:RadioButtonList ID="radDamaged" runat="server" RepeatDirection="Horizontal"
                    CssClass="radio-node">
                    <asp:ListItem Value="1">Damaged</asp:ListItem>
                    <asp:ListItem Value="0">Defective</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfradDamaged" ControlToValidate="radDamaged"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="The merchandise Damaged/Broken or Defective is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="lrtradDamaged" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <asp:TextBox ID="txtItemDamaged" runat="server" MaxLength="30" CssClass="form-control"
                TextMode="Multiline" placeholder="What item on your order Damaged/Broken or Defective? "></asp:TextBox>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rftxtItemDamaged" ControlToValidate="txtItemDamaged"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Item on your order Damaged/Broken or Defective is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrItemDamaged" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <asp:TextBox ID="txtDescDamaged" runat="server" MaxLength="30" CssClass="form-control"
                TextMode="Multiline" placeholder=" Please describle how the package was packed?  "></asp:TextBox>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rftxtDescDamaged" ControlToValidate="txtDescDamaged"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Describle how the package was packed is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrDescDamaged" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <p>
                Is there any visible damaged and/or tampering with the carton?
            </p>
            <div class="group-radio">
                <asp:RadioButtonList ID="radYesNoDamaged" runat="server" RepeatDirection="Horizontal"
                    CssClass="radio-node">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfradYesNoDamaged" ControlToValidate="radYesNoDamaged"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="There any visible damaged and/or tampering with the carton is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrYesNoDamaged" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 hidden-xs">
        </div>
        <div class="col-sm-5 txt-required">
            <p>
                Once the issue is resolved, would you like your merchandise reshipped or credited.</p>
            <div class="group-radio" style="margin-bottom: 10px;">
                <asp:RadioButtonList ID="radCreditedDamaged" runat="server" RepeatDirection="Horizontal"
                    CssClass="radio-node">
                    <asp:ListItem Value="1">Reshipped</asp:ListItem>
                    <asp:ListItem Value="0">Credited</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfradCreditedDamaged" ControlToValidate="radCreditedDamaged"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Your merchandise reshipped or credited is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltrCreditedDamaged" runat="server"></asp:Literal>
        </div>
    </div>
</div>
<div id="trItemReturn" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs">
    </div>
    <div class="col-sm-9">
        <div class="div-error1">
           <%-- <asp:RequiredFieldValidator runat="server" ID="rfItemNo" ControlToValidate="txtItem1"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="* Item No is required <br>"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator runat="server" ID="rfQty" ControlToValidate="txtQty1"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="* Quantity is required <br>"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator runat="server" ID="rfReason" ControlToValidate="txtReason1"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="* Reason For Return is required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>
            <asp:CustomValidator ID="cusItemNo" runat="server" ClientValidationFunction="CheckItemNo"
                CssClass="text-danger" ControlToValidate="txtItem1" Display="Dynamic" ErrorMessage="Item No is required."
                ValidateEmptyText="True" ValidationGroup="valContact"></asp:CustomValidator>
        </div>
        <div class="gr-add-item item-no">
            <span class="rt-text">Item No</span> <span class="txt-required"></span>
        </div>
        <div class="gr-add-item item-desc">
            <span class="rt-text">Product Name</span>
        </div>
        <div class="gr-add-item item-qty">
            <span class="rt-text">Quantity</span> <span class="txt-required"></span>
        </div>
        <div class="gr-add-item item-reason">
            <span class="rt-text">Reason For Return</span> 
        </div>
        <div><span class="txt-required"></span></div>
        <div class="gr-ctl">
             <asp:TextBox ID="txtItem1" runat="server" MaxLength="30" CssClass="form-control item-no"
                placeholder="Item No"></asp:TextBox>
              <asp:TextBox ID="txtItemDesc1" runat="server" MaxLength="30" CssClass="form-control item-desc"
                placeholder="Product Name"></asp:TextBox>
             <asp:TextBox ID="txtQty1" runat="server" MaxLength="30" CssClass="form-control item-qty"
                placeholder="Qty"></asp:TextBox>
             <asp:TextBox ID="txtReason1" runat="server" MaxLength="30" CssClass="form-control item-reason"
                placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
             <asp:TextBox ID="txtItem2" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>
             <asp:TextBox ID="txtItemDesc2" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
        <%--     <asp:Literal ID="ltrQty" runat="server"></asp:Literal>--%>
            <asp:TextBox ID="txtQty2" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <%--<asp:Literal ID="ltrReason" runat="server"></asp:Literal>--%>
            <asp:TextBox ID="txtReason2" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtItem3" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>
            <asp:TextBox ID="txtItemDesc3" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
            <asp:TextBox ID="txtQty3" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <asp:TextBox ID="txtReason3" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtItem4" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>     
            <asp:TextBox ID="txtItemDesc4" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
            <asp:TextBox ID="txtQty4" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <asp:TextBox ID="txtReason4" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
                  <div id="divMore" class="text-right">
            <a href="javascript:void(0);" onclick="document.getElementById('tr1').style.display='block'; document.getElementById('divMore').style.display='none';return false;">Add More Items</a> &raquo;</div>
    </div>
    <div class="col-sm-3 hidden-xs">
    </div>
    <div id="tr1" class="col-sm-9" style="display: none;">
        <div class="gr-ctl">
             <asp:TextBox ID="txtItem5" runat="server" MaxLength="30" CssClass="form-control item-no"
                placeholder="Item No"></asp:TextBox>
              <asp:TextBox ID="txtItemDesc5" runat="server" MaxLength="30" CssClass="form-control item-desc"
                placeholder="Product Name"></asp:TextBox>
             <asp:TextBox ID="txtQty5" runat="server" MaxLength="30" CssClass="form-control item-qty"
                placeholder="Qty"></asp:TextBox>
             <asp:TextBox ID="txtReason5" runat="server" MaxLength="30" CssClass="form-control item-reason"
                placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
             <asp:TextBox ID="txtItem6" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>
             <asp:TextBox ID="txtItemDesc6" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
        <%--     <asp:Literal ID="ltrQty" runat="server"></asp:Literal>--%>
            <asp:TextBox ID="txtQty6" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <%--<asp:Literal ID="ltrReason" runat="server"></asp:Literal>--%>
            <asp:TextBox ID="txtReason6" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtItem7" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>
            <asp:TextBox ID="txtItemDesc7" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
            <asp:TextBox ID="txtQty7" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <asp:TextBox ID="txtReason7" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtItem8" runat="server" MaxLength="30" CssClass="form-control item-no" placeholder="Item No"></asp:TextBox>     
            <asp:TextBox ID="txtItemDesc8" runat="server" MaxLength="30" CssClass="form-control item-desc" placeholder="Product Name"></asp:TextBox>
            <asp:TextBox ID="txtQty8" runat="server" MaxLength="30" CssClass="form-control item-qty" placeholder="Qty"></asp:TextBox>
            <asp:TextBox ID="txtReason8" runat="server" MaxLength="30" CssClass="form-control item-reason" placeholder="Reason For Return"></asp:TextBox>
        </div>
    </div>
</div>
<div id="trPriceRequest" runat="server">
    <div class="form-group">
        <div class="col-sm-3 hidden-xs text-right">
            Adjustment Type</div>
        <div class="col-sm-5  txt-required">
             <div class="group-radio">
                <asp:RadioButtonList ID="radAdjustmentType" runat="server" RepeatDirection="Horizontal" CssClass="radio-node">
                    <asp:ListItem Value="1">Sales Price</asp:ListItem>
                    <asp:ListItem Value="2">Promo Code</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <asp:RequiredFieldValidator runat="server" ID="rfAdjustmentType" ControlToValidate="radAdjustmentType"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Adjustment Type is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrAdjustmentType" runat="server"></asp:Literal>
    </div>
    <div class="form-group">
        <div class="col-sm-3 hidden-xs text-right">
            Item(s) to Adjust</div>
        <div class="col-sm-5  txt-required" style="margin-bottom:10px;">
            <asp:TextBox ID="txtItemAdjust" runat="server" MaxLength="30" CssClass="form-control"
                TextMode="Multiline" placeholder="Ex: 119000 - Artisan nail glue"></asp:TextBox>
        </div>
        <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfItemAdjust" ControlToValidate="txtItemAdjust"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Item(s) to Adjust is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrItemAdjust" runat="server"></asp:Literal>
    </div>
    </div>
</div>
<div id="trComments" runat="server" class="form-group">
    <div class="col-sm-3 hidden-xs text-right">
        Comments</div>
    <div class="col-sm-5 txt-required">
        <asp:TextBox ID="txtComments" runat="server" MaxLength="30" CssClass="form-control"
            TextMode="Multiline" placeholder="Comments"></asp:TextBox>
    </div>
    <div class="div-error">
        <asp:RequiredFieldValidator runat="server" ID="rfvComments" ControlToValidate="txtComments"
            Display="Dynamic" ValidationGroup="valContact" ErrorMessage="Comments is required"
            CssClass="text-danger"></asp:RequiredFieldValidator>
        <asp:Literal ID="ltrComments" runat="server"></asp:Literal>
    </div>
</div>
<div id="trTypecode" runat="server">
    <div class="form-group">
        <div class="col-sm-3 hidden-xs text-right">
            <span id="labeltxtCaptcha" runat="server">Type the code shown</span>
        </div>
        <div class="col-sm-5  txt-required">
            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="142" AutoCompleteType="Disabled"
                placeholder="Type the code shown" autocomplete="off" CssClass="form-control" />
        </div>
        <div class="div-error">
            <asp:RequiredFieldValidator runat="server" ID="rfvCaptcha" ControlToValidate="txtCaptcha"
                Display="Dynamic" ValidationGroup="valContact" ErrorMessage="The code shown required"
                CssClass="text-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="ltCapcha" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 text-right">
        </div>
        <div class="col-sm-5">
            <img src="/members/captcha.aspx" alt="" runat="server" id="imgCaptcha" />
        </div>
    </div>
</div>
<div class="form-group">
    <div class="col-sm-3 text-right">
    </div>
    <div class="col-sm-8 content">
        <asp:Button runat="server" ID="btnSave" data-btn="submit" Text="Submit" CssClass="btn btn-submit"
            CausesValidation="true" ValidationGroup="valContact" /><br />
    </div>
</div>
<div class="form-group">
    <div class="col-sm-3 text-right">
    </div>
    <div class="col-sm-8">
        <uc1:contact ID="ucNeedUs" runat="server" />
    </div>
</div>
</asp:Panel>
<script type="text/javascript">
    $(window).load(function () {
        if ($("#drCountry").val() == "US") {
            //$("#txtBillingPhone").mask("?999-999-9999 9999");
            fnFormatPhoneUS('#txtBillingPhone','', '');
        }
    });

    $('#txtBillingPhone').keyup(function (e) {
        if ($("#drCountry").val() == "US") {
           fnFormatPhoneUS(this, e, this.id);
        }

    });
    function is_int(value) {
        for (i = 0; i < value.length; i++) {
            if ((value.charAt(i) < '0') || (value.charAt(i) > '9')) return false
        }
        return true;
    }
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
                        document.getElementById('cusvPhoneInt').innerHTML = 'Daytime phone must be at least 10 digit.';
                        return;
                    }
                }
            }
        }
    }
    function CheckPhoneUS(sender, args) {
        var phone = fnPhoneUS(document.getElementById('txtBillingPhone').value, 'BillingPhone');
       // var phone = document.getElementById('txtBillingPhone').value;
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
                document.getElementById('cusvPhoneUS').innerHTML = 'Daytime phone number is invalid';
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
                document.getElementById('cusvPhoneUS').innerHTML = 'Daytime phone number is invalid';
            }
        }
        catch (e) {
            args.IsValid = false;
            document.getElementById('cusvPhoneUS').innerHTML = 'Daytime phone number is invalid';
            return;
        }

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
    function CheckItemNo(sender, args) {
        var hasOne = false;
        for (var i = 1; i < 9; i++) {
            var itemno = document.getElementById('txtItem' + i).value;
            var qty = document.getElementById('txtQty' + i).value;
            var reason = document.getElementById('txtReason' + i).value;
            if (itemno != '') {
                hasOne = true;
                if (qty == '') {
                    args.IsValid = false;
                    document.getElementById('cusItemNo').innerHTML = 'Quantity #' + i + ' is required';
                    return;
                }
                else if (!is_int(qty) || qty < 1) {
                    args.IsValid = false;
                    document.getElementById('cusItemNo').innerHTML = 'Quantity #' + i + ' is invalid';
                    return;
                }
                if (reason == '') {
                    args.IsValid = false;
                    document.getElementById('cusItemNo').innerHTML = 'Reason For Return #' + i + ' is required';
                    return;
                }
            }
        }
        if (!hasOne) {
            document.getElementById("cusItemNo").innerHTML = 'Please specify at least one item and quantity and reason.';
            args.IsValid = false;
            return;
        }
    }
</script>
<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>
