<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_groups_options_choices_Edit" Title="Group Option Choice" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If ChoiceId = 0 Then%>Add<% Else%>Edit<% End If%>
        Group Option Choice</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Option:
            </td>
            <td class="field">
                <asp:DropDownList ID="drpOptionId" runat="server" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvOptionId" CssClass="msgError" runat="server"  Display="Dynamic" ControlToValidate="drpOptionId"
                    ErrorMessage="Field 'Option Id' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Choice Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtChoiceName" runat="server" MaxLength="50" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvChoiceName" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtChoiceName"
                    ErrorMessage="Field 'Choice Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image File:<br />
                <span class="smaller">50 x 50</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuThumbImage" EnableDelete="false" AutoResize="true" 
                     DisplayImage="false" runat="server" />
              <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,ico,png" ID="fuValidateThumbImage" CssClass="msgError"
                    runat="server" Display="Dynamic" ControlToValidate="fuThumbImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <span style="color: Red">
                    <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Group Option Choice?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>
