<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageDropDown.ascx.vb" Inherits="ImageDropDown" %>
<td class="field">
	<asp:Image ID="imgPreview" Runat="server"></asp:Image><br>
	<asp:DropDownList ID="drpImages" Runat="server"/>
	<asp:requiredfieldvalidator id="valRequiredImage" runat="server" errormessage="Image is blank" controltovalidate="drpImages"
		Display="Dynamic" />
</td>
