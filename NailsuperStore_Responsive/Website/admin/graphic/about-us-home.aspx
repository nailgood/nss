<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="about-us-home.aspx.vb" Inherits="admin_graphic_about_us_home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>
    <script type="text/javascript">

        //path vao filemanager chua dialog.aspx
        tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
        tinymce.init({
            //content_css: "/includes/tinymce_4/css/content.css",
            selector: "#<%=txtDescription.ClientID %>",
            theme: "modern",
            menubar: true, //on/off menu editor
            plugins: [
                        "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                        "searchreplace wordcount visualblocks visualchars code fullscreen",
                        "insertdatetime media nonbreaking save table contextmenu directionality",
                        "emoticons template paste textcolor spellchecker insertdatetime tinyfilemanager.net"
                    ],
            // Theme options
            toolbar1: "newdocument,|,bold,italic,underline,strikethrough,|,visualchars,nonbreaking,template,pagebreak,",
            toolbar2: "styleselect,formatselect,fontselect,fontsizeselect",
            toolbar3: "cut,copy,paste,pastetext,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,insertdatetime,|,forecolor,backcolor,|,hr,removeformat,|,spellchecker",
            toolbar4: "charmap,emoticons,media,image,tinyfilemanager.net,|,preview,print,|,ltr,rtl,|,code,fullscreen",
            image_advtab: true,
            //mac dinh da remove cac the dac biet tu worđ, khai bao de remove them mot so the khac khi copy tu word sang
            paste_word_valid_elements: "b,strong,i,em,h1,h2"
        });
        
    </script>
    <h4>About Us Home</h4>
<table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                Left Text:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="500" Width="600px"  TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvName" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                Right Text:
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" runat="server" Width="600px" Height="240px" MaxLength="500"
                    TextMode="MultiLine"></asp:TextBox>
                    <p style="font-style:italic;color:Red;">[break] is used to separate content</p>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvDesc" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Description cannot be blank" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Link:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLink" runat="server" MaxLength="200" Columns="50" Style="width:600px;"></asp:TextBox>
                <br />
                <span class="smaller">Ex: /store/dealday.aspx</span>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="refvLink" ValidationGroup="inforbanner" runat="server"
                    CssClass="msgError" ErrorMessage="Link cannot be blank" ControlToValidate="txtLink"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image File:<br />
                <span id="imgsize">130x130</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/"
                    ImageDisplayFolder="/assets/Banner" DisplayImage="false" runat="server" />
                <div id="msgError" class="msgError">
                    <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></div>
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,jpg,jpeg,gif,bmp,png" ID="feImage"
                    runat="server" Display="Dynamic" ValidationGroup="inforbanner" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
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
    </table>
    <p>
    </p>
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn" />
    <asp:Button ID="btnDelete" runat="server" Message="Are you sure want to delete this Sales Banner?" Text="Delete" CssClass="btn" CausesValidation="False" />
</asp:Content>

