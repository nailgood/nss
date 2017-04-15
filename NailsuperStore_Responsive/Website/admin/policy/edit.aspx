<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="Admin_EditPolicyItem"  Title="Edit Policy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %> 

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }
</script>

<script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>

    <script type="text/javascript">

        //path vao filemanager chua dialog.aspx
        tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
        tinymce.init({
            selector: "#<%=txtContent.ClientID %>",
            theme: "modern",
            menubar: true, //on/off menu editor
            plugins: [
                        "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                        "searchreplace wordcount visualblocks visualchars code fullscreen",
                        "insertdatetime media nonbreaking save table contextmenu directionality",
                        "emoticons template paste textcolor spellchecker insertdatetime tinyfilemanager.net"
                    ],
            // Theme options
            content_css: "/includes/scripts/tinymce/css/content.css",
            toolbar1: "newdocument,|,bold,italic,underline,strikethrough,|,visualchars,nonbreaking,template,pagebreak,",
            toolbar2: "styleselect,formatselect,fontselect,fontsizeselect",
            toolbar3: "cut,copy,paste,pastetext,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,insertdatetime,|,forecolor,backcolor,|,hr,removeformat,|,spellchecker",
            toolbar4: "charmap,emoticons,media,image,tinyfilemanager.net,|,preview,print,|,ltr,rtl,|,code,fullscreen",
            image_advtab: true,
            //mac dinh da remove cac the dac biet tu worđ, khai bao de remove them mot so the khac khi copy tu word sang
            paste_word_valid_elements: "b,strong,i,em,h1,h2"
        });
        
    </script>


<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="Add new Item Policy"></asp:Literal></h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Title:</td>
		<td class="field" style="width:500px"><asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Width="300px"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Message:</td>
		<td class="field"><asp:TextBox ID="txtMessage" runat="server" MaxLength="255" Width="800px" TextMode="MultiLine" Rows="2"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvMessage" runat="server" Display="Dynamic" ControlToValidate="txtMessage" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Text Link:</td>
		<td class="field"><asp:TextBox ID="txtTextLink" runat="server" MaxLength="255" Width="300px" TextMode="SingleLine"></asp:TextBox></td>
		<td></td>
	</tr>
    <tr>
		<td class="optional">Content:</td>
		<td class="field">
		    <asp:TextBox ID="txtContent" runat="server" Height="500px" Width="800px" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
    <tr>
		<td class="optional"></td>
		<td class="field"><asp:RadioButton runat="server" ID="radPopup" Checked="true" GroupName="Display" Text="Popup" />&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton runat="server" ID="radPage" Text="New Page" GroupName="Display" />
        </td>
	</tr>

    <tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" Checked="true" /></td>
	</tr>
</table>

<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</div>

<script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:content>


