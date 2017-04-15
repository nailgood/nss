<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_themes_Edit"  Title="Theme"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ThemeId = 0 Then %>Add<% Else %>Edit<% End If %> Theme</h4>

<div style="border:solid 2px red;background-color:#ffeeee; width:300px;padding:10px;margin-bottom:10px;"><b>NOTE:</b> <span class="bold red">Any unentered values will use the default</span></div>

<table border="0" cellspacing="3" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Theme:</td>
		<td class="field"><asp:textbox id="txtTheme" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvTheme" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtTheme" ErrorMessage="Field 'Theme' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr runat="server" id="trActive">
		<td class="required"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
	<tr>
		<td class="required">Header Logo:<div class="smaller">119 x 97</div></td>
		<td class="field"><CC:FileUpload ID="fuLogo" Folder="/assets/theme/logo" ImageDisplayFolder="/assets/theme/logo" DisplayImage="True" runat="server" style="width: 319px;" /></td>
	</tr>
	<tr>
		<td class="required">Footer Logo:<div class="smaller">44 x 36</div></td>
		<td class="field"><CC:FileUpload ID="fuLogoFooter" Folder="/assets/theme/logo/small" ImageDisplayFolder="/assets/theme/logo/small" DisplayImage="True" runat="server" style="width: 319px;" /></td>
	</tr>
	<tr>
		<td class="required">Tagline:<div class="smaller">139 x 33</div></td>
		<td class="field"><CC:FileUpload ID="fuTagline" Folder="/assets/theme/tagline" ImageDisplayFolder="/assets/theme/tagline" DisplayImage="True" runat="server" style="width: 319px;" /></td>
	</tr>
	<tr>
		<td colspan="3" class="optional">
			<h4 style="margin-top:5px;margin-bottom:5px;">CSS Styles</h4>
		</td>
	</tr>
	<asp:Repeater runat="server" ID="rptClasses">
		<ItemTemplate>
			<tr>
			<td colspan="3" class="optional">
			<table border="0" cellspacing="0" cellpadding="2" width="100%">
			<tr>
				<td colspan="2">
					<table border="0" cellpadding="0" cellspacing="0" width="100%">
						<tr>
							<td>
								<b><%#Container.dataItem("ClassName")%></b>
								<asp:Label runat="server" ID="lblCssClassId" Visible="false" Text='<%#Container.DataItem("CssClassId")%>' />
							</td>
							<td align="right">
								<asp:Literal runat="server" ID="litImg" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr class="alternate">
			<td colspan="2" style="padding-left:10px;">
			<table border="0" cellspacing="0" cellpadding="2">
			<asp:Repeater runat="server" ID="rptProperties">
				<ItemTemplate>
					<tr class="alternate">
						<td><%#Container.DataItem("Property")%>:<asp:Label runat="server" ID="lblPropertyId" Visible="false" Text='<%#Container.DataItem("PropertyId")%>' /></td>
						<td runat="server" id="tdEdit"><asp:TextBox runat="server" ID="txtProperty" value='<%#Container.DataItem("value")%>' /><div class="smaller"><b>Default Value:</b> <span class="red"><%#Container.DataItem("DefaultValue")%></span></div></td>
						<td runat="server" id="tdReadOnly"><span style="color:#be048c;" runat="server" id="spanValue"><%#Container.DataItem("defaultvalue")%></span></td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>
			</table>
			</td>
			</tr>
			</table>
			</td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Theme?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

