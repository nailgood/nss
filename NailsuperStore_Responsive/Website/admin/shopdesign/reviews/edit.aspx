<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_shopdesign_reviews_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4 id="pageTitle" runat="server">Shop Design Review</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Shop Design:
            </td>
            <td class="field">
                <asp:HyperLink ID="hplShopDesign" runat="server"></asp:HyperLink>
            </td>
            <td>
            </td>
        </tr>
          <tr id="trParentComment" runat="server">
            <td class="optional" style="width:100px;">
                 Comment:
            </td>
            <td class="field">
               <asp:Label ID="lblParentComment" runat="server"></asp:Label>
            </td>
            <td valign="top">
             
            </td>
        </tr>
        <tr>
            <td class="optional">
               <asp:Label ID="lblUserName" runat="server"></asp:Label>
            </td>
            <td class="field">
                <asp:HyperLink ID="hplEmail" runat="server"></asp:HyperLink>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                   <asp:Label ID="lblLabelDate" runat="server"></asp:Label>
            </td>
            <td class="field">
                <asp:Label runat="server" ID="lblDate"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
       
        <tr>
            <td class="required" style="width:100px;">
                 <asp:Label runat="server" ID="lblComment"></asp:Label>
            </td>
            <td class="field">
                <textarea id="txtComment" runat="server" style="width: 500px; height: 200px"></textarea>
            </td>
            <td valign="top">
                <asp:RequiredFieldValidator ID="rfvComment" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtComment"
                    ErrorMessage="Field 'Comment' is blank"></asp:RequiredFieldValidator>
                                    
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" onclick="CheckActive(this.checked);" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CausesValidation="true" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this comment?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>

