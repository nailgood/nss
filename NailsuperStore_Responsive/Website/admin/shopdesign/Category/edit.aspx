<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_shopdesign_Category_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Category</h4>
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
             <td class="optional">
                Parent:
            </td>
            <td class="field">
                <asp:DropDownList ID="drlParent" runat="server"></asp:DropDownList>
            </td>
            <td>               
            </td>
        </tr>
        <tr>
            <td class="required">
                Category Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="128" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtName"
                    CssClass="msgError" ErrorMessage="Field 'Category Name' is blank"></asp:RequiredFieldValidator>
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
            <td class="required">
                Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                 <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    CssClass="msgError" ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
             Meta Keywords:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="1000" TextMode="MultiLine" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                 <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword"
                    CssClass="msgError" ErrorMessage="Field 'Meta Keyword' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
               <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                    CssClass="msgError" ErrorMessage="Field 'Meta Description' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>

