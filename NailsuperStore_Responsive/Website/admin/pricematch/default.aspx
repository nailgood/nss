<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Price Match" CodeFile="default.aspx.vb" Inherits="admin_pricematch_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Price Match</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Your Name:</th>
<td valign="top" class="field"><asp:textbox id="F_YourName" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Email Address:</th>
<td valign="top" class="field"><asp:textbox id="F_EmailAddress" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Create Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_CreateDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_CreateDateUbound" runat="server" /></td>
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
<CC:OneClickButton id="AddNew" Runat="server" Visible="false" Text="Add New Price Match" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?PriceMatchId=" & DataBinder.Eval(Container.DataItem, "PriceMatchId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Price Match?" runat="server" NavigateUrl= '<%# "delete.aspx?PriceMatchId=" & DataBinder.Eval(Container.DataItem, "PriceMatchId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="YourName" HeaderText="Your Name"></asp:BoundField>
		<asp:BoundField DataField="EmailAddress" HeaderText="Email Address"></asp:BoundField>
		<asp:BoundField DataField="CompetitorsCompanyName" HeaderText="Competitor's Company Name"></asp:BoundField>
		<asp:BoundField DataField="CompetitorsPhoneNumber" HeaderText="Competitor's Phone Number"></asp:BoundField>
		<asp:BoundField DataField="CompetitorsWebsite" HeaderText="Competitor's Website"></asp:BoundField>
		<asp:BoundField SortExpression="CreateDate" DataField="CreateDate" HeaderText="Create Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
</asp:content>

