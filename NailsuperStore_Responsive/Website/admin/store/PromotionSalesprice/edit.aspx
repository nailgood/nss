<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_mixmatch_Edit"  Title="Mix Match"%>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
	<script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>  

    <script type="text/javascript" language="javascript">

        tinyMCE.init({
            // General options
            mode: "exact",
            elements: "#<%=txtTextHtml.ClientID %>",
            theme: "advanced",

            // Theme options

            plugins: "spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

            // Theme options
            theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",

            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left"
        });

    </script>
<h4><% If Id = 0 Then %>Add<% Else %>Edit<% End If %> Top Promotion Link </h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">SubTitle:</td>
		<td class="field"><asp:textbox id="txtSubTitle" runat="server" maxlength="255" columns="50" style="width: 419px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvSubTitle" runat="server" Display="Dynamic" ControlToValidate="txtSubTitle" CssClass="msgError" ErrorMessage="Field 'Sub Title' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">MainTitle:</td>
		<td class="field"><asp:textbox id="txtMainTitle" runat="server" maxlength="255" columns="50" style="width: 419px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvMainTitle" runat="server" Display="Dynamic" ControlToValidate="txtMainTitle" CssClass="msgError" ErrorMessage="Field 'MainTitle' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">LinkPage:</td>
		<td class="field"><asp:textbox id="txtLinkPage" runat="server" maxlength="255" columns="50" style="width: 419px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvLinkPage" runat="server" Display="Dynamic" ControlToValidate="txtLinkPage" CssClass="msgError" ErrorMessage="Field 'LinkPage' is blank"></asp:RequiredFieldValidator></td>
	</tr>

	<tr>
		<td class="optional">Starting Date:</td>
		<td class="field"><CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvStartingDate" ControlToValidate="dtStartingDate" CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Ending Date:</td>
		<td class="field"><CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvEndingDate" ControlToValidate="dtEndingDate" CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="required"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" Checked="true" /></td>
	</tr>
	<tr style="display:none">
		<td class="required"><b>Is Homepage?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsHomePage" AutoPostBack="true" /></td>
	</tr>
	<tr style="display:none">
		<td class="required"><b>Is ReSource?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkResource" AutoPostBack="true" /></td>
	</tr>
	<tr style="display:none">
		<td valign="top" class="required"><b>Departments:</b></td>
		<td class="field" width="485">
			Please select below all departments that this item belongs to.<br>
		   <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true" />
		</td>
	</tr>
	<tr>
		<td class="optional">Image File:<br /><span class="smaller">475 x 205</span></td>
		<td class="field"><CC:FileUpload ID="fuImage" AutoResize=true Folder="/assets/SalesPrice" ImageDisplayFolder="/assets/SalesPrice" DisplayImage="False" runat="server" style="width: 475px;" /><div runat="server" id="divImg">
			<b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
			<div><asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
		</div></td>
		<td><CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">TextHtml:<div class="smaller" style="font-weight:normal;">Promtotion</div></td>
		<td class="field">
		  <asp:TextBox ID="txtTextHtml"  Width="600px" Height="300px"  runat="server" TextMode="MultiLine"></asp:TextBox>
                
		</td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Mix Match Promotion?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

