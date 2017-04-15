<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_navision_mixmatch_Edit" Title="Mix Match" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <%-- <script language="javascript">
      function ReloadFrame(name)
      {
     
       if(name=='load'){
       location.reload();}
       }
      </script>--%>
    <script src="/includes/scripts/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript" language="javascript">

        // document.parent.frames['main'].document.refresh();
        tinyMCE.init({
            // General options
            mode: "exact",
            elements: "<%=txtTextHtml.ClientID %>",
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
    <h4>
        <% If Id = 0 Then%>Add new
        <% Else%>Edit
        <% End If%>
        <asp:Literal ID="ltrHeader" runat="server"></asp:Literal></h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Main Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMainTitle" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvMainTitle" runat="server" Display="Dynamic" ControlToValidate="txtMainTitle"
                    CssClass="msgError" ErrorMessage="Field 'MainTitle' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required" style="color: Black">
                Sub Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvSubTitle" runat="server" Display="Dynamic" ControlToValidate="txtSubTitle"
                    CssClass="msgError" ErrorMessage="Field 'Sub Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Link Page:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLinkPage" runat="server" MaxLength="1000" Columns="50" Style="width: 419px;"></asp:TextBox>
                <span class="smaller">Ex: /store/coupon.aspx</span>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Start Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvStartingDate" ControlToValidate="dtStartingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                End Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvEndingDate" ControlToValidate="dtEndingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" />
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
        <%--   <tr>
		<td class="required"><b>Is Homepage?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsHomePage" AutoPostBack="true" /></td>
	</tr>
	<tr>
		<td class="required"><b>Is ReSource?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkResource" AutoPostBack="true" /></td>
	</tr>--%>
        <tr id="trDepartment" runat="server" visible="false">
            <td valign="top" class="required">
                <b>Departments:</b>
            </td>
            <td class="field" width="485">
                Please select below all departments that this item belongs to.<br>
                <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="false" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Image File:<br />
                <span class="smaller">
                    <% If iType = 3 Or iType = 4 Then%>475 x 205
                    <%ElseIf iType = 1 Then%>
                    width=235
                    <%ElseIf iType = 0 Then%>
                    1140 x auto
                    <% End If%></span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete=false
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                    <span style="color: Red">
                        <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator><span style="color: Red"><asp:Literal
                        ID="ltMessage" runat="server"></asp:Literal></span>
            </td>
        </tr>
        <tr id="trMobileImage" runat="server">
            <td class="optional">
               Mobile Image File:<br />
                <span class="smaller">768 x auto</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuMobileImage"  EnableDelete=false
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                (This image will be display on mobile device)<div runat="server" id="divImgMobile">
                    <b>Preview with Map:</b><map name="hpimgmapmobile"><asp:Literal runat="server" ID="litMapMobile" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimgMobile" usemap="#hpimgmapmobile" /></div>
                    <span style="color: Red">
                        <asp:Literal ID="ltMssImageMobile" runat="server"></asp:Literal></span>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImageMobile"
                    runat="server" Display="Dynamic" ControlToValidate="fuMobileImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator><span style="color: Red"><asp:Literal
                        ID="ltImageMobileMessage" runat="server"></asp:Literal></span>
            </td>
        </tr>
        <tr>
            <td class="optional">
                TextHtml:<div class="smaller" style="font-weight: normal;">
                    Promtotion</div>
            </td>
            <td class="field">
                <asp:TextBox ID="txtTextHtml" Width="600px" Height="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Mix Match Promotion?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>
