<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_store_Album_default" MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>

    <h4>Album</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
   
            <tr>
                <th valign="top">
                    Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Name" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Is Active:</b>
                </th>
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
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <p></p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Abum" CssClass="btn"></CC:OneClickButton>
    <p></p>
    
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
           
            <asp:BoundField DataField="Name" HeaderStyle-Width="250" HeaderText="Album Name"></asp:BoundField>
           
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/images/admin/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "AlbumId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/images/admin/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
           <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/images/admin/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/images/admin/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
    
        </Columns>
    </CC:GridView>
</asp:Content>