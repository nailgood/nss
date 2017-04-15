<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="edit.aspx.vb" Inherits="admin_promotions_dealday_edit" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>DealDay</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%--<span class="red">Errore Insert</span> --%>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="required">
                Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle"
                    CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Starting Date:
            </td>
            <td class="field">
                <table>
                    <tr>
                        <td>
                            <CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartHour" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartMinute" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvStartingDate" ControlToValidate="dtStartingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Ending Date:
            </td>
            <td class="field">
                <table>
                    <tr>
                        <td>
                            <CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndHour" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndMinute" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvEndingDate" ControlToValidate="dtEndingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" />
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
                Image File:<br />
                <span class="smaller">754 x 320</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" AutoResize="true" Folder="/assets/dealday" ImageDisplayFolder="/assets/dealday"
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                SKU:
            </td>
            <td class="field">
                <asp:TextBox ID="txtSKU" runat="server" MaxLength="6" Columns="50" Style="width: 60px;"></asp:TextBox><input
                    type="button" class="btn" id="btnAddSKU" value="Add SKU" onclick="OpenPopUp();" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvSKU" runat="server" Display="Dynamic" ControlToValidate="txtSKU"
                    CssClass="msgError" ErrorMessage="Field 'SKU' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    CssClass="msgError" ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Keywords:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic"
                    ControlToValidate="txtMetaKeyword" CssClass="msgError" ErrorMessage="Field 'Meta Keywords' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic"
                    ControlToValidate="txtMetaDescription" CssClass="msgError" ErrorMessage="Field 'Meta Description' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>

    <script>
        function OpenPopUp() {

            var item = document.getElementById('<%=txtSKU.ClientID %>').value;
            var url = '../../promotions/ShopSave/SearchItem.aspx?Type=0&F_HasSalesPrice=&item=' + item
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');

            var brow = GetBrowser();
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '')
                        document.getElementById('<%=txtSKU.ClientID %>').value = p;
                }
            }
        }
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
            }


        }
    </script>

</asp:Content>
