<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_Banner_Edit" Title="Sales Banner" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Main Banner</h4>
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
                <asp:DropDownList runat="server" onchange="ChangeDepartment(this.value);" ID="ddlDepartment">
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
            <td class="required">
                Image File:<br />
                <span id="imgsize">1140x400</span>
                <br />
                < <span id="imglength">150</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/"
                    ImageDisplayFolder="/assets/Banner" DisplayImage="false" runat="server" />
                   <div id="msgError" class="msgError"><asp:Literal ID="ltMssImage" runat="server"></asp:Literal></div>
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                        (This image will be display on live web)
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
            </td>
        </tr>
        <tr>
            <td class="required">
                Mobile Image File:<br />
                <span id="imgsize1">540x189</span>
                 <br />
                < <span id="imglength1">65</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImageMobile" EnableDelete="false" AutoResize="true" Folder="/assets/banner/mobile/"
                    ImageDisplayFolder="/assets/banner/mobile/" DisplayImage="false" runat="server" />
                   <div id="msgError1" class="msgError"><asp:Literal ID="ltmsgimage1" runat="server"></asp:Literal></div>
                <div runat="server" id="divImg1">
                    <b>Preview with Map:</b><map name="hpimgmap1"><asp:Literal runat="server" ID="litmap1" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg1" usemap="#hpimgmap1" /></div>
                        (This image will be display on live web)
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="FileUploadExtensionValidator2"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
            </td>
        </tr>
        <tr id="trBackground" runat="server">
            <td class="optional">
                Background:
                <div>
                    <asp:RadioButton ID="radColor" runat="server" GroupName="gpbackground" Checked="true"
                        AutoPostBack="true" />
                    Color
                </div>
                <div>
                    <asp:RadioButton ID="radImage" runat="server" GroupName="gpbackground" AutoPostBack="true" />
                    Image</div>
            </td>
            <td class="field">
                <div id="dtxt" runat="server">
                    <asp:TextBox ID="txtColor" runat="server"></asp:TextBox>
                    (ex: #000 or #000000)</div>
                <div id="dimage" runat="server">
                    <CC:FileUpload ID="fuBackground" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/background/"
                        ImageDisplayFolder="/assets/Banner/background/" DisplayImage="false" runat="server" />
                    <div>
                        <b>Preview with Map:</b><map name="hpimgbg">
                            <asp:Image runat="server" ID="imgbg" usemap="#hpimgmap" />
                    </div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="FileUploadExtensionValidator1"
                    runat="server" Display="Dynamic" ControlToValidate="fuBackground" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <span style="color: Red">
                    <asp:Literal ID="ltmsgbg" runat="server"></asp:Literal></span>
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
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvStartingDate" ControlToValidate="dtStartingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" />
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
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvEndingDate" ControlToValidate="dtEndingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" />
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
          
            if (value != 23)
                document.getElementById('<%=trBackground.ClientID %>').style.display = '';
            else document.getElementById('<%=trBackground.ClientID %>').style.display = 'none';
        }
    
    </script>
</asp:Content>
