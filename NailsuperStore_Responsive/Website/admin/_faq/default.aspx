<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="ASP20.NET" CodeFile="default.aspx.vb" Inherits="admin__faq_index"  %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>FAQ Administration</h4>

<p>
<asp:button id="AddNew" Runat="server" Text="Add New Question" cssClass="btn"></asp:button>
</p>

<table cellSpacing="0" cellPadding="0" border="0" id="tblList" runat="server">
<tr><td>

<asp:datagrid id="dgList" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
<ItemStyle CssClass="row"></ItemStyle>
<HeaderStyle CssClass="header"></HeaderStyle>
<Columns>
	<asp:TemplateColumn>
		<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?FaqId=" & DataBinder.Eval(Container.DataItem, "FaqId") & "&" & params %>' ImageUrl="/images/admin/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:TemplateColumn>
		<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this question?" runat="server" NavigateUrl= '<%# "delete.aspx?FaqId=" & DataBinder.Eval(Container.DataItem, "FaqId") & "&" & params %>' ImageUrl="/images/admin/delete.gif" ID="Confirmlink1">Delete</CC:ConfirmLink>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="Question" HeaderText="Question"></asp:BoundColumn>
	<asp:BoundColumn DataField="IsActive" HeaderText="Is Active?"></asp:BoundColumn>
	<asp:TemplateColumn>
		<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=UP&FaqId=" & DataBinder.Eval(Container.DataItem, "FaqId")  & "&" & params %>' ImageUrl="/images/admin/moveup.gif" ID="Hyperlink2">Move up</asp:HyperLink>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:TemplateColumn>
		<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=DOWN&FaqId=" & DataBinder.Eval(Container.DataItem, "FaqId")  & "&" & params %>' ImageUrl="/images/admin/movedown.gif" ID="Hyperlink3">Move down</asp:HyperLink>
		</ItemTemplate>
	</asp:TemplateColumn>
</Columns>
<PagerStyle Visible="False"></PagerStyle>
</asp:datagrid>

</td>
</tr>
<tr>
	<td><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
</tr>
</table>

<asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match the search criteria</asp:placeholder>

</asp:content>