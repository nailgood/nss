<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Us" CodeFile="default.aspx.vb" Inherits="admin_contact_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Contact Us Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Subject Id:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_SubjectId" runat="server" /></td>
</tr>
<tr>
<th valign="top">First Name:</th>
<td valign="top" class="field"><asp:textbox id="F_FirstName" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Last Name:</th>
<td valign="top" class="field"><asp:textbox id="F_LastName" runat="server" Columns="30" MaxLength="30"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Email Address:</th>
<td valign="top" class="field"><asp:textbox id="F_EmailAddress" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Phone:</th>
<td valign="top" class="field"><asp:textbox id="F_Phone" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Order Number:</th>
<td valign="top" class="field"><asp:textbox id="F_OrderNumber" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Contact Us" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Contact Us?" runat="server" NavigateUrl= '<%# "delete.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="SubjectId" DataField="SubjectId" HeaderText="Subject Id"></asp:BoundField>
		<asp:BoundField SortExpression="FirstName" DataField="FirstName" HeaderText="First Name"></asp:BoundField>
		<asp:BoundField SortExpression="LastName" DataField="LastName" HeaderText="Last Name"></asp:BoundField>
		<asp:BoundField SortExpression="EmailAddress" DataField="EmailAddress" HeaderText="Email Address"></asp:BoundField>
		<asp:BoundField SortExpression="Phone" DataField="Phone" HeaderText="Phone"></asp:BoundField>
		<asp:BoundField SortExpression="OrderNumber" DataField="OrderNumber" HeaderText="Order Number"></asp:BoundField>
		<asp:BoundField SortExpression="CreateDate" DataField="CreateDate" HeaderText="Create Date"></asp:BoundField>
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
</asp:content>

