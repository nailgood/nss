<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Main-block-banner.aspx.vb" Inherits="admin_graphic_FlashBanner_Main_block_banner" %>


<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Main Block Banner</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
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
                 <span id="imgsize">983x433</span>
                  <br />
                < <span id="imglength">130</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/"
                    ImageDisplayFolder="/assets/Banner" DisplayImage="false" runat="server" />
                     <div id="msgError" class="msgError">
                    <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></div>
               <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
            </td>
        </tr>
        <tr id="trBackground" runat="server" visible="true">
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
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Sales Banner?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>
