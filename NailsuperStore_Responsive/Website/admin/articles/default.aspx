<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Article" CodeFile="default.aspx.vb" Inherits="admin_articles_index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Article</h4>

<span class="smaller">Please provide search criteria below</span>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Title:</th>
<td valign="top" class="field"><asp:textbox id="F_Title" runat="server" Columns="50" MaxLength="255"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>IsActive:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsActive" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<th valign="top"><b>PostDate:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From<br /><CC:DatePicker ID="F_PostDateLbound" runat="server">
    </CC:DatePicker></td><td>&nbsp;</td><td  class="smaller">To<br /><CC:DatePicker id="F_PostDateUbound" runat="server" /></td>
</tr>
</table>

</td>
<td>
<CC:DateValidator ID="DateValidator1" runat="server" Display="Dynamic" ControlToValidate="F_PostDateLBound" ErrorMessage="Invalid 'From date'"></CC:DateValidator>
<CC:DateValidator ID="DateValidator2" runat="server" Display="Dynamic" ControlToValidate="F_PostDateUBound" ErrorMessage="Invalid 'To date'"></CC:DateValidator>
</td>

</tr>
<tr>
<td colspan="2" align="right" class="field">
<asp:button id="btnSearch" Runat="server" Text="Search" cssClass="btn"></asp:button>
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>

<p></p>
<asp:button id="AddNew" Runat="server" Text="Add New Article" cssClass="btn"></asp:button>
<p></p>

<CC:GridView ShowFooter="true" id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="25" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
	<RowStyle CssClass="row"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?ArticleId=" & DataBinder.Eval(Container.DataItem, "ArticleId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Article?" runat="server" NavigateUrl= '<%# "delete.aspx?ArticleId=" & DataBinder.Eval(Container.DataItem, "ArticleId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField  SortExpression="Title" DataField="Title" HeaderText="Title"></asp:BoundField>
		<asp:BoundField  SortExpression="PostDate" DataField="PostDate" HeaderText="PostDate" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="IsActive"/>
	</Columns>
</CC:GridView>
</asp:content>

