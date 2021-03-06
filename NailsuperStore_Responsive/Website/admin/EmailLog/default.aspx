<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Email" CodeFile="default.aspx.vb" Inherits="admin_emailLog_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Email Log Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">

<tr>
<th valign="top"><b>Send Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller"><CC:DatePicker id="F_StartDateLbound" runat="server" /></td><td>&nbsp;</td>
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
<CC:OneClickButton id="AddNew" CausesValidation="False" Runat="server" Text="Add New Email Template" cssClass="btn" Visible ="false" ></CC:OneClickButton>

<p></p>
<div><table><tr>
		<th valign="top">Contents: </th>
		<td class="field"><asp:textbox id="F_Contents" runat="server" maxlength="50" columns="50" style="width: 800px;" Height="600px" TextMode="MultiLine" ></asp:textbox></td>
		<td></td>
</tr></table></div>
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?EmailId=" & DataBinder.Eval(Container.DataItem, "EmailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Email Template?" runat="server" NavigateUrl= '<%# "delete.aspx?EmailId=" & DataBinder.Eval(Container.DataItem, "EmailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="SubjectId" DataField="SubjectId" HeaderText="Subject Id"></asp:BoundField>
		<asp:BoundField SortExpression="Subject" DataField="Subject" HeaderText="Subject Name"></asp:BoundField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
		<asp:BoundField SortExpression="StartDate" DataField="StartDate" HeaderText="Start Date"></asp:BoundField>
		<asp:BoundField SortExpression="EndDate" DataField="EndDate" HeaderText="End Date"></asp:BoundField>
	    <asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active"/>

		</Columns>
</CC:GridView>

</asp:content>

