<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="Edit" Title="Store Promotion" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <h4>
                <% If PromotionId = 0 Then%>Add<% Else%>Edit<% End If%> Order Coupon</h4>
            <table border="0" cellspacing="1" cellpadding="2">
                <tr>
                    <td colspan="2">
                        <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span class="red">
                            <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                        </span>
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
                        <asp:RequiredFieldValidator ID="rfvPromotionName" CssClass="msgError" runat="server" Display="Dynamic"
                            ControlToValidate="txtPromotionName" ErrorMessage="Field 'Promotion Name' is blank"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator ID="rfvPromotionCode" runat="server" Display="Dynamic" CssClass="msgError"
                            ControlToValidate="txtPromotionCode" ErrorMessage="Field 'Promotion Code' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Promotion Type:
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="drpPromotionType" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvdrpPromotionType" runat="server" Display="Dynamic" CssClass="msgError"
                            ControlToValidate="drpPromotionType" ErrorMessage="Field 'Promotion Type' is blank"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtMessage"
                            ErrorMessage="Field 'Message' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trFreeItem" runat="server">
                    <td class="required">
                        Free Item:
                    </td>
                    <td class="field">
                        <input type="button" class="btn" id="btnAddItem" value="Add Free Item" onclick="OpenPopUpItem();" />
                        <asp:Literal ID="ltrFreeItem" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <span style="color: Red;" id="rfvFreeItem" runat="server" visible="false">Field 'Free
                            Item' is blank</span>
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
                        <asp:RequiredFieldValidator ID="rfvDiscount" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtDiscount"
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
                        <CC:RequiredDateValidator Display="Dynamic" runat="server" ID="rdtvStartDate" ControlToValidate="dtStartDate" CssClass="msgError"
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
                        <CC:RequiredDateValidator Display="Dynamic" runat="server" ID="rdtvEndDate" ControlToValidate="dtEndDate" CssClass="msgError"
                            ErrorMessage="Date Field 'End Date' is blank" /><CC:DateValidator Display="Dynamic" CssClass="msgError"
                                runat="server" ID="dtvEndDate" ControlToValidate="dtEndDate" ErrorMessage="Date Field 'End Date' is invalid" />
                    </td>
                </tr>
                <tr>
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
                        Minimum Purchase:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMinimumPurchase" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Maximum Purchase:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMaximumPurchase" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
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
                        <asp:CheckBox runat="server" ID="chkIsActive" />
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Register Send?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsRegisterSend" />
                        <asp:CheckBox runat="server" ID="chkIsProductCoupon" Visible="false" />
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
            <input type="hidden" runat="server" value="" id="hidSaveValue" />
            <input type="hidden" runat="server" value="" id="hidSelectSKU" />
            <input type="hidden" runat="server" value="" id="hidDeleteSKU" />
            <div style="display: none">
                <CC:OneClickButton ID="GetSKU" CausesValidation="false" runat="server" Text="Get Product Infor"
                    CssClass="btn"></CC:OneClickButton></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <script type="text/javascript">


        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;

        }
        function OpenPopUpItem() {
            var brow = GetBrowser();
            var item = document.getElementById('<%=hidSelectSKU.ClientID %>').value;
            var url = '/admin/promotions/ShopSave/SearchItem.aspx?Type=1&item=' + item
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                        var button = document.getElementById('<%=GetSKU.ClientID %>');
                        if (button)
                            button.click();
                    }
                }
            }
            else {
                var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
                if (saveValue == '1') {
                    var button = document.getElementById('<%=GetSKU.ClientID %>');
                    if (button)
                        button.click();
                }
            }

        }
        function DeleteFreeItem(sku) {
            var yes = confirm('Are you sure that you want to remove this item?');
            if (yes) {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = '';
                document.getElementById('<%=hidDeleteSKU.ClientID %>').value = sku
                var button = document.getElementById('<%=GetSKU.ClientID %>');
                if (button)
                    button.click();
            }
        }
    </script>

</asp:Content>
