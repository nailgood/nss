<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_freegift_Edit" Title="Free Gift" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <%--<script language="javascript">
      function ReloadFrame(name)
      {
       if(name=='load'){
       location.reload();}
       }
      </script>--%>
    <h4>
        <% If FreeGiftId = 0 Then%>Add<% Else%>Edit<% End If%> Free Gift</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Item:
            </td>
            <td class="field">
                <asp:DropDownList ID="drpItemId" runat="server" Visible="false" />
                <input type="button" class="btn" id="Button1" value="Add Item Free Gift" onclick="OpenPopUp();" /><asp:TextBox
                    ID="txtSku" runat="server" MaxLength="8" Columns="8" Enabled="false" Style="width: 67px;"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbError" runat="server" CssClass="red"></asp:Label><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSku"
                    CssClass="msgError" ErrorMessage="Please add Item Free Gift"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Level:
            </td>
            <td class="field">
                <asp:DropDownList ID="drlLevel" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvLevel" runat="server" Display="Dynamic" ControlToValidate="drlLevel"
                    CssClass="msgError" ErrorMessage="Field 'Level' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="display: none">
            <td class="required">
                Image:<br />
                <span class="smaller">405 x 78</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" Folder="/assets/freegift" ImageDisplayFolder="/assets/freegift"
                    DisplayImage="True" runat="server" Style="width: 319px;" />
            </td>
            <td>
               <%-- <CC:RequiredFileUploadValidator ID="rfvImage" runat="server" Display="Dynamic" ControlToValidate="fuImage"
                    CssClass="msgError" ErrorMessage="Field 'Image' is required"></CC:RequiredFileUploadValidator>--%><CC:FileUploadExtensionValidator
                        Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage"
                        CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr style="display: none">
            <td class="optional">
                Banner:<br />
                <span class="smaller">475 x 205</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage1" Folder="/assets/freegift" ImageDisplayFolder="/assets/freegift"
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                    <span style="color: Red">
                        <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="FileUploadExtensionValidator1"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator><asp:Literal
                        ID="ltMessage" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Allow Add Cart</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkAddCart" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Free Gift?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
    <input type="hidden" runat="server" value="" id="hidItemId" />
    <div style="display: none">
        <CC:OneClickButton ID="AddNew" runat="server" Text="Add New SalesPrice Image" CssClass="btn">
        </CC:OneClickButton></div>
    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>
    <script>
        function OpenPopUp() {
            var msgErr = $('#ctl00_ph_lbError').text();
            if (msgErr != "")
                $('#ctl00_ph_lbError').text("");

            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '../ShopSave/SearchItem.aspx?Type=0&item=' + item
           
            var brow = GetBrowser();
            if (brow == 'ie') {
                var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                document.getElementById('<%=txtSku.ClientID %>').value = p;
            }
            else {
                ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            }
        }
        function SetValue(save, value) {
           
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
            }

        }
    
    </script>
</asp:Content>
