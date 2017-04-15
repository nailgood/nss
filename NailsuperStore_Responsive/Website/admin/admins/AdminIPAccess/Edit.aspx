<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="admin_admins_AdminIPAccess_Edit" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
 <h4>
        <% If ID = 0 Then%>Add<% Else%>Edit<% End If%>
        IP for <%=UserName()%></h4>
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
                IP:
            </td>
            <td class="field">
                <asp:TextBox ID="txtIP" runat="server" MaxLength="128" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvIP" runat="server" Display="Dynamic" ControlToValidate="txtIP"
                    ErrorMessage="Field 'IP' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
           </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>

