<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Homepage" CodeFile="default.aspx.vb" Inherits="admin_homepage_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Homepage Images</h4>

<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Homepage Image" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?HomepageImageId=" & DataBinder.Eval(Container.DataItem, "HomepageImageId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Homepage?" runat="server" NavigateUrl= '<%# "delete.aspx?HomepageImageId=" & DataBinder.Eval(Container.DataItem, "HomepageImageId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="ImageName" SortExpression="ImageName" HeaderText="Name" />
		<asp:TemplateField HeaderText="IsActive" SortExpression="IsActive">
            <ItemTemplate><%#IIf(Boolean.Parse(Eval("IsActive").ToString()), "Yes", "")%></ItemTemplate>
        </asp:TemplateField>
	</Columns>
</CC:GridView>

</asp:content>

