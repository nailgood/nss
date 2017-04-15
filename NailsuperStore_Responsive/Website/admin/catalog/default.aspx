<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_catalog_default" title="" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
    <h4>Catalog Administration</h4>

    <p><asp:button id="AddNew" Runat="server" Text="Add New Catalog" cssClass="btn"></asp:button>
    <p></p>
    <table cellSpacing="0" cellPadding="0" border="0" id="tblList" runat="server">
        <tr>
            <td><asp:datagrid id="dgList" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
                    CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
                    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="row"></ItemStyle>
                    <HeaderStyle CssClass="header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?CatalogId=" & DataBinder.Eval(Container.DataItem, "CatalogId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this catalog?" runat="server" NavigateUrl= '<%# "delete.aspx?CatalogId=" & DataBinder.Eval(Container.DataItem, "CatalogId") & "&" & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>                            
						<asp:BoundColumn DataField="Title" HeaderText="Title"></asp:BoundColumn>
                        <asp:TemplateColumn>
							<HeaderTemplate>
								Cover Image
							</HeaderTemplate>
                            <ItemTemplate>
								<asp:Image ImageUrl='<%# "/assets/catalog/small/" & DataBinder.Eval(Container.DataItem, "CatalogImage") %>' BorderWidth=0 ID="Image" Runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
						<asp:BoundColumn DataField="IsActive" HeaderText="Active?"></asp:BoundColumn>                                
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=UP&CatalogId=" & DataBinder.Eval(Container.DataItem, "CatalogId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/adminmoveup.gif" ID="Hyperlink2">Move Up</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?ACTION=DOWN&CatalogId=" & DataBinder.Eval(Container.DataItem, "CatalogId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/adminmovedown.gif" ID="Hyperlink3">Move Down</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:datagrid></td>
        </tr>
        <tr>
            <td><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
        </tr>
    </table>
    <asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match the search criteria</asp:placeholder>
</asp:Content>