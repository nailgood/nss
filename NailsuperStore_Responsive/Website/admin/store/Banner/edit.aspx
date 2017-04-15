<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_Banner_Edit" Title="Sales Banner" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Flash Banner</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td valign="top" class="required">
                <b>Departments:</b>
            </td>
            <td class="field" width="485">
                Please select below all departments that this item belongs to.<br>
                <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Url:
            </td>
            <td class="field">
                <asp:TextBox ID="txtUrl" runat="server" MaxLength="200" Columns="50" Style="width: 319px;"></asp:TextBox>
                <br />
                <span class="smaller">Ex: /store/dealday.aspx</span>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image File:<br />
                <span class="smaller">754 x 320</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/"
                    ImageDisplayFolder="/assets/Banner" DisplayImage="false" runat="server" />
                (This image will be display on live web)<div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator CssClass="msgError" Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <span style="color: Red">
                    <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
            </td>
        </tr>
        <tr id="trMobileImage" runat="server" >
            <td class="optional">
                Image File:<br />
                <span class="smaller">320 x 140</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImageMobile" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/Mobile"
                    ImageDisplayFolder="/assets/Banner/Mobile" DisplayImage="false" runat="server" />
                (This image will be display on mobile web)<div runat="server" id="divImgMobile">
                    <b>Preview with Map:</b><map name="hpimgmapMobile"><asp:Literal runat="server" ID="litMapMobile" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimgMobile" usemap="#hpimgmapMobile" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImageMobile" CssClass="msgError"
                    runat="server" Display="Dynamic" ControlToValidate="fuImageMobile" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <span style="color: Red">
                    <asp:Literal ID="ltMssImageMobile" runat="server"></asp:Literal></span>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Starting Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvStartingDate" CssClass="msgError" ControlToValidate="dtStartingDate"
                    ErrorMessage="Date Field 'Starting Date' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Ending Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvEndingDate" CssClass="msgError" ControlToValidate="dtEndingDate"
                    ErrorMessage="Date Field 'Ending Date' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Sales Banner?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    <script type="text/javascript">
        function ChangeDepartment(value) {
            if (value=23)
                document.getElementById('trMobileImage').style.display = '';
            else document.getElementById('trMobileImage').style.display = 'none';
       }
    </script>
</asp:Content>
