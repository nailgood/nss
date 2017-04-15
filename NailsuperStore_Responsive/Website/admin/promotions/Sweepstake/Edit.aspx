<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_promotions_Sweepstake_edit" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>
  <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
    Sweepstake
</h4>

<script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>
<script type="text/javascript">

    //path vao filemanager chua dialog.aspx
    tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
    tinymce.init({
        //content_css: "/includes/tinymce_4/css/content.css",
        selector: "#<%=txtResult.ClientID %>",
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

<table border="0" cellspacing="1" cellpadding="2">
    <tr>
        <td class="required">
            Name:
        </td>
        <td class="field">
            <asp:TextBox ID="txtName" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName"
                CssClass="msgError" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>        
    <tr>
        <td class="optional">
            YouTube URL:
        </td>
        <td class="field">
            <asp:TextBox ID="txtYoutube" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
           
       </td>
    </tr>        
   
	 <tr>
        <td class="required">
            Drawing Date:
        </td>
        <td class="field">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td><CC:DatePicker ID="dprDrawingDate" runat="server" /></td>
                    <td style="padding-left:2px"><asp:TextBox id="txtTime" runat="server" height="14px" width="80px" placeholder="hh:mm:ss"></asp:TextBox><span id="msgTime" class="msgError"></span></td>
                </tr>
            </table>
        <td>
            <CC:RequiredDateValidator ID="rfvDrawingDate" runat="server" Display="Dynamic" ControlToValidate="dprDrawingDate"
                CssClass="msgError" ErrorMessage="Field 'Drawing Date' is blank"></CC:RequiredDateValidator>
            <CC:DateValidator Display="Dynamic" runat="server" id="dtvDrawingDate" ControlToValidate="dprDrawingDate" 
                CssClass="msgError" ErrorMessage="Date Field 'Drawing Date' is invalid" />
        </td>
     </tr>
	<tr>
        <td class="optional">
            Result:
        </td>
        <td class="field">
            <asp:TextBox ID="txtResult" runat="server" TextMode="MultiLine" Width="419" Height="300"></asp:TextBox>
        </td>
        <td>
            
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
        <td class="optional">
            Page Title:
        </td>
        <td class="field">
            <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="1000" TextMode="SingleLine"  Columns="50" Style="width: 419px!important;"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
            <div class="smaller" style="margin-top: 2px">
            </div>

        </td>
        <td>
         
        </td>
    </tr>    
    <tr>
        <td class="optional">
            Meta Description:
        </td>
        <td class="field">
            <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="2000" TextMode="MultiLine"  Columns="50" rows="3"  Style="width: 419px!important;"></asp:TextBox><%= Resources.Admin.lenMetaDesc%>
        </td>
        <td>
         
        </td>
    </tr>
    <tr>
        <td class="optional">
            Meta Keywords:
        </td>
        <td class="field">
            <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="2000" TextMode="MultiLine" rows="3"  Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
         
        </td>
    </tr>
</table>
<p id="pnControlBottom">    
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</p>
<script>
    var timeRegex = new RegExp('^\d{2,}:(?:[0-5]\d):(?:[0-5]\d)$');
    var originalTime = "00:10:20";
    $('#ctl00_ph_txtTime').keyup(function () {
        fnCheckTime($(this).val());

    });
    $('#ctl00_ph_txtTime').blur(function () {
        fnCheckTime($(this).val());
    });
    fnCheckTime = function(value){
        var times = value.match(/^\d{2,}:(?:[0-5]\d):(?:[0-5]\d)$/);
        if (times == null && (value != null && value != '')) {
           // alert(value);
            $('#msgTime').text('Please input format time hh:mm:ss');
            $('#ctl00_ph_btnSave').prop('disabled', true);
        }
        else {
            $('#ctl00_ph_btnSave').prop('disabled', false);
            $('#msgTime').text('');
        }
    }
</script>
<script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>

