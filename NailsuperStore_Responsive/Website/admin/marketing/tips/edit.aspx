<%@ Page Language="VB" ValidateRequest="False" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_tips_Edit"  Title="Tips"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>
<script type="text/javascript">

    //path vao filemanager chua dialog.aspx
    tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
    tinymce.init({
        //content_css: "/includes/tinymce_4/css/content.css",
        selector: "#<%=txtFullText.ClientID %>,#<%=txtVietText.ClientID %>",
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
	 
<h4><% If TipId = 0 Then %>Add<% Else %>Edit<% End If %> Tip</h4>


<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr id="Tr1" runat="server" visible="false">
		<td class="required">Tip Category Id:</td>
		<td class="field"><asp:DropDownList id="drpTipCategoryId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvTipCategoryId" runat="server" Display="Dynamic" ControlToValidate="drpTipCategoryId" CssClass="msgError" ErrorMessage="Field 'Tip Category Id' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Title:</td>
		<td class="field"><asp:textbox id="txtTitle" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Summary:</td>
		<td class="field"><asp:textbox id="txtSummary" runat="server" maxlength="300" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvSummary" runat="server" Display="Dynamic" ControlToValidate="txtSummary" CssClass="msgError" ErrorMessage="Field 'Summary' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Full Text:</td>
		<td class="field"><asp:TextBox TextMode="MultiLine" id="txtFullText" runat="server" Width="600" Height="300" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Viet Title:</td>
		<td class="field"><asp:textbox id="txtVietTitle" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
	</tr>
	<tr>
		<td class="optional">Viet Summary:</td>
		<td class="field"><asp:textbox id="txtVietSummary" runat="server" maxlength="300" columns="50" style="width: 319px;"></asp:textbox></td>
	</tr>
	<tr>
		<td class="optional">Viet Text:</td>
		<td class="field"><asp:TextBox TextMode="MultiLine" id="txtVietText" runat="server" Width="600" Height="300" /></td>
	</tr>
	<tr>
	  <td class="optional">Page Title </td>
	  <td class="field"><asp:TextBox runat="server" ID="txtPageTitle" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
  </tr>
	<!--tr>
	  <td class="optional">Meta Title </td>
	  <td class="field"><asp:TextBox runat="server" ID="txtMetaTitle" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
  </tr-->
	<tr>
		<td class="optional">Meta Description:</td>
		<td class="field"><asp:TextBox runat="server" ID="txtMetaDescription" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
	</tr>
	<tr>
		<td class="optional">Meta Keywords:</td>
		<td class="field"><asp:TextBox runat="server" ID="txtMetaKeywords" TextMode="MultiLine" Rows="5" Columns="40" style="width:319px;" /></td>
	</tr>	
    <tr>
        <td class="optional">
            <b>Is Active?</b>
        </td>
        <td class="field">
            <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
        </td>
    </tr>
</table>
<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Tip?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

