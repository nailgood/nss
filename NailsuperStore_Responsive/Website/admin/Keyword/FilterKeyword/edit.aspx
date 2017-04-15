<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_Keyword_FilterKeyword_edit" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>Filter keyword</h4>
    <table>
        <tr>
            <td valign="top" colspan="2">
                <font class="red">red color</font>- required fields
            </td>
        </tr>
         <tr style="display:none">
            <td class="required" style="width: 110px;">
                <strong>Filter Type:</strong>
            </td>
            <td class="field">
                <asp:DropDownList AutoPostBack="true" ID="ddlFilterType" runat="server" Width="300px" 
                    >
                    <asp:ListItem Text="Replace Keyword" Value="ReplaceKeyword"/>
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvFilterType" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="ddlFilterType"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            
            <td class="required" style="width: 300px;">
                <strong>Keyword Name:</strong>
            </td>
             <td class="required" style="width: 110px;">
                <strong>Original Keyword:</strong>
            </td>
        </tr>
        <tr>
            <td class="field">
                 <asp:ListBox id="lbxKwName" OnSelectedIndexChanged="lbxKwName_SelectedIndexChanged"
           Rows="10" AutoPostBack="true"
           Width="300px"
           SelectionMode="Single" 
           runat="server">
                </asp:ListBox>
                <asp:RequiredFieldValidator ID="refvName" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="lbxKwName">
                </asp:RequiredFieldValidator>
            </td>
            <td class="field">
                <asp:ListBox ID="lbxOriginalKeyword" Rows="10" runat="server" Width="300px" OnSelectedIndexChanged="lbxOriginalKeyword_SelectedIndexChanged1"></asp:ListBox>
               <%-- <asp:RequiredFieldValidator ID="refvOriginalKw" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="lbxOriginalKeyword"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Filter By:</strong>
            </td>
             
            <td class="field">
                <asp:DropDownList AutoPostBack="true" ID="ddlFilterProperty" runat="server" Width="300px" OnSelectedIndexChanged="ddlFilterProperty_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvFilterProperty" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Filter property cannot be blank" ControlToValidate="ddlFilterProperty"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
           <td class="required" style="width: 80px;">
                <strong>Filter Value:</strong>
            </td>
            <td class="field">
                <asp:DropDownList ID="ddlValue" runat="server" Width="300px"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvFilterValue" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="value cannot be blank" ControlToValidate="ddlValue"></asp:RequiredFieldValidator>
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
