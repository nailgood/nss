<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="Edit.aspx.vb" Inherits="admin_FacebookPage_Edit" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Page</h4>
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
            <td class="required">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Link:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLink" runat="server" MaxLength="255" TextMode="SingleLine" Columns="50"
                    Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvLink" runat="server" Display="Dynamic" ControlToValidate="txtLink"
                    CssClass="msgError" ErrorMessage="Field 'Link' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image:<br />
                <span class="smaller">90 x 90</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuThumb" AutoResize="true" DisplayImage="False" runat="server"
                    Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server"
                    Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px; height:150px;"></asp:TextBox>
            </td>
            <td>
             <asp:RequiredFieldValidator ID="refvDesc" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                    CssClass="msgError" ErrorMessage="Field 'Description' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>
