<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Us" CodeFile="default.aspx.vb" Inherits="admin_opinion_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Customer Opinion Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Comment Type:</b></th>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Contact Us" cssClass="btn" Visible="false" ></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Contact Us?" runat="server" NavigateUrl= '<%# "delete.aspx?ContactId=" & DataBinder.Eval(Container.DataItem, "ContactId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField  DataField="Comments" HeaderText="Comments"></asp:BoundField>
		<asp:BoundField SortExpression="FirstName" DataField="FirstName" HeaderText="First Name"></asp:BoundField>
		<asp:BoundField SortExpression="LastName" DataField="LastName" HeaderText="Last Name"></asp:BoundField>
		<asp:BoundField SortExpression="EmailAddress" DataField="EmailAddress" HeaderText="Email Address"></asp:BoundField>
		<asp:BoundField SortExpression="Phone" DataField="Phone" HeaderText="Phone"></asp:BoundField>
		<asp:BoundField SortExpression="OrderNumber" DataField="OrderNumber" HeaderText="Order Number"></asp:BoundField>
		<asp:BoundField SortExpression="CreateDate" DataField="CreateDate" HeaderText="Create Date"></asp:BoundField>
	    <asp:BoundField SortExpression="TypeShipping" DataField="TypeShipping" HeaderText="TypeShipping" ></asp:BoundField>
		<asp:BoundField SortExpression="ItemNotReceived" DataField="ItemNotReceived" HeaderText="ItemNotReceived" ></asp:BoundField>
        <asp:BoundField SortExpression="ItemNumber" DataField="ItemNumber" HeaderText="ItemNumber" ></asp:BoundField>
		 <asp:TemplateField HeaderText="ProductDescription" SortExpression="ProductDescription">
			<ItemTemplate>
				<asp:Label id="lbPro" runat="server"></asp:Label>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="DamagedOrDefective" DataField="DamagedOrDefective" HeaderText="DamagedOrDefective" ></asp:BoundField>
        <asp:BoundField SortExpression="ItemDamaged" DataField="ItemDamaged" HeaderText="ItemDamaged" ></asp:BoundField>
		<asp:BoundField SortExpression="PieceOfMerchandise" DataField="PieceOfMerchandise" HeaderText="PieceOfMerchandise" ></asp:BoundField>
        <asp:BoundField SortExpression="DamagedCarton" DataField="DamagedCarton" HeaderText="DamagedCarton" ></asp:BoundField>
        <asp:BoundField SortExpression="DscPackaging" DataField="DscPackaging" HeaderText="DscPackaging" ></asp:BoundField>
		<asp:BoundField SortExpression="DscMaterial" DataField="DscMaterial" HeaderText="DscMaterial" ></asp:BoundField>
       
	</Columns>
</CC:GridView>

</asp:content>

