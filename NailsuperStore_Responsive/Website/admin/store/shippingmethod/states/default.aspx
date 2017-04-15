<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="default.aspx.vb" Inherits="admin_store_states__default"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
    <h4>Store State Tax Rates Administration</h4>

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
                                <asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?StateId=" & DataBinder.Eval(Container.DataItem, "StateId")  & "&" & params %>' ImageUrl="/images/admin/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
						<asp:BoundColumn DataField="StateName" HeaderText="State Name"></asp:BoundColumn>
						<asp:BoundColumn DataField="StateCode" HeaderText="State Code"></asp:BoundColumn>
						<asp:BoundColumn DataField="TaxRate" HeaderText="Tax Rate"></asp:BoundColumn>								
						<asp:BoundColumn DataField="IncludeGiftWrap" HeaderText="Include Gift Wrap?"></asp:BoundColumn>								
						<asp:BoundColumn DataField="IncludeDelivery" HeaderText="Include Delivery?"></asp:BoundColumn>								
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:datagrid></td>
        </tr>
        <tr>
            <td><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
        </tr>
    </table>
    <asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match the search criteria</asp:placeholder>
</asp:content>