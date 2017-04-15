<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Async="true" Inherits="admin_Video_Video_Edit"
    MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Video</h4>
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
                Category:
            </td>
            <td class="field">
                <CC:CheckBoxList runat="server" ID="cblCategory" RepeatColumns="1" CellPadding="0"
                    CellSpacing="1" />
               
            </td>
            <td>
                <asp:Label ID="lblCategory" runat="server" CssClass="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="required">
                Video Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtName"
                    ErrorMessage="Field 'Video Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Video URL:
            </td>
            <td class="field">
                <asp:TextBox ID="txtURL" runat="server" MaxLength="255" TextMode="SingleLine" Columns="50"
                    Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvURL" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtURL"
                    ErrorMessage="Field 'Video URL' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Sub Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255" TextMode="MultiLine" Columns="50" Rows="2" Style="width: 419px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtShortDescription" runat="server" MaxLength="1000" TextMode="MultiLine" Rows="10" Style="width: 800px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr >
            <td class="optional">
                Thumb Image:<br />
                 <span class="smaller">176 x 99</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuThumb" AutoResize="true"  DisplayImage="False" runat="server"
                    Style="width: 200px;" /><div></div><i><span class="smallest">Image available: 
                        <asp:Label ID="lbUploadfile" runat="server" Text=""></asp:Label></span></i></div>
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,gif" ID="feImage" runat="server" CssClass="msgError"
                    Display="Dynamic" ControlToValidate="fuImage" 
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr  id="trVideo" visible="false" runat="server">
            <td class="optional">
            </td>
            <td colspan="2" runat="server">
                <asp:Literal ID="ltrVideo" runat="server"></asp:Literal>
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
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
            </td>
            <td>    <asp:RequiredFieldValidator ID="refvPageTitle" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                    ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
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
             <asp:RequiredFieldValidator ID="refvMetaKeyword" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword"
                    ErrorMessage="Field 'Meta Keyword' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox><%= Resources.Admin.lenMetaDesc%>
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
    <script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>
