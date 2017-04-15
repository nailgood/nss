<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_shopdesign_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>
<h4>Shop Design</h4>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Title:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_Title" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
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
            <th valign="top">
                <b>Category:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList runat="server" ID="F_CatId" AutoPostBack="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
            </td>
        </tr>
    </table>
    <p>
        <CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn"></CC:OneClickButton></p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:BoundField DataField="Title" HeaderStyle-Width="250" HeaderText="Title"></asp:BoundField>
            <%--<asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="IsActive" HeaderText="Is Active" />--%>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Item">
                <ItemTemplate>
                    <asp:Literal ID="ltItem" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Image">
                <ItemTemplate>
                    <asp:Literal ID="ltImage" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Video">
                <ItemTemplate>
                    <asp:Literal ID="ltVideo" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ShopDesignId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <a href="edit.aspx?ShopDesignId=<%#DataBinder.Eval(Container.DataItem, "ShopDesignId") & "&" & GetPageParams(Components.FilterFieldType.All)%>">
                        <img src="/includes/theme-admin/images/edit.gif" style="border: none;" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ShopDesignId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ShopDesignId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ShopDesignId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
        
</asp:Content>

