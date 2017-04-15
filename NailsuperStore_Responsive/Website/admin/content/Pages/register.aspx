<%@ Page ValidateRequest="false" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" 
    AutoEventWireup="false" CodeFile="register.aspx.vb" Inherits="admin_content_pages_register"
    Title="Register Existing page" %>

<asp:Content ID="Content" ContentPlaceHolderID="ph" runat="Server">
    <script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>
    <script type="text/javascript">
        //path vao filemanager chua dialog.aspx
        tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
        tinymce.init({
            selector: "textarea#<%=txtContent.ClientID %>",
            content_css: "/includes/theme/css/style.css",
            width: 640,
            height: 300,
            plugins: [
         "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
         "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
         "save table contextmenu directionality emoticons template paste textcolor"
   ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | l      ink image | print preview media fullpage | forecolor backcolor emoticons",
            style_formats: [
        { title: 'Bold text', inline: 'b' },
        { title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
        { title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
        { title: 'Example 1', inline: 'span', classes: 'example1' },
        { title: 'Example 2', inline: 'span', classes: 'example2' },
        { title: 'Table styles' },
        { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
    ]

    });
    </script>
    <table>
        <tr>
            <td valign="top" colspan="2">
                <font class="red">red color</font> - required fields
            </td>
        </tr>
        <tr>
            <td class="required">
                <strong>Page Name:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvName" runat="server" CssClass="msgError" ErrorMessage="Page NameL cannot be blank"
                    ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                <strong>Page URL:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageURL" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvURL" runat="server" CssClass="msgError" ErrorMessage="Page URL cannot be blank"
                    ControlToValidate="txtPageURL"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Navigation:</strong>
            </td>
            <td class="field">
                <asp:DropDownList ID="drlNavigation" runat="server"></asp:DropDownList>
            </td>
            <td>
              
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Navigation Text:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtNavigationText" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
              
            </td>
        </tr>
        <tr>
            <td class="optional">
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsShowContent" runat="server" AutoPostBack="True" Text="Show Content" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkIsFullScreen" runat="server" Text="Full Screen" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>H1 Title:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" Width="640px"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
            </td>
            <td>
            </td>
        </tr>
        <tr id="trContent" runat="server">
            <td class="optional">
                <strong>Content:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtContent" runat="server" Height="400px" Width="400px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Meta Title:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaTitle" runat="server" Width="640px"></asp:TextBox><%= Resources.Admin.lenMetaTitle%>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Meta Keywords:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeywords" runat="server" Width="640px" TextMode="MultiLine"
                    Height="80px"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Meta Description:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" Width="640px" TextMode="MultiLine"
                    Height="80px"></asp:TextBox><%= Resources.Admin.lenMetaDesc%>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsIndexed" runat="server" Text="Index  this page" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkIsFollowed" runat="server" Text="Follow links on this page" />
            </td>
            <td>
            </td>
        </tr>
    </table>
    <p>
        <CC:OneClickButton ID="btnSave" runat="server" CssClass="btn" Text="Save"></CC:OneClickButton>&nbsp;
        <CC:OneClickButton ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" CausesValidation="False">
        </CC:OneClickButton>
    </p>
    <script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>
