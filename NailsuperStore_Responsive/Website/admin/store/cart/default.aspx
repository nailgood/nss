<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Shopping Cart Abandonment Metrics" CodeFile="default.aspx.vb" Inherits="admin_store_cart__default"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Shopping Cart Abandonment Metrics</h4>

<span class="smaller">Please provide search criteria below</span>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top" class="required"><b>Order Create Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_OrderDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_OrderDateUbound" runat="server" /></td>
</tr>
</table>
</td>
<td><CC:RequiredDateValidator runat="server" CssClass="msgError" ControlToValidate="F_OrderDateLbound" Display="dynamic" ErrorMessage="Please enter a From date" /><CC:RequiredDateValidator CssClass="msgError" runat="server" ControlToValidate="F_OrderDateUbound" Display="dynamic" ErrorMessage="Please enter a To date" /></td>
</tr>
<tr>
<th valign="top">Username:</th>
<td class="field"><asp:TextBox runat="server" ID="F_Username" style="width:219px;" MaxLength="50" /></td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>

</asp:content>

