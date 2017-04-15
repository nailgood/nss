<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_graphic_InforBanner_edit" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Info Banner</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Name:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvName" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Description:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" runat="server" Width="400px" MaxLength="256" Height="120px"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvDesc" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Description cannot be blank" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Link:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLink" runat="server" MaxLength="200" Columns="50" Style="width: 400px;"></asp:TextBox>
                <br />
                <span class="smaller">Ex: /store/dealday.aspx</span>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="refvLink" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Link cannot be blank" ControlToValidate="txtLink"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Image File:<br />
              <span id="imgsize">268x268</span>  
              <br />
              < <span id="imglength">24</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/inforbanner/"
                    ImageDisplayFolder="/assets/inforbanner" DisplayImage="false" runat="server" />
                    <div id="msgError" class="msgError"><asp:Literal ID="ltMssImage" runat="server"></asp:Literal></div>
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                     
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ValidationGroup="inforbanner" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
            </td>
        </tr>
           <tr>
            <td class="required">
                Mobile Image File:<br />
                <span id="imgsize1">210x210</span>
                 <br />
                < <span id="imglength1">13</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImageMobile" EnableDelete="false" AutoResize="true" Folder="/assets/inforbanner/mobile/" ImageDisplayFolder="/assets/inforbanner/mobile" DisplayImage="false" runat="server" />
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
    <CC:OneClickButton ID="btnSave" runat="server" ValidationGroup="inforbanner" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Sales Banner?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
 </asp:Content>
