<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Email" CodeFile="default.aspx.vb" Inherits="admin_contactEmail_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Contact Email Administration</h4>

<p></p>
<CC:OneClickButton id="AddNew" CausesValidation="False" Runat="server" Text="Add New Contact Email" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CausesValidation="False"  CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?EmailId=" & DataBinder.Eval(Container.DataItem, "EmailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Contact Email?" runat="server" NavigateUrl= '<%# "delete.aspx?EmailId=" & DataBinder.Eval(Container.DataItem, "EmailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="Email" DataField="Email" HeaderText="Email Address"></asp:BoundField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
        <asp:TemplateField HeaderText="Subjects" ItemStyle-CssClass="subject">
			<ItemTemplate>
			    <ul>
			        <asp:Literal runat="server" ID="ltrSubjects"></asp:Literal>
			    </ul>
			</ItemTemplate>
		</asp:TemplateField>
    </Columns>
</CC:GridView>

</asp:content>

