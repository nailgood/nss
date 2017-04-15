<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_Keyword_RedirectKeyword_edit" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>
        Redirect keyword</h4>
    <table>
        <tr>
            <td valign="top" colspan="2">
                <font class="red">red color</font>- required fields
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 110px;">
                <strong>Keyword Name:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvName" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Link:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtLink" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvLink" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Link cannot be blank" ControlToValidate="txtLink"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Description:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <p>
        <CC:OneClickButton ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="level"
            CssClass="btn" Text="Save"></CC:OneClickButton>&nbsp;
        <CC:OneClickButton ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" CausesValidation="False">
        </CC:OneClickButton>
    </p>
</asp:Content>
