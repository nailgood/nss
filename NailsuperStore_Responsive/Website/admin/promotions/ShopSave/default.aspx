<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_shopsave"  Title="Shop Save Now" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

</script>

<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List tabs"></asp:Literal></h4>

<asp:Panel ID="pnNew" runat="server" Visible="false">
<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field" style="width:500px"><asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="300px"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Page Title:</td>
		<td class="field"><asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" Width="300px" TextMode="SingleLine"></asp:TextBox><%= Resources.Admin.lenPageTitle%></td>
		<td><asp:RequiredFieldValidator ID="rfvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Outside US Page Title:</td>
		<td class="field"><asp:TextBox ID="txtOutsideUSPageTitle" runat="server" MaxLength="255" Width="300px" TextMode="SingleLine"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Meta Keyword:</td>
		<td class="field"><asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="500" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Meta Description:</td>
		<td class="field"><asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="500" Width="300px" TextMode="MultiLine"></asp:TextBox><%= Resources.Admin.lenMetaDesc%></td>
		<td><asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription" CssClass="msgError" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Outside US Meta Description:</td>
		<td class="field"><asp:TextBox ID="txtOutsideUSMetaDescription" runat="server" MaxLength="500" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
		<td></td>
	</tr>
     <tr>
		<td class="optional">Home Description:</td>
		<td class="field">
		    <asp:TextBox ID="txtShortContent" runat="server" Height="200px" Width="500px" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
    <tr>
		<td class="optional">Full Description:</td>
		<td class="field">
		    <asp:TextBox ID="txtContent" runat="server" Height="200px" Width="500px" TextMode="MultiLine"></asp:TextBox>
            <p style="font-style:italic;color:Red;">[break] is used to separate content</p>
		</td>
	</tr>
    <%If Type <> 6 Then%>
    <tr runat="server" id="imgbn" visible="false">
		<td class="optional">
            Banner:<br />
            <span class="smaller">1140 x auto</span>
        </td>
        <td class="field">
            <CC:FileUpload ID="fuBanner" AutoResize="true" 
                DisplayImage="False" runat="server" Style="width: 475px;" />
            <div runat="server" id="divBannerImg">
                <b>Preview with Map:</b><map name="hpBannermap"><asp:Literal runat="server" ID="litMapBanner" /></map>
                <div>
                    <asp:Image runat="server" ID="hpBanner" usemap="#hpBannermap" /></div>
            </div>
        </td>
        <td>
            <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feBanner"
                runat="server" Display="Dynamic" ControlToValidate="fuBanner" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
        </td>
	</tr>
	<tr>
		<td class="required">
           Home Banner:<br />
            <span id="imgsize" class="smaller">220 x 220</span>
             <br />
                < <span id="imglength">15</span>kb
        </td>
        <td class="field">
            <CC:FileUpload ID="fuImage" AutoResize="true" Folder="/assets/shopsave/home/" ImageDisplayFolder="/assets/shopsave/home/"
                DisplayImage="False" runat="server" Style="width: 200px;" />
                 <div id="msgError" class="msgError"><asp:Literal ID="ltMssImage" runat="server"></asp:Literal></div>
            <div runat="server" id="dvhBanner">
                <b>Preview with Map:</b><map name="maphBanner"><asp:Literal runat="server" ID="litMaphBanner" /></map>
                <div>
                    <asp:Image runat="server" ID="hpimg" usemap="#maphBanner" /></div>
            </div>
        </td>
        <td>
            <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="FileUploadExtensionValidator1"
                runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
              
        </td>
	</tr>
      <tr>
            <td class="required">
                Mobile Image File:<br />
                <span id="imgsize1">200x200</span>
                 <br />
                < <span id="imglength1">12</span>kb
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImageMobile" EnableDelete="false" AutoResize="true" Folder="/assets/shopsave/home/mobile/" ImageDisplayFolder="/assets/shopsave/home/mobile" DisplayImage="false" runat="server" />
                   <div id="msgError1" class="msgError"><asp:Literal ID="ltmsgimage1" runat="server"></asp:Literal></div>
                <div runat="server" id="divImg1">
                    <b>Preview with Map:</b><map name="hpimgmap1"><asp:Literal runat="server" ID="litmap1" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg1" usemap="#hpimgmap1" /></div>
                        (This image will be display on live web)
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="FileUploadExtensionValidator2"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
               
            </td>
        </tr>
	<%End If%>
    <tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" Checked="true" /></td>
	</tr>
	  <%If Type <> 6 Then%>
	<tr>
		<td class="optional">Url:</td>
		<td class="field" style="width:500px"><asp:TextBox ID="txtUrl" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
		<br /><span class="smaller">Ex: /store/coupon.aspx</span></td>
		<td></td>
	</tr>

    <%End If%>
</table>

<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>
</asp:Panel>

<asp:Panel ID="pnList" runat="server">

<asp:Repeater ID="rptShopSave" runat="server">
    <HeaderTemplate>
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:10px 0">
            <tr style="height:25px">
                <th style="width:200px">Name</th>
                <th style="width:60px">Products</th>
                <% If Type <> 6 Then %><th style="width:50px">Active</th><% End If %>
                <th style="width:50px">Edit</th>
                <th style="width:50px">Delete</th>
                <% If Type <> 6 Then %><th style="width:50px">Arrange</th><% End If %>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row"><asp:Literal ID="litName" runat="server"></asp:Literal></td>
            <td class="row" align="center"><a href="items.aspx?ShopSaveId=<%#Container.DataItem.ShopSaveId%>&TabName=<%#Container.DataItem.Name%>"><img src="/includes/theme-admin/images/Create.gif" style="border:0px" /></a></td>
            <% If Type <> 6 Then %><td class="row" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td><% End If %>
            <td class="row" align="center"><asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" CommandName="Edit" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            <td class="row" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.ShopSaveId%>" OnClientClick="return ConfirmDelete();" /></td>
            <% If Type <> 6 Then %>
              <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
            <% End If %>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate"><asp:Literal ID="litName" runat="server"></asp:Literal></td>
            <td class="alternate" align="center"><a href="items.aspx?ShopSaveId=<%#Container.DataItem.ShopSaveId%>&TabName=<%#Container.DataItem.Name%>"><img src="/includes/theme-admin/images/Create.gif" style="border:0px" /></a></td>
            <% If Type <> 6 Then %><td class="alternate" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td><% End If %>
            <td class="alternate" align="center"><asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" CommandName="Edit" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            <td class="alternate" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.ShopSaveId%>" OnClientClick="return ConfirmDelete();" /></td>
            <% If Type <> 6 Then %>
              <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
            <% End If %>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

<asp:HiddenField ID="hidId" runat="server" />
<asp:HiddenField ID="hidBanner" runat="server" />
<asp:HiddenField ID="hidHomeBanner" runat="server" />
<asp:HiddenField ID="hidMobileBanner" runat="server" />
<CC:OneClickButton id="btnAddNew" runat="server" Text="Add new" cssClass="btn" Visible="false"></CC:OneClickButton>
<CC:OneClickButton id="btnEditMetaTag" Runat="server" Text="Edit Meta Tags" cssClass="btn"></CC:OneClickButton>
</asp:Panel>

</div>
<script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:content>


