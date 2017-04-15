<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_LandingPage_Default" title="Untitled Page" %>

<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
<script>
    function ConfirmDelete() {
        return confirm('Are you sure to delete?');        
    }
</script>
<h4>Landing Page</h4>
<span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
		<table cellSpacing="2" cellPadding="3" border="0">
			<TBODY>
				<tr>
				    <th style="text-align: right;">Title</th>
					<td class="field" width="400">
                        <asp:TextBox ID="F_Title" runat="server" Columns="30"  MaxLength="200"></asp:TextBox>
					</td>
				</tr>	
                <tr>
                    <th style="text-align: right;">Starting Date:</th>
                    <td class="field">                   
                        <CC:DatePicker ID="F_StartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: right;">Ending Date:</th>
                    <td class="field">
                        <CC:DatePicker ID="F_EndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: right;">Is Active</th>
                    <td valign="top" class="field">
                        <asp:DropDownList ID="F_IsActive" runat="server">
                            <asp:ListItem Value="">-- ALL --</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>               
				<tr>
				    <td colspan="2" align="right" valign="top">
				    <p>
				    <CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn"  />
                    <input class="btn" type="button" value="Clear" onclick="window.location='default.aspx';return false;" />
				    </td>
				</tr>
			</TBODY>
		</table>       
	</asp:Panel>  
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New LandingPage" CssClass="btn"></CC:OneClickButton>
    <p></p>
		<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" 
            AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" 
            EmptyDataText="There are no records that match the search criteria" 
            AutoGenerateColumns="False" BorderWidth="0px" PagerSettings-Position="Bottom" 
            CausesValidation="True" PageSelectIndex="0" 
            SortImageAsc="/includes/theme-admin/images/asc3.gif" SortImageDesc="/includes/theme-admin/images/desc3.gif" 
            SortOrder="DESC" SortBy="Id" PageSize="20">
        <HeaderStyle VerticalAlign="Top"></HeaderStyle>
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />
            <Columns>             
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                            ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                            CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'
                            OnClientClick="return ConfirmDelete();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Title" SortExpression="Title">
                    <ItemTemplate>
                        <asp:Label ID="lbTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                        <br />
                        <a target=_blank href="<%# "/landing/" & DataBinder.Eval(Container.DataItem, "URLCode")  %>" > <%#"/landing/" & DataBinder.Eval(Container.DataItem, "URLCode")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="SKU" HeaderStyle-Width="50px" >
                    <ItemTemplate>
                        <asp:Label ID="lbSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SKU") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group" SortExpression="CustomerPriceGroupCode" HeaderStyle-Width="70px" >
                    <HeaderTemplate>
                        <a href="javascript:__doPostBack('ctl00$ph$gvList','Sort$CustomerPriceGroupCode')" title="Customer Price Group" >Group</a>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbCustomerPriceGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerPriceGroupCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Starting Date" SortExpression="StartingDate" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label ID="lbStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StartingDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ending Date" SortExpression="EndingDate" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label ID="lbEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EndingDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="lp.IsActive"  HeaderText="Active">
                    <ItemTemplate>
                        <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                            CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Position="Bottom" />
        </CC:GridView>   

</asp:Content>

