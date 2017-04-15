<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="ASP20.NET" CodeFile="edit.aspx.vb" Inherits="admin__faq_edit" %>

<asp:content ID="Content" runat="server" contentplaceholderid="ph">

<h4>FAQ - Add / Edit Question</h4>

<table cellSpacing="2" cellPadding="3" border="0">
	<tr>
		<td colSpan="2"><span class="red">red color</span> - required fields</td>
	</tr>
	<tr>
		<td class="required"><b>Question:</b></td>
		<td class="field" width="300"><asp:textbox id="txtQuestion" TextMode="MultiLine" runat="server" rows="5" columns="50" /></td>
		<td><asp:requiredfieldvalidator id="rqQuestion" runat="server" CssClass="msgError" ErrorMessage="Question is blank" ControlToValidate="txtQuestion" Display="Dynamic" /></td>
	</tr>
	<tr>
		<td class="required"><b>Answer:</b></td>
		<td class="field" width="300"><asp:textbox id="txtAnswer" TextMode="MultiLine" runat="server" rows="5" columns="50" /></td>
		<td><asp:requiredfieldvalidator id="rqAnswer" runat="server" CssClass="msgError" ErrorMessage="Answer is blank" ControlToValidate="txtAnswer" Display="Dynamic" /></td>
	</tr>
	<tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field" width="300"><asp:CheckBox ID="chkIsActive" Runat="server" /></td>
		<td>&nbsp;</td>
	</tr>
</table>

<p>
<asp:button id="Save" runat="server" Text="Save" cssClass="btn"></asp:button>
<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this question?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>
</p>
				
</asp:content>
