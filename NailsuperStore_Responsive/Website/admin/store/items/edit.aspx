<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="" CodeFile="edit.aspx.vb" Inherits="admin_store_items_edit" ValidateRequest="false" %>

<%@ Register TagPrefix="Ctrl" TagName="ImageDropDown" Src="~/controls/ImageDropDown.ascx" %>
<%@ Register TagPrefix="Ctrl" TagName="DepartmentTree" Src="~/controls/DepartmentTree.ascx" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
        function CheckAccepting(type) {
            if (type == 2) { //Acepting Order
                document.getElementById('<%=chkInStock.ClientID %>').checked=false;
            }
            else
                document.getElementById('<%=chkIsAcceptingOrder.ClientID %>').checked = false;
        }
        function CheckPointValid(sender, args) {


            if (args.Value == '') {
                args.IsValid = false;
                return;
            }
            var point = parseInt(args.Value)
            if (point.toString().length != args.Value.length) {
                args.IsValid = false;
                return;
            }
            if (isNaN(point)) {
                args.IsValid = false;
                return;
            }
            if (point < 1) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
        function CheckRewardPoint(chk) {
            var cusvRewardPoint = document.getElementById('<%=cusvRewardPoint.ClientID %>');
            if (chk.checked) {
                document.getElementById('<%=txtRewardPoint.ClientID %>').disabled = false;
                ValidatorEnable(cusvRewardPoint, true);
                document.getElementById('<%=hidIsRewardPoint.ClientID %>').value = "1";
                document.getElementById('<%=txtRewardPoint.ClientID %>').className = ''
            }
            else {
                document.getElementById('<%=txtRewardPoint.ClientID %>').className = 'txtDisable'
                document.getElementById('<%=txtRewardPoint.ClientID %>').disabled = true;
                document.getElementById('<%=txtRewardPoint.ClientID %>').value = '';
                ValidatorEnable(cusvRewardPoint, false);
                document.getElementById('<%=hidIsRewardPoint.ClientID %>').value = "0";
            }
        }
        function CheckEbayPrice(sender, args) {

            if (args.Value == '') {
                args.IsValid = false;
                return;
            }
            var EbayPrice = parseFloat(args.Value)
            if (EbayPrice.toString().length != args.Value.length) {
                args.IsValid = false;
                return;
            }
            if (isNaN(EbayPrice)) {
                args.IsValid = false;
                return;
            }
            if (EbayPrice < 1) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
        function CheckAllowEbay(chk) {
            var cusEbayPrice = document.getElementById('<%=cusEbayPrice.ClientID %>');
            if (chk.checked) {
                document.getElementById('<%=txtEbayPrice.ClientID %>').disabled = false;
                ValidatorEnable(cusEbayPrice, true);
                document.getElementById('<%=hdEbayPrice.ClientID %>').value = "1";
                document.getElementById('<%=txtEbayPrice.ClientID %>').className = ''
            }
            else {
                document.getElementById('<%=txtEbayPrice.ClientID %>').className = 'txtDisable'
                document.getElementById('<%=txtEbayPrice.ClientID %>').disabled = true;
                document.getElementById('<%=txtEbayPrice.ClientID %>').value = '';
                ValidatorEnable(cusEbayPrice, false);
                document.getElementById('<%=hdEbayPrice.ClientID %>').value = "0";
            }
        }
        <%--function ChangeIsOverSize(value) {
            var pnFlatFee = document.getElementById('pnFlatFee');
            if (value) {
                if (pnFlatFee)
                    pnFlatFee.style.display = '';
            }
            else {
                if (pnFlatFee) {
                    pnFlatFee.style.display = 'none';
                    document.getElementById('<%=chkIsFlatFee.ClientID %>').checked = false;
                    document.getElementById('linkSetupFlatFee').style.display = 'none';
                }
            }

        }

        function ChangeIsFlatFee(value) {
            if (value) {
                document.getElementById('linkSetupFlatFee').style.display = '';
            }
            else
                document.getElementById('linkSetupFlatFee').style.display = 'none';
        }
        function ShowPanelFlatFee(showFlatFee, showLinkSetup) {

            var pnFlatFee = document.getElementById('pnFlatFee');
            if (showFlatFee == '1') {
                pnFlatFee.style.display = '';
                if (showLinkSetup == '1')
                    document.getElementById('linkSetupFlatFee').style.display = '';
                else document.getElementById('linkSetupFlatFee').style.display = 'none';
            }
            else {
                pnFlatFee.style.display = 'none';
                document.getElementById('linkSetupFlatFee').style.display = 'none';
            }
        }
        function OpenFlatFeePopUp() {
            var itemId = document.getElementById('<%=hidItemId.ClientID %>').value;
            var url = 'SetupFlatFee.aspx?itemId=' + itemId
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            if (p == null || p == '') {
                return;
            }
        }--%>

        function ResetButton() {
            document.getElementById('pnControlTop').style.display = '';
            document.getElementById('pnControlBottom').style.display = '';

        }

        function GetItemURLCode(itemId) {

            var xml = getXMLHTTP();
            if (xml) {
                var url = "/includes/ajax.aspx?f=GetItemURLCode&ItemId=" + itemId
                xml.open("GET", url, true);
                xml.onreadystatechange = function() {
                    if (xml.readyState == 4 && xml.responseText) {
                        if (xml.responseText.length > 0) {
                            var aData = xml.responseText;
                            document.getElementById('<%=hidIsDefaultURLCode.ClientID %>').value = 1;
                            document.getElementById('<%=txtURLCode.ClientID %>').value = aData;

                        }
                    }
                }
                xml.send(null);
            }

        }

        function ChangeIsFlammable(value) {
            var pnFlammable = document.getElementById('pnFlammable');
            if (value) {
                if (pnFlammable)
                    pnFlammable.style.display = '';
            }
            else {
                if (pnFlammable) {
                    pnFlammable.style.display = 'none';
                    document.getElementById('<%=chkFlammable.ClientID %>').checked = false;
                }
            }
        }
              
    </script>

    <input type="hidden" value="" id="hidIsDefaultURLCode" runat="server" />
    <h4>
        Item - Add / Edit Item</h4>
    <table cellspacing="1" cellpadding="3" border="0">
        <tr>
            <td colspan="2" style="display: none;" id="pnControlTop">
                <asp:Button ID="Save2" runat="server" Text="Save" CssClass="btn"></asp:Button>
                <asp:Button ID="btnGroupItems2" runat="server" Text="View Group Items" CssClass="btn"
                    Visible="false" />
                <asp:Button ID="RelatedItems2" CausesValidation="false" runat="server" Text="Related Items"
                    CssClass="btn"></asp:Button>
                <asp:Button ID="btnVideo2" CausesValidation="false" runat="server" Text="Video" CssClass="btn"
                    Visible="false"></asp:Button>
                <asp:Button ID="btnAttributes2" CausesValidation="false" runat="server" Text="Item Attributes"
                    CssClass="btn" />
                <asp:Button ID="btnSetupFeeTop" OnClientClick="OpenFlatFeePopUp(); return false;"
                    Visible="false" runat="server" Text="Setup Fee Shipping" CssClass="btn" CausesValidation="False">
                </asp:Button>
                <CC:ConfirmButton ID="Delete2" runat="server" Message="Are you sure want to delete this item?"
                    Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
                <asp:Button ID="Cancel2" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
                </asp:Button>
                <asp:Button ID="btnAlbum2" CausesValidation="false" runat="server" Text="Album" CssClass="btn"
                    Visible="false"></asp:Button>
                   <asp:Button ID="btnRunEbay2" runat="server" Text="Run Tool Ebay" CssClass="btn"></asp:Button>
                <CC:ConfirmButton ID="CloneItem2" Visible="false" runat="server" Text="Clone Item"
                    Message="Are you sure you want to clone this item?" CssClass="btn"></CC:ConfirmButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" width="595">
                <span class="red">red color</span> - required fields
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 180px">
                <b>Item Name:</b>
            </td>
           <td class="field" style="min-width:600px;">
                <asp:TextBox ID="ItemName" runat="server" MaxLength="255" Columns="70" Width="376px"></asp:TextBox>
            </td>
            <td valign="top">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name is blank" CssClass="msgError"
                    ControlToValidate="ItemName" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>SKU:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="SKU" runat="server" MaxLength="255" Columns="70" Width="168px"></asp:TextBox>
            </td>
            <td valign="top">
                <asp:RequiredFieldValidator ID="refvSKU" runat="server" ErrorMessage="Item # is blank" CssClass="msgError"
                    ControlToValidate="SKU" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvCheckSKU" runat="server" OnServerValidate="ServerValidationItemSKU" CssClass="msgError"
                    ControlToValidate="SKU" ErrorMessage="SKU is exists."></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>URL Code:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtURLCode" runat="server" MaxLength="500" Width="376px"></asp:TextBox>
                <asp:Literal ID="ltrLinkURLCodeDefault" runat="server" Visible="false"></asp:Literal>
            </td>
            <td valign="top">
<%--                <asp:RequiredFieldValidator ID="refvURLCode" runat="server" ErrorMessage="URL Code # is blank"
                    ControlToValidate="txtURLCode" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                <asp:CustomValidator ID="cvCheckURL" runat="server" OnServerValidate="ServerValidationItemURLCode" CssClass="msgError"
                    ControlToValidate="txtURLCode" ErrorMessage="URL Code is exists."></asp:CustomValidator>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="required">
                <b>Item Type:</b>
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="drpItemType" onchange="updateItemType(this)">
                    <asp:ListItem Value="Item">Item</asp:ListItem>
                    <asp:ListItem Value="Group">Item Group</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>Primary Collection:</b>
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="ddlCollection" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Page Title:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" Columns="70" Width="376px"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" ErrorMessage="Page Title is blank" CssClass="msgError"
                    ControlToValidate="txtPageTitle" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Outside US Page Title:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSPageTitle" runat="server" MaxLength="255" Columns="70"
                    Width="376px"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Meta Description:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="255" Columns="70" /><%= Resources.Admin.lenMetaDesc%>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" ErrorMessage="Meta Description is blank" CssClass="msgError"
                    ControlToValidate="txtMetaDescription" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Outside US Meta Description:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSMetaDescription" runat="server" MaxLength="255" Columns="70" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Meta Keywords:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="255" Columns="70" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaKeywords" runat="server" ErrorMessage="Meta Keywords is blank" CssClass="msgError"
                    ControlToValidate="txtMetaKeywords" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="required2">
                            <b>Price:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="Price" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Price" Display="Dynamic" CssClass="msgError"
                                ErrorMessage="Please enter a valid Price" ID="RequiredFieldValidator5" />
                            <CC:FloatValidator runat="server" ControlToValidate="Price" Display="Dynamic" ErrorMessage="Please enter a valid Price" CssClass="msgError"
                                ID="FloatValidator1" />
                        </td>
                       <td class="optional3">
                            <b>Case Price:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtCasePrice" runat="server" MaxLength="255" Columns="70" Width="72px">0</asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
               
            </td>
        </tr>
         
        <tr id="Tr1" visible="false" runat="server">
            <td class="optional">
                <b>Sale Price:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="SalePrice" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
            </td>
            <td>
                <CC:FloatValidator runat="server" ControlToValidate="SalePrice" ErrorMessage="Please enter a valid Sale Price" CssClass="msgError"
                    ID="FloatValidator2" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>On Sale?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsOnSale" runat="server" Text="Yes" />
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="required2">
                            <b>Qty On Hand:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="QtyOnHand" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="QtyOnHand" Display="Dynamic" CssClass="msgError"
                                ErrorMessage="Please enter a valid Quantity" ID="Requiredfieldvalidator7" />
                            <CC:IntegerValidator ID="IntegerValidator2" runat="server" ControlToValidate="QtyOnHand" CssClass="msgError"
                                ErrorMessage="Please enter a valid Quantity" Display="Dynamic" />
                        </td>
                   <td class="optional3">
                            <b>Case Quantity:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtCaseQty" runat="server" MaxLength="255" Columns="70"
                                Width="72px">0</asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
               
            </td>
        </tr>
          <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Handling Fee For Item:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="txthdItem" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
                        </td>
                   <td class="optional3">
                            <b>Handling Fee For Case:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txthdCase" runat="server" MaxLength="255" Columns="70"
                                Width="72px">0</asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
               
            </td>
        </tr>
         <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                      <td class="required2">
                            <b>Weight:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="Weight" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
                        </td>
                            <td class="required3">
                            <b>Inventory Stock Notification:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtInventoryStockNotification" runat="server" MaxLength="255" Columns="70"
                                Width="72px"></asp:TextBox>
                        </td>
                        
                    </tr>
                </table>
            </td>
            <td>
                <CC:IntegerValidator runat="server" ControlToValidate="txtInventoryStockNotification" CssClass="msgError"
                    ErrorMessage="Please enter a valid Quantity" ID="IntegerValidator7" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="required2">
                            <b>Unit of Measure:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="PriceDesc" runat="server" MaxLength="50" Columns="20" Width="72px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Unit of Measure is blank" CssClass="msgError"
                                    ControlToValidate="PriceDesc" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td class="optional3">
                            <b>Low Stock Threshold:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtLowStockThreshold" runat="server" MaxLength="255" Columns="70"
                                Width="72px"></asp:TextBox><div class="smaller">
                                    <b>Default:</b> <span class="red">
                                        <%=DataLayer.SysParam.GetValue("HurryMessageThreshold")%></span></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <CC:IntegerValidator ID="IntegerValidator1" runat="server" ControlToValidate="txtLowStockThreshold" CssClass="msgError"
                    ErrorMessage="Please enter a valid threshold" />
            </td>
        </tr>
         <tr>  
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Measurement:</b>
                        </td>
                        <td class="field2">
                            <asp:RadioButton ID="radLiquid" runat="server" Text="liquid" GroupName="Measurement" />&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="radSolid" runat="server" Text="solid" GroupName="Measurement" />
                        </td>
                       
                      
                        <td class="optional3">
                            <b>Ebay/Amazon Price:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtEbayPrice" Enabled="false" runat="server" MaxLength="255" Columns="70"
                                Width="72px"></asp:TextBox>
                            <input type="hidden" id="hdEbayPrice" runat="server" value="" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:CustomValidator ID="cusEbayPrice" runat="server" ControlToValidate="txtEbayPrice" CssClass="msgError"
                    OnServerValidate="ServerCheckEbayPrice" ClientValidationFunction="CheckEbayPrice"
                    ValidateEmptyText="True" Display="Dynamic" ErrorMessage="Please enter Ebay Price"></asp:CustomValidator>
            </td>
        </tr>       
        <tr>  
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Is Rewards point:</b>
                        </td>
                        <td class="field2">
                            <asp:CheckBox ID="chkIsRewardPoint" onClick="CheckRewardPoint(this);" runat="server" />
                        </td>
                        <td class="optional3">
                            <b>Rewards point:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtRewardPoint" Enabled="false" runat="server" MaxLength="255" Columns="70"
                                Width="72px"></asp:TextBox>
                            <input type="hidden" id="hidIsRewardPoint" runat="server" value="" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:CustomValidator ID="cusvRewardPoint" runat="server" ControlToValidate="txtRewardPoint" CssClass="msgError"
                    OnServerValidate="ServerCheckPointValid" ClientValidationFunction="CheckPointValid"
                    ValidateEmptyText="True" Display="Dynamic" ErrorMessage="Please enter a valid reward point"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Maximum Qty per Order:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="txtMaximumQuantity" runat="server" MaxLength="255" Columns="70"
                                Width="72px"></asp:TextBox>
                                <CC:IntegerValidator ID="IntegerValidator5" runat="server" ControlToValidate="txtMaximumQuantity" CssClass="msgError" ErrorMessage="Please enter a valid Maximum Quantity" />
                        </td>
                         <td class="optional3">
                            <b>Low Stock Message:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtLowStockMsg" runat="server" MaxLength="255" Columns="70" Width="168px"></asp:TextBox><div
                                class="smaller">
                                <b>Default:</b> <span class="red">
                                    <%=DataLayer.SysParam.GetValue("HurryMessage")%></span></div>
                        </td>
						
                    </tr>
                </table>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="required">
                <b>Qty Reserved:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="QtyReserved" runat="server" MaxLength="255" Columns="70" Width="72px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="QtyReserved" CssClass="msgError" ErrorMessage="Please enter a valid Quantity"
                    ID="Requiredfieldvalidator8" />
                <CC:IntegerValidator runat="server" ControlToValidate="QtyReserved" CssClass="msgError" ErrorMessage="Please enter a valid Quantity"
                    ID="IntegerValidator4" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>Delivery Method</b>
            </td>
            <td class="field">
                <asp:TextBox ID="ShipMethod" runat="server" MaxLength="70" Columns="70" Width="72px"></asp:TextBox>
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>Delivery Date</b>
            </td>
            <td class="field">
                <CC:DatePicker ID="ShipmentDate" runat="server"></CC:DatePicker>
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="required">
                <b>Inventory Updated on</b>
            </td>
            <td class="field">
                <asp:Literal ID="ltlLastUpdated" runat="server"></asp:Literal>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Collection Name:</b>
                        </td>
                        <td class="field2">
                            <asp:TextBox ID="ItemNameNew" runat="server" MaxLength="50" Columns="20" Width="172px"></asp:TextBox>
                        </td>
                        <td class="optional3">
                            <b>Is Ebay/Amazon Allow:</b>
                        </td>
                        <td class="field3">
                            <asp:CheckBox ID="chkAllowPostEbay" runat="server" Text="Website" onClick="CheckAllowEbay(this);" ></asp:CheckBox>
                            <asp:CheckBox ID="chkIsEbay" Text="Navision" Enabled="false" runat="server"></asp:CheckBox>
                        </td>
                       
                    </tr>

                </table>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>MSDS:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMSDS" runat="server" MaxLength="255" Columns="70" />
            </td>
            <td>
            </td>
        </tr>
               <%--<tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                          
                        </td>
                        <td class="field2">
                           
                        </td>
                        <td class="optional3">
                            <b>Price Description 2:</b>
                        </td>
                        <td class="field3">
                            <asp:TextBox ID="txtChoiceName" runat="server" MaxLength="50" Columns="20" Width="72px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>--%>
        <tr>
            <td colspan="2" width="595">
                If you are uploading a large image, please be patient while we automatically generate
                the thumbnails for your image. This may take a few moments for large file sizes.
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Image:<br />
                    <span class="smaller">490 x 490</span> </b>
            </td>
            <td class="field">
                <CC:FileUpload runat="server" ID="fuImage" Folder="/assets/items/original" DisplayImage="true"
                    ImageDisplayFolder="/assets/items" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Image Alt Tag:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtImageAltTag" runat="server" Columns="50" MaxLength="100" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>Item Number:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="ProductNo" runat="server" MaxLength="255" Columns="70" Width="392px"></asp:TextBox><br />
                <span class="smallest">not required for collections</span>
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Is New?</b>
            </td>
            <td class="field">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:CheckBox ID="IsNew" runat="server" Text="Yes" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                            Until:
                        </td>
                        <td>
                            <CC:DatePicker ID="NewUntil" runat="server"></CC:DatePicker>
                        </td>
                        <td  style="padding-left:20px;">
                            <CC:DateValidator ID="DateValidatorNewUntil" runat="server" CssClass="msgError" ControlToValidate="NewUntil" ErrorMessage="Please enter a valid date" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
            </td>
        </tr>
        <tr>
            <td valign="top" class="required">
                <b>Departments:</b>
            </td>
            <td class="field" width="485">
                Please select below all departments that this item belongs to.<br>
                <Ctrl:DepartmentTree ID="treeDepartments" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Brand Name:</b>
                        </td>
                        <td class="field2">
                            <asp:DropDownList ID="drpBrandId" Width="200px" runat="server" />
                        </td>
                        <td class="optional3">
                            <b>Collection:</b>
                        </td>
                        <td class="field3">
                            <asp:DropDownList runat="server" ID="ddlCollections" />
                            <CC:CheckBoxList ID="cblCollections" Visible="False" RepeatColumns="3" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="tdcolspan">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="optional2">
                            <b>Tone:</b>
                        </td>
                        <td class="field2">
                            <asp:DropDownList runat="server" ID="ddlTones" />
                        </td>
                        <td class="optional3">
                            <b>Shade:</b>
                        </td>
                        <td class="field3">
                            <asp:DropDownList runat="server" ID="ddlShades" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Rush Delivery / Charge:
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsRushDelivery" />
                <asp:TextBox runat="server" ID="txtRushDeliveryCharge" Style="width: 100px;" />
            </td>
            <td>
                <CC:FloatValidator ID="FloatValidator3" runat="server" CssClass="msgError" ControlToValidate="txtRushDeliveryCharge"
                    ErrorMessage="Please enter a valid rush delivery charge" Display="dynamic" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                Lift Gate Charge:
            </td>
            <td class="field">
                <asp:TextBox runat="server" ID="txtLiftGateCharge" Style="width: 100px;" />
            </td>
            <td>
                <CC:FloatValidator runat="server" CssClass="msgError" ControlToValidate="txtLiftGateCharge" ErrorMessage="Please enter a valid lift gate charge"
                    Display="dynamic" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                Call to Schedule Delivery Charge:
            </td>
            <td class="field">
                <asp:TextBox runat="server" ID="txtScheduleDeliveryCharge" Style="width: 100px;" />
            </td>
            <td>
                <CC:FloatValidator ID="FloatValidator4" CssClass="msgError" runat="server" ControlToValidate="txtScheduleDeliveryCharge"
                    ErrorMessage="Please enter a valid schedule delivery charge" Display="dynamic" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional" valign="top">
                <b>Features:</b>
            </td>
            <td class="field" width="485">
                <asp:DataList ID="dlFeatures" runat="server" RepeatColumns="3" CellPadding="1" CellSpacing="1"
                    RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <input type="checkbox" id="chkFeature" name="chkFeature" value="" runat="server" />
                        <asp:Label ID="lblFeature" runat="server" />
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional" valign="top">
                <b>Occasion(s):</b>
            </td>
            <td class="field" width="485">
                <asp:DataList ID="dlOccasions" runat="server" RepeatColumns="3" CellPadding="1" CellSpacing="1"
                    RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <input type="checkbox" id="chkOccasion" name="chkOccasion" value="" runat="server" />
                        <asp:Label ID="lblOccasion" runat="server" />
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional" valign="top">
                <b>Shipping Features:</b>
            </td>
            <td class="field" width="485">
                <asp:DataList ID="dlShippingFeatures" runat="server" RepeatColumns="3" CellPadding="1"
                    CellSpacing="1" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <input type="checkbox" id="chkFeature" name="chkFeature" value="" runat="server" />
                        <asp:Label ID="lblFeature" runat="server" />
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dshort').style.display=='none' ? document.getElementById('dshort').style.display='block' : document.getElementById('dshort').style.display='none';">
                    <b>Short Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dshort" style="display: none;">
                    <asp:TextBox ID="ShortDesc" runat="server" TextMode="MultiLine" Width="540px" Rows="5" CssClass="pro-desc"></asp:TextBox>
                    <br /><span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
                <asp:Label ID="lblShortDesc" runat="server" CssClass="red" Visible="false">ShortDesc # is blank</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dlong').style.display=='none' ? document.getElementById('dlong').style.display='block' : document.getElementById('dlong').style.display='none';">
                    <b>Long Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dlong" style="display: none;">
                    <asp:TextBox ID="LongDesc" runat="server" TextMode="MultiLine" Width="540px" Rows="5" CssClass="pro-desc"></asp:TextBox>
                    <br /><span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
                <asp:Label ID="lblLongDesc" runat="server" CssClass="red" Visible="false">LongDesc # is blank</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dshortv').style.display=='none' ? document.getElementById('dshortv').style.display='block' : document.getElementById('dshortv').style.display='none';">
                    <b>Short Vietnamese Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dshortv" style="display: none;">
                    <asp:TextBox ID="ShortVietDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dlongv').style.display=='none' ? document.getElementById('dlongv').style.display='block' : document.getElementById('dlongv').style.display='none';">
                    <b>Long Vietnamese Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dlongv" style="display: none;">
                    <asp:TextBox ID="LongVietDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dshortf').style.display=='none' ? document.getElementById('dshortf').style.display='block' : document.getElementById('dshortf').style.display='none';">
                    <b>Short French Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dshortf" style="display: none;">
                    <asp:TextBox ID="ShortFrenchDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dlongf').style.display=='none' ? document.getElementById('dlongf').style.display='block' : document.getElementById('dlongf').style.display='none';">
                    <b>Long French Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dlongf" style="display: none;">
                    <asp:TextBox ID="LongFrenchDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dshorts').style.display=='none' ? document.getElementById('dshorts').style.display='block' : document.getElementById('dshorts').style.display='none';">
                    <b>Short Spanish Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dshorts" style="display: none;">
                    <asp:TextBox ID="ShortSpanishDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dlongs').style.display=='none' ? document.getElementById('dlongs').style.display='block' : document.getElementById('dlongs').style.display='none';">
                    <b>Long Spanish Description:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dlongs" style="display: none;">
                    <asp:TextBox ID="LongSpanishDesc" runat="server" TextMode="MultiLine" Width="540px"
                        Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('daddit').style.display=='none' ? document.getElementById('daddit').style.display='block' : document.getElementById('daddit').style.display='none';">
                    <b>Additional Info:</b></a>
            </td>
            <td class="field" width="545">
                <div id="daddit" style="display: none;">
                    <asp:TextBox ID="AdditionalInfo" runat="server" TextMode="Multiline" Width="400px"
                        Height="400px" />
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dspec').style.display=='none' ? document.getElementById('dspec').style.display='block' : document.getElementById('dspec').style.display='none';">
                    <b>Instructions:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dspec" style="display: none;">
                    <asp:TextBox ID="Specifications" runat="server" TextMode="Multiline" Width="400px"
                        Height="400px" />
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dship').style.display=='none' ? document.getElementById('dship').style.display='block' : document.getElementById('dship').style.display='none';">
                    <b>Shipping Info:</b></a>
            </td>
            <td class="field" width="545">
                <div id="dship" style="display: none;">
                    <asp:TextBox ID="ShippingInfo" runat="server" TextMode="Multiline" Width="400px"
                        Height="400px" />
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional" valign="top">
                <a href="javascript:void(0);" onclick="document.getElementById('dhelp').style.display=='none' ? document.getElementById('dhelp').style.display='block' : document.getElementById('dhelp').style.display='none';">
                    <b>Helpful Tips:</b></a>
            </td>
            <td class="field">
                <div id="dhelp" style="display: none;">
                    <asp:TextBox ID="HelpfulTips" runat="server" TextMode="Multiline" Width="400px" Height="400px" />
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span></div>
            </td>
            <td>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional" valign="top">
                <b>Size:</b>
            </td>
            <td class="field" width="545">
                <asp:TextBox runat="server" ID="txtSize" MaxLength="100" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Status:</b>
            </td>
            <td class="field">
                <asp:DropDownList ID="Status" runat="server">
                </asp:DropDownList>
            </td>
            <td valign="top">
                <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" CssClass="msgError" runat="server" ErrorMessage="Status is blank"
                    ControlToValidate="Status" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Backorder Date:</b>
            </td>
            <td class="field">
                <CC:DatePicker runat="server" ID="BODate" />
            </td>
            <td valign="top">
                <CC:DateValidator runat="server" ControlToValidate="BODate"  CssClass="msgError" ErrorMessage="Please enter a valid date" />
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="optional">
                <b>Collection:</b>
            </td>
            <td class="field">
                <asp:DropDownList ID="drpCollection" runat="server">
                </asp:DropDownList>
            </td>
            <td valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="IsActive" Text="Yes" runat="server"></asp:CheckBox>
                <span style="font:italic 11px Arial; padding-left:20px; color:#444;">To search item that has just ACTIVE, you need "RUN QUICK SEARCH"</span>
            </td>
        </tr>
        <tr>
            <td class="optional"></td>
            <td class="field"><asp:CheckBox ID="IsFirstClassPackage" Text="Allow First Class Package" runat="server"></asp:CheckBox></td>
        </tr>
        <tr>
            <td class="optional">
            </td>
            <td class="field chk">
                <asp:CheckBox ID="chkIsFreeShipping" Text="Free Shipping" runat="server"></asp:CheckBox>
                <asp:CheckBox ID="chkIsFreeSample" runat="server" Text="Free Sample"></asp:CheckBox>
                <asp:CheckBox runat="server" ID="chkIsFeatured" Text="Featured" />
               
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b></b>
            </td>
            <td class="field chk">
                <asp:CheckBox runat="server" ID="chkIsBestSeller" Text="Best Seller" />
                <asp:CheckBox runat="server" ID="chkIsHot" Text="Hot" />
                <asp:CheckBox runat="server" ID="chkIsSpecialOrder" Text="Special Order" />
                <asp:CheckBox ID="chkInStock" onClick="CheckAccepting(1)" runat="server" Text="In stock" />
                <asp:CheckBox ID="chkIsAcceptingOrder" onClick="CheckAccepting(2)" runat="server" Text="Accepting Order" />
                <asp:CheckBox ID="IsTaxFree" Visible="false" runat="server" Text="Tax Exempt" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Hide from these Posting Groups:</b>
            </td>
            <td class="field">
                <CC:CheckBoxList runat="server" ID="cblPostingGroups" RepeatColumns="4" />
            </td>
        </tr>
        <tr>
            <td class="optional">
            </td>
            <td class="field chk">
                <%--<asp:CheckBox ID="chkIsHazMat" Text="Flammable" OnClick="ShowBlockFlammable();" runat="server"></asp:CheckBox>

                <asp:CheckBox ID="chkFlammable" runat="server" Text="Block air shipments" class="hide"></asp:CheckBox>--%>

                <asp:CheckBox ID="chkIsHazMat" onclick="ChangeIsFlammable(this.checked)" Text="Flammable" runat="server"></asp:CheckBox>
                <span id="pnFlammable" style="display: none;">
                    <asp:CheckBox ID="chkFlammable" Text="Block air shipments" runat="server"></asp:CheckBox>
                </span>
            </td>

        </tr>
        <tr>
            <td class="optional">
            </td>
            <td class="field chk">
                <asp:CheckBox ID="chkIsOversize" onclick="ChangeIsOverSize(this.checked)" Text="Oversize (Shipment by Truck only)"
                    runat="server"></asp:CheckBox>
                <%--<span id="pnFlatFee" style="display: none;">
                    <asp:CheckBox ID="chkIsFlatFee" Text="Flat Fee" onclick="ChangeIsFlatFee(this.checked)"
                        runat="server"></asp:CheckBox>
                </span><a href='javascript:void(0);' onclick="OpenFlatFeePopUp();" id='linkSetupFlatFee'
                    style="display: none;">Setup Fee Shipping</a>--%>
                <input type="hidden" id="hidItemId" value="" runat="server" />
            </td>
        </tr>
        <%--<tr runat="server" id="trBase">
            <td class="optional">
                <b>Available Base Colors:</b>
            </td>
            <td class="field">
                <CC:CheckBoxList runat="server" ID="cblBaseColor" RepeatColumns="5" CellPadding="0"
                    CellSpacing="1" />
            </td>
        </tr>
        <tr runat="server" id="trCusion">
            <td class="optional">
                <b>Available Cusion Colors:</b>
            </td>
            <td class="field">
                <CC:CheckBoxList runat="server" ID="cblCusionColor" RepeatColumns="5" CellPadding="0"
                    CellSpacing="1" />
            </td>
        </tr>
        <tr runat="server" id="trLaminate">
            <td class="optional">
                <b>Available Laminate Colors:</b>
            </td>
            <td class="field">
                <CC:CheckBoxList runat="server" ID="cblLaminateTrim" RepeatColumns="5" CellPadding="0"
                    CellSpacing="1" />
            </td>
        </tr>--%>
        <tr id="trEbayShippingType" runat="server" visible="false">
            <td class="optional">
                Ebay Shipping Type:
            </td>
            <td class="field">
                <asp:DropDownList ID="drEbayShippingType" runat="server">
                    <asp:ListItem Value="">--Select Shipping Type--</asp:ListItem>
                    <asp:ListItem Value="USPSPriorityMailSmallFlatRateBox">USPS Priority Mail Small Flat Rate Box</asp:ListItem>
                    <asp:ListItem Value="USPSStandardPost">USPS Standard Post</asp:ListItem>
              <%--<asp:ListItem Value="USPSPriorityFlatRateBox">USPS Priority Mail Medium Flat Rate Box</asp:ListItem>
                    <asp:ListItem Value="USPSPriorityMailLargeFlatRateBox">USPS Priority Mail Large Flat Rate Box</asp:ListItem>
                    <asp:ListItem Value="UPSGround">USP Ground</asp:ListItem>--%>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <p>
        <asp:ScriptManager runat="server" ID="sm" />
        <asp:UpdatePanel runat="server" ID="upFeature" UpdateMode="conditional">
            <ContentTemplate>
                <h3 style="margin-bottom: 5px;">
                    Features</h3>
                <asp:LinkButton runat="server" Text="Add New Feature" ID="lnkAddFeature" CausesValidation="false" />
                <table border="0" cellpadding="2" cellspacing="1" style="margin-bottom: 5px;" runat="server"
                    visible="false" id="tblFeature">
                    <tr valign="top">
                        <td class="required">
                            Feature<asp:Label runat="server" Visible="false" ID="lblFeatureId" />
                        </td>
                        <td class="field">
                            <asp:TextBox runat="server" ID="txtFeature" MaxLength="50" Style="width: 319px;" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="msgError" runat="server" Display="dynamic"
                                ControlToValidate="txtFeature" ErrorMessage="Description is required" ValidationGroup="Features" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <CC:OneClickButton runat="server" ID="btnFeaturesave" Text="Save" CssClass="btn"
                                ValidationGroup="Features" />
                            <CC:OneClickButton runat="server" ID="btnFeatureCancel" Text="Cancel" CausesValidation="false"
                                CssClass="btn" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellspacing="1" cellpadding="2">
                    <tr valign="top">
                        <th>
                        </th>
                        <th>
                        </th>
                        <th>
                            Feature
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptFeatures">
                        <ItemTemplate>
                            <tr class="row">
                                <td>
                                    <asp:LinkButton runat="server" ID='lnkEdit' Text='<img src="/images/admin/edit.gif" style="border-style:none;" alt="Edit Feature" />'
                                        CausesValidation="false" CommandName='Modify' CommandArgument='<%#Container.DataItem("FeatureId")%>' />
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID='lnkRemove' Text='<img src="/images/admin/delete.gif" style="border-style:none;" alt="Edit Feature" />'
                                        CausesValidation="false" CommandName='Remove' CommandArgument='<%#Container.DataItem("FeatureId")%>'
                                        OnClientClick="return confirm('Are you sure you want to remove this Feature?');" />
                                </td>
                                <td>
                                    <%#Container.DataItem("Name")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate">
                                <td>
                                    <asp:LinkButton runat="server" ID='lnkEdit' Text='<img src="/images/admin/edit.gif" style="border-style:none;" alt="Edit Feature" />'
                                        CausesValidation="false" CommandName='Modify' CommandArgument='<%#Container.DataItem("FeatureId")%>' />
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID='lnkRemove' Text='<img src="/images/admin/delete.gif" style="border-style:none;" alt="Edit Feature" />'
                                        CausesValidation="false" CommandName='Remove' CommandArgument='<%#Container.DataItem("FeatureId")%>'
                                        OnClientClick="return confirm('Are you sure you want to remove this Feature?');" />
                                </td>
                                <td>
                                    <%#Container.DataItem("Name")%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <p style="display: none;" id="pnControlBottom">
            <asp:Button ID="Save" runat="server" Text="Save" CssClass="btn"></asp:Button>
            <asp:Button ID="btnGroupItems" CausesValidation="false" runat="server" Text="View Group Items"
                CssClass="btn" Visible="false" />
            <asp:Button ID="RelatedItems" CausesValidation="false" runat="server" Text="Related Items"
                CssClass="btn"></asp:Button>
            <asp:Button ID="btnVideo" runat="server" CausesValidation="false" Text="Video" CssClass="btn"
                Visible="false"></asp:Button>
            <asp:Button ID="btnAttributes" CausesValidation="false" runat="server" Text="Item Attributes"
                CssClass="btn" />
            <asp:Button ID="btnSetupFeeBottom" OnClientClick="OpenFlatFeePopUp(); return false;"
                runat="server" Text="Setup Fee Shipping" CssClass="btn" CausesValidation="False"
                Visible="false" />
            <CC:ConfirmButton ID="Delete" runat="server" Message="Are you sure want to delete this item?"
                Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
            </asp:Button>
            <asp:Button ID="btnRunEbay" runat="server" Text="Run Tool Ebay" CssClass="btn"></asp:Button>
            <CC:ConfirmButton ID="CloneItem" Visible="false" runat="server" Text="Clone Item"
                Message="Are you sure you want to clone this item?" CssClass="btn"></CC:ConfirmButton>
            <asp:Button ID="btnAlbum" CausesValidation="false" runat="server" Text="Album" Visible="false"
                CssClass="btn"></asp:Button>
        </p>

<script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>
