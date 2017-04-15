<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Store Order" CodeFile="SendEmail.aspx.vb" Inherits="admin_store_cartDetail_sendEmail"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Send Email</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSendEmail" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Email:</th>
<td valign="top" class="required"><asp:textbox id="F_Email" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
<td><asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="F_Email" ErrorMessage="Field 'Email' is blank"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<th valign="top">Subject:</th>
<td valign="top" class="required"><asp:textbox id="F_Subject" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
<td><asp:RequiredFieldValidator ID="rfvSubject" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="F_Subject" ErrorMessage="Field 'Subject' is blank"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<th valign="top">Content:</th>
<td valign="top" class="required"><asp:textbox id="F_Content" runat="server" Columns="20" Height="400px" TextMode="MultiLine" Width="500px"></asp:textbox></td>
<td><asp:RequiredFieldValidator ID="rfvContent" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="F_Content" ErrorMessage="Field 'Content' is blank"></asp:RequiredFieldValidator></td>
</tr>

<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSendEmail" Runat="server" Text="Send Email" cssClass="btn" />

<CC:OneClickButton id="btnClear" CausesValidation="false" Runat="server" Text="Clear" cssClass="btn" />

</td>
</tr>
</table>
</asp:Panel>
<p></p>


<div runat="server" id="divDownload" visible="false">
<asp:HyperLink id="lnkDownload" runat="server">Download File</asp:HyperLink>
</div>


</asp:content>

