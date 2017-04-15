<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_NewsEvent_Document_Edit" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If ID = 0 Then%>Add<% Else%>Edit<% End If%>
        Document</h4>
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
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="128" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName"
                    CssClass="msgError" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>
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
                Document File:<br />
            </td>
            <td class="field">
                <CC:FileUpload ID="fuDocument" AutoResize="true" DisplayImage="False" runat="server"
                    Style="width: 475px;" />
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
</asp:Content>
