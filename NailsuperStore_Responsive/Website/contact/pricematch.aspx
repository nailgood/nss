<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pricematch.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"
    Inherits="pricematch_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
      <asp:Panel ID="panPriceMatch" runat="server" DefaultButton="btnSave">
        <div class="panel-content">
            <h1 class="formtitle">
                Price Match Guarantee</h1>
            <div class="content">
                <div class="borderpriceMatch">
                    Thank you for shopping at The Nail Superstore. We keep our best effort to make sure
                    you get your products at the most competitive prices. In an unlikely event that
                    you find our competitors offering the same product but at a lower cost, please let
                    us know by filling out the following form. Restrictions apply**
                </div>
                <h3 class="mag">
                    Price Match Request</h3>
                <br />
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Your Name</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtYourName" runat="server" MaxLength="30" CssClass="form-control"
                            placeholder="First Name"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfvYName" ControlToValidate="txtYourName"
                            Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="Your Name is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        Phone Number</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="30" CssClass="form-control"
                            type="tel" placeholder="Phone Number"></asp:TextBox>
                    </div>
                    <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rfPhone" ControlToValidate="txtPhoneNumber"
                            Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="Phone Number is required"
                            CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvPhoneUS" runat="server" ClientValidationFunction="CheckPhoneUS"
                            CssClass="text-danger" ControlToValidate="txtPhoneNumber" Display="Dynamic" ErrorMessage="Phone Number is required."
                            OnServerValidate="ServerCheckPhoneUS" ValidateEmptyText="True" ValidationGroup="valPriceMatch"></asp:CustomValidator>
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
                        <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmailAddress"
                            CssClass="text-danger" ErrorMessage="Email Address is required" ValidationGroup="valPriceMatch"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
                            CssClass="text-danger" ControlToValidate="txtEmailAddress" Display="Dynamic"
                            ErrorMessage="Email Address is invalid." OnServerValidate="ServerCheckValidEmail"
                            ValidateEmptyText="True" ValidationGroup="valPriceMatch"></asp:CustomValidator>
                    </div>
                </div>
                <div id="trPriceMatch" class="form-group">
                    <div class="col-sm-3 hidden-xs">
                    </div>
                    <div class="col-sm-9">
                        <div class="div-error1">
                           <%--<asp:RequiredFieldValidator runat="server" ID="rfItemNo" ControlToValidate="tbItem1"
                                Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="* Item No is required <br>"
                                CssClass="text-danger"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator runat="server" ID="rfQty" ControlToValidate="tbPrice1"
                                Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="* Price is required <br>"
                                CssClass="text-danger"></asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator ID="cusItem" runat="server" ClientValidationFunction="CheckItem"
                            CssClass="text-danger" ControlToValidate="tbItem1" Display="Dynamic"
                            ErrorMessage="Please specify at least one item and price." OnServerValidate="ServerCheckItem"
                            ValidateEmptyText="True" ValidationGroup="valPriceMatch"></asp:CustomValidator>
                        </div>
                        <div class="col-xs-7">
                            Item # or Product Name <span class="red">*</span>
                            <asp:TextBox ID="tbItem1" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem2" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem3" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem4" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem5" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                        </div>
                        <div class="col-xs-5">
                            Competitor Price <span class="red">*</span>
                            <asp:TextBox ID="tbPrice1" runat="server" MaxLength="30" CssClass="form-control lblAsk" 
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice2" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice3" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice4" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice5" runat="server" MaxLength="30" CssClass="form-control"
                                placeholder="Competitor Price"></asp:TextBox>
                            
                        </div>
                        <div id="divMore" class="text-right"><a href="javascript:void(0);" onclick="document.getElementById('tr1').style.display='block'; document.getElementById('divMore').style.display='none';return false;">Add More Items</a> &raquo;</div>
                    </div>
                    <div class="col-sm-3 hidden-xs">
                    </div>
                    <div id="tr1" class="col-sm-9" style="display: none;">
                       <div class="col-xs-7">
                            <asp:TextBox ID="tbItem6" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem7" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem8" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem9" runat="server" MaxLength="30" CssClass="form-control lblAsk" placeholder="Item # or Product Name"></asp:TextBox>
                            <asp:TextBox ID="tbItem10" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Item # or Product Name"></asp:TextBox>
                        </div>
                         <div class="col-xs-5">
                            <asp:TextBox ID="tbPrice6" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice7" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice8" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice9" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                            <asp:TextBox ID="tbPrice10" runat="server" MaxLength="30" CssClass="form-control lblAsk"
                                placeholder="Competitor Price"></asp:TextBox>
                        </div>
                    </div>
                </div>
            <div class="form-group">
                <div class="col-sm-3 hidden-xs text-right">
                    Competitor's Company Name</div>
                <div class="col-sm-5  txt-required">
                    <asp:TextBox ID="txtCompetitorsCompanyName" runat="server" MaxLength="30" CssClass="form-control"
                        placeholder="Competitor's Company Name"></asp:TextBox>
                </div>
                <div class="div-error">
                    <asp:RequiredFieldValidator runat="server" ID="rfCompetitorsCompanyName" ControlToValidate="txtCompetitorsCompanyName"
                        Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="Competitor's Company Name is required"
                        CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 hidden-xs text-right">
                    Competitor's Phone Number</div>
                <div class="col-sm-5  txt-required">
                    <asp:TextBox ID="txtCompetitorsPhoneNumber" runat="server" MaxLength="30" CssClass="form-control"
                        placeholder="Competitor's Phone Number"></asp:TextBox>
                </div>
                <div class="div-error">
                    <asp:RequiredFieldValidator runat="server" ID="rfCompetitorsPhoneNumber" ControlToValidate="txtCompetitorsPhoneNumber"
                        Display="Dynamic" ValidationGroup="valPriceMatch" ErrorMessage="Competitor's Phone Number is required"
                        CssClass="text-danger"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cusPhoneUS2" runat="server" ClientValidationFunction="CheckPhoneUS2"
                        CssClass="text-danger" ControlToValidate="txtCompetitorsPhoneNumber" Display="Dynamic"
                        ErrorMessage="Competitor's Phone Number is required." OnServerValidate="ServerCheckPhoneUS2"
                        ValidateEmptyText="True" ValidationGroup="valPriceMatch"></asp:CustomValidator>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 hidden-xs text-right">
                    Competitor's Website</div>
                <div class="col-sm-5">
                    <asp:TextBox ID="txtCompetitorsWebsite" runat="server" MaxLength="30" CssClass="form-control"
                        placeholder="Competitor's Website"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 text-right">
                </div>
                <div class="col-sm-8 content">
                    <asp:Button runat="server" ID="btnSave" data-btn="submit" Text="Submit"  CausesValidation="true" ValidationGroup="valPriceMatch" CssClass="btn btn-submit" />
                </div>
            </div>
            <%-- <asp:Panel runat="server" id="pnlForm">
                                    <table border="0" cellspacing="1" cellpadding="1" style="height: 100%; padding-left: 0px">
                                        <tr>
                                            <td colspan="2">
                                                (<span class="red">*</span> indicates required fields)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Your Name<span class="red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtYourName" runat="server" maxlength="100" columns="50" style="width: 200px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvYourName" runat="server" enableclientscript="false"
                                                    Display="none" ControlToValidate="txtYourName" ErrorMessage="Field 'Your Name' is blank">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Phone Number<span class="red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtPhoneNumber" runat="server" maxlength="50" columns="50" style="width: 200px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" enableclientscript="false"
                                                    Display="none" ControlToValidate="txtPhoneNumber" ErrorMessage="Field 'Phone Number' is blank">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Email Address<span class="red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtEmailAddress" runat="server" maxlength="100" columns="50" style="width: 200px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" enableclientscript="false"
                                                    Display="none" ControlToValidate="txtEmailAddress" ErrorMessage="Field 'Email Address' is blank">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding: 0px;">
                                                <table border="0" cellspacing="1" cellpadding="2">
                                                    <tr>
                                                        <td class="bold">
                                                            Item # or Product Name<span class="red">*</span><span style="padding-left: 195px">Competitor
                                                                Price<span class="red">*</span></span>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem1" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice1" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td width="100px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator1" runat="server" ControlToValidate="tbPrice1"
                                                                ErrorMessage="Please enter a valid price for item #1" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem2" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice2" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator2" runat="server" ControlToValidate="tbPrice2"
                                                                ErrorMessage="Please enter a valid price for item #2" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem3" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice3" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator3" runat="server" ControlToValidate="tbPrice3"
                                                                ErrorMessage="Please enter a valid price for item #3" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem4" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice4" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator4" runat="server" ControlToValidate="tbPrice4"
                                                                ErrorMessage="Please enter a valid price for item #4" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem5" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice5" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                            <div id="divMore" style="padding-left: 340px">
                                                                <a href="javascript:void(0);" onclick="for(var i = 1; i <= 5; i++) {document.getElementById('tr' + i).style.display='block';} document.getElementById('divMore').style.display='none';document.getElementById('divLess').style.display='block';return false;"
                                                                    class="lnkAccount">Add More Items</a> &raquo;</div>
                                                            <CC:FloatValidator ID="FloatValidator5" runat="server" ControlToValidate="tbPrice5"
                                                                ErrorMessage="Please enter a valid price for item #5" Display="none" EnableClientScript="False" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="tr1" style="display: none">
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem6" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice6" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator6" runat="server" ControlToValidate="tbPrice6"
                                                                ErrorMessage="Please enter a valid price for item #6" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tr2" style="display: none">
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem7" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice7" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator7" runat="server" ControlToValidate="tbPrice7"
                                                                ErrorMessage="Please enter a valid price for item #7" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tr3" style="display: none">
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem8" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice8" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator8" runat="server" ControlToValidate="tbPrice8"
                                                                ErrorMessage="Please enter a valid price for item #8" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tr4" style="display: none">
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem9" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice9" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator9" runat="server" ControlToValidate="tbPrice9"
                                                                ErrorMessage="Please enter a valid price for item #9" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tr5" style="display: none">
                                                        <td>
                                                            <asp:TextBox runat="server" id="tbItem10" maxlength="100" columns="50" style="width: 319px;" />
                                                            &nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPrice10" MaxLength="10" Columns="20"
                                                                style="width: 100px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <CC:FloatValidator ID="FloatValidator10" runat="server" ControlToValidate="tbPrice10"
                                                                ErrorMessage="Please enter a valid price for item #10" Display="none" EnableClientScript="False" />
                                                        </td>
                                                    </tr>
                                                    <div id="divLess">
                                                        <!--&laquo; <a href="javascript:void(0);" onclick="for(var i = 1; i <= 5; i++) {document.getElementById('tr' + i).style.display='none';} document.getElementById('divMore').style.display='block';document.getElementById('divLess').style.display='none';return false;">Show Fewer Items</a>-->
                                                    </div>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Competitor's Company Name<span class="red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtCompetitorsCompanyName" runat="server" maxlength="100" columns="50"
                                                    style="width: 319px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvCompetitorsCompanyName" enableclientscript="false"
                                                    runat="server" Display="none" ControlToValidate="txtCompetitorsCompanyName" ErrorMessage="Field 'Competitor's Company Name' is blank">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Competitor's Phone Number<span class="red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtCompetitorsPhoneNumber" runat="server" maxlength="50" columns="50"
                                                    style="width: 319px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvCompetitorsPhoneNumber" enableclientscript="false"
                                                    runat="server" Display="none" ControlToValidate="txtCompetitorsPhoneNumber" ErrorMessage="Field 'Competitor's Phone Number' is blank">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="bold">
                                                Competitor's Website
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:textbox id="txtCompetitorsWebsite" runat="server" maxlength="100" columns="50"
                                                    style="width: 319px;">
                                                </asp:textbox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="padding-left: 5px; padding-bottom: 5px;">
                                        <asp:Button id="btnSave" runat="server" CssClass="btns100" alternatetext="Price match"
                                            Text="Submit" />
                                </asp:Panel>
                                <asp:Literal runat="server" ID="lit" />
            --%>
           
                <div class="mag" style="font-size: 18px;">
                    Price Match Terms:</div>
                <div class="textService">
                    The Nail Superstore Price Match Guarantee does not apply to competitors' free offers,
                    limited quantity items, open-box, clearance or closeout products, mail-in incentives,
                    financing or bundle offers. The policy does not apply to typographical errors or
                    a competitor's price that result from a price match.<br />
                    Price match will meet exact same shipping, promotion or competitors policy.<br />
                    Price match is not retroactive and must be requested prior to place the order.
                </div>
             </div>
        </div>
    </asp:Panel>
    </div>
    <script type="text/javascript">
        function pageLoad() {
//            $("#txtPhoneNumber").mask("?999-999-9999 9999");
            //            $("#txtCompetitorsPhoneNumber").mask("?999-999-9999 9999");
            fnFormatPhoneUS('#txtPhoneNumber', '', 'txtPhoneNumber');
            fnFormatPhoneUS('#txtCompetitorsPhoneNumber', '', 'txtCompetitorsPhoneNumber');
        }
        $('#txtPhoneNumber, #txtCompetitorsPhoneNumber').keyup(function (e) {
                 fnFormatPhoneUS(this, e, this.id);
        });
     
        function CheckItem(sender, args) {
            var hasOne = false;
            if (document.getElementById("tbItem1").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice1").value == '') {                    
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #1';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem2").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice2").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #2';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem3").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice3").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #3';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem4").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice4").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #4';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem5").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice5").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #5';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem6").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice6").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #6';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem7").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice7").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #7';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem8").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice8").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #8';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem9").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice9").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #9';
                    args.IsValid = false;
                    return;
                }
            }
            if (document.getElementById("tbItem10").value != '') {
                hasOne = true;
                if (document.getElementById("tbPrice10").value == '') {
                    document.getElementById("cusItem").innerHTML = 'Please specify a price for item #10';
                    args.IsValid = false;
                    return;
                }
            }
            if(!hasOne){
                document.getElementById("cusItem").innerHTML = 'Please specify at least one item and price.';
                args.IsValid = false;
                return;
            }
        }
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
        function CheckPhoneUS(sender, args) {
            var phone = document.getElementById('txtPhoneNumber').value;
            if (phone == '') {
                return;
            }
            var phone1, phone2, phone3;
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
                    document.getElementById('cusvPhoneUS').innerHTML = 'Phone Number is invalid';
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
                    document.getElementById('cusvPhoneUS').innerHTML = 'Phone Number is invalid';
                }
            }
            catch (e) {
                args.IsValid = false;
                document.getElementById('cusvPhoneUS').innerHTML = 'Phone Number is invalid';
                return;
            }
        }
        function CheckPhoneUS2(sender, args) {
            var phone = document.getElementById('txtCompetitorsPhoneNumber').value;
            if (phone == '') {
                return;
            }
            var phone1, phone2, phone3;
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
                    document.getElementById('cusPhoneUS2').innerHTML = 'Competitors Phone Number is invalid';
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
                    document.getElementById('cusPhoneUS2').innerHTML = 'Competitors Phone Number is invalid';
                }
            }
            catch (e) {
                args.IsValid = false;
                document.getElementById('cusPhoneUS2').innerHTML = 'Competitors Phone Number is invalid';
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
    </script>
    <%--<script type="text/javascript" src="/includes/scripts/jquery.maskedinput.js"></script>Edit css.xml--%>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
</asp:Content>
