<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="default.aspx.vb" Inherits="admin_AutoRespond_Default" %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<h4>Schedule Holiday</h4>

<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
   <tr>
        <th valign="top"><b>Starting Date:</b></th>
        <td valign="top" class="field">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr><td class="smaller">From <CC:DatePicker id="F_StartingDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_StartingDateUbound" runat="server" /></td>
        </tr>
        </table>
        </td>
    </tr>
    <tr>
        <th valign="top"><b>Ending Date:</b></th>
        <td valign="top" class="field">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr><td class="smaller">From <CC:DatePicker id="F_EndingDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_EndingDateUbound" runat="server" /></td>
        </tr>
        </table>
        </td>
    </tr>

<tr>
<td align="right">
<CC:OneClickButton id="btnAddNew" Runat="server" Text="Add New" cssClass="btn" />
</td>
<td>
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
</td>
</tr>
</table>
</asp:Panel>
<br />
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?DayId=" & DataBinder.Eval(Container.DataItem, "DayId") %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Mix Match Promotion?" runat="server" NavigateUrl= '<%# "delete.aspx?DayId=" & DataBinder.Eval(Container.DataItem, "DayId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="DayName" DataField="DayName" HeaderText="Day Name"  HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="StartingDate" DataField="StartingDate" HeaderText="Starting Date" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField DataField="EndingDate" HeaderText="Ending Date" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="CreateDate" DataField="CreateDate" HeaderText="Create Date" HTMLEncode="False"></asp:BoundField>
		
	</Columns>
	</CC:GridView>

</asp:content>