<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Cusion Color" CodeFile="default.aspx.vb" Inherits="admin_store_cusioncolor_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Cusion Color</h4>

<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Cusion Color" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?CusionColorId=" & DataBinder.Eval(Container.DataItem, "CusionColorId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Cusion Color?" runat="server" NavigateUrl= '<%# "delete.aspx?CusionColorId=" & DataBinder.Eval(Container.DataItem, "CusionColorId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="CusionColor" DataField="CusionColor" HeaderText="Cusion Color"></asp:BoundField>
		<asp:ImageField HeaderText="Swatch" DataImageUrlField="Swatch" DataImageUrlFormatString="/assets/CusionColor/{0}"></asp:ImageField> 
	</Columns>
</CC:GridView>

</asp:content>

