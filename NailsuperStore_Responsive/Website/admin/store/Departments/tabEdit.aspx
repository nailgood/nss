<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tabEdit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_Departments_tabEdit" Title="Department" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If DepartmentTabId = 0 Then%>Add<% Else%>Edit<% End If%>
        Department Tab</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Main department:
            </td>
            <td class="field">
                <asp:DropDownList ID="ddlDepartment" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="reqDdlDepartment" runat="server" ErrorMessage="*"
                    ControlToValidate="ddlDepartment" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName"
                    ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                URL Code:
            </td>
            <td class="field">
                <asp:TextBox ID="txtURLCode" runat="server" MaxLength="30" Width="300px"></asp:TextBox><br />
                <span class="smaller">If name="Customer Favorites" should be entered Url Code="customer-favorites"</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ControlToValidate="txtURLCode" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidator1" CssClass="msgError" runat="server" OnServerValidate="ServerValidationTabURLCode"
                    ControlToValidate="txtURLCode" ErrorMessage="URL Code is exists."></asp:CustomValidator>
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
        <tr>
            <td class="required">
                Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
                   <%= Resources.Admin.lenPageTitle%>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Outside US Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Keywords:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic"
                    ControlToValidate="txtMetaKeyword" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
                <%= Resources.Admin.lenMetaDesc%>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic"
                    ControlToValidate="txtMetaDescription" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Outside US Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOutsideUSMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
		<td class="optional">Description:</td>
		<td class="field">
		    <asp:TextBox ID="txtDescription" runat="server" Height="200px" Width="500px" TextMode="MultiLine"></asp:TextBox>
		</td>
	    </tr>
        <tr>
            <td class="optional">
                Image :<br />
                <span id="imgsize" class="smaller">220 x 220</span>
                <br />
                < <span id="imglength">15</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" Folder="/assets/departments/tab" ImageDisplayFolder="/assets/departments/tab"
                    DisplayImage="True" runat="server" Style="width: 319px;" />
                    <div id="msgError" class="msgError"></div>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp,swf" ID="feLargeImage"
                    CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="fuImage"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
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
