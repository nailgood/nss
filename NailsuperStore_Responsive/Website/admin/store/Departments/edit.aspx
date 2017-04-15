<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_departments_Edit" Title="Department" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If DepartmentId = 0 Then %>Add<% Else %>Edit<% End If %>
        Department</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic"  CssClass="msgError" ControlToValidate="txtName"
                    ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Alternate Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtAlternateName" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
               <%-- <asp:RequiredFieldValidator ID="rfvAlternatename" runat="server" Display="Dynamic" ControlToValidate="txtAlternateName"
                    ErrorMessage="Field 'Alternate Name' is blank"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr runat="server" id="trNameRewriteUrl" visible="false">
            <td class="optional">
                Name RewriteUrl:
            </td>
            <td class="field">
                <asp:TextBox ID="txtNameRewriteUrl" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ControlToValidate="txtName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td class="required">
                URL Code:
            </td>
            <td class="field">
                <asp:TextBox ID="txtURLCode" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvURLCode" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtURLCode" ErrorMessage="Field 'URLCode' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr runat="server" id="trBannerEffect">
            <td class="optional">
                Banner Effect:
            </td>
            <td class="field">
                <%--<asp:DropDownList runat="server" ID="drpEffect">
		        <asp:ListItem Text="Square Rotator" Value="0"></asp:ListItem>
		        <asp:ListItem Text="Fly From Right" Value="1"></asp:ListItem>
		        <asp:ListItem Text="Fade In Fad Out" Value="2"></asp:ListItem>
            </asp:DropDownList>--%>
                <asp:CheckBoxList ID="chkEffect" RepeatLayout="Table" RepeatColumns="4" runat="server" RepeatDirection="Horizontal">                   
                </asp:CheckBoxList>
                <a href="/admin/store/banner/default.aspx?DepartmentId=<%=DepartmentId%>">add banner</a>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Main Image:  <br />
               <span id="imgsize" class="smaller">220 x 220</span>
                <br />
                < <span id="imglength">15</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage"
                    DisplayImage="True" runat="server" Style="width: 319px;" />
                     <div id="msgError" class="msgError"></div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp,swf" ID="feImage" CssClass="msgError"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Large Image Alt Tag:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLargeImageAltTag" runat="server" MaxLength="100" Columns="50"
                    Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Large Image Url:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLargeImageUrl" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Popup Image:
            </td>
            <td class="field">
                <CC:FileUpload ID="fuLargeImage" Folder="/assets/departments" ImageDisplayFolder="/assets/departments"
                    DisplayImage="True" runat="server" Style="width: 319px;" />
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feLargeImage" runat="server" CssClass="msgError"
                    Display="Dynamic" ControlToValidate="fuLargeImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image Alt Tag:
            </td>
            <td class="field">
                <asp:TextBox ID="txtImageAltTag" runat="server" MaxLength="100" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Name Image:
            </td>
            <td class="field">
                <CC:FileUpload ID="fuNameImage" Folder="/assets/departments/name" ImageDisplayFolder="/assets/departments/name"
                    DisplayImage="True" runat="server" Style="width: 319px;" />
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feNameImage" runat="server" CssClass="msgError"
                    Display="Dynamic" ControlToValidate="fuNameImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle" CssClass="msgError"
                    ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Outside US Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSPageTitle" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="255" Columns="50"
                    Style="width: 319px;"></asp:TextBox><%= Resources.Admin.lenMetaDesc%>
            </td>
            <td>
                <%--<asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtMetaDescription" ErrorMessage="Field 'Meta Description' is blank">
                </asp:RequiredFieldValidator>--%>
            </td>
        </tr>
          <tr>
            <td class="optional">
                Outside US Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSMetaDescription" runat="server" MaxLength="255" Columns="50"
                    Style="width: 319px;"></asp:TextBox>
            </td>
            <td>               
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Keywords:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <%--<asp:RequiredFieldValidator ID="refvMetaKeywords" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtMetaKeywords" ErrorMessage="Field 'Meta Keywords' is blank"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Inactive?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsInactive" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Quick Order?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsQuickOrder" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Filter?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsFilter" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" Style="width: 349px;" Columns="55" Rows="5" TextMode="Multiline"
                    runat="server"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    <script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>
