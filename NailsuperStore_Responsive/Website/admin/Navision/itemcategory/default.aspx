<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Item Categories" CodeFile="default.aspx.vb" Inherits="admin_navision_itemCategory_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Item Categories</h4>

<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Navision Item Category" cssClass="btn" Visible="false"></CC:OneClickButton>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?id=" & DataBinder.Eval(Container.DataItem, "id") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="Category" DataField="Category" HeaderText="Category"></asp:BoundField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
		<asp:TemplateField HeaderText="Department(s)">
			<ItemTemplate>
				<asp:Repeater ID="Departments" Runat="server">
					<SeparatorTemplate><br /></SeparatorTemplate>
					<ItemTemplate>
						<li style="list-style-image:url(/includes/theme-admin/images/minifolder.gif)">
							<%#Container.DataItem("NAME")%>
					</ItemTemplate>
				</asp:Repeater>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</CC:GridView>

</asp:content>

