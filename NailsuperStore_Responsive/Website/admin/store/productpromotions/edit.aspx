<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="Edit" Title="Store Promotion" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <h4>
                <% If PromotionId = 0 Then%>Add<% Else%>Edit<% End If%> Product Coupon</h4>
            <table border="0" cellspacing="1" cellpadding="2">
            <tr>
                    <td colspan="2">
                        <span class="red">
                            <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Promotion Name:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPromotionName" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPromotionName" runat="server" Display="Dynamic"
                            ControlToValidate="txtPromotionName" CssClass="msgError" ErrorMessage="Field 'Promotion Name' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Promotion Code:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPromotionCode" runat="server" MaxLength="50" Columns="50" Style="width: 319px;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPromotionCode" runat="server" Display="Dynamic"
                            ControlToValidate="txtPromotionCode" CssClass="msgError" ErrorMessage="Field 'Promotion Code' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Promotion Type:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="drpPromotionType" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvdrpPromotionType" runat="server" Display="Dynamic"
                            ControlToValidate="drpPromotionType" CssClass="msgError" ErrorMessage="Field 'Promotion Type' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Message:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMessage" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvMessage" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtMessage"
                            ErrorMessage="Field 'Message' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trMixMatchNo" runat="server">
                    <td class="required">
                        Mixmatch No:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMixMatchNo" Enabled="false" runat="server" MaxLength="20" Style="width: 80px;"> 
		
                        </asp:TextBox>
                        <input type="button" class="btn" id="btnAddMixMatch" value="Add MixMatch" onclick="OpenPopUpMixMatch();" />
                        <br />
                        <span id="mixmatchName" runat="server" style="color: #BE048D; padding: 0px 0px 2px 4px;
                            float: left; margin-top: 7px;">Name MM</span>
                        <asp:LinkButton ID="lblGetMMDetail" CausesValidation="false" runat="server"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvMixMatchNo" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtMixMatchNo"
                            ErrorMessage="Field 'Mixmatch No' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trDiscount" runat="server">
                    <td class="required">
                        Discount:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDiscount" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvDiscount" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtDiscount"
                            ErrorMessage="Field 'Discount' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
               
                <tr>
                    <td class="required">
                        Start Date:
                    </td>
                    <td class="field">
                        <CC:DatePicker ID="dtStartDate" runat="server"></CC:DatePicker>
                    </td>
                    <td>
                        <CC:RequiredDateValidator Display="Dynamic" CssClass="msgError" runat="server" ID="rdtvStartDate" ControlToValidate="dtStartDate"
                            ErrorMessage="Date Field 'Start Date' is blank" /><CC:DateValidator Display="Dynamic" CssClass="msgError"
                                runat="server" ID="dtvStartDate" ControlToValidate="dtStartDate" ErrorMessage="Date Field 'Start Date' is invalid" />
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        End Date:
                    </td>
                    <td class="field">
                        <CC:DatePicker ID="dtEndDate" runat="server"></CC:DatePicker>
                    </td>
                    <td>
                        <CC:RequiredDateValidator Display="Dynamic" CssClass="msgError" runat="server" ID="rdtvEndDate" ControlToValidate="dtEndDate"
                            ErrorMessage="Date Field 'End Date' is blank" /><CC:DateValidator Display="Dynamic" CssClass="msgError"
                                runat="server" ID="dtvEndDate" ControlToValidate="dtEndDate" ErrorMessage="Date Field 'End Date' is invalid" />
                    </td>
                </tr>
                 <tr >
                    <td class="optional">
                        Customer Price Group:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="drlCustomerPriceGroup" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Min Dollards:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMinimumPurchase" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Max Dollards:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMaximumPurchase" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Min Quantity:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMinimumQuantityPurchase" runat="server" MaxLength="8" Columns="8"
                            Style="width: 67px;"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Image File:<br />
                        <span class="smaller"></span>
                    </td>
                    <td class="field">
                        <CC:FileUpload ID="fuImage" AutoResize="true" Folder="/assets/coupon" ImageDisplayFolder="/assets/coupon"
                            DisplayImage="False" runat="server" Style="width: 319px;" />
                        <div runat="server" id="divImg">
                            <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                            <div>
                                <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                            <span style="color: Red">
                                <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
                        </div>
                    </td>
                    <td>
                        <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage" CssClass="msgError"
                            runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                    </td>
                </tr>
                <tr visible="false" runat="server">
                    <td class="optional">
                        <b>Is Item Specific?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsItemSpecific" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Free Ground Shipping?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsFreeShipping" />
                    </td>
                </tr>
                <tr runat="server" id="trOneUse">
                    <td class="optional">
                        <b>Is One Use?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsOneUse" />
                    </td>
                </tr>
                <tr runat="server" id="trUsed">
                    <td class="optional">
                        <b>Is Used?</b><br />
                        <span class="smaller red">Setting as "used" is irreversible!</span>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsUsed" />
                        <asp:Label runat="server" ID="lblUsed" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Active?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Total Product?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsTotalProduct" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Register Send?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsRegisterSend" />
                        <asp:CheckBox runat="server" ID="chkIsProductCoupon" Checked="true" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Item
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtSKU" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"
                            Enabled="false"></asp:TextBox>
                            
                        <input type="button" class="btn" id="btnAddSKU" runat="server" value="Add SKU" onclick="OpenPopUp();" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="optional">
                        <b>Pricing Type:</b>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="drpPricingType" runat="server">
                            <asp:ListItem Value="">No specific pricing type</asp:ListItem>
                            <asp:ListItem Value="General">General Member Only</asp:ListItem>
                            <asp:ListItem Value="Associate">Associate Only</asp:ListItem>
                            <asp:ListItem Value="Distributor">Distributor Only</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <p>
            </p>
            <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
            <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Store Promotion?"
                Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
            <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
            </CC:OneClickButton>
            <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
            <input type="hidden" runat="server" value="" id="hidOldSKU" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <script>
        function GetMixmatchInfor() {
            __doPostBack('ctl00$ph$lblGetMMDetail', '');

        }
        function OpenPopUpMixMatch() {

            var txtMixMatchNo = document.getElementById('<%=txtMixMatchNo.ClientID %>').value;
            var url = 'searchmixmatch.aspx?no=' + txtMixMatchNo
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            // document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
            //document.getElementById('<%=txtSKU.ClientID %>').value = p;
            var brow = GetBrowser();
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '')
                        document.getElementById('<%=txtMixMatchNo.ClientID %>').value = p;
                    GetMixmatchInfor();

                }
            }


        }
        function OpenPopUp() {

            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var sku = document.getElementById('<%=txtSKU.ClientID %>').value;
            var url = '../../promotions/ShopSave/SearchItem.aspx?Type=0&F_HasSalesPrice=0&item=' + item
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            // document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
            //document.getElementById('<%=txtSKU.ClientID %>').value = p;
            var brow = GetBrowser();
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '')
                        document.getElementById('<%=txtSKU.ClientID %>').value = p;
                    document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                }
            }


        }
        function SetValueMM(save, value) {
            if (save == '1') {
                if (value != '') {
                    document.getElementById('<%=txtMixMatchNo.ClientID %>').value = value;
                    GetMixmatchInfor()
                }
            }


        }

        function SetValue(save, value) {
            if (save == '1') {
                if (value != '') {
                    document.getElementById('<%=txtSKU.ClientID %>').value = value;
                    document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                }
            }


        }
        function ChangeType(selectValue) {
            var trMixMatchNo = document.getElementById('<%=trMixMatchNo.ClientID %>');
            var trDiscount = document.getElementById('<%=trDiscount.ClientID %>');
            var rfvDiscount = document.getElementById('<%=rfvDiscount.ClientID %>');
            var rfvMixMatchNo = document.getElementById('<%=rfvMixMatchNo.ClientID %>');
            var drlCustomerPriceGroup = document.getElementById('<%=drlCustomerPriceGroup.ClientID %>');
            if (selectValue == 'Freeitem') {
                trMixMatchNo.style.display = '';
                trDiscount.style.display = 'none';
                ValidatorEnable(rfvMixMatchNo, true);
                ValidatorEnable(rfvDiscount, false);
                //  drlCustomerPriceGroup.disabled = 'disabled';

            }
            else {
                trMixMatchNo.style.display = 'none';
                trDiscount.style.display = '';
                ValidatorEnable(rfvMixMatchNo, false);
                ValidatorEnable(rfvDiscount, true);
                // drlCustomerPriceGroup.disabled = '';
            }
        }
    </script>

</asp:Content>
