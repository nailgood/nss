<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Job" CodeFile="default.aspx.vb" Inherits="admin_jobs_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Job</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Category:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_CategoryId" runat="server" /></td>
</tr>
<tr>
<th valign="top">Title:</th>
<td valign="top" class="field"><asp:textbox id="F_Title" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top">City:</th>
<td valign="top" class="field"><asp:textbox id="F_City" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>State:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_State" runat="server" /></td>
</tr>
<tr>
<th valign="top">Zip:</th>
<td valign="top" class="field"><asp:textbox id="F_Zip" runat="server" Columns="10" MaxLength="10"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Company:</th>
<td valign="top" class="field"><asp:textbox id="F_Company" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Is Active:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsActive" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
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
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Job" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?JobId=" & DataBinder.Eval(Container.DataItem, "JobId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Job?" runat="server" NavigateUrl= '<%# "delete.aspx?JobId=" & DataBinder.Eval(Container.DataItem, "JobId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
		<asp:BoundField SortExpression="JobCategory" DataField="JobCategory" HeaderText="Category"></asp:BoundField>
		<asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>
		<asp:BoundField DataField="State" SortExpression="State" HeaderText="State"></asp:BoundField>
		<asp:BoundField DataField="Company" HeaderText="Company"></asp:BoundField>
		<asp:BoundField DataField="ShortDescription" HeaderText="Short Description"></asp:BoundField>
		<asp:BoundField DataField="ExpirationDate" HeaderText="Expiration Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active"/>
	</Columns>
</CC:GridView>

</asp:content>

