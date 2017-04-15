<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Free Gift" CodeFile="default.aspx.vb" Inherits="admin_store_freegift_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Free Gift</h4>

<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Free Gift" cssClass="btn"></CC:OneClickButton>
<CC:OneClickButton id="btnEditMetaTag" Runat="server" Text="Edit Meta Tags" cssClass="btn"></CC:OneClickButton>
<p>
Filter by
<asp:DropDownList ID="drpLevel" runat="server" AutoPostBack="true">
</asp:DropDownList></p>
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" 
        PageSize="50" AllowPaging="True" AllowSorting="True" 
        HeaderText="Red highlight that means the item now is inactive in Product > Item" 
        EmptyDataText="There are no records that match the search criteria" 
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" 
        SortOrder="Asc">
<HeaderStyle VerticalAlign="Top"></HeaderStyle>

	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?FreeGiftId=" & DataBinder.Eval(Container.DataItem, "FreeGiftId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Free Gift?" runat="server" NavigateUrl= '<%# "delete.aspx?FreeGiftId=" & DataBinder.Eval(Container.DataItem, "FreeGiftId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:BoundField DataField="SKU" HeaderText="SKU"></asp:BoundField>
		<asp:BoundField DataField="Itemname" HeaderText="Item"></asp:BoundField>

		<%--<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" DataField="IsActive" HeaderText="Active"><ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:Checkboxfield>
        <asp:Checkboxfield ItemStyle-HorizontalAlign="Center" DataField="IsAddCart" HeaderText="Allow Add Cart"><ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:Checkboxfield>--%>
         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
            <ItemTemplate>
                <asp:ImageButton ID="imbIsActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="IsActive" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FreeGiftId")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Allow Add Cart">
            <ItemTemplate>
                <asp:ImageButton ID="imbIsAddCart" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="IsAddCart" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "valTemp")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
            <ItemTemplate>
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FreeGiftId")%>' />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FreeGiftId")%>' CommandName="Down" />
            </ItemTemplate>
        </asp:TemplateField>
	</Columns>
</CC:GridView>

</asp:content>

