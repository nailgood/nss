<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Store Order" CodeFile="default.aspx.vb" Inherits="admin_store_cartDetail_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Shopping Cart Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Order No:</th>
<td valign="top" class="field"><asp:textbox id="F_OrderNo" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Bill To Salon Name:</th>
<td valign="top" class="field"><asp:textbox id="F_BillToSalonName" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Bill To Name:</th>
<td valign="top" class="field"><asp:textbox id="F_BillToName" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Bill To Name 2:</th>
<td valign="top" class="field"><asp:textbox id="F_BillToName2" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Bill To State:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_BillToCounty" runat="server" /></td>
</tr>
<tr>
<th valign="top">Bill To Zipcode:</th>
<td valign="top" class="field"><asp:textbox id="F_BillToZipcode" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr runat="server" visible="false">
<th valign="top"><b>Bill To Province/Region:</b></th>
<td valign="top" class="field"><asp:textbox id="F_BillToRegion" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Bill To Country:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_BillToCountry" runat="server" /></td>
</tr>
<tr>
<th valign="top">Email:</th>
<td valign="top" class="field"><asp:textbox id="F_Email" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Order Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_OrderDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_OrderDateUbound" runat="server" /></td>
</tr>
</table>
</td>
</tr>			
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
</asp:Panel>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?OrderId=" & DataBinder.Eval(Container.DataItem, "OrderId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkEdit">Preview Shopping Cart Detail</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "SendEmail.aspx?OrderId=" & DataBinder.Eval(Container.DataItem, "OrderId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkSendEmail">Send Email</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="OrderNo" DataField="OrderNo" HeaderText="Order No"></asp:BoundField>
		<asp:BoundField DataField="BillToSalonName" HeaderText="Bill To Salon Name"></asp:BoundField>
		<asp:BoundField DataField="BillToName" HeaderText="Bill To Name"></asp:BoundField>
		<asp:BoundField DataField="BillToName2" HeaderText="Bill To Name 2"></asp:BoundField>
		<asp:BoundField DataField="BillToCounty" HeaderText="Bill To State"></asp:BoundField>
		<asp:BoundField DataField="BillToCountry" HeaderText="Bill To Country"></asp:BoundField>
		<asp:BoundField DataField="Remoteip" HeaderText="IP Adress"></asp:BoundField>
		<asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
		<asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="Order Date" HTMLEncode="False"></asp:BoundField>
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
<div runat="server" id="divDownload" visible="false">
<asp:HyperLink id="lnkDownload" runat="server">Download File</asp:HyperLink>
</div>


</asp:content>

