<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="password.aspx.vb" Inherits="admin_password"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Password Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">

<tr>
		<td class="field">Username:</td>
		<td class="field"><asp:textbox id="F_UserName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="field">Email:</td>
		<td class="field"><asp:textbox id="F_Email" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr id ="pass" runat="server">
		<td class="field">Password:</td>
		<td class="field"><asp:Literal  id="LitPass" runat="server"></asp:Literal></td>
		<td></td>
	</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />

</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="Password" Runat="server" Text="Password" Visible="false" cssClass="btn"></CC:OneClickButton>

<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>	
	    <asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "password.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkEdit">Select</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>		
		<asp:BoundField SortExpression="UserName" DataField="UserName" HeaderText="User Name"></asp:BoundField>
		<asp:BoundField SortExpression="Email" DataField="Email" HeaderText="Email"></asp:BoundField>
	</Columns>
</CC:GridView>

</asp:content>

