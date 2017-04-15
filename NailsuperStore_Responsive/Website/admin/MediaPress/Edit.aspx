<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="Edit.aspx.vb" Inherits="admin_MediaPress_Edit" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" runat="Server">
 
  <script src="/includes/scripts/tinymce/tinymce.min.js"></script>
   <script type="text/javascript">
       tinyMCE.init({
           theme: "advanced",
           mode: "exact",
           elements: "#<%=txtShortDescription.ClientID %>",
           plugins: "bbcode,insertdatetime,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",
           theme_advanced_buttons1: "bold,italic,underline,link,unlink,pastetext",
           theme_advanced_buttons2: "",
           theme_advanced_buttons3: "",
           
           theme_advanced_toolbar_location: "top",
           theme_advanced_toolbar_align: "center",
           theme_advanced_styles: "Code=codeStyle;Quote=quoteStyle",
           content_css: "bbcode.css",
           entity_encoding: "raw",
           force_br_newlines: true,
           force_p_newlines: false,
           forced_root_block: '', // Needed for 3.x
           add_unload_trigger: false,
           remove_linebreaks: false
       });

</script>
    <h4>
        <% If ID = 0 Then%>Add<% Else%>Edit<% End If%>
        Media/Press</h4>
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
                <asp:DropDownList ID="ddlCategory" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvCategory" runat="server" Display="Dynamic" ControlToValidate="ddlCategory"
                    CssClass="msgError" ErrorMessage="Field 'Category' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtTitle" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle"
                    CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        
        <tr>
            <td class="optional">
                Short Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtShortDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px; height:120px"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image:<br />
                <span class="smaller">width=754</span>
                <br />
            </td>
            <td class="field">
                <CC:FileUpload ID="fuThumb" AutoResize="true" ImageDisplayFolder="~/assets/media/thumb/" DisplayImage="False" runat="server"
                    Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image Width="200" runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server"
                    Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr id="trVideo" visible="false" runat="server">
            <td class="optional">
            </td>
            <td id="Td1" colspan="2" runat="server">
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
                    Columns="50" Style="width: 419px;"></asp:TextBox>
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
                <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
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
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
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
