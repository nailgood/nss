<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="admin_Video_Category_Edit" MasterPageFile="~/includes/masterpage/admin.master" %>


<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
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
            <td class="required">
                Category Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="128" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvTitle" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtName"
                    ErrorMessage="Field 'Category Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr> 
        <tr>
            <td class="optional">
                Image File:<br />
                <span class="smaller">754 x 100</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" AutoResize="true" Folder="/assets/video/banner/category/" ImageDisplayFolder="/assets/video/banner/category/"
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" CssClass="msgError"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
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
                <asp:RequiredFieldValidator ID="refvPageTitle" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
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
                  <asp:RequiredFieldValidator ID="refvMetaKeyword" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword"
                    ErrorMessage="Field 'Meta Keyword' is blank"></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="refvMetaDescription" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                    ErrorMessage="Field 'Meta Description' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
     
</asp:Content>
