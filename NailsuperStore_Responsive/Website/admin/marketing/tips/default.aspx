<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Tips" CodeFile="default.aspx.vb" Inherits="admin_store_tips_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Tips for Category <asp:Literal runat="server" ID="lit" /></h4>

<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server" runat="server" visible="false">
<span class="smaller">Please provide search criteria below</span>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Category:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_TipCategoryId" runat="server" /></td>
</tr>
<tr>
<th valign="top">Title:</th>
<td valign="top" class="field"><asp:textbox id="F_Title" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
</asp:Panel>

<a href="categories/">&laquo; Back to Tip Categories</a>

<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Tip" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use arrows" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Tip?" runat="server" NavigateUrl= '<%# "delete.aspx?TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink NavigateUrl='<%# "items.aspx?TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' runat="server" text="Items">Items</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink NavigateUrl='<%# "departments.aspx?TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' runat="server" text="Items">Departments</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="TipCategory" Visible="False" HeaderText="Category"></asp:BoundField>
		<asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
		<asp:BoundField DataField="Summary" HeaderText="Summary"></asp:BoundField>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
            <ItemTemplate>
                <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                    CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TipId")%>' />
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=UP&TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&TipCategoryId=" & DataBinder.Eval(Container.DataItem, "TipCategoryId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/moveup.gif" ID="lnkMoveUp">Move Up</asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=DOWN&TipId=" & DataBinder.Eval(Container.DataItem, "TipId") & "&TipCategoryId=" & DataBinder.Eval(Container.DataItem, "TipCategoryId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/movedown.gif" ID="lnkMoveDown">Move Down</asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateField>
	</Columns>
</CC:GridView>

</asp:content>

